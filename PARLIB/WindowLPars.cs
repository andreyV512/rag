using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;


namespace PARLIB
{
    public class WindowLPars : ParBase
    {
        [DisplayName("Наименование"), Browsable(true), De]
        public string Name { get; set; }

        [DisplayName("Положение слева"), Browsable(true), De]
        public int Left { get; set; }

        [DisplayName("Положение сверху"), Browsable(true), De]
        public int Top { get; set; }

        [DisplayName("Ширина"), Browsable(true), De]
        public int Width { get; set; }

        [DisplayName("Высота"), Browsable(true), De]
        public int Height { get; set; }

        [DisplayName("Разделитель 1"), Browsable(true), De]
        public int Splitter1 { get; set; }

        [DisplayName("Разделитель 2"), Browsable(true), De]
        public int Splitter2 { get; set; }

        [DisplayName("Дистанция"), Browsable(true), De]
        public int SplitterDistance { get; set; }

        [DisplayName("Показывать при загрузке"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(false)]
        public bool IsVisible { get; set; }

        public override string ToString() { return (Name==null?"":Name); 
        }

        public string ToStringFull() {
            return (string.Format("{0}[L{1},W{2},T{3},H{4}]",
            Name,
            Left.ToString(),
            Width.ToString(),
            Top.ToString(),
            Width.ToString()));
        }
    }

}
