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
    public partial class FormStatisticsMain : Form
    {
        private FormAuthorization parent;

        public FormStatisticsMain(FormAuthorization parent)
        {
            this.parent = parent;
            InitializeComponent();
            labelUserName.Text = ApiClient.UserName;
            Initialize();
        }
        private void Initialize()
        {
            try
            {
                List<PatternViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<PatternViewModel>>("api/Pattern/GetList")).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                List<StatViewModel> listC =
                    Task.Run(() => ApiClient.PostRequestData<GetListModel,List<StatViewModel>>("api/Stat/GetList",new GetListModel {
                        Skip = 0,
                        Take = 20
                    })).Result;
                if (list != null)
                {
                    dataGridView2.DataSource = listC;
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
            //подготовить тест шаблон
        private void button3_Click(object sender, EventArgs e)
        {
            var form = new FormTestTemplate();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }
        //изменить шаблон
        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = new FormTestTemplate();
                form.Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }
        //удалить шаблон
        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => ApiClient.PostRequest("api/Pattern/DelElement/" + id));
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

        //сменить пользователя
        private void button7_Click(object sender, EventArgs e)
        {
            parent.Show();
            parent = null;
            Close();
        }
        //категории вопросы
        private void button2_Click(object sender, EventArgs e)
        {
            var form = new FormCategories();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }
        //управление пользователями 
        private void button8_Click(object sender, EventArgs e)
        {
            var form = new FormUserControl();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }
    
        //сохранить в файл
        private void button4_Click(object sender, EventArgs e)
        {

        }
        //выход
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
