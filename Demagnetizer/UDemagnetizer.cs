using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UPAR_common;

namespace Demagnetizer
{
    public partial class UDemagnetizer : UserControl
    {
        public UDemagnetizer()
        {
            InitializeComponent();
        }

        public JDemagnetizer jDemagnetizer = null;
        public void Init(DemagnetizerPars _Demagnetizer, DemagnetizerTSPars _DemagnetizerTSPars)
        {
            jDemagnetizer = new JDemagnetizer(_Demagnetizer, _DemagnetizerTSPars, true);
            Clear();
        }
        public new void Dispose()
        {
            jDemagnetizer.Dispose();
            base.Dispose();
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

        void prs(string _msg)
        {
            richTextBox1.Text = _msg;
        }

        private void bRead_Click(object sender, EventArgs e)
        {
            Draw(jDemagnetizer.GetState());
        }
        void Draw(JDemagnetizer.State _state)
        {
            if (_state.Error != null)
            {
                Clear();
                prs(_state.Error);
                return;
            }
            tbPositive.Text = _state.OffsetPositive.ToString();
            tbNegative.Text = _state.OffsetNegative.ToString();
            tbFrequency.Text = _state.Frequency.ToString();
            prs("Ok");
        }
        void Clear()
        {
            tbPositive.Text = null;
            tbNegative.Text = null;
            tbFrequency.Text = null;
            prs(null);
        }

        private void bPositive_Click(object sender, EventArgs e)
        {
            int OffsetPositive;
            if (!Int32.TryParse(tbPositive.Text, out OffsetPositive))
            {
                prs("Не корректное значение!");
                return;
            }
            jDemagnetizer.ResetError();
            jDemagnetizer.SetOffset(OffsetPositive);
            prs(jDemagnetizer.IsError ? jDemagnetizer.LastError : "Ok");
        }

        private void bNegative_Click(object sender, EventArgs e)
        {
            int OffsetNegative;
            if (!Int32.TryParse(tbNegative.Text, out OffsetNegative))
            {
                prs("Не корректное значение!");
                return;
            }
            jDemagnetizer.ResetError();
            jDemagnetizer.SetOffset(-OffsetNegative);
            prs(jDemagnetizer.IsError ? jDemagnetizer.LastError : "Ok");
        }

        private void bFrequency_Click(object sender, EventArgs e)
        {
            int Frequency;
            if (!Int32.TryParse(tbFrequency.Text, out Frequency))
            {
                prs("Не корректное значение!");
                return;
            }
            jDemagnetizer.ResetError();
            jDemagnetizer.SetFrequency(Frequency);
            prs(jDemagnetizer.IsError ? jDemagnetizer.LastError : "Ok");
        }

        private void bOn_Click(object sender, EventArgs e)
        {
            jDemagnetizer.ResetError();
            jDemagnetizer.SetOnOff(true);
            prs(jDemagnetizer.IsError ? jDemagnetizer.LastError : "Ok");
        }

        private void bOff_Click(object sender, EventArgs e)
        {
            jDemagnetizer.ResetError();
            jDemagnetizer.SetOnOff(false);
            prs(jDemagnetizer.IsError ? jDemagnetizer.LastError : "Ok");
        }
    }
}
