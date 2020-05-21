using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Share;
using Protocol;
using BankLib;

namespace Defect.Work
{
    public class JResult : IJob, IDisposable
    {
        Bank bank;
        SignalListDef SL;
        int period;
        Thread th = null;
        volatile bool terminate = false;
        volatile bool complete = false;

        public JResult(Bank _bank, SignalListDef _SL, int _period)
        {
            bank = _bank;
            SL = _SL;
            period = _period;
        }
        public void Dispose()
        {
            if (th == null)
                return;
            terminate = true;
            th.Join();
            th = null;
        }
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        public bool IsComplete { get { return (complete); } }
        public void Finish()
        {
            Dispose();
            complete = true;
        }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus { set { onStatus = value; } }
        //        bool started = false;

        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(1, _msg);
        }
        public void Exec(int _tick) { }
        public void Start(int _tick)
        {
            th = new Thread(new ThreadStart(Run));
            th.Start();
        }
        void Run()
        {
            for (; ; )
            {
                if (DoBank())
                {
                    complete = true;
                    break;
                }
                if (terminate)
                    break;
                Thread.Sleep(period);
            }
        }
        bool DoBank()
        {
            for (; ; )
            {
                BankZoneResult z = bank.GetNextResultZone();
                if (z == null)
                    break;
                pr(z.ToString());
                SL.oRESULT.Val = !z.OkResult;
                SL.oSTROBE.Val = true;
                Thread.Sleep(45);
                SL.oSTROBE.Val = false;
                SL.oRESULT.Val = false;
                Thread.Sleep(45);
                if (z.last)
                    return (true);
            }
            return (false);
        }
        void pr(string _msg)
        {
            ProtocolST.pr("JResult: " + _msg);
        }
        public bool SendResult()
        {
            EClass RClass = bank.GetResultTube();
            pr("ResultTube: " + ( RClass!=EClass.Brak ? "Годно" : "Брак"));
            SL.oRESULT.Val = RClass != EClass.Brak;
            SL.oSTROBE.Val = RClass != EClass.Brak;
            SL.oRESULT_COMMON.Val = true;
            Thread.Sleep(500);
            return (true);
        }
    }
}
