using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motion.Durability
{
    public enum Category
    {
        Bodies = 0,
        Forces = 1,
        UserDefinedFunctions = 2,
        FEBodies = 3

    }

    //public enum AnalysisScenario
    //{
    //    Dynamics = 0,
    //    Static = 1
    //}
    public enum ConnectionTypeForBody
    {
        force = 0,
        constraints = 1,
        motion = 2
    }

    public enum ReferenceFrameOfMotion
    {
        vehiclebody = 0,
        global = 1
    }

    public enum BaseOrActionForce
    {
        Base = 0,
        Action = 1
    }

    public enum ForceTypeofForce
    {
        TSpringDamper = 0,
        Bush = 1,
        Tire = 2
    }

    public enum ResultValueType
    {
        Original = 0,
        Transform = 1,
        FixedStep = 2
    }

    public enum FileFormat
    {
        RPC = 0,
        CSV = 1,
        MCF = 2, 
        Static = 3
    }

    public class EntityForBody
    {
        public EntityForBody()
        {
            Initialize();
        }
        public EntityForBody(string _name, ConnectionTypeForBody _type, bool _rotation_flag)
        {
            str_name = _name;
            enum_typeForBody = _type;
            b_rotation_flag = _rotation_flag;

            Initialize();

        }

        private string str_name;
        private double[] m_dUnitScale;
        private ConnectionTypeForBody enum_typeForBody;
        private bool b_rotation_flag;
        private BaseOrActionForce enum_appliedForcetype;
        private ReferenceFrameOfMotion enum_rf_type;

        private List<double[]> lst_oriValue;
        private List<double[]> lst_fixedstepValue;
        private List<double[]> lst_transValue;
        private List<double[]> lst_position;
        private List<double[]> lst_orientation;
        private List<string> lst_result_name;
        private List<double> lst_MaxValue;

        public string Name
        {
            get { return str_name; }
            set { str_name = value; }

        }

        public double[] UnitScaleFactor
        {
            get { return m_dUnitScale; }
            set { m_dUnitScale = value; }
        }

        public ConnectionTypeForBody ConnectionType
        {
            get { return enum_typeForBody; }
            set { enum_typeForBody = value; }
        }

        public ReferenceFrameOfMotion ReferenceFrame
        {
            get { return enum_rf_type; }
            set { enum_rf_type = value; }
        }
        public bool UseRotationFlag
        {
            get { return b_rotation_flag; }
            set { b_rotation_flag = value; }
        }

        public BaseOrActionForce AppliedForceType
        {
            get { return enum_appliedForcetype; }
            set { enum_appliedForcetype = value; }
        }

        public string Unit1 { get; set; }

        public string Unit2 { get; set; }

        public List<double> MaxValues
        {
            get { return lst_MaxValue; }
            set { lst_MaxValue = value; }
        }

        public List<double[]> OrinalValue
        {
            get { return lst_oriValue; }
            set { lst_oriValue = value; }
        }

        public List<double[]> FixedStepValue
        {
            get { return lst_fixedstepValue; }
            set { lst_fixedstepValue = value; }
        }

        public List<double[]> TransformValue
        {
            get { return lst_transValue; }
            set { lst_transValue = value; }
        }

        public List<double[]> RF_Positions
        {
            get { return lst_position; }
            set { lst_position = value; }
        }

        public List<double[]> RF_Orientations
        {
            get { return lst_orientation; }
            set { lst_orientation = value; }
        }

        public List<string> ResultNames
        {
            get { return lst_result_name; }
            set { lst_result_name = value; }
        }

        private void Initialize()
        {
            lst_oriValue = new List<double[]>();
            lst_fixedstepValue = new List<double[]>();
            lst_transValue = new List<double[]>();
            lst_position = new List<double[]>();
            lst_orientation = new List<double[]>();
            lst_result_name = new List<string>();
            m_dUnitScale = new double[2];
            lst_MaxValue = new List<double>();
        }

    }

    public class Body
    {
        public Body()
        {
            Initialize();
        }
        public Body(string _name)
        {
            name = _name;
            Initialize();
        }

        private string name;
        private List<EntityForBody> lst_entity;
        private List<double[]> lst_position;
        private List<double[]> lst_orientation;
        private List<double[]> m_body_Tvel;
        private List<double[]> m_body_Rvel;
        private List<double[]> m_body_Tacc;
        private List<double[]> m_boody_Racc;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        

        public List<EntityForBody> Entities
        {
            get { return lst_entity; }
            set { lst_entity = value; }
        }

        public List<double[]> RF_Positions
        {
            get { return lst_position; }
            set { lst_position = value; }
        }

        public List<double[]> RF_Orientations
        {
            get { return lst_orientation; }
            set { lst_orientation = value; }
        }

        public List<double[]> TranslationalVelocity
        {
            get { return m_body_Tvel; }
            set { m_body_Tvel = value; }
        }

        public List<double[]> RotationalVelocity
        {
            get { return m_body_Rvel; }
            set { m_body_Rvel = value; }
        }

        public List<double[]> TranslationalAcceleration
        {
            get { return m_body_Tacc; }
            set { m_body_Tacc = value; }
        }

        public List<double[]> RotationalAcceleration
        {
            get { return m_boody_Racc; }
            set { m_boody_Racc = value; }
        }

        void Initialize()
        {
            lst_entity = new List<EntityForBody>();
            lst_position = new List<double[]>();
            lst_orientation = new List<double[]>();
            m_body_Tvel = new List<double[]>();
            m_body_Rvel = new List<double[]>();
            m_body_Tacc = new List<double[]>();
            m_boody_Racc = new List<double[]>();
        }
    }

    public class EntityForForce
    {
        public EntityForForce()
        {
            Initialize();
        }

        public EntityForForce(string _name)
        {
            name = _name;

            Initialize();
        }

        private string name;
        private double[] m_dUnitScale;
        
        private ReferenceFrameOfMotion enum_rf_type;

        private List<double[]> lst_oriValue;
        private List<double[]> lst_fixedstepValue;
        private List<double[]> lst_transValue;
        private List<string> lst_result_name;
        private List<double> lst_MaxValue;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Unit1 { get; set; }
        public string Unit2 { get; set; }

        public double[] UnitScaleFactor
        {
            get { return m_dUnitScale; }
            set { m_dUnitScale = value; }
        }

        public ReferenceFrameOfMotion ReferenceFrame
        {
            get { return enum_rf_type; }
            set { enum_rf_type = value; }
        }

        public List<string> ResultNames
        {
            get { return lst_result_name; }
            set { lst_result_name = value; }
        }

        public List<double> MaxValues
        {
            get { return lst_MaxValue; }
            set { lst_MaxValue = value; }
        }

        public List<double[]> OrinalValue
        {
            get { return lst_oriValue; }
            set { lst_oriValue = value; }
        }

        public List<double[]> FixedStepValue
        {
            get { return lst_fixedstepValue; }
            set { lst_fixedstepValue = value; }
        }

        public List<double[]> TransformValue
        {
            get { return lst_transValue; }
            set { lst_transValue = value; }
        }

        void Initialize()
        {
            lst_oriValue = new List<double[]>();
            lst_fixedstepValue = new List<double[]>();
            lst_transValue = new List<double[]>();
            lst_result_name = new List<string>();
            m_dUnitScale = new double[2];
            lst_MaxValue = new List<double>();
        }

    }

    public class Force
    {
        public Force()
        {
            lst_entity = new List<EntityForForce>();
            lst_Base_position = new List<double[]>();
            lst_Base_orientation = new List<double[]>();
            lst_Action_position = new List<double[]>();
            lst_Action_orientation = new List<double[]>();
            BaseBody = "";
            ActionBody = "";

        }
        public Force(string _name)
        {
            name = _name;
            lst_entity = new List<EntityForForce>();
            lst_Base_position = new List<double[]>();
            lst_Base_orientation = new List<double[]>();
            lst_Action_position = new List<double[]>();
            lst_Action_orientation = new List<double[]>();
            BaseBody = "";
            ActionBody = "";
        }

        private string name;
        private List<EntityForForce> lst_entity;
        private ForceTypeofForce typeofForce;
        private List<double[]> lst_Base_position;
        private List<double[]> lst_Base_orientation;
        private List<double[]> lst_Action_position;
        private List<double[]> lst_Action_orientation;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<EntityForForce> Entities
        {
            get { return lst_entity; }
            set { lst_entity = value; }
        }

        public ForceTypeofForce TypeofForce
        {
            get { return typeofForce; }
            set { typeofForce = value; }
        }

        public List<double[]> Base_Positions
        {
            get { return lst_Base_position; }
            set { lst_Base_position = value; }
        }

        public List<double[]> Base_Orientations
        {
            get { return lst_Base_orientation; }
            set { lst_Base_orientation = value; }
        }

        public List<double[]> Action_Positions
        {
            get { return lst_Action_position; }
            set { lst_Action_position = value; }
        }

        public List<double[]> Action_Orientations
        {
            get { return lst_Action_orientation; }
            set { lst_Action_orientation = value; }
        }

        public string BaseBody { get; set; }

        public string ActionBody { get; set; }


    }

    public class FEBody
    {
        public FEBody()
        {
            m_lstOriginStep_MC = new List<double[]>();
            m_lstFixedStep_MC = new List<double[]>();
        }

        private string m_strName;
        private List<double[]> m_lstOriginStep_MC;
        private List<double[]> m_lstFixedStep_MC;

        public string Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        public List<double[]> OriginalTime_Modal_Coordinates
        {
            get { return m_lstOriginStep_MC; }
            set { m_lstOriginStep_MC = value; }
        }

        public List<double[]> FixedTime_Modal_Coordinates
        {
            get { return m_lstFixedStep_MC; }
            set { m_lstFixedStep_MC = value; }
        }

        

        public int NumofMode { get; set; }

    }

    public class DurabilityData
    {
        public DurabilityData()
        {
            Initialize();

            m_scale_force = 1.0;
            m_scale_displacement = 1.0;
            m_scale_angle = 1.0;
            m_scale_time = 1.0;
            Precision = "e6";
        }
        //public DurabilityData(Category _type, double _d_unit_force, double _d_unit_displacement, double _d_unit_angle, double _d_unit_time, double _dStepsize)
        //{
        //    type = _type;
        //    m_unit_force = _d_unit_force;
        //    m_unit_displacement = _d_unit_displacement;
        //    m_unit_angle = _d_unit_angle;
        //    m_unit_time = _d_unit_time;
        //    dStepsize = _dStepsize;
        //    Initialize();
        //}

        private Category type;
        private double m_scale_force;
        private double m_scale_displacement;
        private double m_scale_angle;
        private double m_scale_time;

        private string m_unit_force;
        private string m_unit_displacement;
        private string m_unit_angle;
        private string m_unit_time;

        private double dStepsize;
        private double m_endTime;
        private double m_endTime_mod;
        private int m_nResultStep;
        private int m_nNumofResult;

        private Body m_Body;
        private List<FEBody> m_lstFEBody;
        private List<Force> m_lstForce;
        private List<double> m_oriTime;
        private List<double> m_fixedTime;
        private List<double[]> m_chassis_pos;
        private List<double[]> m_chassis_ori;
        private List<double[]> m_chassis_Tvel;
        private List<double[]> m_chassis_Rvel;
        private List<double[]> m_chassis_Tacc;
        private List<double[]> m_chassis_Racc;
        private bool m_bIsChassis;

        #region Properties
        public Category Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Precision { get; set; }

        public double Full_Scale { get; set; }

        public string Version { get; set; }

        public string Unit_Force
        {
            get { return m_unit_force; }
            set { m_unit_force = value; }
        }

        public string Unit_Length
        {
            get { return m_unit_displacement; }
            set { m_unit_displacement = value; }
        }

        public string Unit_Angle
        {
            get { return m_unit_angle; }
            set { m_unit_angle = value; }
        }

        public string Unit_Time
        {
            get { return m_unit_time; }
            set { m_unit_time = value; }
        }

        public double Scale_Force
        {
            get { return m_scale_force; }
            set { m_scale_force = value; }
        }

        public double Scale_Length
        {
            get { return m_scale_displacement; }
            set { m_scale_displacement = value; }
        }

        public double Scale_Angle
        {
            get { return m_scale_angle; }
            set { m_scale_angle = value; }
        }

        public double Scale_Time
        {
            get { return m_scale_time; }
            set { m_scale_time = value; }
        }

        public double StepSize
        {
            get { return dStepsize; }
            set { dStepsize = value; }
        }

        public double EndTime
        {
            get { return m_endTime; }
            set { m_endTime = value; }
        }

        public double EndTime_Modify
        {
            get { return m_endTime_mod; }
            set { m_endTime_mod = value; }
        }

        public int ResultStep
        {
            get { return m_nResultStep; }
            set { m_nResultStep = value; }
        }

        public int NumOfResult
        {
            get 
            {
                Calculate_Num_Of_Result();
                return m_nNumofResult; 
            }
        }

        public Body Body
        {
            get { return m_Body; }
            set { m_Body = value; }
        }

        public List<FEBody> FEBodies
        {
            get { return m_lstFEBody; }
            set { m_lstFEBody = value; }
        }

        public List<Force> Forces
        {
            get { return m_lstForce; }
            set { m_lstForce = value; }
        }

        public List<double> OriginalTimes
        {
            get { return m_oriTime; }
            set { m_oriTime = value; }
        }

        public List<double> FixedTimes
        {
            get { return m_fixedTime; }
            set { m_fixedTime = value; }
        }

        public List<double[]> PositionOfChassis
        {
            get { return m_chassis_pos; }
            set { m_chassis_pos = value; }
        }

        public List<double[]> OrientationOfChassis
        {
            get { return m_chassis_ori; }
            set { m_chassis_ori = value; }
        }

        public List<double[]> TranslationalVelocity
        {
            get { return m_chassis_Tvel; }
            set { m_chassis_Tvel = value; }
        }

        public List<double[]> RotationalVelocity
        {
            get { return m_chassis_Rvel; }
            set { m_chassis_Rvel = value; }
        }

        public List<double[]> TranslationalAcceleration
        {
            get { return m_chassis_Tacc; }
            set { m_chassis_Tacc = value; }
        }

        public List<double[]> RotationalAcceleration
        {
            get { return m_chassis_Racc; }
            set { m_chassis_Racc = value; }
        }

        public bool ExistChassis
        {
            get { return m_bIsChassis; }
            set { m_bIsChassis = value; }
        }

        #endregion

        #region Member functions

        void Initialize()
        {
            m_Body = new Body();
            m_lstFEBody = new List<FEBody>();
            m_lstForce = new List<Force>();
            m_oriTime = new List<double>();
            m_fixedTime = new List<double>();
            m_chassis_pos = new List<double[]>();
            m_chassis_ori = new List<double[]>();
            m_bIsChassis = false;
            m_chassis_Tvel = new List<double[]>();
            m_chassis_Rvel = new List<double[]>();
            m_chassis_Tacc = new List<double[]>();
            m_chassis_Racc = new List<double[]>();

            m_nNumofResult = 0;

            Full_Scale = 32752.0;
        }

        void Calculate_Num_Of_Result()
        {
            int nCount ;

            if(Type == Category.Bodies)
            {
                nCount = 0;
                foreach (EntityForBody entity in m_Body.Entities)
                {
                    nCount = nCount + entity.FixedStepValue[0].Length;
                }

                m_nNumofResult = nCount;
            }
            else if(Type == Category.Forces)
            {
                nCount = 0;
                foreach (Force force_data in m_lstForce)
                {
                    foreach(EntityForForce entity in force_data.Entities)
                    {
                        if(entity.FixedStepValue.Count != 0)
                            nCount = nCount + entity.FixedStepValue[0].Length;
                    }
                }

                m_nNumofResult = nCount;
            }
            else if(Type == Category.UserDefinedFunctions)
            {
                m_nNumofResult = 0;
            }
            else
            {
                m_nNumofResult = 0;
            }
        }

        #endregion
    }


    public class StaticResult
    {
        public StaticResult()
        {
            m_lstResultFile = new List<string>();
            m_lstForceName = new List<string>();
            m_lstData = new List<double[]>();
        }

        List<string> m_lstResultFile;
        List<string> m_lstForceName;
        List<double[]> m_lstData;


        public List<string> ResultFiles
        {
            get { return m_lstResultFile; }
            set { m_lstResultFile = value; }
        }

        public List<string> ForceNames
        {
            get { return m_lstForceName; }
            set { m_lstForceName = value; }
        }

        public List<double[]> StaticData
        {
            get { return m_lstData; }
            set { m_lstData = value; }
        }


    }
}
