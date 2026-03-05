using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class B_Direccion
    {

        private static int idParaDireccion()
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COALESCE(Max(id_direccion), 100)+1 from direccion", conn);
                int id = int.Parse(cmd.ExecuteScalar().ToString());
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
        }

        public static int carga_direccion(estructuras.direccionCarga direccion)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"INSERT INTO direccion 
                           (id_direccion, calle, numero, barrio, localidad, provincia) 
                           VALUES 
                           (@id , @calle , @numero, @barrio , @localidad , @provincia)";
                int id = idParaDireccion();
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@calle", SqlDbType.VarChar).Value = direccion.calle;
                    cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value = direccion.numero;
                    cmd.Parameters.Add("@barrio", SqlDbType.VarChar).Value = direccion.barrio;
                    cmd.Parameters.Add("@localidad", SqlDbType.VarChar).Value = direccion.localidad;
                    cmd.Parameters.Add("@provincia", SqlDbType.VarChar).Value = direccion.provincia;



                    cmd.ExecuteNonQuery();
                }

                return id;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

    }
}
