using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;


using UPAR;
using UPAR.TS.TSDef;
using Defect.Work;
using UPAR.SG;

using Demagnetizer;

namespace Defect.SG
{
    public partial class uGSPF : UserControl
    {
        SGSet sgSet = null;
        DataPointCollection mpI;
        DataPointCollection mpU;
        DataPointCollection mpB;
        DataPointCollection mpD0;
        DataPointCollection mpD1;
        DataPointCollection mpD2;
        SignalListDef SL = null;
        JDemagnetizer jDemagnetizer = null;

        public uGSPF()
        {
            InitializeComponent();
            timer1.Interval = 100;
            mpI = chart1.Series[0].Points;
            mpU = chart1.Series[1].Points;
            mpB = chart1.Series[2].Points;
            mpD0 = chart1.Series[3].Points;
            mpD1 = chart1.Series[4].Points;
            mpD2 = chart1.Series[5].Points;
        }
        public void Init(SignalListDef _SL, JDemagnetizer _jDemagnetizer)
        {
            SL = _SL;
            label2.Text = null;
            jDemagnetizer = _jDemagnetizer;
        }
        public new void Dispose()
        {
            if (DesignMode)
                return;
            Reset();
            base.Dispose();
        }
        public string Title { get { return (label1.Text); } set { label1.Text = value; } }
        void prstat(string _msg)
        {
            toolStripStatusLabel1.Text = _msg;
        }
        void Clear()
        {
            mpI.Clear();
            mpU.Clear();
            mpB.Clear();
            mpD0.Clear();
            mpD1.Clear();
            mpD2.Clear();
        }
        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Clear();
                sgSet = new SGSet(SL, pr);
                if (checkBox2.Checked)
                    jDemagnetizer.Start(0);
                if (checkBox3.Checked)
                {
                    string ret = sgSet.StartGSPF();
                    if (ret != null)
                    {
                        prstat(ret);
                        Reset();
                        checkBox1.Checked = false;
                        checkBox1.Text = checkBox1.Checked ? "Выключить" : "Включить";
                        return;
                    }
                }
                label2.Text = sgSet.CheckDTypeSize()?null:"Датчики не соответствуют типоразмеру";
                    
                SolidGroupPars sgpars = ParAll.SG;
                L502Ch[] MCh = new L502Ch[6]
                {
                    sgpars.Sensor_I,
                    sgpars.Sensor_B,
                    sgpars.Sensor_U,
                    sgpars.Sensor_D0,
                    sgpars.Sensor_D1,
                    sgpars.Sensor_D2
                };
                sgSet.StartL502(MCh);
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                timer1.Enabled = true;
            }
            else
            {
                pr("3 " + (checkBox1.Checked ? "true" : "false"));
                timer1.Enabled = false;
                Reset();
            }
            checkBox1.Text = checkBox1.Checked ? "Выключить" : "Включить";
        }
        void Reset()
        {
            if(sgSet!=null)
                sgSet.Dispose();
            jDemagnetizer.Finish();
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            double[] buf = sgSet.ReadL502();
            prstat("buf=" + (buf == null ? "null" : buf.Length.ToString()));
            chart1.SuspendLayout();
            chart1.Series.SuspendUpdates();
            chart1.ChartAreas.SuspendUpdates();
            Clear();
            int packets = buf.Length / 6;
            for (int p = 0; p < packets; p++)
            {
                mpI.AddY(buf[p * 6]);
                mpU.AddY(buf[p * 6 + 1]);
                mpB.AddY(buf[p * 6 + 2]);
                mpD0.AddY(buf[p * 6 + 3]);
                mpD1.AddY(buf[p * 6 + 4]);
                mpD2.AddY(buf[p * 6 + 5]);
            }
            chart1.Series.ResumeUpdates();
            chart1.ChartAreas.ResumeUpdates();
            chart1.ResumeLayout();
        }

        private void uGSPF_Resize(object sender, EventArgs e)
        {
            chart1.Width = ClientSize.Width - chart1.Left * 2;
            chart1.Height = statusStrip1.Top - chart1.Top;
        }
        void pr(string _Msg)
        {
            Protocol.ProtocolST.pr("uGSPF" + _Msg);
        }
    }
}
