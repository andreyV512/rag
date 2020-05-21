using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PARLIB;

namespace UPAR.SG
{
    [DisplayName("Группа прочности")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SolidGroupPars : ParBase
    {
        [DisplayName("Работа"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsWork { get; set; }

        [DisplayName("Параметры"), Browsable(true), De]
        public SGPars sgPars { get; set; }

        [Browsable(true), De]
        public SGSomePars Some { get; set; }

        [DisplayName("Датчик тока"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public L502Ch Sensor_I { get; set; }

        [DisplayName("Датчик напряжения"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public L502Ch Sensor_U { get; set; }

        [DisplayName("Датчик Баргаузен"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public L502Ch Sensor_B { get; set; }

        [DisplayName("Датчик D0"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public L502Ch Sensor_D0 { get; set; }

        [DisplayName("Датчик D1"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public L502Ch Sensor_D1 { get; set; }

        [DisplayName("Датчик D2"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public L502Ch Sensor_D2 { get; set; }

        [DisplayName("Уровень 1 датчика типоразмера,В"), Browsable(true), DefaultValue(2.5), De]
        public double LevelD { get; set; }

        [DisplayName("Фиксированная"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsFix { get; set; }

        [DisplayName("Фиксированная группа"), Browsable(true), De]
        public string GroupFix { get; set; }

        [DisplayName("Размер буфера сбора, мб"), Browsable(true), De]
        public int Buffer { get; set; }

        [DisplayName("Период сбора СОП, с"), Browsable(true), DefaultValue(5), De]
        public int SOPPeriod { get; set; }

        [DisplayName("Показывать график СОП"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsGraph { get; set; }

        [DisplayName("Генератор"), Browsable(true), De]
        public GSPFPars GSPF { get; set; }


//        [DisplayName("Работа"), Browsable(true), De]
//        public WorkPars Work { get; set; }
    }
}
