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
    public class D_Finanzas
    {

        public static DataTable pagos_listar_por_obra(int id_obra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
        SELECT 
            p.id_pago, p.id_obra, p.id_usuario, p.descripcion,
            p.fecha, p.fecha_ven, p.monto, p.estado_pago,  
            CASE 
              WHEN p.estado_pago = 0 
               AND p.fecha_ven IS NOT NULL
               AND CAST(p.fecha_ven AS date) < CAST(GETDATE() AS date)
              THEN 1 ELSE 0
            END AS vencido
        FROM pagos p
        WHERE p.id_obra = @id_obra AND p.eliminado IS NULL
        ORDER BY 
            CASE WHEN p.estado_pago = 0 
                   AND p.fecha_ven IS NOT NULL
                   AND CAST(p.fecha_ven AS date) < CAST(GETDATE() AS date)
                 THEN 0 ELSE 1 END,
            CASE WHEN p.estado_pago = 0 THEN 0 ELSE 1 END,  
            p.fecha_ven, p.id_pago;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = id_obra;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            finally { cn.Close(); }
        }

        public static int obra_get_id_usuario(int id_obra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = "SELECT id_usuario FROM obras WHERE id_obra = @id_obra;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id_obra", id_obra);
                    object o = cmd.ExecuteScalar();
                    return (o == null || o == DBNull.Value) ? 0 : Convert.ToInt32(o);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static int pago_crear(pagoCrear pago)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                int id = obra_get_id_usuario(pago.id_obra);

                string sql = @"
                INSERT INTO pagos
                    (id_obra, id_usuario, descripcion, fecha, fecha_ven, monto, estado_pago)
                VALUES
                    (@id_obra, @id_usuario, @descripcion, @fecha,
                     DATEADD(DAY, 15, @fecha), @monto, @estado);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = pago.id_obra;
                    cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = pago.descripcion;
                    cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = pago.fecha.Date;
                    cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = pago.monto;
                    cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = pago.estado_pago;

                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static void pago_actualizar(formPago pago )
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                UPDATE dbo.pagos
                SET monto = @monto, descripcion = @descripcion 
                WHERE id_pago = @id;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = pago.id_pago;

                    var pMonto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
                    pMonto.Precision = 18;  
                    pMonto.Scale = 2;        
                    pMonto.Value = pago.monto;   

                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50)
                       .Value = (object)(pago.descripcion?.Trim()) ?? DBNull.Value;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static void pago_cambiar_estado(int id_pago, bool pagado)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = "UPDATE dbo.pagos SET estado_pago=@e WHERE id_pago=@id;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id_pago;
                    cmd.Parameters.Add("@e", SqlDbType.Bit).Value = pagado;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static void pago_eliminar(int id_pago)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = "UPDATE pagos SET eliminado = CONVERT(date, GETDATE()) WHERE id_pago=@id;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id_pago;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static DataTable obras_listar_por_usuario(int id_usuario)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
                SELECT id_obra, nombre
                FROM obras
                WHERE id_usuario = @id_usuario AND (eliminado IS NULL)
                ORDER BY nombre;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = id_usuario;

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

        public static DataTable pagos_listar_usuario(int id_usuario, int id_obra, bool soloPendientes)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"
            SELECT  p.id_pago, p.id_obra, o.nombre AS obra,
                    p.descripcion, p.fecha, p.fecha_ven,
                    p.monto, p.estado_pago 
            FROM pagos p
            JOIN obras o ON o.id_obra = p.id_obra
            WHERE o.id_usuario = @id_usuario
              AND (@id_obra = 0 OR p.id_obra = @id_obra)
              AND p.eliminado IS NULL
              AND (@soloPend = 0 OR p.estado_pago = 0)
            ORDER BY p.fecha_ven, p.id_pago;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = id_usuario;
                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = id_obra;
                    cmd.Parameters.Add("@soloPend", SqlDbType.Bit).Value = soloPendientes;

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