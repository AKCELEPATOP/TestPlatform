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
    public partial class FormTestTemplateQuestions : Form
    {
        public List<PatternQuestionsBindingModel> listQ { get; set; }
        List<QuestionViewModel> listC;
        public FormTestTemplateQuestions()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize() {
             listC= Task.Run(() => ApiClient.GetRequestData<List<QuestionViewModel>>("api/Question/GetList")).Result;
            if (listC != null)
            {
                dataGridView1.DataSource = listC;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (listQ != null)
            {
                dataGridView2.DataSource = listQ;
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        //>
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                listQ.Add(new PatternQuestionsBindingModel
                {
                    QuestionId = id


                });
            }
        }
        //>>
        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listC.Count; i++) {
                listQ.Add(new PatternQuestionsBindingModel
                {
                    QuestionId = listC[i].Id
                });
            }
        }
        //<
        private void button6_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                listQ.RemoveAt(id);
            }
        }
        //<<
        private void button2_Click(object sender, EventArgs e)
        {
            listQ.Clear();
        }
        //save
        private void button3_Click(object sender, EventArgs e)
        {
            var form = new FormTestTemplate();
            form.listQ = listQ;
            if (form.ShowDialog() == DialogResult.OK)
            {
                Close();
            }
        }
        //nazad
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ПКМ -> Обновить
        private void MouseDown_Form(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }
        private void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
