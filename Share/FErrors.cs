using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PARLIB;

namespace Share
{
    public partial class FErrors : Form
    {
        public FErrors()
        {
            InitializeComponent();
        }
        private void FErrors_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                L_WindowLPars.CurrentWins.LoadFormRect(this);
            }

        }
        private void FErrors_FormClosed(object sender, FormClosedEventArgs e)
        {
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }
        public void Add(string _msg)
        {
            Protocol.Items.Add(_msg);
        }
        public void Clear()
        {
            Protocol.Items.Clear();
        }
        public void Add(List<string> _list)
        {
            foreach (string s in _list)
            {
                if (s != null)
                    Protocol.Items.Add(s);
            }
        }
    }
}
