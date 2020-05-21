using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Share;
using ResultLib;

namespace Defect
{
    public partial class USelectResult : UserControl
    {
        public delegate void DOnClass(EClass _rClass);
        EClass rClass = EClass.Brak;
        bool inited = false;

        public USelectResult()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
        public void Init()
        {
            inited = true;
            RClass = EClass.None;
        }
        int borderWidth = 2;
        private void USelectResult_Paint(object sender, PaintEventArgs e)
        {
            if (Enabled)
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                        ControlPaint.Light(BackColor), borderWidth, ButtonBorderStyle.Outset,
                        ControlPaint.Light(BackColor), borderWidth, ButtonBorderStyle.Outset,
                        ControlPaint.Dark(BackColor), borderWidth, ButtonBorderStyle.Inset,
                        ControlPaint.Dark(BackColor), borderWidth, ButtonBorderStyle.Inset);
            else
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                        ControlPaint.Dark(BackColor), borderWidth, ButtonBorderStyle.Outset,
                        ControlPaint.Dark(BackColor), borderWidth, ButtonBorderStyle.Outset,
                        ControlPaint.Light(BackColor), borderWidth, ButtonBorderStyle.Inset,
                        ControlPaint.Light(BackColor), borderWidth, ButtonBorderStyle.Inset);
        }
        public EClass RClass
        {
            get { return (rClass); }
            set
            {
                if (!inited)
                    return;
                rClass = value;
                BackColor = Classer.GetColor(rClass);
                label1.Text = Classer.ToStr(rClass);
                Invalidate();
            }
        }
        public event DOnClass OnClass = null;

        private void USelectResult_Resize(object sender, EventArgs e)
        {
            label1.Left = borderWidth;
            label1.Top = borderWidth;
            label1.Width = ClientSize.Width - borderWidth * 2;
            label1.Height = ClientSize.Height - borderWidth * 2;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (OnClass == null)
                return;
            switch (rClass)
            {
                case EClass.Class1:
                    rClass = EClass.Class2;
                    break;
                case EClass.Class2:
                    rClass = EClass.Brak;
                    break;
                case EClass.Brak:
                    rClass = EClass.Class1;
                    break;
                case EClass.Dead:
                case EClass.None:
                    break;
            }
            RClass = rClass;
            OnClass(rClass);
        }
        public void Clear()
        {
            RClass = EClass.None;
        }
    }
}
