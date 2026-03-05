using System;
using System.Data;
using System.Data.SqlClient;

namespace BIZ
{
    public class docs
    {
        public static int insertar_doc(
            int idCarpeta, string tipo, string nombreArchivo,
            int version, DateTime fecha, string usuario, byte[] bin)
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                INSERT INTO dbo.documentos_tecnicos
                    (id_carpeta, tipo, archivo, versionn, fecha, usuario, archivo_bin)
                VALUES
                    (@id_carpeta, @tipo, @archivo, @versionn, @fecha, @usuario, @bin);
                SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_carpeta", idCarpeta);
                    cmd.Parameters.AddWithValue("@tipo", (object)tipo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@archivo", (object)nombreArchivo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@versionn", version);
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@usuario", (object)usuario ?? DBNull.Value);
                    cmd.Parameters.Add("@bin", SqlDbType.VarBinary, -1).Value = (object)bin ?? DBNull.Value;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            finally { conn.Close(); }
        }

       
        public static DataTable listar_docs(int? idObra = null, int? idCarpeta = null)
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                SELECT d.id_documento, d.id_carpeta, c.descripcion AS carpeta,
                       d.tipo, d.archivo, d.versionn, d.fecha, d.usuario
                FROM dbo.documentos_tecnicos d
                JOIN dbo.carpetas_docs c ON c.id_carpeta = d.id_carpeta
                WHERE d.eliminado IS NULL
                  AND (@obra IS NULL OR c.id_obra = @obra)
                  AND (@carpeta IS NULL OR d.id_carpeta = @carpeta)
                ORDER BY d.fecha DESC, d.id_documento DESC;";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@obra", (object)idObra ?? DBNull.Value);
                    da.SelectCommand.Parameters.AddWithValue("@carpeta", (object)idCarpeta ?? DBNull.Value);
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            finally { conn.Close(); }
        }

        public static (string nombre, byte[] bin)? obtener_doc(int idDocumento)
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT archivo, archivo_bin FROM dbo.documentos_tecnicos
                               WHERE id_documento = @id AND eliminado IS NULL;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idDocumento;
                    using (var rd = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess))
                    {
                        if (!rd.Read()) return null;
                        string nombre = rd.GetString(0);
                        using (var ms = new System.IO.MemoryStream())
                        {
                            const int B = 81920;
                            long off = 0, r;
                            var buf = new byte[B];
                            while ((r = rd.GetBytes(1, off, buf, 0, buf.Length)) > 0)
                            { ms.Write(buf, 0, (int)r); off += r; }
                            return (nombre, ms.ToArray());
                        }
                    }
                }
            }
            finally { conn.Close(); }
        }

        public static void eliminar_doc(int idDocumento)
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                using (var cmd = new SqlCommand(
                    "UPDATE dbo.documentos_tecnicos SET eliminado = GETDATE() WHERE id_documento=@id;", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idDocumento;
                    cmd.ExecuteNonQuery();
                }
            }
            finally { conn.Close(); }
        }

      
        public static int proxima_version(int idCarpeta, string tipo)
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT ISNULL(MAX(versionn),0)+1
                               FROM dbo.documentos_tecnicos
                               WHERE id_carpeta=@carpeta AND tipo=@tipo AND eliminado IS NULL;";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@carpeta", SqlDbType.Int).Value = idCarpeta;
                    cmd.Parameters.Add("@tipo", SqlDbType.VarChar, 50).Value = (object)tipo ?? DBNull.Value;
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            finally { conn.Close(); }
        }
    }
}
