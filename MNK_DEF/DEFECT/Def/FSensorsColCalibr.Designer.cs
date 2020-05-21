namespace Defect.Def
{
    partial class FSensorsColCalibr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSensorsColCalibr));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chScroll = new System.Windows.Forms.CheckBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bConfirm = new System.Windows.Forms.Button();
            this.cbCalibr = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.uSensorsColCalibr1 = new Defect.Def.USensorsColCalibr();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chScroll);
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bConfirm);
            this.panel1.Controls.Add(this.cbCalibr);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(635, 31);
            this.panel1.TabIndex = 0;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // chScroll
            // 
            this.chScroll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chScroll.Location = new System.Drawing.Point(563, 4);
            this.chScroll.Name = "chScroll";
            this.chScroll.Size = new System.Drawing.Size(69, 24);
            this.chScroll.TabIndex = 3;
            this.chScroll.Text = "Скроллинг";
            this.chScroll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chScroll, "Переключатель колёсика мышки:\r\n- скроллинг \r\n- движение курсора на графике");
            this.chScroll.UseVisualStyleBackColor = true;
            this.chScroll.CheckedChanged += new System.EventHandler(this.chScroll_CheckedChanged);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(249, 3);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(72, 23);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Отменить";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bConfirm
            // 
            this.bConfirm.Location = new System.Drawing.Point(87, 3);
            this.bConfirm.Name = "bConfirm";
            this.bConfirm.Size = new System.Drawing.Size(156, 23);
            this.bConfirm.TabIndex = 1;
            this.bConfirm.Text = "Подтвердить и пересчитать";
            this.bConfirm.UseVisualStyleBackColor = true;
            this.bConfirm.Click += new System.EventHandler(this.bConfirm_Click);
            // 
            // cbCalibr
            // 
            this.cbCalibr.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbCalibr.AutoSize = true;
            this.cbCalibr.Location = new System.Drawing.Point(3, 3);
            this.cbCalibr.Name = "cbCalibr";
            this.cbCalibr.Size = new System.Drawing.Size(78, 23);
            this.cbCalibr.TabIndex = 0;
            this.cbCalibr.Text = "Калибровка";
            this.cbCalibr.UseVisualStyleBackColor = true;
            this.cbCalibr.CheckedChanged += new System.EventHandler(this.cbCalibr_CheckedChanged);
            // 
            // uSensorsColCalibr1
            // 
            this.uSensorsColCalibr1.AutoScroll = true;
            this.uSensorsColCalibr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uSensorsColCalibr1.HH = 0;
            this.uSensorsColCalibr1.IsScroll = false;
            this.uSensorsColCalibr1.Location = new System.Drawing.Point(0, 31);
            this.uSensorsColCalibr1.Name = "uSensorsColCalibr1";
            this.uSensorsColCalibr1.Size = new System.Drawing.Size(635, 409);
            this.uSensorsColCalibr1.TabIndex = 1;
            // 
            // FSensorsColCalibr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 440);
            this.Controls.Add(this.uSensorsColCalibr1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FSensorsColCalibr";
            this.Text = "FSensors";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FSensors_FormClosed);
            this.Load += new System.EventHandler(this.FSensorsColCalibr_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbCalibr;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bConfirm;
        private USensorsColCalibr uSensorsColCalibr1;
        private System.Windows.Forms.CheckBox chScroll;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}