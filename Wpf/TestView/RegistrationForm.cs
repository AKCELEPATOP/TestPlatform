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
            if (textBoxPassword1.Text == textBoxPassword2.Text)
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
            }
            else
            {
                DialogResult result = MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
