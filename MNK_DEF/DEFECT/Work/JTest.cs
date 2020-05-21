using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using ResultLib;
using BankLib;
using UPAR;
using Protocol;
using RectifierNS;
using Signals;

namespace Defect.Work
{
    class JTest : IJob, IDisposable
    {
        SignalListDef SL = null;
        uint Step = 0;
        List<IJob> J = new List<IJob>();
        Bank bank;
        JAlarmList jAlarm;
        JCollect jCollectCross=null;
        JCollect jCollectLine=null;
        JTransportTest jTransportTest;
        JRectifierTh jRectifierThCross=null;
        JRectifierTh jRectifierThLine = null;
        public DOnExec onExec;
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        int startTick;
        cIW IW;

        public JTest(SignalListDef _SL, DOnExec _onExec, DOnStatus _OnStatus = null)
        {
            SL = _SL;
            onExec = _onExec;
            prst("Режим ТЕСТ");
            IsComplete = false;
            RK.ST.result = new Result();
            IW = new cIW(true);
            IW.Thick = false;
            IW.SG = false;
            bank = new Bank(IW);

            J.Add(jAlarm = new JAlarmList());
            if (IW.Cross)
                J.Add(jCollectCross = new JCollect(bank, EUnit.Cross,IW));
            if (IW.Line)
                J.Add(jCollectLine = new JCollect(bank, EUnit.Line, IW));
            J.Add(jTransportTest = new JTransportTest(bank));
            if (ParAll.ST.Defect.Some.TestWithMagnit)
            {
                J.Add(jRectifierThCross = new JRectifierTh(ParAll.ST.Defect.Cross.Rectifiers, ParAll.CTS.Cross.Rectifier, true, false));
                J.Add(jRectifierThLine = new JRectifierTh(ParAll.ST.Defect.Line.Rectifiers, ParAll.CTS.Line.Rectifier, true, false));
            }

            OnStatus = _OnStatus;
        }
        public void Dispose()
        {
            IsComplete = true;
            foreach (IJob job in J.OfType<IDisposable>())
                (job as IDisposable).Dispose();
        }
        public void Exec(int _tick)
        {
            if (IsComplete)
                return;
            if (IsError)
                return;
            foreach (IJob job in J)
            {
                job.Exec(_tick);
                if (job.IsError)
                {
                    LastError = job.LastError;
                    Finish();
                    return;
                }
            }
            switch (Step)
            {
                case 0:
                    Send("CLEAR");
                    Send("SETRESULT");
                    jAlarm.Add(SL.iCC, true);
                    if (!ParAll.ST.Defect.Some.TestWithMagnit)
                    {
                        Step = 101;
                        break;
                    }
                    prst("Ждем магнитные поля ");
                    if(jRectifierThCross!=null)
                        jRectifierThCross.Start(0);
                    if(jRectifierThLine!=null)
                        jRectifierThLine.Start(0);
                    startTick = _tick;
                    Step = 100;
                    break;
                case 100:
                    if (jRectifierThCross != null)
                    {
                        if (_tick - startTick > ParAll.ST.Defect.Cross.Rectifiers.MagnitWait * 1000)
                        {
                            LastError = "Не дождались магнитных полей поперечного";
                            break;
                        }
                        UITH uith = jRectifierThCross.GetUITH();
                        if (uith == null || !uith.IsOk)
                            break;
                    }
                    if (jRectifierThLine != null)
                    {
                        if (_tick - startTick > ParAll.ST.Defect.Line.Rectifiers.MagnitWait * 1000)
                        {
                            LastError = "Не дождались магнитных полей продольного";
                            break;
                        }
                        UITH uith = jRectifierThLine.GetUITH();
                        if (uith == null || !uith.IsOk)
                            break;
                    }
                    Step = 101;
                    break;
                case 101:
                    startTick = Environment.TickCount;
                    bank.Start(startTick);
                    jTransportTest.Start(startTick);
                    if(jCollectCross!=null)
                        jCollectCross.Start(startTick);
                    if (jCollectLine != null)
                        jCollectLine.Start(startTick);
                    prst("Ждем завершения Банка");
                    Step = 1;
                    break;
                case 1:
                    bool was=false;
                    BankZoneDataA z = bank.GetNextZoneCross();
                    if (z != null)
                    {
                        was=true;
                        RK.ST.result.Cross.AddZoneA(bank.CrossData, z);
                    }
                    z = bank.GetNextZoneLine();
                    if (z != null)
                    {
                        was = true;
                        RK.ST.result.Line.AddZoneA(bank.LineData, z);
                    }
                    if(was)
                    {
                        RK.ST.result.Sum.Compute(RK.ST.result.Cross, RK.ST.result.Line, null);
                        List<EClass> L = RK.ST.result.Sum.MClass;
                        for (int i = 0; i < L.Count; i++)
                            bank.AddResultZone(i, Classer.ToBool(L[i]));
                        Send("DRAW");
                        if (was)
                            break;
                    }
                    for (; ; )
                    {
                        if (bank.GetNextResultZone() == null)
                            break;
                    }
                    if (!bank.IsGaveZones)
                        break;
                    bank.NoWait = true;
                    for (; ; )
                    {
                        if (bank.GetNextResultZone() == null)
                            break;
                    }
                    RK.ST.result.Compute();
                    List<EClass> LL = RK.ST.result.Sum.MClass;
                    for (int i = 0; i < LL.Count; i++)
                        bank.AddResultZone(i, Classer.ToBool(LL[i]));
                    Send("DRAW");
                    if (bank.Complete != null)
                    {
                        pr(bank.Complete);
                        break;
                    }
                    Step = 1000;
                    break;
                case 1000:
                    IsComplete = true;
                    prst("работа выполнена");
                    Finish();
                    break;
                default:
                    LastError = "Неизвестный шаг";
                    break;
            }
            if (IsError)
                Finish();
        }
        public bool IsComplete { get; private set; }
        public void Finish()
        {
            IsComplete = true;
            foreach (IJob job in J)
                job.Finish();
            Dispose();
            Send("VIEW");
            if (!IsError)
                prs("Режим ТЕСТ завершен");
            else
                prs("Режим ТЕСТ: Ошибка");
        }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus
        {
            set
            {
                onStatus = value;
                foreach (IJob job in J)
                    job.OnStatus = value;
            }
        }
        void prst(string _msg)
        {
            prs("Режим ТЕСТ: " + _msg);
        }
        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(0, _msg);
            pr(_msg);
        }
        void pr(string _msg)
        {
            ProtocolST.pr("JTestThick: " + _msg);
        }
        public void Start(int _tick) { }
        void Send(string _msg)
        {
            if (onExec != null)
                onExec(_msg);
        }
        public UITH StateHCross
        {
            get
            {
                if (IW.Cross)
                {
                    if (jRectifierThCross == null)
                        return (null);
                    return (jRectifierThCross.GetUITH());
                }
                return (null);
            }
        }
        public UITH StateHLine
        {
            get
            {
                if (IW.Line)
                {
                    if (jRectifierThLine == null)
                        return (null);
                    return (jRectifierThLine.GetUITH());
                }
                return (null);
            }
        }
    }
}
