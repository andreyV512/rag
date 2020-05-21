using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;
using Defect.SG.ColorPicker;

using SQL;
using PARLIB;
using UPAR;
using UPAR.SG;

namespace Defect.SG
{
    /*
    public static string Title = "Пороги";
    public static void LoadKey(BaseDBKey _Key, BindingSource _bs){}
    public static bool InsertKey(BaseDBKey _Key, BindingSource _bs){return(false);}
    */
    public partial class DGV : UserControl
    {
        public DGV()
        {
            InitializeComponent();
        }
        string typeName;
        public string SaveName;
        public string TypeName
        {
            get { return (typeName); }
            set
            {
                typeName = value;
                tp = Type.GetType(TypeName);
                FieldInfo fi = tp.GetField("Title");
                if (fi != null)
                    label1.Text = fi.GetValue(null) as string;
                dg.Columns.Clear();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    BrowsableAttribute br = Attribute.GetCustomAttribute(pi, typeof(BrowsableAttribute)) as BrowsableAttribute;
                    if (br != null)
                    {
                        if (!br.Browsable)
                            continue;
                    }
                    DataGridViewColumn col;
                    if (pi.PropertyType == typeof(Color))
                        col = new ColorColumn();
                    else if (pi.PropertyType == typeof(bool))
                        col = new DataGridViewCheckBoxColumn();
                    else
                        col = new DataGridViewTextBoxColumn();
                    col.DataPropertyName = pi.Name;
                    col.HeaderText = pi.Name;
                    DisplayNameAttribute dn = Attribute.GetCustomAttribute(pi, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                    if (dn != null)
                        col.HeaderText = dn.DisplayName;
                    col.Name = "col_" + pi.Name;
                    dg.Columns.Add(col);
                }
            }
        }

        bool block_save = false;
        Type tp = null;
        BaseDBKey parentKey;
        DOnPrs onPrs = null; public DOnPrs OnPrs { set { onPrs = value; } } void prs(string _msg) { if (onPrs != null) onPrs(_msg); }
        Rectangle rect_default;
        MSPanel[] MP = null;
        public ControlCollection CC { get; set; }
        bool edit = false;
        public void RLoad(BaseDBKey _parentKey)
        {
            parentKey = _parentKey;
            RLoad();
        }
        void RLoad()
        {
            MethodInfo mi = tp.GetMethod("LoadKey");
            block_save = true;
            mi.Invoke(null, new object[] { parentKey, bs });
            block_save = false;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (bs.Current == null)
                return;
            if ((bs.Current as BaseItem).Delete())
                RLoad();
            else
                prs("Не могу удалить данные");
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            MethodInfo mi = tp.GetMethod("InsertKey");
            if (!(bool)mi.Invoke(null, new object[] { parentKey, bs }))
                prs("Не могу добавить данные");
        }

