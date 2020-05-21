using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Share;
using RAGLib;
using UPAR;
using ResultLib.SG;

namespace Defect.SG
{
    public partial class USG : UserControl
    {
        SGState state = null;
        public USG()
        {
            InitializeComponent();
        }
        public SGState State { get { return (state); } set { state = value; RDraw(); } }
        void RDraw()
        {
            if (state == null)
            {
                label2.Text = null;
                label2.BackColor = SystemColors.ButtonFace;
            }
            else
            {
                label2.Text = state.Group;
                label2.BackColor = state.RColor;
            }
        }
        public void Clear() { State = null; }
        public void Init()
        {
            LoadSettings();
        }
        public void LoadSettings()
        {
            CBIsWork.Checked=ParAll.ST.Defect.Cross.SolidGroup.IsWork;
            State = null;
        }

        private void CBIsWork_CheckedChanged(object sender, EventArgs e)
        {
            ParAll.ST.Defect.Cross.SolidGroup.IsWork = CBIsWork.Checked;
        }
    }
}
