using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using Protocol;
using UPAR;

namespace BankLib
{
    class L_BankZoneResult : List<BankZoneResult>
    {
        public int confirmed = 0;
        public L_BankZoneResult()
        {
            Clear();
        }
        public void Add(int _index, bool _result)
        {
            if (_index < Count)
            {
                if (this[_index].OkResult != _result)
                    pr("AddResultZone: Change result: " + this[_index].ToString());
                this[_index].OkResult = _result;
            }
            else
            {
                if (maxCount == null || maxCount.Value < Count)
                {
                    BankZoneResult z = new BankZoneResult(_result, _index);
                    pr("AddResultZone: " + z.ToString());
                    Add(z);
                }
            }
        }
        void pr(string _msg)
        {
            ProtocolST.pr(_msg);
        }
        public new void Clear()
        {
            NoWait = false;
            waitZones = 0;
            GaveCount = 0;
            base.Clear();
        }
        int GaveCount = 0;
        int? maxCount = null;
        public int MaxCount
        {
            get
            {
                return (maxCount == null ? 0 : maxCount.Value);
            }
            set
            {
                if (maxCount == null)
                    maxCount = value;
            }
        }
        public BankZoneResult GetNextResultZone()
        {
            if (!NoWait)
            {
                if (GaveCount >= confirmed)
                    return (null);
                if (GaveCount >= Count - WaitZones)
                    return (null);
            }
            if (GaveCount >= Count)
                return (null);
            BankZoneResult ret = this[GaveCount++];
            if (maxCount != null)
            {
                if (GaveCount == maxCount.Value)
                    ret.last = true;
            }
            pr("GaveCount=" + GaveCount);
            return (ret);
        }
        public bool Gave
        {
            get
            {
                if (maxCount == null)
                    return (false);
                if (GaveCount >= maxCount.Value)
                    return (true);
                return (false);
            }
        }
        public EClass ResultTube = EClass.None;
        public int waitZones = 0;
        public int WaitZones
        {
            get { return (waitZones); }
            set
            {
                if (value >= 0)
                {
                    if (waitZones < value)
                        waitZones = value;
                }

            }
        }
        public bool NoWait;
    }
}
