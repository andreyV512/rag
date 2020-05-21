namespace Defect.Work
{
    partial class FTune
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTune));
            this.uRectifierC = new RectifierNS.uRectifier();
            this.uRectifierL = new RectifierNS.uRectifier();
            this.uInverter1 = new InverterNS.UInverter();
            this.uacs1 = new Defect.ACS.UACS();
            this.uGSPF1 = new Defect.SG.uGSPF();
            this.uDemagnetizer1 = new Demagnetizer.UDemagnetizer();
            this.SuspendLayout();
            // 
            // uRectifierC
            // 
            this.uRectifierC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uRectifierC.Location = new System.Drawing.Point(12, 12);
            this.uRectifierC.Name = "uRectifierC";
            this.uRectifierC.Size = new System.Drawing.Size(202, 205);
            this.uRectifierC.TabIndex = 0;
            this.uRectifierC.Title = "Выпр. Поперечного";
            // 
            // uRectifierL
            // 
            this.uRectifierL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uRectifierL.Location = new System.Drawing.Point(220, 12);
            this.uRectifierL.Name = "uRectifierL";
            this.uRectifierL.Size = new System.Drawing.Size(202, 205);
            this.uRectifierL.TabIndex = 1;
            this.uRectifierL.Title = "Выпр. продольного";
            // 
            // uInverter1
            // 
            this.uInverter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uInverter1.Location = new System.Drawing.Point(428, 12);
            this.uInverter1.Name = "uInverter1";
            this.uInverter1.Size = new System.Drawing.Size(211, 256);
            this.uInverter1.TabIndex = 4;
            this.uInverter1.Title = "Инвертер продольного";
            // 
            // uacs1
            // 
            this.uacs1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uacs1.Location = new System.Drawing.Point(645, 12);
            this.uacs1.Name = "uacs1";
            this.uacs1.Size = new System.Drawing.Size(208, 169);
            this.uacs1.TabIndex = 3;
            // 
            // uGSPF1
            // 
            this.uGSPF1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uGSPF1.Location = new System.Drawing.Point(12, 274);
            this.uGSPF1.Name = "uGSPF1";
            this.uGSPF1.Size = new System.Drawing.Size(1003, 219);
            this.uGSPF1.TabIndex = 2;
            this.uGSPF1.Title = "Генератор";
            // 
            // uDemagnetizer1
            // 
            this.uDemagnetizer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uDemagnetizer1.Location = new System.Drawing.Point(859, 12);
            this.uDemagnetizer1.Name = "uDemagnetizer1";
            this.uDemagnetizer1.Size = new System.Drawing.Size(156, 262);
            this.uDemagnetizer1.TabIndex = 5;
            this.uDemagnetizer1.Title = "Размагничиватель";
            // 
            // FTune
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 515);
            this.Controls.Add(this.uDemagnetizer1);
            this.Controls.Add(this.uInverter1);
            this.Controls.Add(this.uacs1);
            this.Controls.Add(this.uGSPF1);
            this.Controls.Add(this.uRectifierL);
            this.Controls.Add(this.uRectifierC);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FTune";
            this.Text = "Наладка";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FTune_FormClosed);
            this.Load += new System.EventHandler(this.FTune_Load);
            this.Resize += new System.EventHandler(this.FTune_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private RectifierNS.uRectifier uRectifierC;
        private RectifierNS.uRectifier uRectifierL;
        private Defect.SG.uGSPF uGSPF1;
        private ACS.UACS uacs1;
        private InverterNS.UInverter uInverter1;
        private Demagnetizer.UDemagnetizer uDemagnetizer1;
    }
}