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
    public partial class FormAdminsControl : Form
    {
        public FormAdminsControl()
        {
            InitializeComponent();

            if (!ApiClient.Role.Equals(ApplicationRoles.SuperAdmin))
            {
                buttonRemoveAdminStatus.Enabled = false;
                buttonRemoveAdminStatus.Visible = false;
            }
        }
        private void Initialize()
        {
            try
            {
                List<UserViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<UserViewModel>>("api/Admin/GetList")).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[4].Visible = false;
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

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
            buttonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            buttonRemoveAdminStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            contextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            dataGridView1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            обновитьToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize); 

            buttonDelete.BackColor = Design.Invert(this.ForeColor);
            buttonRemoveAdminStatus.BackColor = Design.Invert(this.ForeColor);
            dataGridView1.BackgroundColor = Design.Invert(this.ForeColor);
        }
        // ПКМ -> Обновить
        private void MouseDown_Form(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить администратора?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string id = Convert.ToString(dataGridView1.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => ApiClient.PostRequest("api/Admin/DelElement/" + id));
                    task.ContinueWith((prevTask) => MessageBox.Show("Администратор удалён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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

        private async void buttonRemoveAdminStatus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                try
                {
                    Task task = Task.Run(() => ApiClient.PostRequest("api/Admin/SetUser/" + Convert.ToString(dataGridView1.SelectedRows[0].Cells[0].Value)));
                    await task;
                    MessageBox.Show("У пользователя отобраны права админа", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Initialize();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Initialize();

        }
    }
}