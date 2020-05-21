namespace Protocol
{
    partial class FProtocol
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FProtocol));
            this.ucProtocol1 = new Protocol.UCProtocol();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ucProtocol1
            // 
            this.ucProtocol1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucProtocol1.IsFile = false;
            this.ucProtocol1.IsSave = false;
            this.ucProtocol1.Location = new System.Drawing.Point(0, 0);
            this.ucProtocol1.Name = "ucProtocol1";
            this.ucProtocol1.Size = new System.Drawing.Size(223, 247);
            this.ucProtocol1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FProtocol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 247);
            this.Controls.Add(this.ucProtocol1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FProtocol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Протокол";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FProtocol_FormClosing);
            this.Load += new System.EventHandler(this.FProtocol_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UCProtocol ucProtocol1;
        private System.Windows.Forms.Timer timer1;

    }
}