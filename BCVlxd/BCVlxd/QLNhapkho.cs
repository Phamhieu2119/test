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
    public partial class QLNhapkho : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        private Main mainForm;
        public QLNhapkho()
        {
            InitializeComponent();
        }
        public QLNhapkho(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }
        private void loadComboboxMK()
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

        private void loadComboboxMNCC()
        {
            pd.ketnoi();
            string query = "select distinct MaNCC from Nhacungcap";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaNcc.DataSource = dataTable;
            cmbMaNcc.DisplayMember = "MaNCC";
            cmbMaNcc.ValueMember = "MaNCC";
            cmbMaNcc.Text = "";
            cmbMaNcc.SelectedIndex = -1;
        }
        private void loadComboboxMNV()
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
        private void QLNhapkho_Load(object sender, EventArgs e)
        {
            loadComboboxMK();
            loadComboboxMNCC();
            loadComboboxMNV();
            dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngaynhap,MaNCC,MaNV,Makho from Nhapkho");
            dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
            dataGridView1.Columns[1].HeaderText = "Ngày nhập";
            dataGridView1.Columns[2].HeaderText = "Mã nhà cung cấp";
            dataGridView1.Columns[3].HeaderText = "Mã nhân viên";
            dataGridView1.Columns[4].HeaderText = "Mã kho";
          

            dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Nhapkho");
            dataGridView2.Columns[0].HeaderText = "Mã hóa đơn";
            dataGridView2.Columns[1].HeaderText = "Tổng tiền";
       
        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngaynhap,MaNCC,MaNV,Makho from Nhapkho");
            dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Nhapkho");
        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            txtMahoadon.Text = "";
            cmbMakho.SelectedIndex = -1;
            cmbMaNcc.SelectedIndex = -1;
            cmbMaNV.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            cmbMaNcc.SelectedIndex = -1;
            cmbMaNV.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày nhập lớn hơn ngày hiện tại");
                return;
          
            }
            if (txtMahoadon.Text == "")
            {
                MessageBox.Show("Bạn phải nhập mã hóa đơn");
                txtMahoadon.Focus();
            }
            else
            {
                if (cmbMaNcc.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn mã nhà cung cấp !");
                    cmbMaNcc.Focus();
                }
                else if (cmbMaNV.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn mã nhân viên !");
                    cmbMaNV.Focus();
                }
                else if (cmbMakho.Text == "")
                {

                    MessageBox.Show("Bạn phải chọn mã kho !");
                    cmbMakho.Focus();
                }
                else
                {
                    DataTable dthoadon = pd.docbang("select Mahoadon,Ngaynhap,MaNCC,MaNV,Makho from Nhapkho where" + " Mahoadon = N'" + (txtMahoadon.Text).Trim() + "'");
                    if (dthoadon.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã hóa đơn này đã có, hãy nhập mã khác!");
                        txtMahoadon.Focus();
                    }
                    else
                    {

                        pd.capNhat("INSERT INTO Nhapkho (Mahoadon,Ngaynhap,MaNCC,MaNV,Makho) "
                            + " values( N'" + txtMahoadon.Text + "', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',N'" + cmbMaNcc.Text + "',N'" + cmbMaNV.Text + "', N'" + cmbMakho.Text + "')");
                        pd.capNhat("update Nhapkho set TongTien = 0 where Mahoadon = N'" + txtMahoadon.Text.ToString() + "'");
                        MessageBox.Show("Bạn đã thêm mới thành công");
                        dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngaynhap,MaNCC,MaNV,Makho from Nhapkho");
                        dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Nhapkho");
                    }

                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa hóa đơn nhập này không ? ", "warning ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                pd.ketnoi();
                string query5 = "select c.Mavattu from Chitietnhapkho as c join Nhapkho as n on c.Mahoadon = n.Mahoadon  where n.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + txtMahoadon.Text.ToString() + "'";
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
                    string sql10 = "select sum(c.soluong) from Chitietnhapkho as c join Nhapkho as n on c.Mahoadon = n.Mahoadon where c.MaVattu = N'" + material + "'and n.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + txtMahoadon.Text.ToString() + "'";
                    SqlCommand cmd10 = new SqlCommand(sql10, pd.Con);
                    int count10 = Convert.ToInt32(cmd10.ExecuteScalar());
                    // lấy số lượng vật tư cần xóa hiện có trong kho
                    pd.ketnoi();
                    string sql9 = "select sum(soluong) from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.MaVattu = N'" + material + "'and c.Makho = N'" + cmbMakho.Text + "'";
                    SqlCommand cmd9 = new SqlCommand(sql9, pd.Con);
                    int count9 = Convert.ToInt32(cmd9.ExecuteScalar());
                    if (count9 > count10)
                    {
                        string sql6 = "update Chitietkhohang set soluong = soluong - '" + count10 + "' " +
                                            "FROM Vattu v " +
                                            "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                            "JOIN Khohang k ON k.Makho = c.Makho " +
                                            " where c.Mavattu = N'" + material + "' and c.Makho = N'" + cmbMakho.Text + "'";
                        pd.capNhat(sql6);
                    }
                    else
                    {
                        string sql7 = "update Chitietkhohang set soluong = '0' " +
                                            "FROM Vattu v " +
                                            "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                            "JOIN Khohang k ON k.Makho = c.Makho " +
                                            " where c.Mavattu = N'" + material + "' and c.Makho = N'" + cmbMakho.Text + "'";
                        pd.capNhat(sql7);
                    }
                }
                pd.capNhat("Delete Chitietnhapkho where Mahoadon = N'" + txtMahoadon.Text.ToString() + "'");
                pd.capNhat("Delete Nhapkho where Mahoadon = N'" + txtMahoadon.Text.ToString() + "'");
                MessageBox.Show("Xóa thành công !");
                dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngaynhap,MaNCC,MaNV,Makho from Nhapkho");
                dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Nhapkho");
            }
        }

        private void btnTongtien_Click(object sender, EventArgs e)
        {
            pd.ketnoi();
            string query = "select  Mahoadon from Nhapkho";
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
                string sql1 = "update Nhapkho set TongTien = (select sum(thanhtien) from Chitietnhapkho where Mahoadon = N'" + material + "') where Mahoadon = N'" + material + "'";
                pd.capNhat(sql1);
            }

            dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Nhapkho");
        }
        private bool check()
        {
            if (txtMahoadon.Text.Trim().Equals("") ||
                cmbMaNcc.Text.Trim().Equals("") ||
                cmbMaNV.Text.Trim().Equals("") ||
                cmbMakho.Text.Trim().Equals("")
              ) return false;
            return true;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày nhập lớn hơn ngày hiện tại");
                return;
            }
            if (check())
            {
                if (MessageBox.Show("Bạn có muốn sửa hóa đơn này không ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    string sql;
                    sql = "UPDATE Nhapkho SET MaNCC=N'" + cmbMaNcc.Text +
                    "',MaNV=N'" + cmbMaNV.Text +
                     "',Ngaynhap = N'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',MAkho=N'" + cmbMakho.Text + "' WHERE Mahoadon=N'" + txtMahoadon.Text + "'";
                    pd.capNhat(sql);
                    dataGridView1.DataSource = pd.docbang("select Mahoadon,Ngaynhap,MaNCC,MaNV,Makho from Nhapkho");
                    dataGridView2.DataSource = pd.docbang("select Mahoadon,TongTien from Nhapkho");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin");
                txtMahoadon.Focus();
            }
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
            tenvung.Value = " Hóa đơn nhập kho ";
            exSheet.get_Range("A1: H1").Merge(true);
            exSheet.get_Range("A2:F2").Font.Size = 14;
            exSheet.get_Range("A2:F2").Font.Bold = true;
            exSheet.get_Range("A2").Value = "STT";
            exSheet.get_Range("B2").Value = "Mã Hóa đơn";
            exSheet.get_Range("C2").Value = "Ngày nhập";
            exSheet.get_Range("D2").Value = "Mã NCC";
            exSheet.get_Range("E2").Value = "Mã NV";
            exSheet.get_Range("F2").Value = "Mã kho";
            exSheet.get_Range("G2").Value = "Tổng tiền";

            int k = dataGridView1.Rows.Count - 1;
            exSheet.get_Range("A2:G" + (k + 2).ToString()).
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
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                exSheet.get_Range("G" + (3 + i).ToString()).Value =
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
            txtMahoadon.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            cmbMaNcc.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            cmbMaNV.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            cmbMakho.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            mainForm.ShowChiTietNK();
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            mainForm.showTimKiemHDN();
        }
    }
}
