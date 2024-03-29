﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using VM.Models;

namespace Motion.Durability
{
    public partial class Form_config : Form
    {
        OpenFileDialog m_open_motionresult = new OpenFileDialog();
        OpenFileDialog m_open_map = new OpenFileDialog();
        SaveFileDialog m_save_file = new SaveFileDialog();

        DurabilityData m_durability = null;
        Functions m_functions = null;
        FileFormat m_fileFormat;
        ResultValueType m_resultType;

        bool bDebugging = false;

        public Form_config()
        {
            InitializeComponent();
        }

        #region Member functions

        private void calculation()
        {
            int[] a = new int[5] { 1, 100, 301, 2900, 5000 };
            int[] b = new int[5];
            double nValue;
            // var compare;
            for (int i = 0; i < 5; i++)
            {
                nValue = Math.Log10(a[i]) / Math.Log10(2);
                b[i] = (int)Math.Ceiling(nValue);

                var compare = Math.Pow(2, b[i]);

                if (a[i] > compare)
                    b[i] = b[i] + 1;
            }

            tb_motionresult.Text = Math.Truncate(Math.Log10(9)).ToString() + ", " + Math.Truncate(Math.Log10(10)).ToString() + ", " + Math.Truncate(Math.Log10(100)).ToString();

        }

        private void Operation_File_Format()
        {
            if (rb_csv.Checked)
            {
                rb_original.Enabled = true;
                rb_transfrom.Enabled = true;
                rb_transfrom.Enabled = true;
            }
            else if (rb_RPC.Checked)
            {
                rb_fixedstep.Checked = true;
                rb_original.Enabled = false;
                rb_transfrom.Enabled = false;
            }
            else
            {
                rb_fixedstep.Checked = true;
                rb_original.Enabled = true;
                rb_transfrom.Enabled = false;
            }
        }

        #endregion


        #region Member Events

        private void btn_MotionResult_Click(object sender, EventArgs e)
        {
            m_open_motionresult.Title = "Select the ANSYSMotion result file";
            m_open_motionresult.Multiselect = false;
            m_open_motionresult.RestoreDirectory = true;
            m_open_motionresult.Filter = "DRF file(*.dfr)|*.dfr";

            if (DialogResult.OK == m_open_motionresult.ShowDialog())
            {
                tb_motionresult.Text = Path.GetFileName(m_open_motionresult.FileName);
            }
        }

        private void btn_Map_Click(object sender, EventArgs e)
        {
            m_open_map.Title = "Select the map file for ANSYSMotion durability analysis";
            m_open_map.Multiselect = false;
            m_open_map.RestoreDirectory = true;
            m_open_map.Filter = "Map file(*.xml)|*.xml";

            if (DialogResult.OK == m_open_map.ShowDialog())
            {
                tb_map.Text = Path.GetFileName(m_open_map.FileName);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            if (bDebugging)
            {
                //m_open_motionresult.FileName = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Example\Results\VM_Demo_Vehicle_lt.dfr");
                m_open_motionresult.FileName = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\2023\Force Export for durability\model\Work\Result_Simulation_T_Motor_Modified_20202710.dfg.dfr");

                //m_open_map.FileName = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Example\Map\Body_LCA_export.xml");
                //m_open_map.FileName = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Example\Map\Force_force_FrontLeft_export.xml");
                //m_open_map.FileName = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Example\Map\Force_Tire.xml");
                m_open_map.FileName = Path.Combine(Directory.GetCurrentDirectory(), @"D:\Development\Code\MotionCarDurability\Example\Map\FE_body.xml");

                //m_save_file.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Example\Output\RPC");

                if (rb_RPC.Checked)
                    m_save_file.FileName = Path.Combine(m_save_file.InitialDirectory, "Force_Tire.rsp");
                else if (rb_csv.Checked)
                    m_save_file.FileName = Path.Combine(m_save_file.InitialDirectory, "Force_Tire.csv");
                else if (rb_MCF.Checked)
                    m_save_file.FileName = Path.Combine(m_save_file.InitialDirectory, Path.GetFileName(m_open_motionresult.FileName) + "_");
            }

            if (m_open_map.FileName == "")
            {
                MessageBox.Show("There is not the Map file. Please check it");
                return;
            }

            if (m_open_motionresult.FileName == "")
            {
                MessageBox.Show("There is not the ANSYSMotion result file. Please check it");
                return;
            }

            if (rb_csv.Checked)
                m_fileFormat = FileFormat.CSV;
            else if (rb_RPC.Checked)
                m_fileFormat = FileFormat.RPC;
            else
                m_fileFormat = FileFormat.MCF;

            if (rb_original.Checked)
                m_resultType = ResultValueType.Original;
            else if (rb_transfrom.Checked)
                m_resultType = ResultValueType.Transform;
            else
                m_resultType = ResultValueType.FixedStep;

            m_save_file.RestoreDirectory = true;

            if (m_fileFormat == FileFormat.CSV)
                m_save_file.Filter = "CSV (*.csv)|*.csv";
            else if (m_fileFormat == FileFormat.RPC)
                m_save_file.Filter = "RPC III (*.rsp)|*.rsp";
            else
            {
                m_save_file.Title = "Please write a pre-fixed name for the creation of MCF(ex. pre-fixed name_ )";
                m_save_file.Filter = "Modal Coordinates File (*.mcf)|*.mcf";
                m_save_file.FileName = "pre-fixed name_";
            }

            if (DialogResult.OK == m_save_file.ShowDialog())
            {
                m_functions = new Functions();
                string errMessage = "";
                m_durability = m_functions.BuildDataFromMap(m_open_motionresult.FileName, m_open_map.FileName, AnalysisModelType.Dynamics, ref errMessage);

                if (m_durability == null)
                {
                    MessageBox.Show(errMessage, "Error");
                    return;
                }




                if (false == m_functions.WriteResultToFile(m_fileFormat, m_resultType, m_save_file.FileName, m_durability, ref errMessage))
                {
                    MessageBox.Show("Failed to export results to file.\n" + errMessage, "Error");
                    TB_savepath.Text = "Fail !!!";

                    return;
                }

                TB_savepath.Text = m_save_file.FileName;
            }



        }


        private void rb_csv_CheckedChanged(object sender, EventArgs e)
        {
            Operation_File_Format();
        }

        private void rb_RPC_CheckedChanged(object sender, EventArgs e)
        {
            Operation_File_Format();
        }

        private void rb_MCF_CheckedChanged(object sender, EventArgs e)
        {
            Operation_File_Format();
        }

        #endregion


    }
}
