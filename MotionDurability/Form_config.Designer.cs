
namespace Motion.Durability
{
    partial class Form_config
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
            this.gb_input = new System.Windows.Forms.GroupBox();
            this.tlp_input = new System.Windows.Forms.TableLayoutPanel();
            this.btn_MotionResult = new System.Windows.Forms.Button();
            this.btn_Map = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_motionresult = new System.Windows.Forms.RichTextBox();
            this.tb_map = new System.Windows.Forms.RichTextBox();
            this.gb_Output = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gb_fileformat = new System.Windows.Forms.GroupBox();
            this.tlp_fileformat = new System.Windows.Forms.TableLayoutPanel();
            this.rb_csv = new System.Windows.Forms.RadioButton();
            this.rb_RPC = new System.Windows.Forms.RadioButton();
            this.gb_resulttype = new System.Windows.Forms.GroupBox();
            this.tlp_resulttype = new System.Windows.Forms.TableLayoutPanel();
            this.rb_original = new System.Windows.Forms.RadioButton();
            this.rb_transfrom = new System.Windows.Forms.RadioButton();
            this.rb_fixedstep = new System.Windows.Forms.RadioButton();
            this.gb_save = new System.Windows.Forms.GroupBox();
            this.tlp_savepath = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_savepath = new System.Windows.Forms.RichTextBox();
            this.gb_Operation = new System.Windows.Forms.GroupBox();
            this.tlp_operation = new System.Windows.Forms.TableLayoutPanel();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.tlp_main.SuspendLayout();
            this.gb_input.SuspendLayout();
            this.tlp_input.SuspendLayout();
            this.gb_Output.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gb_fileformat.SuspendLayout();
            this.tlp_fileformat.SuspendLayout();
            this.gb_resulttype.SuspendLayout();
            this.tlp_resulttype.SuspendLayout();
            this.gb_save.SuspendLayout();
            this.tlp_savepath.SuspendLayout();
            this.gb_Operation.SuspendLayout();
            this.tlp_operation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp_main
            // 
            this.tlp_main.ColumnCount = 1;
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_main.Controls.Add(this.gb_input, 0, 0);
            this.tlp_main.Controls.Add(this.gb_Output, 0, 1);
            this.tlp_main.Controls.Add(this.gb_Operation, 0, 2);
            this.tlp_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_main.Location = new System.Drawing.Point(0, 0);
            this.tlp_main.Name = "tlp_main";
            this.tlp_main.RowCount = 3;
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.08772F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.91228F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tlp_main.Size = new System.Drawing.Size(772, 420);
            this.tlp_main.TabIndex = 0;
            // 
            // gb_input
            // 
            this.gb_input.Controls.Add(this.tlp_input);
            this.gb_input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_input.Location = new System.Drawing.Point(3, 3);
            this.gb_input.Name = "gb_input";
            this.gb_input.Size = new System.Drawing.Size(766, 114);
            this.gb_input.TabIndex = 0;
            this.gb_input.TabStop = false;
            this.gb_input.Text = "Input data";
            // 
            // tlp_input
            // 
            this.tlp_input.ColumnCount = 4;
            this.tlp_input.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlp_input.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlp_input.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlp_input.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlp_input.Controls.Add(this.btn_MotionResult, 3, 0);
            this.tlp_input.Controls.Add(this.btn_Map, 3, 1);
            this.tlp_input.Controls.Add(this.label1, 0, 0);
            this.tlp_input.Controls.Add(this.label2, 0, 1);
            this.tlp_input.Controls.Add(this.tb_motionresult, 2, 0);
            this.tlp_input.Controls.Add(this.tb_map, 2, 1);
            this.tlp_input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_input.Location = new System.Drawing.Point(3, 21);
            this.tlp_input.Name = "tlp_input";
            this.tlp_input.RowCount = 2;
            this.tlp_input.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_input.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_input.Size = new System.Drawing.Size(760, 90);
            this.tlp_input.TabIndex = 0;
            // 
            // btn_MotionResult
            // 
            this.btn_MotionResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_MotionResult.Location = new System.Drawing.Point(713, 3);
            this.btn_MotionResult.Name = "btn_MotionResult";
            this.btn_MotionResult.Size = new System.Drawing.Size(44, 39);
            this.btn_MotionResult.TabIndex = 0;
            this.btn_MotionResult.Text = "...";
            this.btn_MotionResult.UseVisualStyleBackColor = true;
            this.btn_MotionResult.Click += new System.EventHandler(this.btn_MotionResult_Click);
            // 
            // btn_Map
            // 
            this.btn_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Map.Location = new System.Drawing.Point(713, 48);
            this.btn_Map.Name = "btn_Map";
            this.btn_Map.Size = new System.Drawing.Size(44, 39);
            this.btn_Map.TabIndex = 1;
            this.btn_Map.Text = "...";
            this.btn_Map.UseVisualStyleBackColor = true;
            this.btn_Map.Click += new System.EventHandler(this.btn_Map_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 45);
            this.label1.TabIndex = 2;
            this.label1.Text = "Motion Result";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 45);
            this.label2.TabIndex = 3;
            this.label2.Text = "Map";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_motionresult
            // 
            this.tb_motionresult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_motionresult.Location = new System.Drawing.Point(429, 3);
            this.tb_motionresult.Name = "tb_motionresult";
            this.tb_motionresult.ReadOnly = true;
            this.tb_motionresult.Size = new System.Drawing.Size(278, 39);
            this.tb_motionresult.TabIndex = 4;
            this.tb_motionresult.Text = "";
            // 
            // tb_map
            // 
            this.tb_map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_map.Location = new System.Drawing.Point(429, 48);
            this.tb_map.Name = "tb_map";
            this.tb_map.ReadOnly = true;
            this.tb_map.Size = new System.Drawing.Size(278, 39);
            this.tb_map.TabIndex = 5;
            this.tb_map.Text = "";
            // 
            // gb_Output
            // 
            this.gb_Output.Controls.Add(this.tableLayoutPanel1);
            this.gb_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Output.Location = new System.Drawing.Point(3, 123);
            this.gb_Output.Name = "gb_Output";
            this.gb_Output.Size = new System.Drawing.Size(766, 216);
            this.gb_Output.TabIndex = 1;
            this.gb_Output.TabStop = false;
            this.gb_Output.Text = "Output ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gb_fileformat, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gb_resulttype, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gb_save, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(760, 192);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gb_fileformat
            // 
            this.gb_fileformat.Controls.Add(this.tlp_fileformat);
            this.gb_fileformat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_fileformat.Location = new System.Drawing.Point(3, 3);
            this.gb_fileformat.Name = "gb_fileformat";
            this.gb_fileformat.Size = new System.Drawing.Size(754, 57);
            this.gb_fileformat.TabIndex = 0;
            this.gb_fileformat.TabStop = false;
            this.gb_fileformat.Text = "File format";
            // 
            // tlp_fileformat
            // 
            this.tlp_fileformat.ColumnCount = 5;
            this.tlp_fileformat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_fileformat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_fileformat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_fileformat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_fileformat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_fileformat.Controls.Add(this.rb_csv, 1, 0);
            this.tlp_fileformat.Controls.Add(this.rb_RPC, 3, 0);
            this.tlp_fileformat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_fileformat.Location = new System.Drawing.Point(3, 21);
            this.tlp_fileformat.Name = "tlp_fileformat";
            this.tlp_fileformat.RowCount = 1;
            this.tlp_fileformat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_fileformat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tlp_fileformat.Size = new System.Drawing.Size(748, 33);
            this.tlp_fileformat.TabIndex = 0;
            // 
            // rb_csv
            // 
            this.rb_csv.AutoSize = true;
            this.rb_csv.Checked = true;
            this.rb_csv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rb_csv.Location = new System.Drawing.Point(152, 3);
            this.rb_csv.Name = "rb_csv";
            this.rb_csv.Size = new System.Drawing.Size(143, 27);
            this.rb_csv.TabIndex = 0;
            this.rb_csv.TabStop = true;
            this.rb_csv.Text = "CSV";
            this.rb_csv.UseVisualStyleBackColor = true;
            this.rb_csv.CheckedChanged += new System.EventHandler(this.rb_csv_CheckedChanged);
            // 
            // rb_RPC
            // 
            this.rb_RPC.AutoSize = true;
            this.rb_RPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rb_RPC.Location = new System.Drawing.Point(450, 3);
            this.rb_RPC.Name = "rb_RPC";
            this.rb_RPC.Size = new System.Drawing.Size(143, 27);
            this.rb_RPC.TabIndex = 1;
            this.rb_RPC.TabStop = true;
            this.rb_RPC.Text = "RPC";
            this.rb_RPC.UseVisualStyleBackColor = true;
            this.rb_RPC.CheckedChanged += new System.EventHandler(this.rb_RPC_CheckedChanged);
            // 
            // gb_resulttype
            // 
            this.gb_resulttype.Controls.Add(this.tlp_resulttype);
            this.gb_resulttype.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_resulttype.Location = new System.Drawing.Point(3, 66);
            this.gb_resulttype.Name = "gb_resulttype";
            this.gb_resulttype.Size = new System.Drawing.Size(754, 58);
            this.gb_resulttype.TabIndex = 1;
            this.gb_resulttype.TabStop = false;
            this.gb_resulttype.Text = "Result type";
            // 
            // tlp_resulttype
            // 
            this.tlp_resulttype.ColumnCount = 5;
            this.tlp_resulttype.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_resulttype.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_resulttype.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_resulttype.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_resulttype.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_resulttype.Controls.Add(this.rb_original, 0, 0);
            this.tlp_resulttype.Controls.Add(this.rb_transfrom, 2, 0);
            this.tlp_resulttype.Controls.Add(this.rb_fixedstep, 4, 0);
            this.tlp_resulttype.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_resulttype.Location = new System.Drawing.Point(3, 21);
            this.tlp_resulttype.Name = "tlp_resulttype";
            this.tlp_resulttype.RowCount = 1;
            this.tlp_resulttype.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_resulttype.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlp_resulttype.Size = new System.Drawing.Size(748, 34);
            this.tlp_resulttype.TabIndex = 0;
            // 
            // rb_original
            // 
            this.rb_original.AutoSize = true;
            this.rb_original.Checked = true;
            this.rb_original.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rb_original.Location = new System.Drawing.Point(3, 3);
            this.rb_original.Name = "rb_original";
            this.rb_original.Size = new System.Drawing.Size(143, 28);
            this.rb_original.TabIndex = 0;
            this.rb_original.TabStop = true;
            this.rb_original.Text = "Original value";
            this.rb_original.UseVisualStyleBackColor = true;
            // 
            // rb_transfrom
            // 
            this.rb_transfrom.AutoSize = true;
            this.rb_transfrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rb_transfrom.Location = new System.Drawing.Point(301, 3);
            this.rb_transfrom.Name = "rb_transfrom";
            this.rb_transfrom.Size = new System.Drawing.Size(143, 28);
            this.rb_transfrom.TabIndex = 1;
            this.rb_transfrom.Text = "Transformation value";
            this.rb_transfrom.UseVisualStyleBackColor = true;
            // 
            // rb_fixedstep
            // 
            this.rb_fixedstep.AutoSize = true;
            this.rb_fixedstep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rb_fixedstep.Location = new System.Drawing.Point(599, 3);
            this.rb_fixedstep.Name = "rb_fixedstep";
            this.rb_fixedstep.Size = new System.Drawing.Size(146, 28);
            this.rb_fixedstep.TabIndex = 2;
            this.rb_fixedstep.Text = "Fixed step value";
            this.rb_fixedstep.UseVisualStyleBackColor = true;
            // 
            // gb_save
            // 
            this.gb_save.Controls.Add(this.tlp_savepath);
            this.gb_save.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_save.Location = new System.Drawing.Point(3, 130);
            this.gb_save.Name = "gb_save";
            this.gb_save.Size = new System.Drawing.Size(754, 59);
            this.gb_save.TabIndex = 2;
            this.gb_save.TabStop = false;
            this.gb_save.Text = "Save path";
            // 
            // tlp_savepath
            // 
            this.tlp_savepath.ColumnCount = 3;
            this.tlp_savepath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_savepath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlp_savepath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlp_savepath.Controls.Add(this.label3, 0, 0);
            this.tlp_savepath.Controls.Add(this.TB_savepath, 2, 0);
            this.tlp_savepath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_savepath.Location = new System.Drawing.Point(3, 21);
            this.tlp_savepath.Name = "tlp_savepath";
            this.tlp_savepath.RowCount = 1;
            this.tlp_savepath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_savepath.Size = new System.Drawing.Size(748, 35);
            this.tlp_savepath.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 35);
            this.label3.TabIndex = 0;
            this.label3.Text = "Path";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TB_savepath
            // 
            this.TB_savepath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TB_savepath.Location = new System.Drawing.Point(226, 3);
            this.TB_savepath.Name = "TB_savepath";
            this.TB_savepath.ReadOnly = true;
            this.TB_savepath.Size = new System.Drawing.Size(519, 29);
            this.TB_savepath.TabIndex = 1;
            this.TB_savepath.Text = "";
            // 
            // gb_Operation
            // 
            this.gb_Operation.Controls.Add(this.tlp_operation);
            this.gb_Operation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Operation.Location = new System.Drawing.Point(3, 345);
            this.gb_Operation.Name = "gb_Operation";
            this.gb_Operation.Size = new System.Drawing.Size(766, 72);
            this.gb_Operation.TabIndex = 2;
            this.gb_Operation.TabStop = false;
            this.gb_Operation.Text = "Operation";
            // 
            // tlp_operation
            // 
            this.tlp_operation.ColumnCount = 5;
            this.tlp_operation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_operation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_operation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_operation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_operation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_operation.Controls.Add(this.btn_export, 1, 0);
            this.tlp_operation.Controls.Add(this.btn_close, 3, 0);
            this.tlp_operation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_operation.Location = new System.Drawing.Point(3, 21);
            this.tlp_operation.Name = "tlp_operation";
            this.tlp_operation.RowCount = 1;
            this.tlp_operation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_operation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlp_operation.Size = new System.Drawing.Size(760, 48);
            this.tlp_operation.TabIndex = 0;
            // 
            // btn_export
            // 
            this.btn_export.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_export.Location = new System.Drawing.Point(155, 3);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(146, 42);
            this.btn_export.TabIndex = 0;
            this.btn_export.Text = "Export";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // btn_close
            // 
            this.btn_close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_close.Location = new System.Drawing.Point(459, 3);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(146, 42);
            this.btn_close.TabIndex = 1;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // Form_config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 420);
            this.ControlBox = false;
            this.Controls.Add(this.tlp_main);
            this.Name = "Form_config";
            this.Text = "Motion durabilty configuration";
            this.tlp_main.ResumeLayout(false);
            this.gb_input.ResumeLayout(false);
            this.tlp_input.ResumeLayout(false);
            this.tlp_input.PerformLayout();
            this.gb_Output.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gb_fileformat.ResumeLayout(false);
            this.tlp_fileformat.ResumeLayout(false);
            this.tlp_fileformat.PerformLayout();
            this.gb_resulttype.ResumeLayout(false);
            this.tlp_resulttype.ResumeLayout(false);
            this.tlp_resulttype.PerformLayout();
            this.gb_save.ResumeLayout(false);
            this.tlp_savepath.ResumeLayout(false);
            this.tlp_savepath.PerformLayout();
            this.gb_Operation.ResumeLayout(false);
            this.tlp_operation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_main;
        private System.Windows.Forms.GroupBox gb_input;
        private System.Windows.Forms.GroupBox gb_Output;
        private System.Windows.Forms.GroupBox gb_Operation;
        private System.Windows.Forms.TableLayoutPanel tlp_input;
        private System.Windows.Forms.Button btn_MotionResult;
        private System.Windows.Forms.Button btn_Map;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox tb_motionresult;
        private System.Windows.Forms.RichTextBox tb_map;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gb_fileformat;
        private System.Windows.Forms.GroupBox gb_resulttype;
        private System.Windows.Forms.TableLayoutPanel tlp_fileformat;
        private System.Windows.Forms.TableLayoutPanel tlp_resulttype;
        private System.Windows.Forms.GroupBox gb_save;
        private System.Windows.Forms.TableLayoutPanel tlp_savepath;
        private System.Windows.Forms.TableLayoutPanel tlp_operation;
        private System.Windows.Forms.RadioButton rb_csv;
        private System.Windows.Forms.RadioButton rb_RPC;
        private System.Windows.Forms.RadioButton rb_original;
        private System.Windows.Forms.RadioButton rb_transfrom;
        private System.Windows.Forms.RadioButton rb_fixedstep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox TB_savepath;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Button btn_close;
    }
}

