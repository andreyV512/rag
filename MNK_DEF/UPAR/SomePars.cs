using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PARLIB;
using UPAR_common;

namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SomePars : ParBase
    {
        [DisplayName("Результат по второму классу"), DefaultValue(false), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsClass2Result { get; set; }
        
        [DisplayName("Разделитель 1"), Browsable(true), De]
        public int Splitter1 { get; set; }

        [DisplayName("Разделитель 2"), Browsable(true), De]
        public int Splitter2 { get; set; }

        [DisplayName("Разделитель 3"), Browsable(true), De]
        public int Splitter3 { get; set; }

        [DisplayName("Количество датчиков в строке"), Browsable(true), De]
        [DefaultValue(4), ConstraintInt(1, 32), TypeConverter(typeof(ConstraintIntTypeConverter))]
        public int SensorsPerLine { get; set; }

        //[DisplayName("Удаленный доступ к именованному каналу"), Browsable(true), De]
        //[TypeConverter(typeof(BooleanconverterRUS))]
        //public bool PipeRemote { get; set; }

        //[DisplayName("Последний зарегистрированный пользователь"), Browsable(true), De]
        //public string LastAuthorisation { get; set; }

        [Browsable(true), De]
        public SignalsViewPars SignalsView { get; set; }

        //[DisplayName("Требуется перезагрузка настроек"), Browsable(true), De]
        //[TypeConverter(typeof(BooleanconverterRUS))]
        //public bool LoadSettings { get; set; }

        [DisplayName("Тестовая длина трубы, мм"), DefaultValue(10000), Browsable(true), De]
        public int TestTubeLength { get; set; }

        [DisplayName("Тестовая скорость трубы, мм/c"), DefaultValue(600), Browsable(true), De]
        public int TestTubeSpeed { get; set; }

        [DisplayName("Панель сигналов"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SignalsPanelPars SignalsPanel { get; set; }

        [DisplayName("Толщина линий порогов"), Browsable(true), De, DefaultValue(1)]
        public int BorderFatness { get; set; }

        [DisplayName("Обмен настройками между СУБД и файлом"), Browsable(true), DefaultValue(true), De, Confirm]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsSaveInter { get; set; }

        [DisplayName("Максимальное количество труб в базе данных"), Browsable(true), De, DefaultValue(1000)]
        public int MaxDBTubes { get; set; }
    }
}
