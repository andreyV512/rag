namespace Defect
{
    partial class FPrevTube
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPrevTube));
            this.uSumM1 = new Defect.USumM1();
            this.SuspendLayout();
            // 
            // uSumM1
            // 
            this.uSumM1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uSumM1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uSumM1.Location = new System.Drawing.Point(0, 0);
            this.uSumM1.Name = "uSumM1";
            this.uSumM1.Size = new System.Drawing.Size(387, 376);
            this.uSumM1.TabIndex = 0;
            this.uSumM1.Title = "Итог";
            // 
            // FPrevTube
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 376);
            this.Controls.Add(this.uSumM1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FPrevTube";
            this.Text = "Предыдущая труба";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FPrevTube_FormClosed);
            this.Load += new System.EventHandler(this.FPrevTube_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private USumM1 uSumM1;
    }
}