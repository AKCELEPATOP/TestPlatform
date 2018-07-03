using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormUserControl : Form
    {

        public FormUserControl()
        {
            InitializeComponent();
 
 
 
            Initialize();
        }
        private async void Initialize() {
            try
            {
                List<UserViewModel> list = await ApiClient.GetRequestData<List<UserViewModel>>("api/User/GetList");
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[5].Visible = false;
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

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new FormUserEdit();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = new FormUserEdit();
                form.Id = Convert.ToString(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string id = Convert.ToString(dataGridView1.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        await ApiClient.PostRequest("api/User/DelElement/" + id);
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

        private void button5_Click(object sender, EventArgs e)
        {
            var form = new FormGroupControl();
            
            if (form.ShowDialog() == DialogResult.OK)
            {
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

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private async void buttonSetAdmin_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                try
                {
                    await ApiClient.PostRequest("api/Admin/SetAdmin/" + Convert.ToString(dataGridView1.SelectedRows[0].Cells[0].Value));
                    MessageBox.Show("Пользователю выданы права админа", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
}
