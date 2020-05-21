namespace Protocol
{
    partial class UCProtocol
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
            this.CBFile = new System.Windows.Forms.CheckBox();
            this.Protocol = new System.Windows.Forms.ListBox();
            this.CBSave = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // CBFile
            // 
            this.CBFile.Appearance = System.Windows.Forms.Appearance.Button;
            this.CBFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CBFile.Location = new System.Drawing.Point(65, 3);
            this.CBFile.Name = "CBFile";
            this.CBFile.Size = new System.Drawing.Size(51, 23);
            this.CBFile.TabIndex = 7;
            this.CBFile.Text = "В файл";
            this.CBFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CBFile.UseVisualStyleBackColor = true;
            this.CBFile.CheckedChanged += new System.EventHandler(this.CBFile_CheckedChanged);
            // 
            // Protocol
            // 
            this.Protocol.FormattingEnabled = true;
            this.Protocol.Location = new System.Drawing.Point(3, 28);
            this.Protocol.Name = "Protocol";
            this.Protocol.Size = new System.Drawing.Size(198, 303);
            this.Protocol.TabIndex = 5;
            // 
            // CBSave
            // 
            this.CBSave.Appearance = System.Windows.Forms.Appearance.Button;
            this.CBSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CBSave.Location = new System.Drawing.Point(3, 3);
            this.CBSave.Name = "CBSave";
            this.CBSave.Size = new System.Drawing.Size(56, 23);
            this.CBSave.TabIndex = 6;
            this.CBSave.Text = "Память";
            this.CBSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CBSave.UseVisualStyleBackColor = true;
            // 
            // UCProtocol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Protocol);
            this.Controls.Add(this.CBSave);
            this.Controls.Add(this.CBFile);
            this.Name = "UCProtocol";
            this.Size = new System.Drawing.Size(210, 349);
            this.Resize += new System.EventHandler(this.UCProtocol_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox CBFile;
        private System.Windows.Forms.ListBox Protocol;
        private System.Windows.Forms.CheckBox CBSave;

    }
}
