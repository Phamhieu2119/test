using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace BCVlxd
{
   
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            login lg = new login(Pannellogin);
            lg.Dock = DockStyle.Fill;
            Pannellogin.Controls.Clear();
            Pannellogin.Controls.Add(lg);
            lg.BringToFront();



        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void Pannellogin_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Pannellogin_Paint_1(object sender, PaintEventArgs e)
        {
            // Đặt màu nền của Guna2Panel với độ trong suốt
            guna2Panel1.BackColor = Color.FromArgb(80, Color.White);

            // Sử dụng đối tượng LinearGradientBrush để tạo hiệu ứng trong suốt
            System.Drawing.Drawing2D.LinearGradientBrush brush =
                new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Point(0, 0),
                    new Point(guna2Panel1.Width, guna2Panel1.Height),
                    Color.FromArgb(80, Color.White),
                    Color.FromArgb(80, Color.White));

            // Vẽ hình chữ nhật bằng brush trong suốt
            e.Graphics.FillRectangle(brush, 0, 0, guna2Panel1.Width, guna2Panel1.Height);

        }
    }
}
