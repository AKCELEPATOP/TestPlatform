using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class TestingForm : Form
    {

        public int Id { set { id = value; } }

        private int? id;

        private Timer tmrShow;

        int IdQuestions = 0;

        int Amount = 0;

        int Time;

        private TestViewModel model;

        public TestingForm()
        {
            InitializeComponent();
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
                EndTest();
            }
        }

        private async void EndTest()
        {
            List<QuestionResponseModel> UserAnswers = new List<QuestionResponseModel>();
            for (int i = 0; i < model.Questions.Count; i++)
            {
                UserAnswers.Add(new QuestionResponseModel
                {
                    QuestionId = model.Questions[i].Id,
                    Answers = model.Questions[i].Answers.Select(rec => rec.Id).ToList()
                });
            }
            try
            {
                var result = await ApiClient.PostRequestData<TestResponseModel, StatViewModel>("api/Pattern/CheakTest", new TestResponseModel
                {
                    PatternId = model.PatternId,
                    QuestionResponses = UserAnswers
                });
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            FormResultOfTest resultsForm = new FormResultOfTest();
            Close();
            resultsForm.Show();
        }

        private void Initialize()
        {
            try
            {

                model = Task.Run(() => ApiClient.GetRequestData<TestViewModel>("api/Pattern/CreateTest/" + id)).Result;
                if (model != null)
                {
                    listBoxQuestions.DataSource = model.Questions;
                    listBoxQuestions.DisplayMember = "Text";
                    listBoxQuestions.ValueMember = "Id";
                }


                Time += Convert.ToInt32(model.Time);

                textBoxTime.Text = (Time / 60) + " минут " + (Time % 60) + " секунд ";

                tmrShow = new Timer();
                tmrShow.Interval = 5000;
                tmrShow.Tick += tmrShow_Tick;
                tmrShow.Enabled = true;
                //label1.Text = "Категория " + list[id.Value].Questions[IdQuestions].CategoryName;    
                /////////////////////
                SetNextQuestion();


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

        private void SetNextQuestion()
        {
            questionGroupBox.Text = "Вопрос № " + listBoxQuestions.SelectedIndex;//чекать

            IdQuestions = Convert.ToInt32(Convert.ToInt32(listBoxQuestions.SelectedIndex));

            QuestionViewModel question;
            if ((question = model.Questions.FirstOrDefault(rec => rec.Id == IdQuestions)).Multi)
            {
                answerGroupBoxCheckButtons.Enabled = true;
                answerGroupBoxCheckButtons.Visible = true;
                answer1.Text = question.Answers[0].Text;
                answer2.Text = question.Answers[1].Text;
                answer3.Text = question.Answers[2].Text;
                answer4.Text = question.Answers[3].Text;
            }
            else
            {
                answerGroupBoxRadioButtons.Enabled = true;
                answerGroupBoxRadioButtons.Visible = true;
                radioButton1.Text = question.Answers[0].Text;
                radioButton2.Text = question.Answers[1].Text;
                radioButton3.Text = question.Answers[2].Text;
                radioButton4.Text = question.Answers[3].Text;
            }
            question.Text = question.Text;
        }


        private void appendixForQestion_Click(object sender, EventArgs e)
        {
            // Пока не реализовано
            AppendixForm appendixForm = new AppendixForm();
            appendixForm.Show();
        }

        private void endTest_Click(object sender, EventArgs e)
        {
            EndTest();
        }

        private void GetAnswer()
        {
            //List<bool> idAnswers = new List<bool> { false, false, false, false };
            //model.Questions[IdQuestions].Answers = new List<AnswerViewModel>();
            if (IdQuestions < model.Questions.Count)
            {
                if (Amount > 1)
                {
                    if (answer1.Checked) model.Questions[IdQuestions].Answers[0].True = true;
                    if (answer2.Checked) model.Questions[IdQuestions].Answers[1].True = true;
                    if (answer3.Checked) model.Questions[IdQuestions].Answers[2].True = true;
                    if (answer4.Checked) model.Questions[IdQuestions].Answers[3].True = true;
                }
                else
                {
                    if (radioButton1.Checked) model.Questions[IdQuestions].Answers[0].True = true;
                    if (radioButton2.Checked) model.Questions[IdQuestions].Answers[1].True = true;
                    if (radioButton3.Checked) model.Questions[IdQuestions].Answers[2].True = true;
                    if (radioButton4.Checked) model.Questions[IdQuestions].Answers[3].True = true;
                }
            }

            model.Questions[IdQuestions].Active = true;
        }


        private void nextQuestion_Click(object sender, EventArgs e)
        {

            if (IdQuestions < model.Questions.Count)
            {
                GetAnswer();
                SetNextQuestion();
            }
        }

        // ПКМ -> Обновить
        private void MouseDown_Form(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void listBoxQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAnswer();
            SetNextQuestion();
        }


        private SolidBrush reportsForegroundBrushSelected = new SolidBrush(Color.White);
        private SolidBrush reportsForegroundBrush = new SolidBrush(Color.Black);
        private SolidBrush reportsBackgroundBrushSelected = new SolidBrush(Color.FromKnownColor(KnownColor.Highlight));
        private SolidBrush reportsBackgroundBrushSeen = new SolidBrush(Color.DeepSkyBlue);
        private SolidBrush reportsBackgroundBrushAnswered = new SolidBrush(Color.LightSeaGreen);
        private SolidBrush reportsBackgroundBrushNonActive = new SolidBrush(Color.MintCream);

        private void listBoxQuestions_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;

            if (index >= 0 && index < listBoxQuestions.Items.Count)
            {
                string text = listBoxQuestions.Items[index].ToString();

                Graphics g = e.Graphics;

                SolidBrush backgroundBrush;

                if (selected)
                    backgroundBrush = reportsBackgroundBrushSelected;
                else if ((listBoxQuestions.Items[index] as QuestionViewModel).Answers.Select(rec => rec.True).FirstOrDefault())
                    backgroundBrush = reportsBackgroundBrushAnswered;
                else if((listBoxQuestions.Items[index] as QuestionViewModel).Active)
                    backgroundBrush = reportsBackgroundBrushSeen;
                else
                    backgroundBrush = reportsBackgroundBrushNonActive;
                g.FillRectangle(backgroundBrush, e.Bounds);

                
                SolidBrush foregroundBrush = (selected) ? reportsForegroundBrushSelected : reportsForegroundBrush;
                g.DrawString(text, e.Font, foregroundBrush, listBoxQuestions.GetItemRectangle(index).Location);

                e.DrawFocusRectangle();
            }
        }
    }
}
