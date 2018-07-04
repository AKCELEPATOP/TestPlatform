﻿using System;
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

        private  Series series1;
        public StatisticForm() => InitializeComponent();

        private async void Initialize()
        {
            var model = new GetListModel { Take = 50 };

            StatChartViewModel stat1;

            List<SeriesChartType> list = new List<SeriesChartType>() {
                SeriesChartType.Column,SeriesChartType.FastLine,SeriesChartType.Line,SeriesChartType.StepLine, SeriesChartType.Area, SeriesChartType.Candlestick
            };
            series1 = new Series("График");
            series1.ChartType = SeriesChartType.Column;
            comboBox1.DataSource = list;
            stat1 = await ApiClient.PostRequestData<GetListModel, StatChartViewModel>("api/Stat/GetUserChart", model);
            DrawOs(stat1);
        }

        


        public int n; //Число участков графика   
        public int uvel; // во сколько раз увеличить график
        public int centrX, centrY;
        public int x, y;
        public int funkcii;
        public void DrawOs(StatChartViewModel stat)
        {

            
                        

            try
            {

               

                float wX;
                float hX;
                int step;
                double yF;
                wX = chart1.Width;  //Значение ширины
                hX = chart1.Height; //Значение высоты




                //кладем его на форму и растягиваем на все окно.
                chart1.Parent = this;
                //добавляем в Chart область для рисования графиков, их может быть
                //много, поэтому даем ей имя.
                chart1.ChartAreas.Add(new ChartArea("График"));
                //Создаем и настраиваем набор точек для рисования графика, в том
                //не забыв указать имя области на которой хотим отобразить этот
                //набор точек.

               
                series1.ChartArea = "График";
                chart1.ChartAreas[0].AxisY.Maximum = 100;
                chart1.ChartAreas[0].AxisY.Interval = 5;

                for (step = 0; step < stat.Results.Count; step++)
                {

                    double tmp = stat.Results[step];
                    yF = tmp*100;
 
                    series1.Points.AddXY(stat.Dates[step],yF);
                }
                chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                //Добавляем созданный набор точек в Chart
                chart1.Series.Add(series1);

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
            series1.ChartType =(SeriesChartType) comboBox1.SelectedValue;
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
