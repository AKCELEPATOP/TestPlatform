using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        public FormQuestionEditor()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {

            if (id.HasValue) {
                try
                {
                    var question = Task.Run(() => ApiClient.GetRequestData<QuestionViewModel>("api/Question/Get/" + id.Value)).Result;
                    textBox6.Text = question.Text;

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
            maskedTextBox1.Text = "02 / 00";
            List<string> source = new List<string>
            {
            QuestionComplexity.Easy.ToString(), QuestionComplexity.Middle.ToString(), QuestionComplexity.Difficult.ToString()
            };
            domainUpDown2.Items.AddRange (source);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text)&& string.IsNullOrEmpty(textBox3.Text)&& string.IsNullOrEmpty(textBox4.Text)&& string.IsNullOrEmpty(textBox5.Text)
                && string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Task task;
            string text = textBox1.Text;
            List<string> listanswer = new List<string>() {
                textBox2.Text,textBox3.Text,textBox4.Text,textBox5.Text
            };
            int idTrue=-1;
            if (checkBox1.Checked)
            {
                idTrue = 0;
            }
            else if (checkBox2.Checked)
            {
                idTrue = 1;
            }
            else if (checkBox3.Checked)
            {
                idTrue = 2;
            }
            else if (checkBox4.Checked)
            {
                idTrue = 3;
            }
            else {
                MessageBox.Show("Выберите верный ответ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bool active = checkBox5.Checked;
            
            string[] timestr = maskedTextBox1.ToString().Split('/');
            long time = Convert.ToInt32(timestr[0]) / 60 + Convert.ToInt32(timestr[1]);
            QuestionComplexity complexity = (QuestionComplexity)Enum.Parse(typeof(QuestionComplexity), domainUpDown2.SelectedItem.ToString(), true);
            List<Answer> answers = new List<Answer>(4);
            for (int i = 0; i < 4; i++) {
                if (i == idTrue) {
                     answers.Add(new Answer
                     {
                         Text = listanswer[i],
                         True =true
                });
                } else {
                    answers.Add(new Answer {
                        Text = listanswer[i],
                        True =false
    
                });
                }
            } 
            if (id.HasValue)
            {
                task = Task.Run(() => ApiClient.PostRequestData("api/Question/UpdElement", new QuestionBindingModel
                {
                    Id = id.Value,
                    Text= text,
                    Complexity=complexity,
                    Active=active,
                    Time= time,
                    Answers=answers

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
                    Answers = answers

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
    }
}
