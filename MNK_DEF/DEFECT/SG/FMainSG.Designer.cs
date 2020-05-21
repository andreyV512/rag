namespace Defect.SG
{
    partial class FMainSG
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMainSG));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.протоколToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.окнаПоУмолчаниюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.sgWork1 = new Defect.SG.SGWork();
            this.dgvTubePars = new Defect.SG.DGV();
            this.dgvTube = new Defect.SG.DGV();
            this.dgvEtalonPars = new Defect.SG.DGV();
            this.dgvEtalon = new Defect.SG.DGV();
            this.dgvGroup = new Defect.SG.DGV();
            this.dgvTresh = new Defect.SG.DGV();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ucGraph1 = new Defect.SG.UCGraph();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CMS_Tube = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.CMS_Tube.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.протоколToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1284, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // протоколToolStripMenuItem
            // 
            this.протоколToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.окнаПоУмолчаниюToolStripMenuItem});
            this.протоколToolStripMenuItem.Name = "протоколToolStripMenuItem";
            this.протоколToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.протоколToolStripMenuItem.Text = "Файл";
            // 
            // окнаПоУмолчаниюToolStripMenuItem
            // 
            this.окнаПоУмолчаниюToolStripMenuItem.Name = "окнаПоУмолчаниюToolStripMenuItem";
            this.окнаПоУмолчаниюToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.окнаПоУмолчаниюToolStripMenuItem.Text = "Окна по умолчанию";
            this.окнаПоУмолчаниюToolStripMenuItem.Click += new System.EventHandler(this.окнаПоУмолчаниюToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.sgWork1);
            this.splitContainer1.Panel1.Controls.Add(this.dgvTubePars);
            this.splitContainer1.Panel1.Controls.Add(this.dgvTube);
            this.splitContainer1.Panel1.Controls.Add(this.dgvEtalonPars);
            this.splitContainer1.Panel1.Controls.Add(this.dgvEtalon);
            this.splitContainer1.Panel1.Controls.Add(this.dgvGroup);
            this.splitContainer1.Panel1.Controls.Add(this.dgvTresh);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Resize += new System.EventHandler(this.splitContainer1_Panel1_Resize);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucGraph1);
            this.splitContainer1.Size = new System.Drawing.Size(1484, 870);
            this.splitContainer1.SplitterDistance = 1074;
            this.splitContainer1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(200, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 25;
            this.button2.Text = "График";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // sgWork1
            // 
            this.sgWork1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sgWork1.Location = new System.Drawing.Point(3, 3);
            this.sgWork1.Name = "sgWork1";
            this.sgWork1.Size = new System.Drawing.Size(631, 166);
            this.sgWork1.TabIndex = 24;
            // 
            // dgvTubePars
            // 
            this.dgvTubePars.AllowDelete = false;
            this.dgvTubePars.AllowInsert = false;
            this.dgvTubePars.CC = null;
            this.dgvTubePars.DragFrom = false;
            this.dgvTubePars.DragTo = false;
            this.dgvTubePars.Location = new System.Drawing.Point(335, 401);
            this.dgvTubePars.Name = "dgvTubePars";
            this.dgvTubePars.Size = new System.Drawing.Size(299, 199);
            this.dgvTubePars.TabIndex = 23;
            this.dgvTubePars.TypeName = "Defect.SG.TubePars";
            // 
            // dgvTube
            // 
            this.dgvTube.AllowDelete = true;
            this.dgvTube.AllowInsert = false;
            this.dgvTube.CC = null;
            this.dgvTube.DragFrom = true;
            this.dgvTube.DragTo = false;
            this.dgvTube.Location = new System.Drawing.Point(335, 199);
            this.dgvTube.Name = "dgvTube";
            this.dgvTube.Size = new System.Drawing.Size(299, 199);
            this.dgvTube.TabIndex = 22;
            this.dgvTube.TypeName = "Defect.SG.Tube";
            // 
            // dgvEtalonPars
            // 
            this.dgvEtalonPars.AllowDelete = false;
            this.dgvEtalonPars.AllowInsert = false;
            this.dgvEtalonPars.CC = null;
            this.dgvEtalonPars.DragFrom = false;
            this.dgvEtalonPars.DragTo = false;
            this.dgvEtalonPars.Location = new System.Drawing.Point(-2, 597);
            this.dgvEtalonPars.Name = "dgvEtalonPars";
            this.dgvEtalonPars.Size = new System.Drawing.Size(299, 199);
            this.dgvEtalonPars.TabIndex = 21;
            this.dgvEtalonPars.TypeName = "Defect.SG.EtalonPars";
            // 
            // dgvEtalon
            // 
            this.dgvEtalon.AllowDelete = true;
            this.dgvEtalon.AllowInsert = true;
            this.dgvEtalon.CC = null;
            this.dgvEtalon.DragFrom = false;
            this.dgvEtalon.DragTo = true;
            this.dgvEtalon.Location = new System.Drawing.Point(-2, 401);
            this.dgvEtalon.Name = "dgvEtalon";
            this.dgvEtalon.Size = new System.Drawing.Size(301, 199);
            this.dgvEtalon.TabIndex = 20;
            this.dgvEtalon.TypeName = "Defect.SG.Etalon";
            // 
            // dgvGroup
            // 
            this.dgvGroup.AllowDelete = true;
            this.dgvGroup.AllowInsert = true;
            this.dgvGroup.CC = null;
            this.dgvGroup.DragFrom = false;
            this.dgvGroup.DragTo = false;
            this.dgvGroup.Location = new System.Drawing.Point(-2, 199);
            this.dgvGroup.Name = "dgvGroup";
            this.dgvGroup.Size = new System.Drawing.Size(299, 199);
            this.dgvGroup.TabIndex = 19;
            this.dgvGroup.TypeName = "Defect.SG.Group";
            // 
            // dgvTresh
            // 
            this.dgvTresh.AllowDelete = true;
            this.dgvTresh.AllowInsert = true;
            this.dgvTresh.CC = null;
            this.dgvTresh.DragFrom = false;
            this.dgvTresh.DragTo = false;
            this.dgvTresh.Location = new System.Drawing.Point(335, 606);
            this.dgvTresh.Name = "dgvTresh";
            this.dgvTresh.Size = new System.Drawing.Size(299, 199);
            this.dgvTresh.TabIndex = 17;
            this.dgvTresh.TypeName = "Defect.SG.Tresh";
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Location = new System.Drawing.Point(935, 237);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(18, 18);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Р";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 802);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ucGraph1
            // 
            this.ucGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGraph1.Location = new System.Drawing.Point(0, 0);
            this.ucGraph1.Name = "ucGraph1";
            this.ucGraph1.Schema = null;
            this.ucGraph1.Size = new System.Drawing.Size(402, 866);
            this.ucGraph1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 873);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1284, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // CMS_Tube
            // 
            this.CMS_Tube.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem10,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.CMS_Tube.Name = "contextMenuStrip2";
            this.CMS_Tube.Size = new System.Drawing.Size(201, 114);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem6.Text = "Из файла";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem7.Text = "В файл";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem10.Text = "График";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem8.Text = "Рассчитать параметры";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem9.Text = "Рассчитать ГП";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FMainSG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 895);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FMainSG";
            this.Text = "Группа прочности";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FMain_FormClosed);
            this.Load += new System.EventHandler(this.FMain_Load);
            this.Resize += new System.EventHandler(this.FMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.CMS_Tube.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem протоколToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Defect.SG.UCGraph ucGraph1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip CMS_Tube;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripMenuItem окнаПоУмолчаниюToolStripMenuItem;
        private DGV dgvTresh;
        private DGV dgvGroup;
        private DGV dgvEtalon;
        private DGV dgvEtalonPars;
        private DGV dgvTube;
        private DGV dgvTubePars;
        private SGWork sgWork1;
        private System.Windows.Forms.Button button2;

    }
}

