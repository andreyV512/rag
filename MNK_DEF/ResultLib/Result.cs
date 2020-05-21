using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using Protocol;
using PARLIB;
using UPAR;
using UPAR.TS;
using UPAR.Def;
using ResultLib.Def;
using ResultLib.Thick;
using ResultLib.SG;
using Share;
using SQL;
using BankLib;

namespace ResultLib
{
    public delegate void DOnProtocol(string _msg);
    public class Result
    {
        public string user = null;
        public string ts_name = null;
        public ResultDef Cross;
        public ResultDef Line;
        public ResultThickLite Thick;
        public ResultSG SG;
        public SumResult Sum;
        public string Genesis = null;
        public bool FromFile = false;
        public string Fname { get; private set; }
        public int IdTube = 0;
        public double TubeLength = 0;

        public Result()
        {
            Error = null;
            Thick = new ResultThickLite();
            Cross = new ResultDef(EUnit.Cross);
            Line = new ResultDef(EUnit.Line);
            SG = new ResultSG();
            Sum = new SumResult();
        }
        //public Result(string _fname, EUnit _tp, int _version = 0)
        //    : this()
        //{
        //    FromFile = true;
        //    Fname = _fname;
        //    Genesis = "Из файла: " + _fname;
        //    ProtocolST.pr(user);
        //    switch (_tp)
        //    {
        //        case EUnit.Cross:
        //            Cross.LoadBINDKB2(_fname);
        //            Compute();
        //            break;
        //        case EUnit.Line:
        //            Line.LoadBINDKB2(_fname);
        //            Compute();
        //            break;
        //    }
        //}
        public void Compute()
        {
            Cross.Compute();
            Line.Compute();
            Sum.Compute(Cross,Line,Thick);
        }
        public string Error { get; private set; }
        public static DialogResult OpenDialog(string _head, string _filter, ref string _fname)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = _head;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = _filter;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            DialogResult ret = openFileDialog1.ShowDialog();
            if (ret == DialogResult.OK)
                _fname = openFileDialog1.FileName;
            return (ret);
        }
        public static DialogResult SaveDialog(string _head, string _filter, ref string _fname)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = _head;
            saveFileDialog.FileName = "";
            saveFileDialog.Filter = _filter;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            DialogResult ret = saveFileDialog.ShowDialog();
            if (ret == DialogResult.OK)
                _fname = saveFileDialog.FileName;
            return (ret);
        }
        static public IResultPars DeSerialize(Type _tp, FileStream _stream)
        {
            return (new XmlSerializer(_tp).Deserialize(_stream) as IResultPars);
        }
        public void SaveToDB()
        {
            Execute Ex = new Execute("dbo.ClearTubes");
            Ex.Input("@MaxSize", ParAll.ST.Some.MaxDBTubes);
            Ex.Exec();
            string SQL =
"insert into dbo.Tubes (" +
" TypeSize," + // 0
" Zones," +	// 1
" Length," +	// 2
" RUser," +	// 3
" Client," +	// 4
" Result," +	// 5
" SolidGroup," + // 6
" Cut1," + // 7
" Cut2," + // 8
" CrossResult," + // 9
" LineResult," + // 10
" ThickResult," + //11
" MinThickness)" + // 12
"values(";

            SQL += AddToSQLS(ParAll.CTS.Name) + ",";
            SQL += AddToSQLI(Sum.MClass.Count) + ",";
            SQL += AddToSQLD(TubeLength) + ",";
            SQL += AddToSQLS(User.current.Name) + ",";
            SQL += AddToSQLS(ParAll.ST.Clients.Current.Name) + ",";
            SQL += AddToSQLC(Classer.ToChar(Sum.RClass)) + ",";
            SQL += AddToSQLS(SG.sgState == null ? null : SG.sgState.Group) + ",";
            SQL += "NULL,";
            SQL += "NULL,";
            SQL += AddToSQLS(CrossResult()) + ",";
            SQL += AddToSQLS(LineResult()) + ",";
            SQL += AddToSQLS(ThickResult()) + ",";
            SQL += AddToSQLD(Thick.MinThickness) + ")";
            ExecSQL E = new ExecSQL(SQL);

        }
        string AddToSQLS(string _par)
        {
            if (_par == null)
                return ("NULL");
            return ("'" + _par + "'");
        }
        string AddToSQLI(int _par)
        {
            return (_par.ToString());
        }
        string AddToSQLD(double? _par)
        {
            if (_par == null)
                return ("NULL");
            return (_par.Value.ToString("F3").Replace(',', '.'));
        }
        string AddToSQLC(char _par)
        {
            return ("'" + _par + "'");
        }
        string CrossResult()
        {
            if (Cross.MZone == null || Cross.MZone.Count == 0)
                return (null);
            string ret = "";
            foreach (Zone z in Cross.MZone)
                ret += Classer.ToChar(z.Class);
            return (ret);
        }
        string LineResult()
        {
            if (Line.MZone == null || Line.MZone.Count == 0)
                return (null);
            string ret = "";
            foreach (Zone z in Line.MZone)
                ret += Classer.ToChar(z.Class);
            return (ret);
        }
        string ThickResult()
        {
            if (Thick.MZone == null || Thick.MZone.Count == 0)
                return (null);
            string ret = "";
            foreach (BankZoneThick z in Thick.MZone)
                ret += Classer.ToChar(z.RClass);
            return (ret);
        }
    }
}
