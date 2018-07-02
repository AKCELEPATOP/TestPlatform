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
    public partial class FormTestTemplate : MetroForm
    {
        public int Id { set { id = value; } }

        private int? id;

        private PatternCategoryViewModel current;

        private List<CategoryViewModel> listC;

        private List<PatternCategoryViewModel> listPC;

        private BindingSource source;

        public FormTestTemplate()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            ShadowType = MetroFormShadowType.DropShadow;
            if (FormStatisticsMain.DarkTheme)
            {
                Theme = MetroFramework.MetroThemeStyle.Dark;
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                groupBox1.ForeColor = Color.White;
            }
            else
            {
                Theme = MetroFramework.MetroThemeStyle.Light;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                groupBox1.ForeColor = Color.Black;
            }
        }
        private async void Initialize()
        {
            List<GroupViewModel> list = await ApiClient.GetRequestData<List<GroupViewModel>>("api/Group/GetList");
            if (list != null)
            {
                comboBox1.DataSource = list;
                comboBox1.ValueMember = "Id";
                comboBox1.DisplayMember = "Name";
                comboBox1.SelectedItem = null;
            }
            if (id.HasValue)
            {
                try
                {
                    var pattern = await ApiClient.GetRequestData<PatternViewModel>("api/Pattern/Get/" + id.Value);
                    textBox1.Text = pattern.Name;
                    listPC = pattern.PatternCategories;
                    comboBox1.SelectedItem = pattern.UserGroupId;
                    if (pattern.PatternCategories.Count > 0)
                    {
                        dataGridView2.Rows[0].Selected = true;
                        textBoxEasy.Text = pattern.PatternCategories[0].Easy.ToString();
                        textBoxMid.Text = pattern.PatternCategories[0].Middle.ToString();
                        textBoxDif.Text = pattern.PatternCategories[0].Complex.ToString();
                        textBoxCount.Text = pattern.PatternCategories[0].Count.ToString();
                    }
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
            else
            {
                listPC = new List<PatternCategoryViewModel>();
            }
            source = new BindingSource
            {
                DataSource = listPC
            };
            dataGridView2.DataSource = source;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].Visible = false;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Visible = false;
            dataGridView2.Columns[6].Visible = false;
            dataGridView2.Columns[7].Visible = false;
            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            listC = await ApiClient.GetRequestData<List<CategoryViewModel>>("api/Category/GetList");
            if (listC != null)
            {
                dataGridView1.DataSource = listC;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        //сохранить
        private async void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveCategory();
            string name = textBox1.Text;

            List<PatternQuestionsBindingModel> listQuestions = listPC.SelectMany(rec => rec.PatternQuestions)
                .Select(rec => new PatternQuestionsBindingModel
            {
                QuestionId = rec.QuestionId
            }).ToList();
            try
            {
                if (id.HasValue)
                {
                    List<PatternCategoriesBindingModel> bin = new List<PatternCategoriesBindingModel>(listPC.Count);

                    for (int i = 0; i < listPC.Count; i++)
                    {
                        bin.Add(new PatternCategoriesBindingModel
                        {
                            PatternId = id.Value,
                            CategoryId = listPC[i].CategoryId,
                            Middle = listPC[i].Middle,
                            Copmlex = listPC[i].Complex,
                            Easy = listPC[i].Easy
                        });
                    }
                    if (listQuestions != null && listQuestions.Count != 0)
                    {

                        for (int i = 0; i < listQuestions.Count; i++)
                        {
                            listQuestions[i].PatternId = id.Value;
                        }
                        await ApiClient.PostRequestData("api/Pattern/UpdElement", new PatternBindingModel
                        {
                            Id = id.Value,
                            Name = name,
                            PatternCategories = bin,
                            PatternQuestions = listQuestions
                        });
                    }
                    else
                    {
                        await ApiClient.PostRequestData("api/Pattern/UpdElement", new PatternBindingModel
                        {
                            Id = id.Value,
                            Name = name,
                            PatternCategories = bin
                        });
                    }
                }
                else
                {

                    List<PatternCategoriesBindingModel> bin = new List<PatternCategoriesBindingModel>(listPC.Count);

                    for (int i = 0; i < listPC.Count; i++)
                    {
                        bin.Add(new PatternCategoriesBindingModel
                        {
                            CategoryId = listPC[i].CategoryId,
                            Middle = listPC[i].Middle,
                            Copmlex = listPC[i].Complex,
                            Easy = listPC[i].Easy

                        });
                    }
                    if (listQuestions != null && listQuestions.Count != 0)
                    {
                        await ApiClient.PostRequestData("api/Pattern/AddElement", new PatternBindingModel
                        {
                            Name = name,
                            PatternCategories = bin,
                            PatternQuestions = listQuestions

                        });
                    }
                    else
                    {
                        await ApiClient.PostRequestData("api/Pattern/AddElement", new PatternBindingModel
                        {
                            Name = name,
                            PatternCategories = bin
                        });
                    }

                }
                MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        //отмена 
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listPC.Count == 0)
            {
                MessageBox.Show("Выберите категории", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var form = new FormTestTemplateQuestions();
            foreach (var el in listPC)
            {
                form.listPC.Add(new PatternCategoryViewModel
                {
                    CategoryId = el.CategoryId,
                    CategoryName = el.CategoryName,
                    Complex = el.Complex,
                    Count = el.Count,
                    Easy = el.Easy,
                    Middle = el.Middle,
                    PatternQuestions = new List<PatternQuestionViewModel>(el.PatternQuestions)
                });
            }
            if (form.ShowDialog() == DialogResult.OK)
            {
                listPC = form.listPC;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ChangeCategory();
        }

        private void ChangeCategory()
        {
            if (dataGridView2.SelectedRows.Count == 1)
            {
                if (current != null)
                {
                    SaveCategory();
                }

                current = listPC.FirstOrDefault(rec => rec.CategoryId == Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[2].Value));
                textBoxEasy.Text = current.Easy.ToString();
                textBoxMid.Text = current.Middle.ToString();
                textBoxDif.Text = current.Complex.ToString();
                textBoxCount.Text = current.Count.ToString();
            }
        }

        private int Sum()
        {
            int easy = 0;
            int mid = 0;
            int complex = 0;
            if ((!string.IsNullOrEmpty(textBoxEasy.Text) && !Int32.TryParse(textBoxEasy.Text, out easy)) || easy < 0 ||
                (!string.IsNullOrEmpty(textBoxMid.Text) && !Int32.TryParse(textBoxMid.Text, out mid)) || mid < 0 ||
                (!string.IsNullOrEmpty(textBoxDif.Text) && !Int32.TryParse(textBoxDif.Text, out complex)) || complex < 0)
            {
                MessageBox.Show("Неверные значения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            int sum = easy + mid + complex;
            textBoxCount.Text = sum.ToString();
            return sum;
        }
        private void textBoxEasy_TextChanged(object sender, EventArgs e)
        {
            Sum();
        }

        private void textBoxDif_TextChanged(object sender, EventArgs e)
        {
            Sum();
        }

        private void textBoxMid_TextChanged(object sender, EventArgs e)
        {
            Sum();
        }
        // >
        private void button1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (listPC.Select(rec => rec.CategoryId).Contains(Id))
                {
                    return;
                }
                if (id.HasValue)
                {
                    listPC.Add(new PatternCategoryViewModel
                    {
                        PatternId = id.Value,
                        CategoryId = Id,
                        CategoryName = listC.FirstOrDefault(rec => rec.Id == Id).Name,
                        PatternQuestions = new List<PatternQuestionViewModel>()


                    });
                }
                else
                {
                    listPC.Add(new PatternCategoryViewModel
                    {
                        CategoryId = Id,
                        CategoryName = listC.FirstOrDefault(rec => rec.Id == Id).Name,
                        PatternQuestions = new List<PatternQuestionViewModel>()
                    });
                }
                source.ResetBindings(false);
                dataGridView2.Rows[listPC.Count-1].Selected = true;
                ChangeCategory();
            }
        }
        // >>
        private void button5_Click(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                for (int i = 0; i < listC.Count; i++)
                {
                    if (listPC.Select(rec => rec.CategoryId).Contains(listC[i].Id))
                    {
                        continue;
                    }
                    listPC.Add(new PatternCategoryViewModel
                    {
                        PatternId = id.Value,
                        CategoryId = listC[i].Id,
                        CategoryName = listC[i].Name,
                        PatternQuestions = new List<PatternQuestionViewModel>()

                    });
                }
            }
            else
            {

                for (int i = 0; i < listC.Count; i++)
                {
                    if (listPC.Select(rec => rec.CategoryId).Contains(listC[i].Id))
                    {
                        continue;
                    }
                    listPC.Add(new PatternCategoryViewModel
                    {
                        CategoryId = listC[i].Id,
                        CategoryName = listC[i].Name,
                        PatternQuestions = new List<PatternQuestionViewModel>()
                    });
                }
            }
            source.ResetBindings(false);
            if (listPC!=null && listPC.Count > 0)
            {
                dataGridView2.Rows[0].Selected = true;
                ChangeCategory();
            }
        }
        // <
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 1)
            {
                int Id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[2].Value);
                listPC.Remove(listPC.FirstOrDefault(rec => rec.CategoryId == Id));
                source.ResetBindings(false);
            }
            if (listPC != null && listPC.Count > 0)
            {
                dataGridView2.Rows[0].Selected = true;
                ChangeCategory();
            }
        }
        // <<
        private void button2_Click(object sender, EventArgs e)
        {
            listPC.Clear();
            source.ResetBindings(false);
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

        private void SaveCategory()
        {
            if (dataGridView2.SelectedRows.Count == 1)
            {
                int count = Sum();
                if (count < 0)
                {
                    MessageBox.Show("Заполните количество вопросов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (current != null)
                {
                    current.Count = count;
                    current.Easy = Convert.ToInt32(textBoxEasy.Text);
                    current.Middle = Convert.ToInt32(textBoxMid.Text);
                    current.Complex = Convert.ToInt32(textBoxDif.Text);
                }
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
