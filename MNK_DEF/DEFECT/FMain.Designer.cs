namespace Defect
{
    partial class FMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.протоколToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поперечныйToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.продольныйToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поперечныйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.продольныйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пересчитатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.предыдущаяТрубаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bTest = new System.Windows.Forms.Button();
            this.uSplitter2 = new RAGLib.USplitter();
            this.uSplitter1 = new RAGLib.USplitter();
            this.uSplitter3 = new RAGLib.USplitter();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uThick1 = new Defect.UThick();
            this.uCross1 = new Defect.UCrossLine();
            this.uSum1 = new Defect.USumM1();
            this.uWork1 = new Defect.Work.UWork();
            this.uManage1 = new Defect.UManage();
            this.uLine1 = new Defect.UCrossLine();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(881, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.протоколToolStripMenuItem,
            this.загрузитьToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.пересчитатьToolStripMenuItem,
            this.предыдущаяТрубаToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            this.файлToolStripMenuItem.DropDownOpening += new System.EventHandler(this.файлToolStripMenuItem_DropDownOpening);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // протоколToolStripMenuItem
            // 
            this.протоколToolStripMenuItem.Name = "протоколToolStripMenuItem";
            this.протоколToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.протоколToolStripMenuItem.Text = "Протокол";
            this.протоколToolStripMenuItem.Click += new System.EventHandler(this.протоколToolStripMenuItem_Click);
            // 
            // загрузитьToolStripMenuItem
            // 
            this.загрузитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.поперечныйToolStripMenuItem1,
            this.продольныйToolStripMenuItem1});
            this.загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            this.загрузитьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.загрузитьToolStripMenuItem.Text = "Загрузить";
            // 
            // поперечныйToolStripMenuItem1
            // 
            this.поперечныйToolStripMenuItem1.Name = "поперечныйToolStripMenuItem1";
            this.поперечныйToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.поперечныйToolStripMenuItem1.Text = "Поперечный";
            this.поперечныйToolStripMenuItem1.Click += new System.EventHandler(this.поперечныйToolStripMenuItem1_Click);
            // 
            // продольныйToolStripMenuItem1
            // 
            this.продольныйToolStripMenuItem1.Name = "продольныйToolStripMenuItem1";
            this.продольныйToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.продольныйToolStripMenuItem1.Text = "Продольный";
            this.продольныйToolStripMenuItem1.Click += new System.EventHandler(this.продольныйToolStripMenuItem1_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.поперечныйToolStripMenuItem,
            this.продольныйToolStripMenuItem});
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            // 
            // поперечныйToolStripMenuItem
            // 
            this.поперечныйToolStripMenuItem.Name = "поперечныйToolStripMenuItem";
            this.поперечныйToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.поперечныйToolStripMenuItem.Text = "Поперечный";
            this.поперечныйToolStripMenuItem.Click += new System.EventHandler(this.поперечныйToolStripMenuItem_Click);
            // 
            // продольныйToolStripMenuItem
            // 
            this.продольныйToolStripMenuItem.Name = "продольныйToolStripMenuItem";
            this.продольныйToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.продольныйToolStripMenuItem.Text = "Продольный";
            this.продольныйToolStripMenuItem.Click += new System.EventHandler(this.продольныйToolStripMenuItem_Click);
            // 
            // пересчитатьToolStripMenuItem
            // 
            this.пересчитатьToolStripMenuItem.Name = "пересчитатьToolStripMenuItem";
            this.пересчитатьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.пересчитатьToolStripMenuItem.Text = "Пересчитать";
            this.пересчитатьToolStripMenuItem.Click += new System.EventHandler(this.пересчитатьToolStripMenuItem_Click);
            // 
            // предыдущаяТрубаToolStripMenuItem
            // 
            this.предыдущаяТрубаToolStripMenuItem.Name = "предыдущаяТрубаToolStripMenuItem";
            this.предыдущаяТрубаToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.предыдущаяТрубаToolStripMenuItem.Text = "Предыдущая труба";
            this.предыдущаяТрубаToolStripMenuItem.Click += new System.EventHandler(this.предыдущаяТрубаToolStripMenuItem_Click);
            // 
            // bTest
            // 
            this.bTest.Location = new System.Drawing.Point(64, 1);
            this.bTest.Name = "bTest";
            this.bTest.Size = new System.Drawing.Size(75, 23);
            this.bTest.TabIndex = 14;
            this.bTest.Text = "RAG test";
            this.bTest.UseVisualStyleBackColor = true;
            this.bTest.Visible = false;
            this.bTest.Click += new System.EventHandler(this.button1_Click);
            // 
            // uSplitter2
            // 
            this.uSplitter2.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.uSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.uSplitter2.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.uSplitter2.Location = new System.Drawing.Point(12, 401);
            this.uSplitter2.Name = "uSplitter2";
            this.uSplitter2.Size = new System.Drawing.Size(834, 10);
            this.uSplitter2.TabIndex = 11;
            // 
            // uSplitter1
            // 
            this.uSplitter1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.uSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.uSplitter1.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.uSplitter1.Location = new System.Drawing.Point(12, 329);
            this.uSplitter1.Name = "uSplitter1";
            this.uSplitter1.Size = new System.Drawing.Size(834, 10);
            this.uSplitter1.TabIndex = 19;
            // 
            // uSplitter3
            // 
            this.uSplitter3.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.uSplitter3.BackColor = System.Drawing.SystemColors.Control;
            this.uSplitter3.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.uSplitter3.Location = new System.Drawing.Point(12, 492);
            this.uSplitter3.Name = "uSplitter3";
            this.uSplitter3.Size = new System.Drawing.Size(834, 10);
            this.uSplitter3.TabIndex = 20;
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // uThick1
            // 
            this.uThick1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uThick1.Location = new System.Drawing.Point(12, 165);
            this.uThick1.Name = "uThick1";
            this.uThick1.Size = new System.Drawing.Size(834, 92);
            this.uThick1.TabIndex = 21;
            this.uThick1.Title = "Толщиномер";
            // 
            // uCross1
            // 
            this.uCross1.Borders = new double[] {
        991D,
        991D};
            this.uCross1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uCross1.IsWorkVisible = true;
            this.uCross1.Location = new System.Drawing.Point(12, 345);
            this.uCross1.Name = "uCross1";
            this.uCross1.Size = new System.Drawing.Size(834, 50);
            this.uCross1.StateH = null;
            this.uCross1.TabIndex = 18;
            this.uCross1.Title = "Поперечный";
            this.uCross1.ViewMode = true;
            // 
            // uSum1
            // 
            this.uSum1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uSum1.Location = new System.Drawing.Point(12, 508);
            this.uSum1.Name = "uSum1";
            this.uSum1.Size = new System.Drawing.Size(834, 61);
            this.uSum1.TabIndex = 17;
            this.uSum1.Title = "Итог";
            // 
            // uWork1
            // 
            this.uWork1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uWork1.Location = new System.Drawing.Point(12, 27);
            this.uWork1.Name = "uWork1";
            this.uWork1.Size = new System.Drawing.Size(834, 89);
            this.uWork1.TabIndex = 16;
            // 
            // uManage1
            // 
            this.uManage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uManage1.Location = new System.Drawing.Point(12, 122);
            this.uManage1.Name = "uManage1";
            this.uManage1.sgState = null;
            this.uManage1.Size = new System.Drawing.Size(834, 37);
            this.uManage1.TabIndex = 15;
            // 
            // uLine1
            // 
            this.uLine1.Borders = new double[] {
        991D,
        991D};
            this.uLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uLine1.IsWorkVisible = true;
            this.uLine1.Location = new System.Drawing.Point(12, 417);
            this.uLine1.Name = "uLine1";
            this.uLine1.Size = new System.Drawing.Size(834, 69);
            this.uLine1.StateH = null;
            this.uLine1.TabIndex = 7;
            this.uLine1.Title = "Продольный";
            this.uLine1.ViewMode = true;
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 581);
            this.Controls.Add(this.uThick1);
            this.Controls.Add(this.uSplitter3);
            this.Controls.Add(this.uSplitter1);
            this.Controls.Add(this.uCross1);
            this.Controls.Add(this.uSum1);
            this.Controls.Add(this.uWork1);
            this.Controls.Add(this.uManage1);
            this.Controls.Add(this.bTest);
            this.Controls.Add(this.uSplitter2);
            this.Controls.Add(this.uLine1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FMain";
            this.Text = "Модуль магнитоиндукционной дефектоскопии поперечных и продольных дефектов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FMain_FormClosed);
            this.Load += new System.EventHandler(this.FMain_Load);
            this.Resize += new System.EventHandler(this.FMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private UCrossLine uLine1;
        private RAGLib.USplitter uSplitter2;
        private System.Windows.Forms.Button bTest;
        private System.Windows.Forms.ToolStripMenuItem протоколToolStripMenuItem;
        private UManage uManage1;
        private Work.UWork uWork1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пересчитатьToolStripMenuItem;
        private USumM1 uSum1;
        private UCrossLine uCross1;
        private RAGLib.USplitter uSplitter1;
        private RAGLib.USplitter uSplitter3;
        private UThick uThick1;
        private System.Windows.Forms.ToolStripMenuItem поперечныйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem продольныйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поперечныйToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem продольныйToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem предыдущаяТрубаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
    }
}

