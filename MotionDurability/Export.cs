using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Motion.Durability
{
    public partial class Export : Form
    {
        OpenFileDialog m_open_dfr = null;
        OpenFileDialog m_open_map = null;

        SaveFileDialog m_save_map = null;
        SaveFileDialog m_save_csv = null;
        SaveFileDialog m_save_rpc = null;
        SaveFileDialog m_save_static = null;

        //DurabilityData m_durability = null;
        Functions m_functions = null;
        XmlDocument m_dom_Config = null;
        XmlDocument m_dom_UserItems = null;

        //FileFormat m_fileFormat;
        ResultValueType m_resultType;

        public Export()
        {
            InitializeComponent();
            Initialize();
        }

        #region Member Event

        #region Click Event
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            m_open_dfr.Title = "Select the ANSYSMotion result file";
            m_open_dfr.Multiselect = true;
            m_open_dfr.RestoreDirectory = true;
            m_open_dfr.Filter = "DRF file(*.dfr)|*.dfr";

            if(DialogResult.OK == m_open_dfr.ShowDialog())
            {
                Add_Result_List(m_open_dfr.FileNames);
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView_result_list.SelectedItems)
            {
                listView_result_list.Items.Remove(item);
            }

            if(listView_result_list.Items.Count == 0)
            {
                listView_type.Items.Clear();
                listView_Entity.Items.Clear();
            }
        }

        private void btn_Map_Add_Click(object sender, EventArgs e)
        {
            m_open_map.Title = "Select the map file for ANSYSMotion durability analysis";
            m_open_map.Multiselect = true;
            m_open_map.RestoreDirectory = true;
            m_open_map.Filter = "Map file(*.xml)|*.xml";

            if(DialogResult.OK == m_open_map.ShowDialog())
            {
                Add_Map_List(m_open_map.FileNames);
            }
        }

        private void btn_Map_Remove_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView_Map.SelectedItems)
            {
                listView_Map.Items.Remove(item);
            }
        }

        private void btn_Export_Map_Click(object sender, EventArgs e)
        {
            if (false == Validation())
                return;

            m_dom_UserItems = m_functions.CreateDurabilityXML();
            if (false == CreateNode_UserDefinedItems(m_dom_UserItems, combo_Type.SelectedIndex))
                return;

            m_save_map = new SaveFileDialog();
            m_save_map.Title = "Save the map file for ANSYSMotion durability analysis";
            m_save_map.Filter = "ANSYSMotion Durability Analysis File (*.xml)|*.xml";

            if(DialogResult.OK == m_save_map.ShowDialog())
            {
                m_functions.WriteMap(m_save_map.FileName, m_dom_UserItems);
            }

        }

        private void btn_Write_RPC_Click(object sender, EventArgs e)
        {
            if (false == Validation())
                return;

            m_dom_UserItems = m_functions.CreateDurabilityXML();
            if (false == CreateNode_UserDefinedItems(m_dom_UserItems, combo_Type.SelectedIndex))
                return;

            m_save_rpc = new SaveFileDialog();
            m_save_rpc.Title = "Save the RPC III file for ANSYSMotion durability analysis";
            m_save_rpc.Filter = "RPC III (*.rsp)|*.rsp";

            if (DialogResult.OK == m_save_rpc.ShowDialog())
            {
                string _dir = Path.GetDirectoryName(m_save_rpc.FileName);
                string _userNamed = Path.GetFileNameWithoutExtension(m_save_rpc.FileName);

                if (0 == tab_main.SelectedIndex)
                {

                    foreach (ListViewItem item in listView_result_list.Items)
                    {
                        string _output = item.Text + "_" + _userNamed + ".rsp";
                        string _path = Path.Combine(_dir, _output);

                        string _result = item.Tag as string;

                        DurabilityData durability = m_functions.BuildDataFromSelection(m_dom_UserItems, _result);
                        if (durability == null)
                            continue;

                        if (durability.ExistChassis == false)
                        {
                            string str_error = string.Format(" “{0}” cannot export time history data", _output);
                            MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        if (false == m_functions.WriteResultToFile(FileFormat.RPC, m_resultType, _path, durability))
                        {
                            //MessageBox.Show("Failed to export results to file.");
                            continue;
                        }

                    }
                }
                else
                {
                    foreach (ListViewItem item in listView_result_list.Items)
                    {
                        foreach (ListViewItem item_map in listView_Map.Items)
                        {
                            string _output = _userNamed + "_" + item.Text + "_" + item_map.Text + ".rsp";
                            string _path = Path.Combine(_dir, _output);

                            string _result = item.Tag as string;
                            string _map = item_map.Tag as string;

                            DurabilityData durability = m_functions.BuildDataFromMap(_result, _map);
                            if (durability == null)
                                continue;

                            if (durability.ExistChassis == false)
                            {
                                string str_error = string.Format(" “{0}” cannot export time history data", _output);
                                MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }

                            if (false == m_functions.WriteResultToFile(FileFormat.RPC, m_resultType, _path, durability))
                            {
                                //MessageBox.Show("Failed to export results to file.");
                                continue;
                            }

                        }
                    }
                }


            }

        }

        private void btn_Write_CSV_Click(object sender, EventArgs e)
        {
            if (false == Validation())
                return;

            m_dom_UserItems = m_functions.CreateDurabilityXML();
            if (false == CreateNode_UserDefinedItems(m_dom_UserItems, combo_Type.SelectedIndex))
                return;

            m_save_csv = new SaveFileDialog();
            if(2 == combo_Type.SelectedIndex)
            {
                m_save_csv.Title = "Save the MCF for ANSYSMotion durability analysis";
                m_save_csv.Filter = "Modal Coordinates File (*.mcf)|*.mcf";

                if (DialogResult.OK == m_save_csv.ShowDialog())
                {
                    string _dir = Path.GetDirectoryName(m_save_csv.FileName);
                    string _userNamed = Path.GetFileNameWithoutExtension(m_save_csv.FileName);

                    if (0 == tab_main.SelectedIndex)
                    {
                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            string _output = item.Text + "_" + _userNamed + ".mcf";
                            string _path = Path.Combine(_dir, _output);

                            string _result = item.Tag as string;

                            DurabilityData durability = m_functions.BuildDataFromSelection(m_dom_UserItems, _result);
                            if (durability == null)
                                continue;

                            if (false == m_functions.WriteResultToFile(FileFormat.MCF, m_resultType, _path, durability))
                            {
                                //MessageBox.Show("Failed to export results to file.");
                                continue;
                            }

                        }
                    }
                    else
                    {
                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            foreach (ListViewItem item_map in listView_Map.Items)
                            {
                                string _output = _userNamed + "_" + item.Text + "_" + item_map.Text + ".mcf";
                                string _path = Path.Combine(_dir, _output);

                                string _result = item.Tag as string;
                                string _map = item_map.Tag as string;

                                DurabilityData durability = m_functions.BuildDataFromMap(_result, _map);
                                if (durability == null)
                                    continue;


                                if (false == m_functions.WriteResultToFile(FileFormat.MCF, m_resultType, _path, durability))
                                {
                                    //MessageBox.Show("Failed to export results to file.");
                                    continue;
                                }

                            }
                        }

                    }

                }
            }
            else
            {
                m_save_csv.Title = "Save the CSV file for ANSYSMotion durability analysis";
                m_save_csv.Filter = "CSV (*.csv)|*.csv";

                if (DialogResult.OK == m_save_csv.ShowDialog())
                {
                    string _dir = Path.GetDirectoryName(m_save_csv.FileName);
                    string _userNamed = Path.GetFileNameWithoutExtension(m_save_csv.FileName);

                    if (0 == tab_main.SelectedIndex)
                    {
                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            string _output = item.Text + "_" + _userNamed + ".csv";
                            string _path = Path.Combine(_dir, _output);

                            string _result = item.Tag as string;

                            DurabilityData durability = m_functions.BuildDataFromSelection(m_dom_UserItems, _result);
                            if (durability == null)
                                continue;

                            if (durability.ExistChassis == false)
                            {
                                string str_error = string.Format(" “{0}” cannot export time history data", _output);
                                MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }

                            if (false == m_functions.WriteResultToFile(FileFormat.CSV, m_resultType, _path, durability))
                            {
                                //MessageBox.Show("Failed to export results to file.");
                                continue;
                            }

                        }
                    }
                    else
                    {
                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            foreach (ListViewItem item_map in listView_Map.Items)
                            {
                                string _output = _userNamed + "_" + item.Text + "_" + item_map.Text + ".csv";
                                string _path = Path.Combine(_dir, _output);

                                string _result = item.Tag as string;
                                string _map = item_map.Tag as string;

                                DurabilityData durability = m_functions.BuildDataFromMap(_result, _map);
                                if (durability == null)
                                    continue;

                                if (durability.ExistChassis == false)
                                {
                                    string str_error = string.Format(" “{0}” cannot export time history data", _output);
                                    MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;
                                }

                                if (false == m_functions.WriteResultToFile(FileFormat.CSV, m_resultType, _path, durability))
                                {
                                    //MessageBox.Show("Failed to export results to file.");
                                    continue;
                                }

                            }
                        }
                    }

                }
            }
        }

        private void btn_WriteStaticResults_Click(object sender, EventArgs e)
        {
            if (false == Validation())
                return;

            m_dom_UserItems = m_functions.CreateDurabilityXML();
            m_save_static = new SaveFileDialog();
        }

        #endregion

        #region Change Event

        private void tab_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 == (sender as TabControl).SelectedIndex)
            {
                btn_Export_Map.Visible = true;
            }
            else if (1 == (sender as TabControl).SelectedIndex)
            {
                btn_Export_Map.Visible = false;
            }
        }

        private void combo_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = false;
                listView_RF.Enabled = true;
                btn_Write_CSV.Text = "Write CSV";
                
            }
            else if (1 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = true;
                listView_RF.Enabled = false;
                btn_Write_CSV.Text = "Write CSV";
            }
            else if (2 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = true;
                listView_RF.Enabled = false;
                btn_Write_CSV.Text = "Write MCF";
            }

            if (listView_result_list.Items.Count > 0)
                ChangeDisplay_ListViewType_From_Combo_Type(combo_Type.SelectedIndex);
            else
                listView_type.Items.Clear();

            if (listView_type.Items.Count > 0)
                ChangeDisplay_listview_Entity_From_ListViewType(listView_type.Items[0]);
            else
                listView_Entity.Items.Clear();
        }

        private void listView_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ChangeDisplay_CBList_From_ListViewType(listView_type.SelectedItems[0]);
        }

        private void listView_type_Click(object sender, EventArgs e)
        {
            ChangeDisplay_listview_Entity_From_ListViewType(listView_type.SelectedItems[0]);
        }

        #endregion

        #endregion

        #region Member Function

        void Initialize()
        {
            m_open_dfr = new OpenFileDialog();
            m_open_map = new OpenFileDialog();

            listView_result_list.Items.Clear();
            listView_type.Items.Clear();
            listView_Map.Items.Clear();

            listView_Entity.Items.Clear();

            combo_Type.SelectedIndex = 0;
            combo_Force.SelectedIndex = 0;
            combo_length.SelectedIndex = 0;
            combo_Angle.SelectedIndex = 0;
            combo_Time.SelectedIndex = 0;

            m_functions = new Functions();

            listView_RF.Items.Add("VehicleBody");
            listView_RF.Items.Add("Global");

            //m_dom_UserItems = m_functions.CreateDurabilityXML();

            m_resultType = ResultValueType.FixedStep;

        }

        bool Validation()
        {
            if(0 == listView_result_list.Items.Count)
            {
                MessageBox.Show("There is no result of ANSYSMotion. After adding the result, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(0 == tab_main.SelectedIndex)
            {
                if(0 == combo_Type.SelectedIndex)
                {
                    if(0 == listView_type.Items.Count)
                    {
                        MessageBox.Show("There is no body data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_type.SelectedItems.Count)
                    {
                        MessageBox.Show("There is no selected body. After selecting the body, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_Entity.Items.Count)
                    {
                        MessageBox.Show("There is no entity data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_Entity.SelectedItems.Count)
                    {
                        MessageBox.Show("There is no selected entity. After selecting the entity, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_RF.SelectedItems.Count)
                    {
                        MessageBox.Show("There is no selected reference frame. After selecting the reference frame, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (1 == combo_Type.SelectedIndex)
                {
                    if (0 == listView_type.Items.Count)
                    {
                        MessageBox.Show("There is no force data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_type.SelectedItems.Count)
                    {
                        MessageBox.Show("There is no selected force. After selecting the force, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_Entity.Items.Count)
                    {
                        MessageBox.Show("There is no entity data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_Entity.SelectedItems.Count)
                    {
                        MessageBox.Show("There is no selected entity. After selecting the entity, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    if (0 == listView_type.Items.Count)
                    {
                        MessageBox.Show("There is no FE body data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == listView_type.SelectedItems.Count)
                    {
                        MessageBox.Show("There is no selected FE body. After selecting the FE body, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if("" == tb_stepsize.Text)
                {
                    MessageBox.Show("There is no value for the step size. After defining thw step size, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                try
                {
                    double dstepsize = Convert.ToDouble(tb_stepsize.Text);

                    if(dstepsize <= 0.0)
                    {
                        MessageBox.Show(" The value of step size must be greater than or equal to zero. Please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show(" The value of step size must be real value. Please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                if (0 == listView_Map.Items.Count)
                {
                    MessageBox.Show("There is no map file. After adding the map file, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //if(0 == listView_Map.SelectedItems.Count)
                //{
                //    MessageBox.Show("There is no selected map file. After selecting the map file, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return false;
                //}
            }

            return true;
        }


        void Add_Result_List(string[] ar_path)
        {
            int i;

            if (0 == listView_result_list.Items.Count)
            {
                m_dom_Config = m_functions.CreateXMLFromPost(ar_path[0]);


                for (i = 0; i < ar_path.Length; i++)
                {
                    ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(ar_path[i]));
                    item.Tag = ar_path[i];

                    listView_result_list.Items.Add(item);
                }

                ChangeDisplay_ListViewType_From_Combo_Type(combo_Type.SelectedIndex);
            }
            else
            {
                string str_old_path = "";
                bool is_Exist = false;

                for(i = 0; i < ar_path.Length; i++)
                {
                    is_Exist = false;
                    foreach(ListViewItem lvi in listView_result_list.Items)
                    {
                        str_old_path = lvi.Tag as string;

                        if (true == ar_path[i].Contains(str_old_path))
                        {
                            is_Exist = true;
                            break;

                        }
                    }

                    if (false == is_Exist)
                    {
                        ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(ar_path[i]));
                        item.Tag = ar_path[i];

                        listView_result_list.Items.Add(item);
                    }

                }
            }
        }

        void Add_Map_List(string[] ar_path)
        {
            int i;

            if (0 == listView_Map.Items.Count)
            {

                for (i = 0; i < ar_path.Length; i++)
                {
                    ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(ar_path[i]));
                    item.Tag = ar_path[i];

                    listView_Map.Items.Add(item);
                }
            }
            else
            {
                string str_old_path = "";
                bool is_Exist = false;

                for (i = 0; i < ar_path.Length; i++)
                {
                    is_Exist = false;
                    foreach (ListViewItem lvi in listView_Map.Items)
                    {
                        str_old_path = lvi.Tag as string;

                        if (true == ar_path[i].Contains(str_old_path))
                        {
                            is_Exist = true;
                            break;

                        }
                    }

                    if (false == is_Exist)
                    {
                        ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(ar_path[i]));
                        item.Tag = ar_path[i];

                        listView_Map.Items.Add(item);
                    }

                }
            }
        }

        bool CreateNode_UserDefinedItems(XmlDocument _dom, int Type_selectedIndex)
        {
            XmlNode node_UserItems = _dom.DocumentElement.SelectSingleNode("UserDefinedItems");
            XmlNode node_Item = null;

            string[] ar_Unit = new string[4];
            for (int i = 0; i < 4; i++)
                ar_Unit[i] = "";

            if(0 == Type_selectedIndex)
            {
                // Bodies
                node_Item = m_functions.CreateNodeAndAttribute(_dom, "Item", "name", "Bodies");

                string str_bd_name = listView_type.SelectedItems[0].Text;

                XmlNode node_body = m_functions.CreateNodeAndAttribute(_dom, "Body", "name", str_bd_name);

                string str_entity_name = "";
                foreach(ListViewItem item in listView_Entity.SelectedItems)
                {
                    str_entity_name = item.Text;
                    XmlNode node_config_entity = item.Tag as XmlNode;
                    string str_type = node_config_entity.Attributes.GetNamedItem("type").Value;

                    if(str_type == "motion")
                    {
                        foreach(ListViewItem item_RF in listView_RF.SelectedItems)
                        {
                            XmlNode node_Entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                            m_functions.CreateAttributeXML(_dom, ref node_Entity, "type", str_type);

                            if (item.Checked == true)
                            {
                                m_functions.CreateAttributeXML(_dom, ref node_Entity, "rotation_flag", "true");
                            }
                            else
                            {
                                m_functions.CreateAttributeXML(_dom, ref node_Entity, "rotation_flag", "false");
                            }

                            m_functions.CreateAttributeXML(_dom, ref node_Entity, "reference_frame", item_RF.Text);

                            node_body.AppendChild(node_Entity);
                        }
                    }
                    else
                    {
                        XmlNode node_Entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                        m_functions.CreateAttributeXML(_dom, ref node_Entity, "type", str_type);

                        if (item.Checked == true)
                        {
                            m_functions.CreateAttributeXML(_dom, ref node_Entity, "rotation_flag", "true");
                        }
                        else
                        {
                            m_functions.CreateAttributeXML(_dom, ref node_Entity, "rotation_flag", "false");
                        }
                        node_body.AppendChild(node_Entity);

                    }
                }

                Get_Unit_String(ar_Unit);

                XmlNode node_Unit = m_functions.CreateNodeAndAttribute(_dom, "Unit", "force", ar_Unit[0]);
                m_functions.CreateAttributeXML(_dom, ref node_Unit, "length", ar_Unit[1]);
                m_functions.CreateAttributeXML(_dom, ref node_Unit, "angle", ar_Unit[2]);
                m_functions.CreateAttributeXML(_dom, ref node_Unit, "time", ar_Unit[3]);

                node_Item.AppendChild(node_body);
                node_UserItems.AppendChild(node_Unit);

            }
            else if( 1 == Type_selectedIndex)
            {
                // Forces
                node_Item = m_functions.CreateNodeAndAttribute(_dom, "Item", "name", "Forces");

                string str_force_name = "";
                string str_entity_name = "";
                foreach (ListViewItem item in listView_Entity.SelectedItems)
                {
                    str_force_name = item.Text;
                    XmlNode node_Force = m_functions.CreateNodeAndAttribute(_dom, "Force", "name", str_force_name);

                    foreach(ListViewItem item_force in listView_Entity.SelectedItems)
                    {
                        str_entity_name = item_force.Text;
                        XmlNode node_entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                        node_Force.AppendChild(node_entity);
                    }

                    node_Item.AppendChild(node_Force);
                }

                Get_Unit_String(ar_Unit);

                XmlNode node_Unit = m_functions.CreateNodeAndAttribute(_dom, "Unit", "force", ar_Unit[0]);
                m_functions.CreateAttributeXML(_dom, ref node_Unit, "length", ar_Unit[1]);
                m_functions.CreateAttributeXML(_dom, ref node_Unit, "angle", ar_Unit[2]);
                m_functions.CreateAttributeXML(_dom, ref node_Unit, "time", ar_Unit[3]);

                node_UserItems.AppendChild(node_Unit);

            }
            else if(2 == Type_selectedIndex)
            {
                // FE Modal bodies
                node_Item = m_functions.CreateNodeAndAttribute(_dom, "Item", "name", "FlexibleBodies");
                string str_FB_name = "";
                foreach (ListViewItem item in listView_Entity.SelectedItems)
                {
                    str_FB_name = item.Text;

                    XmlNode node_FBody = m_functions.CreateNodeAndAttribute(_dom, "Body", "name", str_FB_name);
                    node_Item.AppendChild(node_FBody);
                }

                
            }

            //double dStepsize = Convert.ToDouble(tb_stepsize.Text);

            XmlNode node_StepSize = m_functions.CreateNodeAndAttribute(_dom, "Stepsize", "value", tb_stepsize.Text);
            
            node_UserItems.AppendChild(node_StepSize);

            node_UserItems.AppendChild(node_Item);

            return true;
        }

      

        bool ChangeDisplay_ListViewType_From_Combo_Type(int selectedIndex)
        {
            listView_type.Items.Clear();
            if (m_dom_Config == null)
                return true;

            XmlNodeList lst_type = m_dom_Config.DocumentElement.SelectNodes("Configuration/Result/Types/Type");

            if (0 == selectedIndex)
            {
                // Bodies
                
                XmlNode node_bodies = null;

                foreach(XmlNode n in lst_type)
                {
                    if("Bodies" == n.Attributes.GetNamedItem("name").Value)
                    {
                        node_bodies = n;
                        break;
                    }
                }

                if(node_bodies == null)
                {
//                    MessageBox.Show();
                    return false;
                }

                XmlNodeList lst_body = node_bodies.SelectNodes("Body");

                string str_bd_name = "";
                foreach(XmlNode n in lst_body)
                {
                    str_bd_name = n.Attributes.GetNamedItem("name").Value;
                    ListViewItem item = new ListViewItem(str_bd_name);
                    item.Tag = n;
                    listView_type.Items.Add(item);
                }

            }
            else if( 1 == selectedIndex)
            {
                // Forces

                XmlNode node_forces = null;

                foreach (XmlNode n in lst_type)
                {
                    if ("Forces" == n.Attributes.GetNamedItem("name").Value)
                    {
                        node_forces = n;
                        break;
                    }
                }

                if (node_forces == null)
                {
                    //                    MessageBox.Show();
                    return false;
                }

                XmlNodeList lst_force = node_forces.SelectNodes("Force");

                string str_force_name = "";
                foreach (XmlNode n in lst_force)
                {
                    str_force_name = n.Attributes.GetNamedItem("name").Value;
                    ListViewItem item = new ListViewItem(str_force_name);
                    item.Tag = n;
                    listView_type.Items.Add(item);
                }

            }
            else if(2 == selectedIndex)
            {
                // FE Bodies

                XmlNode node_FEbodies = null;

                foreach (XmlNode n in lst_type)
                {
                    if ("Flexible Bodies" == n.Attributes.GetNamedItem("name").Value)
                    {
                        node_FEbodies = n;
                        break;
                    }
                }

                if (node_FEbodies == null)
                {
                    //                    MessageBox.Show();
                    return false;
                }

                XmlNodeList lst_Fbody = node_FEbodies.SelectNodes("FBody");

                string str_fbody_name = "";
                foreach (XmlNode n in lst_Fbody)
                {
                    str_fbody_name = n.Attributes.GetNamedItem("name").Value;
                    ListViewItem item = new ListViewItem(str_fbody_name);
                    item.Tag = n;
                    listView_type.Items.Add(item);
                }
            }

            return true;
        }

        bool ChangeDisplay_listview_Entity_From_ListViewType(ListViewItem _item)
        {
            listView_Entity.Items.Clear();

            XmlNode node_data = _item.Tag as XmlNode;

            if ("Body" == node_data.Name)
            {
                // Bodies

                XmlNodeList lst_entity = node_data.SelectNodes("Entity");

                string str_E_name = "";
                foreach(XmlNode n in lst_entity)
                {
                    str_E_name = n.Attributes.GetNamedItem("name").Value;
                    ListViewItem item = new ListViewItem(str_E_name);
                    item.Tag = n;

                    listView_Entity.Items.Add(item);

                }

            }
            else if ("Force" == node_data.Name)
            {
                // Forces

                XmlNodeList lst_entity = node_data.SelectNodes("Entity");

                string str_E_name = "";
                foreach (XmlNode n in lst_entity)
                {
                    str_E_name = n.Attributes.GetNamedItem("name").Value;

                    ListViewItem item = new ListViewItem(str_E_name);
                    item.Tag = n;

                    listView_Entity.Items.Add(item);

                }

            }
            //else if ("FBody" == node_data.Name)
            //{
            //    // FE Bodies
            //}

            return true;
        }

        bool Export_Data(string str_FileType, XmlDocument dom)
        {
            if("Map" == str_FileType)
            {

            }
            else if ("CSV" == str_FileType)
            {

            }
            else if ("RPC" == str_FileType)
            {

            }
            else if ("Static" == str_FileType)
            {

            }



            return true;
        }

        void Get_Unit_String(string[] ar_Unit)
        {
            // ar_Unit[0] : Force
            // ar_Unit[1] : Disp
            // ar_Unit[2] : Angle
            // ar_Unit[3] : Time

            if(0 == combo_Force.SelectedIndex)
                ar_Unit[0] = "N";
            else if (1 == combo_Force.SelectedIndex)
                ar_Unit[0] = "kgf";
            else 
                ar_Unit[0] = "lbf";

            if (0 == combo_length.SelectedIndex)
                ar_Unit[1] = "mm";
            else if (1 == combo_length.SelectedIndex)
                ar_Unit[1] = "m";
            else
                ar_Unit[1] = "inch";

            if (0 == combo_Angle.SelectedIndex)
                ar_Unit[2] = "deg";
            else 
                ar_Unit[2] = "rad";

            ar_Unit[3] = "sec";
            

        }


        #endregion


    }
}
