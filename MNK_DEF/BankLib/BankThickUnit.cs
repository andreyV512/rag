using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UPAR_common;
using UPAR;
using Share;
using Protocol;

namespace BankLib
{
    public class BankThickUnit : BankUnit, ILoadSettings
    {
        public List<BankZoneThick> MZone = new List<BankZoneThick>();
        public bool GotZones = false;
        public int GaveZones { get; private set; }
        public BankThickUnit()
            : base(EUnit.Thick)
        {
            GaveZones = 0;
            Clear();
        }
        public new void Clear()
        {
            MZone.Clear();
            GotZones = false;
            GaveZones = 0;
            base.Clear();
        }
        public void LoadSettings()
        {
            Clear();
        }
        public BankZoneThick GetNextZone()
        {
            if (IsComplete())
                return (null);
            if (!isStarted)
                return (null);
            if (GaveZones >= MZone.Count)
                return (null);
            BankZoneThick ret = MZone[GaveZones];
            GaveZones++;
            if (ret.last)
                complete = true;
            return (ret);
        }
        void pr(string _msg)
        {
            ProtocolST.pr(_msg);
        }
        public void Add(BankZoneThick _zone)
        {
            if (IsComplete())
                return;
            if (!isStarted)
                return;
            MZone.Add(_zone);
            if (_zone.last)
                GotZones = true;
        }
    }
}
