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
using TestRestApi.Models;

namespace TestView
{
    public partial class FormChangedGlavAdmin : Form
    {
        private FormAuthorization parent;

        public FormChangedGlavAdmin(FormAuthorization parent)
        {
            this.parent = parent;
            InitializeComponent();
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string password = textBox1.Text;
            if (Regex.IsMatch(password, @"(?=.*[a-z])(?=.*[0-9])^[a-zA-Z0-9]{5,}$"/*@"^(?=.*[0-9]$)(?=.*[a-zA-Z]){5,}"*/))
            {
                textBox1.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                textBox1.BackColor = System.Drawing.Color.IndianRed;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string password2 = textBox2.Text;
            string password1 = textBox1.Text;
            if (password1.Equals(password2) && !string.IsNullOrEmpty(password1) && !string.IsNullOrEmpty(password2))
            {
                textBox2.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.IndianRed;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string password1 = textBox3.Text;
            string password2 = textBox1.Text;
            string password3 = textBox2.Text;
            string message = string.Empty;
            if (!Regex.IsMatch(password2, @"(?=.*[a-z])(?=.*[0-9])^[a-zA-Z0-9]{5,}$"/*@"^(?=.*[0-9]$)(?=.*[a-zA-Z]){5,}"*/))
            {
                message += " Пароль должен быть не короче 5 символов, содержать хотя бы одну лат букву в ниж регистре и одну цифру.";
            }
            if (!password2.Equals(password3))
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
                await ApiClient.PostRequestData("api/Account/ChangePassword", new ChangePasswordBindingModel
                {
                    OldPassword = password1,
                    NewPassword = password2,
                    ConfirmPassword = password3
                });
                MessageBox.Show("Пароль успешно изменён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FormStatisticsMain formMainAdmin = new FormStatisticsMain(parent);
                formMainAdmin.Show();
                Hide();
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

        private void button2_Click(object sender, EventArgs e)
        {
            FormAuthorization formMainAdmin = new FormAuthorization();
            formMainAdmin.Show();
            Hide();
        }
    }
}
