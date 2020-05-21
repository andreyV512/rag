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
using Protocol;
using UPAR;

namespace Defect.Def
{
    public partial class USensor : UserControl
    {
        ResultDef result = null;
        int iz = -1;
        int iis = -1;
        int space = 3;

        public delegate void DOnCalibrate(int _sensor);
        public delegate void DOnCursor(bool _on);
        public delegate void DOnGain();

        public event DOnCalibrate OnCalibrate;
        public event UCalibr.DOnStep OnStep;
        public event DOnCursor OnCursor;
        public event DOnGain OnGain;

        public double Step { get { return (uCalibr1.Step); } set { uCalibr1.Step = value; } }
        public bool IsCalibr { get { return (uCalibr1.IsCalibr); } }
        public bool CalibrVisible { get { return (uCalibr1.Visible); } set { uCalibr1.Visible = value; } }
        public double Gain { get { return (uCalibr1.Gain); } set { uCalibr1.Gain = value; } }

        public USensor()
        {
            InitializeComponent();
            uSensorData1.OnCursorMove = OnCursorMove;
            label2.Text = null;
        }
        public bool IsWheel { get { return (uSensorData1.IsWheel); } set { uSensorData1.IsWheel = value; } }
        public void Init(ResultDef _Result, int _iz, int _is)
        {
            result = _Result;
            iz = _iz;
            iis = _is;
            label1.Text = string.Format("Д{0}", iis + 1);
            label1.BackColor = Classer.GetColor(result.MZone[iz].MSensor[iis].Class);
            uSensorData1.InitSingle(_Result, _iz, _is, uCalibr1.Gain);
        }
        public void Clear()
        {
            uSensorData1.Clear();
            label1.Text = null;
            uCalibr1.Gain = 0;
        }
        public void OnCursorMove(PointSubj _pointSubj, double _y, Color _color)
        {
            if (_pointSubj == null)
            {
                label2.Text = null;
                return;
            }
            label2.Text = string.Format("C:{0}, У:{1}", _pointSubj.iM.ToString(), _y.ToString("0.###"));
            label2.BackColor = _color;
            if (OnCursor != null)
                OnCursor(_pointSubj != null);
        }
        public bool CanFocused { get { return (uSensorData1.CanFocused); } set { uSensorData1.CanFocused = value; } }
        
        void panel1_Resize(object sender, EventArgs e)
        {
            uCalibr1.Left = panel1.ClientSize.Width - uCalibr1.Width - space;
        }

        void pr(string _msg) { ProtocolST.pr(_msg); }

        void uCalibr1_OnGain()
        {
            result.MZone[iz].MSensor[iis].CalcClassGain(uCalibr1.Gain);
            label1.BackColor = Classer.GetColor(result.MZone[iz].MSensor[iis].Class);
            uSensorData1.InitSingle(result, iz, iis, uCalibr1.Gain);
            if (OnGain != null)
                OnGain();
        }

        void uCalibr1_OnCalibrate()
        {
            if (OnCalibrate != null)
                OnCalibrate(iis);
        }

        private void uCalibr1_OnStep(double _step)
        {
            if (OnStep != null)
                OnStep(_step);
        }
        public void WheelCursor(int _delta)
        {
            uSensorData1.WheelCursor(_delta);
        }
    }
}
