using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

//using ResultLib.Thick;
using UPAR;
//using ResultLib.Thick.RTParsNS;
using Share;

namespace ResultLib
{
    public enum EGenesis { None, File, AB, EKE, LV }
//    public enum EType { Cross, Line, Thick, None };
    public static class Classer
    {
        public static string ETypeStr(EUnit _tp)
        {
            switch (_tp)
            {
                case EUnit.Cross:
                    return ("Поперечн.");
                case EUnit.Line:
                    return ("Продольн.");
                case EUnit.Thick:
                    return ("Толщиномер.");
                default:
                    return ("Неопределенн.");
            }
        }
        public static string ToStr(EClass _cl)
        {
            switch (_cl)
            {
                case EClass.Brak:
                    return ("Брак");
                case EClass.Class1:
                    return ("Годно");
                case EClass.Class2:
                    return ("Класс 2");
                case EClass.Dead:
                    return ("Мертвый");
                default:
                    return ("Неизмерено");
            }
        }
        public static int ToIntACS(EClass _cl)
        {
            switch (_cl)
            {
                case EClass.Brak:
                    return (0);
                case EClass.Class1:
                    return (1);
                case EClass.Class2:
                    return (2);
                case EClass.Dead:
                    return (1);
                default:
                    return (-1);
            }
        }
        public static int ToInt(EClass _cl)
        {
            switch (_cl)
            {
                case EClass.Brak:
                    return (0);
                case EClass.Class1:
                    return (2);
                case EClass.Class2:
                    return (1);
                case EClass.Dead:
                    return (-2);
                default:
                    return (-1);
            }
        }
        public static int ToIntDB(EClass _cl)
        {
            switch (_cl)
            {
                case EClass.Brak:
                    return (2);
                case EClass.Class1:
                    return (0);
                case EClass.Class2:
                    return (1);
                case EClass.Dead:
                    return (1);
                default:
                    return (-1);
            }
        }
        public static bool ToBool(EClass _cl)
        {
            switch (_cl)
            {
                case EClass.Brak:
                    return (false);
                case EClass.Class1:
                    return (true);
                case EClass.Class2:
                    return (true);
                case EClass.Dead:
                    return (true);
                case EClass.None:
                    return (true);
                default:
                    return (false);
            }
        }
        public static char ToChar(EClass _cl)
        {
            switch (_cl)
            {
                case EClass.Brak:
                    return ('б');
                case EClass.Class1:
                    return ('г');
                case EClass.Class2:
                    return ('2');
                case EClass.Dead:
                    return ('д');
                default:
                    return ('н');
            }
        }
        public static EClass FromChar(char _c)
        {
            switch (_c)
            {
                case 'б':
                    return (EClass.Brak);
                case 'г':
                    return (EClass.Class1);
                case '2':
                    return (EClass.Class2);
                case 'д':
                    return (EClass.Dead);
                default:
                    return (EClass.None);
            }
        }

