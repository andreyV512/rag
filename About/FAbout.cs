using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PARLIB;

namespace About
{
    public partial class FAbout : Form
    {
        string head = "Версия: 1866.1.1.";
        string head1 = "A";
        public FAbout()
        {
            InitializeComponent();
        }
        public DateTime Version
        {
            set
            {
                string dts = value.Year.ToString("D4")+value.Month.ToString("D2")+value.Day.ToString("D2");
                label1.Text = head + dts + head1;
            }
        }

        private void FAbout_Load(object sender, EventArgs e)
        {
            L_WindowLPars.CurrentWins.LoadFormRect(this);
        }

        private void FAbout_FormClosed(object sender, FormClosedEventArgs e)
        {
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }

    }
}
