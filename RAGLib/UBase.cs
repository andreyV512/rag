using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RAGLib
{
    public partial class UBase : UserControl
    {
        public UBase()
        {
            InitializeComponent();
            MinHeight = panel1.Height;
        }
        int space = 5;
        public int MinHeight { get; protected set; }
        public string Title
        {
            get { return (label1.Text); }
            set
            {
                label1.Text = value;
                CBIsWork.Left = label1.Left + label1.Width + space;
            }
        }
    }
}
