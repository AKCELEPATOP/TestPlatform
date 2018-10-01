using System.Drawing;
using System.Windows.Forms;

namespace TestView
{
    public static class Design
    {
        private static Color BackColor = Color.White;
        private static Color FontColor = Color.Black;
        public static int FontSize { get; set; } = 8;

        public static void SetDefaultBackColor(Color backcolor)
        {
            BackColor = backcolor;
        }
        public static void SetDefaultFontColor(Color fontColor)
        {
            FontColor = fontColor;
        }
        
        public static void SetBackColor(Form owner)
        {
            var manager = new Form.ControlCollection(owner);
            owner.BackColor = BackColor;
        }
        public static void SetFontColor(Form owner)
        {
            var manager = new Form.ControlCollection(owner);
            owner.ForeColor = FontColor;
        }
        public static Color Invert(Color c)
        {
            return Color.FromArgb(c.A, 0xFF - c.R, 0xFF - c.G, 0xFF - c.B);
        }
    }
}
