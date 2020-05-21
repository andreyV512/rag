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
    public class JTransportStrobe : IJob
    {
        bool complete = false;
        Bank bank;
        SignalListDef SL;
        int StartTick = 0;
        int step = 0;
        bool started = false;
        int tp_index = 0;

        TESignal teOn;
        TESignal teOff0;
        TEStrobe teStrobe;

        List<TickPosition> L = new List<TickPosition>();
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }

        public JTransportStrobe(Bank _bank, SignalListDef _SL, cIW _IW)
        {
            bank = _bank;
            SL = _SL;
            DimensionsPars Pars = ParAll.ST.Dimensions;

            if (_IW.Thick)
            {
                teOn = new TESignal(SL.iCONTROL1, true, Pars.Stand1);
                teOff0 = new TESignal(SL.iCONTROL1, false, Pars.Stand2);
            }
            else if (_IW.Cross)
            {
                teOn = new TESignal(SL.iCONTROL2, true, Pars.Stand2);
                teOff0 = new TESignal(SL.iCONTROL2, false, Pars.Stand3);
            }
            else if (_IW.Line)
            {
                teOn = new TESignal(SL.iCONTROL3, true, Pars.Stand3);
                teOff0 = new TESignal(SL.iCONTROL3, false, Pars.Stand4);
            }

            teStrobe = new TEStrobe(SL.iSTROBE, true, teOn.Position, ParAll.ST.ZoneSize);
            SL.CatchClear();
            SL.CatchAdd(SL.iCONTROL1);
            SL.CatchAdd(SL.iCONTROL2);
            SL.CatchAdd(SL.iCONTROL3);
            SL.CatchAdd(SL.iSTROBE);

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
                switch (step)
                {

                    case 0:
                        // Ждем начала
                        if (!teOn.Check(se))
                            break;
                        pr(teOn.ToString());
                        L.Add(new TickPosition(teOn.Tick, teOn.Position));
                        teStrobe.FirstTick = teOn.Tick;
                        step = 2;
                        break;
                    case 2:
                        // Собираем стробы
                        if (teStrobe.Check(se))
                        {
                            pr(teStrobe.ToString());
                            L.Add(new TickPosition(teStrobe.Tick,
                                teStrobe.Position));
                        }
                        if (!teOff0.Was)
                        {
                            if (teOff0.Check(se))
                            {
                                // Если снялся контроль - вычисляем длину трубы.
                                pr(teOff0.ToString());
                                {
                                    int curr_position = teStrobe.Position + CalcDelta(teOff0.Tick);
                                    //                            int TubeLength = curr_position - teOff0.Position;
                                    int TubeLength = curr_position - teOff0.Position;
                                    pr("curr_position=" + curr_position.ToString());
                                    pr("teOn.Position=" + teOn.Position.ToString());
                                    pr("teOff0.Position=" + teOff0.Position.ToString());
                                    pr("TubeLength=" + TubeLength.ToString());
                                    bank.TubeLength = TubeLength;
                                    SendTubeLength(TubeLength);
                                    L.Add(new TickPosition(teOff0.Tick, teOff0.Position + TubeLength));
                                }
                            }
                        }
                        break;
                }
                //foreach (TickPosition tp in L)
                //{
                //    Send(tp);
                //    bank.AddTickPosition(tp);
                //}
                //L.Clear();

                if (teOff0.Was)
                {
                    // Если Знаем конец трубы - сбрасываем зоны в банк
                    foreach (TickPosition tp in L)
                    {
                        Send(tp);
                        bank.AddTickPosition(tp);
                    }
                    L.Clear();
                }
                else
                {
                    // Если НЕ знаем конец трубы - сбрасываем в банк только те стробы,
                    // после которых уже пришли стробы на длину, не менее длины
                    // между первым и последним контролем.
                    //
                    // Иначе говоря зедержим тот строб, которым может оказаться за границей трубы, чтобы не выдать зону из воздуха.
                    if (L.Count != 0)
                    {
                        double last_position = L[L.Count - 1].position;
                        for (; ; )
                        {
                            TickPosition p = L[0];
                            if (last_position - p.position >= teOff0.Position -
                                teOn.Position)
                            {
                                Send(p);
                                bank.AddTickPosition(p);
                                L.Remove(p);
                            }
                            else
                            {
                                //							AnsiString a = "";
                                //							a += last_position;
                                //							a += "-";
                                //							a += p->position;
                                //							a+=">";
                                //							a+=teOff0->Position();
                                //							a+="-";
                                //							a+=teOn->Position();
                                //							pr(a);
                                break;
                            }
                            if (L.Count == 0)
                                break;
                        }

                    }

                }
            }
        }
        int CalcDelta(int _tick)
        {
            double V = teStrobe.Speed;
            double T = _tick - teStrobe.Tick;
            int S = (int)(V * T);
            pr("Delta: V=" + V.ToString());
            pr("Delta: T=" + T.ToString());
            pr("Delta: S=" + S.ToString());
            return (S);
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
            ProtocolST.pr("JTransportStrobe: " + _msg);
        }
        void Send(TickPosition _tp)
        {
            ExecSQL E = new ExecSQL(string.Format("insert into TickPositions values({0},{1},{2})",
                tp_index++.ToString(),
                Convert.ToInt32(Math.Round(_tp.tick)).ToString(),
                _tp.position.ToString()));
        }
        void SendTubeLength(int _TubeLength)
        {
            ExecSQL E = new ExecSQL(string.Format("update ThickWork set TubeLength = {0}",
                _TubeLength.ToString()));
        }
    }
}
