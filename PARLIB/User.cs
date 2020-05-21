using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;


namespace PARLIB
{
    public class User : ParBase
    {
        [DisplayName("Имя"), Browsable(true), De]
        public string Name { get; set; }

        [DisplayName("Группа"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EGroup Group { get; set; }

        [DisplayName("Пароль"), PasswordPropertyText(true), Browsable(true), De]
        public string Pwd {get;set;}

        public override string ToString() { return (Name); }

        static User Default()
        {
            User u = new User();
            u.Name = "Master";
            u.Group = EGroup.Master;
            return (u);
        }

        public static User current = Default();
    }
}
