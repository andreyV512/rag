using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Share;
using UPAR;
using UPAR.TS.TSDef;
using UPAR.Def;
using UPAR.SG;
using Defect.LCard;
using Defect.Work;
using Defect.GSPF052PCI;
using SQL;

namespace Defect.SG
{
    public class SGSet : IDisposable
    {
        ILCard502 lcard = null;
        GSPF gspf = null;
        DOnPr OnPr = null;
        SignalListDef SL;
        bool lcard_local;

        public SGSet(ILCard502 _lcard, SignalListDef _SL, DOnPr _OnPr = null)
        {
            lcard_local = false;
            lcard = _lcard;
            gspf = new GSPF(OnPr);
            SL = _SL;
        }
        public SGSet(SignalListDef _SL, DOnPr _OnPr = null)
        {
            lcard_local = true;
            OnPr = _OnPr;
            lcard = LoadLCard(_OnPr);
            gspf = new GSPF(OnPr);
            SL = _SL;
        }
        static ILCard502 LoadLCard(DOnPr _OnPr = null)
        {
            ILCard502 ret;
#if LCARD_VIRTUAL
            ret = new L502virtual();
#else
            ret = new LCard502(ParAll.ST.Defect.Cross.L502, _OnPr);
#endif
            return (ret);
        }
        public void Dispose()
        {
            if (lcard != null)
            {
                lcard.Dispose();
                lcard = null;
            }
            if (gspf != null)
            {
                gspf.Dispose();
                gspf = null;
            }
        }
        public string StartGSPF(SGWorkPars _tsDefSG = null)
        {
            if (gspf == null)
                return (null);
            if (_tsDefSG == null)
                _tsDefSG = new SGWorkPars();
            return (gspf.Start(_tsDefSG));
        }
        public void StopGSPF()
        {
            if (gspf == null)
                return;
            gspf.Stop();
        }
        public void StartL502(L502Ch[] _channels)
        {
            if (!lcard_local)
                return;
            if (lcard == null)
                return;
            DefCL dcl = new DefCL(EUnit.Cross);
            lcard.Start(dcl.L502, _channels);
        }
        public void StopL502()
        {
            if (!lcard_local)
                return;
            if (lcard == null)
                return;
            lcard.Stop();
        }
        public double[] ReadL502()
        {
            if (!lcard_local)
                return (null);
            if (lcard == null)
                return (null);
            return (lcard.Read());
        }
        public string Start(SGWorkPars _tsDefSG, L502Ch[] _channels)
        {
            string ret = StartGSPF(_tsDefSG);
            StartL502(_channels);
            return (ret);
        }
        public void Stop()
        {
            StopL502();
            StopGSPF();
        }
        public bool CheckDTypeSize()
        {
            return (SCheckDTypeSize(lcard));
        }
        public static bool SCheckDTypeSize(ILCard502 _lcard = null)
        {
            if (_lcard != null)
                return (CheckDTypeSize0(_lcard));
            ILCard502 l = LoadLCard();
            bool ret = CheckDTypeSize0(l);
            l.Dispose();
            return (ret);
        }
        static bool CheckDTypeSize0(ILCard502 _lcard)
        {
            Thread.Sleep(50);
            if (!CheckDTypeSize0(ParAll.CTS.Cross.SG.Sensors.D0, ParAll.SG.Sensor_D0, _lcard))
                return false;
            if (!CheckDTypeSize0(ParAll.CTS.Cross.SG.Sensors.D1, ParAll.SG.Sensor_D1, _lcard))
                return false;
            if (!CheckDTypeSize0(ParAll.CTS.Cross.SG.Sensors.D2, ParAll.SG.Sensor_D2, _lcard))
                return false;
            return true;
        }
        static bool CheckDTypeSize0(bool _need, L502Ch _channel, ILCard502 _lcard)
        {
            if (_lcard == null)
                return (false);
            bool ret = false;
            double LevelD = ParAll.SG.LevelD;
            double v = _lcard.GetValueV(_channel, ParAll.ST.Defect.Cross.L502, ref ret);
            if (!ret)
                return false;
            if (_need)
            {
                if (v >= LevelD)
                    return (true);
            }
            else
            {
                if (v < LevelD)
                    return (true);
            }
            return (false);
        }
        public static void SaveParsToDB()
        {
            SGPars p = ParAll.SG.sgPars;
            new ExecSQL(string.Format("update Uran.SGPars set veracity={0},period={1},period_dif={2},borders={3},borders1={4},algorithm='{5}',pro='{6}',tubeCount={7},ValUI='{8}',FullPeriod='{9}'",
                p.Veracity.ToString(),
                p.HalfPeriod.ToString(),
                p.HalfPeriodDif.ToString(),
                p.BorderBegin.ToString(),
                p.BorderEnd.ToString(),
                p.AlgorithmPoints,
                p.AlgorithmSG,
                p.TubeCount.ToString(),
                p.ValIU.ToString(),
                p.FullPeriod ? "true" : "false"));
        }
        public static SGState SaveToDb(double[] _data, int _size, out string _ret)
        {
            _ret = null;
            if (_data == null || _size <= 0)
                return (null);
            MIU miu = new MIU(_size, _data);
            miu.ZeroI();
            if (miu.Img == null)
                return (null);
            Execute E = new Execute("Uran.AddGetSGTube");
            E.Input("@typeSize", ParAll.CTS.Name);
            E.Input("@img", miu.Img);
            E.OutputString("@group", 50);
            E.OutputDouble("@probability");
            E.OutputInt("@color");

            int ret = E.Exec();
            if (ret != 1)
            {
                _ret = ("SG::SaveGetSG: Не удалось добавить трубу");
                return (null);
            }
            return (new SGState() { Group = E.AsString("@group"), Metric = E.AsDouble("@probability"), DBColor = E.AsInt("@color") });
        }
    }
}
