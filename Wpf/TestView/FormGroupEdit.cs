using MetroFramework.Forms;
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
    public partial class FormGroupEdit : MetroForm
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormGroupEdit()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            Initialize();
        }

        private void Initialize() {
            if (id.HasValue)
            {
                try
                {
                    var group = Task.Run(() => ApiClient.GetRequestData<GroupViewModel>("api/Group/Get/" + id.Value)).Result;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Заполните Название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Task task;
            string name = textBox1.Text;
            if (id.HasValue)
            {



                task = Task.Run(() => ApiClient.PostRequestData("api/Group/UpdElement", new GroupBindingModel
                {
                    Id = id.Value,
                    Name = name

                }));


            }
            else
            {
                task = Task.Run(() => ApiClient.PostRequestData("api/Group/AddElement", new CategoryBindingModel
                {
                    Name = name

                }));

            }
            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
