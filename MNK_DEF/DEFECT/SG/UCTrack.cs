using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defect.SG
{
    public delegate void UCTrackOnClickDelegate(int _pos);
    public partial class UCTrack : UserControl
    {
        public UCTrackOnClickDelegate ROnClick = null;
        public UCTrack()
        {
            InitializeComponent();
            count = 1000;
            pos = 0;
            zone = 1000;
            RPaint();
            CheckPar();
        }
        public void SetCount(int _count,int _zone)
        {
            count=_count;
            zone=_zone;
            CheckPar();
            RPaint();
        }
        public void Delta(int _delta, int _ppos)
        {
            if (_ppos < pos || _ppos > pos + zone)
                return;
            double ddd = _ppos - pos;
            if (_delta > 0)
            {
                int zone1 = (int)((double)zone * 1.12);
                if (zone1 == zone)
                    zone++;
                else
                    zone = zone1;
                ddd *= 1.12;
            }
            else
            {
                int zone1 = (int)((double)zone * 0.88);
                if (zone1 == zone)
                    zone--;
                else
                    zone = zone1;
                ddd *= 0.88;
            }
            RSet(_ppos - (int)ddd);
        }
        public void Delta(int _delta)
        {
            Delta(_delta, pos + zone / 2);
        }
        private int count;
        private int zone;
        private int pos;
        private void UCTrack_Resize(object sender, EventArgs e)
        {
            panel1.Left = button2.Width;
            panel1.Width = button3.Left - panel1.Left;
            button1.Height = panel1.Height - 4;
            button1.Top = 0;
            RPaint();
        }
        private void RPaint()
        {
            if (count == 0)
            {
                button1.Width = 10;
                button1.Left = 0;
                return;
            }
            double d = zone;
            d *= panel1.Width;
            d /= count;
            button1.Width = (int)d;
            if (button1.Width < 10)
                button1.Width = 10;
            d = pos;
            d *= panel1.Width;
            d /= count;
            button1.Left = (int)d;
        }
        private void RSet(int _pos)
        {
            pos = _pos;
            CheckPar();
            RPaint();
            if (ROnClick != null)
                ROnClick(pos);
        }
        private void RMove(int _x)
        {
            double d = _x;
            d *= count;
            d /= panel1.Width;
            RSet((int)d);
        }
        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            RMove(button1.Left + e.X - button1.Width / 2);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            RMove(e.X - button1.Width / 2);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RSet(pos - zone);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RSet(pos + zone);
        }
        private void CheckPar()
        {
            if (pos < 0)
                pos = 0;
            else if (pos > count - zone)
                pos = count - zone;
        }
    }
}
