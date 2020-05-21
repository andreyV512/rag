using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Signals
{
    public partial class UCSignal : UserControl
    {
        protected Signal signal=null;
        protected Color color_false;
        public bool alive = false;
        public UCSignal()
        {
            InitializeComponent();
        }
        public UCSignal(Signal _signal)
        {
            InitializeComponent();
            signal = _signal;
        }
        bool clicked = false;
        Point MouseDownLocation;
        private void UCSignal_Paint(object sender, PaintEventArgs e)
        {
            if (signal==null)
                return;
            if (signal.input)
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Etched);
            else
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Raised);
        }

        private void UCSignal_MouseDown(object sender, MouseEventArgs e)
        {
            if (clicked)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                clicked = true;
                MouseDownLocation = e.Location;
                this.BringToFront();
            }
        }

        private void UCSignal_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
        }

        private void UCSignal_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
                Point p = new Point();
                p.X = e.X + Left - MouseDownLocation.X;
                p.Y = e.Y + Top - MouseDownLocation.Y;
                Left = p.X;
                Top = p.Y;
                if (Left < 0)
                    Left = 0;
                if (Top < 0)
                    Top = 0;
            }
        }
        public void PanelSave()
        {
            signal.X = Left;
            signal.Y = Top;
        }
        public void PanelLoad()
        {
            Left = signal.X;
            Top = signal.Y;
        }
    }
}
