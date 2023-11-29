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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Excel = Microsoft.Office.Interop.Excel;
namespace BCVlxd
{
    public partial class BaoCao : UserControl
    {
        ProcessDataBase pd = new ProcessDataBase();
        public BaoCao()
        {
            InitializeComponent();
        }

        private void guna2ComboBox1_DropDown(object sender, EventArgs e)
        {
            cmbLuaChonBC.Items.Clear();
            cmbLuaChonBC.Items.Add("Danh sách các sản phẩm không xuất được trong một quý");
            cmbLuaChonBC.Items.Add("Danh sách 2 hoá đơn có tổng tiền nhập lớn nhất");
            cmbLuaChonBC.Items.Add("Danh sách hoá đơn và tổng tiền mua hàng của một khách hàng");
            cmbLuaChonBC.Items.Add("Chi tiết danh sách các mặt hàng trong một kho");
        }

        private void cmbQuy_DropDown(object sender, EventArgs e)
        {
            cmbQuy.Items.Clear();
            cmbQuy.Items.Add("1");
            cmbQuy.Items.Add("2");
            cmbQuy.Items.Add("3");
            cmbQuy.Items.Add("4");
        }

        private void BaoCao_Load(object sender, EventArgs e)
        {
            loadComboboxMK();
            loadnam();
            loadComboboxMKH();
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
        private void loadnam()
        {
            pd.ketnoi();
            string query = "select distinct year(Ngayxuat) as Year from Xuatkho";
            SqlDataAdapter adapter = new SqlDataAdapter(query, pd.Con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            cmbNam.DataSource = dataTable;
            cmbNam.DisplayMember = "Year";
            cmbNam.ValueMember = "Year";
            cmbNam.Text = "";
            cmbNam.SelectedIndex = -1;
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

        private void lbNam_Click(object sender, EventArgs e)
        {

        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            cmbNam.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            cmbMakho.SelectedIndex = -1;
            cmbMaKH.SelectedIndex = -1;
            cmbMaKH.SelectedIndex = -1;
            cmbQuy.SelectedIndex = -1;
            if (cmbLuaChonBC.Text == "Danh sách các sản phẩm không xuất được trong một quý")
            {
                cmbMaKH.Enabled = false;
            }
            else if (cmbLuaChonBC.Text == "Danh sách 2 hoá đơn có tổng tiền nhập lớn nhất")
            {
                cmbMaKH.Enabled = false;
                cmbNam.Enabled = false;
                cmbQuy.Enabled = false;
            }
            else if (cmbLuaChonBC.Text == "Danh sách hoá đơn và tổng tiền mua hàng của một khách hàng")
            {
                cmbMakho.Enabled = false;
            }
            else if (cmbLuaChonBC.Text == "Chi tiết danh sách các mặt hàng trong một kho")
            {
                cmbMaKH.Enabled = false;
                cmbNam.Enabled = false;
                cmbQuy.Enabled = false;
            }
            else
            {
                cmbMaKH.Enabled = true;
                cmbNam.Enabled = true;
                cmbQuy.Enabled = true;
                cmbMakho.Enabled = true;
            }

        }

        private void btnHien_Click(object sender, EventArgs e)
        {
            if(cmbLuaChonBC.Text == "")
            {
                MessageBox.Show("Vui lòng chựa chọn báo cáo !");
            }
            else
            {
                if (cmbLuaChonBC.Text == "Danh sách các sản phẩm không xuất được trong một quý")
                {
                    if(cmbQuy.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn quý!");
                        cmbQuy.Focus();
                    }else if(cmbNam.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn năm !");
                        cmbNam.Focus();
                    }else if(cmbMakho.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn mã kho !");
                        cmbMakho.Focus();
                    }
                    else
                    {
                        pd.ketnoi();
                        SqlCommand command = new SqlCommand("VatTuTonKho", pd.Con);
                        command.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số
                        command.Parameters.Add(new SqlParameter("@quy", int.Parse(cmbQuy.SelectedItem.ToString())));
                        command.Parameters.Add(new SqlParameter("@nam", int.Parse(cmbNam.SelectedValue.ToString())));
                        command.Parameters.Add(new SqlParameter("@makho", cmbMakho.SelectedValue.ToString()));

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                       /* dataGridView1.Columns[0].HeaderText = "Mã vật tư";
                        dataGridView1.Columns[1].HeaderText = "Tên vật tư";
                        dataGridView1.Columns[2].HeaderText = "Giá nhập";
                        dataGridView1.Columns[3].HeaderText = "Giá xuất";
                        dataGridView1.Columns[4].HeaderText = "Mã NCC";*/
                    }
                }
                else if (cmbLuaChonBC.Text == "Danh sách 2 hoá đơn có tổng tiền nhập lớn nhất")
                {
                    if (cmbMakho.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn mã kho !");
                        cmbMakho.Focus();
                    }
                    else
                    {
                        pd.ketnoi();
                        SqlCommand command = new SqlCommand("HoaDonNhapKho", pd.Con);
                        command.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số
                        command.Parameters.Add(new SqlParameter("@makho", cmbMakho.SelectedValue));
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
                        dataGridView1.Columns[1].HeaderText = "Ngày nhập";
                        dataGridView1.Columns[2].HeaderText = "Mã NCC";
                        dataGridView1.Columns[3].HeaderText = "Mã nhân viên";
                        dataGridView1.Columns[4].HeaderText = "Mã kho";
                        dataGridView1.Columns[5].HeaderText = "Tổng tiền hóa đơn";
                    }
                }
                else if (cmbLuaChonBC.Text == "Danh sách hoá đơn và tổng tiền mua hàng của một khách hàng")
                {
                    if (cmbQuy.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn quý!");
                        cmbQuy.Focus();
                    }
                    else if (cmbNam.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn năm !");
                        cmbNam.Focus();
                    }
                    else if (cmbMaKH.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn mã kho !");
                        cmbMaKH.Focus();
                    }
                    else
                    {
                        pd.ketnoi();
                        SqlCommand command = new SqlCommand("hienthi", pd.Con);
                        command.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số
                        command.Parameters.Add(new SqlParameter("@quy", int.Parse(cmbQuy.SelectedItem.ToString())));
                        command.Parameters.Add(new SqlParameter("@makh", cmbMaKH.SelectedValue));
                        command.Parameters.Add(new SqlParameter("@nam", int.Parse(cmbNam.SelectedValue.ToString())));
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Tính tổng số tiền
                        double totalAmount = 0;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            totalAmount += Convert.ToDouble(row["TienHoaDon"]);
                        }

                        // Thêm dòng mới chứa tổng số tiền vào DataTable
                        DataRow totalRow = dataTable.NewRow();
                        totalRow["Lydoxuat"] = "Tổng tiền";
                        totalRow["TienHoaDon"] = totalAmount;
                        dataTable.Rows.Add(totalRow);

                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
                        dataGridView1.Columns[1].HeaderText = "Ngày xuất";
                        dataGridView1.Columns[2].HeaderText = "Mã khách hàng";
                        dataGridView1.Columns[3].HeaderText = "Mã kho";
                        dataGridView1.Columns[4].HeaderText = "Lý do xuất";
                        dataGridView1.Columns[5].HeaderText = "Tiền hóa đơn";
                    }
                }
                else if (cmbLuaChonBC.Text == "Chi tiết danh sách các mặt hàng trong một kho")
                {
                    if(cmbMakho.Text == "")
                    {
                        MessageBox.Show("Vui lòng chọn mã kho !");
                        cmbMakho.Focus();
                    }
                    else
                    {
                        pd.ketnoi();
                        SqlCommand command = new SqlCommand("VatTuKho", pd.Con);
                        command.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số
                        command.Parameters.Add(new SqlParameter("@makho", cmbMakho.SelectedValue));

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.Columns[0].HeaderText = "Mã vật tư";
                        dataGridView1.Columns[1].HeaderText = "Tên vật tư";
                        dataGridView1.Columns[2].HeaderText = "Số lượng";
                        dataGridView1.Columns[3].HeaderText = "Giá nhập";
                        dataGridView1.Columns[4].HeaderText = "Giá xuất";
                        dataGridView1.Columns[5].HeaderText = "Mã NCC";
                    }
                }
            }
            
        }

        private void cmbLuaChonBC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLuaChonBC.Text == "Danh sách các sản phẩm không xuất được trong một quý")
            {
                cmbMaKH.Enabled = false;
                cmbNam.Enabled = true;
                cmbQuy.Enabled = true;
                cmbMakho.Enabled = true;
            }
            else if(cmbLuaChonBC.Text == "Danh sách 2 hoá đơn có tổng tiền nhập lớn nhất")
            {
                cmbMakho.Enabled = true;
                cmbMaKH.Enabled = false;
                cmbNam.Enabled = false;
                cmbQuy.Enabled = false;
            }else if(cmbLuaChonBC.Text == "Danh sách hoá đơn và tổng tiền mua hàng của một khách hàng")
            {
                cmbMakho.Enabled = false;
                cmbMaKH.Enabled = true;
                cmbNam.Enabled = true;
                cmbQuy.Enabled = true;
            }
            else if(cmbLuaChonBC.Text == "Chi tiết danh sách các mặt hàng trong một kho")
            {
                cmbMakho.Enabled = true;
                cmbMaKH.Enabled = false;
                cmbNam.Enabled = false;
                cmbQuy.Enabled = false;
            }
            else
            {
                cmbMaKH.Enabled = true;
                cmbNam.Enabled = true;
                cmbQuy.Enabled = true;
                cmbMakho.Enabled = true;
            }
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            if (cmbLuaChonBC.Text == "Danh sách các sản phẩm không xuất được trong một quý")
            {
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook =
                exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet =
                    (Excel.Worksheet)exBook.Worksheets[1];
                Excel.Range tenvung = (Excel.Range)exSheet.Cells[1, 1];
                tenvung.Font.Name = "Arial"; tenvung.Font.Size = 16;
                tenvung.Font.Color = Color.Blue;
                tenvung.Value = " Báo cáo danh sách các sản phẩm trong kho " + cmbMakho.Text + " không xuất được trong quý " + cmbQuy.Text + " của năm " + cmbNam.Text;
                exSheet.get_Range("A1: O1").Merge(true);
                exSheet.get_Range("A2:F2").Font.Size = 14;
                exSheet.get_Range("A2:F2").Font.Bold = true;
                exSheet.get_Range("A2").Value = "STT";
                exSheet.get_Range("B2").Value = "Mã vật tư";
                exSheet.get_Range("C2").Value = "Tên vật tư";
                exSheet.get_Range("D2").Value = "Giá nhập";
                exSheet.get_Range("E2").Value = "Giá xuất";
                exSheet.get_Range("F2").Value = "Mã NCC";

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
            else if (cmbLuaChonBC.Text == "Danh sách 2 hoá đơn có tổng tiền nhập lớn nhất")
            {
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook =
                exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet =
                    (Excel.Worksheet)exBook.Worksheets[1];
                Excel.Range tenvung = (Excel.Range)exSheet.Cells[1, 1];
                tenvung.Font.Name = "Arial"; tenvung.Font.Size = 16;
                tenvung.Font.Color = Color.Blue;
                tenvung.Value = " Báo cáo danh sách 2 hoá đơn có tổng tiền nhập lớn nhất ở kho có mã " + cmbMakho.Text;
                exSheet.get_Range("A1: O1").Merge(true);
                exSheet.get_Range("A2:G2").Font.Size = 14;
                exSheet.get_Range("A2:G2").Font.Bold = true;
                exSheet.get_Range("A2").Value = "STT";
                exSheet.get_Range("B2").Value = "Mã hóa đơn";
                exSheet.get_Range("C2").Value = "Ngày nhập";
                exSheet.get_Range("D2").Value = "Mã NCC";
                exSheet.get_Range("E2").Value = "Mã nhân viên";
                exSheet.get_Range("F2").Value = "Mã kho";
                exSheet.get_Range("G2").Value = "Tổng tiền hóa đơn";

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
                    exSheet.get_Range("G" + (3 + i).ToString()).Value =
                       dataGridView1.Rows[i].Cells[5].Value.ToString();
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
            else if (cmbLuaChonBC.Text == "Danh sách hoá đơn và tổng tiền mua hàng của một khách hàng")
            {
                pd.ketnoi();
                SqlCommand command = new SqlCommand("hienthi", pd.Con);
                command.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số
                command.Parameters.Add(new SqlParameter("@quy", int.Parse(cmbQuy.SelectedItem.ToString())));
                command.Parameters.Add(new SqlParameter("@makh", cmbMaKH.SelectedValue));
                command.Parameters.Add(new SqlParameter("@nam", int.Parse(cmbNam.SelectedValue.ToString())));
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Tính tổng số tiền
                double totalAmount = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    totalAmount += Convert.ToDouble(row["TienHoaDon"]);
                }

                // Thêm dòng mới chứa tổng số tiền vào DataTable
                DataRow totalRow = dataTable.NewRow();
                totalRow["Lydoxuat"] = "Tổng tiền";
                totalRow["TienHoaDon"] = totalAmount;
                dataTable.Rows.Add(totalRow);

                dataGridView1.DataSource = dataTable;

                dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
                dataGridView1.Columns[0].Width = 80;
                dataGridView1.Columns[1].HeaderText = "Ngày xuất";
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[2].HeaderText = "Mã khách hàng";
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].HeaderText = "Mã kho";
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[4].HeaderText = "Lý do xuất";
                dataGridView1.Columns[4].Width = 140;
                dataGridView1.Columns[5].HeaderText = "Tiền hóa đơn";
                dataGridView1.Columns[5].Width = 140;
            }
            else if (cmbLuaChonBC.Text == "Chi tiết danh sách các mặt hàng trong một kho")
            {
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook =
                exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet =
                    (Excel.Worksheet)exBook.Worksheets[1];
                Excel.Range tenvung = (Excel.Range)exSheet.Cells[1, 1];
                tenvung.Font.Name = "Arial"; tenvung.Font.Size = 16;
                tenvung.Font.Color = Color.Blue;
                tenvung.Value = " Báo cáo chi tiết danh sách các mặt hàng trong kho có mã " + cmbMakho.Text;
                exSheet.get_Range("A1: O1").Merge(true);
                exSheet.get_Range("A2:G2").Font.Size = 14;
                exSheet.get_Range("A2:G2").Font.Bold = true;
                exSheet.get_Range("A2").Value = "STT";
                exSheet.get_Range("B2").Value = "Mã vật tư";
                exSheet.get_Range("C2").Value = "Tên vậ tư";
                exSheet.get_Range("D2").Value = "Số lượng";
                exSheet.get_Range("E2").Value = "Giá nhập";
                exSheet.get_Range("F2").Value = "Giá xuất";
                exSheet.get_Range("G2").Value = "Mã NCC";

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
                    exSheet.get_Range("G" + (3 + i).ToString()).Value =
                       dataGridView1.Rows[i].Cells[5].Value.ToString();
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
    }
}
