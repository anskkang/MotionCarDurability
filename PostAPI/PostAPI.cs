using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VM;
using VM.Enums.Post;
using VM.Models;
using VM.Models.OutputReader;
using VM.Post.API.OutputReader;
using VM.Models.Post;

namespace PostAPI
{
    public class PostAPI
    {
        public PostAPI()
        {
        }

        public PostAPI(string filepath)
        {
            OutputReader = new OutputReader(filepath); 
        }

        private OutputReader OutputReader { get; set; }

        public IDictionary<string, IList<Point2D>> GetCurves(IPlotParameters parameters)
        {
            return this.OutputReader.GetCurves(parameters);
        }

        public IEnumerable<(BodyType, string)> GetBodies(BodyType type)
        {
            return this.OutputReader.GetBodies(type, true);
        }

        public IList<(ConnectorType, ActionType, string)> GetConnectors(string name)
        {
            return this.OutputReader.GetConnectors(name);
        }

        public IList<double[]> GetMarkerInfo(string name)
        {
            return this.OutputReader.GetMarkerInfo(name);
        }

        public Dictionary<string, KeyValuePair<string, double>> GetUnits()
        {
            return this.OutputReader.GetUnits();
        }

        public IDictionary<string, IVectorDisplayAnimatinoData> GetVector(string target, string path)
        {
            return this.OutputReader.GetVector(target, path);
        }

        public IDictionary<string, IVectorDisplayAnimatinoData> GetVector(string target, string path, AnalysisModelType analysisModelType = AnalysisModelType.Dynamics)
        {
            return this.OutputReader.GetVector(target, path, analysisModelType);
        }

        public (VM.Enums.Post.InterpolationErrorType, double[], double[]) InterpolationAkimaSpline(double[] X, double[] Y, int NoOfPnt,
            int NoOfDesiredPnt, double StartPnt, double EndPnt)
        {
            return this.OutputReader.InterpolationAkimaSpline(X, Y, NoOfPnt, NoOfDesiredPnt, StartPnt, EndPnt);
        }

        // for Modal
        public int GetModalModeCount(string target)
        {
            return this.OutputReader.GetModalModeCount(target);
        }

        public string Version
        {
            get { return this.OutputReader.Version; }
        }

        public AnalysisModelType GetPrimaryAnalysisType()
        {
            return this.OutputReader.GetPrimaryAnalysisType();
        }

        public void Close()
        {
            this.OutputReader.Close();
        }
    }
}
