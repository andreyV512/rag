using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PARLIB;

namespace Defect
{
    public partial class FPrevTube : Form
    {
        public FPrevTube()
        {
            InitializeComponent();  
        }

        private void FPrevTube_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            L_WindowLPars.CurrentWins.LoadFormRect(this);
        }

        private void FPrevTube_FormClosed(object sender, FormClosedEventArgs e)
        {
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }
        public void Clear() { uSumM1.Clear(); }
        public void Draw() { uSumM1.Draw(); }
    }
}
