using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Share;
using ResultLib;
using ResultLib.Def;
using UPAR;
using UPAR.Def;

namespace Defect.Def
{
    public partial class FData : FBase, IView
    {
        public string SaveName = "FData";
        string title;
        int range;
        int zone_size = 0;

        CursorBorder cursorBorder;

        EUnit Tp;
        public FData(EUnit _Tp, string _title, int _range, CursorBorder _cursorBorder)
        {
            InitializeComponent();
            Tp = _Tp;
            label2.Text = null;
            title = _title;
            range = _range;
            cursorBorder = _cursorBorder;
            OnHide = ExecHide;
        }
        public void RDraw()
        {
            if (RK.ST.cDef(Tp).Zone == null || RK.ST.cDef(Tp).Sensor == null)
            {
                Clear();
                return;
            }
            if (range < 0)
                range = 0;
            uSensorData1.OnMouseMoveR = OnMouseMoveR;
            uSensorData1.OnCursorMove = OnCursorMove;
            Init();
        }
        new void Init()
        {
            int iz0;
            int iz1;
            if (range == 0)
            {
                iz0 = RK.ST.cDef(Tp).Zone.Value;
                iz1 = RK.ST.cDef(Tp).Zone.Value;
                Text = string.Format("{0}: Зона: {1}, Датчик: {2}", title, iz0 + 1, RK.ST.cDef(Tp).Sensor.Value + 1);
            }
            else
            {
                iz0 = RK.ST.cDef(Tp).Zone.Value - range / 2;
                iz1 = iz0 + range - 1;
                if (iz0 < 0)
                    iz0 = 0;
                if (iz1 >= RK.ST.cDef(Tp).result.MZone.Count)
                    iz1 = RK.ST.cDef(Tp).result.MZone.Count - 1;
                Text = string.Format("{0}: Зоны с {1} по {2}, Датчик: {3}", title, iz0 + 1, iz1 + 1, RK.ST.cDef(Tp).Sensor.Value + 1);
            }
            DefCL dcl = new DefCL(Tp);
            uSensorData1.InitRange(RK.ST.cDef(Tp).result, iz0, iz1, RK.ST.cDef(Tp).Sensor.Value,
                 dcl.LCh[RK.ST.cDef(Tp).Sensor.Value].Gain);
            zone_size = ParAll.ST.ZoneSize;
            if (range == 0)
                cursorBorder.DrawSingle(iz0, RK.ST.cDef(Tp).Sensor.Value);
            else
                cursorBorder.DrawLine(iz0, iz1, RK.ST.cDef(Tp).Sensor.Value);
            base.Init();
        }
        public void Clear()
        {
            uSensorData1.Clear();
            Text = title;
            label1.Text = null;
            label2.Text = null;
            if (range == 0)
                cursorBorder.ClearSingle();
            else
                cursorBorder.ClearLine();
        }
        public void OnMouseMoveR(int? _x, int? _y)
        {
            if (_x != null)
                label1.Text = string.Format("[{0},{1}]", _x.ToString(), _y.ToString());
            else
                label1.Text = null;
        }
        public void OnCursorMove(PointSubj _pointSubj, double _y, Color _color)
        {
            if (_pointSubj == null)
            {
                label2.Text = null;
                return;
            }
            double s = RK.ST.cDef(Tp).result.MMByMeas(_pointSubj.iZ.Value, _pointSubj.iS.Value, _pointSubj.iM.Value);
            if (range != 0)
                label2.Text = string.Format("З:{0} C:{1}, У:{2}%, P:{3}мм",
                    _pointSubj.iZ + 1,
                    _pointSubj.iM.ToString(),
                    _y.ToString("0.###"),
                    s.ToString("0.#"));
            else
                label2.Text = string.Format("C:{0}, У:{1}%, P:{2}мм",
                    _pointSubj.iM.ToString(),
                    _y.ToString("0.###"),
                    s.ToString("0.#"));
            label2.BackColor = _color;
        }

        private void FData_Activated(object sender, EventArgs e)
        {
            uSensorData1.CanFocused = true;
        }

        private void FData_Deactivate(object sender, EventArgs e)
        {
            uSensorData1.CanFocused = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    RK.ST.cDef(Tp).Zone -= 1;
                    Init();
                    break;
                case Keys.Right:
                    RK.ST.cDef(Tp).Zone += 1;
                    Init();
                    break;
                case Keys.Up:
                    RK.ST.cDef(Tp).Sensor += 1;
                    Init();
                    break;
                case Keys.Down:
                    RK.ST.cDef(Tp).Sensor -= 1;
                    Init();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void ExecHide()
        {
            if (range == 0)
                cursorBorder.ClearSingle();
            else
                cursorBorder.ClearLine();
        }
    }
}
