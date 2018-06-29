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
using TestView;

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
                Hide();
            }
        }
    }
}
