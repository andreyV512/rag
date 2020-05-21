using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;

namespace PARLIB
{
    abstract class MetaTree
    {
        protected LParam L = new LParam();
        abstract public void Load(ParMainLite _O);
        abstract public void Save(ParMainLite _O);
        protected void ExecSave(object _o, string _path)
        {
            foreach (PropertyInfo pi in _o.GetType().GetProperties())
            {
                Attribute at = Attribute.GetCustomAttribute(pi, typeof(De));
                if ((Attribute.GetCustomAttribute(pi, typeof(De)) as De) == null)
                    continue;
                string ipath = _path + "." + pi.Name;
                object io = pi.GetValue(_o, null);
                if (io is IParentBase)
                {
                    L.Add(ipath, null);
                    ExecSave(io, ipath);
                    if (io is IParentList)
                    {
                        foreach (object nio in io as IEnumerable)
                        {
                            string nio_path = ipath + string.Format("[{0}]", (nio as IParent).PropertyIndex.ToString("00000"));
                            L.Add(nio_path, null);
                            ExecSave(nio, nio_path);
                        }
                        SaveCurrent(io as IParentList, ipath);
                    }
                }
                else
                    L.Add(ipath, StringByPiObject(pi, io));
            }
        }
        protected void SaveCurrent(IParentList _o, string _path)
        {
            PropertyInfo pi = _o.GetType().GetProperty("Current");
            if (pi == null)
                return;
            IParent o_current = pi.GetValue(_o, null) as IParent;
            if (o_current != null)
                L.Add(_path + ".Current", o_current.PropertyIndex.ToString());
        }
        protected string StringByPiObject(PropertyInfo _pi, Object _O)
        {
            if (_O == null)
                return ("");
            if (Attribute.GetCustomAttribute(_pi, typeof(PasswordPropertyTextAttribute)) != null)
            {
                using (Cry cry = new Cry())
                {
                    string ret = cry.to(_O.ToString());
                    return (ret);
                }
            }
            if (_O is Color)
                return (ColorTr.ToWin32((Color)_O));
            return (_O.ToString());
        }
        protected object GetValue(PropertyInfo _pi, string _path, string _sval)
        {
            if (_sval == null)
            {
                DefaultValueAttribute dva = Attribute.GetCustomAttribute(_pi, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                if (dva != null)
                {
                    if (dva.Value == null)
                    {
                        if (_pi.PropertyType.IsGenericType && _pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            return (null);
                    }
                    return (Convert.ChangeType(dva.Value, _pi.PropertyType));
                }
            }
            Type tp = _pi.PropertyType;
            if (tp == typeof(string))
                return (_sval);
            if (tp == typeof(int))
                return (Convert.ToInt32(_sval));
            if (tp == typeof(int?))
                return (Convert.ToInt32(_sval));
            if (tp == typeof(uint))
                return (Convert.ToUInt32(_sval));
            if (tp == typeof(double))
                return (Convert.ToDouble(_sval, System.Globalization.CultureInfo.GetCultureInfo("ru-RU")));
            if (tp == typeof(float))
                return (Convert.ToSingle(_sval, System.Globalization.CultureInfo.GetCultureInfo("ru-RU")));
            if (tp == typeof(bool))
                return (_sval == "True");
            if (tp == typeof(short))
                return (Convert.ToInt16(_sval, System.Globalization.CultureInfo.GetCultureInfo("ru-RU")));
            if (tp == typeof(char))
            {
                if (_sval == null)
                    return (' ');
                return (_sval.Length == 0 ? ' ' : _sval[0]);
            }
            if (tp == typeof(Color))
                return (ColorTr.From(_sval));
            if (tp.BaseType == typeof(Enum))
            {
                object eo = null;
                try
                {
                    eo = Enum.Parse(tp, _sval);
                }
                catch
                {
                    eo = null;
                }
                return (eo);
            }
            throw new InvalidOperationException("MetaPar.GetValue: Неопределенный тип: " + tp.ToString());
            //                FN.fatal("FN.ToDBString: Неопределенный тип: " + tp.ToString());
        }
        protected void ExecLoad(SerialTree _serialTree, object _o, string _path)
        {
            foreach (PropertyInfo pi in _o.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi, typeof(De)) == null)
                    continue;
                string ipath = _path + "." + pi.Name;
                object io = (_o as IParent).AddNew(pi);
                if (io == null)
                {
                    if (Attribute.GetCustomAttribute(pi, typeof(PasswordPropertyTextAttribute)) == null)
                        pi.SetValue(_o, GetValue(pi, ipath, _serialTree[ipath]), null);
                    else
                    {
                        using (Cry cry = new Cry())
                        {
                            string bpwd = cry.from(_serialTree[ipath]);
                            pi.SetValue(_o, bpwd, null);
                        }
                    }
                }
                if (io is IParent)
                    ExecLoad(_serialTree, io, ipath);
                if (io is IParentList)
                {
                    foreach (string npath in _serialTree.GetList(ipath))
                        ExecLoad(_serialTree, (io as IParentList).AddNew(), npath);
                    SetCurrent(_serialTree, io, ipath);
                }
            }
        }
        void SetCurrent(SerialTree _serialTree, object _o, string _path)
        {
            Type tp = _o.GetType();
            PropertyInfo pi = tp.GetProperty("Current");
            if (pi == null)
                return;
            int ival = Convert.ToInt32(_serialTree[_path + ".Current"]);
            MethodInfo mi = tp.GetMethod("get_Item", new Type[] { typeof(int) });
            try
            {
                pi.SetValue(_o, mi.Invoke(_o, new object[] { ival }), null);
            }
            catch
            {
                pi.SetValue(_o, null, null);
            }
        }
    }
}