        private void bs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (block_save)
                return;
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                if (!(bs.Current as BaseItem).Update())
                    prs("Не смогли записать данные");
            }
        }

        private void DGV_Resize(object sender, EventArgs e)
        {
            bn.Top = ClientSize.Height - bn.Height - MSPanel.size;
            dg.Width = ClientSize.Width - MSPanel.size * 2;
            dg.Height = bn.Top - dg.Top;
        }

        private void DGV_Load(object sender, EventArgs e)
        {
            SaveName = Name;
            rect_default = new Rectangle(Location, Size);
            MP = new MSPanel[4];
            for (int i = 0; i < MP.Length; i++)
            {
                MP[i] = new MSPanel(i, ClientSize, onResize);
                Controls.Add(MP[i]);
            }
            label1.Left = MSPanel.size;
            label1.Top = MSPanel.size;

            bn.Left = MSPanel.size;
            bn.Top = ClientSize.Height - bn.Height - MSPanel.size;

            dg.Left = MSPanel.size;
            dg.Width = ClientSize.Width - MSPanel.size * 2;
            dg.Top = label1.Top + label1.Height + 3;
            dg.Height = bn.Top - dg.Top;

            SetPanel();
        }
        void onResize(bool _move, Point _deltap)
        {
            Rectangle r = new Rectangle();
            if (_move)
            {
                r.X = Left + _deltap.X;
                r.Y = Top + _deltap.Y;
                r.Width = Width;
                r.Height = Height;
            }
            else
            {
                r.X = Left;
                r.Y = Top;
                r.Width = Width + _deltap.X;
                r.Height = Height + _deltap.Y;
            }
            if (!CheckCC(r))
                return;
            Left = r.Left;
            Top = r.Top;
            Width = r.Width;
            Height = r.Height;
            SetPanel();
        }
        bool CheckCC(Rectangle _r)
        {
            if (CC == null)
                return (true);
            foreach (Control c in CC)
            {
                if (c == this)
                    continue;
                if (!c.Visible)
                    continue;
                Rectangle rc = new Rectangle(c.Location, c.Size);
                if (_r.IntersectsWith(rc))
                    return (false);
            }
            return (true);
        }
        void SetPanel()
        {
            Refresh();
            if (edit)
            {
                Graphics g = CreateGraphics();
                Rectangle r = new Rectangle(4, 4, ClientSize.Width - 8, ClientSize.Height - 8);
                Pen pen = new Pen(Color.Black, 1);
                pen.DashStyle = DashStyle.Dot;
                g.DrawRectangle(pen, r);
            }
            if (MP != null)
            {
                foreach (MSPanel p in MP)
                {
                    p.ParentClientSize = ClientSize;
                    p.Visible = edit;
                }
            }
        }
        public bool Edit
        {
            set
            {
                edit = value;
                dg.Enabled = !value;
                bn.Enabled = !value;
                SetPanel();
            }
        }
        public void ResizeDefault()
        {
            Left = rect_default.Left;
            Width = rect_default.Width;
            Top = rect_default.Top;
            Height = rect_default.Height;
        }
        public void LoadRectangle()
        {
            L_WindowLPars.CurrentWins.LoadFormRect(this);
            L_GridPars lGridPars = ParAll.SG.Some.Grids;
            GridPars gridPars = lGridPars[Name];
            if (gridPars == null)
                return;
            L_ColumnPars lColumnPars = gridPars.Columns;
            foreach (DataGridViewColumn col in dg.Columns)
            {
                ColumnPars columnPars = lColumnPars[col.Name];
                if (columnPars == null)
                    continue;
                col.Width = columnPars.Width;
            }
        }
        public void SaveRectangle()
        {
            L_GridPars lGridPars = ParAll.SG.Some.Grids;
            GridPars gridPars = lGridPars[Name];
            if (gridPars == null)
            {
                gridPars = lGridPars.AddNewTree(ParAll.ST.MP) as GridPars;
                gridPars.Name = Name;
            }
            L_ColumnPars lColumnPars = gridPars.Columns;
            foreach (DataGridViewColumn col in dg.Columns)
            {
                ColumnPars columnPars = lColumnPars[col.Name];
                if (columnPars == null)
                {
                    columnPars = lColumnPars.AddNewTree(ParAll.ST.MP) as ColumnPars;
                    columnPars.Name = col.Name;
                }
                columnPars.Width = col.Width;
            }
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }
        public delegate void DOnButton(object _current, Point _point);
        public void AddButton(string _name, DOnButton _OnButton)
        {
            ToolStripButton bt = new ToolStripButton(_name);
            bt.BackColor = SystemColors.ButtonShadow;
            bt.Tag = _OnButton;
            bt.Click += new EventHandler(bt_Click);
            bn.Items.Insert(0, bt);

        }
        void bt_Click(object sender, EventArgs e)
        {
            ToolStripButton bt = sender as ToolStripButton;
            if (bt.Tag == null)
                return;
            DOnButton p = bt.Tag as DOnButton;
            Rectangle r = bt.Bounds;
            p(bs.Current, bn.PointToScreen(new Point(r.X, r.Y + r.Height)));
        }
        event DOnCurrent onCurrent; public event DOnCurrent OnCurrent { add { onCurrent += value; } remove { onCurrent -= value; } }
        private void bs_CurrentChanged(object sender, EventArgs e)
        {
            if (bs.Current != null)
            {
                if (onCurrent != null)
                    onCurrent(Key);
            }
        }
        public BaseDBKey Key
        {
            get
            {
                if (bs.Current == null)
                    return (null);
                BaseItem bi = bs.Current as BaseItem;
                if (bi == null)
                    return (null);
                return (bi.Key);
            }
        }
        public BaseItem Current { get { return (bs.Current as BaseItem); } }
        public bool AllowInsert { get { return (bindingNavigatorAddNewItem.Visible); } set { bindingNavigatorAddNewItem.Visible = value; } }
        public bool AllowDelete { get { return (bindingNavigatorDeleteItem.Visible); } set { bindingNavigatorDeleteItem.Visible = value; } }
        public bool DragFrom { get; set; }
        public bool DragTo { get { return (dg.AllowDrop); } set { dg.AllowDrop = value; } }
        bool dd = false;
        bool ddd = false;
        Rectangle dragBoxFromMouseDown = Rectangle.Empty;
        private void dg_MouseDown(object sender, MouseEventArgs e)
        {
            if (DragFrom)
            {
                if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                    return;
                if (bs.Current == null)
                    return;
                //            pr("dg_MouseDown " + (bs.Current as Tube).ToString());
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                dd = true;
            }
        }

        private void dg_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragFrom)
            {
                if (!dd)
                    return;
                if (ddd)
                    return;
                if (dragBoxFromMouseDown.Contains(e.X, e.Y))
                    return;
                //pr("DoDragDrop");
                dg.DoDragDrop(bs.Current, DragDropEffects.All);
                ddd = true;
            }
        }

        private void dg_MouseUp(object sender, MouseEventArgs e)
        {
            if (DragFrom)
            {
                if (!dd)
                    return;
                ddd = false;
                dd = false;
                //            pr("dg_MouseUp");
            }
        }

        private void dg_DragDrop(object sender, DragEventArgs e)
        {
            if (DragTo)
            {
                Tube tb = e.Data.GetData(typeof(Tube)) as Tube;
                Group.DBKey gKey = parentKey as Group.DBKey;
                int new_id = Etalon.TubeToEtalon(tb, gKey);
                if (new_id < 0)
                {
                    prs("Не удалось записать трубу в эталон");
                    return;
                }
                RLoad();
                foreach (Etalon o in bs)
                {
                    if (o.Id == new_id)
                    {
                        bs.Position = bs.IndexOf(o);
                        break;
                    }
                }
            }
        }

        private void dg_DragOver(object sender, DragEventArgs e)
        {
            if (DragTo)
            {
                if (e.Data.GetDataPresent(typeof(Tube)))
                    e.Effect = DragDropEffects.Copy;
            }
        }
    }
}
