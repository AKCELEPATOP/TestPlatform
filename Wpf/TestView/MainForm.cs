using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestView;
using TestService.ViewModels;
using TestService.BindingModels;
using MetroFramework.Forms;

namespace TestView
{
    public partial class FormMain : MetroForm
    {
        public Color colorBack;
        public Color colorFont;

        public string UserLogin { get; set; }

        private FormAuthorization parent;

        public FormMain(FormAuthorization parent)
        {
            this.parent = parent;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            ShadowType = MetroFormShadowType.DropShadow;
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                List<PatternViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<PatternViewModel>>("api/Pattern/GetUserList")).Result;
                if (list != null)
                {
                    dataGridViewAvailablePatterns.DataSource = list;
                    dataGridViewAvailablePatterns.Columns[0].Visible = false;
                    dataGridViewAvailablePatterns.Columns[2].Visible = false;
           //         dataGridViewAvailablePatterns.Columns[4].Visible = false;
                    dataGridViewAvailablePatterns.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                List<StatViewModel> listС =
                    Task.Run(() => ApiClient.PostRequestData<GetListModel,List<StatViewModel>>("api/Stat/GetUserList",new GetListModel
                    {
                        Skip = 0,
                        Take = 20
                    })).Result;
                if (listС != null)
                {
                    dataGridViewPassedTests.DataSource = listС;
                    dataGridViewPassedTests.Columns[4].Visible = false;
                    dataGridViewPassedTests.Columns[5].Visible = false;
                    dataGridViewPassedTests.Columns[6].Visible = false;
                    //    dataGridViewPassedTests.Columns[2].Visible = false;
                    //dataGridViewPassedTests.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private void button7_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorFont = cd.Color;
            }
            this.ForeColor = colorFont;
        }


        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorBack = cd.Color;

                buttonChangeColorFont.BackColor = colorBack;

            }
            this.BackColor = colorBack;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridViewAvailablePatterns.SelectedRows.Count == 1)
            {
                TestingForm Testing = new TestingForm();
                Testing.Id = Convert.ToInt32(dataGridViewAvailablePatterns.SelectedRows[0].Cells[0].Value);
                Testing.Show();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonChangeUser_Click(object sender, EventArgs e)
        {
            parent.Show();
            parent = null;
            Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //textBoxCurrentUser.Text = ;
            //textBoxGroupUser.Text = ;
            Initialize();
            this.ForeColor = colorFont;
            this.BackColor = colorBack;
        }

        private void buttonStatistic_Click(object sender, EventArgs e)
        {
            StatisticForm statisticForm = new StatisticForm();
            statisticForm.Show();
        }

        private void dataGridViewAvailablePatterns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewAvailablePatterns.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridViewAvailablePatterns.SelectedRows[0].Cells[0].Value);
                List<PatternViewModel> list =
                Task.Run(() => ApiClient.GetRequestData<List<PatternViewModel>>("api/Pattern/GetList/" + Id)).Result;
                if (list != null)
                {
                    dataGridViewAvailablePatterns.DataSource = list;
                    dataGridViewAvailablePatterns.Columns[0].Visible = false;
                    dataGridViewAvailablePatterns.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void dataGridViewPassedTests_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewPassedTests.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridViewPassedTests.SelectedRows[0].Cells[0].Value);
                List<TestViewModel> list =
                Task.Run(() => ApiClient.GetRequestData<List<TestViewModel>>("api/Stat/GetList/" + Id)).Result;
                if (list != null)
                {
                    dataGridViewPassedTests.DataSource = list;
                    dataGridViewPassedTests.Columns[0].Visible = false;
                    dataGridViewPassedTests.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private async void buttonSaveToPdf_Click(object sender, EventArgs e)
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
                    Task task = Task.Run(() => ApiClient.PostRequestData("api/stat/SaveToPdf", new ReportBindingModel
                    {
                        FilePath = fileName,
                    }));
                    await task;
                    MessageBox.Show("Файл успешно сохранен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
}
