using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace Defect.SG.ColorPicker
{
    public class ColorEditingControl : Button, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        Color val=Color.Yellow;
        public Color Value
        {
            get
            {
                return (val);
            }
            set
            {
                val = value;
                BackColor = val;
            }
        }
        public ColorEditingControl():base()
        {
            Click += new EventHandler(ColorEditingControl_Click);
            Resize += new EventHandler(ColorEditingControl_Resize);
        }

        void ColorEditingControl_Resize(object sender, EventArgs e)
        {
            Width = Height;
        }
        void ColorEditingControl_Click(object sender, EventArgs e)
        {
            ColorDialog diag = new ColorDialog();
            diag.Color = Value;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                if (Value != diag.Color)
                {
                    valueChanged = true;
                    EditingControlDataGridView.NotifyCurrentCellDirty(true);
                    Value = diag.Color;
                }
            }
        }
        public object EditingControlFormattedValue
        {
            get 
            {
                return Value;
            }
            set
            {
                if (value != null)
                { 
                    Value = (Color)value;
                }
            }
        }
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        { return EditingControlFormattedValue; }
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        { this.Font = dataGridViewCellStyle.Font; }
        public int EditingControlRowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }
        public void PrepareEditingControlForEdit(bool selectAll) { }
        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }
        public DataGridView EditingControlDataGridView
        {
            get { return dataGridView; }
            set { dataGridView = value; }
        }
        public bool EditingControlValueChanged
        {
            get { return valueChanged; }
            set { valueChanged = value; }
        }
        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }
    }
}
