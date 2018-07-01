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
    public partial class FormTestTemplate : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private int idCat;

        private int check;
        private List<CategoryViewModel> listC;
        private List<PatternCategoryViewModel> listPC;
        public List<PatternQuestionsBindingModel> listQ { get; set; }
        public FormTestTemplate()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            List<GroupViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<GroupViewModel>>("api/Group/GetList")).Result;
            if (list != null)
            {
                comboBox1.DataSource = list;
                comboBox1.ValueMember = "Id";
                comboBox1.DisplayMember = "Name";
                comboBox1.SelectedValue = null;
            }
            if (id.HasValue)
            {
                try
                {
                    var pattern = Task.Run(() => ApiClient.GetRequestData<PatternViewModel>("api/Pattern/Get/" + id.Value)).Result;
                    textBox1.Text = pattern.Name;
                    listPC = pattern.PatternCategories;
                    dataGridView2.DataSource = listPC;
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[2].Visible = false;
                    dataGridView2.Columns[4].Visible = false;//mistake
                    dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    comboBox1.SelectedItem = pattern.UserGroupId;

                    textBoxEasy.Text = pattern.PatternCategories[0].Easy.ToString();
                    textBoxMid.Text = "";
                    textBoxDif.Text = "";
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
            listC =Task.Run(() => ApiClient.GetRequestData<List<CategoryViewModel>>("api/Category/GetList")).Result;
            if (listC != null)
            {
                dataGridView1.DataSource = listC;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[4].Visible = false;//mistake
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        //сохранить
        private void button3_Click(object sender, EventArgs e)
        {
            if (sum() && textBox1.Text==null)
            {
                MessageBox.Show("Неверные значения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Task task;
            string name = textBox1.Text;
            if (id.HasValue)
            {
                List<PatternCategoriesBindingModel> bin = new List<PatternCategoriesBindingModel>(listPC.Count);

                for (int i = 0; i < listPC.Count; i++) {
                    bin.Add(new PatternCategoriesBindingModel {
                        PatternId = id.Value,
                        CategoryId = listC[i].Id,
                        Count = listPC[idCat].Count,
                        Middle = listPC[idCat].Middle,
                        Copmlex = listPC[idCat].Complex


                    });
                }
                if (listQ.Count != 0)
                {

                    for (int i = 0; i < listQ.Count; i++)
                    {
                        listQ[i].PatternId = id.Value;
                    }
                    task = Task.Run(() => ApiClient.PostRequestData("api/Pattern/UpdElement", new PatternBindingModel
                    {
                        Id = id.Value,
                        Name = name,
                        PatternCategories = bin,
                        PatternQuestions = listQ


                    }));
                }
                else {
                    task = Task.Run(() => ApiClient.PostRequestData("api/Pattern/UpdElement", new PatternBindingModel
                    {
                        Id = id.Value,
                        Name = name,
                        PatternCategories = bin


                    }));
                }
            }
            else
            {

                List<PatternCategoriesBindingModel> bin = new List<PatternCategoriesBindingModel>(listPC.Count);

                for (int i = 0; i < listPC.Count; i++)
                {
                    bin.Add(new PatternCategoriesBindingModel
                    {
                        CategoryId = listC[i].Id,
                        Count = listPC[idCat].Count,
                        Middle = listPC[idCat].Middle,
                        Copmlex = listPC[idCat].Complex


                    });
                }
                if (listQ.Count != 0)
                {
                    task = Task.Run(() => ApiClient.PostRequestData("api/Pattern/AddElement", new PatternBindingModel
                    {
                        Name = name,
                        PatternCategories = bin,
                        PatternQuestions=listQ

                    }));
                }
                else
                {
                    task = Task.Run(() => ApiClient.PostRequestData("api/Pattern/AddElement", new PatternBindingModel
                    {
                        Name = name,
                        PatternCategories = bin

                    }));
                }

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
        //отмена 
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var form = new FormTestTemplateQuestions();
            form.listQ =listQ;
            if (form.ShowDialog() == DialogResult.OK)
            {
                Initialize();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 1)
            {
                idCat = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                

                textBoxEasy.Text = listPC[idCat].Easy.ToString();
                textBoxMid.Text = listPC[idCat].Middle.ToString();
                textBoxDif.Text = listPC[idCat].Complex.ToString();
                textBoxCount.Text = listPC[idCat].Count.ToString();


            }
        }

        private bool sum() {
            if ((Convert.ToDouble(textBoxMid.Text) > 1 && Convert.ToDouble(textBoxEasy.Text) > 1 && Convert.ToDouble(textBoxDif.Text) > 1) && (Convert.ToDouble(textBoxMid.Text) <0 && Convert.ToDouble(textBoxEasy.Text) < 0 && Convert.ToDouble(textBoxDif.Text) < 0)) {
                MessageBox.Show("Неверные значения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            double text1 = Convert.ToDouble(textBoxEasy.Text);
            double text2 = Convert.ToDouble(textBoxMid.Text);
            double text3 = Convert.ToDouble(textBoxDif.Text);
            double proc=0;
            switch (check) {
                case 1:
                     proc = text2+text3;
                    if (proc + text1 > 1) {
                        text2 /= 2;
                        text3 /= 2;
                    }
                    break;
                case 2:
                    proc = text1 + text3;
                    if (proc + text2 > 1)
                    {
                        text1 /= 2;
                        text3 /= 2;
                    }
                    break;
                case 3:
                     proc = text2 + text1;
                    if (proc + text3 > 1)
                    {
                        text2 /= 2;
                        text1 /= 2;
                    }
                    break;
                default:
                    check = 0;
                    break;
            }
            check = 0;
            if (proc == 1) {
                return true;
            }
            else{
                return false;
            }
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
            check = 1;
            sum();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
            check = 2;
            sum();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
            check = 3;
            sum();
        }
        // >
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (id.HasValue)
                {
                    listPC.Add(new PatternCategoryViewModel
                    {
                        PatternId=id.Value,
                        CategoryId=listC[Id].Id,
                        CategoryName= listC[Id].Name


                    });
                }
                else {
                    listPC.Add(new PatternCategoryViewModel
                    {
                        CategoryId = listC[Id].Id,
                        CategoryName = listC[Id].Name
                    });
                }
            }
        }
        // >>
        private void button5_Click(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                for (int i = 0; i < listC.Count; i++)
                {
                    listPC.Add(new PatternCategoryViewModel
                    {
                        PatternId = id.Value,
                        CategoryId = listC[i].Id,
                        CategoryName = listC[i].Name


                    });
                }
            }
            else {
                for (int i = 0; i < listC.Count; i++)
                {
                    listPC.Add(new PatternCategoryViewModel
                    {
                        CategoryId = listC[i].Id,
                        CategoryName = listC[i].Name
                    });
                }
            }
        }
        // <
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                listPC.RemoveAt(Id);
                idCat = -1;
            }
        }
        // <<
        private void button2_Click(object sender, EventArgs e)
        {
            listPC.Clear();
        }

        // ПКМ -> Обновить
        private void MouseDown_Form(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        //Сохранить категорию в шаблон 
        private void button8_Click(object sender, EventArgs e)
        {
            if (textBoxCount.Text == null && sum())
            {
                MessageBox.Show("Неверные значения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
              
            }
            listPC[idCat].Count=Convert.ToInt32(textBoxCount.Text);
            listPC[idCat].Easy= Convert.ToDouble(textBoxEasy.Text);
            listPC[idCat].Middle= Convert.ToDouble(textBoxMid.Text);
            listPC[idCat].Complex = Convert.ToDouble(textBoxDif.Text);


            idCat = -1;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
