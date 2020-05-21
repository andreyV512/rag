using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Share
{
    public enum EClass { Class1, Class2, Brak, Dead, None };
    public enum EUnit { Cross, Line, Thick, SG, None };
    public enum EWorkMode { Stop, Stoping, Work, Interrupt }
    public delegate void DOnWorkMode(EWorkMode _workMode);
    public enum EThickState { None, On, Ready, Rotate, Collect, Complete, Error };
    public enum EThickCommand { None, Reset, Ready, Rotate, Collect };
    public delegate void DOnPr(string _msg);
    //void SetWorkMode(EWorkMode _workMode)
    //{
    //    switch (_workMode)
    //    {
    //        case EWorkMode.Stop:
    //        case EWorkMode.Stoping:
    //            break;
    //        case EWorkMode.Work:
    //            break;
    //        case EWorkMode.Interrupt:
    //            break;
    //    }
    //}

    public static class Current
    {
        public static string EUnitToString(EUnit _type)
        {
            switch (_type)
            {
                case EUnit.Cross:
                    return ("Пеперечный");
                case EUnit.Line:
                    return ("Продольный");
                case EUnit.None:
                    return ("Неопределенный");
                case EUnit.SG:
                    return ("ГП");
                case EUnit.Thick:
                    return ("Толщиномер");
                default:
                    return ("Неопределенный");
            }
        }
        public static EUnit UnitType = EUnit.None;
        public static string FieldPath
        {
            get
            {
                switch (UnitType)
                {
                    case EUnit.Cross:
                        return ("pathFileNameCross");
                    case EUnit.Line:
                        return ("pathFileNameLong");
                    case EUnit.Thick:
                        return ("pathFileNameThick");
                    default:
                        return (null);
                }
            }
        }
        public static string FieldTresh
        {
            get
            {
                switch (UnitType)
                {
                    case EUnit.Cross:
                        return ("thresholdC");
                    case EUnit.Line:
                        return ("thresholdL");
                    case EUnit.Thick:
                        return ("thresholdT");
                    default:
                        return (null);
                }
            }
        }
        public static string TableResult
        {
            get
            {
                switch (UnitType)
                {
                    case EUnit.Cross:
                        return ("resultCross");
                    case EUnit.Line:
                        return ("resultLong");
                    case EUnit.Thick:
                        return ("resultThick");
                    default:
                        return (null);
                }
            }
        }
        public static string FieldPars
        {
            get
            {
                switch (UnitType)
                {
                    case EUnit.Cross:
                        return ("PCross");
                    case EUnit.Line:
                        return ("PLine");
                    case EUnit.Thick:
                        return ("PThick");
                }
                return (null);
            }
        }
    }
    public class SGState
    {
        public string Group = null;
        public Color RColor = Color.Black;
        public double Metric = 0;
        public int DBColor { get { return (RColor.ToArgb()); } set { RColor = Color.FromArgb(value); } }
    }
}
