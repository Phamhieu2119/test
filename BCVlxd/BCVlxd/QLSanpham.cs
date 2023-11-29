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
    public partial class QLSanpham : Form
    {
       
        ProcessDataBase pd = new ProcessDataBase();
        private bool checkbutton = false;
        public QLSanpham()
        {
            InitializeComponent();
            txtSoluong.Enabled = false;
            pd.capNhat($"UPDATE Chitietkhohang SET soluong = COALESCE(soluong, 0);");
        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            txtSoluong.Enabled = false;
            dataGridView1.DataSource = pd.docbang("select c.Makho,v.Mavattu,v.Tenvattu,c.soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho");

        }
        private void loadComboboxMaDVT()
        {
            pd.ketnoi();
            string query = "select distinct Madonvitinh from Donvitinh";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaDVT.DataSource = dataTable;
            cmbMaDVT.DisplayMember = "Madonvitinh";
            cmbMaDVT.ValueMember = "Madonvitinh";
            cmbMaDVT.Text = "";
            cmbMaDVT.SelectedIndex = -1;
        }
        private void loadComboboxMaNCC()
        {
            pd.ketnoi();
            string query = "select distinct MaNCC from Nhacungcap";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaNCC.DataSource = dataTable;
            cmbMaNCC.DisplayMember = "MaNCC";
            cmbMaNCC.ValueMember = "MaNCC";
            cmbMaNCC.Text = "";
            cmbMaNCC.SelectedIndex = -1;
        }
        private void loadComboboxMaKho()
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
        private void QLSanpham_Load(object sender, EventArgs e)
        {
            loadComboboxMaDVT();
            loadComboboxMaNCC();
            loadComboboxMaKho();
            dataGridView1.DataSource = pd.docbang("select c.Makho,v.Mavattu,v.Tenvattu,c.soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho");
            dataGridView1.Columns[0].HeaderText = "Mã kho";

            dataGridView1.Columns[1].HeaderText = "Mã vật tư";

            dataGridView1.Columns[2].HeaderText = "Tên vật tư";

            dataGridView1.Columns[3].HeaderText = "Số lượng";

            dataGridView1.Columns[4].HeaderText = "Mã đơn vị tính";

            dataGridView1.Columns[5].HeaderText = "Mã nhà cung cấp";

            dataGridView1.Columns[6].HeaderText = "Giá nhập";

            dataGridView1.Columns[7].HeaderText = "Giá xuất";

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtSoluong.Enabled = false;
            if (txtMaVT.Text == "")
            {
                MessageBox.Show("Bạn phải nhập mã vật tư");
                txtMaVT.Focus();
            }
            else if (txtTenVT.Text == "")
            {
                MessageBox.Show("Bạn phải nhập tên vật tư");
                txtTenVT.Focus();
            }
            else if (cmbMaDVT.Text == "")
            {
                MessageBox.Show("Bạn phải chọn mã đơn bị tính !");
                cmbMaDVT.Focus();
            }
            else if (cmbMaNCC.Text == "")
            {
                MessageBox.Show("Bạn phải chọn mã nhà cung cấp !");
                cmbMaNCC.Focus();
            }
            else if (cmbMakho.Text == "")
            {
                MessageBox.Show("Bạn phải chọn mã nhà cung cấp !");
                cmbMakho.Focus();
            }
            else
            {
                pd.ketnoi();
                string sql10 = "Select count(*) from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.Mavattu = N'" + txtMaVT.Text.ToString() + "' and c.Makho = N' " + cmbMakho.Text + "'";
                SqlCommand cmd10 = new SqlCommand(sql10, pd.Con);
                int count10 = Convert.ToInt32(cmd10.ExecuteScalar());
                if (count10 == 1)
                {
                    MessageBox.Show("Mã vật tư " + txtMaVT.Text.ToString() + " đã tồn tại trong có mã " + cmbMakho.Text + ", hãy nhập mã khác!");
                    txtMaVT.Focus();
                }
                else
                {
                    pd.ketnoi();
                    string query = "select  Mavattu from Vattu";
                    SqlCommand cmd = new SqlCommand(query, pd.Con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<string> list = new List<string>();
                    while (reader.Read())
                    {
                        string Mavattu = reader["Mavattu"].ToString();
                        list.Add(Mavattu);
                    }
                    pd.dongketnoi();
                    if (list.Contains(txtMaVT.Text.ToString()))
                    {

                        pd.capNhat("INSERT INTO Chitietkhohang(Makho,Mavattu,soluong) values(N'" + cmbMakho.Text.ToString() + "',N'" + txtMaVT.Text.ToString() + "','0')");
                        MessageBox.Show("Bạn đã thêm thành công");
                        dataGridView1.DataSource = pd.docbang("select c.Makho,v.Mavattu,v.Tenvattu,c.soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho");

                    }
                    else
                    {
                        pd.capNhat("INSERT INTO Vattu (Mavattu,Tenvattu,Madonvitinh,MaNCC) "
                            + " values( N'" + txtMaVT.Text.ToString() + "', N'" + txtTenVT.Text.ToString() + "',N'" + cmbMaDVT.Text + "', N'" + cmbMaNCC.Text + "')");
                        pd.capNhat("INSERT INTO Chitietkhohang(Makho,Mavattu,soluong) values(N'" + cmbMakho.Text.ToString() + "',N'" + txtMaVT.Text.ToString() + "','0')");
                        MessageBox.Show("Bạn đã thêm mới thành công");
                        dataGridView1.DataSource = pd.docbang("select c.Makho,v.Mavattu,v.Tenvattu,c.soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho");

                    }
                }

            }
        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            txtMaVT.Text = "";
            txtTenVT.Text = "";
            cmbMaDVT.SelectedIndex = -1;
            cmbMaNCC.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            cmbMaDVT.SelectedIndex = -1;
            cmbMaNCC.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            txtSoluong.Text = "";
            
            
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            txtSoluong.Enabled = false;
            if (MessageBox.Show("Bạn có muốn xóa Vật tư này không ? ", "warning ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql16 = "Delete Chitietkhohang " +
                                 "FROM Vattu v " +
                                 "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                 "JOIN Khohang k ON k.Makho = c.Makho " +
                                 " where c.Mavattu = N'" + (txtMaVT.Text.ToString()) + "' and c.Makho = N'" + cmbMakho.Text + "'";
                pd.capNhat(sql16);
                MessageBox.Show("Xóa thành công !");
                dataGridView1.DataSource = pd.docbang("select c.Makho,v.Mavattu,v.Tenvattu,c.soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho");

            }
        }
        private bool check()
        {
            if (txtMaVT.Text.Trim().Equals("") ||
                txtTenVT.Text.Trim().Equals("") ||
                cmbMaNCC.Text.Trim().Equals("") ||
                cmbMaDVT.Text.Trim().Equals("") ||
                cmbMakho.Text.Trim().Equals("")
              ) return false;
            return true;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (MessageBox.Show("Bạn có muốn sửa sản phẩm này không ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (cmbMakho.Text != dataGridView1.CurrentRow.Cells[0].Value.ToString())
                    {
                        MessageBox.Show("Không được sửa Mã kho !");
                        cmbMakho.Focus();
                        return;
                    }
                    if (txtMaVT.Text != dataGridView1.CurrentRow.Cells[1].Value.ToString())
                    {
                        MessageBox.Show("Không được sửa Mã vật tư !");
                        txtMaVT.Focus();
                        return;
                    }
                    string sql = "update Vattu SET Tenvattu=N'" + txtTenVT.Text.ToString() +
                                  "',MaNCC=N'" + cmbMaNCC.Text +
                                  "',Madonvitinh=N'" + cmbMaDVT.Text +
                                  "' FROM Vattu v " +
                                  "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                  "JOIN Khohang k ON k.Makho = c.Makho " +
                                  " where c.Mavattu = N'" + (txtMaVT.Text.ToString()) + "' and c.Makho = N'" + cmbMakho.Text + "'";
                    pd.capNhat(sql);
                    MessageBox.Show("Sửa thành công !");
                    dataGridView1.DataSource = pd.docbang("select c.Makho,v.Mavattu,v.Tenvattu,c.soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho");

                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin");
                txtMaVT.Focus();
            }
        }

        private void btnSptrongkho_Click(object sender, EventArgs e)
        {
            txtSoluong.Enabled = false;
            checkbutton = true;
            if (cmbMakho.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mã kho !");
                cmbMakho.Focus();
            }
            else
            {
                dataGridView1.DataSource = pd.docbang("select c.Makho,v.Mavattu,v.Tenvattu,c.soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where k.Makho = N'" + cmbMakho.Text + "'");

            }
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            if (checkbutton)
            {
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook =
                exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet =
                    (Excel.Worksheet)exBook.Worksheets[1];
                Excel.Range tenvung = (Excel.Range)exSheet.Cells[1, 1];
                tenvung.Font.Name = "Arial"; tenvung.Font.Size = 16;
                tenvung.Font.Color = Color.Blue;
                tenvung.Value = " Sản phẩm trong kho có mã " + cmbMakho.Text;
                exSheet.get_Range("A1: H1").Merge(true);
                exSheet.get_Range("A2:H2").Font.Size = 14;
                exSheet.get_Range("A2:H2").Font.Bold = true;
                exSheet.get_Range("A2").Value = "STT";
                exSheet.get_Range("B2").Value = "Mã vật tư";
                exSheet.get_Range("C2").Value = "Tên vật tư";
                exSheet.get_Range("D2").Value = "Số lượng";
                exSheet.get_Range("E2").Value = "Mã DVT";
                exSheet.get_Range("F2").Value = "Mã NCC";
                exSheet.get_Range("G2").Value = "Giá nhập";
                exSheet.get_Range("H2").Value = "Giá xuất";

                int k = dataGridView1.Rows.Count - 1;
                exSheet.get_Range("A2:H" + (k + 2).ToString()).
                    Borders.LineStyle
                    = Excel.XlLineStyle.xlDouble;//.Borders( true);
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    exSheet.get_Range("A" + (3 + i).ToString()).Value =
                        (i + 1).ToString();
                    exSheet.get_Range("B" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[1].Value.ToString();
                    exSheet.get_Range("C" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[2].Value.ToString();
                    exSheet.get_Range("D" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[3].Value.ToString();
                    exSheet.get_Range("E" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[4].Value.ToString();
                    exSheet.get_Range("F" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[5].Value.ToString();
                    exSheet.get_Range("G" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[6].Value.ToString();
                    exSheet.get_Range("H" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[7].Value.ToString();

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
            else
            {
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook =
                exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet =
                    (Excel.Worksheet)exBook.Worksheets[1];
                Excel.Range tenvung = (Excel.Range)exSheet.Cells[1, 1];
                tenvung.Font.Name = "Arial"; tenvung.Font.Size = 16;
                tenvung.Font.Color = Color.Blue;
                tenvung.Value = " Sản phẩm trong các kho";
                exSheet.get_Range("A1: I1").Merge(true);
                exSheet.get_Range("A2:I2").Font.Size = 14;
                exSheet.get_Range("A2:I2").Font.Bold = true;
                exSheet.get_Range("A2").Value = "STT";
                exSheet.get_Range("B2").Value = "Mã kho";
                exSheet.get_Range("C2").Value = "Mã vật tư";
                exSheet.get_Range("D2").Value = "Tên vật tư";
                exSheet.get_Range("E2").Value = "Số lượng";
                exSheet.get_Range("F2").Value = "Mã DVT";
                exSheet.get_Range("G2").Value = "Mã NCC";
                exSheet.get_Range("H2").Value = "Giá nhập";
                exSheet.get_Range("I2").Value = "Giá xuất";

                int k = dataGridView1.Rows.Count - 1;
                exSheet.get_Range("A2:I" + (k + 2).ToString()).
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
                    exSheet.get_Range("H" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[6].Value.ToString();
                    exSheet.get_Range("I" + (3 + i).ToString()).Value =
                        dataGridView1.Rows[i].Cells[7].Value.ToString();

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
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                cmbMakho.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtMaVT.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtTenVT.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                cmbMaDVT.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                cmbMaNCC.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            txtSoluong.Enabled = true;
            if (txtTenVT.Text == "")
            {
                MessageBox.Show("Vui lòng chọn tên vật tư cần tìm kiếm !");
                txtTenVT.Focus();
            }
            else if (cmbMakho.Text == "")
            {
                MessageBox.Show("Vui lòng chọn mã kho muốn tìm kiếm !");
                cmbMakho.Focus();
            }
            else if (txtSoluong.Text == "")
            {
                MessageBox.Show("vui lòng điền số lượng !");
                txtSoluong.Focus();
            }
            else
            {
                dataGridView1.DataSource = pd.docbang("select k.Makho,k.Tenkho,v.Mavattu,v.Tenvattu,c.Soluong,v.Madonvitinh,v.MaNCC,v.Gianhap,v.Giaxuat from Vattu as v join Chitietkhohang as c " +
                                                   "on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.Soluong = N'" + txtSoluong.Text.ToString() + "' and v.Tenvattu = N'" + txtTenVT.Text.ToString() + "' and k.Makho = N'" + cmbMakho.Text + "'");

            }
        }

        private void cmbMaDVT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
