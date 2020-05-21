using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;


namespace UPAR
{
    public class DefectWork : WorkPars
    {
        [DisplayName("Поперечный в работе"), DefaultValue(false), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsWorkCross { get; set; }

        [DisplayName("Продольный в работе"), DefaultValue(false), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsWorkLine { get; set; }

        [DisplayName("Группа прочности в работе"), DefaultValue(false), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsWorkSG { get; set; }

        public override string ToString()
        { 
            string ret="";
            if(IsWorkCross)
                ret+="Поперечный";
            if(IsWorkLine)
            {
                if(ret.Length!=0)
                    ret+=",";
                ret+="Продольный";
            }
            if (IsWorkSG)
            {
                if (ret.Length != 0)
                    ret += ",";
                ret += "ГП";
            }
            return (ret);
        }
    }
}
