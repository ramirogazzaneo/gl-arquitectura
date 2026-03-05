using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class C_EyT
    {
       
        public static DataTable obras_listar_comboID(int ID)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT id_obra, nombre 
                               FROM dbo.obras 
                               WHERE id_usuario = @id AND eliminado IS NULL 
                               ORDER BY nombre;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = ID;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static DataTable obras_listar_comboE()
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT id_obra, nombre 
                               FROM dbo.obras 
                               WHERE eliminado IS NULL AND id_estado = 1 
                               ORDER BY nombre;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

       
        public static DataTable etapas_listar_por_obra(int id_obra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                SELECT id_etapa, id_obra, nombre
                FROM dbo.etapas
                WHERE id_obra = @id_obra AND eliminado IS NULL
                ORDER BY id_etapa;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = id_obra;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static DataTable etapas_listar_por_obra_estado(int id_obra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                SELECT id_etapa, id_obra, nombre
                FROM dbo.etapas
                WHERE id_obra = @id_obra AND eliminado IS NULL
                ORDER BY id_etapa;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = id_obra;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void etapa_renombrar(int id_etapa, string nombre)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE dbo.etapas 
                               SET nombre = @nombre 
                               WHERE id_etapa = @id_etapa;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_etapa", SqlDbType.Int).Value = id_etapa;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100)
                       .Value = (object)(nombre?.Trim()) ?? DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void etapa_borrar(int id_etapa)
        {
            SqlConnection cn = A_conexion.conexionDB();
            SqlTransaction tx = null;
            try
            {
                tx = cn.BeginTransaction();

               
                using (SqlCommand c1 = new SqlCommand(
                    "UPDATE dbo.tarea SET eliminado = GETDATE() WHERE id_etapa = @id_etapa;", cn, tx))
                {
                    c1.Parameters.Add("@id_etapa", SqlDbType.Int).Value = id_etapa;
                    c1.ExecuteNonQuery();
                }

              
                using (SqlCommand c2 = new SqlCommand(
                    "UPDATE dbo.etapas SET eliminado = GETDATE() WHERE id_etapa = @id_etapa;", cn, tx))
                {
                    c2.Parameters.Add("@id_etapa", SqlDbType.Int).Value = id_etapa;
                    c2.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch (Exception e)
            {
                tx?.Rollback();
                throw e;
            }
            finally { cn.Close(); }
        }

        public static int etapa_crear(int id_obra, string nombre)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                INSERT INTO dbo.etapas (id_obra, nombre)
                VALUES (@id_obra, @nombre);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = id_obra;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50)
                       .Value = (object)(nombre?.Trim());
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

      
       
        public static DataTable tareas_listar_por_etapa(int id_etapa)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                SELECT  t.id_tarea,
                        e.id_obra,         
                        t.id_etapa,
                        t.descripcion,
                        t.fecha_inicio,
                        t.fecha_fin,
                        t.es_hito
                FROM dbo.tarea  t
                JOIN dbo.etapas e ON e.id_etapa = t.id_etapa
                WHERE t.id_etapa = @id_etapa
                  AND t.eliminado IS NULL
                ORDER BY t.id_tarea;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_etapa", SqlDbType.Int).Value = id_etapa;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

       
        public static int tarea_crear(estructuras.tareaCrear tarea)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                INSERT INTO dbo.tarea
                    (id_etapa, descripcion, fecha_inicio, fecha_fin, es_hito)
                VALUES
                    (@id_etapa, @descripcion, @fecha_inicio, @fecha_fin, @es_hito);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_etapa", SqlDbType.Int).Value = tarea.id_etapa;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 150)
                        .Value = (object)(tarea.descripcion?.Trim()) ?? DBNull.Value;
                    cmd.Parameters.Add("@fecha_inicio", SqlDbType.Date)
                        .Value = (object)tarea.fecha_inicio ?? DBNull.Value;
                    cmd.Parameters.Add("@fecha_fin", SqlDbType.Date)
                        .Value = (object)tarea.fecha_fin ?? DBNull.Value;
                    cmd.Parameters.Add("@es_hito", SqlDbType.Bit).Value = tarea.es_hito;

                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void tarea_finalizar(int id_tarea, DateTime? fecha_fin = null)
        {
            using (SqlConnection cn = A_conexion.conexionDB())
            {
                try
                {
                    string sql = @"
DECLARE @ff  date  = ISNULL(@fecha_fin, CONVERT(date, GETDATE()));

UPDATE dbo.tarea
SET fecha_fin = @ff
WHERE id_tarea = @id_tarea
  AND eliminado IS NULL;
";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.Add("@id_tarea", SqlDbType.Int).Value = id_tarea;
                        cmd.Parameters.Add("@fecha_fin", SqlDbType.Date).Value = (object)fecha_fin ?? DBNull.Value;
                        cmd.ExecuteNonQuery();
                    }
                }
                finally { cn.Close(); }
            }
        }

        public static void tarea_borrar(int id_tarea)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = "UPDATE dbo.tarea SET eliminado = GETDATE() WHERE id_tarea = @id_tarea;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_tarea", SqlDbType.Int).Value = id_tarea;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }
    }
}
