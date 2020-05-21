using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Share
{
    public partial class UDBTube : UserControl
    {
        public UDBTube()
        {
            InitializeComponent();
            Clear();
        }
        public string TypeSize { get { return (lTypeSize.Text); } set { lTypeSize.Text = value; } }
        public void Clear()
        {
            TypeSize = null;
        }
    }
}

