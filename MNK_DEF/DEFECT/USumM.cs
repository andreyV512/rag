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
using UPAR;
using RAGLib;
using Share;

namespace Defect
{
    public partial class USumM : UBase
    {
        DataPointCollection mp;
        DataPointCollection mp1;
        public USumM()
        {
            InitializeComponent();
            mp = chart1.Series["Series1"].Points;
            mp1 = chart1.Series["Series2"].Points;
        }
        public void Init()
        {
            LoadSettings();
        }
        double K;
        void LoadSettings()
        {
            K = ParAll.ST.ZoneSize;
            K/=1000;
            chart1.Series["Series2"].Color = ParAll.ST.Colors.GoodArea;
            chart1.ChartAreas[0].AxisX.Maximum = ParAll.ST.MaxZones * K;

        }
        internal void Draw()
        {
            SumResult sumResult = RK.ST.result.Sum;
            Clear();
            for (int i = 0; i < sumResult.MClass.Count(); i++)
            {
                double pos = i;
                pos *= K;
                pos += 0.5*K;
                DataPoint p = new DataPoint(pos, 1);
                p.Color = Classer.GetColor(sumResult.MClass[i]);
                mp.Add(p);
            }
        }
        public void DrawGoodArea()
        {
            if (RK.ST.result == null)
                return;
            SumResult sumResult = RK.ST.result.Sum;
            cbGoodBad.Checked = sumResult.RClass == EClass.Brak;
            cbGoodBad_CheckedChanged(null, null);
            cbGoodBad.Enabled = true;
            mp1.Clear();
            SumResult.PP pp = sumResult.MaxGood;
            if (pp == null)
                return;
            double pos = pp.index;
            pos *= K;
            mp1.Add(new DataPoint(pos, 1.2));
            pos = pp.index+pp.count;
            pos *= K;
            mp1.Add(new DataPoint(pos, 1.2));
        }
        public void Clear()
        {
            mp.Clear();
            mp1.Clear();
            cbGoodBad.Checked = false;
            cbGoodBad.Enabled = false;
        }

        private void cbGoodBad_CheckedChanged(object sender, EventArgs e)
        {
            cbGoodBad.Text = cbGoodBad.Checked?"БРАК":"ГОДНО";
            cbGoodBad.BackColor = cbGoodBad.Checked ? ParAll.ST.Colors.Brak : ParAll.ST.Colors.Class1;
            if (cbGoodBad.Enabled)
                RK.ST.result.Sum.RClass = cbGoodBad.Checked ? EClass.Brak : EClass.Class1;
        }
    }
}
