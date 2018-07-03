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
    public partial class StatisticForm : Form
    {

        public static bool DarkTheme { get { return DarkTheme; } set { DarkTheme = false; } }
        public StatisticForm()
        {
            InitializeComponent();



            if (FormMain.DarkTheme)
            {

                label5.ForeColor = Color.White;
            }
            else
            {

                label5.ForeColor = Color.Black;
            }
            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                // Переделать или удалить
                StatChartViewModel list =
                         await ApiClient.PostRequestData<GetListModel, StatChartViewModel>("api/Stat/GetUserChart", model);
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
        public async void DrawOs()
        {
            float wX;
            float hX;
            double xF, yF;
            int step;

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

            try
            {
                var model = new GetListModel { };
                // График
                StatChartViewModel stat =
                        await ApiClient.PostRequestData<GetListModel, StatChartViewModel>("api/Stat/GetUserChart", model);

                if (stat.Results != null)
                {
                    if (stat.Results.Count > 50)
                    {
                        stat.Results.RemoveRange(0, stat.Results.Count - 50);
                    }
                    for (step = 0; step <= stat.Results.Count; step++)
                    {
                        xF = (step * 25) + (int)(wX / 2);
                        double tmp = stat.Results[step];
                        tmp *= 50;
                        yF = (int)(hX / 2) - tmp;
                        graph.SetPixel((int)xF, (int)yF, Color.Red);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при загрузке графика Ошибка:"+ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            pictureBox1.Image = graph;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            StatChartViewModel stat =
                        await ApiClient.PostRequestData<GetListModel, StatChartViewModel>("api/Stat/GetUserChart", model);
            StatViewModel result;
            try
            {
                result = await ApiClient.GetRequestData<StatViewModel>("api/Stat/GetUserChartLast/" + (stat.Results.Count-1).ToString());

                FormResultOfTest resultOfLastTest = new FormResultOfTest(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка загрузки данных"+'\n'+"Ошибка: "+ex.Message,"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void back_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
