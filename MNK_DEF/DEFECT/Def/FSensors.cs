using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Share;
using ResultLib;
using ResultLib.Def;
using PARLIB;
using UPAR;
using UPAR.Def;

namespace Defect.Def
{
    public partial class FSensors : Form
    {
        public string SaveName = "FSensors";
        string title;
        List<USensor> L = new List<USensor>();
        bool needRecalc;
        public bool NeedRecalc { get; private set; }
        EUnit Tp;
        public FSensors(string _title, EUnit _Tp)
        {
            InitializeComponent();
            needRecalc = false;
            title = _title;
            Tp = _Tp;
        }
        private void FSensors_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                L_WindowLPars.CurrentWins.LoadFormRect(this);
            bConfirm.Visible = false;
            bConfirm.Enabled = false;
            bCancel.Visible = false;
            bCancel.Enabled = false;
            needRecalc = false;
            if (RK.ST.cDef(Tp).Zone == null)
                return;
            Zone z = RK.ST.cDef(Tp).result.MZone[0];
            for (int i = 0; i < z.MSensor.Length; i++)
            {
                USensor p = new USensor();
                p.OnStep += new UCalibr.DOnStep(p_OnStep);
                p.OnCalibrate += new USensor.DOnCalibrate(p_OnCalibrate);
                p.OnGain += new USensor.DOnGain(p_OnGain);
                p.CalibrVisible = false;
                L.Add(p);
                Controls.Add(p);
            }
            RResize();
            SetGainsFromPars();
            Draw();
            NeedRecalc = false;
        }
        private void FSensors_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!DesignMode)
                L_WindowLPars.CurrentWins.SaveFormRect(this);
            if (needRecalc)
                CalcZone();
        }
        private void RResize()
        {
            int space = 1;
            int perLine = ParAll.ST.Some.SensorsPerLine;
            if (perLine < 1)
                perLine = 1;
            int w = (ClientSize.Width - space) / perLine;
            if (RK.ST.cDef(Tp) == null
                || RK.ST.cDef(Tp).result == null
                || RK.ST.cDef(Tp).result.MZone.Count == 0
                || RK.ST.cDef(Tp).result.MZone.Count == 0
                || RK.ST.cDef(Tp).result.MZone[0].MSensor.Length == 0)
                return;
            int sensors = RK.ST.cDef(Tp).result.MZone[0].MSensor.Length;
            int lines = sensors / perLine;
            if (lines * perLine < sensors)
                lines++;
            int h = (ClientSize.Height - panel1.Height - space) / lines;
            int x = 0;
            int y = 0;
            foreach (USensor p in Controls.OfType<USensor>())
            {
                p.Top = panel1.Height + space + y * h;
                p.Height = h - space;
                p.Left = space + x * w;
                p.Width = w - space;
                x++;
                if (x == perLine)
                {
                    x = 0;
                    y++;
                }
            }
        }
        private void FSensors_Resize(object sender, EventArgs e)
        {
            RResize();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!cbCalibr.Checked)
            {
                switch (keyData)
                {
                    case Keys.Left:
                        RK.ST.cDef(Tp).Zone -= 1;
                        Draw();
                        break;
                    case Keys.Right:
                        RK.ST.cDef(Tp).Zone += 1;
                        Draw();
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void Draw()
        {
            Text = string.Format("{0}: Зона: {1}", title, RK.ST.cDef(Tp).Zone.Value + 1);
            for (int i = 0; i < L.Count(); i++)
                L[i].Init(RK.ST.cDef(Tp).result, RK.ST.cDef(Tp).Zone.Value, i);
        }

        void SetState()
        {
            if (needRecalc)
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

        void SetGainsFromPars()
        {
            L_L502Ch lch = new DefCL(Tp).LCh;
            for (int i = 0; i < L.Count; i++)
                L[i].Gain = lch[i].Gain;
        }
        void SetGainsToPars()
        {
            L_L502Ch lch = new DefCL(Tp).LCh;
            for (int i = 0; i < L.Count; i++)
                lch[i].Gain = L[i].Gain;
        }

        void p_OnGain()
        {
            needRecalc = true;
            SetState();
        }

        void p_OnCalibrate(int _sensor)
        {
            bool[] mb = new bool[L.Count];
            double[] gains = new double[L.Count];
            for (int i = 0; i < L.Count; i++)
            {
                mb[i] = L[i].IsCalibr;
                gains[i] = L[i].Gain;
            }
            Zone Z = RK.ST.cDef(Tp).result.MZone[RK.ST.cDef(Tp).Zone.Value];
            Z.Calibrate(_sensor, mb, gains);
            for (int i = 0; i < L.Count; i++)
                L[i].Gain = gains[i];
            Draw();
            needRecalc = true;
            SetState();
        }

        void p_OnStep(double _step)
        {
            foreach (USensor us in Controls.OfType<USensor>())
                us.Step = _step;
        }


        private void cbCalibr_CheckedChanged(object sender, EventArgs e)
        {
            foreach (USensor us in L)
                us.CalibrVisible = cbCalibr.Checked;
            bConfirm.Visible = cbCalibr.Checked;
            bCancel.Visible = cbCalibr.Checked;
            SetState();
        }

        void bConfirm_Click(object sender, EventArgs e)
        {
            SetGainsToPars();
            CalcZone();
            Draw();
            needRecalc = false;
            NeedRecalc = true;
            SetState();
        }

        void bCancel_Click(object sender, EventArgs e)
        {
            SetGainsFromPars();
            CalcZone();
            Draw();
            needRecalc = false;
            SetState();
        }
        void CalcZone()
        {
            double[] gains = new double[L.Count];
            for (int i = 0; i < L.Count; i++)
                gains[i] = L[i].Gain;
            Zone Z = RK.ST.cDef(Tp).result.MZone[RK.ST.cDef(Tp).Zone.Value];
            Z.CalcClassGain(gains);
        }
    }
}
