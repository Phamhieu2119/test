using connectdatabase;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCVlxd
{
    public partial class QLkhachhang : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        public QLkhachhang()
        {
            InitializeComponent();
            
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
          


        }
        private bool check()
        {
            if (txtMaKH.Text.Trim().Equals("") ||
                txtTenKH.Text.Trim().Equals("") ||
                txtDiachi.Text.Trim().Equals("") ||
                txtDienThoai.Text.Trim().Equals("")
              ) return false;
            return true;
        }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("select * from Khachhang where not Makhachhang = '0'");
        }

        private void QLkhachhang_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("select * from Khachhang where NOT Makhachhang = '0' ");
            dataGridView1.Columns[0].HeaderText = "Mã khách hàng";
          
            dataGridView1.Columns[1].HeaderText = "Địa chỉ";
          
            dataGridView1.Columns[2].HeaderText = "Điện thoại";
          
            dataGridView1.Columns[3].HeaderText = "Tên khách hàng";
            
            comboBox1.Items.Add("Mã khách hàng");
            comboBox1.Items.Add("Địa chỉ");
            comboBox1.Items.Add("Điện thoại");
            comboBox1.Items.Add("Tên khách hàng");

        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
           
            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtDiachi.Text = "";
            txtDienThoai.Text = "";
            comboBox1.SelectedIndex= -1;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (check())
            {

                DataTable tem = pd.docbang("Select Makhachhang from Khachhang where " + "Makhachhang=N'" + txtMaKH.Text + "'");
                if (tem.Rows.Count > 0)
                {
                    MessageBox.Show(" Mã Khách hàng đã tồn tại");
                    txtMaKH.Focus();
                }
                else
                {
                    string sql;
                    sql = "insert into Khachhang " +
                        "values(N'" + txtMaKH.Text + "'," + "N'" +
                        txtDiachi.Text + "'," + "N'" + txtDienThoai.Text + "',N'" + txtTenKH.Text + "')";
                    pd.capNhat(sql);
                    dataGridView1.DataSource = pd.docbang("select * from Khachhang where not Makhachhang = '0'");
                }
            }
            else
            {
                MessageBox.Show("Fill full data");
                txtMaKH.Focus();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa khách hàng này không ? ", "warning ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                pd.ketnoi();

                string sql2 = "update Xuatkho set Makhachhang = '0' where Makhachhang = N'" + txtMaKH.Text.ToString() + "'";
                pd.capNhat(sql2);

                string sql1 = "delete Khachhang where Makhachhang =N'" + txtMaKH.Text.ToString() + "'";
                pd.capNhat(sql1);


                dataGridView1.DataSource = pd.docbang("select * from Khachhang where not Makhachhang = '0'");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaKH.Text != dataGridView1.CurrentRow.Cells[0].Value.ToString())
            {
                MessageBox.Show("Không được sửa mã khách hàng !");
                txtMaKH.Focus();
                return;
            }
            if (check())
            {
                string sql;
                sql = "UPDATE Khachhang SET Diachi=N'" + txtDiachi.Text +
                "',Dienthoai=N'" + txtDienThoai.Text +
                "',Tenkhachhang=N'" + txtTenKH.Text + "' WHERE Makhachhang=N'" + txtMaKH.Text + "'";
                pd.capNhat(sql);
                dataGridView1.DataSource = pd.docbang("select * from Khachhang where not Makhachhang = '0'");
            }
            else
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin");
                txtMaKH.Focus();
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Vui lòng chọn loại tìm kiếm!");
                comboBox1.Focus();
            }
            else
            {
                if (comboBox1.Text == "Mã khách hàng")
                {
                    if (txtMaKH.Text != "")
                    {
                        string sql;
                        sql = "select * from  Khachhang where  Makhachhang=N'" + txtMaKH.Text + "'";
                        pd.capNhat(sql);
                        dataGridView1.DataSource = pd.docbang("select * from Khachhang where  Makhachhang=N'"
                            + txtMaKH.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền mã khách hàng.");
                        txtMaKH.Focus();
                    }
                }
                if (comboBox1.Text == "Tên khách hàng")
                {
                    if (txtTenKH.Text != "")
                    {
                        string sql;
                        sql = "select * from  Khachhang where Tenkhachhang=N'" + txtTenKH.Text + "'";
                        pd.capNhat(sql);
                        dataGridView1.DataSource = pd.docbang("select * from Khachhang where Tenkhachhang=N'" + txtTenKH.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền tên khách hàng.");
                        txtTenKH.Focus();
                    }
                }
                if (comboBox1.Text == "Địa chỉ")
                {
                    if (txtDiachi.Text != "")
                    {
                        string sql;
                        sql = "select * from  Khachhang where  Diachi=N'" + txtDiachi.Text + "'";
                        pd.capNhat(sql);
                        dataGridView1.DataSource = pd.docbang("select * from Khachhang Diachi=N'" + txtDiachi.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền địa chỉ.");
                        txtDiachi.Focus();
                    }
                }
                if (comboBox1.Text == "Điện thoại")
                {
                    if (txtDienThoai.Text != "")
                    {
                        string sql;
                        sql = "select * from  Khachhang where  Dienthoai=N'" + txtDienThoai.Text + "'";
                        pd.capNhat(sql);
                        dataGridView1.DataSource = pd.docbang("select * from Khachhang where  Dienthoai=N'" + txtDienThoai.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền số điện thoại.");
                        txtDienThoai.Focus();
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtMaKH.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtTenKH.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtDiachi.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtDienThoai.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {
            

        }

        private void guna2GroupBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
