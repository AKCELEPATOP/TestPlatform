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

        AttachmentViewModel attachment;
        public AppendixForm(AttachmentViewModel model)
        {
            InitializeComponent();
            this.attachment = model;
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                var buffer = Convert.FromBase64String(attachment.Image);
                HttpPostedFileBase objFile = (HttpPostedFileBase)new MemoryPostedFile(buffer);
                var image = Image.FromStream(objFile.InputStream, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка"+'\n'+"Ошибка: "+ex.Message, "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
