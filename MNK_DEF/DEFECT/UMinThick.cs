using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defect
{
    public partial class UMinThick : UserControl
    {
        public UMinThick()
        {
            InitializeComponent();
            Clear();
        }
        double? minThick = null;
        public double? MinThick
        {
            get { return (minThick); }
            set
            {
                minThick = value;
                lMinThick.Text = minThick == null ? null : minThick.Value.ToString("F1");
            }
        }
        public void Clear()
        {
            minThick=null;
        }
    }
}

