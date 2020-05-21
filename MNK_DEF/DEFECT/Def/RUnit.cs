using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using ResultLib;
using ResultLib.Def;
using Share;
using UPAR;

namespace Defect.Def
{
    class RUnit
    {
        public enum EType { Source, Median, Filter, FilterIn }
        EType type;
        CheckBox box;
        SignalViewPars pars;
        Series series;
        BorderLine bline = null;
        BorderLine bline1 = null;
        bool IsOn = false;

        public RUnit(EType _type, Series _series, Axis _axisY, CheckBox _box, string _hint, ToolTip _tt)
        {
            type = _type;
            series = _series;
            bline = new BorderLine(_axisY, Classer.GetColor(EClass.Brak), Classer.GetColor(EClass.Class2));
            bline1 = new BorderLine(_axisY, Classer.GetColor(EClass.Brak), Classer.GetColor(EClass.Class2));
            box = _box;
            _tt.SetToolTip(box, _hint);
            box.CheckedChanged += new System.EventHandler(CheckedChanged);
        }
        public void Save()
        {
            if (pars != null)
                pars.View = box.Checked;
        }
        void CheckedChanged(object sender, EventArgs e)
        {
            series.Enabled = box.Checked;
            box.ForeColor = box.Checked ? pars.SColor : Color.Black;
            bline.Visible = box.Checked;
            bline1.Visible = box.Checked;
            pars.View = box.Checked;
        }
        public void Load(Meas[] _MMeas, double _gain, SignalViewPars _pars, Color _color_dead, double[] _borders, bool _IsOn)
        {
            pars = _pars;
            IsOn = _IsOn;
            DataPointCollection pp = series.Points;
            pp.Clear();
            box.Visible = IsOn;
            series.Enabled = IsOn;
            bline.Visible = IsOn;
            bline1.Visible = IsOn;
            if (!IsOn)
                return;

            series.Color = _pars.SColor;
            series.Enabled = _pars.View;
            box.Checked = _pars.View;
            box.ForeColor = _pars.View ? _pars.SColor : Color.Black;
            if (_MMeas == null)
                return;
            for (int i = 0; i < _MMeas.Length; i++)
            {
                double v = 0;
                Meas m = _MMeas[i];
                switch (type)
                {
                    case EType.Source:
                        v = m.Source;
                        break;
                    case EType.Median:
                        v = m.Median;
                        break;
                    case EType.Filter:
                        v = m.Filter;
                        break;
                    case EType.FilterIn:
                        v = -m.FilterIn;
                        break;
                }
                v *= _gain;
                DataPoint p = new DataPoint(i, v);
                p.Color = m.Dead ? _color_dead : _pars.SColor;
                pp.Add(p);
            }
            bline.SetBorders(_borders);
            bline.Visible = box.Checked;
            bline1.SetBorders(new double[2] { -_borders[0], -_borders[1] });
            bline1.Visible = box.Checked;
        }
        public void Clear()
        {
            series.Points.Clear();
            box.Visible = false;
            series.Enabled = false;
            bline.Visible = false;
            bline1.Visible = false;
        }
        public double? Val(int? _x)
        {
            if (_x == null)
                return (null);
            if (_x < 0)
                return (null);
            if (!box.Checked)
                return (null);
            DataPointCollection p = series.Points;
            if (_x >= p.Count())
                return (null);
            return (p[(int)_x].YValues[0]);
        }
        public string SVal(int? _x)
        {
            double? ret = Val(_x);
            if (ret == null)
                return (null);
            return (box.Text + ":" + ((double)ret).ToString("0.0##") + " ");
        }
    }
}

