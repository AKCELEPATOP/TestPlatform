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
            this.buttonChangeUser = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonChangeColorBack = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxFontSize = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonChangeFont = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.dataGridViewPassedTests = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonStatistic = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToPDF = new System.Windows.Forms.Button();
            this.textBoxCurrentUser = new System.Windows.Forms.Label();
            this.textBoxGroupUser = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAvailablePatterns)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPassedTests)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewAvailablePatterns
            // 
            this.dataGridViewAvailablePatterns.AllowUserToAddRows = false;
            this.dataGridViewAvailablePatterns.AllowUserToResizeColumns = false;
            this.dataGridViewAvailablePatterns.AllowUserToResizeRows = false;
            this.dataGridViewAvailablePatterns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewAvailablePatterns.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewAvailablePatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAvailablePatterns.ColumnHeadersVisible = false;
            this.dataGridViewAvailablePatterns.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewAvailablePatterns.Location = new System.Drawing.Point(15, 98);
            this.dataGridViewAvailablePatterns.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewAvailablePatterns.MultiSelect = false;
            this.dataGridViewAvailablePatterns.Name = "dataGridViewAvailablePatterns";
            this.dataGridViewAvailablePatterns.RowHeadersVisible = false;
            this.dataGridViewAvailablePatterns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAvailablePatterns.Size = new System.Drawing.Size(404, 343);
            this.dataGridViewAvailablePatterns.TabIndex = 0;
            this.dataGridViewAvailablePatterns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Form);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Доступные шаблоны:";
            // 
            // buttonBeginTest
            // 
            this.buttonBeginTest.Location = new System.Drawing.Point(103, 449);
            this.buttonBeginTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonBeginTest.Name = "buttonBeginTest";
            this.buttonBeginTest.Size = new System.Drawing.Size(204, 54);
            this.buttonBeginTest.TabIndex = 10;
            this.buttonBeginTest.Text = "Начать тест";
            this.buttonBeginTest.UseVisualStyleBackColor = true;
            this.buttonBeginTest.Click += new System.EventHandler(this.button5_Click);
            // 
            // buttonChangeUser
            // 
            this.buttonChangeUser.Location = new System.Drawing.Point(852, 12);
            this.buttonChangeUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonChangeUser.Name = "buttonChangeUser";
            this.buttonChangeUser.Size = new System.Drawing.Size(125, 43);
            this.buttonChangeUser.TabIndex = 12;
            this.buttonChangeUser.Text = "Сменить";
            this.buttonChangeUser.UseVisualStyleBackColor = true;
            this.buttonChangeUser.Click += new System.EventHandler(this.buttonChangeUser_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(451, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Текущий пользователь: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 31);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Сменить тему";
            // 
            // buttonChangeColorBack
            // 
            this.buttonChangeColorBack.Location = new System.Drawing.Point(19, 66);
            this.buttonChangeColorBack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonChangeColorBack.Name = "buttonChangeColorBack";
            this.buttonChangeColorBack.Size = new System.Drawing.Size(227, 42);
            this.buttonChangeColorBack.TabIndex = 16;
            this.buttonChangeColorBack.Text = "Сменить";
            this.buttonChangeColorBack.UseVisualStyleBackColor = true;
            this.buttonChangeColorBack.Click += new System.EventHandler(this.buttonChangeColorBack_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.comboBoxFontSize);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.buttonChangeFont);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonChangeColorBack);
            this.groupBox2.Location = new System.Drawing.Point(1019, 129);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(259, 313);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройка цветов";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 233);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(173, 17);
            this.label7.TabIndex = 25;
            this.label7.Text = "Сменить размер шрифта";
            // 
            // comboBoxFontSize
            // 
            this.comboBoxFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFontSize.FormattingEnabled = true;
            this.comboBoxFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14"});
            this.comboBoxFontSize.Location = new System.Drawing.Point(67, 266);
            this.comboBoxFontSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxFontSize.Name = "comboBoxFontSize";
            this.comboBoxFontSize.Size = new System.Drawing.Size(132, 24);
            this.comboBoxFontSize.TabIndex = 24;
            this.comboBoxFontSize.SelectedIndexChanged += new System.EventHandler(this.comboBoxFontSize_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 130);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Сменить цвет шрифта";
            // 
            // buttonChangeFont
            // 
            this.buttonChangeFont.Location = new System.Drawing.Point(19, 166);
            this.buttonChangeFont.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonChangeFont.Name = "buttonChangeFont";
            this.buttonChangeFont.Size = new System.Drawing.Size(227, 42);
            this.buttonChangeFont.TabIndex = 18;
            this.buttonChangeFont.Text = "Сменить";
            this.buttonChangeFont.UseVisualStyleBackColor = true;
            this.buttonChangeFont.Click += new System.EventHandler(this.buttonChangeFont_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(1148, 491);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(116, 34);
            this.buttonExit.TabIndex = 19;
            this.buttonExit.Text = "Выход";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // dataGridViewPassedTests
            // 
            this.dataGridViewPassedTests.AllowUserToAddRows = false;
            this.dataGridViewPassedTests.AllowUserToResizeColumns = false;
            this.dataGridViewPassedTests.AllowUserToResizeRows = false;
            this.dataGridViewPassedTests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPassedTests.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewPassedTests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPassedTests.ColumnHeadersVisible = false;
            this.dataGridViewPassedTests.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewPassedTests.Location = new System.Drawing.Point(455, 98);
            this.dataGridViewPassedTests.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewPassedTests.MultiSelect = false;
            this.dataGridViewPassedTests.Name = "dataGridViewPassedTests";
            this.dataGridViewPassedTests.RowHeadersVisible = false;
            this.dataGridViewPassedTests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPassedTests.Size = new System.Drawing.Size(529, 343);
            this.dataGridViewPassedTests.TabIndex = 23;
            this.dataGridViewPassedTests.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Form);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(515, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "Пройденные тесты:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(985, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 26;
            this.label3.Text = "Группа:";
            // 
            // buttonStatistic
            // 
            this.buttonStatistic.Location = new System.Drawing.Point(492, 449);
            this.buttonStatistic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStatistic.Name = "buttonStatistic";
            this.buttonStatistic.Size = new System.Drawing.Size(204, 54);
            this.buttonStatistic.TabIndex = 28;
            this.buttonStatistic.Text = "Общая статистика";
            this.buttonStatistic.UseVisualStyleBackColor = true;
            this.buttonStatistic.Click += new System.EventHandler(this.buttonStatistic_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 28);
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(147, 24);
            this.обновитьToolStripMenuItem.Text = "Обновить";
            this.обновитьToolStripMenuItem.Click += new System.EventHandler(this.обновитьToolStripMenuItem_Click_1);
            // 
            // SaveToPDF
            // 
            this.SaveToPDF.Location = new System.Drawing.Point(741, 449);
            this.SaveToPDF.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SaveToPDF.Name = "SaveToPDF";
            this.SaveToPDF.Size = new System.Drawing.Size(204, 54);
            this.SaveToPDF.TabIndex = 29;
            this.SaveToPDF.Text = "Сохранить PDF";
            this.SaveToPDF.UseVisualStyleBackColor = true;
            this.SaveToPDF.Click += new System.EventHandler(this.buttonSaveToPdf_Click);
            // 
            // textBoxCurrentUser
            // 
            this.textBoxCurrentUser.AutoSize = true;
            this.textBoxCurrentUser.Location = new System.Drawing.Point(696, 26);
            this.textBoxCurrentUser.Name = "textBoxCurrentUser";
            this.textBoxCurrentUser.Size = new System.Drawing.Size(0, 17);
            this.textBoxCurrentUser.TabIndex = 30;
            // 
            // textBoxGroupUser
            // 
            this.textBoxGroupUser.AutoSize = true;
            this.textBoxGroupUser.Location = new System.Drawing.Point(1081, 26);
            this.textBoxGroupUser.Name = "textBoxGroupUser";
            this.textBoxGroupUser.Size = new System.Drawing.Size(0, 17);
            this.textBoxGroupUser.TabIndex = 31;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1295, 554);
            this.Controls.Add(this.textBoxGroupUser);
            this.Controls.Add(this.textBoxCurrentUser);
            this.Controls.Add(this.SaveToPDF);
            this.Controls.Add(this.buttonStatistic);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewPassedTests);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonChangeUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonBeginTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewAvailablePatterns);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(27, 74, 27, 25);
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
        private System.Windows.Forms.Button buttonChangeUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonChangeColorBack;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.DataGridView dataGridViewPassedTests;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonStatistic;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.Button SaveToPDF;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonChangeFont;
        private System.Windows.Forms.Label textBoxCurrentUser;
        private System.Windows.Forms.Label textBoxGroupUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxFontSize;
    }
}