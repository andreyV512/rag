using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BankLib;
using Defect.LCard;
using UPAR;
using UPAR.Def;
using Protocol;
using Share;

namespace Defect.Work
{
    class JCollect : IJob, IDisposable
    {
        Bank bank;
        public ILCard502 lcard;
        int ReadPeriod;
        bool started;
        public string LastError { get; private set; }
        int LastExec;
        EUnit Tp;
        cIW IW;

        public JCollect(Bank _bank, EUnit _Tp, cIW _IW)
        {
            bank = _bank;
            IW = _IW;
            Tp = _Tp;
#if LCARD_VIRTUAL
            lcard = new L502virtual();
#else
            if(Tp==EUnit.Cross)
                lcard = new LCard502(ParAll.ST.Defect.Cross.L502, pr);
            else if(Tp==EUnit.Line)
                lcard = new LCard502E(ParAll.ST.Defect.Line.L502, pr);
#endif
            started = false;
            LastError = null;
            LastExec = Environment.TickCount;
        }
        public void Dispose()
        {
            lcard.Dispose();
        }
        public bool IsComplete { get { return (!started); } }
        public void Finish()
        {
            Finish(Environment.TickCount);
        }
        public void Finish(int _tick)
        {
            if (!started)
                return;
            started = false;
            Exec(_tick);
            lcard.Stop();
        }
        public DOnStatus OnStatus { get; set; }
        void pr(string _msg)
        {
            ProtocolST.pr(_msg);
        }

        public void Start(int _startTick)
        {
            if (bank == null)
                return;
            if (started)
                Finish(_startTick);

            DefCL dcl = new DefCL(Tp);
            ReadPeriod = dcl.L502.ReadPeriod;
            List<L502Ch> L = new List<L502Ch>();
            if (Tp == EUnit.Line)
            {
                for (int i = 0; i < dcl.LCh.Count; i++)
                    L.Add(dcl.LCh[i]);
            }
            if (Tp == EUnit.Cross)
            {
                if (IW.Cross)
                {
                    for (int i = 0; i < dcl.LCh.Count; i++)
                        L.Add(dcl.LCh[i]);
                }
                if (IW.SG)
                {
                    L.Add(ParAll.SG.Sensor_I);
                    L.Add(ParAll.SG.Sensor_B);
                }
            }
            lcard.Start(dcl.L502, L.ToArray());
            started = true;
            bank.FirstTick = _startTick;
        }
        // ---------------------------------------------------------------------------
        public void Exec(int _tick)
        {
            if (bank == null)
                return;
            if (!started)
                return;
            //if (_tick - LastExec < ReadPeriod)
            //    return;
            LastExec = _tick;
            //	pr(AnsiString("ExecTick=")+_tick);
            double[] buf = lcard.Read();
            if (buf == null)
            {
                LastError = "не смогли прочитать";
                Finish();
                return;
            }
            bank.AddGroup(Tp, buf);
        }
        public double GetValueV(L502Ch _channel, ref bool _ret)
        {
            if (started)
                throw (new Exception("JCollect.GetValueV: Нельзя получать значения стартовавшего сборщика"));
            DefCL dcl = new DefCL(Tp);
            return (lcard.GetValueV(_channel, dcl.L502, ref _ret));
        }

        // ---------------------------------------------------------------------------
        public double GetValueP(L502Ch _channel, ref bool _ret)
        {
            if (started)
                throw (new Exception("JCollect.GetValueP: Нельзя получать значения стартовавшего сборщика"));
            DefCL dcl = new DefCL(Tp);
            return (lcard.GetValueP(_channel, dcl.L502, ref _ret));
        }
        public bool IsError { get { return (LastError != null); } }
    }
}
