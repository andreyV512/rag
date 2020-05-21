using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Signals
{
    public partial class UCSignalIn : UCSignal
    {
        public delegate void DOnClick(Signal _s);
        public UCSignalIn(Signal _s):base(_s)
        {
            InitializeComponent();
        }
        private void UCSignalIn_Load(object sender, EventArgs e)
        {
            label1.Text = string.Format("[{0}]{1}{2} {3}",
                signal.board.ToString(),
                signal.position.ToString(),
                signal.digital ? "Д" : "",
                signal.name);
            TT.SetToolTip(this, signal.hint);
            TT.SetToolTip(label1, signal.hint);
            label1.Top = (ClientSize.Height - label1.Height) / 2;
            if (label1.Top < 0)
                label1.Top = 0;
            label1.Left = 2;
            color_false = BackColor;
            TT.Active = false;
            TT.Active = true;
            Exec();
        }
        public void Exec()
        {
            if (signal.Val)
                BackColor = Color.Green;
            else
                BackColor = color_false;
        }
    }
}
