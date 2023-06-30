using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motion.Durability.RPCReader
{
    public enum RPC_FORMAT
    {
        BINARY_IEEE_LITTLE_END = 0,
        BINARY_IEEE_BIG_END = 1,
        BINARY = 2,
        ASCII = 3
    }

    public enum RPC_FILE_TYPE
    {
        TIME_HISTRY = 0,
        CONFIGURATION = 1,
        MATRIX = 2,
        FATIGUE = 3,
        ROAD_SURFACE = 4,
        SPECTRAL = 5,
        START = 6
    }

    public enum BYPASS_FILTER
    {
        Off = 0,
        On = 1
    }

    public enum DATA_TYPE
    {
        SHORT_INTEGER = 0,
        FLOATING_POINT = 1
    }

    public enum TIME_TYPE
    {
        DRIVE = 0,
        RESPONSE = 1,
        MULT_DRIVE = 2,
        MULT_RESP = 3,
        CONFIG_DRIVE = 4,
        CONFIG_RESP = 5,
        PEAK_PICK = 6
    }

    public class RPCReader
    {
        public RPCReader(string _name)
        {
            m_name = _name;
            m_time = new List<double>();
            m_int_full_scale = 32752;
            m_datatype = DATA_TYPE.SHORT_INTEGER;

            m_numheaderblocks = 0;
            m_num_params = 0;
            m_delta_t = 0.0;
            m_pts_per_frame = 0;
            m_pts_per_group = 0;
            m_frames = 0;
            m_channels = 0;
            m_half_frames = 0;
            m_repeats = 0;

            m_data = new List<RPCData>();
        }

        #region Variables
        string m_name;
        RPC_FORMAT m_format;
        RPC_FILE_TYPE m_filetype;
        BYPASS_FILTER m_bypassfilter;
        DATA_TYPE m_datatype;
        TIME_TYPE m_timetype;
        Int32 m_numheaderblocks;
        Int32 m_num_params;
        double m_delta_t;
        Int32 m_pts_per_frame;
        Int32 m_pts_per_group;
        Int32 m_channels;
        Int32 m_frames;
        Int32 m_half_frames;
        Int32 m_repeats;
        List<double> m_time;
        Int16 m_int_full_scale;
        string m_operation;
        List<RPCData> m_data;

        #endregion

        #region Properties
        public string Name
        { 
            get { return m_name; }
            set { m_name = value; }
        }

        public RPC_FORMAT Format
        {
            get { return m_format; }
            set { m_format = value; }
        }

        public RPC_FILE_TYPE File_Type
        {
            get { return m_filetype; }
            set { m_filetype = value; }
        }

        public BYPASS_FILTER Bypass_Filter
        {
            get { return m_bypassfilter; }
            set { m_bypassfilter = value; }
        }

        public DATA_TYPE Data_Type
        {
            get { return m_datatype; }
            set { m_datatype = value; }
        }

        public TIME_TYPE Time_Type
        {
            get { return m_timetype; }
            set { m_timetype = value; }
        }

        public Int32 Num_Header_Blocks
        {
            get { return m_numheaderblocks; }
            set { m_numheaderblocks = value; }
        }

        public Int32 Num_Params
        {
            get { return m_num_params; }
            set { m_num_params = value; }
        }

        public double Delta_T
        {
            get { return m_delta_t; }
            set {m_delta_t = value; }
        }

        public Int32 Pts_Per_Frame
        {
            get { return m_pts_per_frame; }
            set { m_pts_per_frame = value; }
        }

        public Int32 Pts_Per_Group
        {
            get { return m_pts_per_group; }
            set { m_pts_per_group = value; }
        }

        public Int32 Channels
        {
            get { return m_channels; }
            set { m_channels = value; }
        }

        public Int32 Frames
        {
            get { return m_frames; }
            set { m_frames = value; }
        }

        public Int32 Half_Frames
        {
            get { return m_half_frames; }
            set { m_half_frames = value; }
        }

        public Int32 Repeats
        {
            get { return m_repeats; }
            set { m_repeats = value; }
        }

        public List<double> Times
        {
            get { return m_time; }
            set { m_time = value; }
        }

        public List<RPCData> Datas
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public short INT_FULL_SCALE
        {
            get { return m_int_full_scale; }
        }

        public string OPERATION
        {
            get { return m_operation; }
            set { m_operation = value; }
        }

        

        #endregion

        #region Functions


        #endregion
    }

    public class RPCData
    {
        public RPCData()
        {
            m_Ori_INT_data = new List<short>();
            m_data = new List<double>();
        }

        string m_desc_chan;
        string m_units;
        double m_scale;
        double m_up_limit;
        double m_low_limit;
        Int32 m_map;

        List<short> m_Ori_INT_data;
        List<double> m_data;

        public string DESC_CHAN
        {
            get { return m_desc_chan; }
            set { m_desc_chan = value; }
        }

        public string UNITS
        {
            get { return m_units; }
            set { m_units = value; }
        }

        public double SCALE_CHAN
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        public double UPPER_LIMIT
        {
            get { return m_up_limit; }
            set { m_up_limit = value; }
        }

        public double LOWER_LIMIT
        {
            get { return m_low_limit; }
            set { m_low_limit = value; }
        }

        public Int32 MAP_CHAN
        {
            get { return m_map; }
            set { m_map = value; }
        }

        public List<short> Orifinal_Data
        {
            get { return m_Ori_INT_data; }
            set { m_Ori_INT_data = value; }
        }

        public List<double> Export_Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

    }

    
}
