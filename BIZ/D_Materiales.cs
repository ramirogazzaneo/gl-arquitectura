 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class D_Materiales
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

        public static DataTable rubros_listar_por_obra(int id_obra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT id_rubro, descripcion, id_obra
                           FROM dbo.rubro
                           WHERE id_obra = @id_obra and eliminado is null
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

        public static DataTable materiales_listar_por_rubro(int id_rubro)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"SELECT id_material, descripcion, cantidad, id_rubro, entrega
                           FROM dbo.materiales
                           WHERE id_rubro = @id_rubro and eliminado is null
                           ORDER BY descripcion;";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.SelectCommand.Parameters.Add("@id_rubro", SqlDbType.Int).Value = id_rubro;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static int rubro_crear(int id_obra, string descripcion)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"INSERT INTO dbo.rubro (id_obra, descripcion)
                           VALUES (@id_obra, @descripcion);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = id_obra;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50)
                       .Value = (object)(descripcion?.Trim()) ?? DBNull.Value;

                    object id = cmd.ExecuteScalar();
                    return (id == null || id == DBNull.Value) ? 0 : Convert.ToInt32(id);
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void rubro_renombrar(int id_rubro, string descripcion)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE dbo.rubro
                           SET descripcion = @descripcion
                           WHERE id_rubro = @id_rubro;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_rubro", SqlDbType.Int).Value = id_rubro;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 100)
                       .Value = (object)(descripcion?.Trim()) ?? DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void rubro_borrar(int id_rubro)
        {
            SqlConnection cn = A_conexion.conexionDB();
            SqlTransaction tx = null;
            try
            {
                tx = cn.BeginTransaction();
                string sql = @"update dbo.materiales set eliminado = GETDATE() WHERE id_rubro = @id_rubro;
                           update dbo.rubro set eliminado = GETDATE() WHERE id_rubro = @id_rubro;";
                using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                {
                    cmd.Parameters.Add("@id_rubro", SqlDbType.Int).Value = id_rubro;
                    cmd.ExecuteNonQuery();
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

        public static int material_crear(estructuras.materialCrear mat)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"INSERT INTO dbo.materiales (id_rubro, descripcion,cantidad, entrega)
                           VALUES (@id_rubro, @descripcion, @cantidad, @entrega);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_rubro", SqlDbType.Int).Value = mat.id_rubro;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 100)
                       .Value = (object)(mat.descripcion?.Trim()) ?? DBNull.Value;
                    cmd.Parameters.Add("@entrega", SqlDbType.Bit).Value = mat.entrega;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = mat.cantidad;

                    object id = cmd.ExecuteScalar();
                    return (id == null || id == DBNull.Value) ? 0 : Convert.ToInt32(id);
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void material_borrar(int id_material)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE dbo.materiales SET eliminado = GETDATE() WHERE id_material = @id_material;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_material", SqlDbType.Int).Value = id_material;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { throw e; }
            finally { cn.Close(); }
        }

        public static void material_set_entrega(int id_material, bool entrega)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE dbo.materiales 
                           SET entrega = @entrega
                           WHERE id_material = @id_material;";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id_material", SqlDbType.Int).Value = id_material;
                    cmd.Parameters.Add("@entrega", SqlDbType.Bit).Value = entrega;
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
    }
}
