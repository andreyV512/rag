using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Protocol;
using Share;
using SQL;
using BankLib;
using ResultLib;
using UPAR;

namespace Defect.Work
{
    class JThick : IJob, IDisposable
    {
        bool started = false;
        uint Step = 0;
        int startTickCount;
        public bool Collect=false;
        int last_index = 0;
        Bank bank;
        public double MaxThickness = 16;
        public double Border1 = 0;
        public double Border2 = 0;

        public JThick(Bank _bank)
        {
            bank = _bank;
            State = EState.None;
        }
        public void Dispose() { SendCmd(EThickCommand.Reset); }
        public bool IsComplete { get; private set; }
        public void Start(int _tick)
        {
            started = true;
        }
        public void Finish() { IsComplete = true; Dispose();  }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus { set { onStatus = value; } }
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(1, _msg);
        }
        void pr(string _msg)
        {
            ProtocolST.pr("JThick: " + _msg);
        }
        bool SendCmd(EThickCommand _cmd)
        {
            ExecSQL E=new ExecSQL(string.Format("update ThickWork set cmd='{0}'",_cmd.ToString()));
            if (E.RowsAffected != 1)
            {
                LastError = "Не могу записать команду для Толщиномера в СУБД";
                return (false);
            }
            pr("SendCmd: " + _cmd.ToString());
            return (true);
        }
        EThickState ReceiveState()
        {
            Select S = new Select("select state from ThickWork");
            if(!S.Read())
            {
                S.Dispose();
                LastError = "Не могу прочитать состояние Толщиномера из СУБД";
                return (EThickState.None);
            }
            string s=S[0] as string;
            S.Dispose();

            EThickState ret;
            if (Enum.TryParse<EThickState>(s, out ret))
            {
                if (ret == EThickState.Error)
                {
                    LastError = "Ошибка толщиномера";
                }
                pr("ReceiveState: " + ret.ToString());
                return (ret);
            }
            else
            {
                pr("ReceiveState: " + ret.ToString());
                return (EThickState.None);
            }
        }
        void SendTypeSize()
        {
            ExecSQL E = new ExecSQL(string.Format("update ThickWork set TypeSize='{0}'", ParAll.CTS.Name));
            if (E.RowsAffected != 1)
                LastError = "Не могу записать типоразмер для Толщиномера в СУБД";
        }
        void GetPars()
        {
            Select S = new Select("select MaxThickness, Border1, Border2 from ThickWork");
            if (!S.Read())
            {
                S.Dispose();
                LastError = "Не могу прочитать состояние Толщиномера из СУБД";
                return;
            }
            object o=S[0];
            if (!(o is DBNull))
                MaxThickness = Convert.ToDouble(o);
            o = S[1];
            if (!(o is DBNull))
                Border1 = Convert.ToDouble(o);
            o = S[2];
            if (!(o is DBNull))
                Border2 = Convert.ToDouble(o);
            S.Dispose();
        }
        void GetZones()
        {
            Select S = new Select(string.Format("select * from ThickZones where Zone > {0} order by Zone",
                last_index.ToString()));
            while (S.Read())
            {
                BankZoneThick z = new BankZoneThick();
                z.index = Convert.ToInt32(S["Zone"]);
                z.Length = Convert.ToInt32(S["Length"]);
                object o = S["RLevel"];
                if (o is DBNull)
                    z.Level = null; 
                else
                    z.Level = Convert.ToDouble(S["RLevel"]);
                z.RClass=Classer.FromChar(Convert.ToChar(S["Class"]));
                z.last = Convert.ToBoolean(S["Last"]);
                pr(z.ToString());
                bank.AddThickZone(z);
                last_index = z.index;
                if (z.last)
                {
                    pr("Закончили работу на последней зоне");
                    IsComplete = true;
                }
            }
            S.Dispose();
        }
        public void Exec(int _tick)
        {
            if (!started || IsComplete || IsError)
                return;
            switch (Step)
            {
                case 0:
                    pr("Обнуляем толщиномер");
                    SendCmd(EThickCommand.Reset);
                    startTickCount = _tick;
                    Step = 1;
                    break;
                case 1:
                    if (ReceiveState() != EThickState.On)
                        break;
                    pr("Ждем готовности толщиномера");
                    SendTypeSize();
                    SendCmd(EThickCommand.Ready);
                    startTickCount = _tick;
                    Step = 2;
                    break;
                case 2:
                    if (ReceiveState() != EThickState.Ready)
                        break;
                    GetPars();
                    pr("Ждем вращения толщиномера");
                    
                    SendCmd(EThickCommand.Rotate);
                    startTickCount = _tick;
                    Step = 3;
                    break;
                case 3:
                    if (ReceiveState() != EThickState.Rotate)
                        break;
                    State = EState.Rotation;
                    pr("Ждем команды на сбор толщиномера");
                    Step = 4;
                    break;
                case 4:
                    if (!Collect)
                        break;
                    pr("Ждем сбора толщиномера");
                    SendCmd(EThickCommand.Collect);
                    startTickCount = _tick;
                    Step = 5;
                    break;
                case 5:
                    if (ReceiveState() != EThickState.Collect)
                        break;
                    State = EState.Collect;
                    pr("Ждем окончания сбора толщиномера");
                    Step = 6;
                    break;
                case 6:
                    GetZones();
                    if (ReceiveState() != EThickState.Complete)
                        break;
                    State = EState.Complete;
                    pr("Закончили работу");
                    IsComplete = true;
                    break;
            }
            if (IsError)
                Dispose();
        }
        public enum EState { None, Rotation, Collect, Complete }
        public EState State { get; private set; }
    }
}
