using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormUserEdit : Form
    {
        public string Id { set { id = value; } }

        private string id;

        public FormUserEdit()
        {
            InitializeComponent();
        }

        private void Initialize() {
            try
            {
                var groups = Task.Run(() => ApiClient.GetRequestData<List<GroupViewModel>>("api/Group/GetList")).Result;
                comboBoxGroups.DisplayMember = "Name";
                comboBoxGroups.ValueMember = "Id";
                comboBoxGroups.DataSource = groups;
                comboBoxGroups.SelectedItem = null;
            }
            catch(Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!string.IsNullOrEmpty(id))
            {
                try
                {


                    var user = Task.Run(() => ApiClient.GetRequestData<UserViewModel>("api/User/Get/" + id)).Result;
                    textBoxFIO.Text = user.FIO;
                    textBoxUserName.Text = user.UserName;
                    textBoxEmail.Text = user.Email;
                    comboBoxGroups.SelectedValue = user.GroupId ?? null;
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
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxUserName.Text))
            {
                MessageBox.Show("Заполните Логин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Заполните Пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Task task;
            string fio = textBoxFIO.Text;
            string username = textBoxUserName.Text;
            string password = textBoxPassword.Text;
            string mail = textBoxEmail.Text;
            if (!string.IsNullOrEmpty(mail))
            {
                if (!Regex.IsMatch(mail, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                {
                    MessageBox.Show("Неверный формат для электронной почты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (!string.IsNullOrEmpty(id))
            {
                if (comboBoxGroups.SelectedValue != null)
                {
                    task = Task.Run(() => ApiClient.PostRequestData("api/User/UpdElement", new UserBindingModel
                    {
                        Id = id,
                        UserName = username,
                        Email = mail,
                        GroupId = Convert.ToInt32(comboBoxGroups.SelectedValue),
                        FIO = fio,
                        PasswordHash = password
                    }));
                }
                else
                {
                    task = Task.Run(() => ApiClient.PostRequestData("api/User/UpdElement", new UserBindingModel
                    {
                        Id = id,
                        UserName = username,
                        Email = mail,
                        FIO = fio,
                        PasswordHash = password
                    }));
                }
            }
            else
            {
                if (comboBoxGroups.SelectedValue != null)
                {
                    task = Task.Run(() => ApiClient.PostRequestData("api/User/AddElement", new UserBindingModel
                    {
                        UserName = username,
                        Email = mail,
                        GroupId = Convert.ToInt32(comboBoxGroups.SelectedValue),
                        FIO = fio,
                        PasswordHash = password
                    }));
                }
                else
                {
                    task = Task.Run(() => ApiClient.PostRequestData("api/User/AddElement", new UserBindingModel
                    {
                        UserName = username,
                        Email = mail,
                        FIO = fio,
                        PasswordHash = password
                    }));
                }

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

        private void FormUserEdit_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
