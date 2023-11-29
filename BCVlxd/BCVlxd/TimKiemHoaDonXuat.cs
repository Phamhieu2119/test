using connectdatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCVlxd
{
    public partial class TimKiemHoaDonXuat : Form
    {
        ProcessDataBase pd = new ProcessDataBase();
        private Main mainForm;
        public TimKiemHoaDonXuat()
        {
            InitializeComponent();
        }
        public TimKiemHoaDonXuat(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
        private void loadComboboxVT()
        {
            pd.ketnoi();
            string query = "select distinct Mavattu from Vattu";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaVT.DataSource = dataTable;
            cmbMaVT.DisplayMember = "Mavattu";
            cmbMaVT.ValueMember = "Mavattu";
            cmbMaVT.Text = "";
            cmbMaVT.SelectedIndex = -1;
        }
        private void loadComboboxKho()
        {
            pd.ketnoi();
            string query = "select distinct Makho from Khohang";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMakho.DataSource = dataTable;
            cmbMakho.DisplayMember = "Makho";
            cmbMakho.ValueMember = "Makho";
            cmbMakho.Text = "";
            cmbMakho.SelectedIndex = -1;
        }
        private void loadComboboMaHD()
        {
            pd.ketnoi();
            string query = "select distinct Mahoadon from Xuatkho";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaHD.DataSource = dataTable;
            cmbMaHD.DisplayMember = "Mahoadon";
            cmbMaHD.ValueMember = "Mahoadon";
            cmbMaHD.Text = "";
            cmbMaHD.SelectedIndex = -1;
        }
        private void TimKiemHoaDonXuat_Load(object sender, EventArgs e)
        {
            loadComboboxVT();
            loadComboboxKho();
            loadComboboMaHD();
            dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang,TongTien from Xuatkho");
            dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
            dataGridView1.Columns[1].HeaderText = "Ngày xuất";
            dataGridView1.Columns[2].HeaderText = "Mã kho";
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[3].HeaderText = "Lý do xuất";
            dataGridView1.Columns[3].Width = 170;
            dataGridView1.Columns[4].HeaderText = "Thuế VAT";
            dataGridView1.Columns[5].HeaderText = "Mã khách hàng";
            dataGridView1.Columns[6].HeaderText = "Tổng tiền";
            dataGridView1.Columns[6].Width = 145;


            dataGridView2.DataSource = pd.docbang("select Mahoadon,Mavattu,soluong,thanhtien from Chitietxuatkho order by Mahoadon");
            dataGridView2.Columns[0].HeaderText = "Mã hóa đơn";
            dataGridView2.Columns[1].HeaderText = "Mã vật tư";
            dataGridView2.Columns[2].HeaderText = "Số lượng";
            dataGridView2.Columns[3].HeaderText = "Thành tiền";        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang,TongTien from Xuatkho");
            dataGridView2.DataSource = pd.docbang("select Mahoadon,Mavattu,soluong,thanhtien from Chitietxuatkho order by Mahoadon");
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (cmbMaVT.Text == "")
            {
                MessageBox.Show("Vui lòng chọn mã vật tư muốn tìm kiếm !");
                cmbMaVT.Focus();
            }else if(cmbMakho.Text == "")
            {
                MessageBox.Show("Vui lòng chọn mã kho muốn tìm kiếm !");
                cmbMakho.Focus();
            }
            else
            {
                dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang,TongTien from Xuatkho  where Makho = N'" + cmbMakho.Text.ToString() + "' and Ngayxuat = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' order by Mahoadon");
                dataGridView2.DataSource = pd.docbang("select c.Mahoadon,c.Mavattu,c.soluong,c.thanhtien from Chitietxuatkho as c join Xuatkho as n on c.Mahoadon =n.Mahoadon " +
                    "where c.Mavattu = N'" + cmbMaVT.Text.ToString() + "' and n.Makho = N'" + cmbMakho.Text.ToString() + "' and n.Ngayxuat = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'order by c.Mahoadon");

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang,TongTien from Xuatkho where Mahoadon = N'" + cmbMaHD.Text + "'");
            dataGridView2.DataSource = pd.docbang("select Mahoadon,Mavattu,soluong,thanhtien from Chitietxuatkho where Mahoadon = N'" + cmbMaHD.Text + "'");

        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            cmbMaVT.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Opacity = 1.0;
            mainForm.HideForm3();
        }
    }
}
