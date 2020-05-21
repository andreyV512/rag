using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

using PARLIB;
using UPAR_common;

namespace UPAR.TS.TSDef
{
    [DisplayName("Продольный модуль")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TSLine : ParBase
    {
        [DisplayName("Порог брака, %"), Browsable(true), De]
        public double Border1 { get; set; }

        [DisplayName("Порог 2 класса, %"), Browsable(true), De]
        public double Border2 { get; set; }

        [DisplayName("Порог брака, %"), Browsable(true), De]
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
        [Browsable(false)]
        public double[] BordersIn
        {
            get
            {
                double[] ret = new double[2];
                ret[0] = Border1In;
                ret[1] = Border2In;
                return (ret);
            }
        }
    }
}
