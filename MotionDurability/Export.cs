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
using VM.Models;

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
        AnalysisModelType m_analysisScenario;

        ListViewItem m_item_type = null;

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
                Add_Result_List(m_open_dfr.FileNames, false);
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            ListViewItem Item_First = listView_result_list.Items[0];
            bool _bUpdate = false;

            foreach(ListViewItem item in listView_result_list.SelectedItems)
            {
                if (Item_First == item)
                    _bUpdate = true;

                listView_result_list.Items.Remove(item);
            }

            if(listView_result_list.Items.Count == 0)
            {
                Initialize();
            }
            else if(listView_result_list.Items.Count == 1)
            {
                ListViewItem item = listView_result_list.Items[0];
                string[] ar_path = new string[1];
                ar_path[0] = item.Tag as string;

                Add_Result_List(ar_path, _bUpdate);

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

            Define_Progress(0, 1);
            if (DialogResult.OK == m_save_map.ShowDialog())
            {
                 if (260 <= m_save_map.FileName.Length)
                {
                    MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File name : " + m_save_map.FileName + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_functions.WriteMap(m_save_map.FileName, m_dom_UserItems);
                pBar1.PerformStep();
            }

        }

        private void btn_Write_RPC_Click(object sender, EventArgs e)
        {
            if (false == Validation())
                return;

            if (0 == tab_main.SelectedIndex)
            {
                m_dom_UserItems = m_functions.CreateDurabilityXML();
                if (false == CreateNode_UserDefinedItems(m_dom_UserItems, combo_Type.SelectedIndex))
                    return;
            }

            m_save_rpc = new SaveFileDialog();
            m_save_rpc.Title = "Save the RPC III file for ANSYSMotion durability analysis";
            m_save_rpc.Filter = "RPC III (*.rsp)|*.rsp";

            if (DialogResult.OK == m_save_rpc.ShowDialog())
            {
                string _dir = Path.GetDirectoryName(m_save_rpc.FileName);
                string _userNamed = Path.GetFileNameWithoutExtension(m_save_rpc.FileName);

                if (248 <= _dir.Length)
                {
                    MessageBox.Show("Error : The length of the directory is too long. Its length must be less than 248.\n" + "Directory : " + _dir + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (260 <= _userNamed.Length)
                {
                    MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File name : " + _userNamed + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string errMessage = "";

                if (0 == tab_main.SelectedIndex)
                {
                    Define_Progress(0, listView_result_list.Items.Count);
                    foreach (ListViewItem item in listView_result_list.Items)
                    {
                        pBar1.PerformStep();

                        string _output = item.Text + "_" + _userNamed + ".rsp";
                        string _path = Path.Combine(_dir, _output);

                        if (260 <= (_path.Length - 4)) 
                        {
                            MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File path : " + _path + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string _result = item.Tag as string;

                        DurabilityData durability = m_functions.BuildDataFromSelection(m_dom_UserItems, _result, AnalysisModelType.Dynamics, ref errMessage);
                        if (durability == null)
                            continue;

                        if (durability.ExistChassis == false)
                        {
                            string str_error = string.Format(" “{0}” cannot export time history data", _output);
                            MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        if (false == m_functions.WriteResultToFile(FileFormat.RPC, m_resultType, _path, durability, ref errMessage))
                        {
                            //MessageBox.Show("Failed to export results to file.");
                            continue;
                        }

                    }
                }
                else
                {
                    int nCount = listView_result_list.Items.Count * listView_Map.Items.Count;
                    Define_Progress(0, nCount - 1);
                    foreach (ListViewItem item in listView_result_list.Items)
                    {
                        foreach (ListViewItem item_map in listView_Map.Items)
                        {
                            pBar1.PerformStep(); 

                            string _output = _userNamed + "_" + item.Text + "_" + item_map.Text + ".rsp";
                            string _path = Path.Combine(_dir, _output);

                            string _result = item.Tag as string;
                            string _map = item_map.Tag as string;

                            DurabilityData durability = m_functions.BuildDataFromMap(_result, _map, AnalysisModelType.Dynamics, ref errMessage);
                            if (durability == null)
                                continue;

                            if (durability.ExistChassis == false)
                            {
                                string str_error = string.Format(" “{0}” cannot export time history data", _output);
                                MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }

                            if (false == m_functions.WriteResultToFile(FileFormat.RPC, m_resultType, _path, durability, ref errMessage))
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

            if (0 == tab_main.SelectedIndex)
            {
                m_dom_UserItems = m_functions.CreateDurabilityXML();
                if (false == CreateNode_UserDefinedItems(m_dom_UserItems, combo_Type.SelectedIndex))
                    return;
            }

            m_save_csv = new SaveFileDialog();
            if(2 == combo_Type.SelectedIndex)
            {
                m_save_csv.Title = "Save the MCF for ANSYSMotion durability analysis";
                m_save_csv.Filter = "Modal Coordinates File (*.mcf)|*.mcf";

                if (DialogResult.OK == m_save_csv.ShowDialog())
                {
                    string _dir = Path.GetDirectoryName(m_save_csv.FileName);
                    string _userNamed = Path.GetFileNameWithoutExtension(m_save_csv.FileName);
                    string errMessage = "";

                    if (248 <= _dir.Length)
                    {
                        MessageBox.Show("Error : The length of the directory is too long. Its length must be less than 248.\n" + "Directory : " + _dir + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (260 <= _userNamed.Length)
                    {
                        MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File name : " + _userNamed + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (0 == tab_main.SelectedIndex)
                    {
                        int nCount = listView_result_list.Items.Count ;
                        Define_Progress(0, nCount);

                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            pBar1.PerformStep();

                            string _output = item.Text + "_" + _userNamed + ".mcf";
                            string _path = Path.Combine(_dir, _output);

                            string _result = item.Tag as string;

                            DurabilityData durability = m_functions.BuildDataFromSelection(m_dom_UserItems, _result, AnalysisModelType.Dynamics, ref errMessage);
                            if (durability == null)
                                continue;

                            if (false == m_functions.WriteResultToFile(FileFormat.MCF, m_resultType, _path, durability, ref errMessage))
                            {
                                //MessageBox.Show("Failed to export results to file.");
                                continue;
                            }

                        }

                        if("" != errMessage)
                        {
                            MessageBox.Show("Description : The following error occurred while writing the file.\n"
                                            + errMessage + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        int nCount = listView_result_list.Items.Count * listView_Map.Items.Count;
                        Define_Progress(0, nCount);

                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            foreach (ListViewItem item_map in listView_Map.Items)
                            {
                                pBar1.PerformStep();

                                string _output = _userNamed + "_" + item.Text + "_" + item_map.Text + ".mcf";
                                string _path = Path.Combine(_dir, _output);

                                string _result = item.Tag as string;
                                string _map = item_map.Tag as string;

                                DurabilityData durability = m_functions.BuildDataFromMap(_result, _map, AnalysisModelType.Dynamics, ref errMessage);
                                if (durability == null)
                                    continue;


                                if (false == m_functions.WriteResultToFile(FileFormat.MCF, m_resultType, _path, durability, ref errMessage))
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

                    if (248 <= _dir.Length)
                    {
                        MessageBox.Show("Error : The length of the directory is too long. Its length must be less than 248.\n" + "Directory : " + _dir + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (260 <= _userNamed.Length)
                    {
                        MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File name : " + _userNamed + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string errMessage = "";

                    if (0 == tab_main.SelectedIndex)
                    {
                        int nCount = listView_result_list.Items.Count;
                        Define_Progress(0, nCount);

                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            pBar1.PerformStep();

                            string _output = item.Text + "_" + _userNamed + ".csv";
                            string _path = Path.Combine(_dir, _output);

                            if (260 <= (_path.Length - 4))
                            {
                                MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File path : " + _path + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string _result = item.Tag as string;

                            DurabilityData durability = m_functions.BuildDataFromSelection(m_dom_UserItems, _result, AnalysisModelType.Dynamics, ref errMessage);
                            if (durability == null)
                                continue;

                            //if (durability.ExistChassis == false)
                            //{
                            //    string str_error = string.Format(" “{0}” cannot export time history data", _output);
                            //    MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    continue;
                            //}

                            if (false == m_functions.WriteResultToFile(FileFormat.CSV, m_resultType, _path, durability, ref errMessage))
                            {
                                //MessageBox.Show("Failed to export results to file.");
                                continue;
                            }

                        }
                    }
                    else
                    {
                        int nCount = listView_result_list.Items.Count * listView_Map.Items.Count;
                        Define_Progress(0, nCount );

                        foreach (ListViewItem item in listView_result_list.Items)
                        {
                            foreach (ListViewItem item_map in listView_Map.Items)
                            {
                                pBar1.PerformStep();

                                string _output = _userNamed + "_" + item.Text + "_" + item_map.Text + ".csv";
                                string _path = Path.Combine(_dir, _output);

                                string _result = item.Tag as string;
                                string _map = item_map.Tag as string;

                                DurabilityData durability = m_functions.BuildDataFromMap(_result, _map, AnalysisModelType.Dynamics, ref errMessage);
                                if (durability == null)
                                    continue;

                                if (durability.ExistChassis == false)
                                {
                                    string str_error = string.Format(" “{0}” cannot export time history data", _output);
                                    MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;
                                }

                                if (false == m_functions.WriteResultToFile(FileFormat.CSV, m_resultType, _path, durability, ref errMessage))
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

            if (0 == tab_main.SelectedIndex)
            {
                m_dom_UserItems = m_functions.CreateDurabilityXML();
                if (false == CreateNode_UserDefinedItems_for_Static(m_dom_UserItems, combo_Type.SelectedIndex))
                    return;
            }

            m_save_static = new SaveFileDialog();
            m_save_static.Title = "Save the static results for ANSYSMotion durability analysis";
            m_save_static.Filter = "CSV (*.csv)|*.csv";

            if (DialogResult.OK == m_save_static.ShowDialog())
            {
                StaticResult staticResult = null;
                int nSize = 0;
                string errMessage = "";

                if (0 == tab_main.SelectedIndex)
                {
                    staticResult = Get_StaticResult(m_dom_UserItems, combo_Type.SelectedIndex);

                    nSize = 1;
                    Define_Progress(0, nSize);

                    if (false == m_functions.WriteToStatic(m_save_static.FileName, staticResult, ref errMessage))
                        return;

                    pBar1.PerformStep();
                }
                else
                {
                    string _dir = Path.GetDirectoryName(m_save_static.FileName);
                    string _userNamed = Path.GetFileNameWithoutExtension(m_save_static.FileName);

                    if (248 <= _dir.Length)
                    {
                        MessageBox.Show("Error : The length of the directory is too long. Its length must be less than 248.\n" + "Directory : " + _dir + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (260 <= _userNamed.Length)
                    {
                        MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File name : " + _userNamed + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string str_path = "";
                    string str_map_path = "";
                    XmlDocument dom = null;
                    XmlNode node_Item = null;

                    nSize = listView_Map.Items.Count;
                    Define_Progress(0, nSize);
                    foreach (ListViewItem item in listView_Map.Items)
                    {
                        string _output = _userNamed + "_" + item.Text + ".csv";
                        str_path = Path.Combine(_dir, _output);

                        if (260 <= (str_path.Length - 4))
                        {
                            MessageBox.Show("Error : The length of the file path is too long. Its length must be less than 260.\n" + "File path : " + str_path + "\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        str_map_path = item.Tag as string;

                        dom = new XmlDocument();
                        dom.Load(str_map_path);

                        node_Item = dom.DocumentElement.SelectSingleNode("UserDefinedItems/Item");
                        string str_item_name = node_Item.Attributes.GetNamedItem("name").Value;

                        if("Bodies" == str_item_name)
                        {
                            XmlNode node_body = node_Item.SelectSingleNode("Body");
                            XmlNodeList lst_node_entity = node_body.SelectNodes("Entity");

                            foreach(XmlNode n in lst_node_entity)
                            {
                                if("motion" == n.Attributes.GetNamedItem("type").Value)
                                {
                                    node_body.RemoveChild(n);
                                }
                            }
                            
                            staticResult = Get_StaticResult(dom, 0);
                        }
                        else if ("Forces" == str_item_name)
                        {
                            XmlNodeList lst_node_force = node_Item.SelectNodes("Force");

                            foreach(XmlNode n_force in lst_node_force)
                            {
                                XmlNodeList lst_node_entity = n_force.SelectNodes("Entity");

                                foreach(XmlNode n in lst_node_entity)
                                {
                                    if (n.Attributes.GetNamedItem("name").Value.Contains("Relative") == true)
                                        n_force.RemoveChild(n);
                                }
                            }

                            staticResult = Get_StaticResult(dom, 1);
                        }
                        else
                        {
                            continue;
                        }


                        if (false == m_functions.WriteToStatic(str_path, staticResult, ref errMessage))
                            return;

                        pBar1.PerformStep();

                    }


                }
            }
        }

        private void dgv_Entity_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int nCount_true = 0;
            int nCount_false = 0;
            int nCount_row = dgv_Entity.RowCount;

            if(e.ColumnIndex == 0)
            {
                foreach(DataGridViewRow row in dgv_Entity.Rows)
                {
                    if ("true" == row.Cells[0].EditedFormattedValue.ToString().ToLower())
                        nCount_true++;
                    else
                        nCount_false++;
                }

                if(nCount_false == nCount_row || nCount_row > nCount_true)
                {
                    foreach (DataGridViewRow row in dgv_Entity.Rows)
                    {
                        row.Cells[0].Value = true;
                    }

                    //foreach (DataGridViewRow row in dgv_Entity.SelectedRows)
                    //{
                    //    row.Cells[0].Value = true;
                    //}

                }
                else if(nCount_true == nCount_row)
                {
                    foreach (DataGridViewRow row in dgv_Entity.Rows)
                        row.Cells[0].Value = false;

                    //foreach (DataGridViewRow row in dgv_Entity.SelectedRows)
                    //{
                    //    row.Cells[0].Value = false;
                    //}
                }
            }
            else if(e.ColumnIndex == 2)
            {
                foreach (DataGridViewRow row in dgv_Entity.Rows)
                {
                    if ("true" == row.Cells[2].EditedFormattedValue.ToString().ToLower())
                        nCount_true++;
                    else
                        nCount_false++;
                }

                if (nCount_false == nCount_row || nCount_row > nCount_true)
                {
                    foreach (DataGridViewRow row in dgv_Entity.Rows)
                        row.Cells[2].Value = true;

                    //foreach (DataGridViewRow row in dgv_Entity.SelectedRows)
                    //{
                    //    row.Cells[2].Value = true;
                    //}
                }
                else if (nCount_true == nCount_row)
                {
                    foreach (DataGridViewRow row in dgv_Entity.Rows)
                        row.Cells[2].Value = false;

                    //foreach (DataGridViewRow row in dgv_Entity.SelectedRows)
                    //{
                    //    row.Cells[2].Value = false;
                    //}
                }
            }
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
            //listView_RF.Items.Clear();
            if (0 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = false;
                gb_RF.Visible = true;
                btn_Write_CSV.Text = "Write CSV";

                btn_Export_Map.Visible = true;
                if (m_analysisScenario == AnalysisModelType.Dynamics)
                {
                    btn_Write_RPC.Visible = true;
                    btn_Write_CSV.Visible = true;
                    btn_WriteStaticResults.Visible = true;
                }
                else
                {
                    btn_Write_RPC.Visible = false;
                    btn_Write_CSV.Visible = false;
                    btn_WriteStaticResults.Visible = true;
                }

                dgv_Entity.Columns[2].Visible = true;
                gb_Unit.Visible = true;
                
            }
            else if (1 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = true;
                gb_RF.Visible = false;
                btn_Write_CSV.Text = "Write CSV";

                btn_Export_Map.Visible = true;
                if (m_analysisScenario == AnalysisModelType.Dynamics)
                {
                    btn_Write_RPC.Visible = true;
                    btn_Write_CSV.Visible = true;
                    btn_WriteStaticResults.Visible = true;
                }
                else
                {
                    btn_Write_RPC.Visible = false;
                    btn_Write_CSV.Visible = false;
                    btn_WriteStaticResults.Visible = true;
                }

                dgv_Entity.Columns[2].Visible = false;
                gb_Unit.Visible = true;
            }
            else if (2 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = true;
                gb_RF.Visible = false;
                btn_Write_CSV.Text = "Write MCF";

                btn_Export_Map.Visible = true;
                if (m_analysisScenario == AnalysisModelType.Dynamics)
                {
                    btn_Write_RPC.Visible = false;
                    btn_Write_CSV.Visible = true;
                    btn_WriteStaticResults.Visible = false;
                }
                else
                {
                    btn_Write_RPC.Visible = false;
                    btn_Write_CSV.Visible = true;
                    btn_WriteStaticResults.Visible = false;
                }

                dgv_Entity.Columns[2].Visible = false;
                gb_Unit.Visible = false;
            }

            if (listView_result_list.Items.Count > 0)
                ChangeDisplay_ListViewType_From_Combo_Type(combo_Type.SelectedIndex);
            else
                listView_type.Items.Clear();

            if (listView_type.Items.Count > 0)
                ChangeDisplay_listview_Entity_From_ListViewType(listView_type.Items[0]);
            else
                dgv_Entity.Rows.Clear();
        }

        private void listView_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ChangeDisplay_CBList_From_ListViewType(listView_type.SelectedItems[0]);
        }

        private void listView_type_Click(object sender, EventArgs e)
        {
            if (listView_type.SelectedItems.Count > 0)
            {
                ChangeDisplay_listview_Entity_From_ListViewType(listView_type.SelectedItems[0]);

                //ChangeBackColor_listview_Type();
            }
        }

        private void listView_type_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (combo_Type.SelectedIndex == 0) {
                if (e.NewValue == CheckState.Checked)
                {
                    if (m_item_type != null)
                        m_item_type.Checked = false;

                    m_item_type = listView_type.Items[e.Index];

                    ChangeDisplay_listview_Entity_From_ListViewType(m_item_type);
                }
                    
            }
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

            dgv_Entity.Rows.Clear();

            combo_Type.SelectedIndex = 0;
            combo_Force.SelectedIndex = 0;
            combo_length.SelectedIndex = 0;
            combo_Angle.SelectedIndex = 0;
            combo_Time.SelectedIndex = 0;

            m_functions = new Functions();

            listView_RF.Items.Clear();
            tb_Description.Text = "";

            //listView_RF.Items.Add("VehicleBody");
            //listView_RF.Items.Add("Global");

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
                int nCount = 0;
                if (0 == combo_Type.SelectedIndex)
                {
                    
                    if (0 == listView_type.Items.Count)
                    {
                        MessageBox.Show("There is no body data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    nCount = 0;
                    foreach (ListViewItem item in listView_type.Items)
                    {
                        if (true == item.Checked)
                            nCount++;
                    }

                    if (0 == nCount)
                    {
                        MessageBox.Show("There is no selected body. After selecting the body, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (0 == dgv_Entity.Rows.Count)
                    {
                        MessageBox.Show("There is no entity data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    nCount = 0;
                    foreach (DataGridViewRow row in dgv_Entity.Rows)
                    {
                        if (row.Cells[0].Value.ToString().ToLower() == "true")
                            nCount++;
                    }

                    if (0 == nCount)
                    {
                        MessageBox.Show("There is no selected entity. After selecting the entity, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    nCount = 0;
                    foreach(ListViewItem item_RF in listView_RF.Items)
                    {
                        if (item_RF.Checked)
                            nCount++;
                    }

                    if (0 == nCount)
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

                    nCount = 0;
                    foreach (ListViewItem item in listView_type.Items)
                    {
                        if (true == item.Checked)
                            nCount++;
                    }

                    if (0 == nCount)
                    {
                        MessageBox.Show("There is no selected force. After selecting the force, please try again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                   
                    if (0 == dgv_Entity.Rows.Count)
                    {
                        MessageBox.Show("There is no entity data. please check it! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    nCount = 0;
                    foreach (DataGridViewRow row in dgv_Entity.Rows)
                    {
                        if (row.Cells[0].Value.ToString().ToLower() == "true")
                            nCount++;
                    }
                    if (0 == nCount)
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

                    nCount = 0;
                    foreach (ListViewItem item in listView_type.Items)
                    {
                        if (true == item.Checked)
                            nCount++;
                    }

                    if (0 == nCount)
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

        void Define_Progress(int nMin, int nMax)
        {
            //pBar1 = new ProgressBar();
            pBar1.Visible = true;
            pBar1.Minimum = nMin;
            pBar1.Maximum = nMax;
            pBar1.Value = nMin;
            pBar1.Step = 1;
        }


        void Add_Result_List(string[] ar_path, bool _bUpdate)
        {
            int i;
            bool isSameAnalysisType = false;
            string errMessage = "";

            if (0 == listView_result_list.Items.Count || _bUpdate == true)
            {
                m_dom_Config = m_functions.CreateXMLFromPost(ar_path[0], ref errMessage);

                if ("dynamics" == m_dom_Config.DocumentElement.SelectSingleNode("Configuration/Result").Attributes.GetNamedItem("analysis").Value)
                {
                    m_analysisScenario = AnalysisModelType.Dynamics;

                    tb_Description.Text = "Only dynamic analysis results can be exported.";
                }
                else
                {
                    m_analysisScenario = AnalysisModelType.Statics;
                    tb_Description.Text = "Only static analysis results can be exported.";
                }

                if (_bUpdate == false)
                {
                    for (i = 0; i < ar_path.Length; i++)
                    {
                        isSameAnalysisType = false;

                        if (i > 0)
                        {
                            m_functions.Distinguish_Analysis_Type(m_analysisScenario, ar_path[i], ref isSameAnalysisType, ref errMessage);

                            if (false == isSameAnalysisType)
                            {
                                string _output1 = Path.GetFileNameWithoutExtension(ar_path[0]);
                                string _output2 = Path.GetFileNameWithoutExtension(ar_path[i]);
                                string str_error;
                                if (m_analysisScenario == AnalysisModelType.Dynamics)
                                    str_error = string.Format(" \"{0}\" cannot be added because the result type is different from \"{1} (Dynamics)\" ", _output2, _output1);
                                else
                                    str_error = string.Format(" \"{0}\" cannot be added because the result type is different from \"{1} (Static)\" ", _output2, _output1);


                                MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }
                        }

                        ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(ar_path[i]));
                        item.Tag = ar_path[i];

                        listView_result_list.Items.Add(item);
                    }
                }

                if (false == ChangeDisplay_ListViewType_From_Combo_Type(combo_Type.SelectedIndex))
                    return;

                // Button Visible
                listView_RF.Items.Clear();
                if (m_analysisScenario == AnalysisModelType.Dynamics)
                {
                    btn_WriteStaticResults.Visible = true;
                    btn_Write_CSV.Visible = true;
                    btn_Write_RPC.Visible = true;

                    if (listView_result_list.Items.Count > 0)
                    {
                        listView_RF.Items.Add("VehicleBody");
                        listView_RF.Items.Add("Global");

                        listView_RF.Items[0].Checked = true;
                        listView_RF.Items[1].Checked = true;
                    }
                }
                else
                {
                    btn_WriteStaticResults.Visible = true;
                    btn_Write_CSV.Visible = false;
                    btn_Write_RPC.Visible = false;

                    if (listView_result_list.Items.Count > 0)
                    {
                        listView_RF.Items.Add("Global");
                        listView_RF.Items[0].Checked = true;
                        listView_RF.Enabled = false;
                    }
                }

            }
            else
            {
                string str_old_path = "";
                bool is_Exist = false;

                for(i = 0; i < ar_path.Length; i++)
                {
                    is_Exist = false;
                    isSameAnalysisType = false;

                    foreach (ListViewItem lvi in listView_result_list.Items)
                    {
                        str_old_path = lvi.Tag as string;

                        if (true == ar_path[i].Contains(str_old_path))
                        {
                            is_Exist = true;
                            break;

                        }
                    }

                    m_functions.Distinguish_Analysis_Type(m_analysisScenario, ar_path[i], ref isSameAnalysisType, ref errMessage);
                    if (false == isSameAnalysisType)
                    {
                        string _output1 = listView_result_list.Items[0].Text;
                        string _output2 = Path.GetFileNameWithoutExtension(ar_path[i]);
                        string str_error, str_analysisType;
                        if (m_analysisScenario == AnalysisModelType.Dynamics)
                            str_analysisType = "Dynamics";
                        else
                            str_analysisType = "Static";

                        str_error = string.Format(" “{0}” cannot be added because the result type is different from {1}({2}) ", _output2, _output1, str_analysisType);
                        MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
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
            string str_Use = "false";

            string[] ar_Unit = new string[4];
            for (int i = 0; i < 4; i++)
                ar_Unit[i] = "";

            List<ListViewItem> lst_Checked = new List<ListViewItem>();
            foreach(ListViewItem item in listView_type.Items)
            {
                if (true == item.Checked)
                    lst_Checked.Add(item);
            }

            if(0 == Type_selectedIndex)
            {
                // Bodies
                node_Item = m_functions.CreateNodeAndAttribute(_dom, "Item", "name", "Bodies");

                string str_bd_name = lst_Checked[0].Text;

                XmlNode node_body = m_functions.CreateNodeAndAttribute(_dom, "Body", "name", str_bd_name);

                string str_entity_name = "";
                
                foreach(DataGridViewRow row in dgv_Entity.Rows)
                {
                    str_entity_name = row.Cells[1].Value.ToString();
                    str_Use = row.Cells[0].Value.ToString().ToLower();
                    XmlNode node_config_entity = row.Tag as XmlNode;
                    string str_type = node_config_entity.Attributes.GetNamedItem("type").Value;

                    if ("true" == str_Use)
                    {
                        if (str_type == "motion")
                        {
                            foreach (ListViewItem item_RF in listView_RF.Items)
                            {
                                if (true == item_RF.Checked)
                                {
                                    XmlNode node_Entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                                    m_functions.CreateAttributeXML(_dom, ref node_Entity, "type", str_type);

                                    if (row.Cells[2].Value.ToString().ToLower() == "true")
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
                        }
                        else
                        {
                            XmlNode node_Entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                            m_functions.CreateAttributeXML(_dom, ref node_Entity, "type", str_type);

                            if (row.Cells[2].Value.ToString().ToLower() == "true")
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
                foreach (ListViewItem item in lst_Checked)
                {
                    str_force_name = item.Text;
                    XmlNode node_Force = m_functions.CreateNodeAndAttribute(_dom, "Force", "name", str_force_name);

                    foreach(DataGridViewRow row in dgv_Entity.Rows)
                    {
                        if (row.Cells[0].Value.ToString().ToLower() == "true")
                        {
                            str_entity_name = row.Cells[1].Value.ToString();
                            XmlNode node_entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                            node_Force.AppendChild(node_entity);
                        }
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
                foreach (ListViewItem item in lst_Checked)
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

        bool CreateNode_UserDefinedItems_for_Static(XmlDocument _dom, int Type_selectedIndex)
        {
            XmlNode node_UserItems = _dom.DocumentElement.SelectSingleNode("UserDefinedItems");
            XmlNode node_Item = null;
            string str_Use = "false";

            string[] ar_Unit = new string[4];
            for (int i = 0; i < 4; i++)
                ar_Unit[i] = "";

            List<ListViewItem> lst_Checked = new List<ListViewItem>();
            foreach (ListViewItem item in listView_type.Items)
            {
                if (true == item.Checked)
                    lst_Checked.Add(item);
            }

            if (0 == Type_selectedIndex)
            {
                // Bodies
                node_Item = m_functions.CreateNodeAndAttribute(_dom, "Item", "name", "Bodies");

                string str_bd_name = lst_Checked[0].Text;

                XmlNode node_body = m_functions.CreateNodeAndAttribute(_dom, "Body", "name", str_bd_name);

                string str_entity_name = "";
                foreach (DataGridViewRow row in dgv_Entity.Rows)
                {
                    str_entity_name = row.Cells[1].Value as string;
                    str_Use = row.Cells[0].Value.ToString().ToLower();
                    XmlNode node_config_entity = row.Tag as XmlNode;
                    string str_type = node_config_entity.Attributes.GetNamedItem("type").Value;

                    if (str_type != "motion" && str_Use == "true")
                    {
                        XmlNode node_Entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                        m_functions.CreateAttributeXML(_dom, ref node_Entity, "type", str_type);

                        if (row.Cells[2].Value.ToString().ToLower() == "true")
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
            else if (1 == Type_selectedIndex)
            {
                // Forces
                node_Item = m_functions.CreateNodeAndAttribute(_dom, "Item", "name", "Forces");

                string str_force_name = "";
                string str_entity_name = "";
                foreach (ListViewItem item in lst_Checked)
                {
                    str_force_name = item.Text;
                    XmlNode node_Force = m_functions.CreateNodeAndAttribute(_dom, "Force", "name", str_force_name);

                    foreach (DataGridViewRow row in dgv_Entity.Rows)
                    {
                        str_entity_name = row.Cells[1].Value.ToString();
                        if (str_entity_name == "Force")
                        {
                            XmlNode node_entity = m_functions.CreateNodeAndAttribute(_dom, "Entity", "name", str_entity_name);
                            node_Force.AppendChild(node_entity);
                        }
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
            else if (2 == Type_selectedIndex)
            {
                // FE Modal bodies

                MessageBox.Show("FE Body is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;


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
            listView_type.CheckBoxes = true;
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

                foreach (ListViewItem item in listView_RF.Items)
                    item.Checked = true;

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
            //listView_Entity.Items.Clear();
            dgv_Entity.Rows.Clear();
            int nRow = 0;

            XmlNode node_data = _item.Tag as XmlNode;

            if ("Body" == node_data.Name)
            {
                // Bodies

                XmlNodeList lst_entity = node_data.SelectNodes("Entity");

                string str_E_name = "";
                string str_E_type = "";

                foreach(XmlNode n in lst_entity)
                {
                    str_E_name = n.Attributes.GetNamedItem("name").Value;
                    str_E_type = n.Attributes.GetNamedItem("type").Value;

                    if (m_analysisScenario == AnalysisModelType.Statics && str_E_type.Contains("motion"))
                        continue;

                    nRow = dgv_Entity.RowCount;
                    dgv_Entity.Rows.Insert(nRow, 1);
                    DataGridViewRow row = dgv_Entity.Rows[nRow];
                    
                    row.Cells[0].Value = true;
                    row.Cells[1].Value = str_E_name;
                    row.Cells[2].Value = true;
                    row.Tag = n;
                    //dgv_Entity.Rows.Add(row);
                    
                    //ListViewItem item = new ListViewItem(str_E_name);
                    //item.Tag = n;

                    //listView_Entity.Items.Add(item);

                }

                foreach (ListViewItem item in listView_RF.Items)
                    item.Checked = true;

            }
            else if ("Force" == node_data.Name)
            {
                // Forces

                XmlNodeList lst_entity = node_data.SelectNodes("Entity");

                string str_E_name = "";
                foreach (XmlNode n in lst_entity)
                {
                    str_E_name = n.Attributes.GetNamedItem("name").Value;

                    if (m_analysisScenario == AnalysisModelType.Statics && str_E_name.Contains("Relative"))
                        continue;

                    nRow = dgv_Entity.RowCount;
                    dgv_Entity.Rows.Insert(nRow, 1);
                    DataGridViewRow row = dgv_Entity.Rows[nRow];

                    row.Cells[0].Value = true;
                    row.Cells[1].Value = str_E_name;
                    row.Cells[2].Value = true;
                    row.Tag = n;
                    //dgv_Entity.Rows.Add(row);

                    //ListViewItem item = new ListViewItem(str_E_name);
                    //item.Tag = n;

                    //listView_Entity.Items.Add(item);

                }

            }
            //else if ("FBody" == node_data.Name)
            //{
            //    // FE Bodies
            //}

            return true;
        }

        StaticResult Get_StaticResult(XmlDocument dom, int nType)
        {
            StaticResult staticResult = new StaticResult();

            int nCount = 0;
            int nResult = 0;
            int nEndStep = 0;           
            int i;
            string errMessage = "";

            foreach (ListViewItem item in listView_result_list.Items)
            {
               

                string _result = item.Tag as string;

                DurabilityData durability = m_functions.BuildDataFromSelection(dom, _result, AnalysisModelType.Statics, ref errMessage);
                if (durability == null)
                    continue;

                nCount++;
                staticResult.ResultFiles.Add(item.Text);
                nResult = durability.NumOfResult;
                nEndStep = durability.FixedTimes.Count;
                if (0 == nType)
                {
                    Body body = durability.Body;
                    if (1 == nCount)
                    {
                        foreach (EntityForBody entity in body.Entities)
                        {
                            foreach (string str in entity.ResultNames)
                            {
                                staticResult.ForceNames.Add(str);
                            }
                        }
                    }

                    List<double> lst_data = new List<double>();

                    foreach (EntityForBody entity in body.Entities)
                    {
                        for (i = 0; i < entity.FixedStepValue[nEndStep - 1].Length; i++)
                        {
                            if(i < 3)
                                lst_data.Add(entity.FixedStepValue[nEndStep - 1][i] * entity.UnitScaleFactor[0]);
                            else
                                lst_data.Add(entity.FixedStepValue[nEndStep - 1][i] * entity.UnitScaleFactor[1]);
                        }
                    }

                    staticResult.StaticData.Add(lst_data.ToArray());
                }
                else if (1 == nType)
                {
                    if (1 == nCount)
                    {
                        foreach (Force force_data in durability.Forces)
                        {
                            foreach (EntityForForce entity in force_data.Entities)
                            {
                                foreach (string str in entity.ResultNames)
                                {
                                    staticResult.ForceNames.Add(str);
                                }
                            }
                        }
                    }

                    List<double> lst_data = new List<double>();

                    foreach (Force force_data in durability.Forces)
                    {
                        foreach (EntityForForce entity in force_data.Entities)
                        {
                            for (i = 0; i < entity.FixedStepValue[nEndStep - 1].Length; i++)
                            {
                                if (i < 3 || (5 < i && i < 9))
                                    lst_data.Add(entity.FixedStepValue[nEndStep - 1][i] * entity.UnitScaleFactor[0]);
                                else
                                    lst_data.Add(entity.FixedStepValue[nEndStep - 1][i] * entity.UnitScaleFactor[1]);
                            }
                        }
                    }

                    staticResult.StaticData.Add(lst_data.ToArray());
                }
            }

            return staticResult;
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

        void  ChangeBackColor_listview_Type()
        {
            foreach (ListViewItem item in listView_type.Items)
                item.ForeColor = SystemColors.WindowText;

            foreach (ListViewItem item in listView_type.SelectedItems)
                item.ForeColor = Color.Red; 

        }





        #endregion

       
    }
}
