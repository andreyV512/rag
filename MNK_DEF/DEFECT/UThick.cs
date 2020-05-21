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
using ResultLib.Thick;
using UPAR;
using RAGLib;

namespace Defect
{
    public partial class UThick : UBase, IView
    {
        DataPointCollection mp;

        public UThick()
        {
            InitializeComponent();
            mp = chart1.Series["Series1"].Points;
        }
        public void Init()
        {
            LoadSettings();
        }
        public void RDraw()
        {
            Clear();
            if (RK.ST.result == null)
                return;
            ResultThickLite rs = RK.ST.result.Thick;
            if (rs == null)
                return;
            if (rs.MZone == null)
                return;
            double? level = null;
            for (int i = 0; i < rs.MZone.Count; i++)
            {
                BankLib.BankZoneThick z = rs.MZone[i];
                double lLevel = z.Level == null ? rs.MaxThickness : z.Level.Value;
                mp.Add(lLevel).Color = Classer.GetColor(z.RClass);
                if (z.Level != null)
                {
                    if (level == null)
                        level = z.Level.Value;
                    else
                    {
                        if (z.Level.Value < level)
                            level = z.Level.Value;
                    }
                }
            }
            uMinThick1.MinThick = level;
        }
        public void Clear()
        {
            mp.Clear();
        }

        public void LoadSettings()
        {
            CBIsWork.Checked = ParAll.ST.Defect.Thick.IsWork;
            chart1.ChartAreas[0].AxisX.Maximum = (double)ParAll.ST.MaxZones + 0.5;
        }

        private void CBIsWork_CheckedChanged(object sender, EventArgs e)
        {
            ParAll.ST.Defect.Thick.IsWork = (sender as CheckBox).Checked;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            uMinThick1.Left = ClientSize.Width - 2 - uMinThick1.Width;
        }
    }
}
