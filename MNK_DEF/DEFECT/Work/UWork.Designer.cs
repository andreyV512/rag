namespace Defect.Work
{
    partial class UWork
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
            this.cbWork = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bTune = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cbTest = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.bReStart = new System.Windows.Forms.Button();
            this.bSG = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbWork
            // 
            this.cbWork.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbWork.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbWork.Location = new System.Drawing.Point(3, 3);
            this.cbWork.Name = "cbWork";
            this.cbWork.Size = new System.Drawing.Size(60, 52);
            this.cbWork.TabIndex = 17;
            this.cbWork.Text = "Пуск";
            this.cbWork.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbWork.UseVisualStyleBackColor = true;
            this.cbWork.Click += new System.EventHandler(this.cbWork_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // bTune
            // 
            this.bTune.Location = new System.Drawing.Point(232, 3);
            this.bTune.Name = "bTune";
            this.bTune.Size = new System.Drawing.Size(75, 23);
            this.bTune.TabIndex = 18;
            this.bTune.Text = "Наладка";
            this.bTune.UseVisualStyleBackColor = true;
            this.bTune.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(151, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Сигналы";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbTest
            // 
            this.cbTest.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbTest.Location = new System.Drawing.Point(69, 3);
            this.cbTest.Name = "cbTest";
            this.cbTest.Size = new System.Drawing.Size(75, 23);
            this.cbTest.TabIndex = 20;
            this.cbTest.Text = "Тест";
            this.cbTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbTest.UseVisualStyleBackColor = true;
            this.cbTest.Click += new System.EventHandler(this.cbTest_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 60);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(758, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(202, 36);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(157, 17);
            this.checkBox1.TabIndex = 22;
            this.checkBox1.Text = "Прерывание на просмотр";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // bReStart
            // 
            this.bReStart.Enabled = false;
            this.bReStart.Location = new System.Drawing.Point(69, 32);
            this.bReStart.Name = "bReStart";
            this.bReStart.Size = new System.Drawing.Size(118, 23);
            this.bReStart.TabIndex = 24;
            this.bReStart.Text = "Продолжить";
            this.bReStart.UseVisualStyleBackColor = true;
            this.bReStart.Click += new System.EventHandler(this.bReStart_Click);
            // 
            // bSG
            // 
            this.bSG.Location = new System.Drawing.Point(313, 3);
            this.bSG.Name = "bSG";
            this.bSG.Size = new System.Drawing.Size(109, 23);
            this.bSG.TabIndex = 25;
            this.bSG.Text = "Группа прочности";
            this.bSG.UseVisualStyleBackColor = true;
            this.bSG.Click += new System.EventHandler(this.bSG_Click);
            // 
            // UWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.bSG);
            this.Controls.Add(this.bReStart);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbTest);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bTune);
            this.Controls.Add(this.cbWork);
            this.Name = "UWork";
            this.Size = new System.Drawing.Size(758, 82);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbWork;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bTune;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbTest;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button bReStart;
        private System.Windows.Forms.Button bSG;
    }
}
