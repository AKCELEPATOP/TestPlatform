using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormStatisticsMain : MetroForm
    {
        private FormAuthorization parent;
        public static bool DarkTheme { get; set; }

        public FormStatisticsMain(FormAuthorization parent)
        {
            this.parent = parent;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            ShadowType = MetroFormShadowType.DropShadow;
            labelUserName.Text = ApiClient.UserName;
            Initialize();
        }
        private async void Initialize()
        {
            try
            {
                List<PatternViewModel> list = await ApiClient.GetRequestData<List<PatternViewModel>>("api/Pattern/GetList");
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                List<StatViewModel> listC = await ApiClient.PostRequestData<GetListModel, List<StatViewModel>>("api/Stat/GetList", new GetListModel{});
                if (listC != null)
                {
                    dataGridView2.DataSource = listC;
                    dataGridView2.Columns[6].Visible = false;
                    dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
        private async void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        await ApiClient.PostRequest("api/Pattern/DelElement/" + id);
                        MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Initialize();
                    }
                    catch(Exception ex)
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

        // ПКМ -> Обновить
        private void MouseDown_Form(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }
        private void обновитьToolStripMenuItem_Click_1(object sender, EventArgs e)
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
        private async void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                try
                {
                    await Task.Run(() => ApiClient.PostRequestData("api/stat/SaveToPdfAdmin",new ReportBindingModel
                    {
                        FilePath = fileName,
                    }));
                    MessageBox.Show("Файл успешно сохранен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
            if (DarkTheme)
            {
                this.Theme = MetroFramework.MetroThemeStyle.Dark;
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                labelUserName.ForeColor = Color.White;
                groupBox1.ForeColor = Color.White;
                groupBox2.ForeColor = Color.White;
                groupBox3.ForeColor = Color.White;
                groupBox4.ForeColor = Color.White;
                groupBox5.ForeColor = Color.White;
                DarkTheme = !DarkTheme;
            }
            else
            {
                this.Theme = MetroFramework.MetroThemeStyle.Light;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                labelUserName.ForeColor = Color.Black;
                groupBox1.ForeColor = Color.Black;
                groupBox2.ForeColor = Color.Black;
                groupBox3.ForeColor = Color.Black;
                groupBox4.ForeColor = Color.Black;
                groupBox5.ForeColor = Color.Black;
                DarkTheme = !DarkTheme;
            }
        }

        private void buttonAdmins_Click(object sender, EventArgs e)
        {
            //сделайте
        }
    }
}
