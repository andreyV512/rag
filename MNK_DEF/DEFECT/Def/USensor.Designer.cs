namespace Defect.Def
{
    partial class USensor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.uCalibr1 = new Defect.Def.UCalibr();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uSensorData1 = new Defect.Def.USensorData();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uCalibr1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(404, 26);
            this.panel1.TabIndex = 3;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // uCalibr1
            // 
            this.uCalibr1.Gain = 0D;
            this.uCalibr1.Location = new System.Drawing.Point(292, 2);
            this.uCalibr1.Name = "uCalibr1";
            this.uCalibr1.Size = new System.Drawing.Size(109, 21);
            this.uCalibr1.Step = 1D;
            this.uCalibr1.TabIndex = 7;
            this.uCalibr1.OnStep += new Defect.Def.UCalibr.DOnStep(this.uCalibr1_OnStep);
            this.uCalibr1.OnGain += new Defect.Def.UCalibr.DOnGain(this.uCalibr1_OnGain);
            this.uCalibr1.OnCalibrate += new Defect.Def.UCalibr.DOnCalibrate(this.uCalibr1_OnCalibrate);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(57, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "asasd";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            // 
            // uSensorData1
            // 
            this.uSensorData1.CanFocused = false;
            this.uSensorData1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uSensorData1.Location = new System.Drawing.Point(0, 26);
            this.uSensorData1.Name = "uSensorData1";
            this.uSensorData1.Size = new System.Drawing.Size(404, 297);
            this.uSensorData1.TabIndex = 4;
            // 
            // USensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.uSensorData1);
            this.Controls.Add(this.panel1);
            this.Name = "USensor";
            this.Size = new System.Drawing.Size(404, 323);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private USensorData uSensorData1;
        private System.Windows.Forms.Label label2;
        private UCalibr uCalibr1;
    }
}
