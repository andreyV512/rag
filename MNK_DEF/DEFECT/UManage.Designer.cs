namespace Defect
{
    partial class UManage
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
            this.lSettings = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.udbTube1 = new Share.UDBTube();
            this.uStatist1 = new Defect.UStatist2();
            this.usg1 = new Defect.SG.USG();
            this.SuspendLayout();
            // 
            // lSettings
            // 
            this.lSettings.AutoSize = true;
            this.lSettings.Location = new System.Drawing.Point(223, 3);
            this.lSettings.Name = "lSettings";
            this.lSettings.Size = new System.Drawing.Size(35, 13);
            this.lSettings.TabIndex = 26;
            this.lSettings.Text = "label1";
            // 
            // udbTube1
            // 
            this.udbTube1.Location = new System.Drawing.Point(3, 3);
            this.udbTube1.Name = "udbTube1";
            this.udbTube1.Size = new System.Drawing.Size(214, 28);
            this.udbTube1.TabIndex = 27;
            this.udbTube1.TypeSize = "";
            // 
            // uStatist1
            // 
            this.uStatist1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uStatist1.Location = new System.Drawing.Point(618, 3);
            this.uStatist1.Name = "uStatist1";
            this.uStatist1.Size = new System.Drawing.Size(497, 28);
            this.uStatist1.TabIndex = 29;
            // 
            // usg1
            // 
            this.usg1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.usg1.Location = new System.Drawing.Point(329, 3);
            this.usg1.Name = "usg1";
            this.usg1.Size = new System.Drawing.Size(283, 30);
            this.usg1.State = null;
            this.usg1.TabIndex = 28;
            // 
            // UManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.uStatist1);
            this.Controls.Add(this.usg1);
            this.Controls.Add(this.udbTube1);
            this.Controls.Add(this.lSettings);
            this.Name = "UManage";
            this.Size = new System.Drawing.Size(1200, 36);
            this.Resize += new System.EventHandler(this.UManage_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lSettings;
        private System.Windows.Forms.ToolTip toolTip1;
        private Share.UDBTube udbTube1;
        private SG.USG usg1;
        private UStatist2 uStatist1;
    }
}
