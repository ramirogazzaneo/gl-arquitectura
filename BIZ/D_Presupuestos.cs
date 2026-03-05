using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIZ.estructuras;

namespace BIZ
{
    public class D_Presupuestos
    {

        public static DataTable obras_listar_combo()
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT id_obra, nombre 
                           FROM dbo.obras 
                           WHERE eliminado IS NULL 
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

        public static DataTable rubrosR_listar_por_obra(int id_obra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT id_rubroR, descripcion, id_obra
                           FROM dbo.rubroR
                           WHERE id_obra = @id_obra AND eliminado is null
                           ORDER BY descripcion;";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.SelectCommand.Parameters.Add("@id_obra", SqlDbType.Int).Value = id_obra;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }


        public static DataTable presupuestos_listar_por_rubro(int id_rubroR)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
        SELECT  p.id_presupuesto,
                p.id_tipo,
                tp.descripcion AS tipo_desc,
                p.valor,
                p.id_rubroR,
                p.monto_real,
                p.fechainicio,
                p.fechafin,
                p.descripcion
        FROM dbo.presupuestos p
        LEFT JOIN dbo.tipo_presupuesto tp ON tp.id_tipo = p.id_tipo
        WHERE p.id_rubroR = @id_rubroR AND p.eliminado IS NULL
        ORDER BY p.id_presupuesto;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_rubroR", SqlDbType.Int).Value = id_rubroR;
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

        public static void rubroR_renombrar(RubroActualizar Rubro)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE dbo.rubroR 
                           SET descripcion=@descripcion
                           WHERE id_rubroR=@id_rubroR;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id_rubroR", Rubro.id_rubroR);
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50)
                       .Value = (object)(Rubro.descripcion?.Trim()) ?? DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void rubroR_borrar(int id_rubroR)
        {
            SqlConnection cn = A_conexion.conexionDB();
            SqlTransaction tx = null;
            try
            {
                tx = cn.BeginTransaction();

                var c1 = new SqlCommand("update presupuestos set eliminado = GETDATE() WHERE id_rubroR=@id_rubroR;", cn, tx);
                c1.Parameters.Add("@id_rubroR", SqlDbType.Int).Value = id_rubroR;
                c1.ExecuteNonQuery();

                var c2 = new SqlCommand("update rubroR set eliminado = GETDATE() WHERE id_rubroR=@id_rubroR;", cn, tx);
                c2.Parameters.Add("@id_rubroR", SqlDbType.Int).Value = id_rubroR;
                c2.ExecuteNonQuery();

                tx.Commit();
            }
            catch (Exception e)
            {
                tx?.Rollback();
                throw e;
            }
            finally { cn.Close(); }
        }

        public static int presupuesto_crear(PresupuestoCrear Pres)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
        DECLARE @hoy DATE = CONVERT(date, GETDATE());
        INSERT INTO dbo.presupuestos
            ( id_tipo, valor, fechainicio, fechafin, monto_real, id_rubroR, descripcion)
        VALUES
            (@id_tipo, @valor, @hoy, DATEADD(MONTH, 3, @hoy), @monto_real, @id_rubroR, @descripcion);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    
                    cmd.Parameters.AddWithValue("@id_tipo", Pres.id_tipo);
                    cmd.Parameters.Add("@valor", SqlDbType.VarChar, 50)
                       .Value = (object)(Pres.valor?.Trim()) ?? DBNull.Value;
                    cmd.Parameters.Add("@monto_real", SqlDbType.Decimal).Value = Pres.monto_real;
                    cmd.Parameters.AddWithValue("@id_rubroR", Pres.id_rubroR);
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 200)
                       .Value = (object)(Pres.descripcion?.Trim()) ?? DBNull.Value;

                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static int rubroR_crear(int ID, String DESC)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                INSERT INTO dbo.rubroR (id_obra, descripcion)
                VALUES (@id_obra, @descripcion);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id_obra", ID);
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50)
                       .Value = (object)(DESC?.Trim()) ?? DBNull.Value;

                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void presupuesto_actualizar(PresupuestoActualizar Pres)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE dbo.presupuestos
                           SET monto_real=@monto, descripcion=@descripcion
                           WHERE id_presupuesto=@id;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", Pres.id_presupuesto);
                    cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = Pres.monto_real;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50)
                       .Value = (object)(Pres.descripcion?.Trim()) ?? DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void presupuesto_borrar(int id_presupuesto)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = "update dbo.presupuestos SET eliminado = GETDATE() WHERE id_presupuesto=@id;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id_presupuesto;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

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
        public static DataTable Tipo_listar()
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT id_tipo, descripcion 
                           FROM dbo.tipo_presupuesto;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
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
    }
}
