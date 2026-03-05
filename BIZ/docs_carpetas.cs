using System;
using System.Data;
using System.Data.SqlClient;

namespace BIZ
{
    public class docs_carpetas
    {
        public static DataTable listar_por_obra(int idObra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                SELECT id_carpeta, descripcion
                FROM dbo.carpetas_docs
                WHERE id_obra = @obra AND eliminado IS NULL
                ORDER BY descripcion;";
                using (var da = new SqlDataAdapter(sql, cn))
                {
                    da.SelectCommand.Parameters.Add("@obra", SqlDbType.Int).Value = idObra;
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            finally { cn.Close(); }
        }

        public static int crear(int idObra, string descripcion)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                INSERT INTO dbo.carpetas_docs(id_obra, descripcion)
                VALUES(@obra, @desc);
                SELECT SCOPE_IDENTITY();";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@obra", SqlDbType.Int).Value = idObra;
                    cmd.Parameters.Add("@desc", SqlDbType.VarChar, 120).Value = descripcion.Trim();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            finally { cn.Close(); }
        }

        public static void eliminar_logico(int idCarpeta)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
               
                string tieneDocs = "SELECT COUNT(*) FROM dbo.documentos_tecnicos WHERE id_carpeta=@id AND eliminado IS NULL;";
                using (var cmd = new SqlCommand(tieneDocs, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCarpeta;
                    int n = Convert.ToInt32(cmd.ExecuteScalar());
                    if (n > 0) throw new Exception("La carpeta tiene documentos. Muévelos o elimínalos primero.");
                }

                using (var cmd2 = new SqlCommand("UPDATE dbo.carpetas_docs SET eliminado=GETDATE() WHERE id_carpeta=@id;", cn))
                {
                    cmd2.Parameters.Add("@id", SqlDbType.Int).Value = idCarpeta;
                    cmd2.ExecuteNonQuery();
                }
            }
            finally { cn.Close(); }
        }

        public static void RenombrarCarpeta(int idCarpeta, string nuevoNombre)
        {
            using (var cn = BIZ.A_conexion.conexionDB())
            {
                var sql = "UPDATE dbo.carpetas_docs SET descripcion=@d WHERE id_carpeta=@id AND eliminado IS NULL;";
                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@d", System.Data.SqlDbType.VarChar, 120).Value = nuevoNombre;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = idCarpeta;
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
        }

    }
}
