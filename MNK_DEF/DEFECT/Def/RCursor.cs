using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace Defect.Def
{
    public class RCursor
    {
        Series series;
        DataPoint p0;
        DataPoint p1;
        Axis axisX;
        public RCursor(Chart _chart)
        {
            series = new Series();
            series.Enabled = false;
            series.ChartType = SeriesChartType.Line;
            series.Color = Color.Black;
            axisX = _chart.ChartAreas[0].AxisX;
            p0 = new DataPoint(0, _chart.ChartAreas[0].AxisY.Minimum);
            p1 = new DataPoint(0, _chart.ChartAreas[0].AxisY.Maximum);
            series.Points.Add(p0);
            series.Points.Add(p1);
            _chart.Series.Add(series);
            _chart.MouseWheel += new System.Windows.Forms.MouseEventHandler(chart_MouseWheel);
            _chart.MouseClick += new System.Windows.Forms.MouseEventHandler(chart_MouseClick);
        }
        public RCursor(Chart _chart, Color _color) : this(_chart) { series.Color = _color; }
        public bool IsWheel = true;
        void chart_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(IsWheel)
                Position += e.Delta / 120;
        }
        void chart_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    double X = axisX.PixelPositionToValue(e.X);
                    Visible = true;
                    Position = (int)X;
                }
                catch { }
            }
            else if (e.Button == MouseButtons.Right)
                Visible = false;

        }
        public bool Visible
        {
            get { return (series.Enabled); }
            set
            {
                series.Enabled = value;
                if (OnMove != null)
                    OnMove(value ? (int?)p0.XValue : null);
            }
        }
        public int Position
        {
            get { return ((int)p0.XValue); }
            set
            {
                double val = value;
                if (val < axisX.Minimum)
                    val = axisX.Minimum;
                if (val > axisX.Maximum - 1)
                    val = axisX.Maximum - 1;
                p0.XValue = val;
                p1.XValue = val;
                if (OnMove != null)
                    OnMove(series.Enabled ? (int?)p0.XValue : null);
            }
        }
        public Color RColor { set { series.Color = value; } }
        public delegate void DOnMove(int? _x);
        public DOnMove OnMove = null;
    }
}
