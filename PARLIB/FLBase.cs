using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using System.Drawing.Design;
using Protocol;

namespace PARLIB
{
    public partial class FLBase : Form
    {
        static int pr_nn = 0;
        void pr(string _msg)
        {
            ProtocolST.pr("FLBase[" + pr_nn++.ToString() + "]: " + _msg);
        }
        public class Editor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return (UITypeEditorEditStyle.Modal); }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (FLBase f = new FLBase(value)) { f.ShowDialog(); }
                return (value);
            }
        }

        IParentList L;
        ParMainLite parMainLite;
        LBLine lbLine = null;
        public FLBase(object _L)
        {
            InitializeComponent();
            L = _L as IParentList;
            parMainLite = (L as IParentBase).parMainLite;
        }
        private void FLBaseT_Load(object sender, EventArgs e)
        {
            PropertyInfo pii = (L as IParentBase).Parent.GetType().GetProperty((L as IParentBase).PropertyName);
            DisplayNameAttribute dn = Attribute.GetCustomAttribute(pii, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            if (dn != null)
                Text = dn.DisplayName;
            else
            {
                DisplayNameAttribute dn1 = Attribute.GetCustomAttribute(L.GetType(), typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                if (dn1 != null)
                    Text = dn1.DisplayName;
            }
            LB.AllowDrop = Attribute.GetCustomAttribute(L.GetType(), typeof(SortableAttribute)) as SortableAttribute != null;
            lbLine = new LBLine(LB);
            int splitterDistance = splitContainer1.SplitterDistance;
            int splitter1 = pdView1.Splitter1;
            int splitter2 = pdView1.Splitter2;
            parMainLite.Wins.LoadFormRect(this, ref splitter1, ref splitter2, ref splitterDistance);
            FLBaseT_Resize(null, null);
            splitContainer1.SplitterDistance = splitterDistance;
            pdView1.Splitter1 = splitter1;
            pdView1.Splitter2 = splitter2;
            //if (!MetaDesc.ST.GetAccess(MetaPar.ExecPath(L) + ".FAccess").CheckUser(User.current))
            //{
            //    BAdd.Visible = false;
            //    BDelete.Visible = false;
            //    BCopy.Visible = false;
            //}

            foreach (object p in L)
            {
                AdjustName(p);
                LB.Items.Add(p);
            }
            Type tp = L.GetType();
            PropertyInfo pi = tp.GetProperty("Current");
            if (pi != null)
                LB.SelectedItem = pi.GetValue(L, null);
            else
            {
                IEnumerator ee = L.GetEnumerator();
                ee.Reset();
                if (ee.MoveNext())
                    LB.SelectedItem = ee.Current;
            }
            pdView1.OnValueChanged = OnValueChanged;
            pdView1.OnRefresh = OnRefresh;
        }

        private void AdjustName(object p)
        {
            //Type tp = p.GetType();
            //FieldInfo fi = p.GetType().GetField("Name",System.Reflection.BindingFlags.Instance);
            //if (fi != null)
            //{
            //    string name = fi.GetValue(p) as string;
            //}
        }
        private void FLBaseT_FormClosed(object sender, FormClosedEventArgs e)
        {
            parMainLite.Wins.SaveFormRect(this, pdView1.Splitter1, pdView1.Splitter2, splitContainer1.SplitterDistance);
            pdView1.Save();
            Type tp = L.GetType();
            PropertyInfo pi = tp.GetProperty("Current");
            if (pi != null)
            {
                if (LB.SelectedItem != null)
                    pi.SetValue(L, LB.SelectedItem, null);
                else
                    pi.SetValue(L, null, null);
            }
        }

        private void BAdd_Click(object sender, EventArgs e)
        {
            object o = L.AddNew();
            parMainLite.MP.CreateEmpty(o);
            int index = LB.Items.Add(o);
            LB.SelectedIndex = index;
            //            listBox1.SelectedIndex = listBox1.Items.Add(o);
        }
        private void BCopy_Click(object sender, EventArgs e)
        {
            if (LB.SelectedItem == null)
                return;
            object o = L.AddNew();
            parMainLite.MP.Copy(LB.SelectedItem, o);
            LB.SelectedIndex = LB.Items.Add(o);
        }

        private void BDelete_Click(object sender, EventArgs e)
        {
            if (LB.SelectedItem == null)
                return;
            int index = LB.SelectedIndex;
            L.RemoveOld(LB.SelectedItem);
            LB.Items.Clear();
            foreach (object p in L)
                LB.Items.Add(p);
            if (index > LB.Items.Count - 1)
                index = LB.Items.Count - 1;
            if (index >= 0)
                LB.SelectedIndex = index;
        }

        private void FLBaseT_Resize(object sender, EventArgs e)
        {
            int space = 4;
            BAdd.Top = ClientSize.Height - BAdd.Height - space;
            BDelete.Top = BAdd.Top;
            BCopy.Top = BAdd.Top;
            splitContainer1.Top = space;
            splitContainer1.Left = space;
            splitContainer1.Width = ClientSize.Width - space * 2;
            splitContainer1.Height = BAdd.Top - space * 2;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pdView1.SelectedObject = LB.SelectedItem;
        }
        private void OnValueChanged(object _v)
        {
            LB.Items[LB.SelectedIndex] = _v;
        }
        void OnRefresh()
        {
            for (int i = 0; i < LB.Items.Count; i++)
                LB.Items[i] = LB.Items[i];
        }

        Rectangle dragBoxFromMouseDown = Rectangle.Empty;
        bool DragOn = false;
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender as ListBox).AllowDrop)
                return;
            if ((e.Button & MouseButtons.Right) != MouseButtons.Right)
                return;
            Size dragSize = SystemInformation.DragSize;
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            if (lbLine.IsSelected(e.X, e.Y))
            {
                DragOn = true;
                pr("DragOn = true;");
            }
        }
        private void LB_MouseUp(object sender, MouseEventArgs e)
        {
            DragOn = false;
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragOn)
            {
                if (dragBoxFromMouseDown.Contains(e.X, e.Y))
                    return;
                if (LB.SelectedItem == null)
                    return;
                LB.DoDragDrop(LB.SelectedItem, DragDropEffects.Move);
                DragOn = false;
                lbLine.ClearLine();
            }
        }
        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = lbLine.Draw(e.X, e.Y) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void LB_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (!lbLine.IsIndex(Control.MousePosition.X, Control.MousePosition.Y))
                e.Action = DragAction.Cancel;
        }
        private void LB_DragDrop(object sender, DragEventArgs e)
        {
            pr("LB_DragDrop " + L.GetItem(LB.SelectedIndex).ToString() + " " + lbLine.IP.ToString() + " " + L.GetItem(lbLine.CurrentIndex).ToString());
            int src = LB.SelectedIndex;
            pr("scr=" + src);
            int trg = lbLine.CurrentIndex;
            if (lbLine.IP == LBLine.InsertPosition.After)
                trg++;
            if (trg > src)
                trg--;
            pr(string.Format("{0}: {1} -> {2}", L.ListCount().ToString(), src.ToString(), trg.ToString()));
            if (L.Move(src, trg))
            {
                LB.Items.Clear();
                foreach (object p in L)
                    LB.Items.Add(p);
                LB.SelectedIndex = trg;
            }
        }

        private void FLBase_Activated(object sender, EventArgs e)
        {
            pdView1.SetReadOnly();
        }
    }
}
