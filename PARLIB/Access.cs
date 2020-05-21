using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;


namespace PARLIB
{
    public class Access 
    {
        EGroup group;
        [DisplayName("Группа"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EGroup Group { get { return (group); } set { group = value; } }

        public Access()
        {
            group = EGroup.Operator;
        }
        public Access(Access _acc)
        {
            Copy(_acc);
        }
        public void Copy(Access _acc)
        {
            group = _acc.group;
        }
        public void Set(string _group)
        {
            if (!Enum.TryParse<EGroup>(_group, out group))
                group = EGroup.Operator;
        }
        public bool CheckUser(User _user)
        {
            return ((_user.Group == EGroup.Master) | (group == _user.Group) | (group == EGroup.Operator));
        }
    }
    //public enum EUnit
    //{
    //    [Description("Дефектоскоп")]
    //    Defect,
    //    [Description("Толщиномер")]
    //    Thick,
    //    [Description("Все")]
    //    All
    //}
    public enum EGroup
    {
        [Description("Мастер")]
        Master,
        [Description("Наладчик")]
        Setter,
        [Description("Оператор")]
        Operator
    }
}
