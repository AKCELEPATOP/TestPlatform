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
        public TestingForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
           // if () Проверка на количество правильных вариантов ответа

            try
            {
                List<QuestionViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<QuestionViewModel>>("api/Questions/GetList")).Result;
                if (list != null)
                {
                    questionList.DataSource = list;
                    questionList.Columns[0].Visible = false;
                    questionList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private void answering_Click(object sender, EventArgs e)
        {

        }

        private void appendixForQestion_Click(object sender, EventArgs e)
        {
            AppendixForm appendixForm = new AppendixForm();
            appendixForm.Show();
        }

        private void endTest_Click(object sender, EventArgs e)
        {
            FormResultOfTest resultsForm = new FormResultOfTest();
            Close();
            resultsForm.Show();
        }

        private void nextQuestion_Click(object sender, EventArgs e)
        {

        }

        private void questionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (questionList.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(questionList.SelectedRows[0].Cells[0].Value);
                List<QuestionViewModel> list =
                Task.Run(() => ApiClient.GetRequestData<List<QuestionViewModel>>("api/Question/GetList/" + Id)).Result;
                if (list != null)
                {
                    questionList.DataSource = list;
                    questionList.Columns[0].Visible = false;
                    questionList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
    }
}
