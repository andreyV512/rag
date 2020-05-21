using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using PARLIB;


namespace UPAR
{
    //public enum ETState
    //{
    //    [Description("Ошибка")]
    //    Error,
    //    [Description("Ожидание")]
    //    Wait,
    //    [Description("Готовность")]
    //    Ready,
    //    [Description("Работа")]
    //    Work,
    //    [Description("Вращение")]
    //    Rotate,
    //    [Description("Завершено")]
    //    Complete,
    //    [Description("Неопределенное")]
    //    None
    //}
    //public enum ETCommand
    //{
    //    [Description("Нет команд")]
    //    None,
    //    [Description("Ожидание")]
    //    Wait,
    //    [Description("Готовность")]
    //    Ready,
    //    [Description("Работа")]
    //    Work,
    //    [Description("Вращение")]
    //    Rotate
    //}


    public enum ETState
    {
        [Description("Неопределенное")]
        None,
        [Description("Готовность")]
        Ready,
        [Description("Работа")]
        Work,
        [Description("Ошибка")]
        Error
    }
    public enum ETCommand
    {
        [Description("Нет")]
        None,
        [Description("Пуск")]
        Start,
        [Description("Стоп")]
        Stop
    }


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WorkPars : ParBase
    { 
        [DisplayName("Состояние"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public ETState State { get; set; }

        [DisplayName("Команда"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public ETCommand Command { get; set; }

        [DisplayName("Сигнал"), Browsable(true), De]
        public string SigCommang { get; set; }

        [DisplayName("Период сигналов"), Browsable(true), De, DefaultValue(500)]
        public int SigTimeout { get; set; }

        [DisplayName("Количество попыток"), Browsable(true), De, DefaultValue(3)]
        public int SigIter { get; set; }

        class PathExeEditor : FileNameEditor { protected override void InitializeDialog(OpenFileDialog openFileDialog) { openFileDialog.Filter = "ini файлы (*.exe)|*.exe"; } }

        [DisplayName("Путь"), Browsable(true), De]
        [EditorAttribute(typeof(PathExeEditor), typeof(UITypeEditor))]
        public string Path { get; set; }

        [DisplayName("Запускать сразу"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsStart { get; set; }

    }
}
