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
    public partial class URectifierVew : UserControl
    {
        public URectifierVew()
        {
            InitializeComponent();
            Draw();
        }
        UITH state = null;
        public UITH StateH
        {
            get { return (state); }
            set
            {
                if (state != value)
                {
                    state = value;
                    Draw();
                }
            }
        }
        //public UIT State
        //{
        //    get { return (state); }
        //    set
        //    {
        //        state = new UITH(value, 0);
        //        Draw();
        //    }
        //}
        void Draw()
        {
            if (state == null)
            {
                lI.Text = null;
                lU.Text = null;
                lR.Text = null;
            }
            else
            {
                lI.Text = state.I.ToString("F2");
                lU.Text = state.U.ToString("F2");
                lR.Text = state.R.ToString("F2");
            }
        }
    }
}