        public static double ToDouble(EClass _cl)
        {
            return (Convert.ToDouble(ToInt(_cl)));
        }
        public static EClass FromInt(int _icl)
        {
            switch (_icl)
            {
                case 0:
                    return (EClass.Brak);
                case 2:
                    return (EClass.Class1);
                case 1:
                    return (EClass.Class2);
                case -2:
                    return (EClass.Dead);
                default:
                    return (EClass.None);
            }
        }
        public static EClass FromDouble(double _dcl)
        {
            return (FromInt(Convert.ToInt32(_dcl)));
        }
        //public static Color GetColor(double? _thickLevel)
        //{
        //    return (GetColor(GetThickClass(_thickLevel)));
        //}
        //public static Color GetColor(EClassThick _class)
        //{
        //    return (GetColor(_class, EGenesis.EKE));
        //}
        public static Color GetColor(EClass _class)
        {
            return (GetColor(_class, EGenesis.EKE));
        }
        public static Color GetColor(EClass _class, EGenesis _eGenesis)
        {
            CColors g = GetColorsGroup(_eGenesis);
            if (_class == EClass.Class1)
                return (g.Class1);
            if (_class == EClass.Class2)
                return (g.Class2);
            if (_class == EClass.Brak)
                return (g.Brak);
            return (g.NotMeasured);
        }
        //public static Color GetColor(EClassThick _class, EGenesis _eGenesis)
        //{
        //    ColorsGroup g = GetColorsGroup(_eGenesis);
        //    if (_class == EClassThick.BrakUp)
        //        return (g.BrakUp);
        //    if (_class == EClassThick.BrakDown)
        //        return (g.BrakDown);
        //    if (_class == EClassThick.Ok)
        //        return (g.Class1);
        //    return (g.NotMeasured);
        //}
        //public static Color GetColor(double? _thickLevel, EGenesis _eGenesis)
        //{
        //    return (GetColor(GetThickClass(_thickLevel), _eGenesis));
        //}
        //        static double MaxThickness { get { return (RTPars.ST.rtTS.MaxThickness); } }
        //public static double GetThickLevel(double? _thickLevel)
        //{
        //    double ret = _thickLevel == null ? MaxThickness : _thickLevel.Value;
        //    return (ret);
        //}
        //public static EClassThick GetThickClass(double? _level)
        //{
        //    if (_level == null)
        //        return (EClassThick.None);
        //    if (_level > RTPars.ST.rtTS.BorderUpLevel)
        //        return (EClassThick.BrakUp);
        //    if (_level < RTPars.ST.rtTS.BorderDownLevel)
        //        return (EClassThick.BrakDown);
        //    return (EClassThick.Ok);
        //}
        public static EClass Compare(EClass _class0, EClass _class1)
        {
            if (_class0 == EClass.Brak || _class1 == EClass.Brak)
                return (EClass.Brak);
            if (_class0 == EClass.Class2 || _class1 == EClass.Class2)
                return (EClass.Class2);
            if (_class0 == EClass.Class1 || _class1 == EClass.Class1)
                return (EClass.Class1);
            if (_class0 == EClass.Dead || _class1 == EClass.Dead)
                return (EClass.Dead);
            return (EClass.None);
        }
       
        public static EClass GetDefClass(double _level, double[] _borders, bool _dead)
        {
            if (_dead)
                return (EClass.Dead);
            if (_level < 0)
                return (EClass.None);
            if (_level >= _borders[0])
                return (EClass.Brak);
            if (_borders[1] != 0 && _level >= _borders[1])
                return (EClass.Class2);
            return (EClass.Class1);
        }
        public static CColors GetColorsGroup(EGenesis _eGenesis)
        {
            return (ParAll.ST.Colors);
        }
        //public static bool[] ComputeDead(int _zone, int _zones, int _ZoneSize, int _DeadZoneStart, int _DeadZoneFinish, int _MeasLength)
        //{
        //    double dead_size0 = _DeadZoneStart;
        //    double dead_size1 = _ZoneSize * _zones - _DeadZoneFinish;

        //    bool[] M = new bool[_MeasLength];
        //    int MMBegin = _zone * _ZoneSize;
        //    int MMEnd = MMBegin + _ZoneSize;
        //    double MMPerMeas = (double)_ZoneSize / _MeasLength;

        //    if (MMBegin >= dead_size0 && MMEnd < dead_size1)
        //    {
        //        for (int i = 0; i < M.Length; i++)
        //            M[i] = false;
        //        return (M);
        //    }
        //    if (MMEnd <= dead_size0)
        //    {
        //        for (int i = 0; i < M.Length; i++)
        //            M[i] = true;
        //        return (M);
        //    }
        //    if (MMBegin > dead_size1)
        //    {
        //        for (int i = 0; i < M.Length; i++)
        //            M[i] = false;
        //        return (M);
        //    }
        //    double pos = MMBegin;
        //    for (int i = 0; i < M.Length; i++)
        //    {
        //        pos += MMPerMeas;
        //        M[i] = pos <= dead_size0 || pos > dead_size1;
        //    }
        //    return (M);
        //}
        //static public void MakeLevel(double? _level, double _nominal, ref double? _levelUp, ref double? _levelDown)
        //{
        //    if (_level == null)
        //        return;
        //    if (_level >= _nominal)
        //    {
        //        if (_levelUp == null || _levelUp < _level)
        //            _levelUp = _level;
        //    }
        //    else
        //    {
        //        if (_levelDown == null || _levelDown > _level)
        //            _levelDown = _level;
        //    }
        //}
        //static public void MakeLevelUp(double? _level, ref double? _levelUp)
        //{
        //    if (_level == null)
        //        return;
        //    if (_levelUp == null || _levelUp < _level)
        //        _levelUp = _level;
        //}
        //static public void MakeLevelDown(double? _level, ref double? _levelDown)
        //{
        //    if (_level == null)
        //        return;
        //    if (_levelDown == null || _levelDown > _level)
        //        _levelDown = _level;
        //}
    }
}
