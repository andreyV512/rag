namespace Defect
{
    partial class USumM
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbGoodBad = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // CBIsWork
            // 
            this.CBIsWork.Location = new System.Drawing.Point(50, 4);
            this.CBIsWork.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbGoodBad);
            this.panel1.Size = new System.Drawing.Size(485, 31);
            this.panel1.Controls.SetChildIndex(this.label1, 0);
            this.panel1.Controls.SetChildIndex(this.CBIsWork, 0);
            this.panel1.Controls.SetChildIndex(this.cbGoodBad, 0);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.Text = "Итог";
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.SystemColors.Control;
            chartArea4.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.Position;
            chartArea4.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea4.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea4.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea4.AxisX.IsMarksNextToAxis = false;
            chartArea4.AxisX.IsStartedFromZero = false;
            chartArea4.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea4.AxisX.MajorGrid.Enabled = false;
            chartArea4.AxisX.MajorGrid.Interval = 1D;
            chartArea4.AxisX.MajorGrid.IntervalOffset = 0.5D;
            chartArea4.AxisX.MajorTickMark.Size = 2F;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisX.ScaleBreakStyle.BreakLineStyle = System.Windows.Forms.DataVisualization.Charting.BreakLineStyle.Straight;
            chartArea4.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea4.AxisX.ScaleBreakStyle.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea4.AxisX.ScaleBreakStyle.Spacing = 0D;
            chartArea4.AxisX.ScaleBreakStyle.StartFromZero = System.Windows.Forms.DataVisualization.Charting.StartFromZero.Yes;
            chartArea4.AxisY.Interval = 3D;
            chartArea4.AxisY.IntervalOffset = 2D;
            chartArea4.AxisY.MajorGrid.Enabled = false;
            chartArea4.AxisY.MajorTickMark.Enabled = false;
            chartArea4.AxisY.Maximum = 1.2D;
            chartArea4.AxisY.Minimum = 0D;
            chartArea4.BackColor = System.Drawing.SystemColors.Control;
            chartArea4.InnerPlotPosition.Auto = false;
            chartArea4.InnerPlotPosition.Height = 80F;
            chartArea4.InnerPlotPosition.Width = 97F;
            chartArea4.InnerPlotPosition.X = 2F;
            chartArea4.InnerPlotPosition.Y = 1F;
            chartArea4.Name = "ChartArea1";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 85F;
            chartArea4.Position.Width = 99F;
            chartArea4.Position.X = 1F;
            chartArea4.Position.Y = 1F;
            this.chart1.ChartAreas.Add(chartArea4);
            this.chart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 31);
            this.chart1.Name = "chart1";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series7.Color = System.Drawing.Color.Fuchsia;
            series7.Name = "Series2";
            series7.YValuesPerPoint = 2;
            series8.BorderColor = System.Drawing.Color.Black;
            series8.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            series8.ChartArea = "ChartArea1";
            series8.CustomProperties = "DrawSideBySide=True, DrawingStyle=Emboss, EmptyPointValue=Zero, PointWidth=1";
            series8.IsVisibleInLegend = false;
            series8.MarkerBorderColor = System.Drawing.Color.White;
            series8.Name = "Series1";
            this.chart1.Series.Add(series7);
            this.chart1.Series.Add(series8);
            this.chart1.Size = new System.Drawing.Size(485, 130);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // cbGoodBad
            // 
            this.cbGoodBad.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbGoodBad.Enabled = false;
            this.cbGoodBad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbGoodBad.Location = new System.Drawing.Point(50, 1);
            this.cbGoodBad.Name = "cbGoodBad";
            this.cbGoodBad.Size = new System.Drawing.Size(240, 24);
            this.cbGoodBad.TabIndex = 3;
            this.cbGoodBad.Text = "ГОДНО";
            this.cbGoodBad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbGoodBad.UseVisualStyleBackColor = true;
            this.cbGoodBad.CheckedChanged += new System.EventHandler(this.cbGoodBad_CheckedChanged);
            // 
            // USumM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart1);
            this.Name = "USumM";
            this.Title = "Итог";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.chart1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox cbGoodBad;
    }
}
