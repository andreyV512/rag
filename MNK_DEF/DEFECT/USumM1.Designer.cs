namespace Defect
{
    partial class USumM1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.uTube = new Share.UNamedLabel();
            this.uSelectResult1 = new Defect.USelectResult();
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
            this.panel1.Controls.Add(this.uSelectResult1);
            this.panel1.Controls.Add(this.uTube);
            this.panel1.Size = new System.Drawing.Size(809, 31);
            this.panel1.Controls.SetChildIndex(this.uTube, 0);
            this.panel1.Controls.SetChildIndex(this.uSelectResult1, 0);
            this.panel1.Controls.SetChildIndex(this.label1, 0);
            this.panel1.Controls.SetChildIndex(this.CBIsWork, 0);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.Text = "Итог";
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.SystemColors.Control;
            chartArea1.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.Position;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.IsMarksNextToAxis = false;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorGrid.Interval = 1D;
            chartArea1.AxisX.MajorGrid.IntervalOffset = 0.5D;
            chartArea1.AxisX.MajorTickMark.Size = 2F;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.ScaleBreakStyle.BreakLineStyle = System.Windows.Forms.DataVisualization.Charting.BreakLineStyle.Straight;
            chartArea1.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea1.AxisX.ScaleBreakStyle.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisX.ScaleBreakStyle.Spacing = 0D;
            chartArea1.AxisX.ScaleBreakStyle.StartFromZero = System.Windows.Forms.DataVisualization.Charting.StartFromZero.Yes;
            chartArea1.AxisY.Interval = 3D;
            chartArea1.AxisY.IntervalOffset = 2D;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.Maximum = 1.2D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.BackColor = System.Drawing.SystemColors.Control;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 80F;
            chartArea1.InnerPlotPosition.Width = 97F;
            chartArea1.InnerPlotPosition.X = 2F;
            chartArea1.InnerPlotPosition.Y = 1F;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 85F;
            chartArea1.Position.Width = 99F;
            chartArea1.Position.X = 1F;
            chartArea1.Position.Y = 1F;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Color = System.Drawing.Color.Fuchsia;
            series1.Name = "Series2";
            series1.YValuesPerPoint = 2;
            series2.BorderColor = System.Drawing.Color.Black;
            series2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            series2.ChartArea = "ChartArea1";
            series2.CustomProperties = "DrawSideBySide=True, DrawingStyle=Emboss, EmptyPointValue=Zero, PointWidth=1";
            series2.IsVisibleInLegend = false;
            series2.MarkerBorderColor = System.Drawing.Color.White;
            series2.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(809, 161);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // uTube
            // 
            this.uTube.Location = new System.Drawing.Point(50, 2);
            this.uTube.Name = "uTube";
            this.uTube.ReSizable = false;
            this.uTube.Size = new System.Drawing.Size(137, 15);
            this.uTube.TabIndex = 4;
            this.uTube.Title = "№";
            this.uTube.Value = "123456";
            this.uTube.ValueLeft = 67;
            this.uTube.ValueWidth = 65;
            // 
            // uSelectResult1
            // 
            this.uSelectResult1.BackColor = System.Drawing.SystemColors.Control;
            this.uSelectResult1.Location = new System.Drawing.Point(193, 3);
            this.uSelectResult1.Name = "uSelectResult1";
            this.uSelectResult1.RClass = Share.EClass.Brak;
            this.uSelectResult1.Size = new System.Drawing.Size(227, 22);
            this.uSelectResult1.TabIndex = 5;
            // 
            // USumM1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart1);
            this.Name = "USumM1";
            this.Size = new System.Drawing.Size(809, 161);
            this.Title = "Итог";
            this.Controls.SetChildIndex(this.chart1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Share.UNamedLabel uTube;
        private USelectResult uSelectResult1;
    }
}
