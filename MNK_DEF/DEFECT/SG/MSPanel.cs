using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Defect.SG
{
    class MSPanel : Panel
    {
        int index;
        public static int size = 7;
        static int half = 3;
        Cursor cursor = Cursors.Default;
        bool IsResize = false;

        public MSPanel(int _index)
        {
            index = _index;
            Width = size;
            Height = size;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Visible = false;
            MouseEnter += new EventHandler(MSPanel_MouseEnter);
            MouseLeave += new EventHandler(MSPanel_MouseLeave);
            MouseDown += new MouseEventHandler(MSPanel_MouseDown);
            MouseUp += new MouseEventHandler(MSPanel_MouseUp);
            MouseMove += new MouseEventHandler(MSPanel_MouseMove);
            switch (index)
            {
                case 0:
                    cursor = Cursors.SizeAll;
                    break;
                case 1:
                    cursor = Cursors.SizeWE;
                    break;
                case 2:
                    cursor = Cursors.SizeNWSE;
                    break;
                case 3:
                    cursor = Cursors.SizeNS;
                    break;
            }

        }
        public MSPanel(int _index, Size _ParentClientSize, ROnResize _onResize)
            : this(_index)
        {
            ParentClientSize = _ParentClientSize;
            onResize = _onResize;
        }

        void MSPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                IsResize = false;
                return;
            }
            IsResize = true;
        }
        void MSPanel_MouseUp(object sender, MouseEventArgs e)
        {
            IsResize = false;
        }
        public delegate void ROnResize(bool _move, Point _deltap);
        public ROnResize onResize = null;
        void MSPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsResize)
                return;
            if (onResize == null)
                return;
            Point deltap = new Point();
            deltap.X = e.X;
            deltap.Y = e.Y;
            switch (index)
            {
                case 1:
                    deltap.Y = 0;
                    break;
                case 3:
                    deltap.X = 0;
                    break;
            }
            if (deltap.X == 0 && deltap.Y == 0)
                return;
            onResize(index == 0, deltap);
        }


        void MSPanel_MouseEnter(object sender, EventArgs e)
        {
            Cursor = cursor;
        }
        void MSPanel_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
        public Size ParentClientSize
        {
            set
            {
                switch (index)
                {
                    case 0:
                        Left = 0;
                        Top = 0;
                        break;
                    case 1:
                        Left = value.Width - size;
                        Top = value.Height / 2 - half;
                        break;
                    case 2:
                        Left = value.Width - size;
                        Top = value.Height - size;
                        break;
                    case 3:
                        Left = value.Width / 2 - half;
                        Top = value.Height - size;
                        break;
                }
            }
        }
    }
}
