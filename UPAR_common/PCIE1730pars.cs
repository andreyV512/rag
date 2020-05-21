using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PCIE1730pars : ParBase
    {
        [DisplayName("Номер платы"), DefaultValue(0), Browsable(true), De]
        public int Devnum { get; set; }

        [DisplayName("Входных портов"), DefaultValue(4), Browsable(true), De] 
        public int PortcountIn { get; set; }

        [DisplayName("Выходных портов"), DefaultValue(4), Browsable(true), De]
        public int PortcountOut { get; set; }

        [DisplayName("Смещение"), DefaultValue(16), Browsable(true), De]
        public int DigitalOffset { get; set; }

        [DisplayName("Период чтения, мс"), DefaultValue(50), Browsable(true), De]
        public int SignalListTimeout { get; set; }

        [Browsable(true), De]
        [Sortable]
        public L_SignalPars Signals { get; set; }

        [DisplayName("Бинарный файл"),Browsable(true), De]
                public Save1730Pars Save1730 { get; set; }

        public override string ToString()
        {
            return (Devnum.ToString());
        }
    }
}
