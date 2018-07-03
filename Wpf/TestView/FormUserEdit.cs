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

        private async void Initialize()
        {
            try
            {
                var groups = await ApiClient.GetRequestData<List<GroupViewModel>>("api/Group/GetList");
                groups.Insert(0, new GroupViewModel
                {
                    Id = null,
                    Name = "Общая"
                });
                comboBoxGroups.DisplayMember = "Name";
                comboBoxGroups.ValueMember = "Id";
                comboBoxGroups.DataSource = groups;
                comboBoxGroups.SelectedItem = groups[0];
            }
            catch (Exception ex)
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
                    var user = await ApiClient.GetRequestData<UserViewModel>("api/User/Get/" + id);
                    textBoxFIO.Text = user.FIO;
                    textBoxUserName.Text = user.UserName;
                    textBoxEmail.Text = user.Email;
                    if (user.GroupId != null)
                    {
                        comboBoxGroups.SelectedValue = user.GroupId;
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
        }
        //сохранить
        private async void button1_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            string fio = textBoxFIO.Text;
            string username = textBoxUserName.Text;
            string password = textBoxPassword.Text;
            string mail = textBoxEmail.Text;
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                message += "Заполните ФИО";
            }
            if (string.IsNullOrEmpty(textBoxUserName.Text))
            {
                message += "Заполните Логин";
            }
            if (!string.IsNullOrEmpty(password)) {
                if (!Regex.IsMatch(password, @"(?=.*[a-z])(?=.*[0-9])^[a-zA-Z0-9]{5,}$"/*@"^(?=.*[0-9]$)(?=.*[a-zA-Z]){5,}"*/))
                {
                    message += " Пароль должен быть не короче 5 символов, содержать хотя бы одну лат букву в ниж регистре и одну цифру.";
                }
            }
            if (!string.IsNullOrEmpty(mail))
            {
                if (!Regex.IsMatch(mail, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                {
                    message += "Неверный формат для электронной почты";
                    return;
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (comboBoxGroups.SelectedValue != null)
                    {
                        int groupId = Convert.ToInt32(comboBoxGroups.SelectedValue);
                        await ApiClient.PostRequestData("api/User/UpdElement", new UserBindingModel
                        {
                            Id = id,
                            UserName = username,
                            Email = mail,
                            GroupId = groupId,
                            FIO = fio,
                            PasswordHash = password
                        });
                    }
                    else
                    {
                        await ApiClient.PostRequestData("api/User/UpdElement", new UserBindingModel
                        {
                            Id = id,
                            UserName = username,
                            Email = mail,
                            FIO = fio,
                            PasswordHash = password
                        });
                    }
                }
                else
                {
                    if (comboBoxGroups.SelectedValue != null)
                    {
                        int groupId = Convert.ToInt32(comboBoxGroups.SelectedValue);
                        await ApiClient.PostRequestData("api/User/AddElement", new UserBindingModel
                        {
                            UserName = username,
                            Email = mail,
                            GroupId = groupId,
                            FIO = fio,
                            PasswordHash = password
                        });
                    }
                    else
                    {
                        await ApiClient.PostRequestData("api/User/AddElement", new UserBindingModel
                        {
                            UserName = username,
                            Email = mail,
                            FIO = fio,
                            PasswordHash = password
                        });
                    }

                }
                MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            textBoxEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            textBoxFIO.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            textBoxUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
        }
    }
}
