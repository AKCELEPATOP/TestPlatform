using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            else if (Time == 0)
            {
                Time--;
                MessageBox.Show("Время вышло", "Тест завершён", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EndTest();
            }
        }

        private async void EndTest()
        {
            GetAnswer();
            List<QuestionResponseModel> UserAnswers = model.Questions.Select(rec => new QuestionResponseModel
            {
                QuestionId = rec.Id,
                Answers = rec.Answers.Where(r => r.True).Select(r => r.Id).ToList()
            }).ToList();
            StatViewModel result = null;
            try
            {
                result = await ApiClient.PostRequestData<TestResponseModel, StatViewModel>("api/Pattern/CheakTest", new TestResponseModel
                {
                    PatternId = model.PatternId,
                    QuestionResponses = UserAnswers
                });
                FormResultOfTest resultsForm = new FormResultOfTest(result);
                resultsForm.Show();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tmrShow.Stop();
            tmrShow.Dispose();
            Close();
        }

        private async Task<bool> Initialize()
        {
            try
            {

                model = await ApiClient.GetRequestData<TestViewModel>("api/Pattern/CreateTest/" + id);

                if (model == null)
                {
                    MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                listBoxQuestions.DataSource = model.Questions;
                listBoxQuestions.DisplayMember = "Text";
                listBoxQuestions.ValueMember = "Id";
                if (model.Questions.Count < 1)
                {
                    MessageBox.Show("В тесте отсутствуют вопросы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                Time += Convert.ToInt32(model.Time);

                textBoxTime.Text = (Time / 60) + " минут " + (Time % 60) + " секунд ";

                tmrShow = new Timer();
                tmrShow.Interval = 1000;
                tmrShow.Tick += tmrShow_Tick;
                tmrShow.Enabled = true;

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

            if (model.Questions[IdQuestions].Images != null)
            {
                appendixForQestion.Enabled = true;
            }
            return true;
        }

        private void SetNextQuestion()
        {
            questionGroupBox.Text = "Вопрос № " + (listBoxQuestions.SelectedIndex + 1);//чекать

            IdQuestions = listBoxQuestions.SelectedIndex;
            label1.Text = "Категория " + model.Questions[IdQuestions].CategoryName;
            if (IdQuestions < model.Questions.Count)
            {
                QuestionViewModel question;
                if ((question = model.Questions[IdQuestions]).Multi)
                {
                    answerGroupBoxCheckButtons.Enabled = true;
                    answerGroupBoxCheckButtons.Visible = true;
                    answerGroupBoxRadioButtons.Enabled = false;
                    answerGroupBoxRadioButtons.Visible = false;
                    answer1.Text = question.Answers[0].Text;
                    answer2.Text = question.Answers[1].Text;
                    answer3.Text = question.Answers[2].Text;
                    answer4.Text = question.Answers[3].Text;
                    answer1.Checked = question.Answers[0].True;
                    answer2.Checked = question.Answers[1].True;
                    answer3.Checked = question.Answers[2].True;
                    answer4.Checked = question.Answers[3].True;
                }
                else
                {
                    answerGroupBoxRadioButtons.Enabled = true;
                    answerGroupBoxRadioButtons.Visible = true;
                    radioButton1.Text = question.Answers[0].Text;
                    radioButton2.Text = question.Answers[1].Text;
                    radioButton3.Text = question.Answers[2].Text;
                    radioButton4.Text = question.Answers[3].Text;
                    radioButton1.Checked = question.Answers[0].True;
                    radioButton2.Checked = question.Answers[1].True;
                    radioButton3.Checked = question.Answers[2].True;
                    radioButton4.Checked = question.Answers[3].True;
                }
                TextBoxQuestion.Text = question.Text;
                if (question.Images != null && question.Images.Count > 0)
                {
                    appendixForQestion.Enabled = true;
                }
                else
                {
                    appendixForQestion.Enabled = false;
                }
            }

        }


        private void appendixForQestion_Click(object sender, EventArgs e)
        {
            if (model.Questions[IdQuestions].Images != null && model.Questions[IdQuestions].Images.Count > 0)
            {
                var buffer = Convert.FromBase64String(model.Questions[IdQuestions].Images.First().Image);
                HttpPostedFileBase objFile = (HttpPostedFileBase)new MemoryPostedFile(buffer);
                var image = ImageProcessing.ResizeImage(Image.FromStream(objFile.InputStream, true, true),
                    SystemInformation.VirtualScreen.Width / 2, (int)(SystemInformation.VirtualScreen.Height / 1.5));
                AppendixForm appendixForm = new AppendixForm(image)
                {
                    Size = new Size(image.Width, image.Height)
                };
                appendixForm.Show();
            }
        }



        private void endTest_Click(object sender, EventArgs e)
        {
            EndTest();
        }

        private void GetAnswer()
        {
            //List<bool> idAnswers = new List<bool> { false, false, false, false };
            //model.Questions[IdQuestions].Answers = new List<AnswerViewModel>();

            if (model.Questions[IdQuestions].Multi)
            {
                model.Questions[IdQuestions].Answers[0].True = answer1.Checked;
                model.Questions[IdQuestions].Answers[1].True = answer2.Checked;
                model.Questions[IdQuestions].Answers[2].True = answer3.Checked;
                model.Questions[IdQuestions].Answers[3].True = answer4.Checked;
            }
            else
            {
                model.Questions[IdQuestions].Answers[0].True = radioButton1.Checked;
                model.Questions[IdQuestions].Answers[1].True = radioButton2.Checked;
                model.Questions[IdQuestions].Answers[2].True = radioButton3.Checked;
                model.Questions[IdQuestions].Answers[3].True = radioButton4.Checked;
            }


            model.Questions[IdQuestions].Active = true;
        }


        private void nextQuestion_Click(object sender, EventArgs e)
        {
            if (listBoxQuestions.SelectedIndex < model.Questions.Count - 1)
            {
                listBoxQuestions.SelectedIndex++;
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

        private async void Form_Load(object sender, EventArgs e)
        {
            if (!await Initialize())
            {
                Close();
            }
        }

        private void listBoxQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxQuestions.SelectedIndex > -1 && listBoxQuestions.SelectedIndex < model.Questions.Count)
            {
                GetAnswer();
                SetNextQuestion();
                listBoxQuestions.Refresh();
            }
        }


        private SolidBrush reportsForegroundBrushSelected = new SolidBrush(Color.White);
        private SolidBrush reportsForegroundBrush = new SolidBrush(Color.Black);
        private SolidBrush reportsBackgroundBrushSelected = new SolidBrush(Color.FromKnownColor(KnownColor.Highlight));
        private SolidBrush reportsBackgroundBrushSeen = new SolidBrush(Color.DeepSkyBlue);
        private SolidBrush reportsBackgroundBrushAnswered = new SolidBrush(Color.LightSeaGreen);
        private SolidBrush reportsBackgroundBrushNonActive = new SolidBrush(Color.Bisque);

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
                else if ((listBoxQuestions.Items[index] as QuestionViewModel).Answers.Select(rec => rec.True).Where(rec => rec).DefaultIfEmpty(false).FirstOrDefault())
                    backgroundBrush = reportsBackgroundBrushAnswered;
                else if ((listBoxQuestions.Items[index] as QuestionViewModel).Active)
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
