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

        DurabilityData m_durability = null;
        Functions m_functions = null;
        XmlDocument m_dom_Config = null;
        XmlDocument m_dom_UserItems = null;

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

        }

        private void btn_Write_RPC_Click(object sender, EventArgs e)
        {

        }

        private void btn_Write_CSV_Click(object sender, EventArgs e)
        {

        }

        private void btn_WriteStaticResults_Click(object sender, EventArgs e)
        {

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
                
            }
            else if (1 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = true;
                listView_RF.Enabled = false;
            }
            else if (2 == combo_Type.SelectedIndex)
            {
                listView_type.MultiSelect = true;
                listView_RF.Enabled = false;
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

            listView_RF.Items.Add("Vehicle Body");
            listView_RF.Items.Add("Global");

            m_dom_UserItems = m_functions.CreateDurabilityXML();

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

        bool CreateNode_UserDefinedItems(XmlDocument _dom, int Type_selectedIndex, ListViewItem _item)
        {
            XmlNode node_Config = _item.Tag as XmlNode;

            XmlNode node_UserItems = _dom.DocumentElement.SelectSingleNode("UserDefinedItems");
            XmlNode node_Item = null;
            

            if(0 == Type_selectedIndex)
            {
                // Bodies
                node_Item = m_functions.CreateNodeAndAttribute(_dom, "Item", "name", "Bodies");

                XmlNodeList lst_entity_Config = node_Config.SelectNodes("Entity");

                foreach (XmlNode n in lst_entity_Config)
                {
                    //XmlNode node_Body = m_functions.CreateNodeAndAttribute
                }

            }
            else if( 1 == Type_selectedIndex)
            {
                // Forces
            }
            else if(2 == Type_selectedIndex)
            {
                // FE Modal bodies
            }

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

                    listView_Entity.Items.Add(str_E_name);

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

                    listView_Entity.Items.Add(str_E_name);

                }

            }
            //else if ("FBody" == node_data.Name)
            //{
            //    // FE Bodies
            //}

            return true;
        }


        #endregion

       
    }
}
