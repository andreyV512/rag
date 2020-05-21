using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using PARLIB;


namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GSPFPars : ParBase
    {
        [DisplayName("Номер устройства"), Browsable(true), DefaultValue(1), De]
        public int DevNum { get; set; }

        EGSPFStart gspfStart;
        [DisplayName("Тип запуска"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EGSPFStart GSPFStart { get { return (gspfStart); } set { gspfStart = value; } }

        EGSPFPlay gspfPlay;
        [DisplayName("Повторение"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EGSPFPlay GSPFSPlay { get { return (gspfPlay); } set { gspfPlay = value; } }

        //    //циклическое проигрывание сигнала
        //    initStr.control = (uint)RshInitGSPF.ControlBit.PlayLoop;
        public enum EGSPFStart
        {
            [Description("Программный")]
            Program,

            [Description("Внешний")]
            External,

            [Description("Внешняя частота")]
            FrequencyExternal
        }
        public uint GetEGSPFStart()
        {
            switch (gspfStart)
            {
                case EGSPFStart.Program:
                    return (1);
                case EGSPFStart.External:
                    return (4);
                case EGSPFStart.FrequencyExternal:
                    return (16);
                default:
                    return (1);
            }
        }
        public enum EGSPFPlay
        {
            [Description("Один раз")]
            PlayOnce,

            [Description("Фильтр")]
            FilterOn,

            [Description("Циклически")]
            PlayLoop,

            [Description("Асинхронно")]
            SynchroDecline,
        }
        public uint GetEGSPFSPlay()
        {
            switch (gspfPlay)
            {
                case EGSPFPlay.PlayOnce:
                    return (0);
                case EGSPFPlay.FilterOn:
                    return (2);
                case EGSPFPlay.PlayLoop:
                    return (4);
                case EGSPFPlay.SynchroDecline:
                    return (8);
                default:
                    return (4);
            }
        }
        //[DisplayName("Частота сбора на канал, Гц"), Browsable(true), De, DefaultValue(8000.0)]
        //public double FrequencyPerChannel { get; set; }

        //class SyncModeConverter : DictionaryConverter
        //{
        //    SyncModeConverter()
        //    {
        //        D = new Dictionary<int, string>()
        //        {
        //            {0,"Внутренний сигнал"},
        //            {1,"От внешнего мастера по разъему синхронизации"}
        //        };
        //    }
        //}

        //[DisplayName("Режим синхронизации"), DefaultValue(0), Browsable(true), De]
        //[TypeConverter(typeof(SyncModeConverter))]
        //public int SyncMode { get; set; }

        //class SyncStartModeConverter : DictionaryConverter
        //{
        //    SyncStartModeConverter()
        //    {
        //        D = new Dictionary<int, string>()
        //        {
        //            {0,"По фронту сигнала DI_SYN1"},
        //            {1,"По фронту сигнала DI_SYN2"},
        //            {2,"По спаду сигнала DI_SYN1"},
        //            {3,"По спаду сигнала DI_SYN2"}
        //        };
        //    }
        //}

        //[DisplayName("Режим старта синхронизации"), DefaultValue(0), Browsable(true), De]
        //[TypeConverter(typeof(SyncStartModeConverter))]
        //public int SyncStartMode { get; set; }

        //[DisplayName("Период чтения с платы, мс"), DefaultValue(100), Browsable(true), De]
        //public int ReadPeriod { get; set; }

        //[DisplayName("TTL параметры"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public TTLPars TTL { get; set; }


        public override string ToString() { return (""); }
    }
}
