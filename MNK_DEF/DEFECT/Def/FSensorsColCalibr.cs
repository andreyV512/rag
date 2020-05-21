using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using Share;
using ResultLib;
//using ResultLib.Def;
using PARLIB;
using Share;
using UPAR.Def;

namespace Defect.Def
{
    public partial class FSensorsColCalibr : Form
    {
        public string SaveName = "FSensorsColCalibr";
        string title;
        public bool NeedRecalc { get; private set; }
        int space = 2;
        EUnit Tp;

        public FSensorsColCalibr(EUnit _Tp, string _title)
        {
            InitializeComponent();
            Tp = _Tp;
            title = _title;
        }
        private void FSensorsColCalibr_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                L_WindowLPars.CurrentWins.LoadFormRect(this);
                bConfirm.Visible = false;
                bConfirm.Enabled = false;
                bCancel.Visible = false;
                bCancel.Enabled = false;
                uSensorsColCalibr1.Init(Tp, SetState);
                NeedRecalc = false;
                RDraw();
                panel1_Resize(null, null);
            }
        }
        private void FSensors_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!DesignMode)
                L_WindowLPars.CurrentWins.SaveFormRect(this);
            if (uSensorsColCalibr1.NeedRecalc)
                uSensorsColCalibr1.RCancel();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!cbCalibr.Checked)
            {
                string sk = keyData.ToString();
                if (sk == "Up, Control")
                {
                    uSensorsColCalibr1.HH--;
                    return(true);
                }
                else if (sk == "Down, Control")
                {
                    uSensorsColCalibr1.HH++;
                    return (true);
                }
                switch (keyData)
                {
                    case Keys.Left:
                        RK.ST.cDef(Tp).Zone -= 1;
                        RDraw();
                        return (true);
                    case Keys.Right:
                        RK.ST.cDef(Tp).Zone += 1;
                        RDraw();
                        return (true);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void RDraw()
        {
            Text = string.Format("{0}: Зона: {1}", title, RK.ST.cDef(Tp).Zone.Value + 1);
            uSensorsColCalibr1.RDraw();
        }

        void SetState()
        {
            if (uSensorsColCalibr1.NeedRecalc)
            {
                bConfirm.Enabled = true;
                bCancel.Enabled = true;
                cbCalibr.Enabled = false;
            }
            else
            {
                bConfirm.Enabled = false;
                bCancel.Enabled = false;
                cbCalibr.Enabled = true;
            }
        }



        private void cbCalibr_CheckedChanged(object sender, EventArgs e)
        {
            uSensorsColCalibr1.CalibrVisible = cbCalibr.Checked;
            bConfirm.Visible = cbCalibr.Checked;
            bCancel.Visible = cbCalibr.Checked;
            SetState();
        }

        void bConfirm_Click(object sender, EventArgs e)
        {
            uSensorsColCalibr1.Confirm();
            NeedRecalc = true;
            SetState();
        }

        void bCancel_Click(object sender, EventArgs e)
        {
            uSensorsColCalibr1.RCancel();
            SetState();
        }

        private void chScroll_CheckedChanged(object sender, EventArgs e)
        {
            if (chScroll.Checked)
            {
                uSensorsColCalibr1.IsScroll = false;
                chScroll.Text = "Курсор";
            }
            else
            {
                uSensorsColCalibr1.IsScroll = true;
                chScroll.Text = "Скроллинг";
            }
        }
        
        private void panel1_Resize(object sender, EventArgs e)
        {
            chScroll.Left = panel1.Width - chScroll.Width - space;
        }
    }
}
