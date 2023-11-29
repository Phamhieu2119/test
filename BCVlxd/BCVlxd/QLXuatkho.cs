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
using Excel = Microsoft.Office.Interop.Excel;
namespace BCVlxd
{
    public partial class QLXuatkho : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        private Main mainForm;
        public QLXuatkho()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView2.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            mainForm.Name = "";
        }
        public QLXuatkho(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }
        private void loadComboboxMKH()
        {
            pd.ketnoi();
            string query = "select distinct Makhachhang from Khachhang";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaKH.DataSource = dataTable;
            cmbMaKH.DisplayMember = "Makhachhang";
            cmbMaKH.ValueMember = "Makhachhang";
            cmbMaKH.Text = "";
            cmbMaKH.SelectedIndex = -1;
        }
        private void loadComboboxMKho()
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
            cmbMakho .SelectedIndex = -1;
        }
        private void QLXuatkho_Load(object sender, EventArgs e)
        {
            loadComboboxMKH();
            loadComboboxMKho();
            dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang from Xuatkho");
            dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
            dataGridView1.Columns[1].HeaderText = "Ngày xuất";
            dataGridView1.Columns[2].HeaderText = "Mã kho";
            dataGridView1.Columns[3].HeaderText = "Lý do xuất";
            dataGridView1.Columns[3].Width = 160;
            dataGridView1.Columns[4].HeaderText = "Thuế VAT";
            dataGridView1.Columns[5].HeaderText = "Mã khách hàng";

            dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Xuatkho");
            dataGridView2.Columns[0].HeaderText = "Mã hóa đơn";
            dataGridView2.Columns[1].HeaderText = "Tổng tiền";
        }
        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            mainForm.showTimKiemHDX();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            mainForm.ShowForm3();
        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang from Xuatkho");
            dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Xuatkho");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày xuất lớn hơn ngày hiện tại");
                return;
               
            }
            if (txtMahoadon.Text == "")
            {
                MessageBox.Show("Bạn phải nhập mã hóa đơn");
                txtMahoadon.Focus();
            }
            else
            {
                if (cmbMaKH.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn mã khách hàng !");
                    cmbMaKH.Focus();
                }
                else if (cmbMakho.Text == "")
                {

                    MessageBox.Show("Bạn phải chọn mã kho !");
                    cmbMakho.Focus();
                }
                else if (txtLydoxuat.Text == "")
                {
                    MessageBox.Show("Bạn phải điền lý do xuất !");
                    txtLydoxuat.Focus();
                }
                else if (txtThue.Text == "")
                {
                    MessageBox.Show("Bạn phải điền thuế VAT !");
                    txtThue.Focus();
                }
                else
                {
                    DataTable dthoadon = pd.docbang("select * from Xuatkho where" + " Mahoadon = N'" + (txtMahoadon.Text).Trim() + "'");
                    if (dthoadon.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã hóa đơn này đã có, hãy nhập mã khác!");
                        txtMahoadon.Focus();
                    }
                    else
                    {

                        pd.capNhat("INSERT INTO Xuatkho (Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang) "
                            + " values( N'" + txtMahoadon.Text + "', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',N'" + cmbMakho.Text + "',N'" + txtLydoxuat.Text.ToString() + "', '" + float.Parse(txtThue.Text.ToString()) + "',N'" + cmbMaKH.Text + "')");
                         
                        pd.capNhat("update Xuatkho set TongTien = 0 where Mahoadon = N'" + txtMahoadon.Text.ToString() + "'");
                        MessageBox.Show("Bạn đã thêm mới thành công");
                        dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang from Xuatkho");
                        dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Xuatkho");
                    }

                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa hóa đơn xuất này không ? ", "warning ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                pd.ketnoi();
                string query5 = "select c.Mavattu from Chitietxuatkho as c join Xuatkho as x on c.Mahoadon = x.Mahoadon  where x.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + txtMahoadon.Text.ToString() + "'";
                SqlCommand cmd5 = new SqlCommand(query5, pd.Con);
                SqlDataReader reader5 = cmd5.ExecuteReader();
                List<string> list = new List<string>();
                while (reader5.Read())
                {
                    string Mavattu = reader5["Mavattu"].ToString();
                    list.Add(Mavattu);
                }
                foreach (string material in list)
                {
                    // lấy số lượng từng mã vật tư cần xóa trên hóa đơn
                    pd.ketnoi();
                    string sql10 = "select sum(c.soluong) from Chitietxuatkho as c join Xuatkho as x on c.Mahoadon = x.Mahoadon where c.MaVattu = N'" + material + "'and x.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + txtMahoadon.Text.ToString() + "'";
                    SqlCommand cmd10 = new SqlCommand(sql10, pd.Con);
                    int count10 = Convert.ToInt32(cmd10.ExecuteScalar());

                    string sql6 = "update Chitietkhohang set soluong = soluong + '" + count10 + "' " +
                                        "FROM Vattu v " +
                                        "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                        "JOIN Khohang k ON k.Makho = c.Makho " +
                                        " where c.Mavattu = N'" + material + "' and c.Makho = N'" + cmbMakho.Text + "'";
                    pd.capNhat(sql6);

                }
                pd.capNhat("Delete Chitietxuatkho where Mahoadon = N'" + txtMahoadon.Text.ToString() + "'");
                pd.capNhat("Delete Xuatkho where Mahoadon = N'" + txtMahoadon.Text.ToString() + "'");
                MessageBox.Show("Xóa thành công !");
                dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang from Xuatkho");
                dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Xuatkho");
            }
        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            txtMahoadon.Text = "";
            cmbMaKH.SelectedIndex = -1;
            cmbMaKH.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            txtLydoxuat.Text = "";
            txtThue.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }
        private bool check()
        {
            if (txtMahoadon.Text.Trim().Equals("") ||
                cmbMakho.Text.Trim().Equals("") ||
                cmbMaKH.Text.Trim().Equals("") ||
                txtThue.Text.Trim().Equals("") ||
                txtLydoxuat.Text.Trim().Equals("")
              ) return false;
            return true;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày xuất lớn hơn ngày hiện tại");
                return;
               
            }
            if (check())
            {

                if (MessageBox.Show("Bạn có muốn sửa hóa đơn này không ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    string sql;
                    sql = "UPDATE Xuatkho SET makhachhang=N'" + cmbMaKH.Text +
                    "',ThueVAT=N'" + txtThue.Text.ToString() +
                     "',Ngayxuat = N'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',Makho=N'" + cmbMakho.Text + "', Lydoxuat = N'" + txtLydoxuat.Text.ToString() + "' WHERE Mahoadon=N'" + txtMahoadon.Text + "'";
                    pd.capNhat(sql);
                    dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngayxuat,Makho,Lydoxuat,ThueVAT,Makhachhang from Xuatkho");
                    dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Xuatkho");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin");
                txtMahoadon.Focus();
            }
        }

        private void btnTongtien_Click(object sender, EventArgs e)
        {
            pd.ketnoi();
            string query = "select  Mahoadon from Xuatkho";
            SqlCommand cmd = new SqlCommand(query, pd.Con);
            SqlDataReader reader = cmd.ExecuteReader();
            List<string> list = new List<string>();
            while (reader.Read())
            {
                string Mahoadon = reader["Mahoadon"].ToString();
                list.Add(Mahoadon);
            }
            pd.dongketnoi();
            foreach (string material in list)
            {
                
                string sql1 = "update Xuatkho set TongTien = (select sum(thanhtien) from Chitietxuatkho where Mahoadon = N'" + material + "')*((ThueVAT +100)/100) where Mahoadon = N'" + material + "'";
                pd.capNhat(sql1);
            }

            dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Xuatkho");
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
            tenvung.Value = " Hóa đơn xuất kho ";
            exSheet.get_Range("A1: H1").Merge(true);
            exSheet.get_Range("A2:F2").Font.Size = 14;
            exSheet.get_Range("A2:F2").Font.Bold = true;
            exSheet.get_Range("A2").Value = "STT";
            exSheet.get_Range("B2").Value = "Mã Hóa đơn";
            exSheet.get_Range("C2").Value = "Ngày xuất";
            exSheet.get_Range("D2").Value = "Mã kho";
            exSheet.get_Range("E2").Value = "Lý do xuất";
            exSheet.get_Range("F2").Value = "Thuế VAT";
            exSheet.get_Range("G2").Value = "Mã khách hàng";
            exSheet.get_Range("H2").Value = "Tổng tiền";

            int k = dataGridView1.Rows.Count - 1;
            exSheet.get_Range("A2:H" + (k + 2).ToString()).
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
                exSheet.get_Range("G" + (3 + i).ToString()).Value =
                  dataGridView1.Rows[i].Cells[5].Value.ToString();
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                exSheet.get_Range("H" + (3 + i).ToString()).Value =
                  dataGridView2.Rows[i].Cells[1].Value.ToString();
            }
            exBook.Activate();
            SaveFileDialog svf = new SaveFileDialog();
            svf.Title = "Chọn đường dẫn và đặt tên tệp lưu dữ liệu ";
            svf.ShowDialog();
            string filename = svf.FileName;
            if (filename == "")
            {
                MessageBox.Show("Bạn chưa đặt tên file");
            }
            exBook.SaveAs(filename);
            exApp.Quit();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtMahoadon.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cmbMakho.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtLydoxuat.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtThue.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                cmbMaKH.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            }
        }
    }
}
