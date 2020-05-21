using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Defect.SG.ColorPicker
{
    public class ColorCell : DataGridViewCell
    {
        protected override void OnClick(DataGridViewCellEventArgs e)
        {
             DataGridView.BeginEdit(false);
        }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            object o = DataGridView.EditingControl;
            ColorEditingControl ctl = DataGridView.EditingControl as ColorEditingControl;
            ctl.Value = (Color)initialFormattedValue;
        }
        public override Type EditType
        {
            get { return typeof(ColorEditingControl); }
        }
        public override Type ValueType
        {
            get { return typeof(Color); }
        }
        public override object DefaultNewRowValue
        {
            get { return Color.White; }
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (this.Selected)
            {
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground = new SolidBrush(cellStyle.SelectionBackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }
            }
            else
            {
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground = new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }
            }
            Color cl = (Color)value;
            int space = 4;
            Rectangle r = new Rectangle(cellBounds.Location, cellBounds.Size);
            r.X += space;
            r.Y += space;
            r.Height -= space*2;
            r.Width = r.Height;
            graphics.FillRectangle(new SolidBrush(cl), r);

            // Draw the cell borders, if specified.
            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            { PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle); }
        }
    }
}

