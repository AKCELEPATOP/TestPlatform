using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
            string[] source = new string[] //пофиксить
           {
            QuestionComplexity.Easy.ToString(), QuestionComplexity.Middle.ToString(), QuestionComplexity.Difficult.ToString()
           };
            comboBox1.Items.AddRange(source);
            comboBox1.Text = QuestionComplexity.Easy.ToString();
            maskedTextBox1.Mask = "00:00";
            maskedTextBox1.Text = "02:00";
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
                    comboBox1.Text = question.ComplexityName;
                    var min = (int)question.Time / 60.0;
                    var sec = question.Time - min;
                    maskedTextBox1.Text = string.Format("{0}:{1}", ((min > 9) ? min.ToString() : string.Format("0{0}", min)), ((sec > 9) ? sec.ToString() : string.Format("0{0}", sec)));
                    answerIds = new List<int>
                    {
                        question.Answers[0].Id,
                        question.Answers[1].Id,
                        question.Answers[2].Id,
                        question.Answers[3].Id
                    };
                    if (question.Images != null && question.Images.Count > 0)
                    {
                        attachment = question.Images.Select(rec => new AttachmentBindingModel
                        {
                            Id = rec.Id,
                            Image = rec.Image
                        }).ToList();
                        buttonShow.Enabled = true;
                    }
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
            if (time < 30)
            {
                MessageBox.Show("Нужно больше времени", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            checkBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            checkBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);

            button1.BackColor = Design.Invert(this.ForeColor);
            button2.BackColor = Design.Invert(this.ForeColor);
            button3.BackColor = Design.Invert(this.ForeColor);
            buttonShow.BackColor = Design.Invert(this.ForeColor);
            comboBox1.BackColor = Design.Invert(this.ForeColor);
        }

        private void button3_Click(object sender, EventArgs e)
        {


            attachment = new List<AttachmentBindingModel>();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // string path = Path.GetFileName(openFileDialog1.FileName);
                    byte[] attachmentForQuestion = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                    if (attachment.Count < 1)
                    {
                        attachment.Add(new AttachmentBindingModel
                        {
                            Image = Convert.ToBase64String(attachmentForQuestion)
                        });
                    }
                    else
                    {
                        attachment.Insert(0, new AttachmentBindingModel
                        {
                            Image = Convert.ToBase64String(attachmentForQuestion)
                        });
                    }
                    MessageBox.Show("Изображение добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonShow.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (attachment != null && attachment.Count > 0)
            {
                var buffer = Convert.FromBase64String(attachment[0].Image);
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
    }
}
