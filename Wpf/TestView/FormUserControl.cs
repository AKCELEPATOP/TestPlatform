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
    public partial class FormUserControl : Form
    {
        public FormUserControl()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize() {
            try
            {
                List<UserViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<UserViewModel>>("api/User/GetList")).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                List<GroupViewModel> listC =
                   Task.Run(() => ApiClient.GetRequestData<List<GroupViewModel>>("api/Group/GetList")).Result;
                if (listC != null)
                {
                    comboBox1.DataSource = listC;
                    comboBox1.DisplayMember = "Name";
                    comboBox1.ValueMember = "Id";
                    comboBox1.SelectedItem = null;
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
            var form = new FormUserEdit();
            form.Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => ApiClient.PostRequest("api/User/DelElement/" + id));
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

        private void button5_Click(object sender, EventArgs e)
        {
            var form = new FormGroupControl();
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1 && comboBox1.SelectedValue !=null)
            { 
                int user_id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                int Grou_id = Convert.ToInt32(comboBox1.SelectedValue);
                string value = user_id + "/" + Grou_id;
                Task task = Task.Run(() => ApiClient.PostRequestData("api/User/SetGroup", value));
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
        }
    }
}
