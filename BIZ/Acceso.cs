using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class Acceso
    {

        public static bool puedeUsarioIngresarPagina(int nivelId, string pagina)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT COUNT(*) from PaginaNivel where id_nivel = @nivelId AND pagina = @pagina";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@nivelId", SqlDbType.Int).Value = nivelId;
                cmd.Parameters.Add("@pagina", SqlDbType.VarChar).Value = pagina;
                int cont = int.Parse(cmd.ExecuteScalar().ToString());
                return cont > 0;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }


    }
}
