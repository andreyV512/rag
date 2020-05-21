namespace Defect
{
    partial class UCrossLine
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.расчетныеДефектыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дефектыПоЗонеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дефектыВСтолбецToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дефектыПо3ДатчикамToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.исходныеСигналыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uRectifierVew1 = new RectifierNS.URectifierVew();
            this.uBorders1 = new Defect.UBorders();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBIsWork
            // 
            this.CBIsWork.Location = new System.Drawing.Point(110, 4);
            this.CBIsWork.CheckedChanged += new System.EventHandler(this.CBIsWork_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uBorders1);
            this.panel1.Controls.Add(this.uRectifierVew1);
            this.panel1.Size = new System.Drawing.Size(815, 31);
            this.panel1.Controls.SetChildIndex(this.label1, 0);
            this.panel1.Controls.SetChildIndex(this.CBIsWork, 0);
            this.panel1.Controls.SetChildIndex(this.uRectifierVew1, 0);
            this.panel1.Controls.SetChildIndex(this.uBorders1, 0);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.Text = "Поперечный";
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.SystemColors.Control;
            chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IntervalOffset = 0.5D;
            chartArea1.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.IsMarksNextToAxis = false;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorGrid.Interval = 1D;
            chartArea1.AxisX.MajorGrid.IntervalOffset = 0.5D;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.Maximum = 60.5D;
            chartArea1.AxisX.Minimum = 0.5D;
            chartArea1.AxisX.ScaleBreakStyle.BreakLineStyle = System.Windows.Forms.DataVisualization.Charting.BreakLineStyle.Straight;
            chartArea1.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea1.AxisX.ScaleBreakStyle.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisX.ScaleBreakStyle.Spacing = 0D;
            chartArea1.AxisX.ScaleBreakStyle.StartFromZero = System.Windows.Forms.DataVisualization.Charting.StartFromZero.Yes;
            chartArea1.AxisY.Interval = 1D;
            chartArea1.AxisY.LabelStyle.IsEndLabelVisible = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Size = 0.5F;
            chartArea1.BackColor = System.Drawing.SystemColors.Control;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 88F;
            chartArea1.InnerPlotPosition.Width = 97F;
            chartArea1.InnerPlotPosition.X = 2F;
            chartArea1.InnerPlotPosition.Y = 1F;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 99F;
            chartArea1.Position.Width = 99F;
            chartArea1.Position.X = 1F;
            chartArea1.Position.Y = 1F;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 31);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(815, 130);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.расчетныеДефектыToolStripMenuItem,
            this.дефектыПоЗонеToolStripMenuItem,
            this.дефектыВСтолбецToolStripMenuItem,
            this.дефектыПо3ДатчикамToolStripMenuItem,
            this.исходныеСигналыToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(288, 114);
            // 
            // расчетныеДефектыToolStripMenuItem
            // 
            this.расчетныеДефектыToolStripMenuItem.Name = "расчетныеДефектыToolStripMenuItem";
            this.расчетныеДефектыToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.расчетныеДефектыToolStripMenuItem.Text = "Дефекты";
            this.расчетныеДефектыToolStripMenuItem.Click += new System.EventHandler(this.расчетныеДефектыToolStripMenuItem_Click);
            // 
            // дефектыПоЗонеToolStripMenuItem
            // 
            this.дефектыПоЗонеToolStripMenuItem.Name = "дефектыПоЗонеToolStripMenuItem";
            this.дефектыПоЗонеToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.дефектыПоЗонеToolStripMenuItem.Text = "Дефекты по зоне и всем датчикам";
            this.дефектыПоЗонеToolStripMenuItem.Click += new System.EventHandler(this.дефектыПоЗонеToolStripMenuItem_Click);
            // 
            // дефектыВСтолбецToolStripMenuItem
            // 
            this.дефектыВСтолбецToolStripMenuItem.Name = "дефектыВСтолбецToolStripMenuItem";
            this.дефектыВСтолбецToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.дефектыВСтолбецToolStripMenuItem.Text = "Дефекты в столбец ";
            this.дефектыВСтолбецToolStripMenuItem.Click += new System.EventHandler(this.дефектыВСтолбецToolStripMenuItem_Click);
            // 
            // дефектыПо3ДатчикамToolStripMenuItem
            // 
            this.дефектыПо3ДатчикамToolStripMenuItem.Name = "дефектыПо3ДатчикамToolStripMenuItem";
            this.дефектыПо3ДатчикамToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.дефектыПо3ДатчикамToolStripMenuItem.Text = "Дефекты по 3 зонам и одному датчику";
            this.дефектыПо3ДатчикамToolStripMenuItem.Click += new System.EventHandler(this.дефектыПо3ДатчикамToolStripMenuItem_Click);
            // 
            // исходныеСигналыToolStripMenuItem
            // 
            this.исходныеСигналыToolStripMenuItem.Name = "исходныеСигналыToolStripMenuItem";
            this.исходныеСигналыToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.исходныеСигналыToolStripMenuItem.Text = "Исходные сигналы";
            this.исходныеСигналыToolStripMenuItem.Click += new System.EventHandler(this.исходныеСигналыToolStripMenuItem_Click);
            // 
            // uRectifierVew1
            // 
            this.uRectifierVew1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uRectifierVew1.Location = new System.Drawing.Point(187, 0);
            this.uRectifierVew1.Name = "uRectifierVew1";
            this.uRectifierVew1.Size = new System.Drawing.Size(349, 29);
            this.uRectifierVew1.StateH = null;
            this.uRectifierVew1.TabIndex = 3;
            // 
            // uBorders1
            // 
            this.uBorders1.Borders = new double[] {
        991D,
        991D};
            this.uBorders1.Brak = 991D;
            this.uBorders1.Class2 = 991D;
            this.uBorders1.Location = new System.Drawing.Point(542, 4);
            this.uBorders1.Name = "uBorders1";
            this.uBorders1.Size = new System.Drawing.Size(268, 22);
            this.uBorders1.TabIndex = 4;
            // 
            // UCrossLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart1);
            this.Name = "UCrossLine";
            this.Size = new System.Drawing.Size(815, 161);
            this.Title = "Поперечный";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.chart1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem расчетныеДефектыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дефектыПоЗонеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem исходныеСигналыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дефектыПо3ДатчикамToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дефектыВСтолбецToolStripMenuItem;
        private RectifierNS.URectifierVew uRectifierVew1;
        private UBorders uBorders1;
    }
}
