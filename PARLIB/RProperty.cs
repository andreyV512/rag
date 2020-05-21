using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace PARLIB
{
    public class DynamicAttribute : Attribute { }
    public class NoCopyAttribute : Attribute { }
    public class SortableAttribute : Attribute { }
    public class ConfirmAttribute : Attribute { }
    public class HideAttribute : Attribute { }
    public class DNameAttribute : Attribute 
    {
        public string Name;
        public DNameAttribute(string _Name)
        {
            Name = _Name;
        }
    }
    public class SignalInputAttribute : Attribute
    {
        public bool Input;
        public SignalInputAttribute(bool _Input)
        {
            Input = _Input;
        }
    }

    public class De : Attribute
    {
        public De()
        {
            acc = new Access();
            Browsable = true;
        }
        public bool Browsable { get; private set; }
        public De(bool _Browsable)
        {
            acc = new Access();
            Browsable = _Browsable;
        }
        Access acc;
        public Access Acc { get { return (acc); } set { acc = value; } }
        public string Description { get; set; }
    }

    public class ConstraintIntAttribute : Attribute
    {
        public ConstraintIntAttribute(int _low, int _high)
        {
            Low = _low;
            High = _high;
        }
        public int Low { get; private set; }
        public int High { get; private set; }
    }
    public class ConstraintIntTypeConverter : Int32Converter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            object ret = base.ConvertFrom(context, culture, value);
            if (value != null && value is string)
            {
                ConstraintIntAttribute cia = context.PropertyDescriptor.Attributes[typeof(ConstraintIntAttribute)] as ConstraintIntAttribute;
                if (cia != null)
                {
                    int v = (int)ret;
                    if (v < cia.Low || v > cia.High)
                        throw new NotSupportedException(string.Format("Значение должно быть от {0} до {1}", cia.Low.ToString(), cia.High.ToString()));
                }
            }
            return (ret);
        }
    }
    public class ConstraintIntNonZeroTypeConverter : Int32Converter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            object ret = base.ConvertFrom(context, culture, value);
            if (value != null && value is string)
            {
                ConstraintIntAttribute cia = context.PropertyDescriptor.Attributes[typeof(ConstraintIntAttribute)] as ConstraintIntAttribute;
                if (cia != null)
                {
                    int v = (int)ret;
                    if (v == 0 || v < cia.Low || v > cia.High)
                        throw new NotSupportedException(string.Format("Значение должно быть от {0} до {1}, но не равно 0", cia.Low.ToString(), cia.High.ToString()));
                }
            }
            return (ret);
        }
    }
}

