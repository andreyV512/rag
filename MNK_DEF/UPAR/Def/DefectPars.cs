using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;

using PARLIB;
using UPAR_common;

namespace UPAR.Def
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DefectPars : ParBase
    {
        [Browsable(true), De]
        public CrossPars Cross { get; set; }

        [Browsable(true), De]
        public LinePars Line { get; set; }

        [Browsable(true), De]
        public ThickPars Thick { get; set; }

        //[Browsable(true), De]
        //public LinePars Line { get; set; }

        //[DisplayName("Группа прочности"), Browsable(true), De]
        //public SolidGroupPars SolidGroup { get; set; }


        //        [DisplayName("Связь с СУБД"), Browsable(true), De]
        [DisplayName("Связь с СУБД"), Browsable(false)]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsDBS { get; set; }

        //[DisplayName("Фильтр внутренний"), Browsable(true), De]
        //public FilterPars FilterIn { get; set; }

        //[DisplayName("Частотный преобразователь"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public ConverterPars Converter { get; set; }

        //[DisplayName("COM порт частотного преобразователя"), Browsable(true), De]
        //public ComPortPars ComPortConverters { get; set; }

        //[DisplayName("COM порт оборотов"), Browsable(true), De]
        //public ComPortPars ComPortRotation { get; set; }

        //[DisplayName("Соленоид"), Browsable(true), De]
        //public SolenoidPars Solenoid { get; set; }

        //[DisplayName("Размер буфера сбора, мб"), Browsable(true), De]
        //public int Buffer { get; set; }

        //[DisplayName("Концевые корректировки"), Browsable(true), De]
        //public TailPars Tails { get; set; }


        //[Browsable(true), De]
        //public CrossPars Cross { get; set; }

        //[Browsable(true), De]
        //public LinePars Line { get; set; }

        [DisplayName("PCIE1730"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PCIE1730pars PCIE1730 { get; set; }

        //[DisplayName("PCIE1730 1"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public PCIE1730pars PCIE1730_1 { get; set; }

        //[DisplayName("PCI1784U"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public PCI1784Upars PCI1784U { get; set; }

        //[DisplayName("Mitsubishi FX"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public MitFXPars MitFX { get; set; }

        //[DisplayName("COM порт частотных преобразователей"), Browsable(true), De]
        //public ComPortPars ComPortConverters { get; set; }

        //[DisplayName("Частотный преобразователь стоек"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public ConverterPars Converter { get; set; }

//        [DisplayName("Дефектотметчик"), Browsable(true), De]
//        public MarkerPars Marker { get; set; }

        [DisplayName("COM порт АСУ"), Browsable(true), De]
        public ComPortPars ComPortASC { get; set; }

        [DisplayName("Размагничиватель"), Browsable(true), De]
        public DemagnetizerPars Demagnetizer { get; set; }

        [DisplayName("Разное"), Browsable(true), De]
        public DefSomePars Some { get; set; }

        //[Browsable(true), De]
        //public ViewZonePars ViewZone { get; set; }

        //[Browsable(true), De]
        //public SOPPars SOP { get; set; }

        //[Browsable(true), De]
        //public L_WindowLPars Wins { get; set; }

//        public override string ToString() { return (Work.IsWorkCross || Work.IsWorkLine ? "Включен" : "Выключен"); }
    }
}
