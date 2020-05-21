using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PARLIB;
//using UPAR_common;

namespace UPAR.Def
{
    [DisplayName("Толщиномер"), TypeConverter(typeof(ExpandableObjectConverter))]
    public class ThickPars : ParBase
    {
        [DisplayName("Работа"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsWork { get; set; }

        [DisplayName("Время ожидания раскручивания,c "), Browsable(true), DefaultValue(20), De]
        public int RotationWait { get; set; }

    }
}
