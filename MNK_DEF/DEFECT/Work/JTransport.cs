using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using UPAR;
using BankLib;
using Protocol;
using Signals;
using SQL;

namespace Defect.Work
{
    public class JTransport : IJob
    {
        bool complete = false;
        Bank bank;
        SignalListDef SL;
        int StartTick = 0;
        int step = 0;
        bool started = false;

        TESignal teCONTROL2On;
        TESignal teCONTROL2Off;
        TESignal teCONTROL3On;
        TESignal teSGInOn;
        TESignal teSGOutOn;


        List<TickPosition> L = new List<TickPosition>();
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        int tp_index = 0;
        int? TubeLength = null;
        List<TESignal> LTE = new List<TESignal>();


        public JTransport(Bank _bank, SignalListDef _SL, cIW _IW)
        {
            bank = _bank;
            SL = _SL;
            DimensionsPars Pars = ParAll.ST.Dimensions;

            if (_IW.Thick) LTE.Add(new TESignal(SL.iCONTROL1, true, Pars.Stand1));
            LTE.Add(teCONTROL2On = new TESignal(SL.iCONTROL2, true, Pars.Stand2));
            LTE.Add(teCONTROL3On = new TESignal(SL.iCONTROL3, true, Pars.Stand3));
            LTE.Add(teSGInOn = new TESignal(SL.iSGIN, true, Pars.SGIn));
            LTE.Add(teSGOutOn = new TESignal(SL.iSGOUT, true, Pars.SGOut));

            if (_IW.Thick) LTE.Add(new TESignal(SL.iCONTROL1, false, Pars.Stand2));
            LTE.Add(teCONTROL2Off = new TESignal(SL.iCONTROL2, false, Pars.Stand3));
            LTE.Add(new TESignal(SL.iCONTROL3, false, Pars.Stand4));
            LTE.Add(new TESignal(SL.iSGIN, false, Pars.SGIn));
            LTE.Add(new TESignal(SL.iSGOUT, false, Pars.SGOut));

            SL.CatchClear();
            SL.CatchAdd(SL.iCONTROL1);
            SL.CatchAdd(SL.iCONTROL2);
            SL.CatchAdd(SL.iCONTROL3);
            SL.CatchAdd(SL.iSGIN);
            SL.CatchAdd(SL.iSGOUT);


            new ExecSQL("update ThickWork set TubeLength = null");
            new ExecSQL("delete from TickPositions");

        }
        public void Start(int _tick)
        {
            if (started)
                return;
            StartTick = _tick;
            complete = false;
            step = 0;
            started = true;
            SL.CatchStart();
        }
        int CalcTubeLength(TESignal _teSignal)
        {
            double Sv = teSGOutOn.Position - teSGInOn.Position;
            double Tv = teSGOutOn.Tick - teSGInOn.Tick;
            double V = Sv / Tv;

            double T = _teSignal.Tick - teSGOutOn.Tick;
            double S = V * T;
            int delta = Convert.ToInt32(Math.Round(S));
            int tubeLength = teSGOutOn.Position - _teSignal.Position + delta;
            pr("Sv=" + Sv.ToString());
            pr("Tv=" + Tv.ToString());
            pr("V=" + V.ToString());
            pr("T=" + T.ToString());
            pr("S=" + S.ToString());
            pr("delta=" + delta.ToString());
            pr("tubeLength=" + tubeLength.ToString());
            return (tubeLength);
        }
        int CalcTubeLength2(TESignal _teSignal)
        {
            double Sv = teSGOutOn.Position - teCONTROL3On.Position;
            double Tv = teSGOutOn.Tick - teCONTROL3On.Tick;
            double V = Sv / Tv;

            double T = _teSignal.Tick - teSGOutOn.Tick;
            double S = V * T;
            int delta = Convert.ToInt32(Math.Round(S));
            int tubeLength = teSGOutOn.Position + delta;
            pr("2 Sv=" + Sv.ToString());
            pr("2 Tv=" + Tv.ToString());
            pr("2 V=" + V.ToString());
            pr("2 T=" + T.ToString());
            pr("2 S=" + S.ToString());
            pr("2 delta=" + delta.ToString());
            pr("2 tubeLength=" + tubeLength.ToString());
            return (tubeLength);
        }
        int CalcTubeLength3()
        {
            double Sv = teCONTROL3On.Position - teCONTROL2On.Position;
            double Tv = teCONTROL3On.Tick - teCONTROL2On.Tick;
            double V = Sv / Tv;


            double T = teCONTROL2Off.Tick - teCONTROL3On.Tick;
            double S = V * T;
            int delta = Convert.ToInt32(Math.Round(S));
            int tubeLength = delta;
            pr("3 Sv=" + Sv.ToString());
            pr("3 Tv=" + Tv.ToString());
            pr("3 V=" + V.ToString());
            pr("3 T=" + T.ToString());
            pr("3 S=" + S.ToString());
            pr("3 delta=" + delta.ToString());
            pr("3 tubeLength=" + tubeLength.ToString());
            return (tubeLength);
        }

        public void Exec(int _tick)
        {
            if (!started)
                return;
            if (complete)
                return;
            for (; ; )
            {
                SignalEvent se = SL.CatchNext();
                if (se == null)
                    break;
                pr(se.ToString());
                TESignal te = LTE[0];
                if (!te.Check(se))
                    continue;
                pr(te.ToString());
                if (te.need)
                    L.Add(new TickPosition(te.Tick, te.Position));
                else
                {
                    if (TubeLength == null)
                    {
                        TubeLength = CalcTubeLength(te);
                        CalcTubeLength2(te);
                        new ExecSQL("update ThickWork set TubeLength = " + TubeLength.Value.ToString());
                        bank.TubeLength = TubeLength;
                    }
                    L.Add(new TickPosition(te.Tick, te.Position + TubeLength.Value));
                }
                LTE.RemoveAt(0);
                if (LTE.Count == 0)
                {
                    complete = true;
                    break;
                }
                foreach (TickPosition tp in L)
                {
                    pr(tp.ToString());
                    bank.AddTickPosition(tp);
                    ExecSQL E = new ExecSQL(string.Format("insert into TickPositions values({0},{1},{2})",
                        tp_index++.ToString(),
                        tp.tick.ToString(),
                        tp.position.ToString()));

                }
                L.Clear();
            }
        }
        public bool IsComplete
        {
            get
            {
                return (complete);
            }
        }
        public void Finish()
        {
            complete = true;
            SL.CatchStop();
        }
        public DOnStatus OnStatus { get; set; }
        void pr(string _msg)
        {
            ProtocolST.pr("JTransport: " + _msg);
        }
    }
}
