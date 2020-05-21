using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using Protocol;

namespace PARLIB
{
    public class MetaPar
    {
        ParMainLite parMainLite;
        MetaDesc metaDesc;
        ESource Source;
        public MetaPar(ESource _Source, ParMainLite _parMainLite)
        {
            Source = _Source;
            parMainLite = _parMainLite;
            metaDesc = new MetaDesc();
        }

        public void LoadTree(string _file, string _schema, string _Unit)
        {
            if (Source == ESource.SQL)
                new MetaTreeSQL(_schema,_Unit).Load(parMainLite);
            else
                new MetaTreeFile(_file).Load(parMainLite);
        }
        public void LoadDesc(string _file_desc)
        {
            metaDesc.LoadDescriptions(_file_desc);
            metaDesc.SetDescription(parMainLite);
        }
        public void SaveToSQL(string _schema, string _Unit)
        {
            new MetaTreeSQL(_schema, _Unit).Save(parMainLite);
        }
        public void SaveToFile(string _file)
        {
            new MetaTreeFile(_file).Save(parMainLite);
        }
        public void Save(string _file, string _schema, string _Unit, string _file_desc)
        {

            if (Source == ESource.SQL)
                SaveToSQL(_schema, _Unit);
            else
                SaveToFile(_file);
            metaDesc.SaveDescriptions(parMainLite, _file_desc);
        }

        public void CreateEmpty(object _o)
        {
            if (!(_o is IParent))
                return;
            foreach (PropertyInfo pi in _o.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi, typeof(De)) == null)
                    continue;
                metaDesc.SetDescription(_o, pi);
                object io = (_o as IParent).AddNew(pi);
                CreateEmpty(io);
            }
        }
        public void Copy(object _src, object _dst)
        {
            foreach (PropertyInfo pi_src in _src.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi_src, typeof(De)) == null)
                    continue;
                PropertyInfo pi_dst = _dst.GetType().GetProperty(pi_src.Name);
                if (!metaDesc.SetDescription(_src, pi_src, _dst, pi_dst))
                    continue;
                NoCopyAttribute no_copy = Attribute.GetCustomAttribute(pi_src, typeof(NoCopyAttribute)) as NoCopyAttribute;
                if (no_copy != null)
                    continue;
                object io_dst = (_dst as IParent).AddNew(pi_dst);
                object io_src = pi_src.GetValue(_src, null);
                if (io_dst == null)
                    pi_dst.SetValue(_dst, io_src, null);
                else
                {
                    if (io_dst is IParent)
                        Copy(io_src, io_dst);
                    if (io_dst is IParentList)
                    {
                        foreach (object nio_src in io_src as IEnumerable)
                            Copy(nio_src, (io_dst as IParentList).AddNew());
                        SetCurrentCopy(io_src, io_dst);
                    }
                }
            }
        }
        void SetCurrentCopy(object _src, object _dst)
        {
            Type tp = _src.GetType();
            PropertyInfo pi = tp.GetProperty("Current");
            if (pi == null)
                return;
            MethodInfo mi = tp.GetMethod("get_Item", new Type[] { typeof(int) });
            object ooo = pi.GetValue(_src, null);
            string sval = pi.GetValue(_src, null).ToString();
            pi.SetValue(_dst, mi.Invoke(_dst, new object[] { sval }), null);
        }

        public static string ExecPath(object _o)
        {
            string path = "";
            object parent;
            bool is_index = false;
            IParentBase p = _o as IParentBase;
            parent = p.Parent;
            if (parent == null)
                return (path);
            if (p is IParent)
            {
                IParent pp = p as IParent;
                if (pp.PropertyIndex < 0)
                    path = p.PropertyName;
                else
                {
                    is_index = true;
                    path = "[" + pp.PropertyIndex.ToString() + "]";
                }
            }
            else
                path = p.PropertyName;
            string ret = ExecPath(parent);
            if (!is_index)
                ret += ".";
            ret += path;
            return (ret);
        }
    }
}
