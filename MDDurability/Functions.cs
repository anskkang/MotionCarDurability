using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using PostAPI;
using VM.Post.API.OutputReader;
using VM.Enums.Post;
using VM.Models;
using VM;
using VM.Models.Post;

namespace Motion.Durability
{
    public class Functions
    {
        string m_strResultPath;
        string m_strMapPath;
        public Functions() { }

        public DurabilityData BuildDataFromMap(string _strResultPath, string _strMapPath, AnalysisModelType scenario, ref string errMessage)
        {
            m_strMapPath = _strMapPath;

            XmlDocument dom = new XmlDocument();
            dom.Load(_strMapPath);

            DurabilityData durability = BuildDataFromSelection(dom, _strResultPath, scenario, ref errMessage);

            return durability;
        }

        public DurabilityData BuildDataFromSelection(XmlDocument dom, string _strResultPath, AnalysisModelType scenario, ref string errMessage)
        {
            m_strResultPath = _strResultPath;
            PostAPI.PostAPI postAPI = new PostAPI.PostAPI(_strResultPath);

            XmlNode node_UserDefinedItem = dom.DocumentElement.SelectSingleNode("UserDefinedItems");
            XmlNode node_Item = node_UserDefinedItem.SelectSingleNode("Item");
            XmlNode node_Unit = node_UserDefinedItem.SelectSingleNode("Unit");
            XmlNode node_Stepsize = node_UserDefinedItem.SelectSingleNode("Stepsize");

            DurabilityData durability = new DurabilityData();

            durability.StepSize = Convert.ToDouble(node_Stepsize.Attributes.GetNamedItem("value").Value);
            if (durability.StepSize <= 0.0)
            {
                //MessageBox.Show("The step size must be greater than zero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errMessage += "Error : The step size must be greater than zero \n";
                return null;
            }

            // Get Unit Conversion Factor
            if (null != node_Unit)
            {
                if (false == Conversion_Unit(postAPI, node_Unit, ref durability))
                    return null;
            }

            // Get Chassis data info
            if (false == GetChassisInfo(postAPI, ref durability))
                return null;


            string str_Category = node_Item.Attributes.GetNamedItem("name").Value;
            // Get Data each type
            if (str_Category == "Bodies")
            {
                durability.Type = Category.Bodies;
                if (false == BuildBodyFromMap(node_Item, postAPI, ref durability, ref errMessage))
                    return null;

                // Translate data in the each reference frame
                if (false == Translate_Data_For_Bodies(postAPI, ref durability))
                    return null;

                // Interpolation given step size
                if (false == Interpolation_For_Body(postAPI, ref durability, ref errMessage))
                    return null;

            }
            else if (str_Category == "Forces")
            {
                durability.Type = Category.Forces;
                if (false == BuildForceFromMap(node_Item, postAPI, ref durability, ref errMessage))
                    return null;

                // Translate data in the each reference frame
                if (false == Translate_Data_For_Forces(postAPI, ref durability))
                    return null;

                // Interpolation given step size
                if (false == Interpolation_For_Force(postAPI, ref durability, ref errMessage))
                    return null;
            }
            else if (str_Category == "Userdefinedfunctions")
            {
                durability.Type = Category.UserDefinedFunctions;
            }
            else if (str_Category == "FlexibleBodies")
            {
                durability.Type = Category.FEBodies;
                if (false == BuildFEBodyFromMap(node_Item, postAPI, ref durability, ref errMessage))
                    return null;

                // Interpolation given step size
                if (false == Interpolation_For_FEBody(postAPI, ref durability, ref errMessage))
                    return null;
            }
            else
            {
                return null;
            }

            postAPI.Close();

            return durability;
        }

