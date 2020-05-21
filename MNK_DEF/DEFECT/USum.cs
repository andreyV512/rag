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

namespace Defect
{
    public partial class USum : UBase
    {
        DataPointCollection mp;
        public USum()
        {
            InitializeComponent();
            mp = chart1.Series["Series1"].Points;
        }
        public void Init()
        {
            LoadSettings();
        }
        internal void Draw(SumResult _sumResult)
        {
            Clear();
            for (int i = 0; i < _sumResult.MClass.Count(); i++)
                mp.Add(1).Color = Classer.GetColor(_sumResult.MClass[i]);

        }
        public void Clear()
        {
            mp.Clear();
        }
        public void LoadSettings()
        {
            chart1.ChartAreas[0].AxisX.Maximum = ParAll.ST.MaxZones + 0.5;
        }
    }
}
