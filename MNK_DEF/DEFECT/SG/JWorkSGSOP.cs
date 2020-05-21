using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using Defect.Work;
using Signals;
using BankLib;
using UPAR;
using UPAR.SG;
using Protocol;
using UPAR.TS.TSDef;
using System.Threading;
using SQL;
using ResultLib.SG;
using Demagnetizer;

namespace Defect.SG
{
    public class JWorkSGSOP : IJob, IDisposable
    {
        Bank bank;
        List<IJob> J = new List<IJob>();
        SignalListDef SL;
        uint Step = 0;
        bool started = false;

        JAlarmList jAlarm = null;
        SGWorkPars tsDefSG = null;
        SGSet sgSet = null;
        JDemagnetizer jDemagnetizer = null;

        public JWorkSGSOP(SGWorkPars _tsDefSG, Bank _bank, SignalListDef _SL, DOnStatus _OnStatus = null)
        {
            tsDefSG = _tsDefSG;
            bank = _bank;
            SL = _SL;
            OnStatus = _OnStatus;
            IsComplete = false;
            J.Add(jAlarm = new JAlarmList());
            sgSet = new SGSet(_SL, pr);
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
            ProtocolST.pr("JWork: " + _msg);
        }
        void prsl(uint _level, string _msg)
        {
            if (onStatus != null)
                onStatus(_level, _msg);
            pr(_msg);
        }
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
                    if (!sgSet.CheckDTypeSize())
                    {
                        LastError = "Датчики не соответствуют типоразмеру";
                        break;
                    }
                    Step = 1;
                    break;
                case 1:
                    jAlarm.Add(SL.iCC, true);
                    jAlarm.Add(SL.iSGIN, true);
                    jAlarm.Add(SL.iSGOUT, true);
                    Step = 2;
                    break;
                case 2:
                    SolidGroupPars sgpars = ParAll.SG;
                    L502Ch[] MCh = new L502Ch[2]
                    {
                        sgpars.Sensor_I,
                        sgpars.Sensor_B,
                    };
                    jDemagnetizer.Start(_tick);
                    string ret = sgSet.Start(tsDefSG, MCh);
                    if (ret != null)
                    {
                        LastError = ret;
                        break;
                    }
                    bank.Start(EUnit.SG, _tick);
                    Step = 3;
                    break;
                case 3:
                    bank.AddGroup(EUnit.Cross, sgSet.ReadL502());
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
            BankZoneData z = bank.GetNextZoneSG();
            if (z != null)
            {
                string lret;
                SGSet.SaveToDb(bank.SGData, z.size, out lret);
                if (lret != null)
                    prsl(1, lret);
            }
            IsComplete = true;
            foreach (IJob job in J)
                job.Finish();
            Dispose();
        }
        int GetCount { get { return (bank == null ? 0 : bank.GetCountOfUnit(Share.EUnit.SG)); } }
    }
}
