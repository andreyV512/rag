using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PARLIB
{
    public class ParMain : ParMainLite
    {
        [Browsable(true), De]
        public L_User Users { get; set; }

        [DisplayName("Последний пользователь"), Browsable(false), De(false)]
        public string LastUser { get; set; }

        [DisplayName("Регистрация пользователя"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsLogin { get; set; }

        protected ParMain(ESource _Source, string _path, string _file, string _schema, string _Unit, string _file_desc)
            : base(_Source, _path, _file, _schema, _Unit, _file_desc) { }
    }
}
