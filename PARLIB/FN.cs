using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace PARLIB
{
    static public class FN
    {
        public static void MBox(string _msg)
        {
            MessageBox.Show(_msg);
        }
        public static void fatal(string _msg)
        {
            throw new Exception(_msg);
            //            MessageBox.Show(_msg);
            //            Environment.Exit(-1);
        }
        public static string sql_str(string _s)
        {
            return ("'" + _s + "'");
        }
        public static void SetPropertyGridSplitter(PropertyGrid _propertyGrid, int _width)
        {
            var realType = _propertyGrid.GetType();
            while (realType != null && realType != typeof(PropertyGrid))
            {
                realType = realType.BaseType;
            }

            var gvf = realType.GetField(@"gridView",
                BindingFlags.NonPublic |
                BindingFlags.GetField |
                BindingFlags.Instance);
            var gv = gvf.GetValue(_propertyGrid);

            var mtf = gv.GetType().GetMethod(@"MoveSplitterTo",
                BindingFlags.NonPublic |
                BindingFlags.InvokeMethod |
                BindingFlags.Instance);
            mtf.Invoke(gv, new object[] { _width });
        }
        public static int GetPropertyGridSplitter(PropertyGrid _propertyGrid)
        {
            var realType = _propertyGrid.GetType();
            while (realType != null && realType != typeof(PropertyGrid))
            {
                realType = realType.BaseType;
            }
            var gvf = realType.GetField(@"gridView",
                BindingFlags.NonPublic |
                BindingFlags.GetField |
                BindingFlags.Instance);
            var gv = gvf.GetValue(_propertyGrid);
            var mtf = gv.GetType();
            int width = (int)mtf.InvokeMember("GetLabelWidth", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance, null, gv, new object[] { });
            return (width);
        }
        public static int GetPropertyGridSplitter2(PropertyGrid _propertyGrid)
        {
            foreach (Control c in _propertyGrid.Controls)
            {
                if (c.GetType().Name == "DocComment")
                    return ((int)c.GetType().GetProperty("Lines").GetValue(c, null));
            }
            return (0);
        }
        public static void SetPropertyGridSplitter2(PropertyGrid _propertyGrid, int _width)
        {
            object o = _width;
            foreach (Control c in _propertyGrid.Controls)
            {
                if (c.GetType().Name == "DocComment")
                {
                    c.GetType().GetProperty("Lines").SetValue(c, _width, null);
                    c.GetType().BaseType.GetField("userSized",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic).SetValue(c, true);
                }
            }
        }

        public static object GetProperty(object _instance, string _name)
        {
            PropertyInfo pi = _instance.GetType().GetProperty(_name);
            //            FieldInfo fi = _instance.GetType().GetField(_name, BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance);
            //            FieldInfo fi = _instance.GetType().GetField(_name, BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Static);
            if (pi == null)
                return (null);
            return (pi.GetValue(_instance, null));
        }
        /*
                public static string ToIniString(object _val)
                {
                    if (_val == null)
                        return ("0");
                    Type tp = _val.GetType();
                    if (tp == typeof(string))
                        return(_val as string);
                    if (tp == typeof(int))
                        return(((int)_val).ToString());
                    if (tp == typeof(double))
                        return(((double)_val).ToString(System.Globalization.CultureInfo.GetCultureInfo("ru-RU")));
                    if (tp == typeof(float))
                        return (((float)_val).ToString(System.Globalization.CultureInfo.GetCultureInfo("ru-RU")));
                    if (tp == typeof(bool))
                        return(((bool)_val) ? "1" : "0");
                    else
                        throw new InvalidOperationException("FN.ToIniString: Неопределенный тип: " + tp.ToString());
        //                FN.fatal("FN.ToIniString: Неопределенный тип: " + tp.ToString());
                }
         */
    }
}
