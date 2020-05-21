namespace Demagnetizer
{
    partial class UDemagnetizer
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.bPositive = new System.Windows.Forms.Button();
            this.bNegative = new System.Windows.Forms.Button();
            this.bFrequency = new System.Windows.Forms.Button();
            this.bOn = new System.Windows.Forms.Button();
            this.bOff = new System.Windows.Forms.Button();
            this.tbPositive = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bRead = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNegative = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFrequency = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Размагничиватель";
            // 
            // bPositive
            // 
            this.bPositive.Location = new System.Drawing.Point(8, 56);
            this.bPositive.Name = "bPositive";
            this.bPositive.Size = new System.Drawing.Size(77, 23);
            this.bPositive.TabIndex = 20;
            this.bPositive.Text = "ПЛЮС";
            this.bPositive.UseVisualStyleBackColor = true;
            this.bPositive.Click += new System.EventHandler(this.bPositive_Click);
            // 
            // bNegative
            // 
            this.bNegative.Location = new System.Drawing.Point(8, 85);
            this.bNegative.Name = "bNegative";
            this.bNegative.Size = new System.Drawing.Size(77, 23);
            this.bNegative.TabIndex = 21;
            this.bNegative.Text = "МИНУС";
            this.bNegative.UseVisualStyleBackColor = true;
            this.bNegative.Click += new System.EventHandler(this.bNegative_Click);
            // 
            // bFrequency
            // 
            this.bFrequency.Location = new System.Drawing.Point(8, 114);
            this.bFrequency.Name = "bFrequency";
            this.bFrequency.Size = new System.Drawing.Size(77, 23);
            this.bFrequency.TabIndex = 22;
            this.bFrequency.Text = "Частота";
            this.bFrequency.UseVisualStyleBackColor = true;
            this.bFrequency.Click += new System.EventHandler(this.bFrequency_Click);
            // 
            // bOn
            // 
            this.bOn.Location = new System.Drawing.Point(8, 143);
            this.bOn.Name = "bOn";
            this.bOn.Size = new System.Drawing.Size(140, 23);
            this.bOn.TabIndex = 23;
            this.bOn.Text = "Включить";
            this.bOn.UseVisualStyleBackColor = true;
            this.bOn.Click += new System.EventHandler(this.bOn_Click);
            // 
            // bOff
            // 
            this.bOff.Location = new System.Drawing.Point(8, 172);
            this.bOff.Name = "bOff";
            this.bOff.Size = new System.Drawing.Size(140, 23);
            this.bOff.TabIndex = 24;
            this.bOff.Text = "Выключить";
            this.bOff.UseVisualStyleBackColor = true;
            this.bOff.Click += new System.EventHandler(this.bOff_Click);
            // 
            // tbPositive
            // 
            this.tbPositive.Location = new System.Drawing.Point(91, 58);
            this.tbPositive.Name = "tbPositive";
            this.tbPositive.Size = new System.Drawing.Size(38, 20);
            this.tbPositive.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "%";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Location = new System.Drawing.Point(8, 204);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(140, 53);
            this.richTextBox1.TabIndex = 28;
            this.richTextBox1.Text = "";
            // 
            // bRead
            // 
            this.bRead.Location = new System.Drawing.Point(8, 27);
            this.bRead.Name = "bRead";
            this.bRead.Size = new System.Drawing.Size(140, 23);
            this.bRead.TabIndex = 29;
            this.bRead.Text = "Прочитать";
            this.bRead.UseVisualStyleBackColor = true;
            this.bRead.Click += new System.EventHandler(this.bRead_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "%";
            // 
            // tbNegative
            // 
            this.tbNegative.Location = new System.Drawing.Point(91, 87);
            this.tbNegative.Name = "tbNegative";
            this.tbNegative.Size = new System.Drawing.Size(38, 20);
            this.tbNegative.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Гц";
            // 
            // tbFrequency
            // 
            this.tbFrequency.Location = new System.Drawing.Point(91, 116);
            this.tbFrequency.Name = "tbFrequency";
            this.tbFrequency.Size = new System.Drawing.Size(38, 20);
            this.tbFrequency.TabIndex = 32;
            // 
            // UDemagnetizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbFrequency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbNegative);
            this.Controls.Add(this.bRead);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbPositive);
            this.Controls.Add(this.bOff);
            this.Controls.Add(this.bOn);
            this.Controls.Add(this.bFrequency);
            this.Controls.Add(this.bNegative);
            this.Controls.Add(this.bPositive);
            this.Controls.Add(this.label1);
            this.Name = "UDemagnetizer";
            this.Size = new System.Drawing.Size(156, 262);
            this.Load += new System.EventHandler(this.UInverter_Load);
            this.Resize += new System.EventHandler(this.UInverter_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bPositive;
        private System.Windows.Forms.Button bNegative;
        private System.Windows.Forms.Button bFrequency;
        private System.Windows.Forms.Button bOn;
        private System.Windows.Forms.Button bOff;
        private System.Windows.Forms.TextBox tbPositive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bRead;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbNegative;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFrequency;
    }
}
