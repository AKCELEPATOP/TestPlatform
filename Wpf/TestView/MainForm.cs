using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TestService.ViewModels;
using TestService.BindingModels;

namespace TestView
{
    public partial class FormMain : Form
    {
        public static bool DarkTheme { get; set; }

        public string UserLogin { get; set; }

        private FormAuthorization parent;
        private Color colorFont;
        private Color colorBack;

        public FormMain(FormAuthorization parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private async void Initialize()
        {
            try
            {
                List<PatternViewModel> list = await ApiClient.GetRequestData<List<PatternViewModel>>("api/Pattern/GetUserList");
                if (list != null)
                {
                    dataGridViewAvailablePatterns.DataSource = list;
                    dataGridViewAvailablePatterns.Columns[0].Visible = false;
                    dataGridViewAvailablePatterns.Columns[2].Visible = false;
                    dataGridViewAvailablePatterns.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                List<StatViewModel> listС = await ApiClient.PostRequestData<GetListModel, List<StatViewModel>>("api/Stat/GetUserList", new GetListModel { });
                if (listС != null)
                {
                    dataGridViewPassedTests.DataSource = listС;
                    dataGridViewPassedTests.Columns[1].Visible = false;
                    dataGridViewPassedTests.Columns[6].Visible = false;
                    dataGridViewPassedTests.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

            comboBoxFontSize.Text = "8";
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
            Initialize();
            textBoxCurrentUser.Text = ApiClient.UserName;
            textBoxGroupUser.Text = ApiClient.UserGroup;
        }

        private void buttonStatistic_Click(object sender, EventArgs e)
        {
            StatisticForm statisticForm = new StatisticForm();
            statisticForm.Show();
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

        private void buttonChangeColorBack_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorBack = cd.Color;
            }
            this.BackColor = colorBack;
        }
        private void buttonChangeFont_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorFont = cd.Color;
            }
            this.ForeColor = colorFont;
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
                    await ApiClient.PostRequestData("api/stat/SaveToPdf", new ReportBindingModel
                    {
                        FilePath = fileName,
                    });
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
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            contextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            dataGridViewAvailablePatterns.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            dataGridViewPassedTests.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            textBoxCurrentUser.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            textBoxGroupUser.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            обновитьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            SaveToPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonChangeColorBack.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonBeginTest.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonChangeFont.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonChangeUser.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonStatistic.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
        }
    }
}