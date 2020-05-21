namespace Signals
{
    partial class FView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FView));
            this.ucSignals1 = new Signals.UCSignals();
            this.SuspendLayout();
            // 
            // ucSignals1
            // 
            this.ucSignals1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSignals1.IsAlive = true;
            this.ucSignals1.Location = new System.Drawing.Point(0, 0);
            this.ucSignals1.Name = "ucSignals1";
            this.ucSignals1.Size = new System.Drawing.Size(284, 261);
            this.ucSignals1.TabIndex = 0;
            // 
            // FView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ucSignals1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FView";
            this.Text = "Сигналы";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FView_FormClosed);
            this.Load += new System.EventHandler(this.FView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UCSignals ucSignals1;
    }
}