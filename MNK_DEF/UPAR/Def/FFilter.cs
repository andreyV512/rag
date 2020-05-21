using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using PARLIB;

namespace UPAR.Def
{
    public partial class FFilter : Form
    {
        FilterPars instance;
        FilterPars instanceBuf=null;
        public bool NeedRecalc = false;
        public delegate void DOnRecalc();
        public DOnRecalc OnRecalc = null;
        public FFilter(FilterPars _instance, bool _IsPanel)
        {
            InitializeComponent();
            if (_IsPanel)
            {
                Panel p = new Panel();
                p.Dock = DockStyle.Top;
                p.Height = 30;
                Button b = new Button();
                b.Top = 3;
                b.Left = 3;
                b.Text = "Сохранить";
                p.Controls.Add(b);
                Controls.Add(p);
                b.Click += new EventHandler(b_Click);
                instanceBuf = _instance;
                instance = _instance.Clone();
            }
            else
                instance = _instance;
        }

        void b_Click(object sender, EventArgs e)
        {
            instanceBuf.Copy(instance);
            NeedRecalc = true;
            if (OnRecalc != null)
                OnRecalc();
        }

        private void FFilter_Load(object sender, EventArgs e)
        {
            int splitterDistance = 0;
            int splitter1 = pdView1.Splitter1;
            int splitter2 = pdView1.Splitter2;
            ParAll.ST.Wins.LoadFormRect(this, ref splitter1, ref splitter2, ref splitterDistance);
            pdView1.Splitter1 = splitter1;
            pdView1.Splitter2 = splitter2;
            instance.SetView();
            pdView1.SelectedObject = instance;
            pdView1.OnValueChanged=OnValueChanged;
        }

        void OnValueChanged(object _v)
        {
            instance.SetView();
            pdView1.RRefresh();
        }

        private void FFilter_FormClosed(object sender, FormClosedEventArgs e)
        {
            ParAll.ST.Wins.SaveFormRect(this, pdView1.Splitter1, pdView1.Splitter2, 0);
            pdView1.Save();
        }

        private void FFilter_Activated(object sender, EventArgs e)
        {
            pdView1.SetReadOnly();
        }
    }
}
