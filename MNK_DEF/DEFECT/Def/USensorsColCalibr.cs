using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ResultLib;
using ResultLib.Def;
using UPAR;
using UPAR.Def;
using Share;

namespace Defect.Def
{
    public partial class USensorsColCalibr : UserControl
    {
        List<USensor> L = new List<USensor>();
        public bool NeedRecalc { get; private set; }
        int? H = null;
        int space = 1;
        public delegate void DOnRecalc();
        DOnRecalc OnRecalc = null;
        EUnit Tp;
        public USensorsColCalibr()
        {
            InitializeComponent();
            NeedRecalc = false;
        }
        public void Init(EUnit _Tp, DOnRecalc _OnRecalc)
        {
            Tp = _Tp;
            OnRecalc = _OnRecalc;
            if (RK.ST.cDef(Tp).Zone == null)
            {
                return;
            }
            H = ParAll.ST.Defect.Some.SensorHeight;
            Zone z = RK.ST.cDef(Tp).result.MZone[0];
            for (int i = 0; i < z.MSensor.Length; i++)
            {
                USensor p = new USensor();
                p.OnStep += new UCalibr.DOnStep(p_OnStep);
                p.OnCalibrate += new USensor.DOnCalibrate(p_OnCalibrate);
                p.OnGain += new USensor.DOnGain(p_OnGain);
                p.CalibrVisible = false;
                p.IsWheel = false;
                p.CanFocused = true;
                L.Add(p);
                Controls.Add(p);
            }
            RResize();
            ResizeCharts();
            SetGainsFromPars();
            IsScroll = true;
        }
        public void RDraw()
        {
            for (int i = 0; i < L.Count(); i++)
                L[i].Init(RK.ST.cDef(Tp).result, RK.ST.cDef(Tp).Zone.Value, i);
        }
        void ResizeCharts()
        {
            if (H == null)
                return;
            ParAll.ST.Defect.Some.SensorHeight = H.Value;
            Point pos = AutoScrollPosition;
            SuspendLayout();
            AutoScrollPosition = Point.Empty;
            int space = 0;
            int ltop = 0;
            foreach (USensor u in Controls.OfType<USensor>())
            {
                (u as Control).Top = ltop;
                (u as Control).Height = H.Value;
                ltop += H.Value + space;
            }
            ResumeLayout();
            AutoScrollPosition = new Point(Math.Abs(pos.X), Math.Abs(pos.Y));
        }
        void SetGainsFromPars()
        {
            DefCL dcl = new DefCL(Tp);
            for (int i = 0; i < L.Count; i++)
                L[i].Gain = dcl.LCh[i].Gain;
        }
        void SetGainsToPars()
        {
            DefCL dcl = new DefCL(Tp);
            for (int i = 0; i < L.Count; i++)
                dcl.LCh[i].Gain = L[i].Gain;
        }
        void p_OnGain()
        {
            NeedRecalc = true;
            OnRecalc();
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
            RDraw();
            NeedRecalc = true;
            OnRecalc();       
        }
       
        public bool CalibrVisible
        {
            set
            {
                foreach (USensor u in L)
                    u.CalibrVisible = value;
            }
        }
        void CalcZone()
        {
            double[] gains = new double[L.Count];
            for (int i = 0; i < L.Count; i++)
                gains[i] = L[i].Gain;
            Zone Z = RK.ST.cDef(Tp).result.MZone[RK.ST.cDef(Tp).Zone.Value];
            Z.CalcClassGain(gains);
        }
        void p_OnStep(double _step)
        {
            foreach (USensor us in L)
                us.Step = _step;
        }

        void USensorsColCalibr_Resize(object sender, EventArgs e)
        {
            RResize();   
        }
        void RResize()
        {
            foreach (USensor p in L)
                p.Width = ClientSize.Width - space * 2;
        }
        public int HH
        {
            get { return (H==null?0:H.Value); }
            set
            {
                if (H == null)
                    return;
                H = value;
                ResizeCharts();
            }
        }
        public void Confirm()
        {
            SetGainsToPars();
            CalcZone();
            RDraw();
            NeedRecalc = false;
        }
        public void RCancel()
        {
            SetGainsFromPars();
            CalcZone();
            RDraw();
            NeedRecalc = false;
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x20a)
            {
                if (!IsScroll)
                {
                    foreach (USensor u in L)
                        u.WheelCursor(m.WParam.ToInt32()>0?1:-1);
                    return;
                }
            }
            base.WndProc(ref m);
        }
        public bool IsScroll { get; set; }
    }
}
