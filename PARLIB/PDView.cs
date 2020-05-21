using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Protocol;

namespace PARLIB
{
    public partial class PDView : UserControl
    {
        public PDView()
        {
            InitializeComponent();
        }
        public PropertyGrid propertyGrid2 { get { return (propertyGrid1); } }
        public int Splitter1
        {
            get
            {
                return (FN.GetPropertyGridSplitter(propertyGrid1));
            }
            set
            {
                FN.SetPropertyGridSplitter(propertyGrid1, value);
            }
        }
        public int Splitter2
        {
            get
            {
                return (splitContainer1.SplitterDistance);
            }
            set
            {
                splitContainer1.SplitterDistance = value;
            }
        }
        ParMainLite parMainLite = null;
        public object SelectedObject
        {
            set
            {
                if (value is ParMainLite)
                    parMainLite = value as ParMainLite;
                else if (value is IParentBase)
                    parMainLite = (value as IParentBase).parMainLite;
                propertyGrid1.SelectedObject = value;
            }
        }
        string current_name = null;
        string current_key = null;
        object current_default = null;
        De de;
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            GridItem g = propertyGrid2.SelectedGridItem;
            if (g == null)
            {
                e.Cancel = true;
                return;
            }

            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Items.Add(MetaPar.ExecPath(g.Parent.Value) + "." + g.PropertyDescriptor.Name).Click += new System.EventHandler(MenuItem_Click);
            contextMenuStrip1.Items.Add(g.PropertyDescriptor.ComponentType.FullName).Click += new System.EventHandler(MenuItem_Click);

            Type ptp = g.Parent.Value.GetType();
            PropertyInfo pi = ptp.GetProperty(g.PropertyDescriptor.Name);
            de = FindDe(pi);

            DisplayNameAttribute dn = Attribute.GetCustomAttribute(pi, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            if (dn == null)
                current_name = null;
            else
                current_name = dn.DisplayName;

            current_key = ptp.Name + "." + g.PropertyDescriptor.Name;
            if (current_key != null)
                contextMenuStrip1.Items.Add("Изменить доступ").Click += new System.EventHandler(ChangeDescription);
            if (!(g.Value is IParentBase))
            {
                DefaultValueAttribute dva = Attribute.GetCustomAttribute(pi, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                if (dva != null)
                {
                    string dv = dva.Value.ToString();
                    if (dv.Length != 0)
                    {
                        contextMenuStrip1.Items.Add("По умолчанию: " + dv).Click += new System.EventHandler(DefaultValue);
                        current_default = dva.Value;
                    }
                }
                if (g.Parent.Value is IParent)
                {
                    if ((g.Parent.Value as IParent).PropertyIndex >= 0)
                        contextMenuStrip1.Items.Add("Установить для всех: " + g.Value.ToString()).Click += new System.EventHandler(ListValue);
                }
            }
        }
        De FindDe(PropertyInfo _pi)
        {
            if (_pi == null)
                return (null);
            PropertyDescriptor pd = TypeDescriptor.GetProperties(_pi.DeclaringType)[_pi.Name];
            if (pd == null)
                return (null);
            De de = (De)pd.Attributes[typeof(De)];
            return (de);
        }
        De FindDe(GridItem _g)
        {
            if (_g == null)
                return (null);
            Type ptp = _g.Parent.Value.GetType();
            if (ptp == null)
                return (null);
            return (FindDe(ptp.GetProperty(_g.PropertyDescriptor.Name)));
        }
        void MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText((sender as ToolStripItem).Text);
            }
            catch
            {
            }
        }
        void ChangeDescription(object sender, EventArgs e)
        {
            if (current_key == null)
                return;
            using (FDescription f = new FDescription(de.Acc, current_key, current_name, parMainLite))
            {
                f.ShowDialog();
            }
        }
        void DefaultValue(object sender, EventArgs e)
        {
            GridItem g = propertyGrid2.SelectedGridItem;
            object parent = g.Parent.Value;
            PropertyInfo pi = parent.GetType().GetProperty(g.PropertyDescriptor.Name);
            pi.SetValue(parent, current_default, null);
            propertyGrid2.Refresh();
        }
        void ListValue(object sender, EventArgs e)
        {
            GridItem g = propertyGrid2.SelectedGridItem;
            object val = g.Value;
            GridItem parent = g.Parent;
            PropertyInfo pi = parent.Value.GetType().GetProperty(g.PropertyDescriptor.Name);
            if (parent == null)
                return;
            IParentBase  pval = parent.Value as IParentBase;
            if (pval == null)
                return;
            IParentList plist = pval.Parent as IParentList;
            if (plist == null)
                return;
            ConfirmAttribute cf = Attribute.GetCustomAttribute(pi, typeof(ConfirmAttribute)) as ConfirmAttribute;
            //if (cf != null)
            //{
            //    if (MessageBox.Show("Подтвердите изменения", "Внимание", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            //        return;
            //}
            int count = plist.ListCount();
            for (int i = 0; i < count; i++)
            {
                object lval = plist.GetItem(i);
                pi.SetValue(lval, val, null);
            }
            propertyGrid2.Refresh();
            if (OnRefresh != null)
                OnRefresh();
        }
        De lde = null;
        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            lde = FindDe(e.NewSelection);
            if (lde != null)
            {
                try
                {
                    richTextBox1.Rtf = lde.Description;
                }
                catch
                {
                    richTextBox1.Text = lde.Description;
                }
            }
        }
        void pr(string _msg) { ProtocolST.pr("PDView: " + _msg); }
        private void richTextBox1_Validated(object sender, EventArgs e)
        {
            if (lde != null)
                lde.Description = richTextBox1.Rtf;
        }
        public void Save()
        {
            if (richTextBox1.ReadOnly)
                return;
            if (lde != null)
                lde.Description = richTextBox1.Rtf;
        }
        public delegate void DOnValueChanged(object _v);
        public DOnValueChanged OnValueChanged = null;
        public delegate void DOnRefresh();
        public DOnRefresh OnRefresh = null;

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyDescriptor pd = e.ChangedItem.PropertyDescriptor;
            Type tc = pd.ComponentType;
            Type tp = pd.PropertyType;
            PropertyInfo pi = tc.GetProperty(pd.Name);
            ConfirmAttribute cf = Attribute.GetCustomAttribute(pi, typeof(ConfirmAttribute)) as ConfirmAttribute;
            //if (cf != null)
            //{
            //    if (MessageBox.Show("Подтвердите изменения", "Внимание", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            //    {
            //        e.ChangedItem.PropertyDescriptor.SetValue(propertyGrid1.SelectedObject, e.OldValue);
            //        propertyGrid1.Refresh();
            //        return;
            //    }
            //}
            if (OnValueChanged != null)
                OnValueChanged(propertyGrid1.SelectedObject);
        }
        public void SetReadOnly()
        {
            richTextBox1.ReadOnly = User.current.Group != EGroup.Master;
        }
        public void RRefresh()
        {
            propertyGrid1.Refresh();
        }
    }
}
