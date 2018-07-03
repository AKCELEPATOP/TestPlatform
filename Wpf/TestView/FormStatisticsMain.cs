
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
            CallBack call = new CallBack(Initialize);
            var form = new FormUserControl(call);

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
            this.BackColor = colorBack;
        }

        private void buttonChangeFont_Click_1(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorFont = cd.Color;
            }
            this.ForeColor = colorFont;
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

        private void buttonChangeColorBack_Click(object sender, EventArgs e)
        {

        }
        private void buttonChangeFont_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdmins_Click(object sender, EventArgs e)
        {
            var form = new FormAdminsControl();

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
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            contextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            dataGridView1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            dataGridView2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button10.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            обновитьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonChangeColorBack.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button3.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonChangeFont.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button4.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button7.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button8.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            button9.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            buttonAdmins.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
            dataGridViewPatternStat.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(comboBoxFontSize.SelectedItem));
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
