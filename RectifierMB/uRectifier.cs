using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using UPAR_common;

namespace RectifierNS
{
    public partial class uRectifier : UserControl
    {
        JRectifierTh jrectifierTh;
        public uRectifier()
        {
            InitializeComponent();
        }
        public void Init(RectifiersPars _rectifiersPars, RectifierPars _rectifierPars)
        {
            jrectifierTh = new JRectifierTh(_rectifiersPars, _rectifierPars, true, true);
            timer1.Interval = _rectifiersPars.ComPort.Timeout;
            if (_rectifierPars.TpIU == EIU.ByI)
            {
                label3.Font = new System.Drawing.Font(label3.Font, FontStyle.Regular);
                label3.BorderStyle = BorderStyle.None;
            }
            else
            {
                label4.Font = new System.Drawing.Font(label4.Font, FontStyle.Regular);
                label4.BorderStyle = BorderStyle.None;
            }
            Draw(null);
        }
        public new void Dispose()
        {
            timer1.Enabled = false;
            if (!DesignMode)
                jrectifierTh.Dispose();
            base.Dispose();
        }
        public string Title { get { return (label1.Text); } set { label1.Text = value; } }
        void prstat(string _msg)
        {
            toolStripStatusLabel1.Text = _msg;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Text = checkBox1.Checked ? "Выключить" : "Включить";
            if (checkBox1.Checked)
            {
                checkBox1.Enabled = false;
                checkBox1.Text = "Включение...";
                jrectifierTh.Start(0);
                timer1.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;
                timer1.Enabled = true;
                checkBox1.Text = "Выключение...";
                jrectifierTh.Finish();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draw(jrectifierTh.GetUITH());
            if (!checkBox1.Enabled)
            {
                if (checkBox1.Text == "Включение...")
                {
                    if (jrectifierTh.IsStarted)
                    {
                        checkBox1.Text = "Выключить";
                        checkBox1.Enabled = true;
                    }
                }
                if (checkBox1.Text == "Выключение...")
                {
                    if (!jrectifierTh.IsStarted)
                    {
                        checkBox1.Text = "Включить";
                        timer1.Enabled = false;
                        checkBox1.Enabled = true;
                    }
                }
            }
            if (jrectifierTh.IsError)
            {
                timer1.Enabled = false;
                checkBox1.Text = "Включить";
                checkBox1.Enabled = true;
                prstat(jrectifierTh.LastError);
            }
        }
        void Draw(UITH _v)
        {
            if (_v == null)
            {
                label3.Text = null;
                label4.Text = null;
                label6.Text = null;
                label8.Text = null;
                prstat(null);
            }
            else
            {
                label3.Text = _v.I.ToString("F2");
                label4.Text = _v.U.ToString("F2");
                label6.Text = _v.R.ToString("F2");
                label8.Text = _v.Ts.ToString(@"hh\:mm\:ss");
                prstat(_v.error);
            }
        }
    }
}
