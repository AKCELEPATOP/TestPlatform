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

            sign_In.BackColor = Design.Invert(this.ForeColor);
            sign_Up.BackColor = Design.Invert(this.ForeColor);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Form formRegistration = new RegistrationForm();
            Design.SetBackColor(formRegistration);
            Design.SetFontColor(formRegistration);
            formRegistration.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ApiClient.Login(textBoxLogin.Text, textBoxPassword.Text);
                if (ApiClient.Role.Equals(ApplicationRoles.SuperAdmin) && textBoxPassword.Text.Equals("Admin777")) {
                    FormChangedGlavAdmin formMainAdmin = new FormChangedGlavAdmin(this);
                    formMainAdmin.Show();
                    Hide();
                }

                if (ApiClient.Role.Equals(ApplicationRoles.Admin) || ApiClient.Role.Equals(ApplicationRoles.SuperAdmin) && !textBoxPassword.Text.Equals("Admin777"))
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
                textBoxLogin.Clear();
                textBoxPassword.Clear();
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show("Произошла ошибка авторизации\nОшибка:" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassword.Clear();
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            textBoxLogin.Text = "";
            textBoxPassword.Text = "";
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            sign_In.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
            sign_Up.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));

        }
    }
}
