using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UPAR_common;

namespace InverterNS
{
    public partial class UInverter : UserControl
    {
        public UInverter()
        {
            InitializeComponent();
        }

        JInverterTh inverter = null;
        public void Init(ComPortPars _ComPort, ConverterPars _pars, int _frequency)
        {
            label2.Text = "Абонент " + _pars.Abonent.ToString();
            textBox1.Text = _frequency.ToString();
            label3.Text = null;
            inverter = new JInverterTh(_ComPort, _pars, _frequency);
            timer1.Interval = inverter.RTimeout;
        }
        public new void Dispose()
        {
            inverter.Dispose();
            base.Dispose();
        }
        bool block_check = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (block_check)
                return;
            if (checkBox1.Checked)
            {
                inverter.Start(0);
                prs("Ok");
                checkBox1.Text = "Стоп";
                foreach (Button b in Controls.OfType<Button>())
                    b.Enabled = false;
                timer1.Enabled = true;
            }
            else
            {
                inverter.Finish();
                //              foreach (Button b in Controls.OfType<Button>())
                //                    b.Enabled = true;
                checkBox1.Text = "Выкл...";
            }
        }
        public string Title { get { return (label1.Text); } set { label1.Text = value; } }

        private void UInverter_Resize(object sender, EventArgs e)
        {
            int space = 4;
            richTextBox1.Left = space;
            richTextBox1.Width = ClientSize.Width - space * 2;
            richTextBox1.Height = ClientSize.Height - richTextBox1.Top - space;
        }

        private void UInverter_Load(object sender, EventArgs e)
        {
            UInverter_Resize(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inverter.setParameterFrequency(Convert.ToInt32(textBox1.Text)))
                inverter.frequency = Convert.ToInt32(textBox1.Text);
            else
                prs(inverter.LastError);
        }
        void prs(string _msg)
        {
            richTextBox1.Text = _msg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ret = inverter.getParameterFrequency();
            if (ret >= 0)
            {
                label3.Text = ret.ToString() + " Гц";
                prs("Ok");
            }
            else
            {
                label3.Text = null;
                prs(inverter.LastError);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            JInverterTh.StateIn ret = inverter.StateRead();
            if (ret != null)
                prs(ret.ToString());
            else
                prs(inverter.LastError);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            prs(inverter.NETManage() ? "Ok" : inverter.LastError);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            prs(inverter.Reset() ? "Ok" : inverter.LastError);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            JInverterTh.StateIn ret = inverter.State;
            prs(ret != null ? ret.ToString() : null);
            if (inverter.IsComplete)
            {
                foreach (Button b in Controls.OfType<Button>())
                    b.Enabled = true;
                checkBox1.Text = "Пуск";
                checkBox1.Checked = false;
            }
            else if (inverter.IsError)
            {
                prs(inverter.LastError);
                inverter.Finish();
                foreach (Button b in Controls.OfType<Button>())
                    b.Enabled = true;
                checkBox1.Checked = false;
                checkBox1.Text = "Пуск";
            }
            else
                timer1.Enabled = true;
        }
    }
}
