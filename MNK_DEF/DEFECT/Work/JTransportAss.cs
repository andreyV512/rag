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
    public class JTransportAss : IJob
    {
        bool complete = false;
        Bank bank;
        SignalListDef SL;
        int StartTick = 0;
        bool started = false;
        double? Speed = null;
        int? TubeLength = null;
        TickPosition lastTickPosition = null;
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        int tp_index = 0;
        List<TESignal> LTE = new List<TESignal>();
        int ZoneSize = ParAll.ST.ZoneSize;
        uint Step = 0;
        cIW IW;
        double waitPeriod;
        DimensionsPars Pars;

        TESignal teCONTROL1On = null;
        TESignal teCONTROL2On = null;
        TESignal teCONTROL3On = null;
        TESignal teSGOutOn = null;

        TESignal teCONTROL1Off = null;
        TESignal teCONTROL2Off = null;

        public JTransportAss(Bank _bank, SignalListDef _SL, cIW _IW)
        {
            IW = _IW;
            bank = _bank;
            SL = _SL;
            Pars = ParAll.ST.Dimensions;

            if (_IW.Thick) LTE.Add(teCONTROL1On = new TESignal(SL.iCONTROL1, true, Pars.Stand1));
            LTE.Add(teCONTROL2On = new TESignal(SL.iCONTROL2, true, Pars.Stand2));
            LTE.Add(teCONTROL3On = new TESignal(SL.iCONTROL3, true, Pars.Stand3));
            LTE.Add(teSGOutOn = new TESignal(SL.iSGOUT, true, Pars.SGOut));

            if (_IW.Thick) LTE.Add(teCONTROL1Off = new TESignal(SL.iCONTROL1, false, Pars.Stand2));
            LTE.Add(teCONTROL2Off = new TESignal(SL.iCONTROL2, false, Pars.Stand3));

            SL.CatchClear();
            SL.CatchAdd(SL.iCONTROL1);
            SL.CatchAdd(SL.iCONTROL2);
            SL.CatchAdd(SL.iCONTROL3);
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
            started = true;
            SL.CatchStart();
            Step = 0;
        }
        double? CalcSpeed()
        {
            if (!teCONTROL3On.Was || !teCONTROL2On.Was)
                return (null);
            double Sv = teCONTROL3On.Position - teCONTROL2On.Position;
            double Tv = teCONTROL3On.Tick - teCONTROL2On.Tick;
            double V = Sv / Tv;
            pr("Sv=" + Sv.ToString());
            pr("Tv=" + Tv.ToString());
            pr("V=" + V.ToString());
            return (V);
        }
        int? CalcTubeLength(TESignal _teSignal)
        {
            if (Speed == null)
                return (null);
            double T = _teSignal.Tick - teSGOutOn.Tick;
            double S = Speed.Value * T;

            int delta = Convert.ToInt32(Math.Round(S));
            int tubeLength = teSGOutOn.Position - _teSignal.Position + delta;
            pr("T=" + T.ToString());
            pr("S=" + S.ToString());
            pr("delta=" + delta.ToString());
            pr("tubeLength=" + tubeLength.ToString());
            return (tubeLength);
        }
        void Send(TickPosition _tp)
        {
            bank.AddTickPosition(_tp);
            ExecSQL E = new ExecSQL(string.Format("insert into TickPositions values({0},{1},{2})",
                tp_index++.ToString(),
                Convert.ToInt32(Math.Round(_tp.tick)).ToString(),
                _tp.position.ToString()));
            lastTickPosition = _tp;
        }
        void SendTubeLength()
        {
            bank.TubeLength = TubeLength.Value;
            ExecSQL E = new ExecSQL(string.Format("update ThickWork set TubeLength = {0}",
                TubeLength.Value.ToString()));
        }
        public void Exec(int _tick)
        {
            if (!started)
                return;
            if (complete)
                return;
            SignalEvent se;
            switch (Step)
            {
                case 0:
                    if (!IW.Thick)
                    {
                        Step = 1;
                        break;
                    }
                    se = SL.CatchNext();
                    if (se == null)
                        break;
                    pr(se.ToString());
                    if (!teCONTROL1On.Check(se))
                        break;
                    Send(new TickPosition(teCONTROL1On.Tick, teCONTROL1On.Position));
                    Step = 1;
                    break;
                case 1:
                    se = SL.CatchNext();
                    if (se == null)
                        break;
                    pr(se.ToString());
                    if (!teCONTROL2On.Check(se))
                        break;
                    Send(new TickPosition(teCONTROL2On.Tick, teCONTROL2On.Position));
                    Step = 2;
                    break;
                case 2:
                    se = SL.CatchNext();
                    if (se == null)
                        break;
                    pr(se.ToString());
                    if (!teCONTROL3On.Check(se))
                        break;
                    Send(new TickPosition(teCONTROL3On.Tick, teCONTROL3On.Position));
                    Speed = CalcSpeed();
                    if (IW.Thick)
                        waitPeriod = (Pars.Stand2 - Pars.Stand1) / Speed.Value;
                    else
                        waitPeriod = (Pars.Stand3 - Pars.Stand2) / Speed.Value;
                    Step = 3;
                    break;
                case 3:
                    for (; ; )
                    {
                        TickPosition nextTickPosition = new TickPosition(lastTickPosition);
                        nextTickPosition.tick += ZoneSize / Speed.Value;
                        nextTickPosition.position += ZoneSize;
                        if (nextTickPosition.tick >= _tick - waitPeriod)
                            break;
                        Send(nextTickPosition);
                    }
                    se = SL.CatchNext();
                    if (se == null)
                        break;
                    pr(se.ToString());
                    if (!teSGOutOn.Check(se))
                        break;
                    Step = 4;
                    break;
                case 4:
                    for (; ; )
                    {
                        TickPosition nextTickPosition = new TickPosition(lastTickPosition);
                        nextTickPosition.tick += ZoneSize / Speed.Value;
                        nextTickPosition.position += ZoneSize;
                        if (nextTickPosition.tick >= _tick - waitPeriod)
                            break;
                        Send(nextTickPosition);
                    }
                    se = SL.CatchNext();
                    if (se == null)
                        break;
                    pr(se.ToString());
                    if (IW.Thick)
                    {
                        if (!teCONTROL1Off.Check(se))
                            break;
                        TubeLength = CalcTubeLength(teCONTROL1Off);
                        SendTubeLength();
                    }
                    else
                    {
                        if (!teCONTROL2Off.Check(se))
                            break;
                        TubeLength = CalcTubeLength(teCONTROL2Off);
                        SendTubeLength();
                    }
                    for (; ; )
                    {
                        TickPosition nextTickPosition = new TickPosition(lastTickPosition);
                        nextTickPosition.tick += ZoneSize / Speed.Value;
                        nextTickPosition.position += ZoneSize;
                        if (nextTickPosition.position > Pars.Stand4 + TubeLength.Value)
                            break;
                        Send(nextTickPosition);
                    }
                    Finish();
                    break;
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
            ProtocolST.pr("JTransportAss: " + _msg);
        }
    }
}
