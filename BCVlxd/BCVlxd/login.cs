using connectdatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BCVlxd
{
    public partial class login : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        public Panel panel;
        public login(Panel panel)
        {
            InitializeComponent();
            this.panel = panel;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            

        }

        

        

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            int transparency1 = 60; // Giả sử bạn muốn độ trong suốt là 80

            this.BackColor = Color.FromArgb(transparency1, Color.White);

            LinearGradientBrush brush =
                new LinearGradientBrush(
                    new Point(0, 0),
                    new Point(this.Width, this.Height),
                    Color.FromArgb(transparency1, Color.White),
                    Color.FromArgb(transparency1, Color.White));

            e.Graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void login_Paint(object sender, PaintEventArgs e)
        {
            int transparency = 80; // Giả sử bạn muốn độ trong suốt là 80

            this.BackColor = Color.FromArgb(transparency, Color.White);

            LinearGradientBrush brush =
                new LinearGradientBrush(
                    new Point(0, 0),
                    new Point(this.Width, this.Height),
                    Color.FromArgb(transparency, Color.White),
                    Color.FromArgb(transparency, Color.White));

            e.Graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Vui lòng điền tài khoản !");
                txtName.Focus();
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show("Vui lòng điền mật khẩu !");
                txtPassword.Focus();
            }
            else
            {
                pd.ketnoi();
                string sql2 = "SELECT COUNT(*) FROM Dangky WHERE Taikhoan=N'" + txtName.Text.ToString() + "'";
                SqlCommand checkTxtTK = new SqlCommand(sql2, pd.Con);
                int tenCount = (int)checkTxtTK.ExecuteScalar();
                if (tenCount > 0)
                {

                    pd.ketnoi();
                    string sql1 = "SELECT COUNT(*) FROM Dangky WHERE Taikhoan=N'" + txtName.Text.ToString() + "' and Matkhau =N'" + txtPassword.Text.ToString() + "'";
                    SqlCommand checkTxtemail = new SqlCommand(sql1, pd.Con);
                    int emailCount = (int)checkTxtemail.ExecuteScalar();
                    if (emailCount > 0)
                    {

                        /*if (MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK) == DialogResult.OK)
                        {*/
                        Main form2 = new Main();
                        form2.Show();
                        this.Hide();
                        

                        //}
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu sai vui lòng nhập lại !");
                        txtPassword.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Tên tài khoản sai vui lòng nhập lại !");
                    txtName.Focus();
                }
            }
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {
            DangKyControl dk = new DangKyControl(this.panel);
            dk.Dock = DockStyle.Fill;
            this.panel.Controls.Clear();
            this.panel.Controls.Add(dk);
            dk.BringToFront();
        }

        private void txtPassword_TextChanged_1(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void guna2PictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
        }

        private void guna2PictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }
    }
}
