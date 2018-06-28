using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            if (ApiClient.role == "admin")
            {
                FormStatisticsMain formMainAdmin = new FormStatisticsMain();
                formMainAdmin.Show();
                Hide();
            }
            else if (ApiClient.role == "user")
            {
                FormMain formMainUser = new FormMain();
                formMainUser.Show();
                Hide();
            }
        }
    }
}
