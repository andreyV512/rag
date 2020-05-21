using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defect.ACS
{
    public partial class UACS : UserControl
    {
        ACS acs=null;
        public UACS()
        {
            InitializeComponent();
            Clear();   
        }
        public void Init()
        {
            acs = new ACS(true);
        }
        public new void Dispose()
        {
            acs.Dispose();
            base.Dispose();
        }

        private void bTest_Click(object sender, EventArgs e)
        {
            lError.Text = acs.Test();
        }
        void Clear()
        {
            lIn.Text = null;
            lOut.Text = null;
            lError.Text = null;
        }

        private void bIn_Click(object sender, EventArgs e)
        {
            int itube;
            lError.Text=acs.TubeNumIn(out itube);
            lIn.Text = (lError.Text == "Ok") ? itube.ToString() : null;
        }

        private void bOut_Click(object sender, EventArgs e)
        {
            int itube;
            lError.Text = acs.TubeNumOut(out itube);
            lOut.Text = (lError.Text == "Ok") ? itube.ToString() : null;
        }

        private void bResult_Click(object sender, EventArgs e)
        {
            lError.Text=acs.SendResult(null);
        }
    }
}
