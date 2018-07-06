using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class StatisticForm : Form
    {

        private Series series1;
        public StatisticForm() => InitializeComponent();
        StatChartViewModel stat;
        List<Series> seriesmas;
        List<string> distinct;
        private async void Initialize()
        {
            var model = new GetListModel { Take = 50 };
            stat = await ApiClient.PostRequestData<GetListModel, StatChartViewModel>("api/Stat/GetUserChart", model);


            List<SeriesChartType> list = new List<SeriesChartType>() {
                SeriesChartType.Column,SeriesChartType.FastLine,SeriesChartType.Line,SeriesChartType.StepLine, SeriesChartType.Area, SeriesChartType.Candlestick
            };
             distinct = stat.TestName.Distinct().ToList();
            
            List<Color> C = new List<Color>(distinct.Count);
              Random r = new Random();
            seriesmas = new List<Series>(distinct.Count);
            for (int i = 0; i < distinct.Count; i++)
            {

                C.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));
                seriesmas.Add(new Series(distinct[i]));
                seriesmas[i].ChartArea = "График";
                seriesmas[i].Color = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));

                seriesmas[i].ChartType = SeriesChartType.Column;
            }
            comboBox1.DataSource = list;
           
            DrawOs();

            back.BackColor = Design.Invert(this.ForeColor);
            comboBox1.BackColor = Design.Invert(this.ForeColor);
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", Design.FontSize);
        }




        public int n;  
        public int uvel; 
        public int centrX, centrY;
        public int x, y;
        public int funkcii;
        public void DrawOs()
        {




            try
            {



                float wX;
                float hX;
                int step;
                double yF;
                wX = chart1.Width;  //Значение ширины
                hX = chart1.Height; //Значение высоты




                
                chart1.Parent = this;
         
                chart1.ChartAreas.Add(new ChartArea("График"));
                chart1.ChartAreas[0].AxisY.Maximum = 100;
                chart1.ChartAreas[0].AxisY.Interval = 5;
              
              
                
                //if()
                List<Color> C = new List<Color>(distinct.Count);



                int z = 0;    

                for (step = 0; step < stat.Results.Count; step++)
                {

                    double tmp = stat.Results[step];
                    yF = tmp * 100;
                    for (int i = 0; i < distinct.Count; i++)
                    {
                        if (stat.TestName[step].Equals(distinct[i])) {
                            z = i;
                        }

                     }
                    seriesmas[z].Points.AddXY(stat.Dates[step], yF);
                  

                }
                
                chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
           
                for (int i = 0; i < distinct.Count; i++)
                {
                    chart1.Series.Add(seriesmas[i]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при загрузке графика Ошибка:" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }







        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < distinct.Count; i++)
            {
                seriesmas[i].ChartType = (SeriesChartType)comboBox1.SelectedValue;
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
            back.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(Design.FontSize));
        }

        private void back_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}