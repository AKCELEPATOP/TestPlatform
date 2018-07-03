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
using TestService.ViewModels;

namespace TestView
{
    public partial class FormGroupEdit : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormGroupEdit()
        {
            InitializeComponent();
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
            textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
        }

        private async void Initialize() {
            if (id.HasValue)
            {
                try
                {
                    var group = await ApiClient.GetRequestData<GroupViewModel>("api/Group/Get/" + id.Value);
                    textBox1.Text = group.Name;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Заполните Название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string name = textBox1.Text;
                if (id.HasValue)
                {
                    await ApiClient.PostRequestData("api/Group/UpdElement", new GroupBindingModel
                    {
                        Id = id.Value,
                        Name = name

                    });
                }
                else
                {
                    await ApiClient.PostRequestData("api/Group/AddElement", new GroupBindingModel
                    {
                        Name = name
                    });
                }
                MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch(Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
