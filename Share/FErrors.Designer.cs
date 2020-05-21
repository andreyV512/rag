namespace Share
{
    partial class FErrors
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FErrors));
            this.Protocol = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Protocol
            // 
            this.Protocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Protocol.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Protocol.ForeColor = System.Drawing.Color.Red;
            this.Protocol.FormattingEnabled = true;
            this.Protocol.ItemHeight = 17;
            this.Protocol.Location = new System.Drawing.Point(0, 0);
            this.Protocol.Name = "Protocol";
            this.Protocol.Size = new System.Drawing.Size(309, 299);
            this.Protocol.TabIndex = 6;
            // 
            // FErrors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 299);
            this.Controls.Add(this.Protocol);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FErrors";
            this.Text = "Список ошибок";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FErrors_FormClosed);
            this.Load += new System.EventHandler(this.FErrors_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Protocol;
    }
}