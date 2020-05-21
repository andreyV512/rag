using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Protocol
{
    internal partial class FProtocol : Form
    {
        List<string> L=new List<string>();
        object Sync = new object();
        public FProtocol(int _Period, bool _isFile,bool _isSave) 
        {
            InitializeComponent();
            isFile = _isFile;
            isSave = _isSave;
            timer1.Interval = _Period > 0 ? _Period : 200;
            timer1.Enabled = true;
        }
        bool isFile;
        bool isSave;
        private void FProtocol_Load(object sender, EventArgs e)
        {
            ucProtocol1.IsFile = isFile;
            ucProtocol1.IsSave = isSave;
        }

        public void pr(string _msg)
        {
            lock (Sync)
            {
                L.Add(_msg);
            }
        }

        public bool IsFile { get { return (ucProtocol1.IsFile); } set { ucProtocol1.IsFile = value; } }
        public bool IsSave { get { return (ucProtocol1.IsSave); } set { ucProtocol1.IsSave = value; } }

        private void FProtocol_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (Sync)
            {
                foreach(string s in L)
                    ucProtocol1.AddList(s);
                L.Clear();
            }
        }
    }
}
