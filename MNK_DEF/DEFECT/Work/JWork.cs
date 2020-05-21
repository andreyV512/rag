using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Share;
using ResultLib;
using BankLib;
using UPAR;
using Protocol;
using RectifierNS;
using Signals;
using Defect.SG;
using InverterNS;


namespace Defect.Work
{
    public class JWork : IJob, IDisposable
    {
        public DOnExec onExec;
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
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        public bool IsComplete { get; private set; }
        uint Step = 0;
        SignalListDef SL;
        List<IJob> J = new List<IJob>();
        Bank bank;

        JAlarmList jAlarm = null;
        JTransportStrobe jTransport = null;
        JCollect jCollectCross = null;
        JCollect jCollectLine = null;
        JResult jResult = null;
        JRectifierTh jRectifierThCross = null;
        JRectifierTh jRectifierThLine = null;
        JWorkSG jWorkSG = null;
        JThick jThick;
        JInverterTh jInverterTh = null;
        JBankResult jBankResult = null;



        cIW IW = null;

        int startTick;

        public JWork(cIW _IW, SignalListDef _SL, bool _restart, DOnExec _OnExec, DOnStatus _OnStatus = null)
        {
            IW = _IW;
            SL = _SL;
            onExec = _OnExec;
            OnStatus = _OnStatus;
            IsComplete = false;
            bank = new Bank(IW);

            J.Add(jAlarm = new JAlarmList());
            J.Add(jTransport = new JTransportStrobe(bank, SL, _IW));
            if (IW.Cross)
            {
                J.Add(jCollectCross = new JCollect(bank, EUnit.Cross, IW));
                J.Add(jRectifierThCross = new JRectifierTh(ParAll.ST.Defect.Cross.Rectifiers, ParAll.CTS.Cross.Rectifier, true, false));
            }
            else
            {
                if (IW.SG)
                    J.Add(jCollectCross = new JCollect(bank, EUnit.Cross, IW));
            }
            if (IW.Line)
            {
                J.Add(jCollectLine = new JCollect(bank, EUnit.Line, IW));
                J.Add(jRectifierThLine = new JRectifierTh(ParAll.ST.Defect.Line.Rectifiers, ParAll.CTS.Line.Rectifier, true, false));
                J.Add(jInverterTh = new JInverterTh(ParAll.ST.Defect.Line.ComPortConverters,
                ParAll.ST.Defect.Line.Converter,
                ParAll.ST.TSSet.Current.Line.Frequency));
            }
            if (IW.Thick)
            {
                J.Add(jThick = new JThick(bank));
            }
            if (IW.SG)
            {
                J.Add(jWorkSG = new JWorkSG(bank, SL, null, _restart));
            }
            J.Add(jResult = new JResult(bank, SL, ParAll.ST.Defect.Some.CheckZonePeriod));
            //            J.Add(jNewTube = new JNewTube(ParAll.ST.Defect.IsDBS));
            J.Add(jBankResult = new JBankResult(_IW, bank, _OnExec, _OnStatus));

        }
        public void Finish()
        {
            IsComplete = true;
            foreach (IJob job in J)
                job.Finish();
            Dispose();
            Send("VIEW");
            if (!IsError)
                prs("Режим РАБОТА завершен");
            else
                prs("Режим РАБОТА: Ошибка");
        }
        public void Dispose()
        {
            IsComplete = true;
            foreach (IJob job in J.OfType<IDisposable>())
                (job as IDisposable).Dispose();
            SL.oWORK2.Val = false;
            SL.oWORK3.Val = false;
        }
        void pr(string _msg)
        {
            ProtocolST.pr("JWork: " + _msg);
        }
        void prsl(uint _level, string _msg)
        {
            if (onStatus != null)
                onStatus(_level, _msg);
            pr(_msg);
        }
        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(0, _msg);
            pr(_msg);
        }
        void prst(string _msg)
        {
            prs("Режим РАБОТА: " + _msg);
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
                    SL.oRESULT.Val = false;
                    SL.oSTROBE.Val = false;
                    SL.oRESULT_COMMON.Val = false;
                    Finish();
                    return;
                }
            }

            switch (Step)
            {
                case 0:
                    prst("Ждем ЦЕПИ УПРАВЛЕНИЯ бесконечно");
                    Step = 1;
                    break;
                case 1:
                    if (!SL.iCC.Val)
                        break;
                    jAlarm.Add(SL.iCC, true);
                    prst("Ждем ЦИКЛ бесконечно");
                    Step = 2;
                    break;
                case 2:
                    if (!SL.iCYCLE.Val)
                        break;
                    jAlarm.Add(SL.iCYCLE, true);
                    prst("Ждем ГОТОВНОСТЬ бесконечно");
                    Step = 3;
                    break;
                case 3:
                    if (!SL.iREADY.Val)
                        break;
                    prst("Проверяем датчики");
                    if (!SGSet.SCheckDTypeSize(jCollectCross == null ? null : jCollectCross.lcard))
                    {
                        LastError = "Датчик группы прочности не соответствуют типоразмеру";
                        break;
                    }
                    prst("Подготавливаем устройства");
                    SL.oRESULT.Val = false;
                    SL.oRESULT_COMMON.Val = false;
                    SL.oSTROBE.Val = false;

                    if (IW.Cross)
                    {
                        jRectifierThCross.Start(0);
                    }
                    if (IW.Line)
                    {
                        jRectifierThLine.Start(0);
                        jInverterTh.Start(0);
                    }
                    if (IW.Thick)
                    {
                        jThick.Start(0);
                    }
                    startTick = _tick;
                    Step = 4;
                    break;
                case 4:
                    if (IW.Thick)
                    {
                        if (jThick.State != JThick.EState.Rotation)
                        {
                            if (_tick - startTick > ParAll.ST.Defect.Thick.RotationWait * 1000)
                                LastError = "Не дождались раскручивания толщиномера";
                            break;
                        }
                    }
                    if (IW.Cross)
                    {
                        UITH uith = jRectifierThCross.GetUITH();
                        pr(uith == null ? "uith==null" : uith.ToString());
                        if (uith == null || !uith.IsOk)
                        {
                            if (_tick - startTick > ParAll.ST.Defect.Cross.Rectifiers.MagnitWait * 1000)
                                LastError = "Не дождались магнитных полей поперечного";
                            break;
                        }
                    }
                    if (IW.Line)
                    {
                        UITH uith = jRectifierThLine.GetUITH();
                        if (uith == null || !uith.IsOk)
                        {
                            if (_tick - startTick > ParAll.ST.Defect.Line.Rectifiers.MagnitWait * 1000)
                                LastError = "Не дождались магнитных полей продольного";
                            break;
                        }
                    }
                    RK.ST.result = new Result();
                    if (IW.Thick)
                    {
                        RK.ST.result.Thick.MaxThickness = jThick.MaxThickness;
                        RK.ST.result.Thick.Border1 = jThick.Border1;
                        RK.ST.result.Thick.Border2 = jThick.Border2;
                    }
                    if (IW.Cross)
                    {
                        SL.oWORK2.Val = true;
                    }
                    if (IW.Line)
                    {
                        SL.oWORK3.Val = true;
                    }
                    if (IW.SG)
                    {
                        jWorkSG.Start(_tick);
                    }

                    prst("Ждем снятия ГОТОВНОСТЬ 10 с");
                    startTick = _tick;
                    Step = 5;
                    break;
                case 5:
                    if (_tick - startTick > 10000)
                    {
                        LastError = "Не дождались снятия готовности";
                        break;
                    }
                    bank.Start(_tick);
                    jTransport.Start(_tick);
                    jResult.Start(_tick);
                    if (IW.Thick)
                    {
                        jThick.Collect = true;
                    }
                    if (IW.Cross)
                    {
                        jCollectCross.Start(_tick);
                    }
                    if (IW.Line)
                    {
                        jCollectLine.Start(_tick);
                    }
                    jBankResult.Start(_tick);
                    prst("Ждем Контроль 20 с");
                    startTick = _tick;
                    Step = 6;
                    break;
                case 6:
                    if (_tick - startTick > 20000)
                    {
                        LastError = "Не дождались КОНТРОЛЬ за 20 с";
                        break;
                    }
                    if (!SL.iCONTROL1.Val && !SL.iCONTROL2.Val && !SL.iCONTROL3.Val)
                        break;
                    prst("Ждем получения всех зон");
                    Step = 7;
                    break;
                case 7:
                    pr(bank.Complete);
                    if (!bank.IsGaveZones)
                        break;
                    RK.ST.result.Compute();
                    List<EClass> LL = RK.ST.result.Sum.MClass;
                    for (int i = 0; i < LL.Count; i++)
                        bank.AddResultZone(i, Classer.ToBool(LL[i]));
                    pr("Отправили все результирующие зоны bank.NoWait = true");
                    bank.NoWait = true;
                    Send("DRAW");
                    jAlarm.Finish();
                    jTransport.Finish();
                    if (IW.Thick)
                    {
                        jThick.Finish();
                    }
                    if (IW.Cross)
                    {
                        jRectifierThCross.Finish();
                        jCollectCross.Finish();
                    }
                    if (IW.Line)
                    {
                        jRectifierThLine.Finish();
                        jCollectLine.Finish();
                        jInverterTh.Finish();
                    }
                    if (IW.SG)
                    {
                    }
                    pr("Ждем завершения банка");
                    jBankResult.Finish();
                    Step = 8;
                    break;
                case 8:
                    if (bank.Complete != null)
                    {
                        pr(bank.Complete);
                        break;
                    }
                    prst("Ждем отправки результатов");
                    Step = 9;
                    break;
                case 9:
                    if (!jResult.IsComplete)
                        break;
                    if (IW.SG)
                    {
                        if (!jWorkSG.IsComplete)
                            break;
                    }
                    jAlarm.Clear();
                    SL.oWORK2.Val = false;
                    SL.oWORK3.Val = false;
                    interrupt = true;
                    Send("RESTART");
                    prst("Ждем продолжения работы");
                    Step = 10;
                    break;
                case 10:
                    if (interrupt)
                        break;
                    bank.SetResultTube(RK.ST.result.Sum.RClass);
                    jResult.SendResult();
                    jResult.Finish();
                    //prst("Записываем в базу данных и в файл");
                    //RK.ST.result.Line.SaveBINDKB2_Msg();
                    RK.ST.result.TubeLength = bank.TubeLength == null ? 0 : Convert.ToDouble(bank.TubeLength.Value) / 1000;
                    RK.ST.result.SaveToDB();
                    Send("STATIST");
                    Finish();
                    IsComplete = true;
                    prst("Данные собраны");
                    break;
                default:
                    LastError = "Ошибка: Неизвестный шаг: " + Step.ToString();
                    break;
            }
            if (IsError)
                Finish();
        }
        void Send(string _msg)
        {
            if (onExec != null)
                onExec(_msg);
        }
        public void Start(int _tick) { }
        bool interrupt = false;
        public void ReStart()
        {
            interrupt = false;
        }
        public UITH StateHCross
        {
            get
            {
                if (IW.Cross)
                    return (jRectifierThCross.GetUITH());
                return (null);
            }
        }
        public UITH StateHLine
        {
            get
            {
                if (IW.Line)
                    return (jRectifierThLine.GetUITH());
                return (null);
            }
        }
    }
}
