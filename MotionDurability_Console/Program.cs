using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Motion.Durability
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Description
            // args description(~ 23R2)
            // args[0] : motion result path(*.dfr)
            // args[1] : Map file path(*.xml)
            // args[2] : Save file path
            // args[3] : Save file format (RPC, CSV, MCF, and Static)

            //System.Windows.Forms.MessageBox.Show("Debugging");
            //args = new string[4];
            //args[0] = @"D:\Development\2023\Force Export for durability\model\FE\Assemblies (3)\SCAR_gvw1920_BIW_FE_bph.dfr";
            //args[1] = @"D:\Development\2023\Force Export for durability\model\Map\FE_body2.xml";
            //args[2] = @"D:\Development\2023\Force Export for durability\model\Export\mcf\Test.mcf";
            //args[3] = "MCF";

            // args description(24R1 ~)
            // args[0] : WRITE
            // args[1] : motion result path(*.dfr)
            // args[2] : Map file path(*.xml)
            // args[3] : Save file path
            // args[4] : Save file format (RPC, CSV, MCF, and Static)

            // args[0] : READ
            // args[1] : Input file path
            // args[2] : Export file path


            //System.Windows.Forms.MessageBox.Show("Debugging");
            //args = new string[5];
            //args[0] = "WRITE";
            //args[1] = @"D:\Development\2023\Force Export for durability\model\FE\Assemblies (3)\SCAR_gvw1920_BIW_FE_bph.dfr";
            //args[2] = @"D:\Development\2023\Force Export for durability\model\Map\FE_body2.xml";
            //args[3] = @"D:\Development\2023\Force Export for durability\model\Export\mcf\Test.mcf";
            //args[4] = "MCF";
            #endregion

            String strError = "";
            if (args[0].ToUpper() == "WRITE")
            {
                if (5 != args.Length)
                {
                    Console.WriteLine("The size of argument must have 4. Please check the number of argument!");
                    return;
                }

                if (null == args[1])
                {
                    Console.WriteLine("The motion result path(*.dfr) is required. Please check 1st argument");
                    return;
                }

                if (null == args[2])
                {
                    Console.WriteLine("The map file path(*.xml) is required. Please check 2nd argument");
                    return;
                }

                if (null == args[3])
                {
                    Console.WriteLine("The save name and full path of the file are required.Please check 3rd argument");
                    return;
                }

                if (null == args[4])
                {
                    Console.WriteLine("The save format (RPC, CSV, MCF, and Static) is required.Please check 4th argument");
                    return;
                }

                
                string strFileFormat = args[4].ToUpper();
                Functions functions = new Functions();
                string _path = "";



                if ("RPC" == strFileFormat)
                {
                    _path = Path.Combine(args[3], ".rsp");
                    string _output = Path.GetFileName(_path);

                    DurabilityData durabilityData = functions.BuildDataFromMap(args[1], args[2], VM.Models.AnalysisModelType.Dynamics, ref strError);
                    if (null == durabilityData)
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }


                    if (durabilityData.ExistChassis == false)
                    {
                        Console.WriteLine(string.Format("Failed to complete {0} file creation because chassis body is not existed", _output));
                        return;
                    }

                    if (false == functions.WriteResultToFile(FileFormat.RPC, ResultValueType.FixedStep, _path, durabilityData, ref strError))
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0} file creation has been completed sucessfully.", _output));
                    }


                }
                else if ("CSV" == strFileFormat)
                {
                    _path = Path.Combine(args[3], ".csv");
                    string _output = Path.GetFileName(_path);

                    DurabilityData durabilityData = functions.BuildDataFromMap(args[1], args[2], VM.Models.AnalysisModelType.Dynamics, ref strError);
                    if (null == durabilityData)
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }

                    if (durabilityData.ExistChassis == false)
                    {
                        strError = string.Format(" “{0}” cannot export time history data", _output);
                        Console.WriteLine(strError);
                        return;
                    }

                    if (false == functions.WriteResultToFile(FileFormat.CSV, ResultValueType.FixedStep, _path, durabilityData, ref strError))
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0} file creation has been completed sucessfully.", _output));
                    }
                }
                else if ("MCF" == strFileFormat)
                {
                    _path = args[3]; //Path.Combine(args[2], ".mcf");
                    string _output = Path.GetFileName(_path);

                    DurabilityData durabilityData = functions.BuildDataFromMap(args[1], args[2], VM.Models.AnalysisModelType.Dynamics, ref strError);
                    if (null == durabilityData)
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }

                    if (false == functions.WriteResultToFile(FileFormat.MCF, ResultValueType.FixedStep, _path, durabilityData, ref strError))
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0} file creation has been completed sucessfully.", _output));
                    }
                }
                else if ("STATIC" == strFileFormat)
                {
                    StaticResult staticResult = new StaticResult();
                    XmlDocument dom = new XmlDocument();
                    XmlNode node_Item = null;

                    int nResult = 0;
                    int nEndStep = 0;
                    int i;

                    _path = Path.Combine(args[3], ".csv");
                    string _output = Path.GetFileName(_path);

                    dom.Load(args[2]);

                    node_Item = dom.DocumentElement.SelectSingleNode("UserDefinedItems/Item");
                    string str_item_name = node_Item.Attributes.GetNamedItem("name").Value;

                    if ("Bodies" == str_item_name)
                    {
                        XmlNode node_body = node_Item.SelectSingleNode("Body");
                        XmlNodeList lst_node_entity = node_body.SelectNodes("Entity");

                        foreach (XmlNode n in lst_node_entity)
                        {
                            if ("motion" == n.Attributes.GetNamedItem("type").Value)
                            {
                                node_body.RemoveChild(n);
                            }
                        }

                    }
                    else if ("Forces" == str_item_name)
                    {
                        XmlNodeList lst_node_force = node_Item.SelectNodes("Force");

                        foreach (XmlNode n_force in lst_node_force)
                        {
                            XmlNodeList lst_node_entity = n_force.SelectNodes("Entity");

                            foreach (XmlNode n in lst_node_entity)
                            {
                                if (n.Attributes.GetNamedItem("name").Value.Contains("Relative") == true)
                                    n_force.RemoveChild(n);
                            }
                        }

                    }
                    else
                    {
                        strError = string.Format("The {0} type is not supported! Please check map file", str_item_name);
                        Console.WriteLine(strError);
                        return;
                    }


                    DurabilityData durability = functions.BuildDataFromSelection(dom, args[1], VM.Models.AnalysisModelType.Statics, ref strError);
                    if (durability == null)
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }

                    staticResult.ResultFiles.Add(Path.GetFileNameWithoutExtension(args[1]));
                    nResult = durability.NumOfResult;
                    nEndStep = durability.FixedTimes.Count;

                    if ("Bodies" == str_item_name)
                    {
                        Body body = durability.Body;

                        foreach (EntityForBody entity in body.Entities)
                        {
                            foreach (string str in entity.ResultNames)
                            {
                                staticResult.ForceNames.Add(str);
                            }
                        }

                        List<double> lst_data = new List<double>();

                        foreach (EntityForBody entity in body.Entities)
                        {
                            for (i = 0; i < entity.FixedStepValue[nEndStep - 1].Length; i++)
                            {
                                if (i < 3)
                                    lst_data.Add(entity.FixedStepValue[nEndStep - 1][i] * entity.UnitScaleFactor[0]);
                                else
                                    lst_data.Add(entity.FixedStepValue[nEndStep - 1][i] * entity.UnitScaleFactor[1]);
                            }
                        }

                        staticResult.StaticData.Add(lst_data.ToArray());

                    }
                    else if ("Forces" == str_item_name)
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

                    if (false == functions.WriteToStatic(_path, staticResult, ref strError))
                    {
                        Console.WriteLine(strError);
                        Console.WriteLine(string.Format("Failed to complete {0} file creation", _output));
                        return;
                    }
                }
                else
                {
                    strError = string.Format("The {0} format is not supported! Please check file format", strFileFormat);
                    Console.WriteLine(strError);
                    return;
                }
            }
            else if(args[0].ToUpper() == "READ")
            {
                //System.Windows.Forms.MessageBox.Show("Debugging","RPC Reader");

                Functions functions = new Functions();
                if(false == functions.ReadFile(args[1], args[2], ref strError))
                {
                    Console.WriteLine(strError);
                    return;
                }
            }
            else
            {
                if(args.Length == 4)
                {
                    strError = "";
                    strError += "========================================================================================= \n";
                    strError += "Please modify it according to the purpose of use. \n";
                    strError += "1. Write file \n";
                    strError += "  A. WRITE \n";
                    strError += "  B. motion result path(*.dfr) \n";
                    strError += "  C. Map file path(*.xml) \n";
                    strError += "  D. Save file path \n";
                    strError += "  E. Save file format (RPC, CSV, MCF, and Static) \n";
                    strError += "2. Read file \n";
                    strError += "  A. READ \n";
                    strError += "  B. Input file path (*.rsp) \n";
                    strError += "  C. Export file path (*.csv) \n";
                    strError += "========================================================================================= \n";

                }
                else
                    strError = string.Format("The {0} operation is not supported! Please check file format", args[0]);

                Console.WriteLine(strError);
                return;
            }

        }
    }
}
