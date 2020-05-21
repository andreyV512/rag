using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.Def
{
    [DisplayName("Просмотр зоны")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ViewZonePars : ParBase
    {
        [DisplayName("Исходный сигнал"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool ViewZoneSSource { get; set; }

        [DisplayName("Медианный фильтр"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool ViewZoneSMedian { get; set; }

        [DisplayName("Частотный фильтр"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool ViewZoneSFilter { get; set; }

        [DisplayName("Частотный фильтр внутренний"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool ViewZoneSFilterIn { get; set; }

        public override string ToString() { return (""); }
    }
}
