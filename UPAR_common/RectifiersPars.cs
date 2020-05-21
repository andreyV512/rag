using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RectifiersPars : ParBase
    {
        [DisplayName("COM порт"), Browsable(true), De]
        public ComPortPars ComPort { get; set; }

        [DisplayName("Абонент"), DefaultValue(1), Browsable(true), De, Confirm]
        public int Abonent { get; set; }

        [DisplayName("Число повторений"), Browsable(true), DefaultValue(5), De]
        public int Iters { get; set; }

        [DisplayName("Задержка повторений, мс"), Browsable(true), DefaultValue(500), De]
        public int Timeout { get; set; }

        [DisplayName("Время ожидания магнитных полей,c "), Browsable(true), DefaultValue(20), De]
        public int MagnitWait { get; set; }
    }
}
