
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormStatisticsMain : Form
    {
        private FormAuthorization parent;
        private Color colorBack;
        private Color colorFont;

        public delegate void CallBack();

        public FormStatisticsMain(FormAuthorization parent)
        {
            this.parent = parent;
            InitializeComponent();
            labelUserName.Text = ApiClient.UserName;
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

                    if (list.Count > 0)
                    {
                        dataGridView1.Rows[0].Selected = true;
                        List<StatViewModel> listPS = await ApiClient.GetRequestData<List<StatViewModel>>("api/Stat/GetPatternList/" + list[0].Id);
                        dataGridViewPatternStat.DataSource = listPS;
                        dataGridViewPatternStat.Columns[0].Visible = false;
                        dataGridViewPatternStat.Columns[6].Visible = false;

                    }
                }
                List<StatViewModel> listC = await ApiClient.PostRequestData<GetListModel, List<StatViewModel>>("api/Stat/GetList", new GetListModel { });
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
            comboBoxFontSize.Text = "8";
        }
        //подготовить тест шаблон
        private void button3_Click(object sender, EventArgs e)
        {
            var form = new FormTestTemplate();
            Design.SetBackColor(form);
            Design.SetFontColor(form);
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
                Design.SetBackColor(form);
                Design.SetFontColor(form);
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
            Design.SetBackColor(parent);
            Design.SetFontColor(parent);
            parent.Show();
            parent = null;
            Close();
        }
        //категории вопросы
        private void button2_Click(object sender, EventArgs e)
        {
            var form = new FormCategories();
            Design.SetBackColor(form);
            Design.SetFontColor(form);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }
        //управление пользователями 
        private void button8_Click(object sender, EventArgs e)
        {
            CallBack call = new CallBack(Initialize);
            var form = new FormUserControl(call);
            Design.SetBackColor(form);
            Design.SetFontColor(form);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }


        private void buttonChangeColorBack_Click_1(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorBack = cd.Color;
            }
            Design.SetDefaultBackColor(cd.Color);
            this.BackColor = colorBack;
        }

        private void buttonChangeFont_Click_1(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorFont = cd.Color;
            }
            Design.SetDefaultFontColor(cd.Color);
            label1.ForeColor = cd.Color;
            labelUserName.ForeColor = cd.Color;
            label4.ForeColor = cd.Color;
            label5.ForeColor = cd.Color;
            label6.ForeColor = cd.Color;
            label7.ForeColor = cd.Color;
            contextMenuStrip1.ForeColor = cd.Color;
            dataGridView1.ForeColor = cd.Color;
            dataGridView2.ForeColor = cd.Color;
            button1.ForeColor = cd.Color;
            button10.ForeColor = cd.Color;
            обновитьToolStripMenuItem.ForeColor = cd.Color;
            button2.ForeColor = cd.Color;
            buttonChangeColorBack.ForeColor = cd.Color;
            button3.ForeColor = cd.Color;
            buttonChangeFont.ForeColor = cd.Color;
            button4.ForeColor = cd.Color;
            button7.ForeColor = cd.Color;
            button8.ForeColor = cd.Color;
            button9.ForeColor = cd.Color;
            buttonAdmins.ForeColor = cd.Color;
            groupBox1.ForeColor = cd.Color;
            groupBox2.ForeColor = cd.Color;
            groupBox3.ForeColor = cd.Color;
            groupBox4.ForeColor = cd.Color;
            groupBox5.ForeColor = cd.Color;
            dataGridViewPatternStat.ForeColor = cd.Color;
            comboBoxFontSize.ForeColor = cd.Color;

            button1.BackColor = Design.Invert(this.ForeColor);
            button10.BackColor = Design.Invert(this.ForeColor);
            button2.BackColor = Design.Invert(this.ForeColor);
            button3.BackColor = Design.Invert(this.ForeColor);
            button4.BackColor = Design.Invert(this.ForeColor);
            button7.BackColor = Design.Invert(this.ForeColor);
            button8.BackColor = Design.Invert(this.ForeColor);
            button9.BackColor = Design.Invert(this.ForeColor);
            buttonAdmins.BackColor = Design.Invert(this.ForeColor);
            buttonChangeColorBack.BackColor = Design.Invert(this.ForeColor);
            buttonChangeFont.BackColor = Design.Invert(this.ForeColor);
            comboBoxFontSize.BackColor = Design.Invert(this.ForeColor);

            button1.BackColor = Design.Invert(this.ForeColor);
            button10.BackColor = Design.Invert(this.ForeColor);
            button2.BackColor = Design.Invert(this.ForeColor);
            button3.BackColor = Design.Invert(this.ForeColor);
            button4.BackColor = Design.Invert(this.ForeColor);
            button7.BackColor = Design.Invert(this.ForeColor);
            button8.BackColor = Design.Invert(this.ForeColor);
            button9.BackColor = Design.Invert(this.ForeColor);
            buttonAdmins.BackColor = Design.Invert(this.ForeColor);
            buttonChangeColorBack.BackColor = Design.Invert(this.ForeColor);
            buttonChangeFont.BackColor = Design.Invert(this.ForeColor);
            comboBoxFontSize.BackColor = Design.Invert(this.ForeColor);

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridView1.BackColor = Design.Invert(this.ForeColor);
            dataGridView1.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);

            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridView2.BackColor = Design.Invert(this.ForeColor);
            dataGridView2.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);

            dataGridViewPatternStat.EnableHeadersVisualStyles = false;
            dataGridViewPatternStat.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridViewPatternStat.BackColor = Design.Invert(this.ForeColor);
            dataGridViewPatternStat.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridViewPatternStat.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
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
                    await Task.Run(() => ApiClient.PostRequestData("api/stat/SaveToPdfAdmin", new ReportBindingModel
                    {
                        FilePath = fileName,
                    }));
                    MessageBox.Show("Файл успешно сохранен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        //выход
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void buttonAdmins_Click(object sender, EventArgs e)
        {
            var form = new FormAdminsControl();
            Design.SetBackColor(form);
            Design.SetFontColor(form);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }

        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                try
                {
                    List<StatViewModel> listPS = await ApiClient.GetRequestData<List<StatViewModel>>("api/Stat/GetPatternList/" + Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
                    dataGridViewPatternStat.DataSource = listPS;
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
            labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            contextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridView1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridView2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button10.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            обновитьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonChangeColorBack.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonChangeFont.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button7.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button8.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button9.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonAdmins.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridViewPatternStat.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            comboBoxFontSize.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);

            button1.BackColor = Design.Invert(this.ForeColor);
            button10.BackColor = Design.Invert(this.ForeColor);
            button2.BackColor = Design.Invert(this.ForeColor);
            button3.BackColor = Design.Invert(this.ForeColor);
            button4.BackColor = Design.Invert(this.ForeColor);
            button7.BackColor = Design.Invert(this.ForeColor);
            button8.BackColor = Design.Invert(this.ForeColor);
            button9.BackColor = Design.Invert(this.ForeColor);
            buttonAdmins.BackColor = Design.Invert(this.ForeColor);
            buttonChangeColorBack.BackColor = Design.Invert(this.ForeColor);
            buttonChangeFont.BackColor = Design.Invert(this.ForeColor);
            comboBoxFontSize.BackColor = Design.Invert(this.ForeColor);

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridView1.BackColor = Design.Invert(this.ForeColor);
            dataGridView1.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);

            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridView2.BackColor = Design.Invert(this.ForeColor);
            dataGridView2.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);

            dataGridViewPatternStat.EnableHeadersVisualStyles = false;
            dataGridViewPatternStat.BackgroundColor = Design.Invert(this.ForeColor);
            dataGridViewPatternStat.BackColor = Design.Invert(this.ForeColor);
            dataGridViewPatternStat.RowsDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);
            dataGridViewPatternStat.ColumnHeadersDefaultCellStyle.BackColor = Design.Invert(this.ForeColor);

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
