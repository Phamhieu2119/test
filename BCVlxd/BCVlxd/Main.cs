using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace BCVlxd
{
    public partial class Main : Form
    {
        private Panel overlayPanel;
        private ChitietXuatkho chitietXuatkho;
        private ChitietNhapkho chitietNhapkho;
        public Main()
        {
            InitializeComponent();
            overlayPanel = new Panel();
            overlayPanel.BackColor = Color.FromArgb(128, Color.White); // Màu nền với độ trong suốt
            overlayPanel.Dock = DockStyle.Fill;
            overlayPanel.Visible = false;
            this.Controls.Add(overlayPanel);
            name.Text = "";
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất không ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
        }
        private void ChangeColor(Guna.UI2.WinForms.Guna2Button selectedButton)
        {
            foreach (Control control in buttonPanel.Controls)
            {
                if (control is Guna.UI2.WinForms.Guna2Button gunaButton)
                {
                    gunaButton.FillColor = Color.White; // Màu mặc định
                    gunaButton.ForeColor = Color.Black;
                }
            }

            selectedButton.FillColor = Color.Pink; // Màu khi được chọn
            selectedButton.ForeColor = Color.White;
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            
        }

        private void btnQLKhachhang_BackColorChanged(object sender, EventArgs e)
        {
            btnQLKhachhang.BackColor = Color.Pink;
        }

        private void btnQLKhachhang_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            QLkhachhang qLkhachhang = new QLkhachhang();
            qLkhachhang.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(qLkhachhang);
            qLkhachhang.BringToFront();
        }

        private void QLNhanvien_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            QLNhanvien qLNhanvien = new QLNhanvien();
            qLNhanvien.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(qLNhanvien);
            qLNhanvien.BringToFront();
        }

        private void QLKhohang_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            QLKhohang qLKhohang = new QLKhohang();
            qLKhohang.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(qLKhohang);
            qLKhohang.BringToFront();
        }

        private void QLSanpham_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            QLSanpham qLSanpham = new QLSanpham();
            qLSanpham.TopLevel = false;
            qLSanpham.FormBorderStyle = FormBorderStyle.None;
            qLSanpham.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(qLSanpham);
            MainPanel.Tag = qLSanpham;
            qLSanpham.BringToFront();
            qLSanpham.Show();
        }

        private void QLXuatkho_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            QLXuatkho qLXuatkho = new QLXuatkho(this);
            qLXuatkho.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(qLXuatkho);
            qLXuatkho.BringToFront();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void ShowForm3()
        {
            name.Text = "Chi tiết xuất kho";
            // Hiển thị Form3
            //chitietXuatkho.ShowDialog();
            QLXuatkho.FillColor = Color.Pink; // Màu khi được chọn
            QLXuatkho.ForeColor = Color.White;
            chitietXuatkho = new ChitietXuatkho(this);
            chitietXuatkho.TopLevel = false;
            chitietXuatkho.FormBorderStyle = FormBorderStyle.None;
            chitietXuatkho.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(chitietXuatkho);
            MainPanel.Tag = chitietXuatkho;
            chitietXuatkho.BringToFront();
            chitietXuatkho.Show();

        }
        public void ShowChiTietNK()
        {
            name.Text = "Chi tiết nhập kho";
            // Hiển thị Form3
            //chitietXuatkho.ShowDialog();
            QLNhapkho.FillColor = Color.Pink; // Màu khi được chọn
            QLNhapkho.ForeColor = Color.White;
            chitietNhapkho = new ChitietNhapkho(this);
            chitietNhapkho.TopLevel = false;
            chitietNhapkho.FormBorderStyle = FormBorderStyle.None;
            chitietNhapkho.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(chitietNhapkho);
            MainPanel.Tag = chitietXuatkho;
            chitietNhapkho.BringToFront();
            chitietNhapkho.Show();
        }

        public void showTimKiemHDX()
        {
            QLXuatkho.FillColor = Color.Pink; // Màu khi được chọn
            QLXuatkho.ForeColor = Color.White;
            name.Text = "Tìm kiếm HĐX";
            TimKiemHoaDonXuat timKiemHoaDonXuat = new TimKiemHoaDonXuat(this);
            timKiemHoaDonXuat.TopLevel = false;
            timKiemHoaDonXuat.FormBorderStyle = FormBorderStyle.None;
            timKiemHoaDonXuat.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(timKiemHoaDonXuat);
            MainPanel.Tag = timKiemHoaDonXuat;
            timKiemHoaDonXuat.BringToFront();
            timKiemHoaDonXuat.Show();
        }
        public void showTimKiemHDN()
        {
            QLNhapkho.FillColor = Color.Pink; // Màu khi được chọn
            QLNhapkho.ForeColor = Color.White;
            name.Text = "Tìm kiếm HĐN";
            TimKiemHoaDonNhap timKiemHoaDonnhap = new TimKiemHoaDonNhap(this);
            timKiemHoaDonnhap.TopLevel = false;
            timKiemHoaDonnhap.FormBorderStyle = FormBorderStyle.None;
            timKiemHoaDonnhap.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(timKiemHoaDonnhap);
            MainPanel.Tag = timKiemHoaDonnhap;
            timKiemHoaDonnhap.BringToFront();
            timKiemHoaDonnhap.Show();
        }
        public void HideChiTietNK()
        {
            name.Text = "";
            // Đóng Form3 và hiện lại form1
            QLNhapkho.FillColor = Color.Pink; // Màu khi được chọn
            QLNhapkho.ForeColor = Color.White;
            QLNhapkho qLNhapkho = new QLNhapkho(this);
            qLNhapkho.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(qLNhapkho);
            qLNhapkho.BringToFront();

        }
        public void HideForm3()
        {
            name.Text = "";
            // Đóng Form3 và hiện lại form1
            QLXuatkho.FillColor = Color.Pink; // Màu khi được chọn
            QLXuatkho.ForeColor = Color.White;
            QLXuatkho qLXuatkho = new QLXuatkho(this);
            qLXuatkho.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(qLXuatkho);
            qLXuatkho.BringToFront();

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void QLNhapkho_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            QLNhapkho qLNhapkho = new QLNhapkho(this);
            qLNhapkho.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(qLNhapkho);
            qLNhapkho.BringToFront();
        }

        private void btnBaocao_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            BaoCao baoCao = new BaoCao();
            baoCao.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(baoCao);
            baoCao.BringToFront();
        }

        private void btnDoanhthu_Click(object sender, EventArgs e)
        {
            ChangeColor((Guna.UI2.WinForms.Guna2Button)sender);
            Doanhthu doanhthu = new Doanhthu(this);
            doanhthu.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(doanhthu);
            doanhthu.BringToFront();
        }
    }
}
