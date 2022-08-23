
namespace Motion.Durability
{
    partial class Export
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlp_main = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_resultlist_main = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_resultlist_header = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Remove = new System.Windows.Forms.Button();
            this.listView_result_list = new System.Windows.Forms.ListView();
            this.tlp_Close = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Close = new System.Windows.Forms.Button();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            this.tlp_data_main = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_export_btn = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Export_Map = new System.Windows.Forms.Button();
            this.btn_Write_RPC = new System.Windows.Forms.Button();
            this.btn_Write_CSV = new System.Windows.Forms.Button();
            this.btn_WriteStaticResults = new System.Windows.Forms.Button();
            this.tab_main = new System.Windows.Forms.TabControl();
            this.tabPage_selection = new System.Windows.Forms.TabPage();
            this.tlp_selection = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_selection_type_data = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_type_header = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.combo_Type = new System.Windows.Forms.ComboBox();
            this.listView_type = new System.Windows.Forms.ListView();
            this.tlp_selection_entities1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.listView_Entity = new System.Windows.Forms.ListView();
            this.tlp_RF_Unit = new System.Windows.Forms.TableLayoutPanel();
            this.gb_RF = new System.Windows.Forms.GroupBox();
            this.listView_RF = new System.Windows.Forms.ListView();
            this.gb_Unit = new System.Windows.Forms.GroupBox();
            this.tlp_Unit = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.combo_Force = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.combo_length = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.combo_Angle = new System.Windows.Forms.ComboBox();
            this.combo_Time = new System.Windows.Forms.ComboBox();
            this.tlp_stepsize = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_stepsize = new System.Windows.Forms.TextBox();
            this.tabPage_map = new System.Windows.Forms.TabPage();
            this.tlp_map_main = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_Map_Add = new System.Windows.Forms.Button();
            this.btn_Map_Remove = new System.Windows.Forms.Button();
            this.listView_Map = new System.Windows.Forms.ListView();
            this.tlp_main.SuspendLayout();
            this.tlp_resultlist_main.SuspendLayout();
            this.tlp_resultlist_header.SuspendLayout();
            this.tlp_Close.SuspendLayout();
            this.tlp_data_main.SuspendLayout();
            this.tlp_export_btn.SuspendLayout();
            this.tab_main.SuspendLayout();
            this.tabPage_selection.SuspendLayout();
            this.tlp_selection.SuspendLayout();
            this.tlp_selection_type_data.SuspendLayout();
            this.tlp_type_header.SuspendLayout();
            this.tlp_selection_entities1.SuspendLayout();
            this.tlp_RF_Unit.SuspendLayout();
            this.gb_RF.SuspendLayout();
            this.gb_Unit.SuspendLayout();
            this.tlp_Unit.SuspendLayout();
            this.tlp_stepsize.SuspendLayout();
            this.tabPage_map.SuspendLayout();
            this.tlp_map_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp_main
            // 
            this.tlp_main.ColumnCount = 2;
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tlp_main.Controls.Add(this.tlp_resultlist_main, 0, 0);
            this.tlp_main.Controls.Add(this.tlp_Close, 1, 1);
            this.tlp_main.Controls.Add(this.tlp_data_main, 1, 0);
            this.tlp_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_main.Location = new System.Drawing.Point(0, 0);
            this.tlp_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_main.Name = "tlp_main";
            this.tlp_main.RowCount = 2;
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlp_main.Size = new System.Drawing.Size(1161, 510);
            this.tlp_main.TabIndex = 0;
            // 
            // tlp_resultlist_main
            // 
            this.tlp_resultlist_main.ColumnCount = 1;
            this.tlp_resultlist_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_resultlist_main.Controls.Add(this.tlp_resultlist_header, 0, 0);
            this.tlp_resultlist_main.Controls.Add(this.listView_result_list, 0, 1);
            this.tlp_resultlist_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_resultlist_main.Location = new System.Drawing.Point(2, 2);
            this.tlp_resultlist_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_resultlist_main.Name = "tlp_resultlist_main";
            this.tlp_resultlist_main.RowCount = 2;
            this.tlp_resultlist_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlp_resultlist_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_resultlist_main.Size = new System.Drawing.Size(286, 456);
            this.tlp_resultlist_main.TabIndex = 0;
            // 
            // tlp_resultlist_header
            // 
            this.tlp_resultlist_header.ColumnCount = 3;
            this.tlp_resultlist_header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_resultlist_header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tlp_resultlist_header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tlp_resultlist_header.Controls.Add(this.label1, 0, 0);
            this.tlp_resultlist_header.Controls.Add(this.btn_Add, 1, 0);
            this.tlp_resultlist_header.Controls.Add(this.btn_Remove, 2, 0);
            this.tlp_resultlist_header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_resultlist_header.Location = new System.Drawing.Point(2, 2);
            this.tlp_resultlist_header.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_resultlist_header.Name = "tlp_resultlist_header";
            this.tlp_resultlist_header.RowCount = 1;
            this.tlp_resultlist_header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_resultlist_header.Size = new System.Drawing.Size(282, 26);
            this.tlp_resultlist_header.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Result List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_Add
            // 
            this.btn_Add.BackColor = System.Drawing.Color.Orange;
            this.btn_Add.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Add.Location = new System.Drawing.Point(198, 2);
            this.btn_Add.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(39, 22);
            this.btn_Add.TabIndex = 1;
            this.btn_Add.Text = "+";
            this.btn_Add.UseVisualStyleBackColor = false;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Remove
            // 
            this.btn_Remove.BackColor = System.Drawing.Color.Orange;
            this.btn_Remove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Remove.Location = new System.Drawing.Point(241, 2);
            this.btn_Remove.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(39, 22);
            this.btn_Remove.TabIndex = 2;
            this.btn_Remove.Text = "-";
            this.btn_Remove.UseVisualStyleBackColor = false;
            this.btn_Remove.Click += new System.EventHandler(this.btn_Remove_Click);
            // 
            // listView_result_list
            // 
            this.listView_result_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_result_list.HideSelection = false;
            this.listView_result_list.Location = new System.Drawing.Point(2, 32);
            this.listView_result_list.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_result_list.Name = "listView_result_list";
            this.listView_result_list.Size = new System.Drawing.Size(282, 422);
            this.listView_result_list.TabIndex = 1;
            this.listView_result_list.UseCompatibleStateImageBehavior = false;
            this.listView_result_list.View = System.Windows.Forms.View.List;
            // 
            // tlp_Close
            // 
            this.tlp_Close.ColumnCount = 2;
            this.tlp_Close.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Close.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            this.tlp_Close.Controls.Add(this.btn_Close, 1, 0);
            this.tlp_Close.Controls.Add(this.pBar1, 0, 0);
            this.tlp_Close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Close.Location = new System.Drawing.Point(292, 462);
            this.tlp_Close.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_Close.Name = "tlp_Close";
            this.tlp_Close.RowCount = 1;
            this.tlp_Close.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Close.Size = new System.Drawing.Size(867, 46);
            this.tlp_Close.TabIndex = 1;
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.Orange;
            this.btn_Close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Close.Location = new System.Drawing.Point(707, 2);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(158, 42);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // pBar1
            // 
            this.pBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBar1.Location = new System.Drawing.Point(2, 2);
            this.pBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(701, 42);
            this.pBar1.TabIndex = 1;
            // 
            // tlp_data_main
            // 
            this.tlp_data_main.ColumnCount = 2;
            this.tlp_data_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_data_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            this.tlp_data_main.Controls.Add(this.tlp_export_btn, 1, 0);
            this.tlp_data_main.Controls.Add(this.tab_main, 0, 0);
            this.tlp_data_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_data_main.Location = new System.Drawing.Point(292, 2);
            this.tlp_data_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_data_main.Name = "tlp_data_main";
            this.tlp_data_main.RowCount = 1;
            this.tlp_data_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_data_main.Size = new System.Drawing.Size(867, 456);
            this.tlp_data_main.TabIndex = 2;
            // 
            // tlp_export_btn
            // 
            this.tlp_export_btn.ColumnCount = 1;
            this.tlp_export_btn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_export_btn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tlp_export_btn.Controls.Add(this.btn_Export_Map, 0, 1);
            this.tlp_export_btn.Controls.Add(this.btn_Write_RPC, 0, 3);
            this.tlp_export_btn.Controls.Add(this.btn_Write_CSV, 0, 5);
            this.tlp_export_btn.Controls.Add(this.btn_WriteStaticResults, 0, 7);
            this.tlp_export_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_export_btn.Location = new System.Drawing.Point(707, 2);
            this.tlp_export_btn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_export_btn.Name = "tlp_export_btn";
            this.tlp_export_btn.RowCount = 10;
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_export_btn.Size = new System.Drawing.Size(158, 452);
            this.tlp_export_btn.TabIndex = 0;
            // 
            // btn_Export_Map
            // 
            this.btn_Export_Map.BackColor = System.Drawing.Color.Orange;
            this.btn_Export_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Export_Map.Location = new System.Drawing.Point(2, 47);
            this.btn_Export_Map.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Export_Map.Name = "btn_Export_Map";
            this.btn_Export_Map.Size = new System.Drawing.Size(154, 41);
            this.btn_Export_Map.TabIndex = 0;
            this.btn_Export_Map.Text = "Export Map";
            this.btn_Export_Map.UseVisualStyleBackColor = false;
            this.btn_Export_Map.Click += new System.EventHandler(this.btn_Export_Map_Click);
            // 
            // btn_Write_RPC
            // 
            this.btn_Write_RPC.BackColor = System.Drawing.Color.Orange;
            this.btn_Write_RPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Write_RPC.Location = new System.Drawing.Point(2, 137);
            this.btn_Write_RPC.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Write_RPC.Name = "btn_Write_RPC";
            this.btn_Write_RPC.Size = new System.Drawing.Size(154, 41);
            this.btn_Write_RPC.TabIndex = 1;
            this.btn_Write_RPC.Text = "Write RPC III";
            this.btn_Write_RPC.UseVisualStyleBackColor = false;
            this.btn_Write_RPC.Click += new System.EventHandler(this.btn_Write_RPC_Click);
            // 
            // btn_Write_CSV
            // 
            this.btn_Write_CSV.BackColor = System.Drawing.Color.Orange;
            this.btn_Write_CSV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Write_CSV.Location = new System.Drawing.Point(2, 227);
            this.btn_Write_CSV.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Write_CSV.Name = "btn_Write_CSV";
            this.btn_Write_CSV.Size = new System.Drawing.Size(154, 41);
            this.btn_Write_CSV.TabIndex = 2;
            this.btn_Write_CSV.Text = "Write CSV";
            this.btn_Write_CSV.UseVisualStyleBackColor = false;
            this.btn_Write_CSV.Click += new System.EventHandler(this.btn_Write_CSV_Click);
            // 
            // btn_WriteStaticResults
            // 
            this.btn_WriteStaticResults.BackColor = System.Drawing.Color.Orange;
            this.btn_WriteStaticResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_WriteStaticResults.Location = new System.Drawing.Point(2, 317);
            this.btn_WriteStaticResults.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_WriteStaticResults.Name = "btn_WriteStaticResults";
            this.btn_WriteStaticResults.Size = new System.Drawing.Size(154, 41);
            this.btn_WriteStaticResults.TabIndex = 3;
            this.btn_WriteStaticResults.Text = "Write Static Results";
            this.btn_WriteStaticResults.UseVisualStyleBackColor = false;
            this.btn_WriteStaticResults.Click += new System.EventHandler(this.btn_WriteStaticResults_Click);
            // 
            // tab_main
            // 
            this.tab_main.Controls.Add(this.tabPage_selection);
            this.tab_main.Controls.Add(this.tabPage_map);
            this.tab_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_main.Location = new System.Drawing.Point(2, 2);
            this.tab_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tab_main.Name = "tab_main";
            this.tab_main.SelectedIndex = 0;
            this.tab_main.Size = new System.Drawing.Size(701, 452);
            this.tab_main.TabIndex = 1;
            this.tab_main.SelectedIndexChanged += new System.EventHandler(this.tab_main_SelectedIndexChanged);
            // 
            // tabPage_selection
            // 
            this.tabPage_selection.Controls.Add(this.tlp_selection);
            this.tabPage_selection.Location = new System.Drawing.Point(4, 22);
            this.tabPage_selection.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_selection.Name = "tabPage_selection";
            this.tabPage_selection.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_selection.Size = new System.Drawing.Size(693, 426);
            this.tabPage_selection.TabIndex = 0;
            this.tabPage_selection.Text = "By Selection";
            this.tabPage_selection.UseVisualStyleBackColor = true;
            // 
            // tlp_selection
            // 
            this.tlp_selection.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlp_selection.ColumnCount = 3;
            this.tlp_selection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlp_selection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlp_selection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlp_selection.Controls.Add(this.tlp_selection_type_data, 0, 0);
            this.tlp_selection.Controls.Add(this.tlp_selection_entities1, 1, 0);
            this.tlp_selection.Controls.Add(this.tlp_RF_Unit, 2, 0);
            this.tlp_selection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_selection.Location = new System.Drawing.Point(2, 2);
            this.tlp_selection.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_selection.Name = "tlp_selection";
            this.tlp_selection.RowCount = 1;
            this.tlp_selection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_selection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 427F));
            this.tlp_selection.Size = new System.Drawing.Size(689, 422);
            this.tlp_selection.TabIndex = 0;
            // 
            // tlp_selection_type_data
            // 
            this.tlp_selection_type_data.ColumnCount = 1;
            this.tlp_selection_type_data.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_selection_type_data.Controls.Add(this.tlp_type_header, 0, 0);
            this.tlp_selection_type_data.Controls.Add(this.listView_type, 0, 1);
            this.tlp_selection_type_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_selection_type_data.Location = new System.Drawing.Point(3, 3);
            this.tlp_selection_type_data.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_selection_type_data.Name = "tlp_selection_type_data";
            this.tlp_selection_type_data.RowCount = 2;
            this.tlp_selection_type_data.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlp_selection_type_data.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_selection_type_data.Size = new System.Drawing.Size(235, 416);
            this.tlp_selection_type_data.TabIndex = 0;
            // 
            // tlp_type_header
            // 
            this.tlp_type_header.ColumnCount = 2;
            this.tlp_type_header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.34085F));
            this.tlp_type_header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.65915F));
            this.tlp_type_header.Controls.Add(this.label2, 0, 0);
            this.tlp_type_header.Controls.Add(this.combo_Type, 1, 0);
            this.tlp_type_header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_type_header.Location = new System.Drawing.Point(2, 2);
            this.tlp_type_header.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_type_header.Name = "tlp_type_header";
            this.tlp_type_header.RowCount = 1;
            this.tlp_type_header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_type_header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_type_header.Size = new System.Drawing.Size(231, 18);
            this.tlp_type_header.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(2, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // combo_Type
            // 
            this.combo_Type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combo_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Type.FormattingEnabled = true;
            this.combo_Type.Items.AddRange(new object[] {
            "Body",
            "Force",
            "FE Modal Body"});
            this.combo_Type.Location = new System.Drawing.Point(85, 2);
            this.combo_Type.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.combo_Type.Name = "combo_Type";
            this.combo_Type.Size = new System.Drawing.Size(144, 20);
            this.combo_Type.TabIndex = 1;
            this.combo_Type.SelectedIndexChanged += new System.EventHandler(this.combo_Type_SelectedIndexChanged);
            // 
            // listView_type
            // 
            this.listView_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_type.HideSelection = false;
            this.listView_type.Location = new System.Drawing.Point(2, 24);
            this.listView_type.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_type.Name = "listView_type";
            this.listView_type.Size = new System.Drawing.Size(231, 390);
            this.listView_type.TabIndex = 1;
            this.listView_type.UseCompatibleStateImageBehavior = false;
            this.listView_type.View = System.Windows.Forms.View.List;
            this.listView_type.SelectedIndexChanged += new System.EventHandler(this.listView_type_SelectedIndexChanged);
            this.listView_type.Click += new System.EventHandler(this.listView_type_Click);
            // 
            // tlp_selection_entities1
            // 
            this.tlp_selection_entities1.ColumnCount = 1;
            this.tlp_selection_entities1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_selection_entities1.Controls.Add(this.label3, 0, 0);
            this.tlp_selection_entities1.Controls.Add(this.listView_Entity, 0, 1);
            this.tlp_selection_entities1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_selection_entities1.Location = new System.Drawing.Point(243, 3);
            this.tlp_selection_entities1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_selection_entities1.Name = "tlp_selection_entities1";
            this.tlp_selection_entities1.RowCount = 2;
            this.tlp_selection_entities1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlp_selection_entities1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_selection_entities1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlp_selection_entities1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlp_selection_entities1.Size = new System.Drawing.Size(235, 416);
            this.tlp_selection_entities1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 22);
            this.label3.TabIndex = 0;
            this.label3.Text = "Exported entities";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listView_Entity
            // 
            this.listView_Entity.CheckBoxes = true;
            this.listView_Entity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Entity.HideSelection = false;
            this.listView_Entity.Location = new System.Drawing.Point(2, 24);
            this.listView_Entity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_Entity.Name = "listView_Entity";
            this.listView_Entity.Size = new System.Drawing.Size(231, 390);
            this.listView_Entity.TabIndex = 1;
            this.listView_Entity.UseCompatibleStateImageBehavior = false;
            this.listView_Entity.View = System.Windows.Forms.View.List;
            // 
            // tlp_RF_Unit
            // 
            this.tlp_RF_Unit.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlp_RF_Unit.ColumnCount = 1;
            this.tlp_RF_Unit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_RF_Unit.Controls.Add(this.gb_RF, 0, 0);
            this.tlp_RF_Unit.Controls.Add(this.gb_Unit, 0, 1);
            this.tlp_RF_Unit.Controls.Add(this.tlp_stepsize, 0, 2);
            this.tlp_RF_Unit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_RF_Unit.Location = new System.Drawing.Point(483, 3);
            this.tlp_RF_Unit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_RF_Unit.Name = "tlp_RF_Unit";
            this.tlp_RF_Unit.RowCount = 3;
            this.tlp_RF_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.8805F));
            this.tlp_RF_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.1195F));
            this.tlp_RF_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlp_RF_Unit.Size = new System.Drawing.Size(203, 416);
            this.tlp_RF_Unit.TabIndex = 2;
            // 
            // gb_RF
            // 
            this.gb_RF.Controls.Add(this.listView_RF);
            this.gb_RF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_RF.Location = new System.Drawing.Point(3, 3);
            this.gb_RF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_RF.Name = "gb_RF";
            this.gb_RF.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_RF.Size = new System.Drawing.Size(197, 77);
            this.gb_RF.TabIndex = 0;
            this.gb_RF.TabStop = false;
            this.gb_RF.Text = "Reference frame for Motion";
            // 
            // listView_RF
            // 
            this.listView_RF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_RF.HideSelection = false;
            this.listView_RF.Location = new System.Drawing.Point(2, 16);
            this.listView_RF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_RF.Name = "listView_RF";
            this.listView_RF.Size = new System.Drawing.Size(193, 59);
            this.listView_RF.TabIndex = 0;
            this.listView_RF.UseCompatibleStateImageBehavior = false;
            this.listView_RF.View = System.Windows.Forms.View.List;
            // 
            // gb_Unit
            // 
            this.gb_Unit.Controls.Add(this.tlp_Unit);
            this.gb_Unit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Unit.Location = new System.Drawing.Point(3, 85);
            this.gb_Unit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_Unit.Name = "gb_Unit";
            this.gb_Unit.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_Unit.Size = new System.Drawing.Size(197, 302);
            this.gb_Unit.TabIndex = 1;
            this.gb_Unit.TabStop = false;
            this.gb_Unit.Text = "Unit Selection";
            // 
            // tlp_Unit
            // 
            this.tlp_Unit.ColumnCount = 2;
            this.tlp_Unit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Unit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Unit.Controls.Add(this.label4, 0, 0);
            this.tlp_Unit.Controls.Add(this.combo_Force, 1, 0);
            this.tlp_Unit.Controls.Add(this.label5, 0, 2);
            this.tlp_Unit.Controls.Add(this.combo_length, 1, 2);
            this.tlp_Unit.Controls.Add(this.label7, 0, 4);
            this.tlp_Unit.Controls.Add(this.label6, 0, 6);
            this.tlp_Unit.Controls.Add(this.combo_Angle, 1, 4);
            this.tlp_Unit.Controls.Add(this.combo_Time, 1, 6);
            this.tlp_Unit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Unit.Location = new System.Drawing.Point(2, 16);
            this.tlp_Unit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_Unit.Name = "tlp_Unit";
            this.tlp_Unit.RowCount = 10;
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_Unit.Size = new System.Drawing.Size(193, 284);
            this.tlp_Unit.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(2, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "Force :";
            // 
            // combo_Force
            // 
            this.combo_Force.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combo_Force.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Force.FormattingEnabled = true;
            this.combo_Force.Items.AddRange(new object[] {
            "N",
            "kg*f",
            "lbf"});
            this.combo_Force.Location = new System.Drawing.Point(98, 2);
            this.combo_Force.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.combo_Force.Name = "combo_Force";
            this.combo_Force.Size = new System.Drawing.Size(93, 20);
            this.combo_Force.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(2, 56);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 28);
            this.label5.TabIndex = 1;
            this.label5.Text = "Displacement :";
            // 
            // combo_length
            // 
            this.combo_length.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combo_length.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_length.FormattingEnabled = true;
            this.combo_length.Items.AddRange(new object[] {
            "mm",
            "m",
            "inch"});
            this.combo_length.Location = new System.Drawing.Point(98, 58);
            this.combo_length.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.combo_length.Name = "combo_length";
            this.combo_length.Size = new System.Drawing.Size(93, 20);
            this.combo_length.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(2, 112);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 28);
            this.label7.TabIndex = 3;
            this.label7.Text = "Angle :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(2, 168);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 28);
            this.label6.TabIndex = 2;
            this.label6.Text = "Time :";
            // 
            // combo_Angle
            // 
            this.combo_Angle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combo_Angle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Angle.FormattingEnabled = true;
            this.combo_Angle.Items.AddRange(new object[] {
            "deg",
            "rad"});
            this.combo_Angle.Location = new System.Drawing.Point(98, 114);
            this.combo_Angle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.combo_Angle.Name = "combo_Angle";
            this.combo_Angle.Size = new System.Drawing.Size(93, 20);
            this.combo_Angle.TabIndex = 6;
            // 
            // combo_Time
            // 
            this.combo_Time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combo_Time.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Time.FormattingEnabled = true;
            this.combo_Time.Items.AddRange(new object[] {
            "sec"});
            this.combo_Time.Location = new System.Drawing.Point(98, 170);
            this.combo_Time.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.combo_Time.Name = "combo_Time";
            this.combo_Time.Size = new System.Drawing.Size(93, 20);
            this.combo_Time.TabIndex = 7;
            // 
            // tlp_stepsize
            // 
            this.tlp_stepsize.ColumnCount = 2;
            this.tlp_stepsize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_stepsize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_stepsize.Controls.Add(this.label8, 0, 0);
            this.tlp_stepsize.Controls.Add(this.tb_stepsize, 1, 0);
            this.tlp_stepsize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_stepsize.Location = new System.Drawing.Point(3, 392);
            this.tlp_stepsize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_stepsize.Name = "tlp_stepsize";
            this.tlp_stepsize.RowCount = 1;
            this.tlp_stepsize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_stepsize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlp_stepsize.Size = new System.Drawing.Size(197, 21);
            this.tlp_stepsize.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(2, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 21);
            this.label8.TabIndex = 0;
            this.label8.Text = "Step size :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_stepsize
            // 
            this.tb_stepsize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_stepsize.Location = new System.Drawing.Point(100, 2);
            this.tb_stepsize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_stepsize.Name = "tb_stepsize";
            this.tb_stepsize.Size = new System.Drawing.Size(95, 21);
            this.tb_stepsize.TabIndex = 1;
            this.tb_stepsize.Text = "0.01";
            this.tb_stepsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabPage_map
            // 
            this.tabPage_map.Controls.Add(this.tlp_map_main);
            this.tabPage_map.Location = new System.Drawing.Point(4, 22);
            this.tabPage_map.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_map.Name = "tabPage_map";
            this.tabPage_map.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_map.Size = new System.Drawing.Size(693, 426);
            this.tabPage_map.TabIndex = 1;
            this.tabPage_map.Text = "By Map";
            this.tabPage_map.UseVisualStyleBackColor = true;
            // 
            // tlp_map_main
            // 
            this.tlp_map_main.ColumnCount = 4;
            this.tlp_map_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlp_map_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tlp_map_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tlp_map_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlp_map_main.Controls.Add(this.label9, 0, 0);
            this.tlp_map_main.Controls.Add(this.btn_Map_Add, 1, 0);
            this.tlp_map_main.Controls.Add(this.btn_Map_Remove, 2, 0);
            this.tlp_map_main.Controls.Add(this.listView_Map, 0, 1);
            this.tlp_map_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_map_main.Location = new System.Drawing.Point(2, 2);
            this.tlp_map_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlp_map_main.Name = "tlp_map_main";
            this.tlp_map_main.RowCount = 2;
            this.tlp_map_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlp_map_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_map_main.Size = new System.Drawing.Size(689, 422);
            this.tlp_map_main.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(2, 0);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(418, 30);
            this.label9.TabIndex = 0;
            this.label9.Text = "Map List";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Map_Add
            // 
            this.btn_Map_Add.BackColor = System.Drawing.Color.Orange;
            this.btn_Map_Add.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Map_Add.Location = new System.Drawing.Point(424, 2);
            this.btn_Map_Add.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Map_Add.Name = "btn_Map_Add";
            this.btn_Map_Add.Size = new System.Drawing.Size(39, 26);
            this.btn_Map_Add.TabIndex = 1;
            this.btn_Map_Add.Text = "+";
            this.btn_Map_Add.UseVisualStyleBackColor = false;
            this.btn_Map_Add.Click += new System.EventHandler(this.btn_Map_Add_Click);
            // 
            // btn_Map_Remove
            // 
            this.btn_Map_Remove.BackColor = System.Drawing.Color.Orange;
            this.btn_Map_Remove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Map_Remove.Location = new System.Drawing.Point(467, 2);
            this.btn_Map_Remove.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Map_Remove.Name = "btn_Map_Remove";
            this.btn_Map_Remove.Size = new System.Drawing.Size(39, 26);
            this.btn_Map_Remove.TabIndex = 2;
            this.btn_Map_Remove.Text = "-";
            this.btn_Map_Remove.UseVisualStyleBackColor = false;
            this.btn_Map_Remove.Click += new System.EventHandler(this.btn_Map_Remove_Click);
            // 
            // listView_Map
            // 
            this.listView_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Map.HideSelection = false;
            this.listView_Map.Location = new System.Drawing.Point(2, 32);
            this.listView_Map.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_Map.Name = "listView_Map";
            this.listView_Map.Size = new System.Drawing.Size(418, 388);
            this.listView_Map.TabIndex = 3;
            this.listView_Map.UseCompatibleStateImageBehavior = false;
            this.listView_Map.View = System.Windows.Forms.View.List;
            // 
            // Export
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 510);
            this.ControlBox = false;
            this.Controls.Add(this.tlp_main);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Export";
            this.Text = "Vehicle Simulation Result Export";
            this.tlp_main.ResumeLayout(false);
            this.tlp_resultlist_main.ResumeLayout(false);
            this.tlp_resultlist_header.ResumeLayout(false);
            this.tlp_resultlist_header.PerformLayout();
            this.tlp_Close.ResumeLayout(false);
            this.tlp_data_main.ResumeLayout(false);
            this.tlp_export_btn.ResumeLayout(false);
            this.tab_main.ResumeLayout(false);
            this.tabPage_selection.ResumeLayout(false);
            this.tlp_selection.ResumeLayout(false);
            this.tlp_selection_type_data.ResumeLayout(false);
            this.tlp_type_header.ResumeLayout(false);
            this.tlp_type_header.PerformLayout();
            this.tlp_selection_entities1.ResumeLayout(false);
            this.tlp_selection_entities1.PerformLayout();
            this.tlp_RF_Unit.ResumeLayout(false);
            this.gb_RF.ResumeLayout(false);
            this.gb_Unit.ResumeLayout(false);
            this.tlp_Unit.ResumeLayout(false);
            this.tlp_Unit.PerformLayout();
            this.tlp_stepsize.ResumeLayout(false);
            this.tlp_stepsize.PerformLayout();
            this.tabPage_map.ResumeLayout(false);
            this.tlp_map_main.ResumeLayout(false);
            this.tlp_map_main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_main;
        private System.Windows.Forms.TableLayoutPanel tlp_resultlist_main;
        private System.Windows.Forms.TableLayoutPanel tlp_resultlist_header;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Remove;
        private System.Windows.Forms.TableLayoutPanel tlp_Close;
        private System.Windows.Forms.TableLayoutPanel tlp_data_main;
        private System.Windows.Forms.TableLayoutPanel tlp_export_btn;
        private System.Windows.Forms.TabControl tab_main;
        private System.Windows.Forms.TabPage tabPage_selection;
        private System.Windows.Forms.TabPage tabPage_map;
        private System.Windows.Forms.TableLayoutPanel tlp_selection;
        private System.Windows.Forms.TableLayoutPanel tlp_selection_type_data;
        private System.Windows.Forms.TableLayoutPanel tlp_type_header;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox combo_Type;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.ListView listView_result_list;
        private System.Windows.Forms.ListView listView_type;
        private System.Windows.Forms.TableLayoutPanel tlp_selection_entities1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Export_Map;
        private System.Windows.Forms.Button btn_Write_RPC;
        private System.Windows.Forms.Button btn_Write_CSV;
        private System.Windows.Forms.Button btn_WriteStaticResults;
        private System.Windows.Forms.TableLayoutPanel tlp_RF_Unit;
        private System.Windows.Forms.GroupBox gb_RF;
        private System.Windows.Forms.GroupBox gb_Unit;
        private System.Windows.Forms.TableLayoutPanel tlp_Unit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox combo_Force;
        private System.Windows.Forms.ComboBox combo_length;
        private System.Windows.Forms.ComboBox combo_Angle;
        private System.Windows.Forms.ComboBox combo_Time;
        private System.Windows.Forms.TableLayoutPanel tlp_stepsize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_stepsize;
        private System.Windows.Forms.TableLayoutPanel tlp_map_main;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_Map_Add;
        private System.Windows.Forms.Button btn_Map_Remove;
        private System.Windows.Forms.ListView listView_Map;
        private System.Windows.Forms.ListView listView_RF;
        private System.Windows.Forms.ListView listView_Entity;
        private System.Windows.Forms.ProgressBar pBar1;
    }
}