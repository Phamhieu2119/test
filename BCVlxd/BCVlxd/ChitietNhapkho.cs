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
    public partial class ChitietNhapkho : Form
    {
        ProcessDataBase pd = new ProcessDataBase();
        private Main mainForm;
        public ChitietNhapkho()
        {
            InitializeComponent();
        }
        public ChitietNhapkho(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }
        private void loadCombobox()
        {
            pd.ketnoi();
            string query = "select distinct Mahoadon from Nhapkho";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMaHD.DataSource = dataTable;
            cmbMaHD.DisplayMember = "Mahoadon";
            cmbMaHD.ValueMember = "Mahoadon";
            cmbMaHD.Text = "";
            cmbMaHD.SelectedIndex = -1;
        }
        private void loadCombobox1()
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
        private void loadCombobox2()
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
        private void ChitietNhapkho_Load(object sender, EventArgs e)
        {
            loadCombobox();
            loadCombobox1();
            loadCombobox2();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                cmbMaVT.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtDongianhap.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtSoluong.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            }
        }
        private void hien()
        {
            dataGridView1.DataSource = pd.docbang("select c.Mavattu,c.dongianhap,c.soluong,c.thanhtien from Chitietnhapkho as c join Nhapkho as n on c.Mahoadon = n.Mahoadon  where n.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + cmbMaHD.Text + "'");
            dataGridView1.Columns[0].HeaderText = "Mã vật tu";
            dataGridView1.Columns[1].HeaderText = "Đơn giá nhập";
            dataGridView1.Columns[2].HeaderText = "Số lượng";
            dataGridView1.Columns[3].HeaderText = "Thành tiền";
        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            string sql = "update Chitietnhapkho set thanhtien = soluong * dongianhap";
            pd.capNhat(sql);

            pd.ketnoi();
            string sql3 = "select count(*) from Nhapkho where Mahoadon = N'" + cmbMaHD.Text + "'and Makho = N'" + cmbMakho.Text + "'";
            SqlCommand cmd = new SqlCommand(sql3, pd.Con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 1)
            {
                hien();
            }
            else
            {
                MessageBox.Show("Chọn sai mã kho vui lòng chọn lại");
                cmbMakho.Focus();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cmbMaHD.Text == "")
            {
                MessageBox.Show("Vui lòng chọn mã hóa đơn !");
                cmbMaHD.Focus();
            }
            else if (cmbMaVT.Text == "")
            {
                MessageBox.Show("Vui lòng chọn mã vật tư");
                cmbMaVT.Focus();
            }
            else if (txtDongianhap.Text == "")
            {
                MessageBox.Show("Vui lòng điền mã nhap !");
                txtDongianhap.Focus();
            }
            else if (txtSoluong.Text == "")
            {
                MessageBox.Show("Vui lòng điền số lượng nhập !");
                txtSoluong.Focus();
            }
            else
            {
                DataTable tem = pd.docbang("Select Mahoadon from Chitietnhapkho where " + "Mahoadon=N'" + cmbMaHD.Text + "' and Mavattu = N'" + cmbMaVT.Text + "'");
                if (tem.Rows.Count > 0)
                {
                    MessageBox.Show(" Mã vât tư  đã tồn tại");
                    cmbMaVT.Focus();
                }
                else
                {
                    pd.ketnoi();
                    string sql4 = "select count(*) from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.MaVattu = N'" + cmbMaVT.Text + "'and c.Makho = N'" + cmbMakho.Text + "'";
                    SqlCommand cmd4 = new SqlCommand(sql4, pd.Con);
                    int count = Convert.ToInt32(cmd4.ExecuteScalar());
                    if (count != 1)
                    {
                        pd.capNhat("Insert into Chitietkhohang(Makho,Mavattu,soluong) values(N'" + cmbMakho.Text + "',N'" + cmbMaVT.Text + "','0')");
                    }
                    string sql = "UPDATE v " +
                                    "SET v.Gianhap = '" + float.Parse(txtDongianhap.Text) + "', v.Giaxuat = '" + (float.Parse(txtDongianhap.Text) * 1.1) + "' " +
                                    "FROM Vattu v " +
                                    "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                    "JOIN Khohang k ON k.Makho = c.Makho " +
                                    "WHERE c.Mavattu = N'" + cmbMaVT.Text + "' AND c.Makho = N'" + cmbMakho.Text + "'";
                    pd.capNhat(sql);
                    string sql1 = "update  c  set c.soluong = c.soluong + '" + int.Parse(txtSoluong.Text) + "' " +
                                    "FROM Vattu v " +
                                    "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                    "JOIN Khohang k ON k.Makho = c.Makho " +
                                    " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                    pd.capNhat(sql1);
                    pd.capNhat("INSERT INTO Chitietnhapkho (Mahoadon,Mavattu,dongianhap,soluong) "
                        + " values( N'" + cmbMaHD.Text + "', N'" + cmbMaVT.Text + "', '" + float.Parse(txtDongianhap.Text) + "','" + int.Parse(txtSoluong.Text) + "')");
                    string sql2 = "update Chitietnhapkho set thanhtien = soluong * dongianhap";
                    pd.capNhat(sql2);
                    MessageBox.Show("Bạn đã thêm mới thành công !");
                    hien();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa vật tư ở hóa đơn nhập này không ? ", "warning ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                pd.ketnoi();
                string sql9 = "select sum(soluong) from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.MaVattu = N'" + cmbMaVT.Text + "'and c.Makho = N'" + cmbMakho.Text + "'";
                SqlCommand cmd9 = new SqlCommand(sql9, pd.Con);
                int count9 = Convert.ToInt32(cmd9.ExecuteScalar());
                if (count9 > int.Parse(txtSoluong.Text))
                {
                    string sql6 = "update Chitietkhohang set soluong = soluong - '" + int.Parse(txtSoluong.Text) + "' " +
                                        "FROM Vattu v " +
                                        "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                        "JOIN Khohang k ON k.Makho = c.Makho " +
                                        " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                    pd.capNhat(sql6);
                }
                else
                {
                    string sql7 = "update Chitietkhohang set soluong = '0' " +
                                        "FROM Vattu v " +
                                        "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                        "JOIN Khohang k ON k.Makho = c.Makho " +
                                        " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                    pd.capNhat(sql7);
                }
                string sql8 = "delete c from Chitietnhapkho as c join Nhapkho as n on c.Mahoadon = n.Mahoadon where c.Mahoadon =N'" + cmbMaHD.Text + "'and c.Mavattu = N'" + cmbMaVT.Text + "' and n.Makho = N'" + cmbMakho.Text + "'";
                pd.capNhat(sql8);
                MessageBox.Show("Xóa thành công !");
                hien();
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
            tenvung.Value = " Hóa đơn nhập kho     Mã hóa đơn :" + cmbMaHD.Text;
            exSheet.get_Range("A1: H1").Merge(true);
            exSheet.get_Range("A2:E2").Font.Size = 14;
            exSheet.get_Range("A2:E2").Font.Bold = true;
            exSheet.get_Range("A2").Value = "STT";
            exSheet.get_Range("B2").Value = "Mã vật tư";
            exSheet.get_Range("C2").Value = "Đơn giá nhập";
            exSheet.get_Range("D2").Value = "Số lượng";
            exSheet.get_Range("E2").Value = "Thành tiền";

            int k = dataGridView1.Rows.Count - 1;
            exSheet.get_Range("A2:E" + (k + 2).ToString()).
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

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            cmbMaVT.SelectedIndex = -1;
            cmbMaVT.SelectedIndex = -1;
            txtSoluong.Text = "";
            txtDongianhap.Text = "";
        }
        private bool check()
        {
            if (cmbMaHD.Text.Trim().Equals("") ||
                cmbMaVT.Text.Trim().Equals("") ||
                txtDongianhap.Text.Trim().Equals("") ||
                txtSoluong.Text.Trim().Equals("")
              ) return false;
            return true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (check())
            {

                if (MessageBox.Show("Bạn có muốn sửa hóa đơn này không ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (cmbMaVT.Text != dataGridView1.CurrentRow.Cells[0].Value.ToString())
                    {
                        MessageBox.Show("Không được sửa mã vật tư !");
                        cmbMaVT.Focus();
                        return;

                    }
                    int valuesold = int.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

                    if (valuesold > int.Parse(txtSoluong.Text.ToString()))
                    {
                        pd.ketnoi();
                        string sql10 = "select sum(soluong) from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.MaVattu = N'" + cmbMaVT.Text + "'and c.Makho = N'" + cmbMakho.Text + "'";
                        SqlCommand cmd10 = new SqlCommand(sql10, pd.Con);
                        int count10 = Convert.ToInt32(cmd10.ExecuteScalar());
                        if (count10 > (valuesold - int.Parse(txtSoluong.Text)))
                        {
                            string sql1 = "update c set soluong = soluong - '" + (valuesold - int.Parse(txtSoluong.Text)) + "' " +
                            "FROM Vattu v " +
                            "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                            "JOIN Khohang k ON k.Makho = c.Makho " +
                            " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                            pd.capNhat(sql1);
                        }
                        else
                        {
                            string sql11 = "update Chitietkhohang set soluong = '0' " +
                                                "FROM Vattu v " +
                                                "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                                "JOIN Khohang k ON k.Makho = c.Makho " +
                                                " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                            pd.capNhat(sql11);
                        }

                    }
                    if (valuesold < int.Parse(txtSoluong.Text.ToString()))
                    {
                        string sql1 = "update Chitietkhohang set soluong = soluong + '" + (int.Parse(txtSoluong.Text) - valuesold) + "' " +
                                    "FROM Vattu v " +
                                    "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                    "JOIN Khohang k ON k.Makho = c.Makho " +
                                    " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                        pd.capNhat(sql1);
                    }

                    string sql3 = "UPDATE v " +
                                      "SET v.Gianhap = '" + float.Parse(txtDongianhap.Text) + "', v.Giaxuat = '" + (float.Parse(txtDongianhap.Text) * 1.1) + "' " +
                                      "FROM Vattu v " +
                                      "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                      "JOIN Khohang k ON k.Makho = c.Makho " +
                                      "WHERE c.Mavattu = N'" + cmbMaVT.Text + "' AND c.Makho = N'" + cmbMakho.Text + "'";
                    pd.capNhat(sql3);


                    string sql;
                    sql = $"UPDATE Chitietnhapkho SET soluong='{int.Parse(txtSoluong.Text.ToString())}'," +
                    $"dongianhap = '{float.Parse(txtDongianhap.Text.ToString())}' " +
                    $"where Mahoadon = N'{cmbMaHD.Text}' and Mavattu=N'{cmbMaVT.Text}'";

                    pd.capNhat(sql);
                    string sql5 = "update Chitietnhapkho set thanhtien = soluong * dongianhap";
                    pd.capNhat(sql5);
                    MessageBox.Show("Bạn đã sửa thành công !");
                    hien();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin");
                cmbMaHD.Focus();
            }
        }

        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Opacity = 1.0;
            mainForm.HideChiTietNK();
        }
    }
}
