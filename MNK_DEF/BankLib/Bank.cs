using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using Protocol;
using UPAR;

namespace BankLib
{
    public class Bank
    {
        object cs = new object();
        cIW IW;
        L_TickPosition MTP = new L_TickPosition();
        BankThickUnit MThick = null;
        BankCLUnitA MCross = null;
        BankCLUnitA MLine = null;
        BankSGUnit MSG = null;
        L_BankZoneResult MZR = new L_BankZoneResult();

        public Bank(cIW _IW)
        {
            IW = _IW;
            if (_IW.Thick)
                MThick = new BankThickUnit();
            if (IW.Cross)
                MCross = new BankCLUnitA(EUnit.Cross, IW);
            if (IW.Line)
                MLine = new BankCLUnitA(EUnit.Line, IW);
            if (IW.SG)
                MSG = new BankSGUnit();
            LoadSettings();
        }

        void Clear0()
        {
            MTP.Clear();
            if (MThick != null)
                MThick.Clear();
            if (MCross != null)
                MCross.Clear();
            if (MLine != null)
                MLine.Clear();
            if (MSG != null)
                MSG.Clear();
            MZR.Clear();
            tubeLength = null;
            pr("=== CLEAR ===");
        }
        void pr(string _msg)
        {
            ProtocolST.pr("Bank: " + _msg);
        }
        public void LoadSettings()
        {
            if (MThick != null)
                MThick.LoadSettings();
            if (MCross != null)
                MCross.LoadSettings();
            if (MLine != null)
                MLine.LoadSettings();
            if (MSG != null)
                MSG.LoadSettings();
        }
        public void Start(int _startTick)
        {
            lock (cs)
            {
                Clear0();
                MTP.startTick = _startTick;
                if (MThick != null)
                    MThick.isStarted = true;

                if (MCross != null)
                {
                    MCross.isStarted = true;
                    MZR.WaitZones = MCross.deadEnd;
                }
                if (MLine != null)
                {
                    MLine.isStarted = true;
                    MZR.WaitZones = MLine.deadEnd;
                }
                //if (MSG != null)
                //{
                //    MSG.isStarted = true;
                //}
            }
        }
        public void Start(EUnit _Tp, int _startTick)
        {
            lock (cs)
            {
                switch (_Tp)
                {
                    case EUnit.Thick:

                        if (MThick != null)
                            MThick.isStarted = true;
                        break;
                    case EUnit.Cross:

                        if (MCross != null)
                        {
                            MCross.isStarted = true;
                            MZR.WaitZones = MCross.deadEnd;
                        }
                        break;
                    case EUnit.Line:
                        if (MLine != null)
                        {
                            MLine.isStarted = true;
                            MZR.WaitZones = MLine.deadEnd;
                        }
                        break;
                    case EUnit.SG:
                        if (MSG != null)
                            MSG.isStarted = true;
                        break;
                }
            }
        }
        public double? FirstTick
        {
            set
            {
                lock (cs)
                {
                    if (MCross != null)
                        MCross.firstTick = value - MTP.startTick.Value;
                    if (MLine != null)
                        MLine.firstTick = value - MTP.startTick.Value;
                }
            }
        }
        public void AddTickPosition(TickPosition _TickPosition)
        {
            lock (cs)
            {
                if (GotZones)
                    return;
                pr("AddTickPosition[" + MTP.Count + "]: " + _TickPosition.ToString());
                MTP.Add(_TickPosition);
                //pr("AddTickPosition2[" + MTP.Count + "]: " + MTP.Last.ToString());
            }
        }
        public void AddGroup(EUnit _Tp, double[] _data)
        {
            lock (cs)
            {
                if (_data == null)
                    return;
                //BankCLUnitA MUnit=null;
                if (_Tp == EUnit.Cross)
                {
                    int lsize = 0;
                    int crossSensors = 0;
                    if (MCross != null)
                    {
                        crossSensors = MCross.Sensors;
                        lsize += MCross.Sensors;
                    }
                    if (MSG != null)
                        lsize += MSG.Sensors;
                    if (MCross != null)
                    {
                        MCross.Add(_data, 0, lsize);
                        pr(string.Format("AddGroup++: {0} size={1} count={2} f_acq={3}",
                        Current.EUnitToString(_Tp),
                        _data.Length.ToString(),
                        MCross.Count.ToString(),
                        MCross.f_acq.ToString()));
                    }
                    if (MSG != null)
                    {
                        if (MSG.LastData)
                            return;
                        MSG.Add(_data, crossSensors, lsize);
                        pr(string.Format("AddGroupSG++: {0} count={1}",
                        Current.EUnitToString(EUnit.SG),
                        MSG.Count.ToString()));
                    }
                }
                else if (_Tp == EUnit.Line)
                {
                    if (MLine != null)
                    {
                        MLine.Add(_data, 0, MLine.Sensors);
                        pr(string.Format("AddGroup++: {0} count={1} f_acq={2}",
                            Current.EUnitToString(_Tp),
                            MLine.Count.ToString(),
                            MLine.f_acq.ToString()));
                    }
                }
            }
        }
        public void AddThickZone(BankZoneThick _zone)
        {
            lock (cs)
            {
                if (_zone == null)
                    return;
                if (MThick == null)
                    return;
                MThick.Add(_zone);
            }
        }
        int GetConfirmed()
        {
            int confirmed = 1000;
            if (MThick != null && confirmed > MThick.GaveZones)
                confirmed = MThick.GaveZones;
            if (MCross != null && confirmed > MCross.GaveZones)
                confirmed = MCross.GaveZones;
            if (MLine != null && confirmed > MLine.GaveZones)
                confirmed = MLine.GaveZones;
            if (confirmed == 1000)
                return (confirmed);
            return (confirmed);
        }
        public BankZoneThick GetNextZoneThick()
        {
            lock (cs)
            {
                if (MThick == null)
                    return (null);
                BankZoneThick z = MThick.GetNextZone();
                if (z != null)
                {
                    MZR.confirmed = GetConfirmed();
                    pr("!!!GetNextThick: " + z.ToString());
                }
                return (z);
            }
        }
        int? tubeLength = null;
        public int? TubeLength { set { lock (cs) { tubeLength = value; } } get { lock (cs) { return (tubeLength); } } }
        bool GotZones
        {
            get
            {
                if (MThick != null)
                {
                    if (!MThick.GotZones) return (false);
                }
                if (MCross != null)
                {
                    if (!MCross.GotZones) return (false);
                }
                if (MLine != null)
                {
                    if (!MLine.GotZones) return (false);
                }
                if (MSG != null)
                {
                    if (!MSG.LastData) return (false);
                }
                return (true);
            }
        }
        public bool SGLastData
        {
            get
            {
                if (MSG == null)
                    return (true);
                return (MSG.LastData);
            }
            set
            {
                if (MSG != null)
                    MSG.LastData = value;
            }
        }
        void SetMaxResultZones()
        {
            if (!GotZones)
                return;
            int maxZones = 1000;
            if (MThick != null)
            {
                if (maxZones > MThick.MZone.Count)
                    maxZones = MThick.MZone.Count;
            }
            if (MCross != null)
            {
                if (maxZones > MCross.MaxCount)
                    maxZones = MCross.MaxCount;
            }
            if (MLine != null)
            {
                if (maxZones > MLine.MaxCount)
                    maxZones = MLine.MaxCount;
            }
            if (maxZones == 1000)
                maxZones = 0;
            MZR.MaxCount = maxZones;
        }
        public BankZoneDataA GetNextZoneCross()
        {
            return (GetNextZoneA(EUnit.Cross));
        }
        public BankZoneDataA GetNextZoneLine()
        {
            return (GetNextZoneA(EUnit.Line));
        }
        BankZoneDataA GetNextZoneA(EUnit _Tp)
        {
            lock (cs)
            {
                BankCLUnitA MUnit = null;
                if (_Tp == EUnit.Cross)
                    MUnit = MCross;
                else if (_Tp == EUnit.Line)
                    MUnit = MLine;
                if (MUnit != null)
                {
                    BankZoneDataA z = MUnit.GetNextDataUnitZoneA(TubeLength, MTP);
                    if (z != null)
                    {
                        MZR.confirmed = GetConfirmed();
                        pr("!!!GetNextZoneA: " + z.ToString());
                    }
                    return (z);
                }
                return (null);
            }
        }
        public BankZoneDataA GetLineZoneACalibr(bool _check)
        {
            lock (cs)
            {
                if (MLine != null)
                {
                    BankZoneDataA z = MLine.GetDataUnitZoneACalibr(_check);
                    if (z != null)
                        pr("!!!GetNextLineZoneACalibr: " + z.ToString());
                    return (z);
                }
                return (null);
            }
        }
        public BankZoneData GetNextZoneSG()
        {
            lock (cs)
            {
                if (MSG != null)
                {
                    BankZoneData z = MSG.GetZone();
                    if (z != null)
                        pr("!!!GetNextZoneSG: " + z.ToString());
                    return (z);
                }
                return (null);
            }
        }
        bool GaveZones
        {
            get
            {
                if (MThick != null)
                {
                    if (!MThick.IsComplete())
                        return (false);
                }
                if (MCross != null)
                {
                    if (!MCross.IsComplete())
                        return (false);
                }
                if (MLine != null)
                {
                    if (!MLine.IsComplete())
                        return (false);
                }
                if (MSG != null)
                {
                    if (!MSG.IsComplete())
                        return (false);
                }
                return (true);
            }
        }
        public bool IsGaveZones { get { lock (cs) { return (GaveZones); } } }
        public void AddResultZone(int _index, bool _result)
        {
            lock (cs)
            {
                MZR.Add(_index, _result);
            }
        }
        public BankZoneResult GetNextResultZone()
        {
            lock (cs)
            {
                return (MZR.GetNextResultZone());
            }
        }
        public void SetResultTube(EClass _class) { lock (cs) { MZR.ResultTube = _class; } }
        public EClass GetResultTube() { lock (cs) { return (MZR.ResultTube); } }
        string StateString()
        {
            if (tubeLength == null)
                return ("Еще не получена длина трубы");
            if (!GotZones)
                return ("Еще не получены данные для всех зон");
            if (!GaveZones)
                return ("Еще не отданы все зоны");
            SetMaxResultZones();
            if (!MZR.Gave)
                return ("Еще не отданы все результирующие зоны");
            return (null);
        }
        public string Complete { get { lock (cs) { return (StateString()); } } }
        public bool NoWait { get { return (MZR.NoWait); } set { MZR.NoWait = value; } }
        public double[] CrossData { get { return (MCross == null ? null : MCross.data); } }
        public double[] LineData { get { return (MLine == null ? null : MLine.data); } }
        public double[] SGData { get { return (MSG == null ? null : MSG.data); } }

