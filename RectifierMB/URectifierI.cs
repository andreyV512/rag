using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RectifierNS
{
    public partial class URectifierI : UserControl
    {
        public URectifierI()
        {
            InitializeComponent();
        }
        public double DC
        {
            get
            {
                if (lDC.Text != null)
                    return (Convert.ToDouble(lDC.Text));
                return (0);
            }
            set
            {
                if (value <= 0)
                    lDC.Text = null;
                else
                    lDC.Text = value.ToString("0.##");
            }
        }
    }
}
