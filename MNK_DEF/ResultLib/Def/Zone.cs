using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UPAR;
using Share;

namespace ResultLib.Def
{
    public class Zone
    {
        EUnit type;
        public EClass Class;
        public int VZoneLength = 0;
        public Sensor[] MSensor = null;

        public int Index { get; private set; }

        public Zone(EUnit _type)
        {
            type = _type;
        }
        public void Load(BinaryReader _br, int _sensors, int _version)
        {
            VZoneLength = 200;
            if (_version == 0)
            {
                double dDClass = _br.ReadDouble();
                int lVZoneLength = _br.ReadInt32();
                if (lVZoneLength > 0)
                    VZoneLength = lVZoneLength;
            }
            else if (_version == 1)
            {
                int zone_data = _br.ReadInt32();
            }

            MSensor = new Sensor[_sensors];
            for (int i_s = 0; i_s < _sensors; i_s++)
            {
                if (_version == 0)
                {
                    double level = _br.ReadDouble();
                }
                else if (_version == 1)
                {
                    int sensor_data = _br.ReadInt32();
                }

                Sensor s = null;
                if (_version == 0)
                {
                    s = new Sensor(type, _br.ReadInt32());
                }
                else if (_version == 1)
                {
                    int ssize = _br.ReadInt16();
                    s = new Sensor(type, ssize);
                }
                s.Load(_br, _version);
                MSensor[i_s] = s;
            }
        }
        public void Save(BinaryWriter _bw)
        {
            _bw.Write(Classer.ToDouble(Class));
            _bw.Write(VZoneLength);
            foreach (Sensor s in MSensor)
                s.Save(_bw);
        }
        //public void Load(double[] _zoneData, int _sensors)
        //{
        //    MSensor = new Sensor[_sensors];
        //    int lMeasLength = _zoneData.Length / _sensors;
        //    if (lMeasLength * _sensors != _zoneData.Length)
        //        throw new Exception("Zone.Load: массив данных не делится на количество сенсоров нацело");
        //    int point = 0;
        //    for (int s = 0; s < MSensor.Length; s++)
        //        MSensor[s] = new Sensor(type, MeasLength);
        //    for (int im = 0; im < MeasLength; im++)
        //    {
        //        for (int s = 0; s < MSensor.Length; s++)
        //            MSensor[s].MMeas[im].Source = _zoneData[point++];
        //    }
        //}
        public void Calc(Zone _prev)
        {
            Class = EClass.None;
            for (int s = 0; s < MSensor.Count(); s++)
            {
                MSensor[s].Calc(s, _prev == null ? null : _prev.MSensor[s]);
                Class = Classer.Compare(Class, MSensor[s].Class);
            }
        }
        public override string ToString()
        {
            return (string.Format("{0}={1} length={2}", Classer.ETypeStr(type), Classer.ToStr(Class), VZoneLength.ToString()));
        }
        public string ToString(int _index)
        {
            return (string.Format("{0}[{1}]={2} length={3}", Classer.ETypeStr(type), _index.ToString(), Classer.ToStr(Class), VZoneLength.ToString()));
        }
        public void Calibrate(int _sensor, bool[] _mb, double[] _gains)
        {
            double level = MSensor[_sensor].GetMaxLevel() * _gains[_sensor];
            L_L502Ch L = type == EUnit.Cross ? ParAll.CTS.Cross.L502Chs : ParAll.CTS.Line.L502Chs;
            for (int i = 0; i < L.Count; i++)
            {
                if (!_mb[i])
                    continue;
                if (i == _sensor)
                    continue;
                _gains[i] = Math.Round(level / MSensor[i].GetMaxLevel(), 2);
            }
            CalcClassGain(_gains);
        }
        public void CalcClassGain(double[] _gains)
        {
            Class = EClass.None;
            for (int s = 0; s < MSensor.Count(); s++)
            {
                MSensor[s].CalcClassGain(_gains[s]);
                Class = Classer.Compare(Class, MSensor[s].Class);
            }
        }
        public double GetMaxLevel()
        {
            double Level = -1;
            foreach (Sensor s in MSensor)
            {
                double v = s.GetMaxLevel();
                if (Level < v)
                    Level = v;
            }
            return (Level);
        }
    }
}
