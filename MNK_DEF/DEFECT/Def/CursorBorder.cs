using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace Defect.Def
{
    public class CursorBorder
    {

        Chart chart;
        List<CBPoint> L = new List<CBPoint>();
        void Clear(CBPoint.Etype _tp)
        {
            foreach (CBPoint dp in L)
                dp.Clear(_tp);
            L.RemoveAll(x => x.IsOn);
        }
        CBPoint FindAdd(CBPoint.Etype _tp, DataPoint _dp)
        {
            CBPoint cbdp = null;
            foreach (CBPoint p in L)
            {
                if (p.dp == _dp)
                {
                    cbdp = p;
                    break;
                }
            }
            if (cbdp == null)
                cbdp = new CBPoint(_dp);
            cbdp.Mark(_tp, true);
            L.Add(cbdp);
            return (cbdp);
        }


        public CursorBorder(Chart _chart)
        {
            chart = _chart;
        }
        public void DrawSingle(int _zone, int _sensor)
        {
            Clear(CBPoint.Etype.Single);
            if (_sensor < 0)
                return;
            if (_sensor >= chart.Series.Count)
                return;
            Series ser = chart.Series[_sensor];
            if (_zone < 0)
                return;
            if (_zone >= ser.Points.Count)
                return;
            CBPoint cbdp = FindAdd(CBPoint.Etype.Single, ser.Points[_zone]);
            cbdp.Draw();
        }
        public void DrawSingle2(int _zone, int _sensor)
        {
            Clear(CBPoint.Etype.Single2);
            if (_sensor < 0)
                return;
            if (_sensor >= chart.Series.Count)
                return;
            Series ser = chart.Series[_sensor];
            if (_zone < 0)
                return;
            if (_zone >= ser.Points.Count)
                return;
            CBPoint cbdp = FindAdd(CBPoint.Etype.Single2, ser.Points[_zone]);
            cbdp.Draw();
        }
        public void DrawColumn(int _zone)
        {
            Clear(CBPoint.Etype.Column);
            if (_zone < 0)
                return;
            foreach (Series ser in chart.Series)
            {
                if (_zone >= ser.Points.Count)
                    continue;
                CBPoint cbdp = FindAdd(CBPoint.Etype.Column, ser.Points[_zone]);
                cbdp.Draw();
            }
        }
        public void DrawColumn2(int _zone)
        {
            Clear(CBPoint.Etype.Column2);
            if (_zone < 0)
                return;
            foreach (Series ser in chart.Series)
            {
                if (_zone >= ser.Points.Count)
                    continue;
                CBPoint cbdp = FindAdd(CBPoint.Etype.Column2, ser.Points[_zone]);
                cbdp.Draw();
            }
        }
        public void DrawLine(int _zone0, int _zone1, int _sensor)
        {
            Clear(CBPoint.Etype.Line);
            if (_sensor < 0)
                return;
            if (_sensor >= chart.Series.Count)
                return;
            Series ser = chart.Series[_sensor];
            for (int z = _zone0; z <= _zone1; z++)
            {
                if (z < 0)
                    continue;
                if (z >= ser.Points.Count)
                    continue;
                CBPoint cbdp = FindAdd(CBPoint.Etype.Line, ser.Points[z]);
                cbdp.Draw();
            }
        }
        public void ClearSingle() { Clear(CBPoint.Etype.Single); }
        public void ClearSingle2() { Clear(CBPoint.Etype.Single2); }
        public void ClearLine() { Clear(CBPoint.Etype.Line); }
        public void ClearColumn() { Clear(CBPoint.Etype.Column); }
        public void ClearColumn2() { Clear(CBPoint.Etype.Column2); }
        public void Clear()
        {
            foreach (CBPoint dp in L)
                dp.Clear();
            L.Clear();
        }
    }
}
class CBPoint
{
    public DataPoint dp = null;
    public bool IsSingle = false;
    public bool IsSingle2 = false;
    public bool IsLine = false;
    public bool IsColumn = false;
    public bool IsColumn2 = false;
    public enum Etype { Single, Single2, Line, Column, Column2 }

    public CBPoint(DataPoint _dp)
    {
        dp = _dp;
    }
    public void Mark(Etype _tp, bool _val)
    {
        switch (_tp)
        {
            case Etype.Single:
                IsSingle = _val;
                break;
            case Etype.Single2:
                IsSingle2 = _val;
                break;
            case Etype.Line:
                IsLine = _val;
                break;
            case Etype.Column:
                IsColumn = _val;
                break;
            case Etype.Column2:
                IsColumn2 = _val;
                break;
        }
    }
    public bool IsOn { get { return (IsSingle || IsSingle2 || IsLine || IsColumn || IsColumn2); } }

    public void Clear(Etype _tp)
    {
        Mark(_tp, false);
        if (!IsOn)
            Clear();
    }
    public void Clear()
    {
        if (dp == null)
            return;
        dp.BorderWidth = 1;
        dp.BorderColor = Color.Empty;
    }
    public void Draw()
    {
        dp.BorderWidth = 2;
        dp.BorderColor = Color.Black;
    }
}
