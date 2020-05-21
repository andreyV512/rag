using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
namespace PARLIB
{
    public class OI
    {
        public object O = null;
        public PropertyInfo I = null;
        public OI() { }
        public OI(object _root, string _path)
        {
            O = _root;
            if (_path == null)
                return;
            OI oi = new OI();
            oi.O = _root;
            foreach (string s in _path.Split('.'))
            {
                FindNext(ref oi, s);
                if (oi.O == null)
                {
                    O = null;
                    I = null;
                    return;
                }
            }
            O = oi.O;
            I = oi.I;
        }
        void FindNext(ref OI _oi, string _s)
        {
            if (_oi.I != null)
                _oi.O = _oi.I.GetValue(_oi.O, null);
            if (_oi.O == null)
                return;
            _oi.I = _oi.O.GetType().GetProperty(FindObjectString(_s), BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            if (_oi.I == null)
            {
                _oi.O = null;
                return;
            }
            if (!FindBraces(_s))
                return;
            _oi.O = _oi.I.GetValue(_oi.O, null);
            _oi.I = null;
            string ssindex = FindIndex(_s);
            if (ssindex == null)
            {
                _oi.O = null;
                return;
            }
            string sindex = FindStringIndex(ssindex);
            if (sindex != null)
            {
                Type tp = _oi.O.GetType();
                PropertyInfo pi_item = tp.GetProperty("Item", new Type[] { typeof(string) });
                _oi.O = pi_item.GetValue(_oi.O, new object[] { sindex });

                return;
            }
            int iindex = FindIntIndex(ssindex);
            if (iindex >= 0)
            {
                Type tp = _oi.O.GetType();
                PropertyInfo pi_item = tp.GetProperty("Item", new Type[] { typeof(int) });
                _oi.O = pi_item.GetValue(_oi.O, new object[] { iindex });
                return;
            }
            _oi.O = null;
        }
        string FindObjectString(string _s)
        {
            int i = _s.IndexOf('[');
            if (i < 1)
                return (_s);
            return (_s.Substring(0, i));
        }
        bool FindBraces(string _s)
        {
            if (_s.IndexOf('[') >= 0)
                return (true);
            if (_s.IndexOf(']') >= 0)
                return (true);
            return (false);
        }
        string FindIndex(string _s)
        {
            int i0 = _s.IndexOf('[');
            if (i0 < 1)
                return (null);
            int i1 = _s.IndexOf(']');
            if (i1 < i0 + 2)
                return (null);
            if (i1 != _s.Length - 1)
                return (null);
            return (_s.Substring(i0 + 1, i1 - i0 - 1));
        }
        string FindStringIndex(string _s)
        {
            if (_s.Length < 3)
                return (null);
            if (_s[0] != '\"')
                return (null);
            if (_s[_s.Length - 1] != '\"')
                return (null);
            return (_s.Substring(1, _s.Length - 2));
        }
        static int FindIntIndex(string _s)
        {
            int index;
            if (!Int32.TryParse(_s, out index))
                return (-1);
            return (index);
        }
        public override string ToString()
        {
            if (O == null)
                return ("");
            if (Attribute.GetCustomAttribute(I, typeof(PasswordPropertyTextAttribute)) != null)
            {
                using (Cry cry = new Cry())
                {
                    string ret = cry.to(O.ToString());
                    return (ret);
                }
            }
            if (O is Color)
                return (ColorTranslator.ToHtml((Color)O));
            return (O.ToString());
        }
        public object GetValue()
        {
            if (O == null)
                return (null);
            if (I == null)
                return (O);
            if (I.PropertyType.IsEnum)
                return (I.GetValue(O, null).ToString());
            else
                return (I.GetValue(O, null));
        }
        public object GetValue(ref bool _ret)
        {
            _ret = false;
            if (O == null)
                return (null);
            _ret = true;
            if (I == null)
                return (O);
            if (I.PropertyType.IsEnum)
                return (I.GetValue(O, null).ToString());
            else
                return (I.GetValue(O, null));
        }
        public bool SetValue(object _val)
        {
            if (O == null)
                return(false);
            if (I == null)
                O = _val;
            else
            {
                Type tp = I.PropertyType;
                if (tp.IsEnum)
                {
                    object o;
                    o = Enum.Parse(tp, _val as string);
                    I.SetValue(O, o, null);
                }
                else
                    I.SetValue(O, _val, null);
            }
            return (true);
        }
        public bool SetValueStr(string _val)
        {
            if (O == null)
                return(false); 
            if (I == null)
                return (false);
            Type tp = I.PropertyType;
            object o;
            if (tp.IsEnum)
                o = Enum.Parse(tp, _val);
            else
                o = Convert.ChangeType(_val, tp);
            I.SetValue(O, o, null);
            return (true);
        }
    }
}
