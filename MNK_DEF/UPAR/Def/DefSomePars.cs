using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Globalization;

using PARLIB;
using UPAR_common;

namespace UPAR.Def
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DefSomePars : ParBase
    {
        class SignalListFileEditor : FileNameEditor { protected override void InitializeDialog(OpenFileDialog openFileDialog) { openFileDialog.Filter = "ini файлы (*.ini)|*.ini|Все файлы (*.*)|*.*"; } }

        [DisplayName("Период основного таймера, мс"), Browsable(true), De, DefaultValue(200)]
        public uint Period { get; set; }

        //[DisplayName("Файл сигналов"), Browsable(true), De]
        //[EditorAttribute(typeof(SignalListFileEditor), typeof(UITypeEditor))]
        //public string SignalListFile { get; set; }

        [DisplayName("Прерывание на просмотр"), DefaultValue(false), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsInterruptView { get; set; }

        //[DisplayName("Брак: Прер. на просм."), DefaultValue(false), Browsable(true), De]
        //[TypeConverter(typeof(BooleanconverterRUS))]
        //public bool IsInterruptViewBrak { get; set; }

        //[DisplayName("Записывать протокол в файл"), DefaultValue(false), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        //public bool ProtocolToFile { get; set; }

        //[DisplayName("Фиксированная группа прочности"), DefaultValue(false), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        //public bool IsSGroupFix { get; set; }

        //[DisplayName("Наименование фиксированной группы прочности"), DefaultValue("E"), Browsable(true), De]
        //public string SGroupFix { get; set; }


        //[DisplayName("Работа с АСУ"), DefaultValue(true), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        //public bool ComWithASU { get; set; }

        [DisplayName("Медианный фильтр"), DefaultValue(true), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool IsMedianFilter { get; set; }

        class MedianConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) { return (new StandardValuesCollection(new List<int>() { 3, 5, 7, 9 })); }
        }
        [DisplayName("Размер медианного фильтра"), DefaultValue(7), TypeConverter(typeof(MedianConverter)), Browsable(true), De]
        public int WidthMedianFilter { get; set; }

        //[DisplayName("Задержка потока сбора, мс"), DefaultValue(10), Browsable(true), De]
        //public int OnLineCycleDelay { get; set; }


        //[DisplayName("Окно сигналов"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(false), Browsable(true), De]
        //public bool SignalsVisible { get; set; }

        //[DisplayName("Сигналы по имени (по имени переменной)"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(false), Browsable(true), De]
        //public bool SignalsHint { get; set; }

        //public class RangeStepConverter : DoubleConverter
        //{
        //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        //    public override StandardValuesCollection GetStandardValues(
        //      ITypeDescriptorContext context)
        //    {
        //        List<double> L = new List<double> { 1, 0.1, 0.01, 0.001}; 
        //        return (new StandardValuesCollection(L));
        //    }
        //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        //    {
        //        return (Convert.ToDouble(value as string));
        //    }
        //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        //    {
        //        return (value.ToString());
        //    }
        //}
        //[DisplayName("Шаг изменения усиления"), Browsable(true), De]
        //[TypeConverter(typeof(RangeStepConverter))]
        //public double RangeStep { get; set; }

        [DisplayName("Параметры сохранения труб"), Browsable(true), De]
        public SaveFilePars SaveFile { get; set; }

        //[EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor)), DisplayName("Путь сохранения СОП"), Browsable(true), De]
        //public string SOPPath { get; set; }

        //[DisplayName("Макс. количество сохраненных труб"), Browsable(true), De]
        //public int MaxCountSaveTubes { get; set; }

        //[DisplayName("Период таймера перезагрузки, ч"), Browsable(true), De]
        //public int MuchWinWorkTimeInterval { get; set; }

        //[DisplayName("Период перезагрузки, ч"), Browsable(true), De]
        //public int MuchWinWorkTime { get; set; }

        [DisplayName("Период проверки зон, мс"), Browsable(true), De]
        public int CheckZonePeriod { get; set; }

        [DisplayName("Протокол"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ProtocolPar Protocol { get; set; }

        //[DisplayName("Имя мьютекса"), Browsable(true), De]
        //public string MutexName { get; set; }

        //[DisplayName("Транспорт тест"), Browsable(true), De]
        //[TypeConverter(typeof(BooleanconverterRUS))]
        //public bool TransportTest { get; set; }

        //[DisplayName("Режим автомат"), Browsable(true), DefaultValue(true), De]
        //[TypeConverter(typeof(BooleanconverterRUS))]
        //public bool IsAuto { get; set; }

        //[DisplayName("Панели по умолчанию"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        //public bool PanelsDefault { get; set; }

        [DisplayName("Высота графика датчиков"), Browsable(true), De, DefaultValue(100)]
        public int SensorHeight { get; set; }

        ETypeView typeView;
        [DisplayName("Тип просмотра"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public ETypeView TypeView { get { return (typeView); } set { typeView = value; } }

        public enum ETypeView
        {
            [Description("Дефекты")]
            Defect,

            [Description("Дефекты по зоне и всем датчикам")]
            DefectZone,

            [Description("Дефекты в столбец")]
            Column,

            [Description("Дефекты по 3 зонам и одному датчику")]
            DefectRange,

            [Description("Исходные сигналы")]
            Source
        }
        [DisplayName("Режим Тест с магнитными полями"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool TestWithMagnit { get; set; }
    }
}
