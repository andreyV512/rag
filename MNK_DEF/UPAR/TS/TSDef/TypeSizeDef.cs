using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;

using PARLIB;
using UPAR_common;

namespace UPAR.TS.TSDef
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TypeSizeDef:ParBase 
    {
        [Browsable(true), De]
        public TSCross Cross { get; set; }

        [Browsable(true), De]
        public TSLine Line { get; set; }

        //[DisplayName("Частота вращения стоек, Гц"), Browsable(true), De]
        //public int Frequency { get; set; }

        [DisplayName("Порог 1 класса, %"), Browsable(true), De]
        public double Border1 { get; set; }

        [DisplayName("Порог 2 класса, %"), Browsable(true), De]
        public double Border2 { get; set; }

        [DisplayName("Порог 1 класса внутренний, %"), Browsable(true), De]
        public double Border1In { get; set; }

        [DisplayName("Порог 2 класса внутренний, %"), Browsable(true), De]
        public double Border2In { get; set; }

        [DisplayName("Мертвая зона в начале, мм"), Browsable(true), De]
        public int DeadZoneStart { get; set; }

        [DisplayName("Мертвая зона в конце, мм"), Browsable(true), De]
        public int DeadZoneFinish { get; set; }

        //        [DisplayName("Скорости вращения"), Browsable(true), De]
        //        public SpeedsPars Speeds { get; set; }

        [DisplayName("Частота, Гц"), Browsable(true), De]
        public int Frequency { get; set; }


        //[DisplayName("Скорость входа"), Browsable(true), De]
        //public int InSpeed { get; set; }

        //[DisplayName("Скорость выхода"), Browsable(true), De]
        //public int OutSpeed { get; set; }

        //[DisplayName("Скорость рабочая"), Browsable(true), De]
        //public int WorkSpeed { get; set; }

        //[DisplayName("Пауза перед включением рабочей скорости"), Browsable(true), De]
        //public int PauseWorkSpeed { get; set; }

        //[DisplayName("Пауза перед остановом"), Browsable(true), De]
        //public int PauseStop { get; set; }

        [DisplayName("Выпрямитель"), Browsable(true), De]
        public RectifierPars Rectifier { get; set; }

        [DisplayName("Датчики"), Browsable(true), De]
        public L_L502Ch L502Chs { get; set; }

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

    }
}
