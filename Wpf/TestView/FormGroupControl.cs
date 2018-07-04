 
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
    public partial class FormGroupControl : Form
    {
        private FormStatisticsMain.CallBack call;

        public FormGroupControl(FormStatisticsMain.CallBack call)
        {
            InitializeComponent();
            this.call = call;
        }

        private async void Initialize() {
            try
            {
                List<GroupViewModel> list = await ApiClient.GetRequestData<List<GroupViewModel>>("api/Group/GetList");
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

        private void button7_Click(object sender, EventArgs e)
        {
            var form = new FormGroupEdit();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new FormGroupEdit();
            form.Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
                call.Invoke();
            }
        }

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

        private async void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись? Шаблоны для этой группы станут общедоступными!", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        await ApiClient.PostRequest("api/Group/DelElement/" + id);
                        MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Initialize();
                        call.Invoke();
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

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
            groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button5.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button7.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            contextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridView1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            обновитьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);

            button1.BackColor = Design.Invert(this.ForeColor);
            button5.BackColor = Design.Invert(this.ForeColor);
            button7.BackColor = Design.Invert(this.ForeColor);
            dataGridView1.BackColor = Design.Invert(this.ForeColor);

        }
    }
}
