using connectdatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace BCVlxd
{
    public partial class QLNhanvien : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        public QLNhanvien()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }
        private void loadCombobox()
        {
            pd.ketnoi();
            string query = "select distinct MaCV from Congviec";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaCV.DataSource = dataTable;
            cmbMaCV.DisplayMember = "MaCV";
            cmbMaCV.ValueMember = "MaCV";
            cmbMaCV.Text = " ";
            cmbMaCV.SelectedIndex = -1;
        }
        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void QLNhanvien_Load(object sender, EventArgs e)
        {
            cmbMaCV.Items.Add("");
            loadCombobox();
            cmbTimKiem.Items.Add("Tên Nhân viên");
            cmbTimKiem.Items.Add("Mã nhân viên");
            cmbTimKiem.Items.Add("Điện thoại");
            cmbTimKiem.Items.Add("Mã công việc");

            comboBox1.Items.Add("Nam");
            comboBox1.Items.Add("Nữ");
            dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0'");

            dataGridView1.Columns[4].HeaderText = "Điện thoại";
            
            dataGridView1.Columns[3].HeaderText = "Ngày sinh";
           
            dataGridView1.Columns[2].HeaderText = "Giới tính";
          
            dataGridView1.Columns[1].HeaderText = "Tên nhân viên";
           
            dataGridView1.Columns[0].HeaderText = "Mã nhân viên";
            
            dataGridView1.Columns[5].HeaderText = "Mã công việc";
         
            dataGridView1.Columns[6].HeaderText = "Tên công việc";
        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0'");

        }
        private void txtDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngăn chặn ký tự không phải số được nhập vào
            }
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {

            string pattern = @"^\d{10,11}$";


            return Regex.IsMatch(phoneNumber, pattern);
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày sinh lớn hơn ngày hiện tại");
                return;
                
            }
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Bạn phải nhập mã Nhân viên");
                txtMaNV.Focus();
            }
            else
            {
                if (txtTenNV.Text == "")
                {
                    MessageBox.Show("Bạn phải nhập tên nhân viên");
                    txtTenNV.Focus();
                }
                if (!IsValidPhoneNumber(txtDienThoai.Text))
                {
                    MessageBox.Show("Số điện thoại không đúng định dạng");
                    txtDienThoai.Focus();
                }

                else
                {
                    DataTable dtnhanven = pd.docbang("Select * from Nhanvien where" + " MaNV = '" + (txtMaNV.Text).Trim() + "'");
                    if (dtnhanven.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã nhân viên này đã có, hãy nhập mã khác!");

                        txtMaNV.Focus();
                    }
                    else
                    {

                        bool isMale = (comboBox1.Text == "Nam");
                        pd.capNhat("INSERT INTO Nhanvien (MaNV,TenNV,Gioitinh,Ngaysinh,Dienthoai,MaCV) "
                            + " values( N'" + txtMaNV.Text + "', N'" + txtTenNV.Text + "', " + (isMale ? "1" : "0") + ", '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', N'" + txtDienThoai.Text + "', N'" + cmbMaCV.Text + "')");

                        MessageBox.Show("Bạn đã thêm mới thành công");
                        dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0'");

                    }

                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa nhân viên này không ? ", "warning ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                pd.ketnoi();
                string sql3 = "update Khohang set MaNV = '0' where MaNV = N'" + txtMaNV.Text.ToString() + "'";
                pd.capNhat(sql3);

                string sql2 = "update Nhapkho set MaNV = '0' where MaNV = N'" + txtMaNV.Text.ToString() + "'";
                pd.capNhat(sql2);

                string sql1 = "delete Nhanvien where MaNV =N'" + txtMaNV.Text.ToString() + "'";
                pd.capNhat(sql1);
                dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0'");

            }
        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            txtDienThoai.Text = "";
            comboBox1.SelectedIndex = -1;
            cmbMaCV.SelectedIndex = -1;
            cmbMaCV.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }
        private bool check()
        {
            if (txtMaNV.Text.Trim().Equals("") ||
                txtTenNV.Text.Trim().Equals("") ||
                cmbMaCV.Text.Trim().Equals("") ||
                txtDienThoai.Text.Trim().Equals("") ||
                comboBox1.Text.Trim().Equals("")
              ) return false;
            return true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày sinh lớn hơn ngày hiện tại");
                return;
                dateTimePicker1.Focus();
            }
            if (check())
            {
                if (MessageBox.Show("Bạn có muốn sửa nhân viên này không", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool isMale = (comboBox1.Text == "Nam");
                    string sql;
                    sql = "UPDATE Nhanvien SET TenNV=N'" + txtTenNV.Text +
                    "',Dienthoai=N'" + txtDienThoai.Text +
                    "',Gioitinh=N'" + isMale.ToString() + "',Ngaysinh = N'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',MaCV=N'" + cmbMaCV.Text + "' WHERE MaNV=N'" + txtMaNV.Text + "'";
                    pd.capNhat(sql);
                    dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0'");

                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin");
                txtMaNV.Focus();
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (cmbTimKiem.Text == "")
            {
                MessageBox.Show("Vui lòng chọn loại tìm kiếm!");
                cmbTimKiem.Focus();
            }
            else
            {
                if (cmbTimKiem.Text == "Tên Nhân viên")
                {
                    if (txtTenNV.Text != "")
                    {
                        dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0' and n.TenNV=N'" + txtTenNV.Text + "'");

                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền tên nhân viên muốn tìm kiếm.");
                        txtTenNV.Focus();
                    }
                }
                if (cmbTimKiem.Text == "Mã nhân viên")
                {
                    if (txtMaNV.Text != "")
                    {
                        dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0' and MaNV=N'" + txtMaNV.Text + "'");

                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền mã nhân viên tìm kiếm!");
                        txtMaNV.Focus();
                    }
                }
                if (cmbTimKiem.Text == "Điện thoại")
                {
                    if (txtDienThoai.Text != "")
                    {
                        dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0' and Dienthoai=N'" + txtDienThoai.Text + "'");

                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền điện thoại muốn tìm kiếm");
                        txtDienThoai.Focus();
                    }
                }
                if (cmbTimKiem.Text == "Mã công việc")
                {
                    if (cmbMaCV.Text != "")
                    {
                        dataGridView1.DataSource = pd.docbang("SELECT n.MaNV,n.TenNV,n.Gioitinh,n.Ngaysinh,n.Dienthoai,n.MaCV, c.TenCongViec  FROM Nhanvien as n JOIN Congviec as c ON n.MaCV = c.MaCV where Not MaNV ='0' and MaCV=N'" + cmbMaCV.Text + "'");

                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền mã công việc.");
                        cmbMaCV.Focus();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Workbook exBook =
            exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet exSheet =
                (Excel.Worksheet)exBook.Worksheets[1];
            Excel.Range tenvung = (Excel.Range)exSheet.Cells[1, 1];
            tenvung.Font.Name = "Arial"; tenvung.Font.Size = 16;
            tenvung.Font.Color = Color.Blue;
            tenvung.Value = " Danh sách nhân viên ";
            exSheet.get_Range("A1: H1").Merge(true);
            exSheet.get_Range("A2:H2").Font.Size = 14;
            exSheet.get_Range("A2:H2").Font.Bold = true;
            exSheet.get_Range("A2").Value = "STT";
            exSheet.get_Range("B2").Value = "Mã nhân viên";
            exSheet.get_Range("C2").Value = "Tên nhân viên";
            exSheet.get_Range("D2").Value = "Giới tính";
            exSheet.get_Range("E2").Value = "Ngày sinh";
            exSheet.get_Range("F2").Value = "Điện thoại";
            exSheet.get_Range("G2").Value = "Mã công việc";
            exSheet.get_Range("H2").Value = "Tên công việc";

            int k = dataGridView1.Rows.Count - 1;
            exSheet.get_Range("A2:H" + (k + 2).ToString()).
                Borders.LineStyle
                = Excel.XlLineStyle.xlDouble;//.Borders( true);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                exSheet.get_Range("A" + (3 + i).ToString()).Value =
                    (i + 1).ToString();
                exSheet.get_Range("B" + (3 + i).ToString()).Value =
                    dataGridView1.Rows[i].Cells[4].Value.ToString();
                exSheet.get_Range("C" + (3 + i).ToString()).Value =
                    dataGridView1.Rows[i].Cells[3].Value.ToString();
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    exSheet.get_Range("D" + (3 + i).ToString()).Value = "Nam";
                }
                else
                {
                    exSheet.get_Range("D" + (3 + i).ToString()).Value = "Nữ";
                }
                exSheet.get_Range("E" + (3 + i).ToString()).Value =
                  dataGridView1.Rows[i].Cells[1].Value.ToString();
                exSheet.get_Range("F" + (3 + i).ToString()).Value =
                 dataGridView1.Rows[i].Cells[0].Value.ToString();
                exSheet.get_Range("G" + (3 + i).ToString()).Value =
                 dataGridView1.Rows[i].Cells[5].Value.ToString();
                exSheet.get_Range("H" + (3 + i).ToString()).Value =
                 dataGridView1.Rows[i].Cells[6].Value.ToString();
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtDienThoai.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                string gioiTinh = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                comboBox1.Text = gioiTinh == "True" ? "Nam" : "Nữ";
                txtTenNV.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtMaNV.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                cmbMaCV.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

            }
        }
    }
}
