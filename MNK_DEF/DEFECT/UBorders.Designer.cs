namespace Defect
{
    partial class UBorders
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
            this.lBrak = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lClass2 = new System.Windows.Forms.Label();
            this.llClass2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lBrak
            // 
            this.lBrak.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lBrak.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lBrak.Location = new System.Drawing.Point(94, 0);
            this.lBrak.Name = "lBrak";
            this.lBrak.Size = new System.Drawing.Size(33, 20);
            this.lBrak.TabIndex = 6;
            this.lBrak.Text = "991";
            this.lBrak.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Порог брака, %";
            // 
            // lClass2
            // 
            this.lClass2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lClass2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lClass2.Location = new System.Drawing.Point(233, 0);
            this.lClass2.Name = "lClass2";
            this.lClass2.Size = new System.Drawing.Size(33, 20);
            this.lClass2.TabIndex = 8;
            this.lClass2.Text = "991";
            this.lClass2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // llClass2
            // 
            this.llClass2.AutoSize = true;
            this.llClass2.Location = new System.Drawing.Point(133, 4);
            this.llClass2.Name = "llClass2";
            this.llClass2.Size = new System.Drawing.Size(94, 13);
            this.llClass2.TabIndex = 7;
            this.llClass2.Text = "Порог класс2 , %";
            // 
            // UBorders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lClass2);
            this.Controls.Add(this.llClass2);
            this.Controls.Add(this.lBrak);
            this.Controls.Add(this.label2);
            this.Name = "UBorders";
            this.Size = new System.Drawing.Size(268, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lBrak;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lClass2;
        private System.Windows.Forms.Label llClass2;
    }
}
