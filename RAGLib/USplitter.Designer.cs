namespace RAGLib
{
    partial class USplitter
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
            this.SuspendLayout();
            // 
            // USplitter
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.Name = "USplitter";
            this.Size = new System.Drawing.Size(746, 10);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.USplitter_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.USplitter_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.USplitter_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.USplitter_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
