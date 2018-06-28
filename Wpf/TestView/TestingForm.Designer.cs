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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestingForm));
            this.questionList = new System.Windows.Forms.DataGridView();
            this.question = new System.Windows.Forms.Label();
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
            this.giveAnswer = new System.Windows.Forms.Button();
            this.nextQuestion = new System.Windows.Forms.Button();
            this.questionGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.endTest = new System.Windows.Forms.Button();
            this.time = new System.Windows.Forms.Label();
            this.appendixForQestion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.questionList)).BeginInit();
            this.answerGroupBoxCheckButtons.SuspendLayout();
            this.answerGroupBoxRadioButtons.SuspendLayout();
            this.questionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // questionList
            // 
            this.questionList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.questionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.questionList.Location = new System.Drawing.Point(794, 0);
            this.questionList.Name = "questionList";
            this.questionList.Size = new System.Drawing.Size(190, 374);
            this.questionList.TabIndex = 0;
            this.questionList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.questionList_CellContentClick);
            // 
            // question
            // 
            this.question.Location = new System.Drawing.Point(6, 29);
            this.question.Name = "question";
            this.question.Size = new System.Drawing.Size(696, 75);
            this.question.TabIndex = 2;
            this.question.Text = resources.GetString("question.Text");
            // 
            // answerGroupBoxCheckButtons
            // 
            this.answerGroupBoxCheckButtons.Controls.Add(this.answerGroupBoxRadioButtons);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer4);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer3);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer2);
            this.answerGroupBoxCheckButtons.Controls.Add(this.answer1);
            this.answerGroupBoxCheckButtons.Enabled = false;
            this.answerGroupBoxCheckButtons.Location = new System.Drawing.Point(43, 151);
            this.answerGroupBoxCheckButtons.Name = "answerGroupBoxCheckButtons";
            this.answerGroupBoxCheckButtons.Size = new System.Drawing.Size(708, 165);
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
            this.answerGroupBoxRadioButtons.Name = "answerGroupBoxRadioButtons";
            this.answerGroupBoxRadioButtons.Size = new System.Drawing.Size(708, 165);
            this.answerGroupBoxRadioButtons.TabIndex = 4;
            this.answerGroupBoxRadioButtons.TabStop = false;
            this.answerGroupBoxRadioButtons.Text = "Варианты ответов:";
            this.answerGroupBoxRadioButtons.Visible = false;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(27, 127);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(113, 17);
            this.radioButton4.TabIndex = 7;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Вариант ответа 4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(27, 92);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(113, 17);
            this.radioButton3.TabIndex = 6;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Вариант ответа 3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(27, 60);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(113, 17);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Вариант ответа 2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(27, 29);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(113, 17);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Вариант ответа 1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // answer4
            // 
            this.answer4.AutoSize = true;
            this.answer4.Location = new System.Drawing.Point(27, 128);
            this.answer4.Name = "answer4";
            this.answer4.Size = new System.Drawing.Size(114, 17);
            this.answer4.TabIndex = 3;
            this.answer4.Text = "Вариант ответа 4";
            this.answer4.UseVisualStyleBackColor = true;
            // 
            // answer3
            // 
            this.answer3.AutoSize = true;
            this.answer3.Location = new System.Drawing.Point(27, 93);
            this.answer3.Name = "answer3";
            this.answer3.Size = new System.Drawing.Size(114, 17);
            this.answer3.TabIndex = 2;
            this.answer3.Text = "Вариант ответа 3";
            this.answer3.UseVisualStyleBackColor = true;
            // 
            // answer2
            // 
            this.answer2.AutoSize = true;
            this.answer2.Location = new System.Drawing.Point(27, 61);
            this.answer2.Name = "answer2";
            this.answer2.Size = new System.Drawing.Size(114, 17);
            this.answer2.TabIndex = 1;
            this.answer2.Text = "Вариант ответа 2";
            this.answer2.UseVisualStyleBackColor = true;
            // 
            // answer1
            // 
            this.answer1.AutoSize = true;
            this.answer1.Location = new System.Drawing.Point(27, 29);
            this.answer1.Name = "answer1";
            this.answer1.Size = new System.Drawing.Size(114, 17);
            this.answer1.TabIndex = 0;
            this.answer1.Text = "Вариант ответа 1";
            this.answer1.UseVisualStyleBackColor = true;
            // 
            // giveAnswer
            // 
            this.giveAnswer.Location = new System.Drawing.Point(52, 335);
            this.giveAnswer.Name = "giveAnswer";
            this.giveAnswer.Size = new System.Drawing.Size(134, 28);
            this.giveAnswer.TabIndex = 4;
            this.giveAnswer.Text = "Ответить";
            this.giveAnswer.UseVisualStyleBackColor = true;
            this.giveAnswer.Click += new System.EventHandler(this.answering_Click);
            // 
            // nextQuestion
            // 
            this.nextQuestion.Location = new System.Drawing.Point(416, 336);
            this.nextQuestion.Name = "nextQuestion";
            this.nextQuestion.Size = new System.Drawing.Size(134, 27);
            this.nextQuestion.TabIndex = 5;
            this.nextQuestion.Text = "Следующий вопрос";
            this.nextQuestion.UseVisualStyleBackColor = true;
            this.nextQuestion.Click += new System.EventHandler(this.nextQuestion_Click);
            // 
            // questionGroupBox
            // 
            this.questionGroupBox.Controls.Add(this.label1);
            this.questionGroupBox.Controls.Add(this.question);
            this.questionGroupBox.Location = new System.Drawing.Point(43, 34);
            this.questionGroupBox.Name = "questionGroupBox";
            this.questionGroupBox.Size = new System.Drawing.Size(708, 111);
            this.questionGroupBox.TabIndex = 6;
            this.questionGroupBox.TabStop = false;
            this.questionGroupBox.Text = "Вопрос № #";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(509, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Категория: #";
            // 
            // endTest
            // 
            this.endTest.Location = new System.Drawing.Point(593, 336);
            this.endTest.Name = "endTest";
            this.endTest.Size = new System.Drawing.Size(134, 27);
            this.endTest.TabIndex = 7;
            this.endTest.Text = "Завершить тест";
            this.endTest.UseVisualStyleBackColor = true;
            this.endTest.Click += new System.EventHandler(this.endTest_Click);
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.Location = new System.Drawing.Point(16, 6);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(53, 13);
            this.time.TabIndex = 8;
            this.time.Text = "Время: #";
            // 
            // appendixForQestion
            // 
            this.appendixForQestion.Location = new System.Drawing.Point(240, 335);
            this.appendixForQestion.Name = "appendixForQestion";
            this.appendixForQestion.Size = new System.Drawing.Size(134, 28);
            this.appendixForQestion.TabIndex = 9;
            this.appendixForQestion.Text = "Приложения к вопросу";
            this.appendixForQestion.UseVisualStyleBackColor = true;
            this.appendixForQestion.Click += new System.EventHandler(this.appendixForQestion_Click);
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 375);
            this.Controls.Add(this.appendixForQestion);
            this.Controls.Add(this.time);
            this.Controls.Add(this.answerGroupBoxCheckButtons);
            this.Controls.Add(this.endTest);
            this.Controls.Add(this.questionGroupBox);
            this.Controls.Add(this.nextQuestion);
            this.Controls.Add(this.giveAnswer);
            this.Controls.Add(this.questionList);
            this.Name = "TestingForm";
            this.Text = "Форма тестирования";
            ((System.ComponentModel.ISupportInitialize)(this.questionList)).EndInit();
            this.answerGroupBoxCheckButtons.ResumeLayout(false);
            this.answerGroupBoxCheckButtons.PerformLayout();
            this.answerGroupBoxRadioButtons.ResumeLayout(false);
            this.answerGroupBoxRadioButtons.PerformLayout();
            this.questionGroupBox.ResumeLayout(false);
            this.questionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView questionList;
        private System.Windows.Forms.Label question;
        private System.Windows.Forms.GroupBox answerGroupBoxCheckButtons;
        private System.Windows.Forms.CheckBox answer4;
        private System.Windows.Forms.CheckBox answer3;
        private System.Windows.Forms.CheckBox answer2;
        private System.Windows.Forms.CheckBox answer1;
        private System.Windows.Forms.Button giveAnswer;
        private System.Windows.Forms.Button nextQuestion;
        private System.Windows.Forms.GroupBox questionGroupBox;
        private System.Windows.Forms.Button endTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Button appendixForQestion;
        private System.Windows.Forms.GroupBox answerGroupBoxRadioButtons;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

