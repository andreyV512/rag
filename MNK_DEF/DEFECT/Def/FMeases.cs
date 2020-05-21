using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class FMeases : FBase, IView
    {
        public string SaveName = "FDataMeases";
//        ResultDef resultDef;
        string title;
        //int iz;
        //int ise;

        SignalsViewPars sv;
        RUnit Source;
        RUnit Filter;
        RUnit FilterIn;
        RUnit Median;
        RCursor cursor;
        CursorBorder cursorBorder;

        Meas[] m;
        DefCL dcl;
        EUnit Tp;
        
        public FMeases(string _title, CursorBorder _cursorBorder, EUnit _tp)
        {
            InitializeComponent();
            title = _title;
            Source = new RUnit(RUnit.EType.Source, chart1.Series[0], chart1.ChartAreas[0].AxisY, CBSource, "Исходный сигнал", toolTip1);
            Median = new RUnit(RUnit.EType.Median, chart1.Series[1], chart1.ChartAreas[0].AxisY, CBMedian, "Медианный сигнал", toolTip1);
            Filter = new RUnit(RUnit.EType.Filter, chart1.Series[2], chart1.ChartAreas[0].AxisY, CBFilter, "Фильтрованный сигнал", toolTip1);
            FilterIn = new RUnit(RUnit.EType.FilterIn, chart1.Series[3], chart1.ChartAreas[0].AxisY, CBFilterIn, "Фильтрованный внутренний сигнал", toolTip1);
            OnHide = null;
            cursor = new RCursor(chart1);
            cursor.OnMove = OnMove;
            label1.Text = null;
            label2.Text = null;
            cursorBorder = _cursorBorder;
            OnHide = ExecHide;
            Tp = _tp;
            dcl = new DefCL(_tp);
        }
        public void RDraw()
        {
//            resultDef = _resultDef;
            RDraw0();
        }
        void RDraw0()
        {
            if (RK.ST.cDef(Tp).Sensor == null)
            {
                Clear();
                return;
            }
            cursor.Visible = false;
            Text = string.Format("{0}: Зона: {1}, Датчик: {2}", title, RK.ST.cDef(Tp).Zone.Value + 1, RK.ST.cDef(Tp).Sensor.Value + 1);
            m = RK.ST.cDef(Tp).result.MZone[RK.ST.cDef(Tp).Zone.Value].MSensor[RK.ST.cDef(Tp).Sensor.Value].MMeas;
            
            double gain = dcl.LCh[RK.ST.cDef(Tp).Sensor.Value].Gain;
            sv = ParAll.ST.Some.SignalsView;
            TypeSize ts = ParAll.ST.TSSet.Current;

            Source.Load(m, gain, sv.Source, ParAll.ST.Colors.NotMeasured, dcl.Borders, true);
            Median.Load(m, gain, sv.Median, ParAll.ST.Colors.NotMeasured, dcl.Borders, true);
            Filter.Load(m, gain, sv.Filter, ParAll.ST.Colors.NotMeasured, dcl.Borders, dcl.Filter.IsFilter);
            FilterIn.Load(m, gain, sv.FilterIn, ParAll.ST.Colors.NotMeasured, dcl.BordersIn, dcl.FilterIn.IsFilter);
            cursorBorder.DrawSingle2(RK.ST.cDef(Tp).Zone.Value, RK.ST.cDef(Tp).Sensor.Value);
            base.Init();
        }
        public void Clear()
        {
            cursor.Visible = false;
            Text = title;
            Source.Clear();
            Median.Clear();
            Filter.Clear();
            FilterIn.Clear();
            cursorBorder.ClearSingle2();
        }
        override public void Save()
        {
            Source.Save();
            Filter.Save();
            FilterIn.Save();
            Median.Save();
            base.Save();
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ChartArea ca = chart1.ChartAreas[0];
                double X = ca.AxisX.PixelPositionToValue(e.X);
                double Y = ca.AxisY.PixelPositionToValue(e.Y);
                if (X >= ca.AxisX.Minimum &&
                    X < ca.AxisX.Maximum &&
                    Y >= ca.AxisY.Minimum &&
                    Y < ca.AxisY.Maximum)
                    label1.Text = string.Format("[{0},{1}]", ((int)X).ToString(), ((int)Y).ToString());
                else
                    label1.Text = "";
            }
            catch
            {
                label1.Text = "";
            }
        }
        void OnMove(int? _x)
        {
            //string sss;
            //if (_x == null)
            //    sss = null;
            //else
            //    sss = "index=" + m[_x.Value].index.ToString();

            label2.Text = (_x != null ? _x.ToString() + " " : null) + Source.SVal(_x) + Median.SVal(_x) + Filter.SVal(_x) + FilterIn.SVal(_x);
        }
        bool CanFocused { get; set; }

        private void FMeases_Activated(object sender, EventArgs e)
        {
            CanFocused = true;
        }

        private void FMeases_Deactivate(object sender, EventArgs e)
        {
            CanFocused = false;
        }

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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    RK.ST.cDef(Tp).Zone -= 1;
                    RDraw0();
                    break;
                case Keys.Right:
                    RK.ST.cDef(Tp).Zone += 1;
                    RDraw0();
                    break;
                case Keys.Up:
                    RK.ST.cDef(Tp).Sensor += 1;
                    RDraw0();
                    break;
                case Keys.Down:
                    RK.ST.cDef(Tp).Sensor -= 1;
                    RDraw0();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void ExecHide()
        {
            cursorBorder.ClearSingle2();
        }
        void pr(string _msg)
        {
            ProtocolST.pr("FMeases: " + _msg);
        }

    }

}
