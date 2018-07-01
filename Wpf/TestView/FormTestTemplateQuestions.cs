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
    public partial class FormTestTemplateQuestions : Form
    {
        public List<PatternCategoryViewModel> listPC { get; set; }
        List<QuestionViewModel> listQ;

        private BindingSource sourceQ;

        private BindingSource sourcePQ;
        public FormTestTemplateQuestions()
        {
            listPC = new List<PatternCategoryViewModel>();
            sourcePQ = new BindingSource();
            sourceQ = new BindingSource();
            InitializeComponent();
        }
        private void Initialize()
        {
            listQ = Task.Run(() => ApiClient.GetRequestData<List<QuestionViewModel>>("api/category/GetListQuestions/" + listPC[0].CategoryId)).Result;
            if (listPC != null)
            {
                dataGridViewCategories.DataSource = listPC;
                dataGridViewCategories.Columns[0].Visible = false;
                dataGridViewCategories.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (listPC[0].PatternQuestions != null)
            {
                sourcePQ.DataSource = listPC[0].PatternQuestions;
                dataGridViewTestQuestions.DataSource = sourcePQ;
                dataGridViewTestQuestions.Columns[0].Visible = false;
                dataGridViewTestQuestions.Columns[1].Visible = false;
                dataGridViewTestQuestions.Columns[2].Visible = false;
                dataGridViewTestQuestions.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (listQ != null)
            {
                sourceQ.DataSource = listQ;
                dataGridViewQuestions.DataSource = sourceQ;
                dataGridViewQuestions.Columns[0].Visible = false;
                dataGridViewQuestions.Columns[1].Visible = false;
                dataGridViewQuestions.Columns[3].Visible = false;
                dataGridViewQuestions.Columns[9].Visible = false;
                dataGridViewQuestions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        //>
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewQuestions.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells[0].Value);
                var question = listQ.FirstOrDefault(rec => rec.Id == id);
                if (dataGridViewCategories.SelectedRows.Count == 1)
                {
                    int categoryId = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells[0].Value);
                    var PQ = listPC.FirstOrDefault(rec => rec.CategoryId == categoryId).PatternQuestions;
                    if (!PQ.Select(rec => rec.QuestionId).Contains(question.Id))
                    {
                        PQ.Add(new PatternQuestionViewModel
                        {
                            QuestionId = question.Id,
                            QuestionText = question.Text,
                            Complexity = question.ComplexityName,
                        });
                        sourcePQ.ResetBindings(false);
                    }
                }
            }
        }
        //>>
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count == 1)
            {
                int categoryId = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells[0].Value);
                var PQ = listPC.FirstOrDefault(rec => rec.Id == categoryId).PatternQuestions;
                foreach (var question in listQ)
                {
                    if (!PQ.Select(rec => rec.QuestionId).Contains(question.Id))
                    {
                        PQ.Add(new PatternQuestionViewModel
                        {
                            QuestionId = question.Id,
                            QuestionText = question.Text,
                            Complexity = question.ComplexityName,
                        });
                    }
                }
                sourcePQ.ResetBindings(false);
            }
        }
        //<
        private void button6_Click(object sender, EventArgs e)
        {

            if (dataGridViewCategories.SelectedRows.Count == 1)
            {
                int categoryId = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells[0].Value);
                var PQ = listPC.FirstOrDefault(rec => rec.Id == categoryId).PatternQuestions;
                int questionId = Convert.ToInt32(dataGridViewTestQuestions.SelectedRows[0].Cells[2].Value);
                PQ.Remove(PQ.FirstOrDefault(rec => rec.QuestionId == questionId));
                sourcePQ.ResetBindings(false);
            }
        }
        //<<
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count == 1)
            {
                int categoryId = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells[0].Value);
                var PQ = listPC.FirstOrDefault(rec => rec.Id == categoryId).PatternQuestions;
                PQ.Clear();
                sourcePQ.ResetBindings(false);
            }
        }
        //save
        private void button3_Click(object sender, EventArgs e)
        {
            var form = new FormTestTemplate();
            //form.listQ = listQ;
            if (form.ShowDialog() == DialogResult.OK)
            {
                Close();
            }
        }
        //nazad
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ПКМ -> Обновить
        private void MouseDown_Form(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }
        private void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private async void dataGridViewCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count == 1)
            {
                int categoryId = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells[0].Value);
                var PC = listPC.FirstOrDefault(rec => rec.CategoryId == categoryId);
                sourcePQ.DataSource = PC.PatternQuestions;
                try
                {
                    listQ = await ApiClient.GetRequestData<List<QuestionViewModel>>("api/category/GetListQuestions/" + categoryId);

                }catch(Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sourceQ.ResetBindings(false);
                sourcePQ.ResetBindings(false);
            }
        }
    }
}
