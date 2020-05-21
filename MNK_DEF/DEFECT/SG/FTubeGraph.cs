using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using PARLIB;
using Protocol;
using UPAR;
using UPAR.SG;

namespace Defect.SG
{
    public partial class FTubeGraph : Form
    {
        MIU miu;
        SGPars.EValIU ValIU;
        string DescValIU;
        DataPointCollection pP;
        string TSName;
        public string Ok = "Ok";
        SGHalfPeriod[] Msghp = null;

        public FTubeGraph(GraphObject _O, SOPPars _sopp)
        {
            InitializeComponent();
            TSName = _O.TypeSizeName;
            miu = _O.GetMIU();
            ValIU = ParAll.SG.sgPars.ValIU;
            DescValIU = new PARLIB.EnumTypeConverter(typeof(SGPars.EValIU)).Desc(ValIU);
            if (!miu.Ok)
            {
                Ok = "Данных нет";
                return;
            }
            Msghp = _O.GetHalfPeriods(_sopp);
        }
        int zone = 2000;
        private void FTubeGraph_Load(object sender, EventArgs e)
        {
            L_WindowLPars.CurrentWins.LoadFormRect(this);
            DataPointCollection pI = chart1.Series[0].Points;
            DataPointCollection pU = chart1.Series[1].Points;
            chart1.Series[1].Color = ValIU == SGPars.EValIU.U ? Color.Red : Color.Green;
            chart1.Series[1].LegendText = DescValIU;
            pP = chart1.Series[2].Points;

            for (int i = 0; i < miu.Length; i++)
            {
                pI.AddXY(i, miu[i].I);
                pU.AddXY(i, miu[i].Val(ValIU));
            }
            MouseWheelHandler.Add(chart1, MyOnMouseWheel);
            ucTrack1.SetCount(miu.Length, zone);
            ucTrack1.ROnClick = TrackClick;
            int[] tresh = Tresh.Vals(TSName);
            for (int i = 0; i < Msghp.Length; i++)
            {
                SGHalfPeriod sghp = Msghp[i];
                for (int j = 0; j < tresh.Length; j++)
                {
                    if (tresh[j] >= sghp.size)
                        continue;
                    int pos = sghp.start + tresh[j];
                    int p = pP.AddXY(pos, miu[pos].Val(ValIU));
                    pP[p].Color = ValIU == SGPars.EValIU.U ? Color.Red : Color.Green;
                }
            }
            Axis ax = chart1.ChartAreas[0].AxisX;
            ax.Maximum = ax.Minimum + zone;
        }
        void TrackClick(int _pos)
        {
            Axis ax = chart1.ChartAreas[0].AxisX;
            ax.Minimum = _pos;
            ax.Maximum = ax.Minimum + zone;
        }
        void MyOnMouseWheel(MouseEventArgs e)
        {
            Axis ax = chart1.ChartAreas[0].AxisX;
            double xValue = ax.PixelPositionToValue(e.X);
            if (xValue < ax.Minimum || xValue >= ax.Maximum)
                return;
            double delta0;
            double delta1;
            if (e.Delta > 0)
            {
                delta0 = (xValue - ax.Minimum) * 1.1;
                delta1 = (ax.Maximum - xValue) * 1.1;
            }
            else
            {
                delta0 = (xValue - ax.Minimum) * 0.9;
                delta1 = (ax.Maximum - xValue) * 0.9;
            }
            ax.Minimum = (int)(xValue - delta0);
            ax.Maximum = (int)(xValue + delta1);
            zone = (int)(ax.Maximum - ax.Minimum);
            if (zone < 50)
                zone = 50;
            if (zone > 50000)
                zone = 50000;
            ax.Maximum = ax.Minimum + zone;
            ucTrack1.SetCount(miu.Length, zone);
        }

        private void FTubeGraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }
        int space = 4;
        private void FTubeGraph_Resize(object sender, EventArgs e)
        {
            ucTrack1.Left = space;
            ucTrack1.Width = ClientSize.Width - space * 2;
            ucTrack1.Top = statusStrip1.Top - ucTrack1.Height - space;
            chart1.Left = space;
            chart1.Width = ClientSize.Width - space * 2;
            chart1.Top = space;
            chart1.Height = ucTrack1.Top - space * 2;
        }

        void prs(string _msg)
        {
            toolStripStatusLabel1.Text = _msg;
        }
        double SelectionSize = 0;
        bool mouse_click_block = false;
        private void chart1_SelectionRangeChanging(object sender, CursorEventArgs e)
        {
            double SelectionSize_new = e.NewSelectionEnd - e.NewSelectionStart;
            if (SelectionSize_new != 0 || SelectionSize != 0 && SelectionSize_new == 0)
                mouse_click_block = true;
            SelectionSize = SelectionSize_new;
            pr("SelectionRangeChanging: " + SelectionSize.ToString());
            if (SelectionSize == 0)
                prs("");
            else
            {
                string msg = string.Format("Смещение: {0}", ((int)SelectionSize).ToString());
                int pos = (int)e.NewSelectionEnd;
                if (pos >= 0 && pos < miu.Length)
                {
                    msg += string.Format(" I={0} {1}={2}",
                        miu[pos].I.ToString(),
                        DescValIU,
                        miu[pos].Val(ValIU).ToString()
                        );
                }
                prs(msg);
            }
        }
        //        int PointIndex = -1;
        void pr(string _msg)
        {
            ProtocolST.pr("FTubeGraph: " + _msg);
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            pr("MouseClick");
            if (mouse_click_block)
            {
                mouse_click_block = false;
                return;
            }
            if (e.Button != MouseButtons.Left)
                return;
            Axis ax = chart1.ChartAreas[0].AxisX;
            int pos = (int)(ax.PixelPositionToValue(e.X));
            if (pos < 0)
                return;
            if (pos >= miu.Length)
                return;
            SGHalfPeriod sghp = null;
            for (int i = 0; i < Msghp.Length - 1; i++)
            {
                if (pos >= Msghp[i].start && pos < Msghp[i + 1].start)
                {
                    sghp = Msghp[i];
                    break;
                }
            }
            if (sghp == null)
                return;
            IU[] liu = new IU[sghp.size];
            for (int i = 0; i < sghp.size; i++)
                liu[i] = new IU(miu[sghp.start + i]);
            using (FTubeHalfPeriod f = new FTubeHalfPeriod(TSName, liu, sghp))
            {
                f.ShowDialog();
                if (f.IsChange)
                {
                    if (f.Ok == "Ok")
                    {
                        IsChandge = true;
                        Close();
                    }
                    else
                        prs("Не смогли записать пороги");
                }
            }
        }
        public bool IsChandge = false;

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectionSize != 0)
                return;
            HitTestResult htr=chart1.HitTest(e.X,e.Y);
            if (htr.Object == null)
                return;
            DataPoint dp = htr.Object as DataPoint;
            if (dp == null)
                prs(null);
            else
                prs("[" + dp.XValue.ToString()+" "+dp.YValues[0].ToString()+"]");
        }
    }
}
