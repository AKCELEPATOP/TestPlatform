namespace TestView
{
    partial class TestingForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestingForm));
            this.TextBoxQuestion = new System.Windows.Forms.Label();
            this.answerGroupBoxCheckButtons = new System.Windows.Forms.GroupBox();
            this.answerGroupBoxRadioButtons = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.answer4 = new System.Windows.Forms.CheckBox();
            this.answer3 = new System.Windows.Forms.CheckBox();
            this.answer2 = new System.Windows.Forms.CheckBox();
            this.answer1 = new System.Windows.Forms.CheckBox();
            this.nextQuestion = new System.Windows.Forms.Button();
            this.questionGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.endTest = new System.Windows.Forms.Button();
            this.labelTime = new System.Windows.Forms.Label();
            this.appendixForQestion = new System.Windows.Forms.Button();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxQuestions = new System.Windows.Forms.ListBox();
            this.answerGroupBoxCheckButtons.SuspendLayout();
            this.answerGroupBoxRadioButtons.SuspendLayout();
            this.questionGroupBox.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBoxQuestion
            // 
            this.TextBoxQuestion.Location = new System.Drawing.Point(8, 36);
            this.TextBoxQuestion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TextBoxQuestion.Name = "TextBoxQuestion";
            this.TextBoxQuestion.Size = new System.Drawing.Size(928, 92);
            this.TextBoxQuestion.TabIndex = 2;
            this.TextBoxQuestion.Text = resources.GetString("TextBoxQuestion.Text");
            // 
            // answerGroupBoxCheckButtons
            // 
            this.answerGroupBoxCheckButtons.Controls.Add(this.answerGroupBoxRadioButtons);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer4);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer3);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer2);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer1);
            this.answerGroupBoxCheckButtons.Enabled = false;
            this.answerGroupBoxCheckButtons.Location = new System.Drawing.Point(57, 186);
            this.answerGroupBoxCheckButtons.Margin = new System.Windows.Forms.Padding(4);
            this.answerGroupBoxCheckButtons.Name = "answerGroupBoxCheckButtons";
            this.answerGroupBoxCheckButtons.Padding = new System.Windows.Forms.Padding(4);
            this.answerGroupBoxCheckButtons.Size = new System.Drawing.Size(944, 203);
            this.answerGroupBoxCheckButtons.TabIndex = 3;
            this.answerGroupBoxCheckButtons.TabStop = false;
            this.answerGroupBoxCheckButtons.Text = "Варианты ответов:";
            this.answerGroupBoxCheckButtons.Visible = false;
            // 
            // answerGroupBoxRadioButtons
            // 
            this.answerGroupBoxRadioButtons.Controls.Add(this.radioButton4);
            this.answerGroupBoxRadioButtons.Controls.Add(this.radioButton3);
            this.answerGroupBoxRadioButtons.Controls.Add(this.radioButton2);
            this.answerGroupBoxRadioButtons.Controls.Add(this.radioButton1);
            this.answerGroupBoxRadioButtons.Enabled = false;
            this.answerGroupBoxRadioButtons.Location = new System.Drawing.Point(0, 0);
            this.answerGroupBoxRadioButtons.Margin = new System.Windows.Forms.Padding(4);
            this.answerGroupBoxRadioButtons.Name = "answerGroupBoxRadioButtons";
            this.answerGroupBoxRadioButtons.Padding = new System.Windows.Forms.Padding(4);
            this.answerGroupBoxRadioButtons.Size = new System.Drawing.Size(944, 203);
            this.answerGroupBoxRadioButtons.TabIndex = 4;
            this.answerGroupBoxRadioButtons.TabStop = false;
            this.answerGroupBoxRadioButtons.Text = "Варианты ответов:";
            this.answerGroupBoxRadioButtons.Visible = false;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(36, 156);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(146, 21);
            this.radioButton4.TabIndex = 7;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Вариант ответа 4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(36, 113);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(146, 21);
            this.radioButton3.TabIndex = 6;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Вариант ответа 3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(36, 74);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(146, 21);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Вариант ответа 2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(36, 36);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(146, 21);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Вариант ответа 1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // answer4
            // 
            this.answer4.AutoSize = true;
            this.answer4.Location = new System.Drawing.Point(36, 158);
            this.answer4.Margin = new System.Windows.Forms.Padding(4);
            this.answer4.Name = "answer4";
            this.answer4.Size = new System.Drawing.Size(147, 21);
            this.answer4.TabIndex = 3;
            this.answer4.Text = "Вариант ответа 4";
            this.answer4.UseVisualStyleBackColor = true;
            // 
            // answer3
            // 
            this.answer3.AutoSize = true;
            this.answer3.Location = new System.Drawing.Point(36, 114);
            this.answer3.Margin = new System.Windows.Forms.Padding(4);
            this.answer3.Name = "answer3";
            this.answer3.Size = new System.Drawing.Size(147, 21);
            this.answer3.TabIndex = 2;
            this.answer3.Text = "Вариант ответа 3";
            this.answer3.UseVisualStyleBackColor = true;
            // 
            // answer2
            // 
            this.answer2.AutoSize = true;
            this.answer2.Location = new System.Drawing.Point(36, 75);
            this.answer2.Margin = new System.Windows.Forms.Padding(4);
            this.answer2.Name = "answer2";
            this.answer2.Size = new System.Drawing.Size(147, 21);
            this.answer2.TabIndex = 1;
            this.answer2.Text = "Вариант ответа 2";
            this.answer2.UseVisualStyleBackColor = true;
            // 
            // answer1
            // 
            this.answer1.AutoSize = true;
            this.answer1.Location = new System.Drawing.Point(36, 36);
            this.answer1.Margin = new System.Windows.Forms.Padding(4);
            this.answer1.Name = "answer1";
            this.answer1.Size = new System.Drawing.Size(147, 21);
            this.answer1.TabIndex = 0;
            this.answer1.Text = "Вариант ответа 1";
            this.answer1.UseVisualStyleBackColor = true;
            // 
            // nextQuestion
            // 
            this.nextQuestion.Location = new System.Drawing.Point(439, 414);
            this.nextQuestion.Margin = new System.Windows.Forms.Padding(4);
            this.nextQuestion.Name = "nextQuestion";
            this.nextQuestion.Size = new System.Drawing.Size(179, 33);
            this.nextQuestion.TabIndex = 5;
            this.nextQuestion.Text = "Следующий вопрос";
            this.nextQuestion.UseVisualStyleBackColor = true;
            this.nextQuestion.Click += new System.EventHandler(this.nextQuestion_Click);
            // 
            // questionGroupBox
            // 
            this.questionGroupBox.Controls.Add(this.label1);
            this.questionGroupBox.Controls.Add(this.TextBoxQuestion);
            this.questionGroupBox.Location = new System.Drawing.Point(57, 42);
            this.questionGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.questionGroupBox.Name = "questionGroupBox";
            this.questionGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.questionGroupBox.Size = new System.Drawing.Size(944, 137);
            this.questionGroupBox.TabIndex = 6;
            this.questionGroupBox.TabStop = false;
            this.questionGroupBox.Text = "Вопрос №";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(679, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Категория: ";
            // 
            // endTest
            // 
            this.endTest.Location = new System.Drawing.Point(784, 414);
            this.endTest.Margin = new System.Windows.Forms.Padding(4);
            this.endTest.Name = "endTest";
            this.endTest.Size = new System.Drawing.Size(179, 33);
            this.endTest.TabIndex = 7;
            this.endTest.Text = "Завершить тест";
            this.endTest.UseVisualStyleBackColor = true;
            this.endTest.Click += new System.EventHandler(this.endTest_Click);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(21, 7);
            this.labelTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(54, 17);
            this.labelTime.TabIndex = 8;
            this.labelTime.Text = "Время:";
            // 
            // appendixForQestion
            // 
            this.appendixForQestion.Location = new System.Drawing.Point(113, 412);
            this.appendixForQestion.Margin = new System.Windows.Forms.Padding(4);
            this.appendixForQestion.Name = "appendixForQestion";
            this.appendixForQestion.Size = new System.Drawing.Size(179, 34);
            this.appendixForQestion.TabIndex = 9;
            this.appendixForQestion.Text = "Приложения к вопросу";
            this.appendixForQestion.UseVisualStyleBackColor = true;
            this.appendixForQestion.Click += new System.EventHandler(this.appendixForQestion_Click);
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(87, 4);
            this.textBoxTime.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(89, 22);
            this.textBoxTime.TabIndex = 10;
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
            // 
            // listBoxQuestions
            // 
            this.listBoxQuestions.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxQuestions.FormattingEnabled = true;
            this.listBoxQuestions.ItemHeight = 16;
            this.listBoxQuestions.Location = new System.Drawing.Point(1008, 12);
            this.listBoxQuestions.Name = "listBoxQuestions";
            this.listBoxQuestions.Size = new System.Drawing.Size(305, 452);
            this.listBoxQuestions.TabIndex = 11;
            this.listBoxQuestions.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxQuestions_DrawItem);
            this.listBoxQuestions.SelectedIndexChanged += new System.EventHandler(this.listBoxQuestions_SelectedIndexChanged);
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 462);
            this.Controls.Add(this.listBoxQuestions);
            this.Controls.Add(this.textBoxTime);
            this.Controls.Add(this.appendixForQestion);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.answerGroupBoxCheckButtons);
            this.Controls.Add(this.endTest);
            this.Controls.Add(this.questionGroupBox);
            this.Controls.Add(this.nextQuestion);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TestingForm";
            this.Text = "Форма тестирования";
            this.Load += new System.EventHandler(this.Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown_Form);
            this.answerGroupBoxCheckButtons.ResumeLayout(false);
            this.answerGroupBoxCheckButtons.PerformLayout();
            this.answerGroupBoxRadioButtons.ResumeLayout(false);
            this.answerGroupBoxRadioButtons.PerformLayout();
            this.questionGroupBox.ResumeLayout(false);
            this.questionGroupBox.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label TextBoxQuestion;
        private System.Windows.Forms.GroupBox answerGroupBoxCheckButtons;
        private System.Windows.Forms.CheckBox answer4;
        private System.Windows.Forms.CheckBox answer3;
        private System.Windows.Forms.CheckBox answer2;
        private System.Windows.Forms.CheckBox answer1;
        private System.Windows.Forms.Button nextQuestion;
        private System.Windows.Forms.GroupBox questionGroupBox;
        private System.Windows.Forms.Button endTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Button appendixForQestion;
        private System.Windows.Forms.GroupBox answerGroupBoxRadioButtons;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxQuestions;
    }
}

