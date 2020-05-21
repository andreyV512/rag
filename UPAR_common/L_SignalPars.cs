using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(CollectionTypeConverter)), Editor(typeof(FLBase.Editor), typeof(UITypeEditor))]
    [DisplayName("Сигналы")]
    [Sortable]
    public class L_SignalPars : ParListBase<SignalPars>
    {
        public override string ToString() { return (string.Format("<{0}>", Count)); }
        public override object AddNew()
        {
            int position_new = FindNewPosition();
            SignalPars p = base.AddNew() as SignalPars;
            p.Name = FindNewName();
            p.Position = position_new;
            return (p);
        }

        private int FindNewPosition()
        {
            int position = -1;
            foreach (SignalPars p in this)
            {
                if (p.Position >= position)
                    position = p.Position;
            }
            return (position+1);
        }
        public override SignalPars this[string _name]
        {
            get
            {
                foreach (SignalPars p in this)
                    if (p.Name == _name)
                        return (p);
                return (null);
            }
        }
        string FindNewName()
        {
            for (int i = 0; ; i++)
            {
                if (this["Новый" + i.ToString()] == null)
                    return ("Новый" + i.ToString());
            }
        }
        public List<string> ToCS()
        {
            List<string> L = new List<string>();
            foreach (SignalPars p in this)
            {
                if (!p.Input)
                    continue;
                L.AddRange(p.ToCS());
                L.Add("");
            }
            L.Add("");
            foreach (SignalPars p in this)
            {
                if (p.Input)
                    continue;
                L.AddRange(p.ToCS());
                L.Add("");
            }
            return (L);
        }
    }
}
