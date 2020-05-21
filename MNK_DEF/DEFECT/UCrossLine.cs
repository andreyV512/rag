using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using ResultLib;
using ResultLib.Def;
using UPAR;
using UPAR.Def;
using Defect.Def;
using RAGLib;
using Share;
using RectifierNS;

namespace Defect
{
    public partial class UCrossLine : UBase
    {
        FData fData = null;
        FData fDataRange = null;
        FMeases fMeases = null;
        public delegate void DOnRecalc();
        public DOnRecalc OnRecalc = null;
        DefSomePars.ETypeView typeView = DefSomePars.ETypeView.Defect;
        CursorBorder cursorBorder;

        public UCrossLine()
        {
            InitializeComponent();
            cursorBorder = new CursorBorder(chart1);
        }

        ResultDef resultDef = null;
        EUnit Tp;
        public void Init(EUnit _Tp)
        {
            Tp = _Tp;
            fData = new FData(Tp, Title, 0, cursorBorder);
            fDataRange = new FData(Tp, Title, 3, cursorBorder);
            fDataRange.SaveName += "Range";
            fMeases = new FMeases(Title, cursorBorder, Tp);
            typeView = ParAll.ST.Defect.Some.TypeView;
            LoadSettings();
        }
        public void LoadSettings()
        {
            if (Tp == EUnit.Cross)
            {
                CBIsWork.Checked = ParAll.ST.Defect.Cross.IsWork;
                Borders = ParAll.CTS.Cross.Borders;
            }
            else
            {
                CBIsWork.Checked = ParAll.ST.Defect.Line.IsWork;
                Borders = ParAll.CTS.Line.Borders;
            }
            chart1.ChartAreas[0].AxisX.Maximum = ParAll.ST.MaxZones + 0.5;
        }
        public void Save()
        {
            ParAll.ST.Defect.Some.TypeView = typeView;
            fData.Save();
            fDataRange.Save();
            fMeases.Save();
        }
        public void RHide()
        {
            fData.Hide();
            fDataRange.Hide();
            fMeases.Hide();
        }
        public void Draw()
        {
            ResultDef resultDef = null;
            if (Tp == EUnit.Cross)
                resultDef = RK.ST.result.Cross;
            else
                resultDef = RK.ST.result.Line;
            Clear();
            for (int zi = 0; zi < resultDef.MZone.Count(); zi++)
                AddZone(resultDef, zi);
        }
        void SetSeries(int _sensors)
        {
            if (_sensors != chart1.Series.Count)
            {
                chart1.ChartAreas[0].AxisY.Maximum = _sensors;
                chart1.Series.Clear();
                for (int i = 0; i < _sensors; i++)
                {
                    Series s = new Series();
                    s.ChartType = SeriesChartType.StackedColumn;
                    s.CustomProperties = "DrawingStyle=Emboss,PointWidth=1";
                    chart1.Series.Add(s);
                }
            }
        }
        void AddZone(ResultDef _resultDef, int _izone)
        {
            Zone zone = _resultDef.MZone[_izone];
            SetSeries(zone.MSensor.Length);
            for (int si = 0; si < zone.MSensor.Count(); si++)
            {
                DataPoint dp = chart1.Series[si].Points.Add(1);
                dp.Color = Classer.GetColor(zone.MSensor[si].Class);
            }
        }
        public void Clear()
        {
            cursorBorder.Clear();
            foreach (Series s in chart1.Series)
                s.Points.Clear();
            RHide();
            ViewMode = false;
        }
        void InitForm()
        {
            RHide();
            cursorBorder.Clear();
            if (RK.ST.cDef(Tp) == null
                || RK.ST.cDef(Tp).result == null
                || RK.ST.cDef(Tp).result.MZone.Count == 0
                || RK.ST.cDef(Tp).result.MZone.Count == 0
                || RK.ST.cDef(Tp).result.MZone[0].MSensor.Length == 0)
                return;

            if (typeView == DefSomePars.ETypeView.Defect)
                fData.RDraw();
            else if (typeView == DefSomePars.ETypeView.DefectRange)
                fDataRange.RDraw();
            else if (typeView == DefSomePars.ETypeView.DefectZone)
            {
                if (!ViewMode)
                    return;
                using (FSensors f = new FSensors(Title, Tp))
                {
                    f.ShowDialog();
                    if (f.NeedRecalc)
                    {
                        if (OnRecalc != null)
                            OnRecalc();
                    }
                }
            }
            else if (typeView == DefSomePars.ETypeView.Column)
            {
                if (!ViewMode)
                    return;
                using (FSensorsColCalibr f = new FSensorsColCalibr(Tp, Title))
                {
                    f.ShowDialog();
                    if (f.NeedRecalc)
                    {
                        if (OnRecalc != null)
                            OnRecalc();
                    }
                }
            }
            else if (typeView == DefSomePars.ETypeView.Source)
                fMeases.RDraw();
        }
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult ht = chart1.HitTest(e.X, e.Y);
            if (ht == null)
                return;
            if (ht.ChartElementType != ChartElementType.DataPoint)
                return;
            RK.ST.cDef(Tp).Zone = ht.PointIndex;
            RK.ST.cDef(Tp).Sensor = chart1.Series.IndexOf(ht.Series);
            if (e.Button == MouseButtons.Left)
                InitForm();
            else if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(chart1, e.X, e.Y);
        }
        PCursor pCursorBlack = new PCursor(Color.Black);
        class PCursor
        {
            public PCursor(Color _selected)
            {
                selected = _selected;
            }
            DataPoint p = null;
            Color pcolor = Color.Black;
            Color selected;
            int width;
            public void Exec(DataPoint _p)
            {
                Clear();
                p = _p;
                pcolor = p.BorderColor;
                p.BorderColor = selected;
                width = p.BorderWidth;
                p.BorderWidth = 2;
            }
            public void Clear()
            {
                if (p != null)
                {
                    p.BorderColor = pcolor;
                    p.BorderWidth = width;
                }
            }
        }

        private void расчетныеДефектыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            typeView = DefSomePars.ETypeView.Defect;
            InitForm();
        }

        private void дефектыПоЗонеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            typeView = DefSomePars.ETypeView.DefectZone;
            InitForm();
        }

        private void исходныеСигналыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            typeView = DefSomePars.ETypeView.Source;
            InitForm();
        }

        private void дефектыПо3ДатчикамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            typeView = DefSomePars.ETypeView.DefectRange;
            InitForm();
        }
        private void дефектыВСтолбецToolStripMenuItem_Click(object sender, EventArgs e)
        {
            typeView = DefSomePars.ETypeView.Column;
            InitForm();
        }

        private void CBIsWork_CheckedChanged(object sender, EventArgs e)
        {
            if (Tp == EUnit.Cross)
                ParAll.ST.Defect.Cross.IsWork = (sender as CheckBox).Checked;
            else
                ParAll.ST.Defect.Line.IsWork = (sender as CheckBox).Checked;
        }
        public bool ViewMode
        {
            get { return (дефектыПоЗонеToolStripMenuItem.Enabled); }
            set
            {
                дефектыПоЗонеToolStripMenuItem.Enabled = value;
                дефектыВСтолбецToolStripMenuItem.Enabled = value;
            }
        }
        public bool IsWorkVisible { get { return (CBIsWork.Visible); } set { CBIsWork.Visible = value; } }
        public UITH StateH { get { return (uRectifierVew1.StateH); } set { uRectifierVew1.StateH = value; } }
        public double[] Borders { get { return (uBorders1.Borders); } set { uBorders1.Borders = value; } }
    }
}
