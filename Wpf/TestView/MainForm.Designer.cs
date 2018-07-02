namespace TestView
{
    partial class FormMain
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
            ApiClient.Logout();
            if (parent != null)
                parent.DisposeForm();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridViewAvailablePatterns = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBeginTest = new System.Windows.Forms.Button();
            this.textBoxCurrentUser = new System.Windows.Forms.TextBox();
            this.buttonChangeUser = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonChangeColorBack = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonExit = new System.Windows.Forms.Button();
            this.dataGridViewPassedTests = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxGroupUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonResultOfTest = new System.Windows.Forms.Button();
            this.buttonStatistic = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAvailablePatterns)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPassedTests)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewAvailablePatterns
            // 
            this.dataGridViewAvailablePatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAvailablePatterns.Location = new System.Drawing.Point(11, 80);
            this.dataGridViewAvailablePatterns.Name = "dataGridViewAvailablePatterns";
            this.dataGridViewAvailablePatterns.Size = new System.Drawing.Size(303, 279);
            this.dataGridViewAvailablePatterns.TabIndex = 0;
            this.dataGridViewAvailablePatterns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAvailablePatterns_CellContentClick);
            this.dataGridViewAvailablePatterns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Form);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Доступные шаблоны:";
            // 
            // buttonBeginTest
            // 
            this.buttonBeginTest.Location = new System.Drawing.Point(86, 365);
            this.buttonBeginTest.Name = "buttonBeginTest";
            this.buttonBeginTest.Size = new System.Drawing.Size(134, 28);
            this.buttonBeginTest.TabIndex = 10;
            this.buttonBeginTest.Text = "Начать тест";
            this.buttonBeginTest.UseVisualStyleBackColor = true;
            this.buttonBeginTest.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBoxCurrentUser
            // 
            this.textBoxCurrentUser.Location = new System.Drawing.Point(510, 38);
            this.textBoxCurrentUser.Name = "textBoxCurrentUser";
            this.textBoxCurrentUser.ReadOnly = true;
            this.textBoxCurrentUser.Size = new System.Drawing.Size(124, 20);
            this.textBoxCurrentUser.TabIndex = 13;
            // 
            // buttonChangeUser
            // 
            this.buttonChangeUser.Location = new System.Drawing.Point(642, 36);
            this.buttonChangeUser.Name = "buttonChangeUser";
            this.buttonChangeUser.Size = new System.Drawing.Size(73, 22);
            this.buttonChangeUser.TabIndex = 12;
            this.buttonChangeUser.Text = "Сменить";
            this.buttonChangeUser.UseVisualStyleBackColor = true;
            this.buttonChangeUser.Click += new System.EventHandler(this.buttonChangeUser_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(363, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Текущий пользователь: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Сменить тему";
            // 
            // buttonChangeColorBack
            // 
            this.buttonChangeColorBack.Location = new System.Drawing.Point(30, 35);
            this.buttonChangeColorBack.Name = "buttonChangeColorBack";
            this.buttonChangeColorBack.Size = new System.Drawing.Size(101, 30);
            this.buttonChangeColorBack.TabIndex = 16;
            this.buttonChangeColorBack.Text = "Сменить";
            this.buttonChangeColorBack.UseVisualStyleBackColor = true;
            this.buttonChangeColorBack.Click += new System.EventHandler(this.buttonChangeColorBack_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonChangeColorBack);
            this.groupBox2.Location = new System.Drawing.Point(764, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 124);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройка цветов";
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(839, 369);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(87, 28);
            this.buttonExit.TabIndex = 19;
            this.buttonExit.Text = "Выход";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // dataGridViewPassedTests
            // 
            this.dataGridViewPassedTests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPassedTests.Location = new System.Drawing.Point(341, 80);
            this.dataGridViewPassedTests.Name = "dataGridViewPassedTests";
            this.dataGridViewPassedTests.Size = new System.Drawing.Size(397, 279);
            this.dataGridViewPassedTests.TabIndex = 23;
            this.dataGridViewPassedTests.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPassedTests_CellContentClick);
            this.dataGridViewPassedTests.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Form);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(338, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Пройденные тесты:";
            // 
            // textBoxGroupUser
            // 
            this.textBoxGroupUser.Location = new System.Drawing.Point(802, 38);
            this.textBoxGroupUser.Name = "textBoxGroupUser";
            this.textBoxGroupUser.ReadOnly = true;
            this.textBoxGroupUser.Size = new System.Drawing.Size(124, 20);
            this.textBoxGroupUser.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(751, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Группа:";
            // 
            // buttonResultOfTest
            // 
            this.buttonResultOfTest.Location = new System.Drawing.Point(464, 366);
            this.buttonResultOfTest.Name = "buttonResultOfTest";
            this.buttonResultOfTest.Size = new System.Drawing.Size(148, 27);
            this.buttonResultOfTest.TabIndex = 27;
            this.buttonResultOfTest.Text = "Результат теста";
            this.buttonResultOfTest.UseVisualStyleBackColor = true;
            // 
            // buttonStatistic
            // 
            this.buttonStatistic.Location = new System.Drawing.Point(781, 254);
            this.buttonStatistic.Name = "buttonStatistic";
            this.buttonStatistic.Size = new System.Drawing.Size(134, 28);
            this.buttonStatistic.TabIndex = 28;
            this.buttonStatistic.Text = "Общая статистика";
            this.buttonStatistic.UseVisualStyleBackColor = true;
            this.buttonStatistic.Click += new System.EventHandler(this.buttonStatistic_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 26);
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.обновитьToolStripMenuItem.Text = "Обновить";
            this.обновитьToolStripMenuItem.Click += new System.EventHandler(this.обновитьToolStripMenuItem_Click_1);
            // 
            // SaveToPDF
            // 
            this.SaveToPDF.Location = new System.Drawing.Point(781, 288);
            this.SaveToPDF.Name = "SaveToPDF";
            this.SaveToPDF.Size = new System.Drawing.Size(134, 28);
            this.SaveToPDF.TabIndex = 29;
            this.SaveToPDF.Text = "Сохранить PDF";
            this.SaveToPDF.UseVisualStyleBackColor = true;
            this.SaveToPDF.Click += new System.EventHandler(this.buttonSaveToPdf_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 432);
            this.Controls.Add(this.SaveToPDF);
            this.Controls.Add(this.buttonStatistic);
            this.Controls.Add(this.buttonResultOfTest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxGroupUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewPassedTests);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBoxCurrentUser);
            this.Controls.Add(this.buttonChangeUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonBeginTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewAvailablePatterns);
            this.Name = "FormMain";
            this.Text = "Главный экран";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Form);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAvailablePatterns)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPassedTests)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAvailablePatterns;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBeginTest;
        private System.Windows.Forms.TextBox textBoxCurrentUser;
        private System.Windows.Forms.Button buttonChangeUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonChangeColorBack;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.DataGridView dataGridViewPassedTests;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxGroupUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonResultOfTest;
        private System.Windows.Forms.Button buttonStatistic;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.Button SaveToPDF;
    }
}