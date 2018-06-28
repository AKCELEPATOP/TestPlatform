using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                string UserPassword = textBoxLogin.Text;
                string UserLogin = textBoxLogin.Text;
            }
            else
            {
                DialogResult result = MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
