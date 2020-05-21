using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using PARLIB;
using Protocol;

using UPAR;
using UPAR.SG;

namespace Defect.SG
{
    public partial class FTubeHalfPeriod : Form
    {
        IU[] iu;
        SGPars.EValIU ValIU;
        string DescValIU;
        DataPointCollection pP;
        Axis ax;
        Axis ay;
        string TSName;
        bool need_save = false;
        public string SaveName = "FTubeHalfPeriod";
        public string Ok = "Ok";
        public bool IsChange=false;
        public FTubeHalfPeriod(string _TSName, IU[] _iu,SGHalfPeriod _sghp)
        {
            InitializeComponent();
            iu = _iu;
            ValIU = ParAll.SG.sgPars.ValIU;
            TSName = _TSName;
            DescValIU = new PARLIB.EnumTypeConverter(typeof(SGPars.EValIU)).Desc(ValIU);
            Text = _sghp.ToString();
        }

        private void FTubeHalfPeriod_Load(object sender, EventArgs e)
        {
            L_WindowLPars.CurrentWins.LoadFormRect(this);
            pr("Load " + Left.ToString() + " " + Top.ToString());
//            chart1.Series["SerPoint"].MarkerImage = Path.GetDirectoryName(Application.ExecutablePath) + "\\SGPoint.bmp";
            DataPointCollection pI = chart1.Series["SerI"].Points;
            DataPointCollection pU = chart1.Series["SerVal"].Points;
            chart1.Series["SerVal"].Color = ValIU == SGPars.EValIU.U ? Color.Red : Color.Green;
            chart1.Series["SerVal"].LegendText = DescValIU;
            pP = chart1.Series["SerPoint"].Points;
            for (int i = 0; i < iu.Length; i++)
            {
                pI.AddXY(i, iu[i].I);
                pU.AddXY(i, iu[i].Val(ValIU));
            }
            //            MouseWheelHandler.Add(chart1, MyOnMouseWheel);
            List<int> tresh = Tresh.ValsL(TSName);
            for (int i = 0; i < tresh.Count; i++)
            {
                if (tresh[i] >= iu.Length)
                    continue;
                int p = pP.AddXY(tresh[i], iu[tresh[i]].Val(ValIU));
                pP[p].Color = ValIU == SGPars.EValIU.U ? Color.Red : Color.Green;
                //p = pP.AddXY(points[i].gpos, 0);
                //pP[p].Color = Color.Black;
                //pP[p].Tag = i;
            }
            ax = chart1.ChartAreas[0].AxisX;
            ax.Minimum = -10;
            ax.Maximum = iu.Length + 10;
            ay = chart1.ChartAreas[0].AxisY;
        }

        private void FTubeHalfPeriod_FormClosed(object sender, FormClosedEventArgs e)
        {
            L_WindowLPars.CurrentWins.SaveFormRect(this);
            if (!need_save)
                return;
            using (FMessage M = new FMessage("Сохранить изменения"))
            {
                M.ShowDialog();
                need_save = M.Yes;
                if (!need_save)
                    return;
            }
            int[] tresh = new int[pP.Count];
            for(int i=0;i<tresh.Length;i++)
                tresh[i]=(int)pP[i].XValue;
            bool ret = Tresh.Save(TSName, tresh);
            if (!ret)
                Ok = "Не смогли записать пороги";
            IsChange = true;
        }
        void pr(string _msg)
        {
            ProtocolST.pr("FTubeHalfPeriod: " + _msg);
        }

        DataPoint dp = null;
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            HitTestResult ht = chart1.HitTest(e.X, e.Y);
            if (ht.Series != chart1.Series[2])
            {
                dp = null;
                return;
            }
            dp = pP[ht.PointIndex];
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (dp == null)
                return;
            dp.Label = null;
            dp = null;
        }

        DataPoint dp_labeled = null;
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult ht = chart1.HitTest(e.X, e.Y);
            if (ht.Series == chart1.Series[2])
            {
                if (dp_labeled != null)
                        dp_labeled.Label = null;
                dp_labeled = ht.Object as DataPoint;
                int ltresh0 = (int)dp_labeled.XValue;
                double per0 = ltresh0;
                per0 /= ParAll.SG.sgPars.HalfPeriod;
                per0 *= 100;
                dp_labeled.Label = ((int)per0).ToString() + "% " + iu[ltresh0].Val(ValIU).ToString() + " " + ltresh0.ToString();
            }
            else
            {
                if (dp_labeled != null)
                {
                    dp_labeled.Label = null;
                    dp_labeled = null;
                }
            }

            if (dp == null)
                return;
            int ltresh = (int)(ax.PixelPositionToValue(e.X));
            if (ltresh < 0)
                ltresh = 0;
            if (ltresh >= iu.Length)
                ltresh = iu.Length - 1;
            dp.XValue = ltresh;
            need_save = true;
            dp.YValues[0] = iu[ltresh].Val(ValIU);
            double per = ltresh;
            per /= ParAll.SG.sgPars.HalfPeriod;
            per *= 100;
            dp.Label = ((int)per).ToString() + " % " + iu[ltresh].Val(ValIU).ToString() + " " + ltresh.ToString();
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            if (dp != null)
                return;
            HitTestResult ht = chart1.HitTest(e.X, e.Y);
            if (ht.Series == chart1.Series[2])
            {
                pP.RemoveAt(ht.PointIndex);
                need_save = true;
            }
            else
            {
                int ltresh = (int)(ax.PixelPositionToValue(e.X));
                if (ltresh < 0)
                    return;
                if (ltresh >= iu.Length)
                    return;
                double lval = ay.PixelPositionToValue(e.Y);
                double lval1 = iu[ltresh].Val(ValIU);
                double delta = lval - lval1;
                if (delta < 0)
                    delta = -delta;
                if (delta <= 2)
                {
                    int p = pP.AddXY(ltresh, iu[ltresh].Val(ValIU));
                    pP[p].Color = ValIU == SGPars.EValIU.U ? Color.Red : Color.Green;
                    need_save = true;
                }
            }
        }
    }
}
