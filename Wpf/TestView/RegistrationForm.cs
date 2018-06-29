using System;
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
        private void registrate_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxFIO.TextLength != 0 || textBoxLogin.TextLength < 8 ||
                    textBoxPassword1.TextLength != 0 || textBoxPassword2.TextLength != 0)
                {
                    if (textBoxPassword1.Text.Equals(textBoxPassword2.Text))
                    {
                        string UserPassword = textBoxPassword1.Text;
                        string UserLogin = textBoxLogin.Text;

                        UserBindingModel newUser = new UserBindingModel
                        {
                            FIO = textBoxFIO.Text,
                            PasswordHash = UserPassword,
                            UserName = UserLogin
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
                    DialogResult result = MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show("Произошла ошибка регистрации\nОшибка:" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
