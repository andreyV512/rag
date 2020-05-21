using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using UPAR;
using BankLib;
using ResultLib;
using Protocol;
using Defect.SG;


namespace Defect.Work
{
    public class JBankResult : IJob, IDisposable
    {
        cIW IW;
        Bank bank;
        DOnExec OnExec;
        DOnStatus onStatus = null;


        public JBankResult(cIW _IW, Bank _bank, DOnExec _OnExec, DOnStatus _OnStatus)
        {
            IW = _IW;
            bank = _bank;
            OnExec = _OnExec;
            OnStatus = _OnStatus;
            IsComplete = true;
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

        public void Dispose() { IsComplete = true; }
        public bool IsComplete { get; private set; }
        public void Start(int _tick) { IsComplete = false; }
        public void Finish() { IsComplete = true; }
        public DOnStatus OnStatus { set { onStatus = value; } }
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        public void Exec(int _tick)
        {
            if (IsComplete)
                return;
            bool was = false;
            if (IW.Cross)
            {
                for (BankZoneDataA z = bank.GetNextZoneCross(); z != null; z = bank.GetNextZoneCross())
                {
                    was = true;
                    RK.ST.result.Cross.AddZoneA(bank.CrossData, z);
                }
            }
            if (IW.Line)
            {
                for (BankZoneDataA z = bank.GetNextZoneLine(); z != null; z = bank.GetNextZoneLine())
                {
                    was = true;
                    RK.ST.result.Line.AddZoneA(bank.LineData, z);
                }
            }
            if (IW.Thick)
            {
                for (BankZoneThick z = bank.GetNextZoneThick(); z != null; z = bank.GetNextZoneThick())
                {
                    was = true;
                    RK.ST.result.Thick.MZone.Add(z);
                }
            }
            if (IW.SG)
            {
                BankZoneData z = bank.GetNextZoneSG();
                if (z != null)
                {
                    was = true;
                    string lret;
                    RK.ST.result.SG.sgState = SGSet.SaveToDb(bank.SGData, z.size, out lret);
                    if (lret != null)
                        prsl(1, lret);
                }
            }
            if (was)
            {
                Result result = RK.ST.result;
                result.Sum.Compute(result.Cross, result.Line, result.Thick);
                List<EClass> L = result.Sum.MClass;
                for (int i = 0; i < L.Count; i++)
                    bank.AddResultZone(i, Classer.ToBool(L[i]));
                Send("DRAW");
            }

        }
        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(1, _msg);
        }
        void Send(string _msg)
        {
            if (OnExec != null)
                OnExec(_msg);
        }
    }
}
