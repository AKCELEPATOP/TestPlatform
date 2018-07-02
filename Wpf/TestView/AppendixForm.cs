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

namespace TestView
{
    public partial class AppendixForm : MetroForm
    {
        public AppendixForm()
        {
            InitializeComponent();
            Initialize();
            FormBorderStyle = FormBorderStyle.None;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            ShadowType = MetroFormShadowType.DropShadow;
            if (FormMain.DarkTheme)
            {
                Theme = MetroFramework.MetroThemeStyle.Dark;
                label1.ForeColor = Color.White;
            }
            else
            {
                Theme = MetroFramework.MetroThemeStyle.Light;
                label1.ForeColor = Color.Black;
            }
        }

        private void Initialize()
        {
            //pictureBox1=
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
