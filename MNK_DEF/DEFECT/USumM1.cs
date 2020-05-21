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
    public partial class USumM1 : UBase
    {
        DataPointCollection mp;
        DataPointCollection mp1;
        const int space = 2;
        public USumM1()
        {
            InitializeComponent();
            mp = chart1.Series["Series1"].Points;
            mp1 = chart1.Series["Series2"].Points;
            uTube.ResizeByTitle();
            Clear();
        }
        public void Init()
        {
            uSelectResult1.Init();
            uSelectResult1.OnClass += new USelectResult.DOnClass(uSelectResult1_OnClass);
            LoadSettings();
        }

        void uSelectResult1_OnClass(EClass _rClass)
        {
            RK.ST.result.Sum.RClass = uSelectResult1.RClass;
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
            Clear();
            if (RK.ST.result == null)
                return;
            uTube.Value = RK.ST.result.IdTube.ToString();
            SumResult sumResult = RK.ST.result.Sum;
            for (int i = 0; i < sumResult.MClass.Count(); i++)
            {
                double pos = i;
                pos *= K;
                pos += 0.5*K;
                DataPoint p = new DataPoint(pos, 1);
                p.Color = Classer.GetColor(sumResult.MClass[i]);
                mp.Add(p);
            }
            uSelectResult1.RClass = sumResult.RClass;
            //uSelectResult1.Enabled = true;
        }
        public void DrawGoodArea()
        {
            if (RK.ST.result == null)
                return;
            SumResult sumResult = RK.ST.result.Sum;
            uSelectResult1.Enabled = true;
            uSelectResult1.RClass = sumResult.RClass;
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
            uTube.Clear();
            uSelectResult1.Enabled = false;
            uSelectResult1.Clear();
        }
    }
}
