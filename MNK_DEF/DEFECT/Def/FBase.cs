using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PARLIB;
using UPAR;

namespace Defect.Def
{
    public partial class FBase : Form
    {
        public FBase()
        {
            InitializeComponent();
            OnHide = null;
        }
        public delegate void DOnHide();
        public DOnHide OnHide { get; set; }
        bool need_save = false;
        private void FBase_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                L_WindowLPars.CurrentWins.LoadFormRect(this);
                need_save = true;
            }
        }
        virtual public void Save()
        {
            if(need_save)
                L_WindowLPars.CurrentWins.SaveFormRect(this);
        }
        private void FBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
            if (OnHide != null)
                OnHide();
        }
        protected void Init()
        {
            Show();
            BringToFront();
        }
    }
}
