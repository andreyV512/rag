using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defect.SG
{
    public partial class FMessage : Form
    {
        public bool Yes = false;
        public FMessage(string _text)
        {
            InitializeComponent();
            label1.Text = _text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Yes = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Yes = false;
            Close();
        }

        private void FMessage_Load(object sender, EventArgs e)
        {
            int space = 10;
            int szl = label1.Width;
            int szb = button1.Width + space + button1.Width;
            int szmax = szl < szb ? szb : szl;
            int sz = Width - ClientSize.Width + szmax + space * 2;
            if (Width < sz)
                Width = sz;
            label1.Left = (ClientSize.Width - szl) / 2;
            button1.Left = (ClientSize.Width - szb) / 2;
            button2.Left = button1.Left + button1.Width + space;
        }
    }
}
