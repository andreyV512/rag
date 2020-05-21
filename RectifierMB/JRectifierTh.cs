using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Share;
using UPAR_common;
using Protocol;

namespace RectifierNS
{
    public class JRectifierTh : IJob, IDisposable
    {
        RCF_modbus rcf;
        Thread th = null;
        int Period = 1;
        volatile bool terminate = false;
        string lastError = null;
        public string LastError { get { lock (Sync) { return (lastError); } } }

        RectifierPars rectifierPars;
        object Sync = new object();
        UIT uit = null;
        bool IsTime;

        public JRectifierTh(RectifiersPars _RectifiersPars, RectifierPars _rectifierPars, bool _verbose, bool _IsTime)
        {
            rcf = new RCF_modbus(_RectifiersPars, _rectifierPars, _verbose);
            rectifierPars = _rectifierPars;
            Period = _rectifierPars.Period;
            IsTime = _IsTime;
        }
        public void Start(int _tick)
        {
            if (th != null)
                return;
            terminate = false;
            th = new Thread(new ThreadStart(Run));
            th.Start();
        }
        public void Finish()
        {
            if (th == null)
                return;
            terminate = true;
            th.Join();
            th = null;
        }
        public void Dispose()
        {
            Finish();
            rcf.Dispose();
        }
        void Run()
        {
            lastError = rcf.Start();
            if (lastError != null)
                return;
            if (terminate)
                return;
            pr("Started");
            while (true)
            {
                pr("Step");
                if (terminate)
                    break;
                UIT luit = IsTime ? rcf.GetUIT() : rcf.GetUITshort();
                lastError = luit.error;
                if (lastError != null)
                    break;
                lock (Sync)
                {
                    uit = luit;
                }
                Thread.Sleep(Period);
                if (terminate)
                    break;
            }
            lastError = rcf.Stop();
            pr("Stoped");
        }
        void pr(string _msg)
        {
            ProtocolST.pr("JRectifierTh" + _msg);
        }
        public UITH GetUITH()
        {
            if (th==null)
                return (null);
            lock (Sync)
            {
                if (uit == null)
                    return (null);
                return (new UITH(uit, rectifierPars.MaxR));
            }
        }
        public void Exec(int _tick) { }
        public DOnStatus OnStatus { get; set; }
        public bool IsError { get { return (lastError != null); } }
        public bool IsComplete { get { return (th==null); } }
        public bool IsStarted { get { return (th != null); } }
    }
}
