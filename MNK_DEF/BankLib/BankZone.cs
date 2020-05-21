using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;

namespace BankLib
{
    public class BankZone
    {
        public enum EType
        {
            BASE, CROSS, LINE, SG, THICK, THICK_TICKS, THICK_VAL, RESULT
        };
        public static EType EUnitToEType(EUnit _Tp)
        {
            switch (_Tp)
            {
                case EUnit.Cross:
                    return (EType.CROSS);
                case EUnit.Line:
                    return (EType.LINE);
                case EUnit.None:
                    return (EType.BASE);
                case EUnit.SG:
                    return (EType.SG);
                case EUnit.Thick:
                    return (EType.THICK);
                default:
                    return (EType.BASE);
            }
        }
        public int index = 0;
        public EType type;
        public bool last = false;

        public BankZone(EType _type)
        {
            type = _type;
        }
        public string TypeToString()
        {
            return (TypeToString(type));
        }
        public static string TypeToString(EType _type)
        {
            switch (_type)
            {
                case EType.BASE:
                    return ("Базовая");
                case EType.CROSS:
                    return ("Пеперечная");
                case EType.LINE:
                    return ("Продольная");
                case EType.SG:
                    return ("ГП");
                case EType.THICK_TICKS:
                    return ("Тики толщины");
                case EType.THICK_VAL:
                    return ("Толщина");
                case EType.RESULT:
                    return ("Результат");
            }
            return ("Неопределенная");
        }
        public override string ToString()
        {
            return (string.Format("{0}[{1}]", TypeToString(), index.ToString()));
        }
    }

    public class BankZoneData : BankZone
    {
        public int idata = -1;
        public int size = 0;
        public int length = -1;
        public BankZoneData(EType _type) : base(_type) { }

        public override string ToString()
        {
            return (base.ToString() + " size=" + size.ToString() + ", length=" + length.ToString());
        }
    }
    public class BankZoneData2 : BankZoneData
    {
        public int idata2 = -1;
        public int size2 = 0;

        BankZoneData2(BankZoneData _z1, BankZoneData _z2)
            : base(_z1.type)
        {
            idata = _z1.idata;
            size = _z1.size;
            idata2 = _z2.idata;
            size2 = _z2.size;

            length = _z1.length;
            index = _z1.index;
            last = _z1.last;
        }
        public static BankZoneData2 Create(BankZoneData _z1, BankZoneData _z2)
        {
            if (_z1 == null || _z2 == null)
                return (null);
            return (new BankZoneData2(_z1, _z2));
        }

        public override string ToString()
        {
            return (string.Format("{0} sz2={1}, len2={2}, delta={3} {4}:{5} {6}:{7}",
                base.ToString(),
                size2.ToString(),
                length.ToString(),
                (idata2 - idata).ToString(),
                idata.ToString(),
                (idata + size).ToString(),
                idata2.ToString(),
                (idata2 + size2).ToString()
                ));
        }
    }
    public class BankZoneDataA : BankZone
    {
        public BankZoneData[] MZones;

        public BankZoneDataA(EType _type, int _length, int _index)
            : base(_type)
        {
            MZones = new BankZoneData[_length];
            for (int i = 0; i < _length; i++)
                MZones[i] = null;
            index = _index;
        }
        public void Add(int _sensor, BankZoneData _z)
        {
            MZones[_sensor] = _z;
        }
        public bool last { get { return (MZones[0] != null && MZones[0].last); } }
        public int zone_length { get { return (MZones[0] == null ? 0 : MZones[0].length); } }
        public int length { get { return (MZones.Length); } }
    }
    public class BankZoneVal : BankZone
    {
        public double Val = 0;
        public bool OkVal = false;

        public BankZoneVal() : base(EType.THICK_VAL) { }

        public override string ToString()
        {
            if (!OkVal)
                return (base.ToString() + " не измерено");
            return (base.ToString() + " " + Val.ToString("F3") + (last ? " last" : null));
        }
    }
    public class BankZoneResult : BankZone
    {
        public bool OkResult = true;
        public BankZoneResult() : base(EType.RESULT) { }
        public BankZoneResult(bool _OkResult, int _index)
            : base(EType.RESULT)
        {
            OkResult = _OkResult;
            index = _index;
        }
        public override string ToString()
        {
            return (base.ToString() + (OkResult ? " Годно" : " Брак"));
        }
    }
    public class BankZoneThick : BankZone
    {
        public double? Level;
        public EClass RClass = EClass.None;
        public int Length;

        public BankZoneThick() : base(EType.THICK) { }
        public override string ToString()
        {
            return (base.ToString()+string.Format("Level={0} {1} length={2}",
                Level==null?"null":Level.Value.ToString(),
                RClass.ToString(),
                Length.ToString()
                ));
        }
    }
}