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
    public partial class FormCategories : Form
    {
        public FormCategories()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            try
            {
                List<CategoryViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<CategoryViewModel>>("api/Category/GetList")).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
                Task.Run(() => ApiClient.GetRequestData<List<QuestionViewModel>>("api/Question/GetList/" + Id)).Result;
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
            var form = new FormCategoryEdit();
            form.Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => ApiClient.DelRequest("api/Category/DelElement/" + id));
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
            var form = new FormQuestionEditor();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new FormQuestionEditor();
            form.Id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => ApiClient.DelRequest("api/Question/DelElement/" + id));
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
    }
}
