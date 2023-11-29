using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;
using BCVlxd;

namespace connectdatabase
{
     class ProcessDataBase
    {
        SqlConnection con = null;
        String constring = @"Data Source=(local);Initial Catalog=qlvlxd;Integrated Security=True";

        public SqlConnection Con { get => con; set => con = value; }

        public ProcessDataBase() {
           
        }
        
        public void ketnoi()
        {
            con = new SqlConnection(constring);
            if (con.State != ConnectionState.Open)
                con.Open();
        }
        public void dongketnoi()
        {
            if (con.State != ConnectionState.Closed)
                con.Close();
            con.Dispose();
        }
        public DataTable docbang(String sql) {
            DataTable tb=new DataTable();
            ketnoi();
            SqlDataAdapter da = new SqlDataAdapter(sql,con);
            da.Fill(tb);
            dongketnoi();
            return tb;
        }
        public void capNhat(String sql)
        {
            ketnoi();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dongketnoi();
            cmd.Dispose();
        }
        
    }
}
