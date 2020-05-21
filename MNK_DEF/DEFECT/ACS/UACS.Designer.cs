namespace Defect.ACS
{
    partial class UACS
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
            this.label1 = new System.Windows.Forms.Label();
            this.bTest = new System.Windows.Forms.Button();
            this.bIn = new System.Windows.Forms.Button();
            this.bOut = new System.Windows.Forms.Button();
            this.lIn = new System.Windows.Forms.Label();
            this.lOut = new System.Windows.Forms.Label();
            this.bResult = new System.Windows.Forms.Button();
            this.lError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "АСУ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bTest
            // 
            this.bTest.Location = new System.Drawing.Point(3, 23);
            this.bTest.Name = "bTest";
            this.bTest.Size = new System.Drawing.Size(120, 23);
            this.bTest.TabIndex = 2;
            this.bTest.Text = "Тест связи";
            this.bTest.UseVisualStyleBackColor = true;
            this.bTest.Click += new System.EventHandler(this.bTest_Click);
            // 
            // bIn
            // 
            this.bIn.Location = new System.Drawing.Point(3, 52);
            this.bIn.Name = "bIn";
            this.bIn.Size = new System.Drawing.Size(120, 23);
            this.bIn.TabIndex = 3;
            this.bIn.Text = "Труба на входе";
            this.bIn.UseVisualStyleBackColor = true;
            this.bIn.Click += new System.EventHandler(this.bIn_Click);
            // 
            // bOut
            // 
            this.bOut.Location = new System.Drawing.Point(3, 81);
            this.bOut.Name = "bOut";
            this.bOut.Size = new System.Drawing.Size(120, 23);
            this.bOut.TabIndex = 4;
            this.bOut.Text = "Труба на индикации";
            this.bOut.UseVisualStyleBackColor = true;
            this.bOut.Click += new System.EventHandler(this.bOut_Click);
            // 
            // lIn
            // 
            this.lIn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lIn.Location = new System.Drawing.Point(129, 53);
            this.lIn.Name = "lIn";
            this.lIn.Size = new System.Drawing.Size(72, 20);
            this.lIn.TabIndex = 6;
            this.lIn.Text = "1234,555";
            this.lIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lOut
            // 
            this.lOut.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lOut.Location = new System.Drawing.Point(129, 82);
            this.lOut.Name = "lOut";
            this.lOut.Size = new System.Drawing.Size(72, 20);
            this.lOut.TabIndex = 7;
            this.lOut.Text = "1234,555";
            this.lOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bResult
            // 
            this.bResult.Location = new System.Drawing.Point(3, 110);
            this.bResult.Name = "bResult";
            this.bResult.Size = new System.Drawing.Size(120, 23);
            this.bResult.TabIndex = 8;
            this.bResult.Text = "Тестовый результат";
            this.bResult.UseVisualStyleBackColor = true;
            this.bResult.Click += new System.EventHandler(this.bResult_Click);
            // 
            // lError
            // 
            this.lError.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lError.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lError.Location = new System.Drawing.Point(3, 136);
            this.lError.Name = "lError";
            this.lError.Size = new System.Drawing.Size(198, 20);
            this.lError.TabIndex = 9;
            this.lError.Text = "1234,555";
            this.lError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UACS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.lError);
            this.Controls.Add(this.bResult);
            this.Controls.Add(this.lOut);
            this.Controls.Add(this.lIn);
            this.Controls.Add(this.bOut);
            this.Controls.Add(this.bIn);
            this.Controls.Add(this.bTest);
            this.Controls.Add(this.label1);
            this.Name = "UACS";
            this.Size = new System.Drawing.Size(208, 162);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bTest;
        private System.Windows.Forms.Button bIn;
        private System.Windows.Forms.Button bOut;
        private System.Windows.Forms.Label lIn;
        private System.Windows.Forms.Label lOut;
        private System.Windows.Forms.Button bResult;
        private System.Windows.Forms.Label lError;
    }
}
