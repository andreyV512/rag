using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.IO;

namespace PARLIB
{
    class MetaDesc
    {
        Descs descs = null;

        public void LoadDescriptions(string _fname)
        {
            descs = new Descs(_fname);
        }
        public void SaveDescriptions(ParMainLite _parMainLite, string _fname)
        {
            Descs descsOut = GetDescription(_parMainLite);
            if (descsOut != null)
                descsOut.Save(_fname);
        }
        public bool SetDescription(object _src, PropertyInfo _pi_src, object _dst, PropertyInfo _pi_dst)
        {
            BrowsableAttribute ba_src = Attribute.GetCustomAttribute(_pi_src, typeof(BrowsableAttribute)) as BrowsableAttribute;
            if (!ba_src.Browsable)
            {
                PropertyDescriptor pd = TypeDescriptor.GetProperties(_pi_dst.DeclaringType)[_pi_dst.Name];
                BrowsableAttribute ba = (BrowsableAttribute)pd.Attributes[typeof(BrowsableAttribute)];
                if (ba != null)
                {
                    FieldInfo isBrowsable = ba.GetType().GetField("Browsable", BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
                    isBrowsable.SetValue(ba, false);
                }
            }
            return (true);
        }
        public void SetDescription(object _o)
        {
            foreach (PropertyInfo pi in _o.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi, typeof(De)) == null)
                    continue;
                SetDescription(_o, pi);
                object io = pi.GetValue(_o, null);
                if (io is IParent)
                    SetDescription(io);
                if (io is IParentList)
                {
                    foreach (object nio in (io as IEnumerable))
                        SetDescription(nio);
                }
            }
        }
        public void SetDescription(object _parent, PropertyInfo _pi)
        {
            De de = Attribute.GetCustomAttribute(_pi, typeof(De)) as De;
            if (de == null)
                return;
            HideAttribute hide = Attribute.GetCustomAttribute(_pi, typeof(HideAttribute)) as HideAttribute;
            if (hide != null)
                return;
            if (descs == null)
                return;
            PropertyDescriptor pd = TypeDescriptor.GetProperties(_pi.DeclaringType)[_pi.Name];
            de = (De)pd.Attributes[typeof(De)];
            string ckey = _parent.GetType().Name + "." + _pi.Name;
            Desc desc = descs[_parent.GetType().Name + "." + _pi.Name];
            if (desc == null)
                desc = new Desc();
            de.Acc = desc.accsess;
            de.Description = desc.description;
            BrowsableAttribute ba = (BrowsableAttribute)pd.Attributes[typeof(BrowsableAttribute)];
            if (ba != null)
            {
                FieldInfo isBrowsable = ba.GetType().GetField("Browsable", BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
                if (de.Browsable)
                    isBrowsable.SetValue(ba, desc.accsess.CheckUser(User.current));
            }
            return;
        }
        Descs GetDescription(ParMainLite _parMainLite)
        {
            Descs ret = new Descs();
            GetExec(_parMainLite, ret);
            if (descs == null)
                return (null);
            foreach (string ckey in descs.Keys)
            {
                if (ret[ckey] == null)
                {
                    Desc desc = descs[ckey];
                    desc.actual = false;
                    ret[ckey] = desc;
                }
            }
            return (ret);
        }
        void GetExec(object _o, Descs _descsOut)
        {
            foreach (PropertyInfo pi in _o.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi, typeof(De)) == null)
                    continue;
                GetExec(_o, pi, _descsOut);
                object io = pi.GetValue(_o, null);
                if (io is IParent)
                    GetExec(io, _descsOut);
                if (io is IParentList)
                {
                    foreach (object nio in (io as IEnumerable))
                        GetExec(nio, _descsOut);
                }
            }
        }
        void GetExec(object _parent, PropertyInfo _pi, Descs _descsOut)
        {
            De de = Attribute.GetCustomAttribute(_pi, typeof(De)) as De;
            if (de == null)
                return;
            PropertyDescriptor pd = TypeDescriptor.GetProperties(_pi.DeclaringType)[_pi.Name];
            de = (De)pd.Attributes[typeof(De)];
            Desc desc = new Desc();
            desc.accsess = de.Acc;
            desc.description = de.Description;
            desc.actual = true;
            string ckey = _parent.GetType().Name + "." + _pi.Name;
            _descsOut[ckey] = desc;
        }
        //Desc this[string _key]
        //{
        //    get
        //    {
        //        Desc desc;
        //        if (!descs.TryGetValue(_key, out desc))
        //        {
        //            desc = new Desc();
        //            descs.Add(_key, desc);
        //        }
        //        return (desc);
        //    }
        //    set
        //    {
        //        descs.Remove(_key);
        //        descs.Add(_key, value);
        //    }
        //}
    }
}
