using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

using UPAR;

namespace Defect.Def
{
    class BorderLine
    {
        StripLine s0 = null;
        StripLine s1 = null;
        Axis axis;
        bool inited = false;
        bool On = false;
        public BorderLine(Axis _axis, Color _color0, Color _color1)
        {
            axis = _axis;
            s0 = new StripLine();
            s0.BorderColor = _color0;
            s0.BorderWidth = ParAll.ST.Some.BorderFatness;
            s1 = new StripLine();
            s1.BorderColor = _color1;
            s1.BorderWidth = ParAll.ST.Some.BorderFatness;
        }
        public void SetBorders(double[] _borders)
        {
            if (_borders == null)
                return;
            if (_borders.Length != 2)
                return;
            inited = true;
            s0.IntervalOffset = _borders[0];
            s1.IntervalOffset = _borders[1];
        }
        public BorderLine(Axis _axis, Color _color0, Color _color1, double[] _borders, int _fatness)
            : this(_axis, _color0, _color1)
        {
            SetBorders(_borders);
        }
        public bool Visible
        {
            set
            {
                if (!inited)
                    return;
                if (value)
                {
                    if (On)
                    {
                        if (s0.IntervalOffset != 0)
                            axis.StripLines.Remove(s0);
                        if (s1.IntervalOffset != 0)
                            axis.StripLines.Remove(s1);
                    }
                    On = true;
                    if (s0.IntervalOffset != 0)
                        axis.StripLines.Add(s0);
                    if (s1.IntervalOffset != 0)
                        axis.StripLines.Add(s1);
                }
                else
                {
                    if (On)
                    {
                        if (s0.IntervalOffset != 0)
                            axis.StripLines.Remove(s0);
                        if (s1.IntervalOffset != 0)
                            axis.StripLines.Remove(s1);
                    }
                    On = false;
                }
            }
        }
    }
}
