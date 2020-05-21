using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InverterNS
{
    public partial class URotate : UserControl
    {
        public URotate()
        {
            InitializeComponent();
            State = null;
        }
        double? state = null;
        public double? State
        {
            get
            {
                return (state);
            }
            set
            {
                state=value;
                if (state == null)
                {
                    label1.Text=null;
                    BackColor = SystemColors.Control;
                    toolTip1.SetToolTip(this, "Неопределенное состояние вращения");
                }
                else if (value.Value != 0)
                {
                    label1.Text = state.Value.ToString("F2")+" Об/c";
                    BackColor = Color.Green;
                    toolTip1.SetToolTip(this, "Вращается");
                }
                else
                {
                    label1.Text = "0 Об/c";
                    BackColor = Color.Gray;
                    toolTip1.SetToolTip(this, "Стоит");
                }
            }
        }
    }
}