        //        public void RawStrobesReport() { MLineThick.rawStrobes.Report(); }

        //        public RawStrobes ThickData { get { return (MThick == null ? null : MThick.rawStrobes); } }
        public int GetCountOfUnit(EUnit _Tp)
        {
            lock (cs)
            {
                BankDataUnit bu = null;
                switch (_Tp)
                {
                    case EUnit.Cross:
                        bu = MCross;
                        break;
                    case EUnit.Line:
                        bu = MLine;
                        break;
                    case EUnit.SG:
                        bu = MSG;
                        break;
                }
                return (bu == null ? 0 : bu.Count);
            }
        }
        public int GetRealSize(EUnit _Tp)
        {
            lock (cs)
            {
                switch (_Tp)
                {
                    case EUnit.Thick:
                        return (0);
                    case EUnit.Cross:
                        return (MCross == null ? 0 : MCross.GetRealSize());
                    case EUnit.Line:
                        return (MLine == null ? 0 : MLine.GetRealSize());
                    case EUnit.SG:
                        return (MSG == null ? 0 : MSG.GetRealSize());
                }
                return (0);
            }
        }
        public string RealSizeString()
        {
            return (string.Format("RealSizes: Cross={0} Line={1} SG={2}",
                MCross == null ? "null" : MCross.GetRealSize().ToString(),
                MLine == null ? "null" : MLine.GetRealSize().ToString(),
                MSG == null ? "null" : MSG.GetRealSize().ToString()));
        }
    }
}
