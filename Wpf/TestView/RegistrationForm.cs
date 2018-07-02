using MetroFramework.Forms;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;

namespace TestView
{
    public partial class RegistrationForm : MetroForm
    {
        public RegistrationForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            ShadowType = MetroFormShadowType.DropShadow;

           
        }
        private async void registrate_Click(object sender, EventArgs e)
        {
            string fio = textBoxFIO.Text;
            string mail = textBoxEmail.Text;
            string message = string.Empty;
            if (!string.IsNullOrEmpty(fio) || !Regex.IsMatch(fio, @"\D{1,} \D{1,} \D{1,}"))
            {
                message += " Неверный формат для ФИО.";
            }
            if (!string.IsNullOrEmpty(mail))
            {
                if (!Regex.IsMatch(mail, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                {
                    message += " Неверный формат для электронной почты.";
                }
            }
            string login = textBoxLogin.Text;
            if (!Regex.IsMatch(login, @"\w{8,}"))
            {
                message += " Логин должен быть не меньше 8 символов.";
            }

            string password = textBoxPassword1.Text;
            if (!Regex.IsMatch(password, @"^(?=.*[0-9]$)(?=.*[a-zA-Z])"))
            {
                message += " Пароль должен быть не короче 5 символов, содержать хотя бы одну лат букву в ниж регистре и одну цифру.";
            }
            if (!password.Equals(textBoxPassword2.Text))
            {
                message += "Пароли должны совпадать.";
            }
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                await ApiClient.PostRequestData<UserBindingModel>("api/Account/Register", newUser);
                MessageBox.Show("Пользователь зарегистрирован.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
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

}
