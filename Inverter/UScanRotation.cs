using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Protocol;
using UPAR_common;

namespace InverterNS
{
    public partial class UScanRotation: UserControl
    {
        public UScanRotation()
        {
            InitializeComponent();
            label3.Text = null;
        }
        ComPortPars ComPortPars=null;
        public void Init(ComPortPars _ComPortPars)
        {
            ComPortPars = _ComPortPars;
        }
        JScanRotationTh scanRotation = null;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Text = checkBox1.Checked ? "Стоп" : "Старт";
            if (checkBox1.Checked)
            {
                scanRotation = new JScanRotationTh(ComPortPars);
                scanRotation.Start(0);
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                checkBox1.Text = "Выключение...";
                scanRotation.Dispose();
                checkBox1.Text = "Старт";
            }
        }
        public new void Dispose()
        {
            if(scanRotation!=null)
                scanRotation.Dispose();
            base.Dispose();
        }
        void pr(string _msg)
        {
            ProtocolST.pr("UScanRotation: " + _msg);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double? fr = scanRotation.Speed;
            if (fr == null)
                label3.Text = "Стоит";
            else
                label3.Text = fr.Value.ToString();
        }
    }
}
