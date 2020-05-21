using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Share;
using ResultLib;
using ResultLib.Def;
using UPAR;
using UPAR.Def;
using UPAR.TS;
using Protocol;

namespace Defect.Def
{
    public partial class USensorData : UserControl
    {
        ResultDef resultDef;
        int iz0;
        int iz1;
        int ise;
        BorderLine bline = null;
        BorderLine blineIn = null;
        RCursor cursor;
        public USensorData()
        {
            InitializeComponent();
            cursor = new RCursor(chart1);
            cursor.OnMove = OnMove;
        }
        public bool IsWheel { get { return (cursor.IsWheel); } set { cursor.IsWheel = value; } }
        public void InitSingle(ResultDef _resultDef, int _iz, int _is, double _gain)
        {
            InitRange(_resultDef, _iz, _iz, _is, _gain);
        }
        public void InitRange(ResultDef _resultDef, int _iz0, int _iz1, int _is, double _gain)
        {
            chart1.Series.SuspendUpdates();
            Clear();
            TypeSize ts = ParAll.ST.TSSet.Current;
            resultDef = _resultDef;
            iz0 = _iz0;
            iz1 = _iz1;
            ise = _is;
            Color DeviderColor = ParAll.ST.Some.SignalsView.DeviderColor;
            DataPointCollection p = chart1.Series[0].Points;
            DataPointCollection pIn = chart1.Series[0].Points;
            int ip = 0;
            int ipIn = 0;
            int istrip = 0;
            chart1.ChartAreas[0].AxisX.StripLines.Clear();
            DefCL dcl = new DefCL(_resultDef.Tp);
            bool IsIn = dcl.IsFinterIn;
            for (int ii = _iz0; ii <= _iz1; ii++)
            {
                Meas[] MMeas = _resultDef.MZone[ii].MSensor[ise].MMeas;
                if (istrip > 0)
                {
                    StripLine sl = new StripLine();
                    sl.BorderColor = DeviderColor;
                    sl.BorderWidth = 2;
                    sl.IntervalOffset = ip;
                    chart1.ChartAreas[0].AxisX.StripLines.Add(sl);
                }
                istrip++;
                for (int i = 0; i < MMeas.Length; i++)
                {
                    Meas m = MMeas[i];
                    DataPoint dp = new DataPoint(ip, m.FilterABC * _gain);
                    dp.Color = m.Dead ? Classer.GetColor(EClass.None) : Classer.GetColor(m.Class);
                    dp.Tag = new PointSubj(ii, ise, i);
                    p.Add(dp);
                    ip++;
                    if (IsIn)
                    {
                        pIn.AddXY(ipIn, -m.FilterInABC * _gain);
                        p[ipIn].Color = m.Dead ? Classer.GetColor(EClass.None) : Classer.GetColor(m.ClassIn);
                        ipIn++;
                    }
                }
                istrip++;
            }
            if (bline != null)
                bline.Visible = false;
            if (blineIn != null)
                blineIn.Visible = false;
            bline = new BorderLine(chart1.ChartAreas[0].AxisY, Classer.GetColor(EClass.Brak), Classer.GetColor(EClass.Class2));
            blineIn = new BorderLine(chart1.ChartAreas[0].AxisY, Classer.GetColor(EClass.Brak), Classer.GetColor(EClass.Class2));
            bline.SetBorders(dcl.Borders);
            bline.Visible = true;

            if (IsIn)
            {
                double[] borders = new double[2];
                borders[0] = -dcl.Border1In;
                borders[1] = -dcl.Border2In;
                blineIn.SetBorders(borders);
                blineIn.Visible = true;
            }
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            if (IsIn)
                chart1.ChartAreas[0].AxisY.Minimum = -100;
            else
                chart1.ChartAreas[0].AxisY.Minimum = 0;
            if (cursor.Visible)
                OnMove(cursor.Position);
            chart1.Series[0].Enabled = true;
            chart1.Series[1].Enabled = IsIn;
            chart1.Series.ResumeUpdates();
        }
        public void Clear()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].Points.Clear();
            if (bline != null)
                bline.Visible = false;
            if (blineIn != null)
                blineIn.Visible = false;
            bline = null;
            blineIn = null;
            cursor.Visible = false;
        }
        public delegate void DOnMoseMove(int? _x, int? _y);
        public DOnMoseMove OnMouseMoveR = null;
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (OnMouseMoveR == null)
                return;
            try
            {
                ChartArea ca = chart1.ChartAreas[0];
                double X = ca.AxisX.PixelPositionToValue(e.X);
                double Y = ca.AxisY.PixelPositionToValue(e.Y);
                if (X >= ca.AxisX.Minimum &&
                    X < ca.AxisX.Maximum &&
                    Y >= ca.AxisY.Minimum &&
                    Y < ca.AxisY.Maximum)
                    OnMouseMoveR((int?)X, (int?)Y);
                else
                    OnMouseMoveR(null, null);
            }
            catch
            {
            }
        }
        void OnMove(int? _x)
        {
            if (OnCursorMove == null)
                return;
            if (_x == null)
                OnCursorMove(null, 0, Color.Black);
            else
            {
                if (_x < 0)
                    OnCursorMove(null, 0, Color.Black);
                else
                {
                    DataPointCollection p = chart1.Series[0].Points;
                    if (_x >= p.Count())
                        OnCursorMove(null, 0, Color.Black);
                    else
                    {
                        OnCursorMove(p[(int)_x].Tag as PointSubj, p[(int)_x].YValues[0], p[(int)_x].Color);
                    }
                }
            }
        }
        public delegate void DOnCursorMove(PointSubj _psbj, double _y, Color _color);
        public DOnCursorMove OnCursorMove = null;

        private void chart1_MouseEnter(object sender, EventArgs e)
        {
            if (CanFocused)
            {
                if (!chart1.Focused)
                    chart1.Focus();
            }
        }

        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            if (CanFocused)
            {
                if (chart1.Focused)
                    chart1.Parent.Focus();
            }
        }
        public bool CanFocused { get; set; }
        void pr(string _msg)
        {
            ProtocolST.pr("USensorData: " + _msg);
        }
        public void WheelCursor(int _delta)
        {
            if (chart1.Focused)
                cursor.Position += _delta;
        }
    }
    public class PointSubj
    {
        public int? iZ = null;
        public int? iS = null;
        public int? iM = null;

        public PointSubj() { }
        public PointSubj(int _iZ, int _iS, int _iM)
        {
            iZ = _iZ;
            iS = _iS;
            iM = _iM;
        }
        public override string ToString()
        {
            string ret = null;
            if (iZ != null)
                ret += "З:" + iZ.ToString() + " ";
            if (iS != null)
                ret += "Д:" + iS.ToString() + " ";
            if (iM != null)
                ret += "И:" + iM.ToString() + " ";
            return ret;
        }
    }
}
