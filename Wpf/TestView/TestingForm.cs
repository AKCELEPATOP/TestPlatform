using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.ViewModels;

namespace TestView
{
    public partial class TestingForm : Form
    {

        public int Id { set { id = value; } }

        private int? id;

        List<TestViewModel> list;
        private readonly Timer tmrShow;
        int IdQuestions = 0;
        int Amount = 0;
        int Time;


        public TestingForm()
        {
            InitializeComponent();
            Initialize();

            list =
                     Task.Run(() => ApiClient.GetRequestData<List<TestViewModel>>("api/Questions/GetList")).Result;
            if (list != null)
            {
                questionList.DataSource = list;
                questionList.Columns[0].Visible = false;
                questionList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


            Time += Convert.ToInt32(list[id.Value].Time);

            textBoxTime.Text = (Time / 60) + " минут " + (Time % 60) + " секунд ";

            tmrShow = new Timer();
            tmrShow.Interval = 5000;
            tmrShow.Tick += tmrShow_Tick;
            tmrShow.Enabled = true;
            label1.Text = "Категория " + list[id.Value].Questions[IdQuestions].CategoryName;
            questionGroupBox.Text = "Вопрос № " + IdQuestions;


        }


        private void tmrShow_Tick(object sender, EventArgs e)
        {
            if (Time > 0)
            {
                Time--;
                textBoxTime.Text = (Time / 60) + " минут " + (Time % 60) + " секунд ";
            }
            else
            {
                MessageBox.Show("Время вышло", "Тест завершён", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormResultOfTest resultsForm = new FormResultOfTest();
                Close();
                resultsForm.Show();
            }
        }

        private void Initialize()
        {
            try
            {
                IdQuestions = Convert.ToInt32(questionList.SelectedRows[0].Cells[0].Value);

                for (int i = 0; i < list[id.Value].Questions.Count; i++)
                {
                    if (list[id.Value].Questions[IdQuestions].Answers[i].True) Amount++;
                }
                if (Amount > 1)
                {
                    answerGroupBoxCheckButtons.Enabled = true;
                    answerGroupBoxCheckButtons.Visible = true;
                    answer1.Text = list[id.Value].Questions[IdQuestions].Answers[0].Text;
                    answer2.Text = list[id.Value].Questions[IdQuestions].Answers[1].Text;
                    answer3.Text = list[id.Value].Questions[IdQuestions].Answers[2].Text;
                    answer4.Text = list[id.Value].Questions[IdQuestions].Answers[3].Text;
                }
                else
                {
                    answerGroupBoxRadioButtons.Enabled = true;
                    answerGroupBoxRadioButtons.Visible = true;
                    radioButton1.Text = list[id.Value].Questions[IdQuestions].Answers[0].Text;
                    radioButton2.Text = list[id.Value].Questions[IdQuestions].Answers[1].Text;
                    radioButton3.Text = list[id.Value].Questions[IdQuestions].Answers[2].Text;
                    radioButton4.Text = list[id.Value].Questions[IdQuestions].Answers[3].Text;
                }
                question.Text = list[id.Value].Questions[IdQuestions].Text;


            }

            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void appendixForQestion_Click(object sender, EventArgs e)
        {
            // Пока не реализовано
            AppendixForm appendixForm = new AppendixForm();
            appendixForm.Show();
        }

        private void endTest_Click(object sender, EventArgs e)
        {
            FormResultOfTest resultsForm = new FormResultOfTest();
            Close();
            resultsForm.Show();
        }

        private void GetAnswer()
        {
            List<bool> idAnswers = new List<bool> { false, false, false, false };
            if (IdQuestions < list.Count)
            {
                if (Amount > 1)
                {
                    if (answer1.Checked) idAnswers[0] = true;
                    if (answer2.Checked) idAnswers[1] = true;
                    if (answer3.Checked) idAnswers[2] = true;
                    if (answer4.Checked) idAnswers[3] = true;
                }
                else
                {
                    if (radioButton1.Checked) idAnswers[0] = true;
                    if (radioButton2.Checked) idAnswers[1] = true;
                    if (radioButton3.Checked) idAnswers[2] = true;
                    if (radioButton4.Checked) idAnswers[3] = true;
                }

                for (int i = 0; i < 4; i++)
                {
                    list[id.Value].Questions[IdQuestions].Answers[i].True = idAnswers[i];
                }
            }

            if (idAnswers.FirstOrDefault(rec => rec.Equals(true)))
            {
               
                questionList.Rows[IdQuestions].DefaultCellStyle.BackColor = Color.Blue;
            }
            else 
            {
                questionList.Rows[IdQuestions].DefaultCellStyle.BackColor = Color.Yellow;
            }
        }


        private void nextQuestion_Click(object sender, EventArgs e)
        {

            if (IdQuestions < list.Count)
            {
                IdQuestions++;
                GetAnswer();
                Initialize();
            }
        }


        private void questionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (questionList.SelectedRows.Count == 1)
            {
                IdQuestions = Convert.ToInt32(questionList.SelectedRows[0].Cells[0].Value);
                GetAnswer();
                Initialize();
            }
        }

    }
}
