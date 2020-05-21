using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using UPAR;
using Defect.GSPF052PCI;
using BankLib;
using Protocol;
using SQL;
using UPAR.SG;
using UPAR.TS.TSDef;
using Defect.Work;
using Share;

namespace Defect.SG
{
    public partial class SGWork : UserControl
    {

        Bank bank;
        JWorkSGSOP jWorkSGSOP = null;
        public delegate void DOnInsert(bool _IsGraph);
        DOnInsert OnInsert = null;
        SignalListDef SL;
        SGWorkPars sgWorkPars;
        DateTime dt0;
        public SGWork()
        {
            InitializeComponent();
        }
        public void Init(SignalListDef _SL, DOnInsert _OnInsert)
        {
            SL = _SL;
            OnInsert = _OnInsert;
            sgWorkPars = new SGWorkPars();
            propertyGrid1.SelectedObject = sgWorkPars;
            timer1.Interval = Convert.ToInt32(ParAll.ST.Defect.Some.Period);
            SGSet.SaveParsToDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sgWorkPars.Save();
        }

        private void cbWork_Click(object sender, EventArgs e)
        {
            cbWork.Text = cbWork.Checked ? "Стоп" : "Пуск";
            if (cbWork.Checked)
            {
                bank = new Bank(new cIW() { SG = true });
                jWorkSGSOP = new JWorkSGSOP(sgWorkPars, bank, SL);
                jWorkSGSOP.Start(Environment.TickCount);
                dt0 = DateTime.Now.AddSeconds(sgWorkPars.SOPPeriod);
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                Stop(true);
            }
        }
        void Stop(bool _Ok)
        {
            if (jWorkSGSOP != null)
            {
                jWorkSGSOP.Finish();
                jWorkSGSOP.Dispose();
                jWorkSGSOP = null;
            }
            if (_Ok && OnInsert != null)
                OnInsert(sgWorkPars.IsGraph);
            bank = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (jWorkSGSOP == null)
                return;
            jWorkSGSOP.Exec(Environment.TickCount);
            if (jWorkSGSOP.IsError)
            {
                prsl(1, jWorkSGSOP.LastError);
                jWorkSGSOP.Dispose();
                jWorkSGSOP = null;
                cbWork.Checked = false;
                cbWork.Text = cbWork.Checked ? "Стоп" : "Пуск";
                Stop(false);
                return;
            }
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt0 - dt;
            if (ts < new TimeSpan())
            {
                cbWork.Checked = false;
                cbWork.Text = cbWork.Checked ? "Стоп" : "Пуск";
                Stop(true);
                return;
            }
            prsl(0, Math.Round(ts.TotalSeconds).ToString() + " c, измерений: " + bank.GetCountOfUnit(Share.EUnit.SG).ToString());
            timer1.Enabled = true;
        }
        void pr(string _msg)
        {
            ProtocolST.pr("SGWork: " + _msg);
        }
        public void prsl(uint _level, string _msg)
        {
            if (_level == 0)
                toolStripStatusLabel1.Text = _msg;
            else
            {
                toolStripStatusLabel2.Text = _msg;
            }
            pr(_msg);
        }
    }
}
