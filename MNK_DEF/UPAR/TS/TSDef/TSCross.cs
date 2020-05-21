using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;

using PARLIB;
using UPAR_common;

namespace UPAR.TS.TSDef
{
    [DisplayName("Поперечный модуль")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TSCross : ParBase
    {
        [DisplayName("Порог брака, %"), Browsable(true), De, Confirm]
        public double Border1 { get; set; }

        [DisplayName("Порог 2 класса, %"), Browsable(true), De, Confirm]
        public double Border2 { get; set; }

        [DisplayName("Мертвая зона в начале, мм"), Browsable(true), De, Confirm]
        public int DeadZoneStart { get; set; }

        [DisplayName("Мертвая зона в конце, мм"), Browsable(true), De, Confirm]
        public int DeadZoneFinish { get; set; }

        [DisplayName("Датчики"), Browsable(true), De, Confirm]
        public L_L502Ch L502Chs { get; set; }

        [Browsable(true), De]
        public TSDefSG SG { get; set; }

        public override string ToString() { return (null); }

        [Browsable(false)]
        public double[] Borders
        {
            get
            {
                double[] ret = new double[2];
                ret[0] = Border1;
                ret[1] = Border2;
                return (ret);
            }
        }
        [DisplayName("Выпрямитель"), Browsable(true), De]
        public RectifierPars Rectifier { get; set; }
    }
}
