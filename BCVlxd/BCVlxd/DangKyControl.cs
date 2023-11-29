using connectdatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCVlxd
{
    public partial class DangKyControl : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        public Panel panel;

        public DangKyControl(   Panel panel)
        {
            InitializeComponent();
            this.panel = panel;
        }

        private void DangKyControl_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            login lg = new login(this.panel);
            lg.Dock = DockStyle.Fill;
            this.panel.Controls.Clear();
            this.panel.Controls.Add(lg);
            lg.BringToFront();
        }
        private bool checkEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool check()
        {
            if (txtEmail.Text == "" || txtTen.Text == "" || txtMatkhau.Text == "" || txtXacNhanMK.Text == "")
            {
                return false;
            }
            return true;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int checkmk = string.Compare(txtMatkhau.Text.ToString(), txtXacNhanMK.Text.ToString());
            if (checkmk != 0)
            {
                MessageBox.Show("Xác nhận mật khẩu sai !");
                txtXacNhanMK.Focus();
                return;
            }
            if (check())
            {
                pd.ketnoi();
                if (!checkEmail(txtEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập định dạng email đúng!");
                    txtEmail.Focus();
                    return;
                }
                string sql1 = "SELECT COUNT(*) FROM Dangky WHERE Email=N'" + txtEmail.Text.ToString() + "'";
                SqlCommand checkTxtemail = new SqlCommand(sql1, pd.Con);
                int emailCount = (int)checkTxtemail.ExecuteScalar();
                if (emailCount > 0)
                {
                    MessageBox.Show("Email đã tồn tại!");
                    return;
                }
                else
                {
                    string sql2 = "SELECT COUNT(*) FROM Dangky WHERE Taikhoan=N'" + txtTen.Text.ToString() + "'";
                    SqlCommand checkTxtTK = new SqlCommand(sql2, pd.Con);
                    int tenCount = (int)checkTxtTK.ExecuteScalar();
                    if (tenCount > 0)
                    {
                        MessageBox.Show("Tài khoản  đã tồn tại!");
                        return;
                    }
                    else
                    {
                        string sql3 = "Insert into Dangky values(N'" + txtEmail.Text.ToString().Trim() + "',N'" + txtTen.Text.ToString().Trim() + "',N'" + txtMatkhau.Text.ToString().Trim() + "')";

                        pd.capNhat(sql3);


                        //SendEmail(txtEmail.Text, "Đăng ký thành công", "Chúc mừng bạn đã đăng ký thành công!");

                        if (MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK) == DialogResult.OK)
                        {
                            txtEmail.Text = "";
                            txtTen.Text = "";
                            txtMatkhau.Text = "";
                        }
                    }
                }


            }
            else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin !");
                txtEmail.Focus();
            }
        }

        private void txtMatkhau_TextChanged(object sender, EventArgs e)
        {
            txtMatkhau.UseSystemPasswordChar = true;
        }

        private void txtXacNhanMK_TextChanged(object sender, EventArgs e)
        {
            txtXacNhanMK.UseSystemPasswordChar = true;
        }

        private void DangKyControl_Paint(object sender, PaintEventArgs e)
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
    }
}
