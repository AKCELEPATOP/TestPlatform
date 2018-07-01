using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormCategories : MetroForm
    {
        public FormCategories()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                List<CategoryViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<CategoryViewModel>>("api/Category/GetList")).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    if (list.Count > 0)
                    {

                        List<QuestionViewModel> listQuestions =
                      Task.Run(() => ApiClient.GetRequestData<List<QuestionViewModel>>("api/Category/GetListQuestions/" + list[0].Id)).Result;
                        if (listQuestions != null)
                        {
                            dataGridView2.DataSource = listQuestions;
                            dataGridView2.Columns[0].Visible = false;
                            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                List<QuestionViewModel> list =
                Task.Run(() => ApiClient.GetRequestData<List<QuestionViewModel>>("api/Category/GetListQuestions/" + Id)).Result;
                if (list != null)
                {
                    dataGridView2.DataSource = list;
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var form = new FormCategoryEdit();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = new FormCategoryEdit();
                form.Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => ApiClient.PostRequest("api/Category/DelElement/" + id));
                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
                }
                Initialize();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = new FormQuestionEditor();
                form.IdCat = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                form.Initialize();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }

        private void MouseDown_Form(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = new FormQuestionEditor();
                form.IdCat = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                form.Id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => ApiClient.PostRequest("api/Question/DelElement/" + id));
                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
                }

                Initialize();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
