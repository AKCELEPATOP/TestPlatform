using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestModels;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormQuestionEditor : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public int IdCat { set { idCat = value; } }

        private int? idCat;

        private List<int> answerIds;

        List<AttachmentBindingModel> attachment;



        public FormQuestionEditor()
        {
            InitializeComponent();
        }

        public async void Initialize()
        {
            if (id.HasValue)
            {
                try
                {
                    var question = await ApiClient.GetRequestData<QuestionViewModel>("api/Question/Get/" + id.Value);
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
                    answerIds = new List<int>
                    {
                        question.Answers[0].Id,
                        question.Answers[1].Id,
                        question.Answers[2].Id,
                        question.Answers[3].Id
                    };
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
            if (idCat.HasValue)
            {
                try
                {
                    var category = await ApiClient.GetRequestData<CategoryViewModel>("api/Category/Get/" + idCat.Value);
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

            maskedTextBox1.Mask = "00:00";
            maskedTextBox1.Text = "02:00";
            string[] source = new string[] //пофиксить
            {
            QuestionComplexity.Easy.ToString(), QuestionComplexity.Middle.ToString(), QuestionComplexity.Difficult.ToString()
            };
            comboBox1.Items.AddRange(source);
            comboBox1.Text = QuestionComplexity.Easy.ToString();
            //   comboBox1.e
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text)
                || string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                return;
            }
            bool active = !checkBox5.Checked;
            if (!Regex.IsMatch(maskedTextBox1.Text.ToString(), @"\d{2}[:]\d{2}") || maskedTextBox1.Text == null)
            {
                MessageBox.Show("Формат времени неверный", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] timestr = maskedTextBox1.Text.ToString().Split(':');
            long time = Convert.ToInt32(timestr[0]) * 60 + Convert.ToInt32(timestr[1]);
            QuestionComplexity complexity = (QuestionComplexity)Enum.Parse(typeof(QuestionComplexity), comboBox1.SelectedItem.ToString(), true);
            List<AnswerBindingModel> answers = new List<AnswerBindingModel>(4);
            for (int i = 0; i < answersString.Count; i++)
            {
                if (answerIds != null)
                {
                    answers.Add(new AnswerBindingModel
                    {
                        Id = answerIds[i],
                        Text = answersString[i],
                        True = checkBoxes[i],
                    });
                }
                else
                {
                    answers.Add(new AnswerBindingModel
                    {
                        Text = answersString[i],
                        True = checkBoxes[i],
                    });
                }
            }
            try
            {
                if (id.HasValue)
                {
                    if (attachment != null)
                    {
                        await ApiClient.PostRequestData("api/Question/UpdElement", new QuestionBindingModel
                        {
                            Id = id.Value,
                            Text = text,
                            Complexity = complexity,
                            Active = active,
                            Time = time,
                            Answers = answers,
                            CategoryId = idCat.Value,
                            Attachments = attachment
                        });
                    }
                    else
                    {
                        await ApiClient.PostRequestData("api/Question/UpdElement", new QuestionBindingModel
                        {
                            Id = id.Value,
                            Text = text,
                            Complexity = complexity,
                            Active = active,
                            Time = time,
                            Answers = answers,
                            CategoryId = idCat.Value
                        });
                    }
                }
                else
                {
                    if (attachment != null)
                    {
                        await ApiClient.PostRequestData("api/Question/AddElement", new QuestionBindingModel
                        {

                            Text = text,
                            Complexity = complexity,
                            Active = active,
                            Time = time,
                            Answers = answers,
                            CategoryId = idCat.Value,
                            Attachments = attachment
                        });
                    }
                    else
                    {
                        await ApiClient.PostRequestData("api/Question/AddElement", new QuestionBindingModel
                        {                           
                            Text = text,
                            Complexity = complexity,
                            Active = active,
                            Time = time,
                            Answers = answers,
                            CategoryId = idCat.Value
                        });
                    }
                }
                MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            byte[] attachmentForQuestion = System.IO.File.ReadAllBytes(folderBrowserDialog.SelectedPath);
            attachment[0].Image = Convert.ToBase64String(attachmentForQuestion);
            attachment[0].Id = 1;
        }
    }
}
