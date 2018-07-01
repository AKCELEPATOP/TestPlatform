﻿using System;
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
    public partial class FormCategoryEdit : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormCategoryEdit()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize() {

            if (id.HasValue)
            {
                try
                {
                    var category = Task.Run(() => ApiClient.GetRequestData<CategoryViewModel>("api/Category/Get/" + id.Value)).Result;
                    textBox1.Text = category.Name;

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
            bool active = !checkBoxNotActive.Checked;
            if (id.HasValue) {
                    task = Task.Run(() => ApiClient.PostRequestData("api/Category/UpdElement", new CategoryBindingModel
                    {
                        Id = id.Value,
                        Name=name,
                        Active = active
                        
                    }));
            } else {
                task = Task.Run(() => ApiClient.PostRequestData("api/Category/AddElement", new CategoryBindingModel
                {
                    Name = name,
                    Active = active
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
