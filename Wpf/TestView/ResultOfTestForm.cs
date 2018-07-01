using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestService.ViewModels;

namespace TestView
{
    public partial class FormResultOfTest : Form
    {
        public FormResultOfTest()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {  // Переделать или удалить
            try
            {
                List<ResultOfTestViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<ResultOfTestViewModel>>("api/ResultOfTest/GetList")).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private void back_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                // Переделать или удалить
                int Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                List<ResultOfTestViewModel> list =
                Task.Run(() => ApiClient.GetRequestData<List<ResultOfTestViewModel>>("api/ResultOfTest/GetList/" + Id)).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
