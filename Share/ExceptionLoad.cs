using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Share
{
    public class ExceptionLoad : Exception
    {
        public ExceptionLoad() : base("Ошибка загрузки файла") { }
        public ExceptionLoad(string message) : base("Ошибка загрузки файла: " + message) { }
        public ExceptionLoad(string message, Exception inner) : base("Ошибка загрузки файла: " + message, inner) { }
    }
}
