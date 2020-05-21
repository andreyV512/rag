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
    public class LCard502Pars : ParBase
    {
        [DisplayName("Номер устройства"), Browsable(true), De]
        public int DevNum { get; set; }

        [DisplayName("Частота сбора на канал, Гц"), Browsable(true), De, DefaultValue(8000.0)]
        public double FrequencyPerChannel { get; set; }

        class SyncModeConverter : DictionaryConverter
        {
            SyncModeConverter()
            {
                D = new Dictionary<int, string>()
                {
                    {0,"Внутренний сигнал"},
                    {1,"От внешнего мастера по разъему синхронизации"}
                };
            }
        }

        [DisplayName("Режим синхронизации"), DefaultValue(0), Browsable(true), De]
        [TypeConverter(typeof(SyncModeConverter))]
        public int SyncMode { get; set; }

        class SyncStartModeConverter : DictionaryConverter
        {
            SyncStartModeConverter()
            {
                D = new Dictionary<int, string>()
                {
                    {0,"По фронту сигнала DI_SYN1"},
                    {1,"По фронту сигнала DI_SYN2"},
                    {2,"По спаду сигнала DI_SYN1"},
                    {3,"По спаду сигнала DI_SYN2"}
                };
            }
        }

        [DisplayName("Режим старта синхронизации"), DefaultValue(0), Browsable(true), De]
        [TypeConverter(typeof(SyncStartModeConverter))]
        public int SyncStartMode { get; set; }

        [DisplayName("Период чтения с платы, мс"), DefaultValue(100), Browsable(true), De]
        public int ReadPeriod { get; set; }

        [DisplayName("TTL параметры"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TTLPars TTL { get; set; }


        public override string ToString() { return (""); }
    }
}
