using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using Defect.Work;
using Defect.LCard;
using Signals;
using BankLib;
using UPAR;
using UPAR.Def;
using Protocol;
using UPAR.TS.TSDef;
using ResultLib.SG;
using SQL;
using Demagnetizer;

namespace Defect.SG
{
    public class JWorkSG : IJob, IDisposable
    {
        Bank bank;
        List<IJob> J = new List<IJob>();
        SignalListDef SL;
        uint Step = 0;
        bool started = false;

        SGSet sgSet = null;
        public SGState sgState = null;
        JDemagnetizer jDemagnetizer = null;
        bool restart;

        public JWorkSG(Bank _bank, SignalListDef _SL, ILCard502 _lcard, bool _restart, DOnStatus _OnStatus = null)
        {
            bank = _bank;
            SL = _SL;
            restart = _restart;
            OnStatus = _OnStatus;
            IsComplete = false;
            sgSet = new SGSet(_lcard, _SL, pr);
            SGSet.SaveParsToDB();
            J.Add(jDemagnetizer = new JDemagnetizer(ParAll.ST.Defect.Demagnetizer, ParAll.CTS.DemagnetizerTS, false));
        }
        public bool IsComplete { get; private set; }
        public void Start(int _tick)
        {
            started = true;
        }
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
            ProtocolST.pr("JWorkSG: " + _msg);
        }
        void prsl(uint _level, string _msg)
        {
            if (onStatus != null)
                onStatus(_level, _msg);
            pr(_msg);
        }
        int startTick;
        public void Exec(int _tick)
        {
            if (!started)
                return;
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
                    pr("Ждем трубу в ГП");
                    if(!restart)
                        jDemagnetizer.CheckSet();
                    Step = 1;
                    break;
                case 1:
                    if (!SL.iSGIN.Val)
                        break;
                    bank.Start(EUnit.SG, _tick);
                    jDemagnetizer.Start(_tick);
                    sgSet.StartGSPF();
                    pr("Ждем трубу в ГП2");
                    Step = 2;
                    break;
                case 2:
                    if (!SL.iSGOUT.Val)
                        break;
                    pr("Ждем выхода трубы из ГП");
                    Step = 3;
                    break;
                case 3:
                    if (SL.iSGOUT.Val)
                        break;
                    pr("Ждем 1 с после выхода трубы из ГП");
                    startTick = _tick;
                    Step = 4;
                    break;
                case 4:
                    if (_tick - startTick < 1000)
                        break;
                    Finish();
                    pr("Закончили");
                    Step = 4;
                    IsComplete = true;
                    break;
                case 5:
                    break;
            }
            if (IsError)
                Finish();
        }
        public void Dispose()
        {
            IsComplete = true;
            foreach (IJob job in J.OfType<IDisposable>())
                (job as IDisposable).Dispose();
            sgSet.Dispose();
        }
        public void Finish()
        {
            bank.SGLastData = true;
            IsComplete = true;
            foreach (IJob job in J)
                job.Finish();
            Dispose();
        }
        public bool CheckDTypeSize()
        {
            return (sgSet.CheckDTypeSize());
        }
    }
}
