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
    public partial class ChitietXuatkho : Form
    {
        ProcessDataBase pd = new ProcessDataBase();
        private Main mainForm;
        
        public ChitietXuatkho()
        {
            InitializeComponent();
        }
        public ChitietXuatkho(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }
        private void loadCombobox()
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
            string query = "select distinct Makho from Xuatkho";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbMakho.DataSource = dataTable;
            cmbMakho.DisplayMember = "Makho";
            cmbMakho.ValueMember = "Makho";
            cmbMakho.Text = "";
            cmbMakho.SelectedIndex = -1;
        }
        private void ChitietXuatkho_Load(object sender, EventArgs e)
        {
            loadCombobox();
            loadCombobox1();
            loadCombobox2();
        }
        private void tinhtien()
        {
            pd.ketnoi();
            string query5 = "select c.Mavattu from Chitietxuatkho as c join Xuatkho as x on c.Mahoadon = x.Mahoadon  where x.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + cmbMaHD.Text + "'";
            SqlCommand cmd5 = new SqlCommand(query5, pd.Con);
            SqlDataReader reader = cmd5.ExecuteReader();
            List<string> list = new List<string>();
            while (reader.Read())
            {
                string Mavattu = reader["Mavattu"].ToString();
                list.Add(Mavattu);
            }
            foreach (string material in list)
            {
                string sql21 = "update ch set ch.thanhtien = ch.soluong * (select a.Giaxuat from (select c.Mavattu, v.Giaxuat from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.Makho = '" + cmbMakho.Text + "' and c.Mavattu = N'" + material + "') as a)" +
                    " from Chitietxuatkho as ch join Xuatkho as x on ch.Mahoadon = x.Mahoadon where x.Makho = N'" + cmbMakho.Text + "' and ch.Mahoadon = N'" + cmbMaHD.Text + "' and ch.Mavattu = N'" + material + "'";
                pd.capNhat(sql21);
            }
        }
        private void hien()
        {
            dataGridView1.DataSource = pd.docbang("select c.Mavattu,c.soluong,c.thanhtien from Chitietxuatkho as c join Xuatkho as x on c.Mahoadon = x.Mahoadon  where x.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + cmbMaHD.Text + "'");
            dataGridView1.Columns[0].HeaderText = "Mã vật tu";
           
            dataGridView1.Columns[1].HeaderText = "Số lượng";
            
            dataGridView1.Columns[2].HeaderText = "Thành tiền";
       

        }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Opacity = 1.0;
            mainForm.HideForm3();
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
            tenvung.Value = " Hóa đơn xuất kho     Mã hóa đơn :" + cmbMaHD.Text;
            exSheet.get_Range("A1: H1").Merge(true);
            exSheet.get_Range("A2:D2").Font.Size = 14;
            exSheet.get_Range("A2:D2").Font.Bold = true;
            exSheet.get_Range("A2").Value = "STT";
            exSheet.get_Range("B2").Value = "Mã vật tư";
            exSheet.get_Range("C2").Value = "Số lượng";
            exSheet.get_Range("D2").Value = "Thành tiền";

            int k = dataGridView1.Rows.Count - 1;
            exSheet.get_Range("A2:D" + (k + 2).ToString()).
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
            else if (txtSoluong.Text == "")
            {
                MessageBox.Show("Vui lòng điền số lượng nhập !");
                txtSoluong.Focus();
            }
            else
            {
                DataTable tem = pd.docbang("Select Mahoadon from Chitietxuatkho where " + "Mahoadon=N'" + cmbMaHD.Text + "' and Mavattu = N'" + cmbMaVT.Text + "'");
                if (tem.Rows.Count > 0)
                {
                    MessageBox.Show(" Mã vật tư đã tồn tại");
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
                        MessageBox.Show("Mã vật tư " + cmbMaVT.Text + " không có trong mã kho " + cmbMakho.Text);
                        return;
                    }
                    else
                    {
                        pd.ketnoi();
                        string sql10 = "select sum(soluong) from Vattu as v join Chitietkhohang as c on v.Mavattu = c.Mavattu join Khohang as k on k.Makho = c.Makho where c.MaVattu = N'" + cmbMaVT.Text + "'and c.Makho = N'" + cmbMakho.Text + "'";
                        SqlCommand cmd10 = new SqlCommand(sql10, pd.Con);
                        int count10 = Convert.ToInt32(cmd10.ExecuteScalar());
                        if (count10 > int.Parse(txtSoluong.Text))
                        {
                            string sql6 = "update  c  set c.soluong = c.soluong - '" + int.Parse(txtSoluong.Text) + "' " +
                                    "FROM Vattu v " +
                                    "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                    "JOIN Khohang k ON k.Makho = c.Makho " +
                                    " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                            pd.capNhat(sql6);
                        }
                        else
                        {
                            MessageBox.Show("Mã vật tư " + cmbMaVT.Text + "  trong kho có mã  " + cmbMakho.Text + " không đủ số lượng để xuất");
                            txtSoluong.Focus();
                            return;
                        }
                    }
                    pd.capNhat("INSERT INTO Chitietxuatkho (Mahoadon,Mavattu,soluong) "
                        + " values( N'" + cmbMaHD.Text + "', N'" + cmbMaVT.Text + "','" + int.Parse(txtSoluong.Text.ToString()) + "')");
                    tinhtien();
                    MessageBox.Show("Bạn đã thêm mới thành công !");
                    hien();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa vật tư ở hóa đơn xuất này không ? ", "warning ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                string sql14 = "update Chitietkhohang set soluong = soluong + '" + int.Parse(txtSoluong.Text) + "' " +
                                    "FROM Vattu v " +
                                    "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                    "JOIN Khohang k ON k.Makho = c.Makho " +
                                    " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                pd.capNhat(sql14);

                string sql8 = "delete c from Chitietxuatkho as c join Xuatkho as x on c.Mahoadon = x.Mahoadon where c.Mahoadon =N'" + cmbMaHD.Text + "'and c.Mavattu = N'" + cmbMaVT.Text + "' and x.Makho = N'" + cmbMakho.Text + "'";
                pd.capNhat(sql8);
                MessageBox.Show("Xóa thành công !");
                hien();
            }
        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            cmbMaVT.SelectedIndex = -1;
            cmbMaVT.SelectedIndex = -1;
            txtSoluong.Text = "";
        }
        private bool check()
        {
            if (cmbMaHD.Text.Trim().Equals("") ||
                cmbMaVT.Text.Trim().Equals("") ||
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
                    int valuesold = int.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());

                    if (valuesold > int.Parse(txtSoluong.Text.ToString()))
                    {
                        string sql8 = "update  c  set c.soluong = c.soluong + '" + (valuesold - int.Parse(txtSoluong.Text.ToString())) + "' " +
                                    "FROM Vattu v " +
                                    "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                    "JOIN Khohang k ON k.Makho = c.Makho " +
                                    " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                        pd.capNhat(sql8);

                    }
                    if (valuesold < int.Parse(txtSoluong.Text.ToString()))
                    {
                        pd.ketnoi();
                        string sql20 = "select sum(c.soluong) from Chitietxuatkho as c join Xuatkho as x on c.Mahoadon = x.Mahoadon where c.MaVattu = N'" + cmbMaVT.Text + "'and x.Makho = N'" + cmbMakho.Text + "' and c.Mahoadon = N'" + cmbMaHD.Text + "'";
                        SqlCommand cmd20 = new SqlCommand(sql20, pd.Con);
                        int count20 = Convert.ToInt32(cmd20.ExecuteScalar());
                        if (count20 > (int.Parse(txtSoluong.Text.ToString()) - valuesold))
                        {
                            string sql9 = "update  c  set c.soluong = c.soluong - '" + (int.Parse(txtSoluong.Text.ToString()) - valuesold) + "' " +
                                    "FROM Vattu v " +
                                    "JOIN Chitietkhohang c ON v.Mavattu = c.Mavattu " +
                                    "JOIN Khohang k ON k.Makho = c.Makho " +
                                    " where c.Mavattu = N'" + cmbMaVT.Text + "' and c.Makho = N'" + cmbMakho.Text + "'";
                            pd.capNhat(sql9);
                        }
                        else
                        {
                            MessageBox.Show("Mã vật tư " + cmbMaVT.Text + "  trong kho có mã  " + cmbMakho.Text + " không đủ số lượng để xuất");
                            txtSoluong.Focus();
                            return;
                        }


                    }

                    string sql;

                    sql = $"UPDATE Chitietxuatkho SET soluong='{int.Parse(txtSoluong.Text.ToString())}'" +
                    $"where Mahoadon = N'{cmbMaHD.Text}' and Mavattu=N'{cmbMaVT.Text}'";

                    pd.capNhat(sql);
                    tinhtien();
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

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                cmbMaVT.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtSoluong.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

            }
        }

        private void btnDulieu_Click(object sender, EventArgs e)
        {
            tinhtien();
            pd.ketnoi();
            string sql3 = "select count(*) from Xuatkho where Mahoadon = N'" + cmbMaHD.Text + "'and Makho = N'" + cmbMakho.Text + "'";
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
    }
}
