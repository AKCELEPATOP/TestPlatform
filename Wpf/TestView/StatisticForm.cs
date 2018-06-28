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
    public partial class StatisticForm : Form
    {
        public StatisticForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                // Переделать или удалить
                List<TestViewModel> list =
                    Task.Run(() => ApiClient.GetRequestData<List<TestViewModel>>("api/PassedTests/GetList")).Result;
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

            DrawOs();
        }


        public int n; //Число участков графика   
        public int uvel; // во сколько раз увеличить график
        public int centrX, centrY;
        public int x, y;
        public int funkcii;
        public void DrawOs()
        {
            int wX;
            int hX;
            double xF, yF;
            double step;

            wX = pictureBox1.Width;  //Значение ширины
            hX = pictureBox1.Height; //Значение высоты

            // Система Координат
            Bitmap graph = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics OSIGraphics = Graphics.FromImage(graph);
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            OSIGraphics.DrawLine(myPen, 0, (int)(hX - 10), wX, (int)(hX - 10)); // Горизонталь
            OSIGraphics.DrawLine(myPen, wX, (int)(hX - 10), wX - 10, (int)(hX - 15));
            OSIGraphics.DrawLine(myPen, wX, (int)(hX - 10), wX - 10, (int)(hX - 5));

            OSIGraphics.DrawLine(myPen, (int)(10), 0, (int)(10), hX); // Вертикаль
            OSIGraphics.DrawLine(myPen, 10, 0, 5, 15);
            OSIGraphics.DrawLine(myPen, 10, 0, 15, 15);


            // График (тестовый)
            for (step = 0; step <= 2 * Math.PI; step += 0.001)
            {
                xF = (step * 25) + (int)(wX / 2);
                double tmp = Math.Sin(step);
                tmp *= 50;
                yF = (int)(hX / 2) - tmp;
                graph.SetPixel((int)xF, (int)yF, Color.Red);
            }
            pictureBox1.Image = graph;
        }


        private void back_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                // Переделать или удалить
                int Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                List<TestViewModel> list =
                Task.Run(() => ApiClient.GetRequestData<List<TestViewModel>>("api/PassedTests/GetList/" + Id)).Result;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
    }
}
