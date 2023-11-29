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
    public partial class QLKhohang : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        public QLKhohang()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaKho.Text == "")
            {
                MessageBox.Show("Vui lòng điền mã kho !");
                txtMaKho.Focus();
            }
            else if (txtTenkho.Text == "")
            {
                MessageBox.Show("Vui lòng điền tên kho !");
                txtTenkho.Focus();
            }
            else if (cmbMaNV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn mã nhân viên !");
                cmbMaNV.Focus();
            }
            else if (!IsValidPhoneNumber(txtDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng");
                txtDienThoai.Focus();
            }
            else if (txtDientich.Text == "")
            {
                MessageBox.Show("Vui lòng điền diện tích kho !");
                txtDientich.Focus();
            }
            else
            {
                DataTable dtnhanven = pd.docbang("Select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang where" + " Makho =N'" + txtMaKho.Text + "'");
                if (dtnhanven.Rows.Count > 0)
                {
                    MessageBox.Show("Mã kho đã có, hãy nhập mã khác!");
                    txtMaKho.Focus();
                }
                else
                {
                    pd.capNhat("insert into Khohang(Makho,Tenkho,MaNV,Dienthoai,Dientich) values(N'" + txtMaKho.Text + "',N'" + txtTenkho.Text + "',N'" + (cmbMaNV.Text).Trim() + "',N'" + txtDienThoai.Text + "',N'" + txtDientich.Text + "')");
                    MessageBox.Show("Bạn đã thêm mới thành công");
                    dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang");
                }
            }
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
        private void loadCombobox()
        {
            pd.ketnoi();
            string query = "select distinct MaNV from Nhanvien";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaNV.DataSource = dataTable;
            cmbMaNV.DisplayMember = "MaNV";
            cmbMaNV.ValueMember = "MaNV";
            cmbMaNV.Text = "";
            cmbMaNV.SelectedIndex = -1;
        }
        private void QLKhohang_Load(object sender, EventArgs e)
        {
            loadCombobox();
            cmbTimKiem.Items.Add("Mã kho");
            cmbTimKiem.Items.Add("Tên kho");
            cmbTimKiem.Items.Add("Mã nhân viên");
            cmbTimKiem.Items.Add("Điện thoại");
            dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang");
            dataGridView1.Columns[0].HeaderText = "Mã kho";
           
            dataGridView1.Columns[1].HeaderText = "Tên kho";
        
            dataGridView1.Columns[2].HeaderText = "Mã nhân viên";
          
            dataGridView1.Columns[3].HeaderText = "Điện thoại";
      
            dataGridView1.Columns[4].HeaderText = "Diện tích";
   
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtMaKho.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtTenkho.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cmbMaNV.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtDienThoai.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtDientich.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

            }
        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            txtMaKho.Text = "";
            txtTenkho.Text = "";
            cmbMaNV.SelectedIndex = -1;
            cmbMaNV.SelectedIndex = -1;
            txtDienThoai.Text = "";
            txtDientich.Text = "";
            cmbTimKiem.Text = "";
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Workbook exBook =
            exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet exSheet =
                (Excel.Worksheet)exBook.Worksheets[1];
            Excel.Range tenvung = (Excel.Range)exSheet.Cells[1, 1];
            tenvung.Font.Name = "Arial"; tenvung.Font.Size = 16;
            tenvung.Font.Color = Color.Blue;
            tenvung.Value = " Thông tin toàn bộ các kho hàng";
            exSheet.get_Range("A1: F1").Merge(true);
            exSheet.get_Range("A2:F2").Font.Size = 14;
            exSheet.get_Range("A2:F2").Font.Bold = true;
            exSheet.get_Range("A2").Value = "STT";
            exSheet.get_Range("B2").Value = "Mã kho";
            exSheet.get_Range("C2").Value = "Tên kho";
            exSheet.get_Range("D2").Value = "Mã nhân viên";
            exSheet.get_Range("E2").Value = "Điện thoại";
            exSheet.get_Range("F2").Value = "Diện tích";


            int k = dataGridView1.Rows.Count - 1;
            exSheet.get_Range("A2:F" + (k + 2).ToString()).
                Borders.LineStyle
                = Excel.XlLineStyle.xlDouble;//.Borders( true);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                exSheet.get_Range("A" + (3 + i).ToString()).Value =
                    (i + 1).ToString();
                exSheet.get_Range("B" + (3 + i).ToString()).Value =
                    dataGridView1.Rows[i].Cells[0].Value.ToString();
                exSheet.get_Range("C" + (3 + i).ToString()).Value =
                    dataGridView1.Rows[i].Cells[1].Value.ToString();
                exSheet.get_Range("D" + (3 + i).ToString()).Value =
                    dataGridView1.Rows[i].Cells[2].Value.ToString();
                exSheet.get_Range("E" + (3 + i).ToString()).Value =
                    dataGridView1.Rows[i].Cells[3].Value.ToString();
                exSheet.get_Range("F" + (3 + i).ToString()).Value =
                    dataGridView1.Rows[i].Cells[4].Value.ToString();


            }
            exBook.Activate();
            SaveFileDialog svf = new SaveFileDialog();
            svf.Title = "Chọn đường dẫn và đặt tên tệp lưu dữ liệu. ";
            svf.ShowDialog();
            string filename = svf.FileName;
            if (filename == "")
            {
                MessageBox.Show("bạn chưa đặt tên File");
            }
            exBook.SaveAs(filename);
            exApp.Quit();
        }
        private bool check()
        {
            if (txtMaKho.Text.Trim().Equals("") ||
                txtTenkho.Text.Trim().Equals("") ||
                cmbMaNV.Text.Trim().Equals("") ||
                txtDienThoai.Text.Trim().Equals("") ||
                txtDientich.Text.Trim().Equals("")
              ) return false;
            return true;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (MessageBox.Show("Bạn có muốn sửa nhân viên này không", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    string sql;
                    sql = "UPDATE Khohang SET Tenkho=N'" + txtTenkho.Text +
                    "',MaNV=N'" + cmbMaNV.Text +
                    "',Dienthoai=N'" + txtDienThoai.Text + "',Dientich = N'" + txtDientich.Text + "' WHERE Makho=N'" + txtMaKho.Text + "'";
                    pd.capNhat(sql);
                    dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin");
                txtMaKho.Focus();
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
                if (cmbTimKiem.Text == "Mã kho")
                {
                    if (txtMaKho.Text != "")
                    {

                        dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang where Makho=N'" + txtMaKho.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền mã kho muốn tìm kiếm.");
                        txtMaKho.Focus();
                    }
                }
                if (cmbTimKiem.Text == "Tên kho")
                {
                    if (txtTenkho.Text != "")
                    {

                        dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang where Makho=N'" + txtTenkho.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền tên kho tìm kiếm!");
                        txtTenkho.Focus();
                    }
                }
                if (cmbTimKiem.Text == "Điện thoại")
                {
                    if (txtDienThoai.Text != "")
                    {

                        dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang where Makho=N'" + txtDienThoai.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền điện thoại muốn tìm kiếm");
                        txtDienThoai.Focus();
                    }
                }
                if (cmbTimKiem.Text == "Mã nhân viên")
                {
                    if (cmbMaNV.Text != "")
                    {

                        dataGridView1.DataSource = pd.docbang("select Makho,Tenkho,MaNV,Dienthoai,Dientich from Khohang where Makho=N'" + cmbMaNV.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng điền mã nhân viên tìm kiếm.");
                        cmbMaNV.Focus();
                    }
                }
            }
        }
    }
}
