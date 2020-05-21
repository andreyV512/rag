using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RAGLib
{
    public partial class USplitter : UserControl
    {
        public USplitter()
        {
            InitializeComponent();
        }

        private void USplitter_Paint(object sender, PaintEventArgs e)
        {
            //            ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Raised);
        }
        UBase Up = null;
        UBase Down = null;
        public void Init(UBase _Up, UBase _Down)
        {
            Up = _Up;
            Down = _Down;
        }
        bool clicked = false;
        int YLocation;
        private void USplitter_MouseDown(object sender, MouseEventArgs e)
        {
            if (clicked)
                return;
            clicked = true;
            YLocation = e.Location.Y;
        }

        private void USplitter_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
                ExecMove(e.Y + Top - YLocation);
        }
        void ExecConnect(int _Y)
        {
            int UpHeight = _Y - Up.Top;
            if (UpHeight < Up.MinHeight)
            {
                UpHeight = Up.MinHeight;
                _Y = Up.Top + Up.MinHeight;
            }
            int DownHeight = Down.Top + Down.Height - _Y - Height;
            if (DownHeight < Down.MinHeight)
                DownHeight = Down.MinHeight;
            Top = _Y;
            Up.Height = UpHeight;
            Down.Top = Top + Height;
            Down.Height = DownHeight;
        }
        void ExecMove(int _Y)
        {
            int UpHeight = _Y - Up.Top;
            if (UpHeight < Up.MinHeight)
                return;
            int DownHeight = Down.Top + Down.Height - _Y - Height;
            if (DownHeight < Down.MinHeight)
                return;
            Top = _Y;
            Up.Height = UpHeight;
            Down.Top = Top + Height;
            Down.Height = DownHeight;
        }

        private void USplitter_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
        }
        public void RLoad(int _par)
        {
            ExecConnect(_par);
        }
    }
}
