﻿using MetroFramework.Forms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormCategoryEdit : MetroForm
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormCategoryEdit()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            ShadowType = MetroFormShadowType.DropShadow;
            if (FormStatisticsMain.DarkTheme)
            {
                Theme = MetroFramework.MetroThemeStyle.Dark;
                label1.ForeColor = Color.White;
                checkBoxNotActive.ForeColor = Color.White;
            }
            else
            {
                Theme = MetroFramework.MetroThemeStyle.Light;
                label1.ForeColor = Color.Black;
                checkBoxNotActive.ForeColor = Color.Black;
            }
        }
        private async void Initialize()
        {

            if (id.HasValue)
            {
                try
                {
                    var category = await ApiClient.GetRequestData<CategoryViewModel>("api/Category/Get/" + id.Value);
                    textBox1.Text = category.Name;
                    checkBoxNotActive.Checked = !category.Active;
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
                bool active = !checkBoxNotActive.Checked;
                if (id.HasValue)
                {
                    await ApiClient.PostRequestData("api/Category/UpdElement", new CategoryBindingModel
                    {
                        Id = id.Value,
                        Name = name,
                        Active = active
                    });
                }
                else
                {
                    await ApiClient.PostRequestData("api/Category/AddElement", new CategoryBindingModel
                    {
                        Name = name,
                        Active = active
                    });
                }
                MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
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

        private void FormCategoryEdit_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
