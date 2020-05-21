using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Drawing.Design;

namespace PARLIB
{
    public interface IParentBase
    {
        string PropertyName { get; set; }
        object Parent { get; set; }
        ParMainLite parMainLite { get; }
    }
    public interface IParent 
    {
        object AddNew(PropertyInfo _pi);
        int PropertyIndex { get; set; }
    }
    public interface IParentList : IEnumerable
    {
        object AddNew();
        object AddNewTree(MetaPar _MP);
        void RemoveOld(object _o);
        int ListCount();
        object GetItem(int _index);
        bool Move(int _index_src, int _index_trg);
        void Clear();
    }
    public class ParBase : IParent, IParentBase
    {
        [Browsable(false)]
        public object Parent { get; set; }
        [Browsable(false)]
        public string PropertyName { get; set; }
        [Browsable(false)]
        public int PropertyIndex { get; set; }
        public object AddNew(PropertyInfo _pi)
        {
            Type tp = _pi.PropertyType;
            if(tp.GetInterface("IParentBase")==null)
                return (null);
            object o = Activator.CreateInstance(_pi.PropertyType, null);
            _pi.SetValue(this, o, null);
            IParentBase p = o as IParentBase;
            p.Parent = this;
            p.PropertyName = _pi.Name;
            if (o is IParent)
                (p as IParent).PropertyIndex = -1;
            return (o);
        }
        public override string ToString() { return (""); }
        [Browsable(false)]
        public ParMainLite parMainLite
        {
            get
            {
                object o = this;
                for (; ; )
                {
                    if (o is IParentBase)
                    {
                        IParentBase p = o as IParentBase;
                        if (p.Parent == null)
                            return (p as ParMainLite);
                        else
                            o = p.Parent;
                    }
                    else
                        return (null);
                }
            }
        }
        public void SimpleCopy(ParBase _src)
        {
            Type tp_dst = GetType();
            Type tp_src = _src.GetType();
            if (tp_dst != tp_src)
                return;
            foreach (PropertyInfo pi_src in _src.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi_src, typeof(De)) == null)
                    continue;
                NoCopyAttribute no_copy = Attribute.GetCustomAttribute(pi_src, typeof(NoCopyAttribute)) as NoCopyAttribute;
                if (no_copy != null)
                    continue;
                PropertyInfo pi_dst = tp_dst.GetProperty(pi_src.Name);
                pi_dst.SetValue(this, pi_src.GetValue(_src, null),null);
            }
        }
    }
    public class CollectionTypeConverter : TypeConverter
    {
        public override object ConvertTo(
          ITypeDescriptorContext context, CultureInfo culture,
          object value, Type destType)
        {
            MethodInfo mi = value.GetType().GetMethod("ToString");
            return (mi.Invoke(value, null));
        }
    }
    [TypeConverter(typeof(CollectionTypeConverter))]
    [Editor(typeof(FLBase.Editor), typeof(UITypeEditor))]
    public class ParListBase<T> : List<T>, IParentList, IParentBase
    {
        [Browsable(false)]
        public object Parent { get; set; }
        [Browsable(false)]
        public string PropertyName { get; set; }
        [Browsable(false)]
        public ParMainLite parMainLite
        {
            get
            {
                object o = this;
                for (; ; )
                {
                    if (o is IParentBase)
                    {
                        IParentBase p = o as IParentBase;
                        if (p.Parent == null)
                            return (p as ParMainLite);
                        else
                            o = p.Parent;
                    }
                    else
                        return (null);
                }
            }
        }
        public virtual T this[string _num] { get { return (this[Convert.ToInt32(_num)]); } }
//        public new T this[int _num] { get { return (_num < Count ? base[_num] : default(T)); } }
        public virtual object AddNew()
        {
            Type tp = typeof(T);
            if (tp.GetInterface("IParent") == null)
                return (null);
            object o = Activator.CreateInstance(tp, null);
            Add((T)o);
            IParentBase p = o as IParentBase;
            p.Parent = this;
            p.PropertyName = null;
            (p as IParent).PropertyIndex = this.IndexOf((T)o);
            return (o);
        }
        public virtual object AddNewTree(MetaPar _MP)
        {
            object o = AddNew();
            _MP.CreateEmpty(o);
            return (o);
        }
        public void RemoveOld(object _o)
        {
            Remove((T)_o);
            for (int i = 0; i < Count; i++)
            {
                IParent p = this[i] as IParent;
                p.PropertyIndex = i;
            }
        }
        public override string ToString()
        {
            return ("<" + Count.ToString() + ">");
        }
        public int ListCount()
        {
            return(base.Count);
        }
        public object GetItem(int _index)
        {
            return (this[_index]);
        }
        public bool Move(int _index_src, int _index_trg)
        {
            if (_index_src < 0 || _index_src >= base.Count)
                return (false);
            if (_index_trg < 0 || _index_trg >= base.Count)
                return (false);
            if (_index_src == _index_trg)
                return (true);
            T o = this[_index_src];
            RemoveAt(_index_src);
            Insert(_index_trg, o);
            foreach (T oo in this)
                (oo as IParent).PropertyIndex = this.IndexOf(oo);
            return (true);
        }
    }
}
