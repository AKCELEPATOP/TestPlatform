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
            maskedTextBox1.Mask = "02 / 00";
            List<string> source = new List<string>
            {
            QuestionComplexity.Easy.ToString(), QuestionComplexity.Middle.ToString(), QuestionComplexity.Difficult.ToString()
            };
            domainUpDown2.Items.AddRange (source);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
