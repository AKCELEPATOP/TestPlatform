using System;
using System.Windows.Forms;
using TestService;

namespace TestView
{
    public partial class FormAuthorization : Form
    {
        public FormAuthorization()
        {
            InitializeComponent();
 
 
 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form formRegistration = new RegistrationForm();
            formRegistration.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ApiClient.Login(textBoxLogin.Text, textBoxPassword.Text);
                if (ApiClient.Role.Equals(ApplicationRoles.Admin) || ApiClient.Role.Equals(ApplicationRoles.SuperAdmin))
                {
                    FormStatisticsMain formMainAdmin = new FormStatisticsMain(this);
                    formMainAdmin.Show();
                    Hide();
                }
                else if (ApiClient.Role.Equals(ApplicationRoles.User))
                {
                    FormMain formMainUser = new FormMain(this);
                    formMainUser.Show();
                    formMainUser.UserLogin = ApiClient.UserName;
                    Hide();
                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show("Произошла ошибка авторизации\nОшибка:" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            textBoxLogin.Text = "";
            textBoxPassword.Text = "";
        }

    }
}
