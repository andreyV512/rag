namespace Share
{
    partial class UNamedLabel
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
            this.lValue = new System.Windows.Forms.Label();
            this.lTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lValue
            // 
            this.lValue.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lValue.Location = new System.Drawing.Point(67, 0);
            this.lValue.Name = "lValue";
            this.lValue.Size = new System.Drawing.Size(65, 15);
            this.lValue.TabIndex = 8;
            this.lValue.Text = "Значение";
            this.lValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Location = new System.Drawing.Point(0, 0);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(61, 13);
            this.lTitle.TabIndex = 7;
            this.lTitle.Text = "Заголовок";
            // 
            // UNamedLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lValue);
            this.Controls.Add(this.lTitle);
            this.Name = "UNamedLabel";
            this.Size = new System.Drawing.Size(134, 15);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lValue;
        private System.Windows.Forms.Label lTitle;
    }
}
