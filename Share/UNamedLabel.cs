using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Share
{
    public partial class UNamedLabel : UserControl
    {
        public UNamedLabel()
        {
            InitializeComponent();
            Clear();
        }
        public string Title { get { return (lTitle.Text); } set { lTitle.Text = value; RResize(); } }
        public string Value { get { return (lValue.Text); } set { lValue.Text = value; RResize(); } }
        public void Clear()
        {
            Value = null;
        }
        int space = 2;
        public bool ReSizable { get; set; }
        void RResize()
        {
            if (!ReSizable)
                return;
            lValue.Left = lTitle.Left + lTitle.Width + space;
            Width = lValue.Left + lValue.Width + space;
        }
        public void ResizeByTitle()
        {
            lValue.Left = lTitle.Left + lTitle.Width + space;
            Width = lValue.Left + lValue.Width + space;
        }
        public int ValueLeft { get { return (lValue.Left); } set { lValue.Left = value; RResize(); } }
        public int ValueWidth { get { return (lValue.Width); } set { lValue.Width = value; RResize(); } }
    }
}

