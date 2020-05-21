using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PARLIB;

namespace UPAR
{
    public class Client:ParBase
    {
        [DisplayName("Имя"), Browsable(true), De]
        public string Name { get; set; }

        public override string ToString() { return (Name); }
    }
}
