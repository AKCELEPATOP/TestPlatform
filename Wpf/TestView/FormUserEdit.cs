using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormUserEdit : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormUserEdit()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            if (id.HasValue)
            {
                try
                {
                    var user = Task.Run(() => ApiClient.GetRequestData<UserViewModel>("api/User/Get/" + id.Value)).Result;
                    textBox1.Text = user.FIO;
                    textBox2.Text = user.UserName;
            // password        textBox3.Text = user.UserName;

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
        //сохранить
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Заполните Логин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Task task;
            string fio = textBox1.Text;
            string username = textBox2.Text;
            string password = textBox3.Text;
            if (id.HasValue)
            {



                task = Task.Run(() => ApiClient.PutRequestData("api/User/UpdElement", new UserBindingModel
                {
                    Id = id.Value.ToString(),
                    UserName=username,
                    PasswordHash=password

                }));


            }
            else
            {
                task = Task.Run(() => ApiClient.PutRequestData("api/User/AddElement", new UserBindingModel
                {
                    
                    UserName = username,
                    PasswordHash = password

                }));

            }
            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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

            Close();
        }
        //cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
