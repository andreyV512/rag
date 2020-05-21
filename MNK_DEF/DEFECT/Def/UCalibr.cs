using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defect.Def
{
    public partial class UCalibr : UserControl
    {
        public delegate void DOnStep(double _step);
        public delegate void DOnGain();
        public delegate void DOnCalibrate();

        public event DOnStep OnStep;
        public event DOnGain OnGain;
        public event DOnCalibrate OnCalibrate;
        double gain;
        
        public UCalibr()
        {
            InitializeComponent();
            toolTip1.SetToolTip(checkBox1, "Участвует ли датчик в общей калибровке");
            toolTip1.SetToolTip(numericUpDown1, "Множитель датчика");
            toolTip1.SetToolTip(button1, "Провести калибровку\nПравая кнопка: Задать шаг калибровки");
            Step0 = 1;
            numericUpDown1.MouseWheel += new MouseEventHandler(UCalibr_MouseWheel);
        }

        void UCalibr_MouseWheel(object sender, MouseEventArgs e)
        {
            (e as HandledMouseEventArgs).Handled = true;
        }
        public double Gain
        {
            get { return (gain); }
            set
            {
                gain = value;
                block_gain = true;
                numericUpDown1.Value = Convert.ToDecimal(value);
                block_gain = false;
            }
        }
        public double Step
        {
            get { return (Convert.ToDouble(toolStripComboBox1.Text)); }
            set
            {
                foreach (string it in toolStripComboBox1.Items)
                {
                    if (value >= Convert.ToDouble(it))
                    {
                        toolStripComboBox1.Text = it;
                        return;
                    }
                }
                toolStripComboBox1.Text = "1";
            }
        }
        decimal Step0
        {
            get { return (numericUpDown1.Increment); }
            set { numericUpDown1.Increment = value; }
        }


        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (OnCalibrate != null)
                    OnCalibrate();
            }
            else
                contextMenuStrip1.Show(button1, e.X, e.Y);
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            Step0 = Convert.ToDecimal(toolStripComboBox1.Text);
            if (OnStep != null)
                OnStep(Step);
        }

        bool block_gain = false;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (block_gain)
                return;
            gain = Convert.ToDouble(numericUpDown1.Value);
            if (OnGain != null)
                OnGain();
        }
        public bool IsCalibr { get { return (checkBox1.Checked); } }
    }
}
