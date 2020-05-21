namespace Defect
{
    partial class UStatist
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
            this.uAll = new Share.UNamedLabel();
            this.uOk = new Share.UNamedLabel();
            this.uBrak = new Share.UNamedLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // uAll
            // 
            this.uAll.Location = new System.Drawing.Point(3, 3);
            this.uAll.Name = "uAll";
            this.uAll.ReSizable = false;
            this.uAll.Size = new System.Drawing.Size(118, 15);
            this.uAll.TabIndex = 0;
            this.uAll.Title = "Всего труб:";
            this.uAll.Value = "122";
            this.uAll.ValueLeft = 65;
            this.uAll.ValueWidth = 50;
            // 
            // uOk
            // 
            this.uOk.Location = new System.Drawing.Point(127, 3);
            this.uOk.Name = "uOk";
            this.uOk.ReSizable = false;
            this.uOk.Size = new System.Drawing.Size(93, 15);
            this.uOk.TabIndex = 1;
            this.uOk.Title = "Годно:";
            this.uOk.Value = "122";
            this.uOk.ValueLeft = 37;
            this.uOk.ValueWidth = 50;
            // 
            // uBrak
            // 
            this.uBrak.Location = new System.Drawing.Point(217, 3);
            this.uBrak.Name = "uBrak";
            this.uBrak.ReSizable = false;
            this.uBrak.Size = new System.Drawing.Size(83, 15);
            this.uBrak.TabIndex = 2;
            this.uBrak.Title = "Брак:";
            this.uBrak.Value = "122";
            this.uBrak.ValueLeft = 32;
            this.uBrak.ValueWidth = 50;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(306, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 3;
            this.button1.Text = "Обнулить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UStatist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uBrak);
            this.Controls.Add(this.uOk);
            this.Controls.Add(this.uAll);
            this.Name = "UStatist";
            this.Size = new System.Drawing.Size(383, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private Share.UNamedLabel uAll;
        private Share.UNamedLabel uOk;
        private Share.UNamedLabel uBrak;
        private System.Windows.Forms.Button button1;
    }
}
