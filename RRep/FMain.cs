using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SQL;

namespace RRep
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
        }
        RRepPars.ParSet PS;
        SqlConnection sqlConnection=null;
        private void FMain_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ParSet == null)
                Properties.Settings.Default.ParSet = new RRepPars.ParSet();
            PS = Properties.Settings.Default.ParSet;
            PS.LWinPars.LoadRect(this);
            if (Left < -10 || Top < -10 || Width < 100 || Height < 100)
            {
                Left=0;
                Top=0;
                Width=100;
                Height=100;
            }
            this.reportViewer1.RefreshReport();
            uSelection1.OnExec = OnExec;
            uSelection1.Selection = PS.Selection;
            if(!DesignMode)
                sqlConnection=CDBS.Connection;
            reportViewer1.Messages = new RMessages();
        }

        private void FMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            PS.LWinPars.SaveRect(this);
            Properties.Settings.Default.Save();
        }

        private void FMain_Resize(object sender, EventArgs e)
        {
            reportViewer1.Width = ClientSize.Width - reportViewer1.Left * 2;
            reportViewer1.Height = statusStrip1.Top - reportViewer1.Top;
        }
        void OnExec()
        {
            Report.Exec(PS.Selection,reportViewer1);                                    
        }
    }
}
