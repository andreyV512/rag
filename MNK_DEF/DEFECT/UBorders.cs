using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defect
{
    public partial class UBorders : UserControl
    {
        public UBorders()
        {
            InitializeComponent();
        }
        public double Brak
        {
            get
            {
                if (lBrak.Text != null)
                    return (Convert.ToDouble(lBrak.Text));
                return (0);
            }
            set
            {
                if (value <= 0)
                    lBrak.Text = null;
                else
                    lBrak.Text = value.ToString("0.#");
            }
        }
        public double Class2
        {
            get
            {
                if (lClass2.Text != null)
                    return (Convert.ToDouble(lClass2.Text));
                return (0);
            }
            set
            {
                if (value <= 0)
                    lClass2.Text = null;
                else
                    lClass2.Text = value.ToString("0.#");
            }
        }
        public double[] Borders
        {
            get { return (new double[2] { Brak, Class2 }); }
            set
            {
                Brak = value[0];
                Class2 = value[1];
            }
        }
        public void Clear()
        {
            Brak = 0;
            Class2 = 0;
        }
    }
}
