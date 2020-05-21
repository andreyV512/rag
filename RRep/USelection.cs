using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RRepPars;

namespace RRep
{
    public partial class USelection : UserControl
    {
        public USelection()
        {
            InitializeComponent();
            OnExec = null;
        }
        SelectionPars selection = null;
        public SelectionPars Selection
        {
            set
            {
                selection = value;
                if(selection==null)
                    return;
                propertyGrid1.SelectedObject = selection;                
//                DTPF0.Value = selection.DT0 > DateTimePicker.MinimumDateTime && selection.DT0 < DateTimePicker.MaximumDateTime ? selection.DT0 : DateTimePicker.MinimumDateTime;
//                DTPF1.Value = selection.DT1 > DateTimePicker.MinimumDateTime && selection.DT1 < DateTimePicker.MaximumDateTime ? selection.DT1 : DateTimePicker.MinimumDateTime;
            }
        }
        public delegate void DOnExec();
        public DOnExec OnExec { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (selection == null)
                return;
            if (OnExec == null)
                return;
//            selection.DT0=DTPF0.Value;
//            selection.DT1=DTPF1.Value;
            OnExec();
        }

    }
}
