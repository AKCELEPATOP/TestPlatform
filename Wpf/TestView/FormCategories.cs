 
using System;
using System.Collections.Generic;
using System.Drawing;
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
        }

        private async void Initialize()
        {
            try
            {
                List<CategoryViewModel> list = await ApiClient.GetRequestData<List<CategoryViewModel>>("api/Category/GetList");
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    if (list.Count > 0)
                    {
                        List<QuestionViewModel> listQuestions = await ApiClient.GetRequestData<List<QuestionViewModel>>("api/Category/GetListQuestions/" + list[0].Id);
                        if (listQuestions != null)
                        {
                            dataGridView2.DataSource = listQuestions;
                            dataGridView2.Columns[0].Visible = false;
                            dataGridView2.Columns[1].Visible = false;
                            dataGridView2.Columns[3].Visible = false;
                            dataGridView2.Columns[5].Visible = false;
                            dataGridView2.Columns[7].Visible = false;
                            dataGridView2.Columns[8].Visible = false;
                            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                List<QuestionViewModel> list = await ApiClient.GetRequestData<List<QuestionViewModel>>("api/Category/GetListQuestions/" + Id);
                if (list != null)
                {
                    dataGridView2.DataSource = list;
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].Visible = false;
                    dataGridView2.Columns[3].Visible = false;
                    dataGridView2.Columns[5].Visible = false;
                    dataGridView2.Columns[7].Visible = false;
                    dataGridView2.Columns[8].Visible = false;
                    dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var form = new FormCategoryEdit();
            Design.SetBackColor(form);
            Design.SetFontColor(form);
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
                Design.SetBackColor(form);
                Design.SetFontColor(form);
                form.Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        await ApiClient.PostRequest("api/Category/DelElement/" + id);
                        MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Initialize();
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
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = new FormQuestionEditor();
                Design.SetBackColor(form);
                Design.SetFontColor(form);
                form.IdCat = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
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
                Design.SetBackColor(form);
                Design.SetFontColor(form);
                form.IdCat = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                form.Id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        await ApiClient.PostRequest("api/Question/DelElement/" + id);
                        MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Initialize();
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
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button10.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button6.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            contextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridView1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridView2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            обновитьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
