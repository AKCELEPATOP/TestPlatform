using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;

namespace TestView
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }
        private async void registrate_Click(object sender, EventArgs e)
        {
            try
            {
                string fio = textBoxFIO.Text;
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
                string login = textBoxLogin.Text;
                if(!Regex.IsMatch(login, @"\w{8,}"))
                {
                    MessageBox.Show("Логин должен быть не меньше 8 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string password = textBoxPassword1.Text;
                if (!Regex.IsMatch(password, @"[0-9a-z]{5,}"))
                {
                    MessageBox.Show("Пароль должен быть не короче 5 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!password.Equals(textBoxPassword2.Text))
                {
                    MessageBox.Show("Пароли должны совпадать", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    UserBindingModel newUser = new UserBindingModel
                    {
                        FIO = fio,
                        PasswordHash = password,
                        UserName = login,
                        Email = mail
                    };
                    Task task = Task.Run(() => ApiClient.PostRequestData<UserBindingModel>("api/Account/Register", newUser));
                    await task;
                    MessageBox.Show("Пользователь зарегистрирован.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch(Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                /*if (textBoxFIO.TextLength != 0 || textBoxLogin.TextLength > 8 ||
                    textBoxPassword1.TextLength != 0 || textBoxPassword2.TextLength != 0 || textBoxEmail.TextLength != 0)
                {
                    if (textBoxPassword1.TextLength > 4)
                    {
                        if (textBoxPassword1.Text.Equals(textBoxPassword2.Text))
                        {
                            string UserPassword = textBoxPassword1.Text;
                            string UserLogin = textBoxLogin.Text;

                            UserBindingModel newUser = new UserBindingModel
                            {
                                FIO = textBoxFIO.Text,
                                PasswordHash = UserPassword,
                                UserName = UserLogin,
                                Email = textBoxEmail.Text
                            };
                            Task task = Task.Run(() => ApiClient.PostRequestData<UserBindingModel>("api/Account/Register", newUser));
                            task.ContinueWith((prevTask) => MessageBox.Show("Пользователь зарегистрирован.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
                          TaskContinuationOptions.OnlyOnRanToCompletion);
                            this.Close();
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Пароль должен быть не короче 5 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show("Произошла ошибка регистрации\nОшибка:" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
