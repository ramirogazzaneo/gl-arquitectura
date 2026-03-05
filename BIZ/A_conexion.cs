using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BIZ
{
    public class A_conexion
    {
        public static SqlConnection conexionDB()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["SQL"].ToString();
            conn.Open();
            return conn;
        }
    }
}
