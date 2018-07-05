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
                Design.SetBackColor(Testing);
                Design.SetFontColor(Testing);
                Testing.ShowDialog();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonChangeUser_Click(object sender, EventArgs e)
        {
            parent.Show();
            Design.SetBackColor(parent);
            Design.SetFontColor(parent);
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
            Design.SetBackColor(statisticForm);
            Design.SetFontColor(statisticForm);
            statisticForm.ShowDialog();
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
            Design.SetDefaultBackColor(cd.Color);
            this.BackColor = cd.Color;
        }

        private void buttonChangeFont_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorFont = cd.Color;
            }
            Design.SetDefaultFontColor(cd.Color);
          

            label1.ForeColor = cd.Color;
            label2.ForeColor = cd.Color;
            label3.ForeColor = cd.Color;
            label4.ForeColor = cd.Color;
            label5.ForeColor = cd.Color;
            label6.ForeColor = cd.Color;
            label7.ForeColor = cd.Color;
            groupBox2.ForeColor = cd.Color;
            contextMenuStrip1.ForeColor = cd.Color;
            dataGridViewAvailablePatterns.ForeColor = cd.Color;
            dataGridViewPassedTests.ForeColor = cd.Color;
            textBoxCurrentUser.ForeColor = cd.Color;
            textBoxGroupUser.ForeColor = cd.Color;
            обновитьToolStripMenuItem.ForeColor = cd.Color;
            SaveToPDF.ForeColor = cd.Color;
            buttonChangeColorBack.ForeColor = cd.Color;
            buttonBeginTest.ForeColor = cd.Color;
            buttonChangeFont.ForeColor = cd.Color;
            buttonChangeUser.ForeColor = cd.Color;
            buttonExit.ForeColor = cd.Color;
            buttonStatistic.ForeColor = cd.Color;
            this.ForeColor = cd.Color;

            buttonBeginTest.BackColor = Design.Invert(this.ForeColor);
            buttonChangeColorBack.BackColor = Design.Invert(this.ForeColor);
            buttonChangeFont.BackColor = Design.Invert(this.ForeColor);
            buttonChangeUser.BackColor = Design.Invert(this.ForeColor);
            buttonExit.BackColor = Design.Invert(this.ForeColor);
            buttonStatistic.BackColor = Design.Invert(this.ForeColor);
            comboBoxFontSize.BackColor = Design.Invert(this.ForeColor);
            SaveToPDF.BackColor = Design.Invert(this.ForeColor);

            dataGridViewAvailablePatterns.EnableHeadersVisualStyles = false;
            dataGridViewAvailablePatterns.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridViewAvailablePatterns.BackColor = Design.Invert(this.ForeColor);
            dataGridViewAvailablePatterns.ForeColor = Design.Invert(this.ForeColor);
            dataGridViewAvailablePatterns.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridViewAvailablePatterns.RowsDefaultCellStyle.ForeColor = Design.Invert(this.ForeColor);
            dataGridViewAvailablePatterns.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);

            dataGridViewPassedTests.EnableHeadersVisualStyles = false;
            dataGridViewPassedTests.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridViewPassedTests.BackColor = Design.Invert(this.ForeColor);
            dataGridViewPassedTests.ForeColor = Design.Invert(this.ForeColor);
            dataGridViewPassedTests.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridViewPassedTests.RowsDefaultCellStyle.ForeColor = Design.Invert(this.ForeColor);
            dataGridViewPassedTests.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
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
        private void comboBoxFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Design.FontSize = Convert.ToInt32(comboBoxFontSize.SelectedItem);
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            contextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridViewAvailablePatterns.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridViewPassedTests.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBoxCurrentUser.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBoxGroupUser.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            обновитьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            SaveToPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonChangeColorBack.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonBeginTest.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonChangeFont.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonChangeUser.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonStatistic.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);

            buttonBeginTest.BackColor = Design.Invert(this.ForeColor);
            buttonChangeColorBack.BackColor = Design.Invert(this.ForeColor);
            buttonChangeFont.BackColor = Design.Invert(this.ForeColor);
            buttonChangeUser.BackColor = Design.Invert(this.ForeColor);
            buttonExit.BackColor = Design.Invert(this.ForeColor);
            buttonStatistic.BackColor = Design.Invert(this.ForeColor);
            comboBoxFontSize.BackColor = Design.Invert(this.ForeColor);
            SaveToPDF.BackColor = Design.Invert(this.ForeColor);
            dataGridViewAvailablePatterns.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridViewPassedTests.BackgroundColor = Design.Invert(this.ForeColor);
        }
    }
}