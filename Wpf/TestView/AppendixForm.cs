using System;
using System.Drawing;
using System.Web;
using System.Windows.Forms;
using TestService.BindingModels;
using TestService.ViewModels;

namespace TestView
{
    public partial class AppendixForm : Form
    {

        private Image image;


        public AppendixForm(Image image)
        {
            InitializeComponent();
            this.image = image;
        }

        private void Initialize()
        {
                pictureBox1.Image = image;         
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
