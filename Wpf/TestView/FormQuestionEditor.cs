using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestModels;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormQuestionEditor : MetroForm
    {
        public int Id { set { id = value; } }

        private int? id;

        public int IdCat { set { idCat = value; } }

        private int? idCat;

        public FormQuestionEditor()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            ShadowType = MetroFormShadowType.DropShadow;

            Initialize();
        }

        public void Initialize() {

            if (id.HasValue) {
                try
                {
                    var question = Task.Run(() => ApiClient.GetRequestData<QuestionViewModel>("api/Question/Get/" + id.Value)).Result;
                    textBox6.Text = question.Text;
                    textBox2.Text = question.Answers[0].Text;
                    textBox3.Text = question.Answers[1].Text;
                    textBox4.Text = question.Answers[2].Text;
                    textBox5.Text = question.Answers[3].Text;
                    checkBox1.Checked = question.Answers[0].True;
                    checkBox2.Checked = question.Answers[1].True;
                    checkBox3.Checked = question.Answers[2].True;
                    checkBox4.Checked = question.Answers[3].True;
                    checkBox5.Checked = !question.Active;
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
            if (idCat.HasValue) {
                try
                {
                    var category = Task.Run(() => ApiClient.GetRequestData<CategoryViewModel>("api/Category/Get/" + idCat.Value)).Result;
                    textBox1.Text = category.Name;
                    textBox1.Enabled = false;
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

            maskedTextBox1.Mask = "00 / 00";
            maskedTextBox1.Text = "02/00";
            string[] source = new string[]
            {
            QuestionComplexity.Easy.ToString(), QuestionComplexity.Middle.ToString(), QuestionComplexity.Difficult.ToString()
            };
            comboBox1.Items.AddRange (source);
            comboBox1.Text=QuestionComplexity.Easy.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrEmpty(textBox4.Text) && string.IsNullOrEmpty(textBox5.Text)
                && string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Task task;
            string text = textBox6.Text;
            List<bool> checkBoxes = new List<bool>
            {
                checkBox1.Checked,
                checkBox2.Checked,
                checkBox3.Checked,
                checkBox4.Checked
            };
            List<string> answersString = new List<string>
            {
                textBox2.Text,
                textBox3.Text,
                textBox4.Text,
                textBox5.Text
            };
            if (!checkBoxes.Contains(true))
            {
                MessageBox.Show("Выберите верный ответ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bool active = !checkBox5.Checked;

            string[] timestr = maskedTextBox1.Text.ToString().Split('.');
            long time = Convert.ToInt32(timestr[0]) * 60 + Convert.ToInt32(timestr[1]);
            QuestionComplexity complexity = (QuestionComplexity)Enum.Parse(typeof(QuestionComplexity), comboBox1.SelectedItem.ToString(), true);
            List<AnswerBindingModel> answers = new List<AnswerBindingModel>(4);
            for (int i = 0; i < answersString.Count; i++)
            {
                answers.Add(new AnswerBindingModel
                {
                    //сохранять id вопросов при редактировании
                    Text = answersString[i],
                    True = checkBoxes[i],
                });
            }
            if (id.HasValue)
            {
                task = Task.Run(() => ApiClient.PostRequestData("api/Question/UpdElement", new QuestionBindingModel
                {
                    Id = id.Value,
                    Text = text,
                    Complexity = complexity,
                    Active = active,
                    Time = time,
                    Answers = answers,
                    CategoryId = idCat.Value
                }));
            }
            else
            {
                task = Task.Run(() => ApiClient.PostRequestData("api/Question/AddElement", new QuestionBindingModel
                {

                    Text = text,
                    Complexity = complexity,
                    Active = active,
                    Time = time,
                    Answers = answers,
                    CategoryId = idCat.Value

                }));

            }
            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