        #region Bodies
        private bool BuildBodyFromMap(XmlNode _node_Item, PostAPI.PostAPI _postAPI, ref DurabilityData durability, ref string errMessage)
        {
            int i, j;
            Body body = durability.Body;

            XmlNode node_Body = _node_Item.SelectSingleNode("Body");
            XmlNodeList lst_Node = node_Body.SelectNodes("Entity");

            body.Name = node_Body.Attributes.GetNamedItem("name").Value;
            //List<EntityForBody> lst_entity = new List<EntityForBody>();
            EntityForBody entity = null;

            string str_type = "";
            PlotParameters parameters = null;
            List<string> str_curve_path = new List<string>();
            IList<Point2D> curve_point = null;
            IDictionary<string, IList<Point2D>> curve = null;
            double[] xarray = null;
            double[] yarray = null;
            List<double[]> lst_arry = new List<double[]>();

            #region Set Body CM Info

            IList<double[]> CMInfo = _postAPI.GetMarkerInfo(body.Name + "/CM");
            if (CMInfo == null || CMInfo.Count == 0)
            {
                string str_result = Path.GetFileName(m_strResultPath);
                string str_error = string.Format("The target body does not exist in “{0}”. . The “{1}” result cannot be exported", str_result, body.Name);
                errMessage += "Error : " + str_error + "\n";
                //MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            foreach (double[] tmp in CMInfo)
            {
                body.RF_Positions.Add(new double[3] { tmp[0], tmp[1], tmp[2] });
                body.RF_Orientations.Add(new double[9] { tmp[3], tmp[4], tmp[5], tmp[6], tmp[7], tmp[8], tmp[9], tmp[10], tmp[11] });
            }
            str_curve_path.Clear();
            str_curve_path.Add("Velocity/X");
            str_curve_path.Add("Velocity/Y");
            str_curve_path.Add("Velocity/Z");

            str_curve_path.Add("Angular Velocity/X");
            str_curve_path.Add("Angular Velocity/Y");
            str_curve_path.Add("Angular Velocity/Z");

            str_curve_path.Add("Acceleration/X");
            str_curve_path.Add("Acceleration/Y");
            str_curve_path.Add("Acceleration/Z");

            str_curve_path.Add("Angular Acceleration/X");
            str_curve_path.Add("Angular Acceleration/Y");
            str_curve_path.Add("Angular Acceleration/Z");

            //// T-Vel
            if (false == GetBodyCMInfo(_postAPI, body.Name, str_curve_path, 0, ref lst_arry))
                return false;

            for (i = 0; i < lst_arry.Count; i++)
            {
                if (lst_arry.Count > body.TranslationalVelocity.Count)
                    body.TranslationalVelocity.Add(new double[3] { 0.0, 0.0, 0.0 });

                for (j = 0; j < 3; j++)
                    body.TranslationalVelocity[i][j] = lst_arry[i][j];
            }
            //// R-Vel
            if (false == GetBodyCMInfo(_postAPI, body.Name, str_curve_path, 3, ref lst_arry))
                return false;

            for (i = 0; i < lst_arry.Count; i++)
            {
                if (lst_arry.Count > body.RotationalVelocity.Count)
                    body.RotationalVelocity.Add(new double[3] { 0.0, 0.0, 0.0 });

                for (j = 0; j < 3; j++)
                    body.RotationalVelocity[i][j] = lst_arry[i][j];
            }
            //// T-Acc
            if (false == GetBodyCMInfo(_postAPI, body.Name, str_curve_path, 6, ref lst_arry))
                return false;

            for (i = 0; i < lst_arry.Count; i++)
            {
                if (lst_arry.Count > body.TranslationalAcceleration.Count)
                    body.TranslationalAcceleration.Add(new double[3] { 0.0, 0.0, 0.0 });

                for (j = 0; j < 3; j++)
                    body.TranslationalAcceleration[i][j] = lst_arry[i][j];
            }
            //// R-Acc
            if (false == GetBodyCMInfo(_postAPI, body.Name, str_curve_path, 9, ref lst_arry))
                return false;

            for (i = 0; i < lst_arry.Count; i++)
            {
                if (lst_arry.Count > body.RotationalAcceleration.Count)
                    body.RotationalAcceleration.Add(new double[3] { 0.0, 0.0, 0.0 });

                for (j = 0; j < 3; j++)
                    body.RotationalAcceleration[i][j] = lst_arry[i][j];
            }

            #endregion

            #region Set Original time history

            parameters = new PlotParameters();
            parameters.Target = body.Name;
            parameters.Paths.Add("Displacement/X");
            curve = _postAPI.GetCurves(parameters);
            if (curve == null)
                return false;
            curve_point = curve[body.Name + "/Displacement/X"];
            xarray = curve_point.Select(s => s.X).ToArray();
            foreach (double _dtime in xarray)
                durability.OriginalTimes.Add(_dtime);

            #endregion

            #region Get Entity data

            var connectors = _postAPI.GetConnectors(body.Name);
            char seperator = '/';
            string result_name = "";
            //string unit_entity = "";
            foreach (XmlNode n in lst_Node)
            {
                str_type = n.Attributes.GetNamedItem("type").Value;

                entity = new EntityForBody();
                entity.Name = n.Attributes.GetNamedItem("name").Value;
                if (str_type == "force")
                    entity.ConnectionType = ConnectionTypeForBody.force;
                else if (str_type == "contraints")
                    entity.ConnectionType = ConnectionTypeForBody.constraints;
                else if (str_type == "motion")
                {
                    entity.ConnectionType = ConnectionTypeForBody.motion;

                    if (n.Attributes.GetNamedItem("reference_frame").Value == "VehicleBody")
                    {
                        entity.ReferenceFrame = ReferenceFrameOfMotion.vehiclebody;
                        if (durability.ExistChassis == false)
                            continue;
                    }
                    else
                        entity.ReferenceFrame = ReferenceFrameOfMotion.global;

                }

                entity.UseRotationFlag = Convert.ToBoolean(n.Attributes.GetNamedItem("rotation_flag").Value);

                if (entity.ConnectionType != ConnectionTypeForBody.motion)
                {
                    for (i = 0; i < connectors.Count; i++)
                    {
                        if (connectors[i].Item3 == entity.Name)
                        {
                            if (connectors[i].Item2 == VM.Enums.Post.ActionType.Base)
                                entity.AppliedForceType = BaseOrActionForce.Base;
                            else
                                entity.AppliedForceType = BaseOrActionForce.Action;
                        }
                        else
                        {
                            //if( i == (connectors.Count -1) )
                            //{
                            //    string str_result = Path.GetFileName(m_strResultPath);
                            //    string str_error = string.Format("Force( {2} ) is not an element connected with Body in “{0}”. The “{1}” : Force result cannot be exported", str_result, body.Name, entity.Name);
                            //    MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    return false;
                            //}
                        }
                    }
                }

                #region Motion
                if (entity.ConnectionType == ConnectionTypeForBody.motion)
                {
                    result_name = body.Name + seperator;
                    if (entity.Name == "Displacement")
                    {
                        entity.UnitScaleFactor[0] = durability.Scale_Length;
                        entity.Unit1 = durability.Unit_Length;
                        //unit_entity = durability.Unit_Length;

                    }
                    else if (entity.Name == "Velocity")
                    {
                        entity.UnitScaleFactor[0] = durability.Scale_Length / durability.Scale_Time;
                        entity.Unit1 = durability.Unit_Length + "/" + durability.Unit_Time;
                        //unit_entity = durability.Unit_Length + "/" + durability.Unit_Time;
                    }
                    else
                    {
                        entity.UnitScaleFactor[0] = durability.Scale_Length / (durability.Scale_Time * durability.Scale_Time);
                        entity.Unit1 = durability.Unit_Length + "/" + durability.Unit_Time + "^2";
                        //unit_entity = durability.Unit_Length + "/" + durability.Unit_Time + "^2";
                    }

                    if (entity.ReferenceFrame == ReferenceFrameOfMotion.vehiclebody)
                    {
                        //entity.ResultNames.Add(result_name + entity.Name + seperator + "X_RF_Vehicle(" + unit_entity + ")");
                        //entity.ResultNames.Add(result_name + entity.Name + seperator + "Y_RF_Vehicle(" + unit_entity + ")");
                        //entity.ResultNames.Add(result_name + entity.Name + seperator + "Z_RF_Vehicle(" + unit_entity + ")");

                        entity.ResultNames.Add(result_name + entity.Name + seperator + "X_RF_Vehicle");
                        entity.ResultNames.Add(result_name + entity.Name + seperator + "Y_RF_Vehicle");
                        entity.ResultNames.Add(result_name + entity.Name + seperator + "Z_RF_Vehicle");
                    }
                    else
                    {
                        //entity.ResultNames.Add(result_name + entity.Name + seperator + "X_RF_Global(" + unit_entity + ")");
                        //entity.ResultNames.Add(result_name + entity.Name + seperator + "Y_RF_Global(" + unit_entity + ")");
                        //entity.ResultNames.Add(result_name + entity.Name + seperator + "Z_RF_Global(" + unit_entity + ")");

                        entity.ResultNames.Add(result_name + entity.Name + seperator + "X_RF_Global");
                        entity.ResultNames.Add(result_name + entity.Name + seperator + "Y_RF_Global");
                        entity.ResultNames.Add(result_name + entity.Name + seperator + "Z_RF_Global");
                    }


                    parameters = new PlotParameters();

                    str_curve_path.Clear();
                    str_curve_path.Add(entity.Name + "/X");
                    str_curve_path.Add(entity.Name + "/Y");
                    str_curve_path.Add(entity.Name + "/Z");

                    parameters.Target = body.Name;
                    parameters.Paths.Add(entity.Name + "/X");
                    parameters.Paths.Add(entity.Name + "/Y");
                    parameters.Paths.Add(entity.Name + "/Z");

                    if (true == entity.UseRotationFlag)
                    {
                        if (entity.Name == "Displacement")
                        {
                            entity.UnitScaleFactor[1] = durability.Scale_Angle;
                            entity.Unit2 = durability.Unit_Angle;
                            //unit_entity = durability.Unit_Angle;
                            if (entity.ReferenceFrame == ReferenceFrameOfMotion.vehiclebody)
                            {
                                //entity.ResultNames.Add(result_name + "Angle" + seperator + "Roll_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angle" + seperator + "Pitch_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angle" + seperator + "Yaw_RF_Vehicle(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Angle" + seperator + "Roll_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Angle" + seperator + "Pitch_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Angle" + seperator + "Yaw_RF_Vehicle");
                            }
                            else
                            {
                                //entity.ResultNames.Add(result_name + "Angle" + seperator + "Roll_RF_Global(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angle" + seperator + "Pitch_RF_Global(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angle" + seperator + "Yaw_RF_Global(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Angle" + seperator + "Roll_RF_Global");
                                entity.ResultNames.Add(result_name + "Angle" + seperator + "Pitch_RF_Global");
                                entity.ResultNames.Add(result_name + "Angle" + seperator + "Yaw_RF_Global");
                            }


                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;
                            List<double[]> zyx = new List<double[]>();
                            for (i = 0; i < 3; i++)
                            {
                                curve_point = curve[body.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        zyx.Add(new double[3] { 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }

                            // Get Roll, Pitch, Yaw
                            if (false == lib_math.Calculate_ROLL_PITCH_YAW(CMInfo, ref zyx, 0))
                                return false;

                            for (i = 0; i < entity.OrinalValue.Count; i++)
                            {
                                for (j = 0; j < 3; j++)
                                    entity.OrinalValue[i][j + 3] = zyx[i][j];
                            }



                        }
                        else if (entity.Name == "Velocity")
                        {
                            entity.UnitScaleFactor[1] = durability.Scale_Angle / durability.Scale_Time;
                            entity.Unit2 = durability.Unit_Angle + "/" + durability.Unit_Time;
                            //unit_entity = durability.Unit_Angle + "/" + durability.Unit_Time;

                            if (entity.ReferenceFrame == ReferenceFrameOfMotion.vehiclebody)
                            {
                                //entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "X_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Y_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Z_RF_Vehicle(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "X_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Y_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Z_RF_Vehicle");
                            }
                            else
                            {
                                //entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "X_RF_Global(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Y_RF_Global(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Z_RF_Global(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "X_RF_Global");
                                entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Y_RF_Global");
                                entity.ResultNames.Add(result_name + "Angular_Velocity" + seperator + "Z_RF_Global");
                            }

                            // In own reference frame
                            str_curve_path.Add("Angular Velocity/X");
                            str_curve_path.Add("Angular Velocity/Y");
                            str_curve_path.Add("Angular Velocity/Z");

                            parameters.Paths.Add("Angular Velocity/X");
                            parameters.Paths.Add("Angular Velocity/Y");
                            parameters.Paths.Add("Angular Velocity/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[body.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            entity.UnitScaleFactor[1] = durability.Scale_Angle / (durability.Scale_Time * durability.Scale_Time);
                            entity.Unit2 = durability.Unit_Angle + "/" + durability.Unit_Time + "^2";
                            //unit_entity = durability.Unit_Angle + "/" + durability.Unit_Time +"^2";

                            if (entity.ReferenceFrame == ReferenceFrameOfMotion.vehiclebody)
                            {
                                //entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "X_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Y_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Z_RF_Vehicle(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "X_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Y_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Z_RF_Vehicle");
                            }
                            else
                            {
                                //entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "X_RF_Global(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Y_RF_Global(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Z_RF_Global(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "X_RF_Global");
                                entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Y_RF_Global");
                                entity.ResultNames.Add(result_name + "Angular_Acceleration" + seperator + "Z_RF_Global");
                            }

                            // In own reference frame
                            str_curve_path.Add("Angular Acceleration/X");
                            str_curve_path.Add("Angular Acceleration/Y");
                            str_curve_path.Add("Angular Acceleration/Z");

                            parameters.Paths.Add("Angular Acceleration/X");
                            parameters.Paths.Add("Angular Acceleration/Y");
                            parameters.Paths.Add("Angular Acceleration/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[body.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }
                        }
                    }
                    else
                    {
                        curve = _postAPI.GetCurves(parameters);
                        if (curve == null)
                            return false;

                        for (i = 0; i < str_curve_path.Count; i++)
                        {
                            curve_point = curve[body.Name + "/" + str_curve_path[i]];
                            yarray = curve_point.Select(s => s.Y).ToArray();

                            j = 0;
                            foreach (double yvalue in yarray)
                            {
                                if (i == 0)
                                {
                                    entity.OrinalValue.Add(new double[3] { 0.0, 0.0, 0.0 });
                                    entity.TransformValue.Add(new double[3] { 0.0, 0.0, 0.0 });
                                    entity.FixedStepValue.Add(new double[3] { 0.0, 0.0, 0.0 });

                                    if (j == 0)
                                    {
                                        xarray = curve_point.Select(s => s.X).ToArray();
                                        durability.EndTime = xarray[yarray.Length - 1];
                                    }
                                }

                                entity.OrinalValue[j][i] = yvalue;
                                j++;
                            }
                        }
                    }
                }
                #endregion
                #region Others
                else
                {
                    entity.UnitScaleFactor[0] = durability.Scale_Force;
                    entity.Unit1 = durability.Unit_Force;
                    //unit_entity = durability.Unit_Force;

                    //entity.ResultNames.Add(entity.Name + seperator + "FX(" + unit_entity + ")");
                    //entity.ResultNames.Add(entity.Name + seperator + "FY(" + unit_entity + ")");
                    //entity.ResultNames.Add(entity.Name + seperator + "FZ(" + unit_entity + ")");

                    entity.ResultNames.Add(entity.Name + seperator + "FX");
                    entity.ResultNames.Add(entity.Name + seperator + "FY");
                    entity.ResultNames.Add(entity.Name + seperator + "FZ");

                    parameters = new PlotParameters();

                    if (entity.AppliedForceType == BaseOrActionForce.Base)
                        str_type = "Force on Base Marker";
                    else
                        str_type = "Force on Action Marker";

                    str_curve_path.Clear();
                    str_curve_path.Add(str_type + "/X");
                    str_curve_path.Add(str_type + "/Y");
                    str_curve_path.Add(str_type + "/Z");

                    parameters.Target = entity.Name;
                    parameters.Paths.Add(str_type + "/X");
                    parameters.Paths.Add(str_type + "/Y");
                    parameters.Paths.Add(str_type + "/Z");

                    if (true == entity.UseRotationFlag)
                    {
                        entity.UnitScaleFactor[1] = durability.Scale_Force * durability.Scale_Length;
                        entity.Unit2 = durability.Unit_Force + durability.Unit_Length;
                        //unit_entity = durability.Unit_Force + durability.Unit_Length;

                        //entity.ResultNames.Add(entity.Name + seperator + "TX(" + unit_entity + ")");
                        //entity.ResultNames.Add(entity.Name + seperator + "TY(" + unit_entity + ")");
                        //entity.ResultNames.Add(entity.Name + seperator + "TZ(" + unit_entity + ")");

                        entity.ResultNames.Add(entity.Name + seperator + "TX");
                        entity.ResultNames.Add(entity.Name + seperator + "TY");
                        entity.ResultNames.Add(entity.Name + seperator + "TZ");

                        if (entity.AppliedForceType == BaseOrActionForce.Base)
                            str_type = "Torque on Base Marker";
                        else
                            str_type = "Torque on Action Marker";

                        str_curve_path.Add(str_type + "/X");
                        str_curve_path.Add(str_type + "/Y");
                        str_curve_path.Add(str_type + "/Z");

                        parameters.Paths.Add(str_type + "/X");
                        parameters.Paths.Add(str_type + "/Y");
                        parameters.Paths.Add(str_type + "/Z");
                    }

                    curve = _postAPI.GetCurves(parameters);
                    if (curve == null)
                        return false;

                    for (i = 0; i < str_curve_path.Count; i++)
                    {
                        curve_point = curve[entity.Name + "/" + str_curve_path[i]];
                        yarray = curve_point.Select(s => s.Y).ToArray();

                        j = 0;
                        foreach (double yvalue in yarray)
                        {
                            if (i == 0)
                            {
                                if (entity.UseRotationFlag == false)
                                {
                                    entity.OrinalValue.Add(new double[3] { 0.0, 0.0, 0.0 });
                                    entity.TransformValue.Add(new double[3] { 0.0, 0.0, 0.0 });
                                    entity.FixedStepValue.Add(new double[3] { 0.0, 0.0, 0.0 });
                                }
                                else
                                {
                                    entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                    entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                    entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                }

                                if (j == 0)
                                {
                                    xarray = curve_point.Select(s => s.X).ToArray();
                                    durability.EndTime = xarray[yarray.Length - 1];
                                }
                            }

                            entity.OrinalValue[j][i] = yvalue;
                            j++;
                        }
                    }


                }
                #endregion

                body.Entities.Add(entity);

            }

            #endregion



            return true;
        }

        private bool Translate_Data_For_Bodies(PostAPI.PostAPI _postAPI, ref DurabilityData durability)
        {
            int i, j, nlength;
            double[] Ci = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] Ai = new double[9];
            double[] Ai_I = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] Fi = new double[3];
            double[] Fi_p = new double[3] { 0.0, 0.0, 0.0 };
            double[] Ti = new double[3];
            double[] Ti_p = new double[3] { 0.0, 0.0, 0.0 };

            durability.Body.RF_Orientations[0].CopyTo(Ci, 0);
            nlength = durability.Body.RF_Orientations.Count;

            foreach (EntityForBody entity in durability.Body.Entities)
            {
                if (entity.ConnectionType == ConnectionTypeForBody.force || entity.ConnectionType == ConnectionTypeForBody.constraints)
                {
                    for (i = 0; i < nlength; i++)
                    {
                        for (j = 0; j < 3; j++)
                        {
                            Ai[j] = durability.Body.RF_Orientations[i][j];
                            Ai[j + 3] = durability.Body.RF_Orientations[i][j + 3];
                            Ai[j + 6] = durability.Body.RF_Orientations[i][j + 6];

                            Fi[j] = entity.OrinalValue[i][j];
                        }

                        lib_math.matmattrvec(Ci, Ai, Fi, ref Fi_p);

                        for (j = 0; j < 3; j++)
                            entity.TransformValue[i][j] = Fi_p[j];

                        if (entity.UseRotationFlag == true)
                        {
                            for (j = 0; j < 3; j++)
                                Ti[j] = entity.OrinalValue[i][j + 3];

                            lib_math.matmattrvec(Ci, Ai, Ti, ref Ti_p);

                            for (j = 0; j < 3; j++)
                                entity.TransformValue[i][j + 3] = Ti_p[j];
                        }

                    }
                }
                else
                {
                    double[] zyx = new double[3] { 0.0, 0.0, 0.0 };
                    double[] wi = new double[3];
                    double[] wi_I = new double[3] { 0.0, 0.0, 0.0 };
                    double[] wi_dot = new double[3];
                    double[] wi_I_dot = new double[3] { 0.0, 0.0, 0.0 };

                    if (entity.ReferenceFrame == ReferenceFrameOfMotion.global)
                    {
                        for (i = 0; i < nlength; i++)
                        {
                            for (j = 0; j < 3; j++)
                                entity.TransformValue[i][j] = entity.OrinalValue[i][j];

                            if (entity.UseRotationFlag == true)
                            {
                                if (entity.Name == "Displacement")
                                {
                                    for (j = 0; j < 3; j++)
                                    {
                                        Ai[j] = durability.Body.RF_Orientations[i][j];
                                        Ai[j + 3] = durability.Body.RF_Orientations[i][j + 3];
                                        Ai[j + 6] = durability.Body.RF_Orientations[i][j + 6];
                                    }

                                    lib_math.matmattr(Ai, Ci, ref Ai_I);
                                    lib_math.Calculate_ROLL_PITCH_YAW(Ai_I, ref zyx);

                                    for (j = 0; j < 3; j++)
                                        entity.TransformValue[i][j + 3] = zyx[j];
                                }
                                else if (entity.Name == "Velocity")
                                {
                                    for (j = 0; j < 3; j++)
                                        wi[j] = entity.OrinalValue[i][j + 3];

                                    lib_math.matvec(Ci, wi, ref wi_I);

                                    for (j = 0; j < 3; j++)
                                        entity.TransformValue[i][j + 3] = wi_I[j];
                                }
                                else
                                {
                                    for (j = 0; j < 3; j++)
                                        wi_dot[j] = entity.OrinalValue[i][j + 3];

                                    lib_math.matvec(Ci, wi_dot, ref wi_I_dot);

                                    for (j = 0; j < 3; j++)
                                        entity.TransformValue[i][j + 3] = wi_I_dot[j];
                                }
                            }
                        }

                    }
                    else
                    {
                        double[] Cj = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                        double[] Aj = new double[9];

                        if (durability.ExistChassis)
                            durability.OrientationOfChassis[0].CopyTo(Cj, 0);
                        else
                            continue;

                        if (entity.Name == "Displacement")
                        {
                            double[] rij = new double[3];
                            double[] rij_p = new double[3] { 0.0, 0.0, 0.0 };
                            double[] Cj_AjT_Ai = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                            double[] Aij_I_p = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

                            for (i = 0; i < nlength; i++)
                            {
                                for (j = 0; j < 3; j++)
                                {
                                    Ai[j] = durability.Body.RF_Orientations[i][j];
                                    Ai[j + 3] = durability.Body.RF_Orientations[i][j + 3];
                                    Ai[j + 6] = durability.Body.RF_Orientations[i][j + 6];

                                    Aj[j] = durability.OrientationOfChassis[i][j];
                                    Aj[j + 3] = durability.OrientationOfChassis[i][j + 3];
                                    Aj[j + 6] = durability.OrientationOfChassis[i][j + 6];

                                    rij[j] = entity.OrinalValue[i][j] - durability.PositionOfChassis[i][j];
                                }

                                lib_math.matmattrvec(Cj, Aj, rij, ref rij_p);

                                for (j = 0; j < 3; j++)
                                    entity.TransformValue[i][j] = rij_p[j];

                                if (entity.UseRotationFlag == true)
                                {
                                    lib_math.matmattrmat(Cj, Aj, Ai, ref Cj_AjT_Ai);
                                    lib_math.matmattr(Cj_AjT_Ai, Ci, ref Aij_I_p);

                                    lib_math.Calculate_ROLL_PITCH_YAW(Aij_I_p, ref zyx);

                                    for (j = 0; j < 3; j++)
                                        entity.TransformValue[i][j + 3] = zyx[j];
                                }
                            }
                        }
                        else if (entity.Name == "Velocity")
                        {
                            double[] rij_dot = new double[3];
                            double[] rij_p_dot = new double[3] { 0.0, 0.0, 0.0 };
                            double[] CjAjT = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                            double[] Ciwi = new double[3] { 0.0, 0.0, 0.0 };
                            double[] wj = new double[3];
                            double[] Cjwj = new double[3] { 0.0, 0.0, 0.0 };
                            double[] wij_I = new double[3];
                            double[] wij_p = new double[3] { 0.0, 0.0, 0.0 };

                            for (i = 0; i < nlength; i++)
                            {
                                for (j = 0; j < 3; j++)
                                {
                                    Aj[j] = durability.OrientationOfChassis[i][j];
                                    Aj[j + 3] = durability.OrientationOfChassis[i][j + 3];
                                    Aj[j + 6] = durability.OrientationOfChassis[i][j + 6];
                                    rij_dot[j] = durability.Body.TranslationalVelocity[i][j] - durability.TranslationalVelocity[i][j];
                                }

                                //lib_math.matmattrvec(Cj, Aj, rij_dot, ref rij_p_dot);
                                lib_math.matmattr(Cj, Aj, ref CjAjT);
                                lib_math.matvec(CjAjT, rij_dot, ref rij_p_dot);

                                for (j = 0; j < 3; j++)
                                    entity.TransformValue[i][j] = rij_p_dot[j];

                                if (entity.UseRotationFlag == true)
                                {
                                    for (j = 0; j < 3; j++)
                                    {
                                        wi[j] = entity.OrinalValue[i][j + 3];
                                        wj[j] = durability.RotationalVelocity[i][j];
                                    }

                                    lib_math.matvec(Ci, wi, ref Ciwi);
                                    lib_math.matvec(Cj, wj, ref Cjwj);

                                    for (j = 0; j < 3; j++)
                                        wij_I[j] = Ciwi[j] - Cjwj[j];

                                    lib_math.matvec(CjAjT, wij_I, ref wij_p);

                                    for (j = 0; j < 3; j++)
                                        entity.TransformValue[i][j + 3] = wij_p[j];
                                }
                            }
                        }
                        else
                        {
                            double[] rij_ddot = new double[3];
                            double[] rij_p_ddot = new double[3] { 0.0, 0.0, 0.0 };
                            double[] CjAjT = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                            double[] Ciwi_dot = new double[3] { 0.0, 0.0, 0.0 };
                            double[] wj_dot = new double[3];
                            double[] Cjwj_dot = new double[3] { 0.0, 0.0, 0.0 };
                            double[] wij_I_dot = new double[3];
                            double[] wij_p_dot = new double[3] { 0.0, 0.0, 0.0 };

                            for (i = 0; i < nlength; i++)
                            {
                                for (j = 0; j < 3; j++)
                                {
                                    Aj[j] = durability.OrientationOfChassis[i][j];
                                    Aj[j + 3] = durability.OrientationOfChassis[i][j + 3];
                                    Aj[j + 6] = durability.OrientationOfChassis[i][j + 6];
                                    rij_ddot[j] = durability.Body.TranslationalAcceleration[i][j] - durability.TranslationalAcceleration[i][j];
                                }

                                lib_math.matmattr(Cj, Aj, ref CjAjT);
                                lib_math.matvec(CjAjT, rij_ddot, ref rij_p_ddot);

                                for (j = 0; j < 3; j++)
                                    entity.TransformValue[i][j] = rij_p_ddot[j];

                                if (entity.UseRotationFlag == true)
                                {
                                    for (j = 0; j < 3; j++)
                                    {
                                        wi_dot[j] = entity.OrinalValue[i][j + 3];
                                        wj_dot[j] = durability.RotationalAcceleration[i][j];
                                    }

                                    lib_math.matvec(Ci, wi_dot, ref Ciwi_dot);
                                    lib_math.matvec(Cj, wj_dot, ref Cjwj_dot);

                                    for (j = 0; j < 3; j++)
                                        wij_I_dot[j] = Ciwi_dot[j] - Cjwj_dot[j];

                                    lib_math.matvec(CjAjT, wij_I_dot, ref wij_p_dot);

                                    for (j = 0; j < 3; j++)
                                        entity.TransformValue[i][j + 3] = wij_p_dot[j];
                                }
                            }
                        }







                    }
                }
            }

            return true;
        }

        private bool Interpolation_For_Body(PostAPI.PostAPI _postAPI, ref DurabilityData durability, ref string errMessage)
        {
            int i, j, k;
            double[] xarray = null;
            double[] yarray = null;
            double err_tol = 1.0e-10;
            double y_value = 0.0, y_max = 0.0;
            bool bSkipInterpolation = false;
            bool bChangeStep = false;

            int nDiffer = 0;
            int nSize_list = durability.Body.Entities[0].TransformValue.Count;
            int nSize_arr = 6;
            k = 0;
            xarray = durability.OriginalTimes.ToArray();
            yarray = new double[nSize_list];

            if(xarray.Length < 3)
            {
                if (xarray.Length == 1)
                    bSkipInterpolation = true;
                else if (xarray[0] == xarray[1])
                    bSkipInterpolation = true;
            }

            if (bSkipInterpolation == false)
            {
                if (false == Determine_Result_Step(ref durability, ref errMessage))
                    return false;

                if (nSize_list != durability.ResultStep)
                {
                    bChangeStep = true;
                }

                foreach (EntityForBody entity in durability.Body.Entities)
                {
                    nSize_arr = entity.TransformValue[0].Length;
                    //entity.FixedStepValue.Clear();

                    if(bChangeStep)
                        entity.FixedStepValue.Clear();


                    for (i = 0; i < nSize_arr; i++)
                    {
                        for (j = 0; j < nSize_list; j++)
                        {
                            yarray[j] = entity.TransformValue[j][i];

                            //if (i == 0)
                            //    entity.FixedStepValue.Add(new double[nSize_arr]);
                        }

                        var result = _postAPI.InterpolationAkimaSpline(xarray, yarray, nSize_list, durability.ResultStep, xarray[0], durability.EndTime_Modify);

                        if (i == 0 && k == 0 )
                        {
                            durability.FixedTimes.Clear();
                            for (j = 0; j < durability.ResultStep; j++)
                                durability.FixedTimes.Add(result.Item2[j]);
                        }

                        for (j = 0; j < durability.ResultStep; j++)
                        {
                            y_value = result.Item3[j];
                            if (err_tol > Math.Abs(y_value))
                                y_value = 0.0;

                            if (j == 0)
                                y_max = Math.Abs(y_value);
                            else
                            {
                                if (Math.Abs(y_value) > y_max)
                                    y_max = Math.Abs(y_value);

                            }

                            if (bChangeStep == true && i == 0)
                                entity.FixedStepValue.Add(new double[nSize_arr]);

                            entity.FixedStepValue[j][i] = y_value;
                        }

                        entity.MaxValues.Add((y_max));
                    }

                    nDiffer = entity.FixedStepValue.Count - durability.ResultStep;
                    if (nDiffer > 0)
                    {
                        for (i = 0; i < nDiffer; i++)
                        {
                            nSize_list = entity.FixedStepValue.Count - 1;

                            entity.FixedStepValue.RemoveAt(nSize_list);
                        }
                    }

                    k++;
                }
            }
            else
            {
                durability.FixedTimes.Clear();
                for(i = 0; i < xarray.Length; i++)
                    durability.FixedTimes.Add(xarray[i]);

                foreach (EntityForBody entity in durability.Body.Entities)
                {
                    nSize_arr = entity.TransformValue[0].Length;

                    for (i = 0; i < nSize_arr; i++)
                    {
                        for (j = 0; j < nSize_list; j++)
                        {
                            y_value = entity.TransformValue[j][i];

                            if (err_tol > Math.Abs(y_value))
                                y_value = 0.0;

                            if (j == 0)
                                y_max = Math.Abs(y_value);
                            else
                            {
                                if (Math.Abs(y_value) > y_max)
                                    y_max = Math.Abs(y_value);

                            }

                            entity.FixedStepValue[j][i] = y_value;
                        }
                        entity.MaxValues.Add((y_max));
                    }

                    k++;
                }
            }


            return true;
        }

        private bool GetBodyCMInfo(PostAPI.PostAPI postAPI, string str_Target_body, List<string> path, int nStartIndexOfPath, ref List<double[]> lst_arry)
        {
            int j, k;
            IEnumerable<(BodyType, string)> bodies = postAPI.GetBodies(VM.Models.Post.BodyType.RIGID);
            IList<double[]> bodyCMinfo = null;
            //List<string> str_curve_path = new List<string>();
            IDictionary<string, IList<Point2D>> curve = null;
            IList<Point2D> curve_point = null;
            double[] yarray = null;

            //for (i = 0; i < bodies.Count(); i++)
            foreach((BodyType, string) body in bodies)
            {
                //if (bodies[i].Item2.Contains(str_Target_body) == true)
                if (body.Item2.Contains(str_Target_body) == true)
                {
                    bodyCMinfo = postAPI.GetMarkerInfo(body.Item2 + "/CM");


                    PlotParameters parameters = new PlotParameters();
                    parameters.Target = body.Item2;

                    for (j = nStartIndexOfPath; j < nStartIndexOfPath + 3; j++)
                        parameters.Paths.Add(path[j]);

                    curve = postAPI.GetCurves(parameters);

                    if (curve == null)
                        return false;

                    lst_arry.Clear();
                    for (j = 0; j < 3; j++)
                    {
                        curve_point = curve[str_Target_body + "/" + path[j + nStartIndexOfPath]];
                        yarray = curve_point.Select(s => s.Y).ToArray();

                        k = 0;
                        foreach (double dv in yarray)
                        {
                            if (lst_arry.Count < yarray.Length)
                                lst_arry.Add(new double[3] { 0.0, 0.0, 0.0 });

                            lst_arry[k][j] = dv;

                            k++;
                        }
                    }





                    break;
                }
            }
            return true;
        }

        private bool GetChassisInfo(PostAPI.PostAPI postAPI, ref DurabilityData durability)
        {
            int j, k;
            IEnumerable<(BodyType, string)> bodies = postAPI.GetBodies(VM.Models.Post.BodyType.RIGID);
            IList<double[]> chassisCMinfo = null;
            List<string> str_curve_path = new List<string>();
            IDictionary<string, IList<Point2D>> curve = null;
            IList<Point2D> curve_point = null;
            double[] yarray = null;

            str_curve_path.Add("Velocity/X");
            str_curve_path.Add("Velocity/Y");
            str_curve_path.Add("Velocity/Z");

            str_curve_path.Add("Angular Velocity/X");
            str_curve_path.Add("Angular Velocity/Y");
            str_curve_path.Add("Angular Velocity/Z");

            str_curve_path.Add("Acceleration/X");
            str_curve_path.Add("Acceleration/Y");
            str_curve_path.Add("Acceleration/Z");

            str_curve_path.Add("Angular Acceleration/X");
            str_curve_path.Add("Angular Acceleration/Y");
            str_curve_path.Add("Angular Acceleration/Z");

            //for (i = 0; i < bodies.Count; i++)
            foreach((BodyType, string)body in bodies)
            {
                if (body.Item2.Contains("chassis") == true)
                {
                    durability.ExistChassis = true;
                    chassisCMinfo = postAPI.GetMarkerInfo(body.Item2 + "/CM");
                    foreach (double[] tmp in chassisCMinfo)
                    {
                        durability.PositionOfChassis.Add(new double[3] { tmp[0], tmp[1], tmp[2] });
                        durability.OrientationOfChassis.Add(new double[9] { tmp[3], tmp[4], tmp[5], tmp[6], tmp[7], tmp[8], tmp[9], tmp[10], tmp[11] });
                    }

                    PlotParameters parameters = new PlotParameters();
                    parameters.Target = body.Item2;

                    for (j = 0; j < str_curve_path.Count; j++)
                        parameters.Paths.Add(str_curve_path[j]);

                    curve = postAPI.GetCurves(parameters);

                    if (curve == null)
                        return false;

                    str_curve_path.Clear();
                    string bd_name = body.Item2;

                    str_curve_path.Add(bd_name + "/Velocity/X");
                    str_curve_path.Add(bd_name + "/Velocity/Y");
                    str_curve_path.Add(bd_name + "/Velocity/Z");

                    str_curve_path.Add(bd_name + "/Angular Velocity/X");
                    str_curve_path.Add(bd_name + "/Angular Velocity/Y");
                    str_curve_path.Add(bd_name + "/Angular Velocity/Z");

                    str_curve_path.Add(bd_name + "/Acceleration/X");
                    str_curve_path.Add(bd_name + "/Acceleration/Y");
                    str_curve_path.Add(bd_name + "/Acceleration/Z");

                    str_curve_path.Add(bd_name + "/Angular Acceleration/X");
                    str_curve_path.Add(bd_name + "/Angular Acceleration/Y");
                    str_curve_path.Add(bd_name + "/Angular Acceleration/Z");

                    // T-Vel
                    for (j = 0; j < 3; j++)
                    {
                        curve_point = curve[str_curve_path[j]];
                        yarray = curve_point.Select(s => s.Y).ToArray();

                        k = 0;
                        foreach (double dv in yarray)
                        {
                            if (j == 0)
                                durability.TranslationalVelocity.Add(new double[3] { 0.0, 0.0, 0.0 });

                            durability.TranslationalVelocity[k][j] = dv;

                            k++;
                        }
                    }

                    // R-Vel
                    for (j = 0; j < 3; j++)
                    {
                        curve_point = curve[str_curve_path[j + 3]];
                        yarray = curve_point.Select(s => s.Y).ToArray();

                        k = 0;
                        foreach (double dv in yarray)
                        {
                            if (j == 0)
                                durability.RotationalVelocity.Add(new double[3] { 0.0, 0.0, 0.0 });

                            durability.RotationalVelocity[k][j] = dv;

                            k++;
                        }
                    }

                    // T-Acc
                    for (j = 0; j < 3; j++)
                    {
                        curve_point = curve[str_curve_path[j + 6]];
                        yarray = curve_point.Select(s => s.Y).ToArray();

                        k = 0;
                        foreach (double dv in yarray)
                        {
                            if (j == 0)
                                durability.TranslationalAcceleration.Add(new double[3] { 0.0, 0.0, 0.0 });

                            durability.TranslationalAcceleration[k][j] = dv;

                            k++;
                        }
                    }

                    // R-Acc
                    for (j = 0; j < 3; j++)
                    {
                        curve_point = curve[str_curve_path[j + 9]];
                        yarray = curve_point.Select(s => s.Y).ToArray();

                        k = 0;
                        foreach (double dv in yarray)
                        {
                            if (j == 0)
                                durability.RotationalAcceleration.Add(new double[3] { 0.0, 0.0, 0.0 });

                            durability.RotationalAcceleration[k][j] = dv;

                            k++;
                        }
                    }



                    break;
                }
            }
            return true;
        }

        #endregion

        #region Forces

        private bool BuildForceFromMap(XmlNode _node_Item, PostAPI.PostAPI _postAPI, ref DurabilityData durability, ref string errMessage)
        {
            int i, j, nNode_force;
            double[] xarray = null;
            double[] yarray = null;
            List<double[]> lst_arry = new List<double[]>();
            IList<double[]> MKInfo = null;

            string str_type = "";
            List<string> str_curve_path = new List<string>();
            //string unit_entity = "";
            char seperator = '/';
            string result_name = "";

            PlotParameters parameters = null;
            Force force_data = null;
            EntityForForce entity = null;

            IList<Point2D> curve_point = null;
            IDictionary<string, IList<Point2D>> curve = null;


            List<Force> lst_data_force = durability.Forces;
            XmlNodeList lst_node_force = _node_Item.SelectNodes("Force");

            IEnumerable<(BodyType, string)> bodies = _postAPI.GetBodies(BodyType.RIGID);
            //IList<(BodyType, string)> ground = _postAPI.GetBodies(BodyType.GROUND);
            //var temp = _postAPI.GetConnectors(ground[0].Item2);

            nNode_force = -1;
            foreach (XmlNode force_node in lst_node_force)
            {
                nNode_force++;

                force_data = new Force();
                force_data.Name = force_node.Attributes.GetNamedItem("name").Value;

                #region Set Marker Info

                // BaseMarker
                MKInfo = _postAPI.GetMarkerInfo(force_data.Name + "/BaseMarker");
                foreach (double[] tmp in MKInfo)
                {
                    force_data.Base_Positions.Add(new double[3] { tmp[0], tmp[1], tmp[2] });
                    force_data.Base_Orientations.Add(new double[9] { tmp[3], tmp[4], tmp[5], tmp[6], tmp[7], tmp[8], tmp[9], tmp[10], tmp[11] });
                }

                // ActionMarker
                MKInfo = _postAPI.GetMarkerInfo(force_data.Name + "/ActionMarker");
                foreach (double[] tmp in MKInfo)
                {
                    force_data.Action_Positions.Add(new double[3] { tmp[0], tmp[1], tmp[2] });
                    force_data.Action_Orientations.Add(new double[9] { tmp[3], tmp[4], tmp[5], tmp[6], tmp[7], tmp[8], tmp[9], tmp[10], tmp[11] });
                }

                #endregion

                #region Find Body

                bool bFindBodies = false;
                //for (i = 0; i < bodies.Count; i++)
                foreach((BodyType, string)body in bodies)
                {
                    var connectors = _postAPI.GetConnectors(body.Item2);

                    for (j = 0; j < connectors.Count; j++)
                    {
                        if (connectors[j].Item3 == force_data.Name)
                        {
                            if (force_data.BaseBody == "" || force_data.ActionBody == "")
                            {
                                if (connectors[j].Item2 == VM.Enums.Post.ActionType.Base)
                                    force_data.BaseBody = body.Item2;
                                else
                                    force_data.ActionBody = body.Item2;

                                if (connectors[j].Item1 == ConnectorType.Tire)
                                {
                                    force_data.TypeofForce = ForceTypeofForce.Tire;
                                    bFindBodies = true;
                                    break;
                                }
                            }

                            if (force_data.BaseBody.Length > 0 && force_data.ActionBody.Length > 0)
                            {
                                bFindBodies = true;

                                if (connectors[j].Item1 == ConnectorType.Bush)
                                    force_data.TypeofForce = ForceTypeofForce.Bush;
                                else if (connectors[j].Item1 == ConnectorType.TSpringDamper)
                                    force_data.TypeofForce = ForceTypeofForce.TSpringDamper;

                                break;

                            }


                        }
                    }

                    if (bFindBodies == true)
                        break;
                }

                if (bFindBodies == false)
                {
                    string str_result = Path.GetFileName(m_strResultPath);
                    string str_error = string.Format("The target force does not exist in “{0}”. . The “{1}” result cannot be exported", str_result, force_data.Name);
                    errMessage += "Error : " + str_error + "\n";
                    //MessageBox.Show(str_error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                    //return false;
                }

                #endregion

                #region Set Original time history
                if (nNode_force == 0)
                {
                    parameters = new PlotParameters();
                    parameters.Target = force_data.ActionBody;
                    parameters.Paths.Add("Displacement/X");
                    curve = _postAPI.GetCurves(parameters);
                    if (curve == null)
                        return false;
                    curve_point = curve[force_data.ActionBody + "/Displacement/X"];
                    xarray = curve_point.Select(s => s.X).ToArray();
                    foreach (double _dtime in xarray)
                        durability.OriginalTimes.Add(_dtime);
                }

                #endregion

                #region Entities

                foreach (XmlNode entity_node in force_node.ChildNodes)
                {
                    parameters = new PlotParameters();
                    entity = new EntityForForce();
                    entity.Name = entity_node.Attributes.GetNamedItem("name").Value;
                    result_name = force_data.Name + seperator;
                    if (entity.Name.Contains("Force"))
                    {
                        entity.UnitScaleFactor[0] = durability.Scale_Force;
                        //unit_entity = durability.Unit_Force;
                        entity.Unit1 = durability.Unit_Force;

                        if (force_data.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            //entity.ResultNames.Add(result_name + entity.Name + "(" + unit_entity + ")");
                            entity.ResultNames.Add(result_name + entity.Name);

                            str_type = "Force on Action Marker";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name;
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[force_data.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[3] { 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[1] { 0.0 });
                                        entity.FixedStepValue.Add(new double[1] { 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }
                        }
                        else if (force_data.TypeofForce == ForceTypeofForce.Bush)
                        {
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "FX(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "FY(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "FZ(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "FX");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "FY");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "FZ");

                            entity.UnitScaleFactor[1] = durability.Scale_Force * durability.Scale_Length;
                            entity.Unit2 = durability.Unit_Force + durability.Unit_Length;
                            //unit_entity = durability.Unit_Force + durability.Unit_Length;

                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "TX(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "TY(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "TZ(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "TX");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "TY");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "TZ");

                            str_type = "Force on Action Marker Measured in Base Marker";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name;
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            str_type = "Torque on Action Marker Measured in Base Marker";
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[force_data.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            //entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Longitudinal_RF_Global(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Lateral_RF_Global(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Vertical_RF_Global(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Longitudinal_RF_Global");
                            entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Lateral_RF_Global");
                            entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Vertical_RF_Global");

                            entity.UnitScaleFactor[1] = durability.Scale_Force * durability.Scale_Length;
                            entity.Unit2 = durability.Unit_Force + durability.Unit_Length;
                            //unit_entity = durability.Unit_Force + durability.Unit_Length;

                            //entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Overturning_RF_Global(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Rolling resistance_RF_Global(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Aligning_RF_Global(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Overturning_RF_Global");
                            entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Rolling resistance_RF_Global");
                            entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Aligning_RF_Global");


                            if (0 < durability.OrientationOfChassis.Count)
                            {
                                // unit_entity = durability.Unit_Force;

                                //entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Longitudinal_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Lateral_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Vertical_RF_Vehicle(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Longitudinal_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Lateral_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Tire Force" + seperator + "Vertical_RF_Vehicle");

                                //unit_entity = durability.Unit_Force + durability.Unit_Length;

                                //entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Overturning_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Rolling resistance_RF_Vehicle(" + unit_entity + ")");
                                //entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Aligning_RF_Vehicle(" + unit_entity + ")");

                                entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Overturning_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Rolling resistance_RF_Vehicle");
                                entity.ResultNames.Add(result_name + "Tire Torque" + seperator + "Aligning_RF_Vehicle");
                            }

                            str_type = "Tire Force";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/Longitudinal");
                            str_curve_path.Add(str_type + "/Lateral");
                            str_curve_path.Add(str_type + "/Vertical");

                            parameters.Target = force_data.Name;
                            parameters.Paths.Add(str_type + "/Longitudinal");
                            parameters.Paths.Add(str_type + "/Lateral");
                            parameters.Paths.Add(str_type + "/Vertical");

                            str_type = "Tire Torque";
                            str_curve_path.Add(str_type + "/Overturning");
                            str_curve_path.Add(str_type + "/Rolling resistance");
                            str_curve_path.Add(str_type + "/Aligning");

                            parameters.Paths.Add(str_type + "/Overturning");
                            parameters.Paths.Add(str_type + "/Rolling resistance");
                            parameters.Paths.Add(str_type + "/Aligning");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[force_data.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (0 < durability.OrientationOfChassis.Count)
                                            entity.FixedStepValue.Add(new double[12] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        else
                                            entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }
                        }

                        force_data.Entities.Add(entity);

                    }
                    else if (entity.Name.Contains("Displacement"))
                    {
                        entity.UnitScaleFactor[0] = durability.Scale_Length;
                        entity.Unit1 = durability.Unit_Length;
                        //unit_entity = durability.Unit_Length;

                        if (force_data.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            entity.ResultNames.Add(result_name + entity.Name);

                            str_type = force_data.Name + "/BaseMarker/Displacement";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name + "/BaseMarker";
                            str_type = "Displacement";
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[1] { 0.0 });
                                        entity.FixedStepValue.Add(new double[1] { 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }


                            str_type = force_data.Name + "/ActionMarker/Displacement";
                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters = new PlotParameters();
                            parameters.Target = force_data.Name + "/ActionMarker";
                            str_type = "Displacement";
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    entity.OrinalValue[j][i + 3] = yvalue;
                                    j++;
                                }
                            }

                            force_data.Entities.Add(entity);
                        }
                        else if (force_data.TypeofForce == ForceTypeofForce.Bush)
                        {
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "X(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "Y(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "Z(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "X");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "Y");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "Z");

                            entity.UnitScaleFactor[1] = durability.Scale_Angle;
                            entity.Unit2 = durability.Unit_Angle;
                            //unit_entity = durability.Unit_Angle ;

                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "AX(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "AY(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "AZ(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "AX");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "AY");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "AZ");

                            str_type = "Translational Deformation";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name;
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            str_type = "AX AY AZ Projection Angle";
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[force_data.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }

                            force_data.Entities.Add(entity);
                        }

                        
                    }
                    else if (entity.Name.Contains("Velocity"))
                    {
                        entity.UnitScaleFactor[0] = durability.Scale_Length / durability.Scale_Time;
                        entity.Unit1 = durability.Unit_Length + "/" + durability.Unit_Time;
                        //unit_entity = durability.Unit_Length + "/" + durability.Unit_Time;

                        if (force_data.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            //entity.ResultNames.Add(result_name + entity.Name + "(" + unit_entity + ")");
                            entity.ResultNames.Add(result_name + entity.Name);

                            str_type = force_data.Name + "/BaseMarker/Velocity";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name + "/BaseMarker";
                            str_type = "Velocity";
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[1] { 0.0 });
                                        entity.FixedStepValue.Add(new double[1] { 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }


                            str_type = force_data.Name + "/ActionMarker/Velocity";
                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters = new PlotParameters();
                            parameters.Target = force_data.Name + "/ActionMarker";
                            str_type = "Velocity";
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    entity.OrinalValue[j][i + 3] = yvalue;
                                    j++;
                                }
                            }

                            force_data.Entities.Add(entity);
                        }
                        else if (force_data.TypeofForce == ForceTypeofForce.Bush)
                        {
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "VX(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "VY(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "VZ(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "VX");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "VY");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "VZ");

                            entity.UnitScaleFactor[1] = durability.Scale_Angle / durability.Scale_Time;
                            entity.Unit2 = durability.Unit_Angle + "/" + durability.Unit_Time;
                            //unit_entity = durability.Unit_Angle;

                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "WX(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "WY(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "WZ(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "WX");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "WY");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "WZ");

                            str_type = "Relative Translational Velocity";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name;
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            str_type = "Relative Angular Velocity";
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[force_data.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }

                            force_data.Entities.Add(entity);
                        }

                        
                    }
                    else
                    {
                        entity.UnitScaleFactor[0] = durability.Scale_Length / (durability.Scale_Time * durability.Scale_Time);
                        entity.Unit1 = durability.Unit_Length + "/" + durability.Unit_Time + "^2";
                        //unit_entity = durability.Unit_Length + "/" + durability.Unit_Time + "^2";

                        if (force_data.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            //entity.ResultNames.Add(result_name + entity.Name + "(" + unit_entity + ")");
                            entity.ResultNames.Add(result_name + entity.Name);

                            str_type = force_data.Name + "/BaseMarker/Acceleration";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name + "/BaseMarker";
                            str_type = "Acceleration";
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[1] { 0.0 });
                                        entity.FixedStepValue.Add(new double[1] { 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }


                            //str_type = force_data.Name + "/ActionMarker/Angular Acceleration";
                            str_type = force_data.Name + "/ActionMarker/Acceleration";
                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters = new PlotParameters();
                            parameters.Target = force_data.Name + "/ActionMarker";
                            //str_type = "Angular Acceleration";
                            str_type = "Acceleration";
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    entity.OrinalValue[j][i + 3] = yvalue;
                                    j++;
                                }
                            }

                            force_data.Entities.Add(entity);
                        }
                        else if (force_data.TypeofForce == ForceTypeofForce.Bush)
                        {
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "VX_dot(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "VY_dot(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "VZ_dot(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "VX_dot");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "VY_dot");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "VZ_dot");

                            entity.UnitScaleFactor[1] = durability.Scale_Angle / (durability.Scale_Time * durability.Scale_Time); ;
                            entity.Unit2 = durability.Unit_Angle + "/" + durability.Unit_Time + "^2";
                            //unit_entity = durability.Unit_Angle + "/" + durability.Unit_Time + "^2";

                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "WX_dot(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "WY_dot(" + unit_entity + ")");
                            //entity.ResultNames.Add(result_name + entity.Name + seperator + "WZ_dot(" + unit_entity + ")");

                            entity.ResultNames.Add(result_name + entity.Name + seperator + "WX_dot");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "WY_dot");
                            entity.ResultNames.Add(result_name + entity.Name + seperator + "WZ_dot");

                            str_type = "Relative Translational Acceleration";

                            str_curve_path.Clear();
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Target = force_data.Name;
                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            str_type = "Relative Angular Acceleration";
                            str_curve_path.Add(str_type + "/X");
                            str_curve_path.Add(str_type + "/Y");
                            str_curve_path.Add(str_type + "/Z");

                            parameters.Paths.Add(str_type + "/X");
                            parameters.Paths.Add(str_type + "/Y");
                            parameters.Paths.Add(str_type + "/Z");

                            curve = _postAPI.GetCurves(parameters);
                            if (curve == null)
                                return false;

                            for (i = 0; i < str_curve_path.Count; i++)
                            {
                                curve_point = curve[force_data.Name + "/" + str_curve_path[i]];
                                yarray = curve_point.Select(s => s.Y).ToArray();

                                j = 0;
                                foreach (double yvalue in yarray)
                                {
                                    if (i == 0)
                                    {
                                        entity.OrinalValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.TransformValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                                        entity.FixedStepValue.Add(new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

                                        if (j == 0)
                                        {
                                            xarray = curve_point.Select(s => s.X).ToArray();
                                            durability.EndTime = xarray[yarray.Length - 1];
                                        }
                                    }

                                    entity.OrinalValue[j][i] = yvalue;
                                    j++;
                                }
                            }

                            force_data.Entities.Add(entity);
                        }

                        
                    }

                }

                #endregion

                lst_data_force.Add(force_data);

            }

            return true;
        }

        private bool Translate_Data_For_Forces(PostAPI.PostAPI _postAPI, ref DurabilityData durability)
        {
            int i, j, nlength, ierror = 0;
            int nRow = 0, nColumn = 0;
            double[] Fi = new double[3] { 0.0, 0.0, 0.0 };
            double[] Ti = new double[3] { 0.0, 0.0, 0.0 };
            double[] rij = new double[3] { 0.0, 0.0, 0.0 };
            double[] rij_dot = new double[3] { 0.0, 0.0, 0.0 };
            double[] rij_ddot = new double[3] { 0.0, 0.0, 0.0 };
            double[] Aj = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] Cj = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

            

            foreach (Force force in durability.Forces)
            {
                foreach (EntityForForce entity in force.Entities)
                {
                    if (entity.Name.Contains("Force"))
                    {
                        if (force.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            nlength = force.Base_Positions.Count;
                            double value_force = 0.0;

                            for (i = 0; i < nlength; i++)
                            {
                                for (j = 0; j < 3; j++)
                                {
                                    rij[j] = force.Action_Positions[i][j] - force.Base_Positions[i][j];
                                    Fi[j] = entity.OrinalValue[i][j];
                                }

                                // rij -> uij
                                lib_math.normalization_vec(ref rij, ref ierror);
                                if (ierror == 1)
                                    return false;

                                lib_math.vectrvec(rij, Fi, ref value_force);

                                entity.TransformValue[i][0] = value_force;
                            }

                        }
                        else if (force.TypeofForce == ForceTypeofForce.Bush)
                        {
                            double[] Fi_2prime = new double[3] { 0.0, 0.0, 0.0 };
                            double[] Ti_2prime = new double[3] { 0.0, 0.0, 0.0 };

                            nlength = entity.OrinalValue.Count;

                            for (i = 0; i < nlength; i++)
                            {
                                //for (j = 0; j < 3; j++)
                                //{
                                //    Fi[j] = entity.OrinalValue[i][j];
                                //    Ti[j] = entity.OrinalValue[i][j + 3];

                                //    Aj[j] = durability.OrientationOfChassis[i][j];
                                //    Aj[j + 3] = durability.OrientationOfChassis[i][j + 3];
                                //    Aj[j + 6] = durability.OrientationOfChassis[i][j + 6];
                                //}

                                //lib_math.mattrvec(Aj, Fi, ref Fi_2prime);
                                //lib_math.mattrvec(Aj, Ti, ref Ti_2prime);

                                //for (j = 0; j < 3; j++)
                                //{
                                //    entity.TransformValue[i][j] = Fi_2prime[j];
                                //    entity.TransformValue[i][j + 3] = Ti_2prime[j];
                                //}

                                for (j = 0; j < 3; j++)
                                {
                                    entity.TransformValue[i][j] = entity.OrinalValue[i][j];
                                    entity.TransformValue[i][j + 3] = entity.OrinalValue[i][j + 3];
                                }
                            }

                        }
                        else
                        {
                            nlength = entity.OrinalValue.Count;

                            if (0 == durability.OrientationOfChassis.Count)
                            {
                                for (i = 0; i < nlength; i++)
                                {
                                    for (j = 0; j < 3; j++)
                                    {
                                        entity.TransformValue[i][j] = entity.OrinalValue[i][j];
                                        entity.TransformValue[i][j + 3] = entity.OrinalValue[i][j + 3];
                                    }
                                }
                            }
                            else
                            {

                                for (i = 0; i < 9; i++)
                                    Cj[i] = durability.OrientationOfChassis[0][i];

                                double[] Fi_prime = new double[3] { 0.0, 0.0, 0.0 };
                                double[] Ti_prime = new double[3] { 0.0, 0.0, 0.0 };

                                for (i = 0; i < nlength; i++)
                                {
                                    for (j = 0; j < 3; j++)
                                    {
                                        Fi[j] = entity.OrinalValue[i][j];
                                        Ti[j] = entity.OrinalValue[i][j + 3];

                                        Aj[j] = durability.OrientationOfChassis[i][j];
                                        Aj[j + 3] = durability.OrientationOfChassis[i][j + 3];
                                        Aj[j + 6] = durability.OrientationOfChassis[i][j + 6];
                                    }

                                    lib_math.matmattrvec(Cj, Aj, Fi, ref Fi_prime);
                                    lib_math.matmattrvec(Cj, Aj, Ti, ref Ti_prime);

                                    for (j = 0; j < 3; j++)
                                    {
                                        entity.TransformValue[i][j] = Fi_prime[j];
                                        entity.TransformValue[i][j + 3] = Ti_prime[j];
                                    }
                                }
                            }
                        }
                    }
                    else if (entity.Name.Contains("Displacement"))
                    {
                        if (force.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            nlength = force.Base_Positions.Count;
                            double value_1 = 0.0, value_2 = 0.0;

                            for (i = 0; i < nlength; i++)
                            {
                                for (j = 0; j < 3; j++)
                                {
                                    rij[j] = force.Action_Positions[i][j] - force.Base_Positions[i][j];
                                }

                                lib_math.vectrvec(rij, rij, ref value_1);
                                value_2 = Math.Sqrt(value_1);

                                entity.TransformValue[i][0] = value_2;
                            }
                        }
                        else if (force.TypeofForce == ForceTypeofForce.Bush)
                        {
                            nRow = entity.OrinalValue.Count;
                            nColumn = entity.OrinalValue[0].Length;

                            for (i = 0; i < nRow; i++)
                            {
                                for (j = 0; j < nColumn; j++)
                                    entity.TransformValue[i][j] = entity.OrinalValue[i][j];
                            }

                        }
                    }
                    else if (entity.Name.Contains("Velocity"))
                    {
                        if (force.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            nRow = entity.OrinalValue.Count;
                            double value_v = 0.0;
                            for (i = 0; i < nRow; i++)
                            {
                                for (j = 0; j < 3; j++)
                                {
                                    rij_dot[j] = entity.OrinalValue[i][j + 3] - entity.OrinalValue[i][j];
                                    rij[j] = force.Action_Positions[i][j] - force.Base_Positions[i][j];
                                }

                                // rij -> uij
                                lib_math.normalization_vec(ref rij, ref ierror);
                                if (ierror == 1)
                                    return false;

                                lib_math.vectrvec(rij_dot, rij, ref value_v);
                                //lib_math.vectrvec(rij, rij_dot, ref value_v);

                                entity.TransformValue[i][0] = value_v;
                            }
                        }
                        else if (force.TypeofForce == ForceTypeofForce.Bush)
                        {
                            
                            nColumn = entity.OrinalValue[0].Length;

                            for (i = 0; i < nRow; i++)
                            {
                                for (j = 0; j < nColumn; j++)
                                    entity.TransformValue[i][j] = entity.OrinalValue[i][j];
                            }
                        }
                    }
                    else
                    {
                        if (force.TypeofForce == ForceTypeofForce.TSpringDamper)
                        {
                            nRow = entity.OrinalValue.Count;
                            double value_a = 0.0;
                            for (i = 0; i < nRow; i++)
                            {
                                for (j = 0; j < 3; j++)
                                {
                                    rij_ddot[j] = entity.OrinalValue[i][j + 3] - entity.OrinalValue[i][j];
                                    rij[j] = force.Action_Positions[i][j] - force.Base_Positions[i][j];
                                }

                                // rij -> uij
                                lib_math.normalization_vec(ref rij, ref ierror);
                                if (ierror == 1)
                                    return false;

                                lib_math.vectrvec(rij_ddot, rij, ref value_a);
                                //lib_math.vectrvec(rij, rij_ddot, ref value_a);

                                entity.TransformValue[i][0] = value_a;
                            }
                        }
                        else if (force.TypeofForce == ForceTypeofForce.Bush)
                        {
                            nRow = entity.OrinalValue.Count;
                            nColumn = entity.OrinalValue[0].Length;
                            for (i = 0; i < nRow; i++)
                            {
                                for (j = 0; j < nColumn; j++)
                                    entity.TransformValue[i][j] = entity.OrinalValue[i][j];
                            }
                        }
                    }
                }
            }




            return true;
        }

        private bool Interpolation_For_Force(PostAPI.PostAPI _postAPI, ref DurabilityData durability, ref string errMessage)
        {
            int i, j, k;
            int nRow = 0, nColumn = 0, nDiffer = 0;
            double[] xarray = null;
            double[] yarray = null;
            double err_tol = 1.0e-10;
            double y_value = 0.0, y_max = 0.0;
            bool bSkipInterpolation = false;
            bool bChangeStep = false;

            k = durability.NumOfResult;
            k = 0;
            xarray = durability.OriginalTimes.ToArray();
            yarray = new double[xarray.Length];

            if (xarray.Length < 3)
            {
                if (xarray.Length == 1)
                    bSkipInterpolation = true;
                else if (xarray[0] == xarray[1])
                    bSkipInterpolation = true;
            }

            if (bSkipInterpolation == false)
            {
                if (false == Determine_Result_Step(ref durability, ref errMessage))
                    return false;

                foreach (Force force_data in durability.Forces)
                {
                    foreach (EntityForForce entity in force_data.Entities)
                    {
                        nRow = entity.TransformValue.Count;
                        nColumn = entity.TransformValue[0].Length;

                        if (nRow != durability.ResultStep)
                        {
                            bChangeStep = true;
                            entity.FixedStepValue.Clear();
                        }
                        else
                            bChangeStep = false;

                        if (entity.Name.Contains("Force") && force_data.TypeofForce == ForceTypeofForce.Tire)
                        {
                            // in Inertia reference frame
                            for (i = 0; i < nColumn; i++)
                            {
                                for (j = 0; j < nRow; j++)
                                {
                                    yarray[j] = entity.OrinalValue[j][i];
                                }

                                var result = _postAPI.InterpolationAkimaSpline(xarray, yarray, nRow, durability.ResultStep, xarray[0], durability.EndTime_Modify);

                                if (i == 0 && k == 0)
                                {
                                    durability.FixedTimes.Clear();
                                    for (j = 0; j < durability.ResultStep; j++)
                                        durability.FixedTimes.Add(result.Item2[j]);
                                }

                                for (j = 0; j < durability.ResultStep; j++)
                                {
                                    y_value = result.Item3[j];
                                    if (err_tol > Math.Abs(y_value))
                                        y_value = 0.0;

                                    if (j == 0)
                                        y_max = Math.Abs(y_value);
                                    else
                                    {
                                        if (Math.Abs(y_value) > y_max)
                                            y_max = Math.Abs(y_value);

                                    }

                                    if (bChangeStep == true && i == 0)
                                    {
                                        if (0 < durability.OrientationOfChassis.Count)
                                            entity.FixedStepValue.Add(new double[nColumn*2]);
                                        else
                                            entity.FixedStepValue.Add(new double[nColumn]);
                                    }

                                    entity.FixedStepValue[j][i] = y_value;
                                }

                                entity.MaxValues.Add((y_max));
                            }

                            if (0 < durability.OrientationOfChassis.Count)
                            {
                                // in Vehicle body reference frame
                                for (i = 0; i < nColumn; i++)
                                {
                                    for (j = 0; j < nRow; j++)
                                    {
                                        yarray[j] = entity.TransformValue[j][i];
                                    }

                                    var result = _postAPI.InterpolationAkimaSpline(xarray, yarray, nRow, durability.ResultStep, xarray[0], durability.EndTime_Modify);


                                    for (j = 0; j < durability.ResultStep; j++)
                                    {
                                        y_value = result.Item3[j];
                                        if (err_tol > Math.Abs(y_value))
                                            y_value = 0.0;

                                        if (j == 0)
                                            y_max = Math.Abs(y_value);
                                        else
                                        {
                                            if (Math.Abs(y_value) > y_max)
                                                y_max = Math.Abs(y_value);

                                        }

                                        entity.FixedStepValue[j][i + 6] = y_value;
                                    }

                                    entity.MaxValues.Add((y_max));
                                }

                            }

                        }
                        else
                        {
                            for (i = 0; i < nColumn; i++)
                            {
                                for (j = 0; j < nRow; j++)
                                {
                                    //yarray[j] = entity.OrinalValue[j][i];
                                    yarray[j] = entity.TransformValue[j][i];
                                }

                                var result = _postAPI.InterpolationAkimaSpline(xarray, yarray, nRow, durability.ResultStep, xarray[0], durability.EndTime_Modify);

                                if (i == 0 && k == 0)
                                {
                                    durability.FixedTimes.Clear();
                                    for (j = 0; j < durability.ResultStep; j++)
                                        durability.FixedTimes.Add(result.Item2[j]);
                                }

                                for (j = 0; j < durability.ResultStep; j++)
                                {
                                    y_value = result.Item3[j];
                                    if (err_tol > Math.Abs(y_value))
                                        y_value = 0.0;

                                    if (j == 0)
                                        y_max = Math.Abs(y_value);
                                    else
                                    {
                                        if (Math.Abs(y_value) > y_max)
                                            y_max = Math.Abs(y_value);

                                    }

                                    if (bChangeStep == true && i == 0)
                                    {
                                        entity.FixedStepValue.Add(new double[nColumn]);
                                    }

                                    entity.FixedStepValue[j][i] = y_value;
                                }

                                entity.MaxValues.Add((y_max));
                            }
                        }

                        nDiffer = entity.FixedStepValue.Count - durability.ResultStep;
                        //nDiffer = nRow - durability.ResultStep;
                        if (nDiffer > 0)
                        {
                            for (i = 0; i < nDiffer; i++)
                            {
                                j = entity.FixedStepValue.Count - 1;

                                entity.FixedStepValue.RemoveAt(j);
                            }
                        }

                        k++;
                    }
                }

            }
            else
            {
                durability.FixedTimes.Clear();
                for (i = 0; i < xarray.Length; i++)
                    durability.FixedTimes.Add(xarray[i]);

                foreach (Force force_data in durability.Forces)
                {
                    foreach (EntityForForce entity in force_data.Entities)
                    {
                        nRow = entity.TransformValue.Count;
                        nColumn = entity.TransformValue[0].Length;
                        if (entity.Name.Contains("Force") && force_data.TypeofForce == ForceTypeofForce.Tire)
                        {
                            // in Inertia reference frame
                            for (i = 0; i < nColumn; i++)
                            {
                                for (j = 0; j < nRow; j++)
                                {
                                    y_value = entity.OrinalValue[j][i];
                                    if (err_tol > Math.Abs(y_value))
                                        y_value = 0.0;

                                    if (j == 0)
                                        y_max = Math.Abs(y_value);
                                    else
                                    {
                                        if (Math.Abs(y_value) > y_max)
                                            y_max = Math.Abs(y_value);

                                    }

                                    entity.FixedStepValue[j][i] = y_value;
                                }
                                entity.MaxValues.Add((y_max));

                            }

                            if (0 < durability.OrientationOfChassis.Count)
                            {
                                // in Vehicle body reference frame
                                for (i = 0; i < nColumn; i++)
                                {
                                    for (j = 0; j < nRow; j++)
                                    {
                                        y_value = entity.TransformValue[j][i];
                                        if (err_tol > Math.Abs(y_value))
                                            y_value = 0.0;

                                        if (j == 0)
                                            y_max = Math.Abs(y_value);
                                        else
                                        {
                                            if (Math.Abs(y_value) > y_max)
                                                y_max = Math.Abs(y_value);

                                        }

                                        entity.FixedStepValue[j][i + 6] = y_value;
                                    }

                                    entity.MaxValues.Add((y_max));
                                }

                            }

                        }
                        else
                        {
                            for (i = 0; i < nColumn; i++)
                            {
                                for (j = 0; j < nRow; j++)
                                {
                                    y_value = entity.TransformValue[j][i];
                                    if (err_tol > Math.Abs(y_value))
                                        y_value = 0.0;

                                    if (j == 0)
                                        y_max = Math.Abs(y_value);
                                    else
                                    {
                                        if (Math.Abs(y_value) > y_max)
                                            y_max = Math.Abs(y_value);

                                    }

                                    entity.FixedStepValue[j][i] = y_value;
                                }

                                entity.MaxValues.Add((y_max));
                            }
                        }

                        k++;
                    }
                }
            }

            return true;
        }

        #endregion

        #region FEBodies

        private bool BuildFEBodyFromMap(XmlNode _node_Item, PostAPI.PostAPI _postAPI, ref DurabilityData durability, ref string errMessage)
        {
            int i, j, k;
            int nFindCount, nBody, nNumofMode, nNumofResultStep = 1;
            string str_bd_name, str_mcf_format;
            MCFFormat mCFFormat;

            IEnumerable<(BodyType, string)> Mbodies = _postAPI.GetBodies(VM.Models.Post.BodyType.MODAL);
            IEnumerable<(BodyType, string)> EF_Mbodies = _postAPI.GetBodies(VM.Models.Post.BodyType.EF_MODAL);

            XmlNodeList lst_Node = _node_Item.SelectNodes("Body");

            if (null == _node_Item.Attributes.GetNamedItem("mcf_format"))
            {
                mCFFormat = MCFFormat.Wrapped;
            }
            else
            {
                str_mcf_format = _node_Item.Attributes.GetNamedItem("mcf_format").Value;

                if ("0" == str_mcf_format)
                    mCFFormat = MCFFormat.Wrapped;
                else
                    mCFFormat = MCFFormat.Unwrapped;
            }

            nBody = lst_Node.Count;
            nFindCount = 0;
            foreach (XmlNode n in lst_Node)
            {
                str_bd_name = n.Attributes.GetNamedItem("name").Value;

                //for (i = 0; i < Mbodies.Count; i++)
                foreach((BodyType, string)mbody in Mbodies)
                {
                    if (mbody.Item2.Contains(str_bd_name) == true)
                    {
                        FEBody body = new FEBody();
                        body.Name = str_bd_name;
                        body.MCF_Write_Format = mCFFormat;
                        durability.FEBodies.Add(body);
                        nFindCount++;
                    }
                }

                if (nFindCount < nBody)
                {
                    //for (i = 0; i < EF_Mbodies.Count; i++)
                    foreach ((BodyType, string) efbody in EF_Mbodies)
                    {
                        if (efbody.Item2.Contains(str_bd_name) == true)
                        {
                            FEBody body = new FEBody();
                            body.Name = str_bd_name;
                            body.MCF_Write_Format = mCFFormat;
                            durability.FEBodies.Add(body);
                            nFindCount++;
                        }
                    }
                }
            }

            if (nFindCount == 0)
            {
                errMessage += "Error : There are not FE body in Motion result. Please confirm the name of FE body.\n";
                //MessageBox.Show("There are not FE body in Motion result. Please confirm the name of FE body", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            nBody = durability.FEBodies.Count;

            // Get Version and Num. of Mode each Modal body.
            for (i = 0; i < nBody; i++)
            {
                if (i == 0)
                    durability.Version = _postAPI.Version;

                str_bd_name = durability.FEBodies[i].Name;
                durability.FEBodies[i].NumofMode = _postAPI.GetModalModeCount(str_bd_name);
            }

            PlotParameters parameters = null;
            IDictionary<string, IList<Point2D>> curve = null;
            List<string> str_curve_path = new List<string>();
            IList<Point2D> curve_point = null;
            double[] xarray = null;
            double[] yarray = null;


            // Get original time and displacement each mode in modal body
            for (i = 0; i < nBody; i++)
            {
                nNumofMode = durability.FEBodies[i].NumofMode;
                str_bd_name = durability.FEBodies[i].Name;
                str_curve_path.Clear();

                parameters = new PlotParameters();
                parameters.Target = str_bd_name;

                for (j = 0; j < nNumofMode; j++)
                    parameters.Paths.Add("Mode_" + (j + 7).ToString() + "/Displacement");

                curve = _postAPI.GetCurves(parameters);

                if (curve == null)
                    return false;

                str_curve_path.Clear();
                for (j = 0; j < nNumofMode; j++)
                    str_curve_path.Add(str_bd_name + "/Mode_" + (j + 7).ToString() + "/Displacement");

                for (j = 0; j < nNumofMode; j++)
                {
                    curve_point = curve[str_curve_path[j]];
                    if (i == 0 && j == 0)
                    {
                        xarray = curve_point.Select(s => s.X).ToArray();
                        nNumofResultStep = xarray.Length;

                        for (k = 0; k < nNumofResultStep; k++)
                            durability.OriginalTimes.Add(xarray[k]);

                        durability.EndTime = xarray[nNumofResultStep - 1];
                    }

                    yarray = curve_point.Select(s => s.Y).ToArray();

                    durability.FEBodies[i].OriginalTime_Modal_Coordinates.Add(new double[nNumofResultStep]);
                    for (k = 0; k < nNumofResultStep; k++)
                        durability.FEBodies[i].OriginalTime_Modal_Coordinates[j][k] = yarray[k];

                }

            }


            durability.Precision = "E6";

            return true;
        }

        private bool Interpolation_For_FEBody(PostAPI.PostAPI _postAPI, ref DurabilityData durability, ref string errMessage)
        {
            int i, j, k, nBody, nNumofMode, nSize_list;
            double[] xarray;
            double[] yarray;
            double y_value = 0.0, y_max = 0.0;
            double err_tol = 1.0e-10;
            //bool bChangeStep;

            if (false == Determine_Result_Step(ref durability, ref errMessage))
                return false;

            nBody = durability.FEBodies.Count;

            xarray = durability.OriginalTimes.ToArray();
            yarray = new double[durability.ResultStep];
            nSize_list = xarray.Length;

            //if (nSize_list != durability.ResultStep)
            //{
            //    bChangeStep = true;
            //}
            //else bChangeStep = false;

            for (i = 0; i < nBody; i++)
            {
                nNumofMode = durability.FEBodies[i].OriginalTime_Modal_Coordinates.Count;

                for (j = 0; j < 6; j++)
                    durability.FEBodies[i].MaxValues.Add(0.0);

                for (j = 0; j < nNumofMode; j++)
                {
                    yarray = durability.FEBodies[i].OriginalTime_Modal_Coordinates[j];

                    var result = _postAPI.InterpolationAkimaSpline(xarray, yarray, nSize_list, durability.ResultStep, xarray[0], durability.EndTime_Modify);

                    if (i == 0 && j == 0)
                    {
                        for (k = 0; k < durability.ResultStep; k++)
                            durability.FixedTimes.Add(result.Item2[k]);
                    }

                    durability.FEBodies[i].FixedTime_Modal_Coordinates.Add(result.Item3);

                    //yarray = durability.FEBodies[i].FixedTime_Modal_Coordinates[j];
                    for (k = 0; k < durability.ResultStep; k++)
                    {
                        y_value = result.Item3[k];
                        if (err_tol > Math.Abs(y_value))
                            y_value = 0.0;

                        if (k == 0)
                            y_max = Math.Abs(y_value);
                        else
                        {
                            if (Math.Abs(y_value) > y_max)
                                y_max = Math.Abs(y_value);

                        }
                    }
                    durability.FEBodies[i].MaxValues.Add(y_max);
                }

            }

            return true;
        }

        #endregion

        #region Write

        public bool WriteMap(string path, XmlDocument dom)
        {
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Indent = true;

            //XmlWriter writer = XmlWriter.Create(path, settings);
            //dom.Save(writer);


            XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            dom.WriteContentTo(writer);
            writer.Flush();
            writer.Close();

            return true;
        }


        public bool WriteResultToFile(FileFormat fileFormat, ResultValueType resulttype, string path, DurabilityData durability, ref string errMessage)
        {
            if (fileFormat == FileFormat.CSV)
            {
                if (false == WriteToCSV(resulttype, path, durability, ref errMessage))
                    return false;
            }
            else if (fileFormat == FileFormat.RPC)
            {
                if (false == WriteToRPC(ResultValueType.FixedStep, path, durability, ref errMessage))
                    return false;
            }
            else if (fileFormat == FileFormat.MCF)
            {
                if (false == WriteToMCF(resulttype, path, durability, ref errMessage))
                    return false;
            }

            return true;
        }

        private bool WriteToCSV(ResultValueType resulttype, string path, DurabilityData durability, ref string errMessage)
        {
            Category category = durability.Type;
            StringBuilder sb = new StringBuilder();
            //String[] arr_str = null;
            string str_Header = "time(" + durability.Unit_Time + ")";
            string seperator = " , ";
            string str_precision = durability.Precision;
            int i, j;
            //double dScalefactor = 0.0;
            double dScaleTime = 0.0;

            if (Path.GetFileNameWithoutExtension(path).Length >= 260)
            {
                errMessage += "Error : The number of characters(path and file name) is required less than 260.\n";
                errMessage += "File Path :" + Path.GetFileNameWithoutExtension(path) + "\n";
                return false;
            }

            int nRowCount = durability.OriginalTimes.Count;
            int nColumnCount = 1;
            #region Bodies
            if (category == Category.Bodies)
            {
                Body body = durability.Body;

                if (resulttype == ResultValueType.Original)
                {
                    foreach (EntityForBody entity in body.Entities)
                    {
                        foreach (string str in entity.ResultNames)
                        {
                            str_Header = str_Header + seperator + str;
                            nColumnCount++;
                        }
                    }
                    sb.AppendLine(str_Header);

                    for (i = 0; i < nRowCount; i++)
                    {
                        str_Header = "";
                        str_Header = durability.OriginalTimes[i].ToString("F6");
                        foreach (EntityForBody entity in body.Entities)
                        {
                            for (j = 0; j < entity.OrinalValue[i].Length; j++)
                            {
                                str_Header = str_Header + seperator + entity.OrinalValue[i][j].ToString(str_precision);
                            }
                        }
                        sb.AppendLine(str_Header);
                    }

                }
                else if (resulttype == ResultValueType.Transform)
                {
                    foreach (EntityForBody entity in body.Entities)
                    {
                        foreach (string str in entity.ResultNames)
                        {
                            str_Header = str_Header + seperator + str;
                            nColumnCount++;
                        }
                    }
                    sb.AppendLine(str_Header);

                    for (i = 0; i < nRowCount; i++)
                    {
                        str_Header = "";
                        str_Header = durability.OriginalTimes[i].ToString("F6");
                        foreach (EntityForBody entity in body.Entities)
                        {
                            for (j = 0; j < entity.TransformValue[i].Length; j++)
                            {
                                str_Header = str_Header + seperator + entity.TransformValue[i][j].ToString(str_precision);
                            }
                        }
                        sb.AppendLine(str_Header);
                    }
                }
                else
                {
                    dScaleTime = durability.Scale_Time;
                    nRowCount = durability.FixedTimes.Count;

                    foreach (EntityForBody entity in body.Entities)
                    {
                        foreach (string str in entity.ResultNames)
                        {
                            str_Header = str_Header + seperator + str;
                            nColumnCount++;
                        }
                    }
                    sb.AppendLine(str_Header);

                    for (i = 0; i < nRowCount; i++)
                    {
                        str_Header = "";
                        str_Header = (durability.FixedTimes[i] * dScaleTime).ToString("F6");
                        foreach (EntityForBody entity in body.Entities)
                        {
                            for (j = 0; j < entity.FixedStepValue[i].Length; j++)
                            {
                                if (j < 3)
                                    str_Header = str_Header + seperator + (entity.FixedStepValue[i][j] * entity.UnitScaleFactor[0]).ToString(str_precision);
                                else
                                    str_Header = str_Header + seperator + (entity.FixedStepValue[i][j] * entity.UnitScaleFactor[1]).ToString(str_precision);
                            }
                        }
                        sb.AppendLine(str_Header);
                    }
                }


            }
            #endregion
            else if (category == Category.Forces)
            {
                foreach (Force force_data in durability.Forces)
                {
                    foreach (EntityForForce entity in force_data.Entities)
                    {
                        if (resulttype != ResultValueType.FixedStep && force_data.TypeofForce == ForceTypeofForce.Tire)
                        {
                            if (resulttype == ResultValueType.Original)
                            {
                                for (i = 0; i < 6; i++)
                                    str_Header = str_Header + seperator + entity.ResultNames[i];
                            }
                            else
                            {
                                for (i = 6; i < 12; i++)
                                    str_Header = str_Header + seperator + entity.ResultNames[i];
                            }
                        }
                        else
                        {
                            foreach (string str in entity.ResultNames)
                            {
                                str_Header = str_Header + seperator + str;
                            }
                        }
                    }
                }
                sb.AppendLine(str_Header);

                if (resulttype == ResultValueType.FixedStep)
                {
                    dScaleTime = durability.Scale_Time;
                    nRowCount = durability.FixedTimes.Count;

                    for (i = 0; i < nRowCount; i++)
                    {
                        str_Header = "";
                        str_Header = (durability.FixedTimes[i] * dScaleTime).ToString("F6");
                        foreach (Force force_data in durability.Forces)
                        {
                            foreach (EntityForForce entity in force_data.Entities)
                            {
                                for (j = 0; j < entity.FixedStepValue[i].Length; j++)
                                {
                                    if (j < 3 || (5 < j && j < 9))
                                        str_Header = str_Header + seperator + (entity.FixedStepValue[i][j] * entity.UnitScaleFactor[0]).ToString(str_precision);
                                    else
                                        str_Header = str_Header + seperator + (entity.FixedStepValue[i][j] * entity.UnitScaleFactor[1]).ToString(str_precision);
                                }
                            }

                        }
                        sb.AppendLine(str_Header);
                    }

                }
                else
                {
                    for (i = 0; i < nRowCount; i++)
                    {
                        str_Header = "";
                        str_Header = durability.OriginalTimes[i].ToString("F6");
                        foreach (Force force_data in durability.Forces)
                        {
                            foreach (EntityForForce entity in force_data.Entities)
                            {
                                if (resulttype == ResultValueType.Original)
                                {
                                    for (j = 0; j < entity.OrinalValue[i].Length; j++)
                                    {
                                        str_Header = str_Header + seperator + entity.OrinalValue[i][j].ToString(str_precision);
                                    }
                                }
                                else
                                {
                                    for (j = 0; j < entity.TransformValue[i].Length; j++)
                                    {
                                        str_Header = str_Header + seperator + entity.TransformValue[i][j].ToString(str_precision);
                                    }
                                }
                            }

                        }
                        sb.AppendLine(str_Header);
                    }
                }


            }
            else if (category == Category.FEBodies)
            {

            }
            else
            {

            }

            File.WriteAllText(path, sb.ToString());

            return true;
        }

        private bool WriteToMCF(ResultValueType resulttype, string path, DurabilityData durability, ref string errMessage)
        {
            int i, j, k, nBody, nNumofMode, nlog10, nNumofResult;
            int nLineChanger;
            Category category = durability.Type;
            StringBuilder sb;
            MCFFormat mCFFormat;
            string str_precision = durability.Precision;
            string str_seperator = " ", str_seperator1 = "  ", str_seperator2 = "      ";
            string str_seperator15 = "               " ,str_CRLF="\r\n";
            string[] ar_space = new string[6];
            nBody = durability.FEBodies.Count;
            double[] xarray;

            ar_space[0] = "      ";
            ar_space[1] = "     ";
            ar_space[2] = "    ";
            ar_space[3] = "   ";
            ar_space[4] = "  ";
            ar_space[5] = " ";

            string str_temp, str_dir, str_filename, str_FEbodyname;
            string[] ar_str_tmp;
            double dvalue;
            nLineChanger = 50;
            if (category == Category.FEBodies)
            {
                for (i = 0; i < nBody; i++)
                {
                    sb = new StringBuilder();
                    // Write Headers
                    sb.AppendLine("Modal Coordinates File - Transient Solution");
                    sb.AppendLine("ANSYSMotion " + durability.Version);

                    DateTime utcNow = DateTime.UtcNow;
                    sb.AppendLine(utcNow.Month.ToString("D2") + "/" + utcNow.Day.ToString("D2") + "/" + utcNow.Year.ToString("D4") + "      " + utcNow.Hour.ToString("D2") + ":" + utcNow.Minute.ToString("D2") + ":" + utcNow.Second.ToString("D2"));

                    sb.AppendLine("Title: AnsysMotion_modal_superposition--Transient (C5)");
                    sb.AppendLine("Number of Modes: " + (durability.FEBodies[i].NumofMode + 6));

                    nNumofMode = durability.FEBodies[i].OriginalTime_Modal_Coordinates.Count + 6;
                    mCFFormat = durability.FEBodies[i].MCF_Write_Format;

                    str_temp = str_seperator1 + "Mode:        ";

                    if (mCFFormat == MCFFormat.Wrapped)
                    {
                        for (j = 0; j < nNumofMode; j++)
                        {
                            nlog10 = (int)Math.Truncate(Math.Log10(j + 1));
                            if (j == (nNumofMode - 1))
                                str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString();
                            else
                            {
                                if ((j + 1) == nLineChanger)
                                {
                                    str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString() + str_seperator2 + str_CRLF + str_seperator15;
                                    nLineChanger = nLineChanger + 50;
                                }
                                else
                                    str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString() + str_seperator2;
                            }
                        }
                    }
                    else
                    {
                        for (j = 0; j < nNumofMode; j++)
                        {
                            nlog10 = (int)Math.Truncate(Math.Log10(j + 1));
                            if (j == (nNumofMode - 1))
                                str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString();
                            else
                                 str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString() + str_seperator2;
                        }
                    }
                    sb.AppendLine(str_temp);

                    str_temp = str_seperator1 + "Frequency:   ";
                    nLineChanger = 50;
                    if (mCFFormat == MCFFormat.Wrapped)
                    {
                        for (j = 0; j < nNumofMode; j++)
                        {
                            nlog10 = (int)Math.Truncate(Math.Log10(j + 1));
                            if (j == (nNumofMode - 1))
                                str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString();
                            else
                            {
                                if ((j + 1) == nLineChanger)
                                {
                                    str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString() + str_seperator2 + str_CRLF + str_seperator15;
                                    nLineChanger = nLineChanger + 50;
                                }
                                else
                                    str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString() + str_seperator2;
                            }
                        }
                    }
                    else
                    {
                        for (j = 0; j < nNumofMode; j++)
                        {
                            nlog10 = (int)Math.Truncate(Math.Log10(j + 1));
                            if (j == (nNumofMode - 1))
                                str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString();
                            else
                                str_temp = str_temp + str_seperator1 + ar_space[nlog10] + (j + 1).ToString() + str_seperator2;
                        }
                    }
                    sb.AppendLine(str_temp);

                    str_temp = str_seperator1 + "    Time     " + str_seperator1 + "Coordinates...";
                    sb.AppendLine(str_temp);

                    // Write modal coordinate
                    if (resulttype == ResultValueType.Original)
                    {
                        xarray = durability.OriginalTimes.ToArray();
                        nNumofResult = xarray.Length;

                        if (mCFFormat == MCFFormat.Wrapped)
                        {
                            for (j = 0; j < nNumofResult; j++)
                            {
                                nLineChanger = 50;
                                str_temp = str_seperator1 + xarray[j].ToString(str_precision);
                                for (k = 0; k < nNumofMode; k++)
                                {
                                    if (k < 6)
                                        dvalue = 0.0;
                                    else
                                        dvalue = durability.FEBodies[i].OriginalTime_Modal_Coordinates[k - 6][j] * durability.Scale_Length;

                                    if (dvalue >= 0.0)
                                    {
                                        if ((k + 1) == nLineChanger)
                                        {
                                            if (nLineChanger == nNumofMode)
                                                str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision);
                                            else
                                                str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision) + str_CRLF + str_seperator15;

                                            nLineChanger = nLineChanger + 50;
                                        }
                                        else
                                            str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision);
                                    }
                                    else
                                    {
                                        if ((k + 1) == nLineChanger)
                                        {
                                            if (nLineChanger == nNumofMode)
                                                str_temp = str_temp + str_seperator + dvalue.ToString(str_precision);
                                            else
                                                str_temp = str_temp + str_seperator + dvalue.ToString(str_precision) + str_CRLF + str_seperator15;

                                            nLineChanger = nLineChanger + 50;
                                        }
                                        else
                                            str_temp = str_temp + str_seperator + dvalue.ToString(str_precision);
                                    }
                                }
                                sb.AppendLine(str_temp);
                            }
                        }
                        else
                        {
                            for (j = 0; j < nNumofResult; j++)
                            {
                                str_temp = str_seperator1 + xarray[j].ToString(str_precision);
                                for (k = 0; k < nNumofMode; k++)
                                {
                                    if (k < 6)
                                        dvalue = 0.0;
                                    else
                                        dvalue = durability.FEBodies[i].OriginalTime_Modal_Coordinates[k - 6][j] * durability.Scale_Length; 

                                    if (dvalue >= 0.0)
                                    {
                                        str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision);
                                    }
                                    else
                                    {
                                        str_temp = str_temp + str_seperator + dvalue.ToString(str_precision);
                                    }
                                }
                                sb.AppendLine(str_temp);
                            }
                        }
                    }
                    else if (resulttype == ResultValueType.FixedStep)
                    {
                        xarray = durability.FixedTimes.ToArray();
                        nNumofResult = xarray.Length;

                        if (mCFFormat == MCFFormat.Wrapped)
                        {
                            for (j = 0; j < nNumofResult; j++)
                            {
                                nLineChanger = 50;
                                str_temp = str_seperator1 + xarray[j].ToString(str_precision);
                                for (k = 0; k < nNumofMode; k++)
                                {
                                    if (k < 6)
                                        dvalue = 0.0;
                                    else
                                        dvalue = durability.FEBodies[i].FixedTime_Modal_Coordinates[k - 6][j] * durability.Scale_Length;

                                    if (dvalue >= 0.0)
                                    {
                                        if ((k + 1) == nLineChanger)
                                        {
                                            if (nLineChanger == nNumofMode)
                                                str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision);
                                            else
                                                str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision) + str_CRLF + str_seperator15;

                                            nLineChanger = nLineChanger + 50;
                                        }
                                        else
                                            str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision);
                                    }
                                    else
                                    {
                                        if ((k + 1) == nLineChanger)
                                        {
                                            if (nLineChanger == nNumofMode)
                                                str_temp = str_temp + str_seperator + dvalue.ToString(str_precision);
                                            else
                                                str_temp = str_temp + str_seperator + dvalue.ToString(str_precision) + str_CRLF + str_seperator15;

                                            nLineChanger = nLineChanger + 50;
                                        }
                                        else
                                            str_temp = str_temp + str_seperator + dvalue.ToString(str_precision);
                                    }
                                }

                                sb.AppendLine(str_temp);
                            }
                        }
                        else
                        {
                            for (j = 0; j < nNumofResult; j++)
                            {
                                nLineChanger = 50;
                                str_temp = str_seperator1 + xarray[j].ToString(str_precision);
                                for (k = 0; k < nNumofMode; k++)
                                {
                                    if (k < 6)
                                        dvalue = 0.0;
                                    else
                                        dvalue = durability.FEBodies[i].FixedTime_Modal_Coordinates[k - 6][j] * durability.Scale_Length;

                                    if (dvalue >= 0.0)
                                    {
                                        str_temp = str_temp + str_seperator1 + dvalue.ToString(str_precision);
                                    }
                                    else
                                    {
                                        str_temp = str_temp + str_seperator + dvalue.ToString(str_precision);
                                    }
                                }

                                sb.AppendLine(str_temp);
                            }
                        }
                    }

                    str_filename = Path.GetFileNameWithoutExtension(path);
                    str_FEbodyname = durability.FEBodies[i].Name;
                    if(str_FEbodyname.Contains("/"))
                    {
                        ar_str_tmp = str_FEbodyname.Split(new char[] { '/' });
                        str_FEbodyname = "";
                        for (j = 0; j < ar_str_tmp.Length; j++)
                        {
                            if (j == 0)
                                str_FEbodyname = ar_str_tmp[j];
                            else
                                str_FEbodyname = str_FEbodyname + "_" + ar_str_tmp[j];
                        }
                    }

                    str_filename = str_filename +"_"+ str_FEbodyname + ".mcf";
                    str_dir = Path.GetDirectoryName(path);

                    str_temp = Path.Combine(str_dir, str_filename);

                    if(Path.GetFileNameWithoutExtension(str_temp).Length >= 260)
                    {
                        errMessage += "Error : The length of the file path is too long. Its length must be less than 260.\n";
                        errMessage += "File Path :" + Path.GetFileNameWithoutExtension(str_temp) + "\n";
                        return false;
                    }

                    File.WriteAllText(str_temp, sb.ToString());
                }
            }
            else
            {
                errMessage += "Error : There are not supported type. Please change type\n";
                //MessageBox.Show("There are not supported type. Please change type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }



            return true;
        }


        private bool WriteToRPC(ResultValueType resulttype, string path, DurabilityData durability, ref string errMessage)
        {
            int i, j, k;
            int nRemain, nRow, nColumn;
            int pts_total, pts_per_frame, frame;

            string str_temp, str_dir, str_filename, str_FEbodyname;
            string[] ar_str_tmp;
            Int16 short_zero = 0;

            if (durability.Type != Category.FEBodies)
            {
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);

                List<string> lst_key = new List<string>();
                List<string> lst_value = new List<string>();
                int nCount_Add_Header = 0;
                Int16 full_scale = 32752;

                if (false == Get_RPC_Header(durability, ref lst_key, ref lst_value, ref nCount_Add_Header, ref errMessage))
                    return false;

                pts_per_frame = Convert.ToInt32(lst_value[12]);
                frame = Convert.ToInt32(lst_value[14]);
                pts_total = pts_per_frame * frame;

                List<double[]> lst_data = new List<double[]>();
                List<double> lst_max = new List<double>();

                if (false == Get_RPC_Data(durability, ref lst_data, ref lst_max))
                    return false;

                List<Int16[]> lst_data_New = new List<Int16[]>();


                if (false == COnvert_RPC_Data_To_INT_FULL_SCALE(lst_data, lst_max, full_scale, ref lst_data_New))
                    return false;

                // Write Header block
                nRow = lst_key.Count;
                for (i = 0; i < nRow; i++)
                {
                    nColumn = lst_key[i].Length;
                    for (j = nColumn; j < 32; j++)
                        lst_key[i] = lst_key[i] + "\0";

                    nColumn = lst_key[i].Length;
                    for (j = 0; j < nColumn; j++)
                        bw.Write(lst_key[i][j]);

                    //nRemain = 32 - nColumn;
                    //for (j = 0; j < nRemain; j++)
                    //    bw.Write("\0");

                    nColumn = lst_value[i].Length;
                    for (j = nColumn; j < 96; j++)
                        lst_value[i] = lst_value[i] + "\0";

                    nColumn = lst_value[i].Length;
                    for (j = 0; j < nColumn; j++)
                        bw.Write(lst_value[i][j]);

                    //nRemain = 96 - nColumn;
                    //for (j = 0; j < nRemain; j++)
                    //    bw.Write("\0");
                }

                string str_null = "";
                for (i = 0; i < 128; i++)
                    str_null = str_null + "\0";

                for (i = 0; i < nCount_Add_Header; i++)
                {
                    for (j = 0; j < 128; j++)
                        bw.Write(str_null[j]);
                }

                // Write data
                nRow = lst_data_New.Count;

                for (i = 0; i < nRow; i++)
                {
                    nColumn = lst_data_New[i].Length;
                    nRemain = pts_total - nColumn;

                    for (j = 0; j < nColumn; j++)
                    {
                        bw.Write(lst_data_New[i][j]);
                    }

                    for (j = 0; j < nRemain; j++)
                    {
                        bw.Write(short_zero);
                        //bw.Write("\0");
                    }
                }

                bw.Close();
                fs.Close();
            }
            else
            {
                for (k = 0; k < durability.FEBodies.Count; k++)
                {
                    str_filename = Path.GetFileNameWithoutExtension(path);
                    str_FEbodyname = durability.FEBodies[k].Name;
                    if (str_FEbodyname.Contains("/"))
                    {
                        ar_str_tmp = str_FEbodyname.Split(new char[] { '/' });
                        str_FEbodyname = "";
                        for (j = 0; j < ar_str_tmp.Length; j++)
                        {
                            if (j == 0)
                                str_FEbodyname = ar_str_tmp[j];
                            else
                                str_FEbodyname = str_FEbodyname + "_" + ar_str_tmp[j];
                        }
                    }

                    str_filename = str_filename + "_" + str_FEbodyname + ".rsp";
                    str_dir = Path.GetDirectoryName(path);

                    str_temp = Path.Combine(str_dir, str_filename);

                    if (Path.GetFileNameWithoutExtension(str_temp).Length >= 260)
                    {
                        errMessage += "Error : The number of characters(path and file name) is required less than 260.\n";
                        errMessage += "File Path :" + Path.GetFileNameWithoutExtension(str_temp) + "\n";
                        return false;
                    }

                    FileStream fs = new FileStream(str_temp, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);

                    List<string> lst_key = new List<string>();
                    List<string> lst_value = new List<string>();
                    int nCount_Add_Header = 0;
                    Int16 full_scale = 32752;

                    if (false == Get_RPC_Header_FE(durability, k, ref lst_key, ref lst_value, ref nCount_Add_Header, ref errMessage))
                        return false;

                    pts_per_frame = Convert.ToInt32(lst_value[12]);
                    frame = Convert.ToInt32(lst_value[14]);
                    pts_total = pts_per_frame * frame;

                    List<double[]> lst_data = new List<double[]>();
                    List<double> lst_max = new List<double>();

                    if (false == Get_RPC_Data_FE(durability, k, ref lst_data, ref lst_max))
                        return false;

                    List<Int16[]> lst_data_New = new List<Int16[]>();


                    if (false == COnvert_RPC_Data_To_INT_FULL_SCALE(lst_data, lst_max, full_scale, ref lst_data_New))
                        return false;

                    // Write Header block
                    nRow = lst_key.Count;
                    for (i = 0; i < nRow; i++)
                    {
                        nColumn = lst_key[i].Length;
                        for (j = nColumn; j < 32; j++)
                            lst_key[i] = lst_key[i] + "\0";

                        nColumn = lst_key[i].Length;
                        for (j = 0; j < nColumn; j++)
                            bw.Write(lst_key[i][j]);

                        //nRemain = 32 - nColumn;
                        //for (j = 0; j < nRemain; j++)
                        //    bw.Write("\0");

                        nColumn = lst_value[i].Length;
                        for (j = nColumn; j < 96; j++)
                            lst_value[i] = lst_value[i] + "\0";

                        nColumn = lst_value[i].Length;
                        for (j = 0; j < nColumn; j++)
                            bw.Write(lst_value[i][j]);

                        //nRemain = 96 - nColumn;
                        //for (j = 0; j < nRemain; j++)
                        //    bw.Write("\0");
                    }

                    string str_null = "";
                    for (i = 0; i < 128; i++)
                        str_null = str_null + "\0";

                    for (i = 0; i < nCount_Add_Header; i++)
                    {
                        for (j = 0; j < 128; j++)
                            bw.Write(str_null[j]);
                    }

                    // Write data
                    nRow = lst_data_New.Count;

                    for (i = 0; i < nRow; i++)
                    {
                        nColumn = lst_data_New[i].Length;
                        nRemain = pts_total - nColumn;

                        for (j = 0; j < nColumn; j++)
                        {
                            bw.Write(lst_data_New[i][j]);
                        }

                        for (j = 0; j < nRemain; j++)
                        {
                            bw.Write(short_zero);
                            //bw.Write("\0");
                        }
                    }

                    bw.Close();
                    fs.Close();
                }
            }

            return true;
        }

        public bool WriteToStatic(string path, StaticResult staticResult, ref string errMessage)
        {
            
            StringBuilder sb = new StringBuilder();
            string str_Header = "Result Name";
            string seperator = " , ";
            string str_precision = "F6";
            int i, j;

            foreach(string str in staticResult.ForceNames)
            {
                str_Header = str_Header + seperator + str;
            }

            sb.AppendLine(str_Header);

            i = 0;
            foreach(string str in staticResult.ResultFiles)
            {
                str_Header = str;

                double[] ar = staticResult.StaticData[i];

                for (j = 0; j < ar.Length; j++)
                    str_Header = str_Header + seperator + ar[j].ToString(str_precision);

                sb.AppendLine(str_Header);

                i++;
            }

            File.WriteAllText(path, sb.ToString());

            return true;
        }

        private bool Get_RPC_Header(DurabilityData durability, ref List<string> lst_key, ref List<string> lst_value, ref int nCount, ref string errMessage)
        {
            int i, j;
            int nchannels, nResultStep;
            int pts_per_frame = 0, frames = 0;
            int num_params, num_header_blocks;
            double delta_T;
            double dFull_Scale = durability.Full_Scale;
            double dScale = 1.0;

            nchannels = durability.NumOfResult;
            nResultStep = durability.ResultStep;
            delta_T = durability.StepSize;

            num_params = 19 + 6 * nchannels;
            num_header_blocks = (int)Math.Ceiling((double)(num_params / 4));
            if (num_params > (4 * num_header_blocks))
                num_header_blocks = num_header_blocks + 1;

            Calculate_NumOfFrame(nResultStep, ref pts_per_frame, ref frames);

            // 1. FORMAT
            lst_key.Add("FORMAT");
            lst_value.Add("BINARY");

            // 2. NUM_HEADER_BLOCKS
            lst_key.Add("NUM_HEADER_BLOCKS");
            lst_value.Add(num_header_blocks.ToString());

            // 3. NUM_PARAMS
            lst_key.Add("NUM_PARAMS");
            lst_value.Add(num_params.ToString());

            // 4. FILE_TYPE
            lst_key.Add("FILE_TYPE");
            lst_value.Add("TIME_HISTORY");

            // 5. TIME_TYPE
            lst_key.Add("TIME_TYPE");
            lst_value.Add("RESPONSE");

            // 6. DATE
            lst_key.Add("DATE");
            DateTime utcNow = DateTime.UtcNow;
            lst_value.Add(utcNow.Day.ToString("D2") + "-" + utcNow.Month.ToString("D2") + "-" + utcNow.Year.ToString("D4") + " " + utcNow.Hour.ToString("D2") + ":" + utcNow.Minute.ToString("D2") + ":" + utcNow.Second.ToString("D2"));

            // 7. OPERATION
            lst_key.Add("OPERATION");
            lst_value.Add("ANSYSMotion");

            // 8. BYPASS_FILTER
            lst_key.Add("BYPASS_FILTER");
            lst_value.Add("0");

            // 9. CHANNELS
            lst_key.Add("CHANNELS");
            lst_value.Add(nchannels.ToString());

            // 10. DATA_TYPE
            lst_key.Add("DATA_TYPE");
            lst_value.Add("SHORT_INTEGER");

            // 11. DELTA_T
            lst_key.Add("DELTA_T");
            lst_value.Add(delta_T.ToString("E6"));

            // 12. REPEATS
            lst_key.Add("REPEATS");
            lst_value.Add("0");

            // 13. PTS_PER_FRAME
            lst_key.Add("PTS_PER_FRAME");
            lst_value.Add(pts_per_frame.ToString());

            // 14. PTS_PER_GROUP
            lst_key.Add("PTS_PER_GROUP");
            lst_value.Add(pts_per_frame.ToString());

            // 15. FRAMES
            lst_key.Add("FRAMES");
            lst_value.Add(frames.ToString());

            // 16. HALF_FRAMES
            lst_key.Add("HALF_FRAMES");
            lst_value.Add("0");

            // 17. PARTITIONS
            lst_key.Add("PARTITIONS");
            lst_value.Add("1");

            // 18. PART.CHAN_1
            lst_key.Add("PART.CHAN_1");
            lst_value.Add("1");

            // 19. PART.NCHAN_1
            lst_key.Add("PART.NCHAN_1");
            lst_value.Add(nchannels.ToString());

            if (durability.Type == Category.Bodies)
            {
                i = 0;
                foreach (EntityForBody entity in durability.Body.Entities)
                {
                    for (j = 0; j < entity.ResultNames.Count; j++)
                    {
                        i++;

                        lst_key.Add("DESC.CHAN_" + i.ToString());
                        lst_value.Add(entity.ResultNames[j]);

                        lst_key.Add("UNITS.CHAN_" + i.ToString());
                        if (j < 3)
                            lst_value.Add(entity.Unit1);
                        else
                            lst_value.Add(entity.Unit2);

                        if(j < 3)
                            dScale = (entity.MaxValues[j] * entity.UnitScaleFactor[0]) / dFull_Scale;
                        else
                            dScale = (entity.MaxValues[j] * entity.UnitScaleFactor[1]) / dFull_Scale;

                        lst_key.Add("SCALE.CHAN_" + i.ToString());
                        lst_value.Add(dScale.ToString("E6"));

                        lst_key.Add("UPPER_LIMIT.CHAN_" + i.ToString());
                        lst_value.Add("1.0");

                        lst_key.Add("LOWER_LIMIT.CHAN_" + i.ToString());
                        lst_value.Add("-1.0");

                        lst_key.Add("MAP.CHAN_" + i.ToString());
                        lst_value.Add(i.ToString());


                    }
                }
            }
            else if (durability.Type == Category.Forces)
            {
                i = 0;
                foreach (Force force_data in durability.Forces)
                {
                    foreach (EntityForForce entity in force_data.Entities)
                    {
                        for (j = 0; j < entity.ResultNames.Count; j++)
                        {
                            i++;

                            lst_key.Add("DESC.CHAN_" + i.ToString());
                            lst_value.Add(entity.ResultNames[j]);

                            lst_key.Add("UNITS.CHAN_" + i.ToString());
                            if (j < 3 || (5 < j && j < 9))
                                lst_value.Add(entity.Unit1);
                            else
                                lst_value.Add(entity.Unit2);

                            if (j < 3 || (5 < j && j < 9))
                                dScale = (entity.MaxValues[j] * entity.UnitScaleFactor[0]) / dFull_Scale;
                            else
                                dScale = (entity.MaxValues[j] * entity.UnitScaleFactor[1]) / dFull_Scale;

                            lst_key.Add("SCALE.CHAN_" + i.ToString());
                            lst_value.Add(dScale.ToString("E6"));

                            lst_key.Add("UPPER_LIMIT.CHAN_" + i.ToString());
                            lst_value.Add("1.0");

                            lst_key.Add("LOWER_LIMIT.CHAN_" + i.ToString());
                            lst_value.Add("-1.0");

                            lst_key.Add("MAP.CHAN_" + i.ToString());
                            lst_value.Add(i.ToString());
                        }
                    }
                }
            }
            else if (durability.Type == Category.UserDefinedFunctions)
            {

            }

            // Add
            nCount = 4 * num_header_blocks - num_params;



            return true;
        }

        private bool Get_RPC_Header_FE(DurabilityData durability, Int32 nIndex, ref List<string> lst_key, ref List<string> lst_value, ref int nCount, ref string errMessage)
        {
            int i;
            int nchannels, nResultStep;
            int pts_per_frame = 0, frames = 0;
            int num_params, num_header_blocks;
            double delta_T;
            double dFull_Scale = durability.Full_Scale;
            double dScale = 1.0;

            nchannels = durability.FEBodies[nIndex].NumofMode + 6;
            nResultStep = durability.ResultStep;
            delta_T = durability.StepSize;

            num_params = 19 + 6 * nchannels;
            num_header_blocks = (int)Math.Ceiling((double)(num_params / 4));
            if (num_params > (4 * num_header_blocks))
                num_header_blocks = num_header_blocks + 1;

            Calculate_NumOfFrame(nResultStep, ref pts_per_frame, ref frames);

            // 1. FORMAT
            lst_key.Add("FORMAT");
            lst_value.Add("BINARY");

            // 2. NUM_HEADER_BLOCKS
            lst_key.Add("NUM_HEADER_BLOCKS");
            lst_value.Add(num_header_blocks.ToString());

            // 3. NUM_PARAMS
            lst_key.Add("NUM_PARAMS");
            lst_value.Add(num_params.ToString());

            // 4. FILE_TYPE
            lst_key.Add("FILE_TYPE");
            lst_value.Add("TIME_HISTORY");

            // 5. TIME_TYPE
            lst_key.Add("TIME_TYPE");
            lst_value.Add("RESPONSE");

            // 6. DATE
            lst_key.Add("DATE");
            DateTime utcNow = DateTime.UtcNow;
            lst_value.Add(utcNow.Day.ToString("D2") + "-" + utcNow.Month.ToString("D2") + "-" + utcNow.Year.ToString("D4") + " " + utcNow.Hour.ToString("D2") + ":" + utcNow.Minute.ToString("D2") + ":" + utcNow.Second.ToString("D2"));

            // 7. OPERATION
            lst_key.Add("OPERATION");
            lst_value.Add("ANSYSMotion");

            // 8. BYPASS_FILTER
            lst_key.Add("BYPASS_FILTER");
            lst_value.Add("0");

            // 9. CHANNELS
            lst_key.Add("CHANNELS");
            lst_value.Add(nchannels.ToString());

            // 10. DATA_TYPE
            lst_key.Add("DATA_TYPE");
            lst_value.Add("SHORT_INTEGER");

            // 11. DELTA_T
            lst_key.Add("DELTA_T");
            lst_value.Add(delta_T.ToString("E6"));

            // 12. REPEATS
            lst_key.Add("REPEATS");
            lst_value.Add("0");

            // 13. PTS_PER_FRAME
            lst_key.Add("PTS_PER_FRAME");
            lst_value.Add(pts_per_frame.ToString());

            // 14. PTS_PER_GROUP
            lst_key.Add("PTS_PER_GROUP");
            lst_value.Add(pts_per_frame.ToString());

            // 15. FRAMES
            lst_key.Add("FRAMES");
            lst_value.Add(frames.ToString());

            // 16. HALF_FRAMES
            lst_key.Add("HALF_FRAMES");
            lst_value.Add("0");

            // 17. PARTITIONS
            lst_key.Add("PARTITIONS");
            lst_value.Add("1");

            // 18. PART.CHAN_1
            lst_key.Add("PART.CHAN_1");
            lst_value.Add("1");

            // 19. PART.NCHAN_1
            lst_key.Add("PART.NCHAN_1");
            lst_value.Add(nchannels.ToString());

            FEBody fbody = durability.FEBodies[nIndex];
            Int32 nNumberofMode = fbody.NumofMode + 6;

            for (i = 0; i < nNumberofMode; i++)
            {
                lst_key.Add("DESC.CHAN_" + (i+1).ToString());
                lst_value.Add("Mode_" + (i + 1).ToString());

                lst_key.Add("UNITS.CHAN_" + (i + 1).ToString());
                lst_value.Add(durability.Unit_Length);

                dScale = (fbody.MaxValues[i] * durability.Scale_Length) / dFull_Scale;

                lst_key.Add("SCALE.CHAN_" + (i + 1).ToString());
                lst_value.Add(dScale.ToString("E6"));

                lst_key.Add("UPPER_LIMIT.CHAN_" + (i + 1).ToString());
                lst_value.Add("1.0");

                lst_key.Add("LOWER_LIMIT.CHAN_" + (i + 1).ToString());
                lst_value.Add("-1.0");

                lst_key.Add("MAP.CHAN_" + (i + 1).ToString());
                lst_value.Add((i + 1).ToString());
            }


            // Add
            nCount = 4 * num_header_blocks - num_params;



            return true;
        }

        private bool Get_RPC_Data(DurabilityData durability, ref List<double[]> lst_data, ref List<double> lst_max)
        {
            int i, j, nRow, nColumn;
            double[] yarray;

            if (durability.Type == Category.Bodies)
            {
                foreach (EntityForBody entity in durability.Body.Entities)
                {
                    //foreach (double[] arr in entity.FixedStepValue)
                    //{
                    //    lst_data.Add(arr);
                    //}
                    nRow = entity.FixedStepValue.Count;
                    nColumn = entity.FixedStepValue[0].Length;

                    for (i = 0; i < nColumn; i++)
                    {
                        yarray = new double[nRow];
                        for (j = 0; j < nRow; j++)
                        {
                            if(i < 3)
                                yarray[j] = entity.FixedStepValue[j][i] * entity.UnitScaleFactor[0];
                            else
                                yarray[j] = entity.FixedStepValue[j][i] * entity.UnitScaleFactor[1];
                        }

                        lst_data.Add(yarray);
                    }

                    i = 0;
                    foreach (double dmax in entity.MaxValues)
                    {
                        if (i < 3)
                            lst_max.Add(dmax * entity.UnitScaleFactor[0]);
                        else
                            lst_max.Add(dmax * entity.UnitScaleFactor[1]);

                        i++;
                    }
                }

            }
            else if (durability.Type == Category.Forces)
            {
                foreach (Force force_data in durability.Forces)
                {
                    foreach (EntityForForce entity in force_data.Entities)
                    {
                        //foreach (double[] arr in entity.FixedStepValue)
                        //{
                        //    lst_data.Add(arr);
                        //}

                        nRow = entity.FixedStepValue.Count;
                        nColumn = entity.FixedStepValue[0].Length;

                        for (i = 0; i < nColumn; i++)
                        {
                            yarray = new double[nRow];
                            for (j = 0; j < nRow; j++)
                            {
                                if (i < 3 || (5 < i && i < 9))
                                    yarray[j] = entity.FixedStepValue[j][i] * entity.UnitScaleFactor[0];
                                else
                                    yarray[j] = entity.FixedStepValue[j][i] * entity.UnitScaleFactor[1];
                            }

                            lst_data.Add(yarray);
                        }

                        i = 0;
                        foreach (double dmax in entity.MaxValues)
                        {
                            if (i < 3 || (5 < i && i < 9))
                                lst_max.Add(dmax * entity.UnitScaleFactor[0]);
                            else
                                lst_max.Add(dmax * entity.UnitScaleFactor[1]);

                            i++;
                        }
                    }
                }

            }
            else if (durability.Type == Category.UserDefinedFunctions)
            {

            }


            return true;
        }

        private bool Get_RPC_Data_FE(DurabilityData durability, Int32 nIndex, ref List<double[]> lst_data, ref List<double> lst_max)
        {
            int i, j, nRow, nColumn;
            double[] yarray;
            FEBody fbody = durability.FEBodies[nIndex];

            nRow = durability.FixedTimes.Count;
            nColumn = fbody.FixedTime_Modal_Coordinates.Count + 6;


            for (i = 0; i < nColumn; i++)
            {
                yarray = new double[nRow];

                for (j = 0; j < nRow; j++)
                {
                    if (i < 6)
                        yarray[j] = 0.0;
                    else
                    {
                        yarray[j] = fbody.FixedTime_Modal_Coordinates[i - 6][j] * durability.Scale_Length;

                    }
                }
                lst_data.Add(yarray);
            }

            foreach (double dmax in fbody.MaxValues)
            {
                lst_max.Add(dmax * durability.Scale_Length);
            }


            return true;
        }

        private bool COnvert_RPC_Data_To_INT_FULL_SCALE(List<double[]> lst_data, List<double> lst_max, Int16 full_scale, ref List<Int16[]> lst_data_new)
        {
            int i, j;
            int nRow, nColumn;
            double dmax;

            nRow = lst_data.Count;
            nColumn = lst_data[0].Length;

            for (i = 0; i < nRow; i++)
            {
                dmax = lst_max[i];
                lst_data_new.Add(new Int16[nColumn]);

                if (0.0 != dmax)
                {
                    for (j = 0; j < nColumn; j++)
                    {
                        lst_data_new[i][j] = (Int16)Math.Round((lst_data[i][j] / dmax) * full_scale);
                    }
                }
                else
                {
                    for (j = 0; j < nColumn; j++)
                    {
                        lst_data_new[i][j] = (Int16)Math.Round(0.0);
                    }
                }
            }


            return true;
        }

        #endregion

        #region Calculation

        private bool Conversion_Unit(PostAPI.PostAPI _postAPI, XmlNode node_Unit, ref DurabilityData durability)
        {
            double dForce_factor = 0.0;
            double dLength_factor = 0.0;
            double dAngle_factor = 0.0;
            double dTime_factor = 0.0;

            // Get Units
            var units = _postAPI.GetUnits();

            string Unit_Force = node_Unit.Attributes.GetNamedItem("force").Value;
            string Unit_Length = node_Unit.Attributes.GetNamedItem("length").Value;
            string Unit_Angle = node_Unit.Attributes.GetNamedItem("angle").Value;
            string Unit_Time = node_Unit.Attributes.GetNamedItem("time").Value;

            // Force
            if (false == UnitConversion(units["F"].Key, Unit_Force, ref dForce_factor))
                return false;
            durability.Unit_Force = Unit_Force;
            durability.Scale_Force = dForce_factor;

            // Length
            if (false == UnitConversion(units["L"].Key, Unit_Length, ref dLength_factor))
                return false;
            durability.Unit_Length = Unit_Length;
            durability.Scale_Length = dLength_factor;

            // Angle
            if (false == UnitConversion(units["A"].Key, Unit_Angle, ref dAngle_factor))
                return false;
            durability.Unit_Angle = Unit_Angle;
            durability.Scale_Angle = dAngle_factor;

            // Time
            if (false == UnitConversion(units["T"].Key, Unit_Time, ref dTime_factor))
                return false;
            durability.Unit_Time = Unit_Time;
            durability.Scale_Time = dTime_factor;

            return true;
        }

        private bool UnitConversion(string fromUnit, string toUnit, ref double _dFactor)
        {

            if (fromUnit == "N")
            {
                if (toUnit == "N")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else if (toUnit == "kgf")
                {
                    _dFactor = 0.1019716212977928;
                    return true;
                }
                else if (toUnit == "lbf")
                {
                    _dFactor = 0.224809;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "kgf")
            {
                if (toUnit == "N")
                {
                    _dFactor = 9.80665;
                    return true;
                }
                else if (toUnit == "kgf")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else if (toUnit == "lbf")
                {
                    _dFactor = 2.2046;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "lbf")
            {
                if (toUnit == "N")
                {
                    _dFactor = 4.4482;
                    return true;
                }
                else if (toUnit == "kgf")
                {
                    _dFactor = 0.453592;
                    return true;
                }
                else if (toUnit == "lbf")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "mm")
            {
                if (toUnit == "mm")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else if (toUnit == "m")
                {
                    _dFactor = 0.001;
                    return true;
                }
                else if (toUnit == "inch")
                {
                    _dFactor = 0.03937;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "m")
            {
                if (toUnit == "mm")
                {
                    _dFactor = 1000.0;
                    return true;
                }
                else if (toUnit == "m")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else if (toUnit == "inch")
                {
                    _dFactor = 39.3701;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "inch")
            {
                if (toUnit == "mm")
                {
                    _dFactor = 25.4;
                    return true;
                }
                else if (toUnit == "m")
                {
                    _dFactor = 0.0254;
                    return true;
                }
                else if (toUnit == "inch")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "deg")
            {
                if (toUnit == "deg")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else if (toUnit == "rad")
                {
                    _dFactor = 0.0174532925;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "rad")
            {
                if (toUnit == "deg")
                {
                    _dFactor = 57.295779513;
                    return true;
                }
                else if (toUnit == "rad")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "sec")
            {
                if (toUnit == "sec")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else if (toUnit == "min")
                {
                    _dFactor = 0.0166666667;
                    return true;
                }
                else
                    return false;
            }
            else if (fromUnit == "min")
            {
                if (toUnit == "sec")
                {
                    _dFactor = 60.0;
                    return true;
                }
                else if (toUnit == "min")
                {
                    _dFactor = 1.0;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private bool Determine_Result_Step(ref DurabilityData durability, ref string errMessage)
        {
            int nCount;

            if (durability.StepSize > durability.EndTime)
            {
                errMessage += "Error : The step size is greater than the end time of result\n";
                //MessageBox.Show("The step size is greater than the end time of result", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            nCount = (int)(durability.EndTime / durability.StepSize);
            durability.EndTime_Modify = nCount * durability.StepSize;

            if (durability.EndTime_Modify > durability.EndTime)
            {
                nCount = nCount - 1;
                durability.EndTime_Modify = nCount * durability.StepSize;
            }

            durability.ResultStep = nCount + 1;


            return true;
        }

        private void Calculate_NumOfFrame(int numofresult, ref int pts_per_frame, ref int frames)
        {
            int tmp, _numofFrame;
            double nValue;

            nValue = Math.Log10(numofresult) / Math.Log10(2);
            tmp = (int)Math.Ceiling(nValue);

            _numofFrame = (int)Math.Pow(2, tmp);

            if (numofresult > 2048)
            {
                tmp = tmp - 11;

                pts_per_frame = 2048;
                frames = (int)Math.Pow(2, tmp);
            }
            else
            {
                pts_per_frame = _numofFrame;
                frames = 1;
            }
        }

        #endregion

        #region Create XML

        public XmlDocument CreateDurabilityXML()
        {
            XmlDocument dom = new XmlDocument();

            dom.AppendChild(dom.CreateElement("MotionDurability"));
            dom.DocumentElement.Attributes.Append(dom.CreateAttribute("xmlns:xsi"));
            dom.DocumentElement.Attributes.GetNamedItem("xmlns:xsi").Value = "http://www.w3.org/2001/XMLSchema-instance";
            //dom.DocumentElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            dom.DocumentElement.AppendChild(dom.CreateElement("Configuration"));
            dom.DocumentElement.AppendChild(dom.CreateElement("UserDefinedItems"));

            XmlDeclaration xmldecl = dom.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
            XmlElement root = dom.DocumentElement;
            dom.InsertBefore(xmldecl, root);

            return dom;
        }

        public XmlDocument CreateXMLFromPost(string _path_dfr, ref string errMessage)
        {
            int j, count_Rbody, count_connector, count_FModal;
            XmlDocument dom = CreateDurabilityXML();
            string str_result_Name = Path.GetFileNameWithoutExtension(_path_dfr);
            string str_res = Path.Combine(Path.GetDirectoryName(_path_dfr), str_result_Name + ".res");

            PostAPI.PostAPI postAPI = new PostAPI.PostAPI(_path_dfr);
            if(postAPI == null)
            {
                MessageBox.Show(string.Format("The {0} file cannot open. Please check it", str_result_Name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if(false == File.Exists(str_res))
            {
                MessageBox.Show(string.Format("{0} file does not exist. Please check it", Path.GetFileName(str_res)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            IEnumerable<(BodyType, string)> Rbodies = postAPI.GetBodies(BodyType.RIGID);
            IEnumerable<(BodyType, string)> Fbodies = postAPI.GetBodies(BodyType.MODAL);

            count_Rbody = Rbodies.Count();
            count_FModal = Fbodies.Count();

            if (count_Rbody == 0 && count_FModal == 0)
            {
                MessageBox.Show(string.Format("{0} does not have a rigid or FE modal body. Please check it", Path.GetFileName(str_result_Name)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            XmlNode node_Result = dom.CreateElement("Result");
            

            CreateAttributeXML(dom, ref node_Result, "path", _path_dfr);
            CreateAttributeXML(dom, ref node_Result, "name", str_result_Name);

            XmlNode node_types = dom.CreateElement("Types");
            XmlNode node_bodies = CreateNodeAndAttribute(dom, "Type", "name", "Bodies");
            XmlNode node_forces = CreateNodeAndAttribute(dom, "Type", "name", "Forces");
            XmlNode node_userfunctions = CreateNodeAndAttribute(dom, "Type", "name", "User defined functions");
            XmlNode node_FEs = CreateNodeAndAttribute(dom, "Type", "name", "Flexible Bodies");

            AnalysisModelType analysisModelType = postAPI.GetPrimaryAnalysisType();
            if (analysisModelType == AnalysisModelType.Dynamics)
                CreateAttributeXML(dom, ref node_Result, "analysis", "dynamics");
            else if (analysisModelType == AnalysisModelType.Statics)
                CreateAttributeXML(dom, ref node_Result, "analysis", "static");
            else
            {
                MessageBox.Show(string.Format("{0} is not supported analysis type(Supported Analysis Type : Dynamics and Statics). Please check it", Path.GetFileName(str_result_Name)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // For Rigid body
            //bool isExistChassis = false;
            //for (i = 0; i < count_Rbody; i++)
            foreach((BodyType, string)rbody in Rbodies)
            {
                XmlNode node_body = CreateNodeAndAttribute(dom, "Body", "name", rbody.Item2);

                //if (Rbodies[i].Item2.Contains("chassis"))
                //    isExistChassis = true;

                var connectors = postAPI.GetConnectors(rbody.Item2);
                count_connector = connectors.Count;

                for(j = 0; j < count_connector; j++)
                {
                    XmlNode node_B_entity = CreateNodeAndAttribute(dom, "Entity", "name", connectors[j].Item3);

                    if (connectors[j].Item1 == ConnectorType.Ball || connectors[j].Item1 == ConnectorType.ConstantVelocity || connectors[j].Item1 == ConnectorType.Cylindrical
                        || connectors[j].Item1 == ConnectorType.Distance || connectors[j].Item1 == ConnectorType.Fixed || connectors[j].Item1 == ConnectorType.Inline
                        || connectors[j].Item1 == ConnectorType.Inplane || connectors[j].Item1 == ConnectorType.Orientation || connectors[j].Item1 == ConnectorType.Parallel
                        || connectors[j].Item1 == ConnectorType.Perpendicular || connectors[j].Item1 == ConnectorType.Plane || connectors[j].Item1 == ConnectorType.Revolute
                        || connectors[j].Item1 == ConnectorType.Screw || connectors[j].Item1 == ConnectorType.Translational || connectors[j].Item1 == ConnectorType.Universal)
                    {
                        CreateAttributeXML(dom, ref node_B_entity, "type", "contraints");
                        node_body.AppendChild(node_B_entity);
                    }
                    else if (connectors[j].Item1 == ConnectorType.Beam || connectors[j].Item1 == ConnectorType.Bush || connectors[j].Item1 == ConnectorType.Matrix
                        || connectors[j].Item1 == ConnectorType.RScalar || connectors[j].Item1 == ConnectorType.RSpringDamper || connectors[j].Item1 == ConnectorType.Tire
                        || connectors[j].Item1 == ConnectorType.TScalar || connectors[j].Item1 == ConnectorType.TSpringDamper || connectors[j].Item1 == ConnectorType.Vector)
                    {
                        CreateAttributeXML(dom, ref node_B_entity, "type", "force");
                        node_body.AppendChild(node_B_entity);

                        if (connectors[j].Item1 == ConnectorType.TSpringDamper || connectors[j].Item1 == ConnectorType.TScalar || connectors[j].Item1 == ConnectorType.Bush || connectors[j].Item1 == ConnectorType.Tire)
                        {
                            XmlNode node_force = CreateNodeAndAttribute(dom, "Force", "name", connectors[j].Item3);
                            XmlNode node_E_force = CreateNodeAndAttribute(dom, "Entity", "name", "Force");
                            XmlNode node_E_Rdisp = CreateNodeAndAttribute(dom, "Entity", "name", "Relative Displacement");
                            XmlNode node_E_Rvelo = CreateNodeAndAttribute(dom, "Entity", "name", "Relative Velocity");
                            XmlNode node_E_Racc = CreateNodeAndAttribute(dom, "Entity", "name", "Relative Acceleration");

                            node_force.AppendChild(node_E_force);
                            node_force.AppendChild(node_E_Rdisp);
                            node_force.AppendChild(node_E_Rvelo);
                            node_force.AppendChild(node_E_Racc);

                            node_forces.AppendChild(node_force);
                        }
                    }

                    //if(connectors[j].Item2 == ActionType.Action)
                    //{
                    //    XmlNode node_B_entity = CreateNodeAndAttribute(dom, "Entity", "name", connectors[j].Item3);

                    //    if (connectors[j].Item1 == ConnectorType.Ball || connectors[j].Item1 == ConnectorType.ConstantVelocity || connectors[j].Item1 == ConnectorType.Cylindrical
                    //        || connectors[j].Item1 == ConnectorType.Distance || connectors[j].Item1 == ConnectorType.Fixed || connectors[j].Item1 == ConnectorType.Inline
                    //        || connectors[j].Item1 == ConnectorType.Inplane || connectors[j].Item1 == ConnectorType.Orientation || connectors[j].Item1 == ConnectorType.Parallel
                    //        || connectors[j].Item1 == ConnectorType.Perpendicular || connectors[j].Item1 == ConnectorType.Plane || connectors[j].Item1 == ConnectorType.Revolute
                    //        || connectors[j].Item1 == ConnectorType.Screw || connectors[j].Item1 == ConnectorType.Translational || connectors[j].Item1 == ConnectorType.Universal)
                    //    {
                    //        CreateAttributeXML(dom, ref node_B_entity, "type", "contraints");
                    //        node_body.AppendChild(node_B_entity);
                    //    }
                    //    else if (connectors[j].Item1 == ConnectorType.Beam || connectors[j].Item1 == ConnectorType.Bush || connectors[j].Item1 == ConnectorType.Matrix
                    //        || connectors[j].Item1 == ConnectorType.RScalar || connectors[j].Item1 == ConnectorType.RSpringDamper || connectors[j].Item1 == ConnectorType.Tire
                    //        || connectors[j].Item1 == ConnectorType.TScalar || connectors[j].Item1 == ConnectorType.TSpringDamper || connectors[j].Item1 == ConnectorType.Vector)
                    //    {
                    //        CreateAttributeXML(dom, ref node_B_entity, "type", "force");
                    //        node_body.AppendChild(node_B_entity);

                    //        if (connectors[j].Item1 == ConnectorType.TSpringDamper || connectors[j].Item1 == ConnectorType.Bush || connectors[j].Item1 == ConnectorType.Tire)
                    //        {
                    //            XmlNode node_force = CreateNodeAndAttribute(dom, "Force", "name", connectors[j].Item3);
                    //            XmlNode node_E_force = CreateNodeAndAttribute(dom, "Entity", "name", "Force");
                    //            XmlNode node_E_Rdisp = CreateNodeAndAttribute(dom, "Entity", "name", "Relative Displacement");
                    //            XmlNode node_E_Rvelo = CreateNodeAndAttribute(dom, "Entity", "name", "Relative Velocity");
                    //            XmlNode node_E_Racc = CreateNodeAndAttribute(dom, "Entity", "name", "Relative Acceleration");

                    //            node_force.AppendChild(node_E_force);
                    //            node_force.AppendChild(node_E_Rdisp);
                    //            node_force.AppendChild(node_E_Rvelo);
                    //            node_force.AppendChild(node_E_Racc);

                    //            node_forces.AppendChild(node_force);
                    //        }
                    //    }
                    //}
                }

               

                // Append motion node 
                XmlNode node_disp = CreateNodeAndAttribute(dom, "Entity", "name", "Displacement");
                CreateAttributeXML(dom, ref node_disp, "type", "motion");
                node_body.AppendChild(node_disp);

                XmlNode node_velo = CreateNodeAndAttribute(dom, "Entity", "name", "Velocity");
                CreateAttributeXML(dom, ref node_velo, "type", "motion");
                node_body.AppendChild(node_velo);

                XmlNode node_acc = CreateNodeAndAttribute(dom, "Entity", "name", "Acceleration");
                CreateAttributeXML(dom, ref node_acc, "type", "motion");
                node_body.AppendChild(node_acc);

                node_bodies.AppendChild(node_body);
            }

            // For Modal body
            //for(i = 0; i < count_FModal; i++)
            foreach((BodyType, string)fbody in Fbodies)
            {
                XmlNode node_Fbody = CreateNodeAndAttribute(dom, "FBody", "name", fbody.Item2);
                CreateAttributeXML(dom, ref node_Fbody, "mcf_format", "0");
                node_FEs.AppendChild(node_Fbody);
            }

            // Append child node
            node_types.AppendChild(node_bodies);
            node_types.AppendChild(node_forces);
            node_types.AppendChild(node_userfunctions);
            node_types.AppendChild(node_FEs);
            node_Result.AppendChild(node_types);
            dom.DocumentElement.SelectSingleNode("Configuration").AppendChild(node_Result);

            // Close PostAPI
            postAPI.Close();

            return dom;
        }

        public XmlNode CreateNodeAndAttribute(XmlDocument dom, string str_name, string att_name, string att_value)
        {
            XmlNode node_target = dom.CreateElement(str_name);
            node_target.Attributes.Append(dom.CreateAttribute(att_name));
            node_target.Attributes.GetNamedItem(att_name).Value = att_value;


            return node_target;
        }

        public void CreateAttributeXML(XmlDocument dom, ref XmlNode node_target, string att_name, string att_value)
        {
            node_target.Attributes.Append(dom.CreateAttribute(att_name));
            node_target.Attributes.GetNamedItem(att_name).Value = att_value;

        }

        public bool Distinguish_Analysis_Type(AnalysisModelType _toolScenario, string _path_dfr, ref bool _isSameAnalysysType, ref string errMessage)
        {
           // int i, j, count_Rbody;
            AnalysisModelType targetScenario;
            string str_result_Name = Path.GetFileNameWithoutExtension(_path_dfr);
            string str_res = Path.Combine(Path.GetDirectoryName(_path_dfr), str_result_Name + ".res");

            if (false == File.Exists(str_res))
            {
                MessageBox.Show(string.Format("{0} file does not exist. Please check it", Path.GetFileName(str_res)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            PostAPI.PostAPI postAPI = new PostAPI.PostAPI(_path_dfr);
            if (postAPI == null)
            {
                MessageBox.Show(string.Format("The {0} file cannot open. Please check it", str_result_Name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            targetScenario = postAPI.GetPrimaryAnalysisType();

            //IList<(BodyType, string)> Rbodies = postAPI.GetBodies(BodyType.RIGID);

            //count_Rbody = Rbodies.Count;

            //if (count_Rbody == 0)
            //{
            //    MessageBox.Show(string.Format("{0} does not have a rigid body. Please check it", Path.GetFileName(str_result_Name)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}

            //targetScenario = AnalysisModelType.Statics;
            //for (i = 0; i < count_Rbody; i++)
            //{
            //    if (Rbodies[i].Item2.Contains("chassis"))
            //    {
            //        targetScenario = AnalysisModelType.Dynamics;
            //        break;
            //    }
            //}

            if (_toolScenario == targetScenario)
                _isSameAnalysysType = true;
            else
                _isSameAnalysysType = false;

            postAPI.Close();

            return true;
        }



        #endregion


        #region Read
        
        public bool ReadFile(string _str_input_path, string _str_export_path, ref string errMessage)
        {
            string extension_input = Path.GetExtension(_str_input_path);
            string name_input = Path.GetFileNameWithoutExtension(_str_input_path);

            string extension_output = "";

            if (extension_input.Contains("rsp"))
            {
                RPCReader.RPCReader rpc_reader = new RPCReader.RPCReader(name_input);

                if (false == Read_RPC(_str_input_path, rpc_reader, ref errMessage))
                {
                    errMessage += "Error : Failed to read RPC file\n";
                    return false;
                }

                if (null != _str_export_path && "" != _str_export_path)
                {
                    extension_output = Path.GetExtension(_str_export_path);

                    if (extension_output.Contains("csv"))
                    {
                        if (false == ReadRPC_WriteCSV(_str_export_path, rpc_reader, ref errMessage))
                        {
                            errMessage += "Error : Failed to write CSV file\n";
                            return false;
                        }
                    }
                    else
                    {
                        errMessage += string.Format("The {0} format is not supported! Please check file format", extension_output);
                        return false;
                    }

                }
            }
            else
            {
                errMessage += string.Format("The {0} format is not supported! Please check file format", extension_input);
                return false;
            }


            return true;
        }

        private bool Read_RPC(string _str_input_path, RPCReader.RPCReader _rpc_data, ref string errMessage)
        {
            try
            {
                string s_key_header;
                string s_value_header;
                string[] tmp_split;

                char[] char_split = { '_' };
                char[] char_trim1 = { '\0'};
                char[] key_header = new char[32];
                char[] value_header = new char[96];

                double dMax;
                Int32 i,j, nCount = 0, nHeaderLines, numChan = 0, numdatas = 0, nInt_Full_Scale = _rpc_data.INT_FULL_SCALE;
                short raw_data_int16;
                //byte raw_data_byte;

                if (File.Exists(_str_input_path))
                {
                    #region Read Binary
                    
                    using (FileStream stream = new FileStream(_str_input_path, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader br = new BinaryReader(stream,Encoding.UTF8))
                        {
                            #region Header Block
                            for (i = 0; i < 3; i++)
                            {
                                nCount++;

                                key_header = br.ReadChars(32);
                                value_header = br.ReadChars(96);

                                s_key_header = new string(key_header);
                                s_value_header = new string(value_header);

                                if (s_key_header.Contains("FORMAT"))
                                {
                                    if (s_value_header.Contains("BINARY"))
                                        _rpc_data.Format = RPCReader.RPC_FORMAT.BINARY;
                                    else if (s_value_header.Contains("BINARY_IEEE_LITTLE_END"))
                                        _rpc_data.Format = RPCReader.RPC_FORMAT.BINARY_IEEE_LITTLE_END;
                                    else if (s_value_header.Contains("BINARY_IEEE_BIG_END"))
                                        _rpc_data.Format = RPCReader.RPC_FORMAT.BINARY_IEEE_BIG_END;
                                    else if (s_value_header.Contains("ASCII"))
                                        _rpc_data.Format = RPCReader.RPC_FORMAT.ASCII;

                                }
                                else if (s_key_header.Contains("NUM_HEADER_BLOCKS"))
                                {
                                    _rpc_data.Num_Header_Blocks = Convert.ToInt32(s_value_header);
                                }
                                else if (s_key_header.Contains("NUM_PARAMS"))
                                {
                                    _rpc_data.Num_Params = Convert.ToInt32(s_value_header);
                                }


                            }

                            if(_rpc_data.Num_Header_Blocks == 0)
                            {
                                errMessage += "The value of NUM_HEADER_BLOCKS isn`t defined\n";
                                return false;
                            }
                            else
                            {
                                nHeaderLines = _rpc_data.Num_Header_Blocks * 4 - 3;

                                for (i = 0; i < nHeaderLines; i++)
                                {
                                    nCount++;

                                    key_header = br.ReadChars(32);
                                    value_header = br.ReadChars(96);

                                    if (key_header[0] != '\0')
                                    {
                                        s_key_header = new string(key_header);
                                        s_value_header = new string(value_header);


                                        if (s_key_header.Contains("CHAN_"))
                                        {
                                            tmp_split = s_key_header.Trim(char_trim1).Split(char_split, StringSplitOptions.RemoveEmptyEntries);
                                            numChan = Convert.ToInt32(tmp_split[tmp_split.Length - 1]);

                                            if(s_key_header.Contains("DESC"))
                                            {
                                                _rpc_data.Datas[numChan - 1].DESC_CHAN = s_value_header.Trim(char_trim1);
                                            }
                                            else if (s_key_header.Contains("UNITS"))
                                            {
                                                _rpc_data.Datas[numChan - 1].UNITS = s_value_header.Trim(char_trim1);
                                            }
                                            else if (s_key_header.Contains("SCALE"))
                                            {
                                                _rpc_data.Datas[numChan - 1].SCALE_CHAN = Convert.ToDouble(s_value_header);
                                            }
                                            else if (s_key_header.Contains("UPPER_LIMIT"))
                                            {
                                                _rpc_data.Datas[numChan - 1].UPPER_LIMIT = Convert.ToDouble(s_value_header);
                                            }
                                            else if (s_key_header.Contains("LOWER_LIMIT"))
                                            {
                                                _rpc_data.Datas[numChan - 1].LOWER_LIMIT = Convert.ToDouble(s_value_header);
                                            }
                                            else if (s_key_header.Contains("MAP"))
                                            {
                                                _rpc_data.Datas[numChan - 1].MAP_CHAN = Convert.ToInt32(s_value_header);
                                            }

                                        }
                                        else if (s_key_header.Contains("FILE_TYPE"))
                                        {
                                            if (s_value_header.Contains("TIME_HISTRY"))
                                                _rpc_data.File_Type = RPCReader.RPC_FILE_TYPE.TIME_HISTRY;
                                            else if (s_value_header.Contains("CONFIGURATION"))
                                                _rpc_data.File_Type = RPCReader.RPC_FILE_TYPE.CONFIGURATION;
                                            else if (s_value_header.Contains("MATRIX"))
                                                _rpc_data.File_Type = RPCReader.RPC_FILE_TYPE.MATRIX;
                                            else if (s_value_header.Contains("FATIGUE"))
                                                _rpc_data.File_Type = RPCReader.RPC_FILE_TYPE.FATIGUE;
                                            else if (s_value_header.Contains("ROAD_SURFACE"))
                                                _rpc_data.File_Type = RPCReader.RPC_FILE_TYPE.ROAD_SURFACE;
                                            else if (s_value_header.Contains("SPECTRAL"))
                                                _rpc_data.File_Type = RPCReader.RPC_FILE_TYPE.SPECTRAL;
                                            else if (s_value_header.Contains("START"))
                                                _rpc_data.File_Type = RPCReader.RPC_FILE_TYPE.START;
                                        }
                                        else if (s_key_header.Contains("TIME_TYPE"))
                                        {
                                            if (s_value_header.Contains("DRIVE"))
                                                _rpc_data.Time_Type = RPCReader.TIME_TYPE.DRIVE;
                                            else if (s_value_header.Contains("RESPONSE"))
                                                _rpc_data.Time_Type = RPCReader.TIME_TYPE.RESPONSE;
                                            else if (s_value_header.Contains("MULT_DRIVE"))
                                                _rpc_data.Time_Type = RPCReader.TIME_TYPE.MULT_DRIVE;
                                            else if (s_value_header.Contains("MULT_RESP"))
                                                _rpc_data.Time_Type = RPCReader.TIME_TYPE.MULT_RESP;
                                            else if (s_value_header.Contains("CONFIG_DRIVE"))
                                                _rpc_data.Time_Type = RPCReader.TIME_TYPE.CONFIG_DRIVE;
                                            else if (s_value_header.Contains("CONFIG_RESP"))
                                                _rpc_data.Time_Type = RPCReader.TIME_TYPE.CONFIG_RESP;
                                            else if (s_value_header.Contains("PEAK_PICK"))
                                                _rpc_data.Time_Type = RPCReader.TIME_TYPE.PEAK_PICK;
                                        }
                                        else if (s_key_header.Contains("OPERATION"))
                                        {
                                            _rpc_data.OPERATION = s_value_header;
                                        }
                                        else if (s_key_header.Contains("BYPASS_FILTER"))
                                        {
                                            if (Convert.ToInt32(s_value_header) == 0)
                                                _rpc_data.Bypass_Filter = RPCReader.BYPASS_FILTER.Off;
                                            else
                                                _rpc_data.Bypass_Filter = RPCReader.BYPASS_FILTER.On;
                                        }
                                        else if (s_key_header.Contains("CHANNELS"))
                                        {
                                            _rpc_data.Channels = Convert.ToInt32(s_value_header);
                                            
                                            for(j = 0; j < _rpc_data.Channels; j++)
                                            {
                                                _rpc_data.Datas.Add(new RPCReader.RPCData());
                                            }
                                        }
                                        else if (s_key_header.Contains("DATA_TYPE"))
                                        {
                                            if (s_value_header.Contains("SHORT_INTEGER"))
                                                _rpc_data.Data_Type = RPCReader.DATA_TYPE.SHORT_INTEGER;
                                            else
                                                _rpc_data.Data_Type = RPCReader.DATA_TYPE.FLOATING_POINT;
                                        }
                                        else if (s_key_header.Contains("DELTA_T"))
                                        {
                                            _rpc_data.Delta_T = Convert.ToDouble(s_value_header);
                                        }
                                        else if (s_key_header.Contains("REPEATS"))
                                        {
                                            _rpc_data.Repeats = Convert.ToInt32(s_value_header);
                                        }
                                        else if (s_key_header.Contains("PTS_PER_FRAME"))
                                        {
                                            _rpc_data.Pts_Per_Frame = Convert.ToInt32(s_value_header);
                                        }
                                        else if (s_key_header.Trim(char_trim1) == "FRAMES")
                                        {
                                            _rpc_data.Frames = Convert.ToInt32(s_value_header);
                                        }
                                        else if (s_key_header.Trim(char_trim1) == "HALF_FRAMES")
                                        {
                                            _rpc_data.Half_Frames = Convert.ToInt32(s_value_header);
                                        }

                                    }



                                }
                            }

                            #endregion

                            #region DATA Blocks
                            if(0 == _rpc_data.Pts_Per_Frame)
                            {
                                errMessage += "There is no information about 'PTS_PER_FRAME' in the rpc file. So please review.\n";
                                return false;
                            }
                            else if(0 == _rpc_data.Frames)
                            {
                                errMessage += "There is no information about 'FRAMES' in the rpc file. So please review.\n";
                                return false;
                            }
                            else if (0 == _rpc_data.Channels)
                            {
                                errMessage += "There is no information about 'CHANNELS' in the rpc file. So please review.\n";
                                return false;
                            }
                            else if (0.0 == _rpc_data.Delta_T)
                            {
                                errMessage += "There is no information about 'DELTA_T' in the rpc file. So please review.\n";
                                return false;
                            }

                            numdatas = _rpc_data.Pts_Per_Frame * _rpc_data.Frames;
                            for (i = 0; i < _rpc_data.Channels; i++)
                            {
                                nCount = 0;

                                for (j = 0; j < numdatas; j++)
                                {
                                    raw_data_int16 = br.ReadInt16();
                                    if (raw_data_int16.ToString() == "\0")
                                    {
                                        if (i == 0)
                                            numdatas = nCount + 1;

                                        continue;
                                    }
                                    else
                                        _rpc_data.Datas[i].Orifinal_Data.Add(raw_data_int16);

                                    nCount++;
                                }
                            }

                            #endregion

                        }
                    }

                    #endregion

                    #region Recovery datas

                    for(i = 0; i < _rpc_data.Channels; i++)
                    {
                        numdatas = _rpc_data.Datas[i].Orifinal_Data.Count;
                        dMax = _rpc_data.Datas[i].SCALE_CHAN;

                        for(j = 0; j < numdatas; j++)
                        {
                            //_rpc_data.Datas[i].Export_Data.Add((_rpc_data.Datas[i].Orifinal_Data[j] / nInt_Full_Scale) * dMax);
                            _rpc_data.Datas[i].Export_Data.Add(_rpc_data.Datas[i].Orifinal_Data[j]  * dMax);
                        }
                    }

                    #endregion

                    #region Generation Time series

                    numdatas = _rpc_data.Datas[0].Orifinal_Data.Count;

                    for (j = 0; j < numdatas; j++)
                        _rpc_data.Times.Add( j * _rpc_data.Delta_T );

                    #endregion

                }
                else
                {
                    errMessage += "RPC file not found.\n";
                    return false;
                }
                return true;
            }
            catch(System.Exception e)
            {
                errMessage += e.Message + "\n";
                //MessageBox.Show(e.Message);
                return false;
            }
        }

        private bool ReadRPC_WriteCSV(string _str_export_path, RPCReader.RPCReader _rpc_data, ref string errMessage)
        {
            StringBuilder sb = new StringBuilder();
           
            string str_Header = "";
            string seperator = " , ";
            string str_precision = "e6";
            
            int i, j;
            int nRow = _rpc_data.Times.Count;
            int nColumn = _rpc_data.Channels;

            for(i = 0; i < nColumn; i++)
            {
                if (i == 0)
                    str_Header = "time" + seperator + _rpc_data.Datas[i].DESC_CHAN + "(" + _rpc_data.Datas[i].UNITS + ")";
                else
                {
                    //str_Header += seperator + "time" + seperator + _rpc_data.Datas[i].DESC_CHAN;

                    str_Header += seperator  + _rpc_data.Datas[i].DESC_CHAN + "(" + _rpc_data.Datas[i].UNITS + ")";
                }
            }
            sb.AppendLine(str_Header);

            for (i = 0; i < nRow; i++)
            {
                str_Header = "";
                str_Header = _rpc_data.Times[i].ToString("F6");

                for (j = 0; j < nColumn; j++)
                {
                    if (j == 0)
                        str_Header = _rpc_data.Times[i].ToString("F6") + seperator + _rpc_data.Datas[j].Export_Data[i].ToString(str_precision);
                    else
                    {
                        //str_Header += seperator + _rpc_data.Times[i].ToString("F6") + seperator + _rpc_data.Datas[i].Export_Data[j].ToString(str_precision);

                        str_Header += seperator + _rpc_data.Datas[j].Export_Data[i].ToString(str_precision);
                    }

                }

                sb.AppendLine(str_Header);
            }

            File.WriteAllText(_str_export_path, sb.ToString());


            return true;
        }

        #endregion


    }
}
