using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR
{
    public class DimensionsPars : ParBase
    {

        [DisplayName("Вход МНК1, мм"), Browsable(true), De]
        public int Stand1 { get; set; }

        [DisplayName("Стойка 2, мм"), Browsable(true), De]
        public int Stand2 { get; set; }

        [DisplayName("Датчики МНК2, мм"), Browsable(true), De]
        public int SensorsC { get; set; }

        [DisplayName("Стойка 3, мм"), Browsable(true), De]
        public int Stand3 { get; set; }

        [DisplayName("Датчики МНК3, мм"), Browsable(true), De]
        public int SensorsL { get; set; }

        [DisplayName("Стойка 4, мм"), Browsable(true), De]
        public int Stand4 { get; set; }

        [DisplayName("МНК4 вход, мм"), Browsable(true), De]
        public int SGIn { get; set; }

        [DisplayName("МНК4 выход, мм"), Browsable(true), De]
        public int SGOut { get; set; }
    }
}
