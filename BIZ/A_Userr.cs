using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIZ.estructuras;
using System.Diagnostics.Contracts;

namespace BIZ
{
    public class A_Userr
    {


        private static int idParaUsuario()
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COALESCE(Max(id_usuario), 100)+1 from usuarios", conn);
                int id = int.Parse(cmd.ExecuteScalar().ToString());
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
        }

        public static int cargar_usuario(estructuras.usuarioCarga usuario)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"INSERT INTO usuarios 
                           (id_usuario, nombre, apellido, email, contraseña, telefono, id_nivel) 
                           VALUES 
                           (@id , @nombre , @apellido, @email , @contraseña , @telefono, @id_nivel)";
                int id = idParaUsuario();
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.nombre;
                    cmd.Parameters.Add("@apellido", SqlDbType.VarChar).Value = usuario.apellido;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = usuario.email;
                    cmd.Parameters.Add("@contraseña", SqlDbType.VarChar).Value = usuario.contraseña;
                    cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = usuario.telefono;
                    cmd.Parameters.Add("@id_nivel", SqlDbType.Int).Value = 1; // Por defecto, nivel de cliente


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

        public static usuario traerUsuario(int id)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string ls_sql = @"SELECT email, telefono, nombre, apellido, id_nivel FROM usuarios WHERE id_usuario=@id";
                SqlCommand cmd = new SqlCommand(ls_sql, cn)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new usuario
                        {
                            email = reader.GetString(reader.GetOrdinal("email")),
                            telefono = reader.GetString(reader.GetOrdinal("telefono")),
                            nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            apellido = reader.GetString(reader.GetOrdinal("apellido")),
                            nivel = reader.GetInt32(reader.GetOrdinal("id_nivel")),
                        };
                    }
                }
                return default;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static void actualizar_contraseña(int userId, string contraseña)
        {
            if (userId == 0) { throw new ArgumentNullException("userId"); }
            if (contraseña == null) { throw new ArgumentNullException("contraseña"); }
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE usuarios
                           SET contraseña = @contraseña WHERE id_usuario = @id";
                int id = idParaUsuario();
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@contraseña", SqlDbType.VarChar).Value = contraseña;

                    cmd.ExecuteNonQuery();
                }
                return;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static (bool esValido, int userId) ValidarContrasenia(string correo, string contra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string ls_sql = @"SELECT id_usuario, contraseña FROM usuarios 
                   WHERE email = @email";
                SqlCommand cmd = new SqlCommand(ls_sql, cn);
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = correo;
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(reader.GetOrdinal("id_usuario"));
                        string contraDB = reader.GetString(reader.GetOrdinal("contraseña"));
                        string Ccontra = Hasher.encriptar(contra);



                        if (Ccontra == contraDB)
                        {
                            return (true, userId);
                        }
                    }
                }

                return (false, -1);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
        }

        
        public static List<usuario> traerUsuarios()
        {
            SqlConnection cn = A_conexion.conexionDB();
            List<usuario> usuarios = new List<usuario>();
            try
            {
                string query = @"SELECT id_usuario, email, telefono, nombre, apellido, u.id_nivel 
                        FROM usuarios u LEFT JOIN nivel n ON n.id_nivel = u.id_nivel
                        where u.eliminado IS NULL
                        ORDER BY u.id_nivel desc";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuario user = new usuario
                            {
                                id = reader.GetInt32(0),
                                email = reader.GetString(1),
                                telefono = reader.GetString(2),
                                nombre = reader.GetString(3),
                                apellido = reader.GetString(4),
                                nivel = reader.GetInt32(5)
                            };
                            usuarios.Add(user);
                        }
                    }
                }
                return usuarios;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static List<(int id_nivel, string descripcion)> traerNiveles()
        {
            SqlConnection cn = A_conexion.conexionDB();
            List<(int id_nivel, string descripcion)> niveles = new List<(int id_nivel, string descripcion)>();
            try
            {
                string query = "SELECT id_nivel, descripcion FROM nivel ORDER BY id_nivel";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        niveles.Add((
                            reader.GetInt32(0),
                            reader.GetString(1)
                        ));
                    }
                }
                return niveles;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static void eliminarUsuario(int idUsuario)
        {
            using (SqlConnection cn = A_conexion.conexionDB())
            {
                if (cn.State != ConnectionState.Open) cn.Open();

                using (SqlTransaction tran = cn.BeginTransaction())
                {
                    try
                    {
                      
                        using (var cmd = new SqlCommand(@"
                    UPDATE p SET eliminado = GETDATE()
                    FROM presupuestos p
                    INNER JOIN rubroR rr ON rr.id_rubroR = p.id_rubroR
                    INNER JOIN obras  o  ON o.id_obra    = rr.id_obra
                    WHERE o.id_usuario = @uid AND p.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                        
                        using (var cmd = new SqlCommand(@"
                    UPDATE rr SET eliminado = GETDATE()
                    FROM rubroR rr
                    INNER JOIN obras o ON o.id_obra = rr.id_obra
                    WHERE o.id_usuario = @uid AND rr.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                      
                        using (var cmd = new SqlCommand(@"
                    UPDATE m SET eliminado = GETDATE()
                    FROM materiales m
                    INNER JOIN rubro r ON r.id_rubro = m.id_rubro
                    INNER JOIN obras o ON o.id_obra  = r.id_obra
                    WHERE o.id_usuario = @uid AND m.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                        
                        using (var cmd = new SqlCommand(@"
                    UPDATE r SET eliminado = GETDATE()
                    FROM rubro r
                    INNER JOIN obras o ON o.id_obra = r.id_obra
                    WHERE o.id_usuario = @uid AND r.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                        
                        using (var cmd = new SqlCommand(@"
                    UPDATE t SET eliminado = GETDATE()
                    FROM tarea t
                    INNER JOIN etapas e ON e.id_etapa = t.id_etapa
                    INNER JOIN obras  o ON o.id_obra  = e.id_obra
                    WHERE o.id_usuario = @uid AND t.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                     
                        using (var cmd = new SqlCommand(@"
                    UPDATE e SET eliminado = GETDATE()
                    FROM etapas e
                    INNER JOIN obras o ON o.id_obra = e.id_obra
                    WHERE o.id_usuario = @uid AND e.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                       
                        using (var cmd = new SqlCommand(@"
                    UPDATE pg SET eliminado = GETDATE()
                    FROM pagos pg
                    INNER JOIN obras o ON o.id_obra = pg.id_obra
                    WHERE o.id_usuario = @uid AND pg.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                      
                       
                        using (var cmd = new SqlCommand(@"
                    UPDATE d SET d.eliminado = GETDATE()
                    FROM documentos_tecnicos d
                    INNER JOIN carpetas_docs c ON c.id_carpeta = d.id_carpeta
                    INNER JOIN obras o ON o.id_obra = c.id_obra
                    WHERE o.id_usuario = @uid
                      AND d.eliminado IS NULL
                      AND c.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                        
                        using (var cmd = new SqlCommand(@"
                    UPDATE c SET c.eliminado = GETDATE()
                    FROM carpetas_docs c
                    INNER JOIN obras o ON o.id_obra = c.id_obra
                    WHERE o.id_usuario = @uid AND c.eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }
                        

                       
                        using (var cmd = new SqlCommand(@"
                    UPDATE d SET d.eliminado = GETDATE()
                    FROM direccion d
                    INNER JOIN obras o ON o.id_direccion = d.id_direccion
                    WHERE o.id_usuario = @uid
                      AND d.eliminado IS NULL
                      AND NOT EXISTS (
                          SELECT 1
                          FROM obras o2
                          WHERE o2.id_direccion = d.id_direccion
                            AND o2.eliminado IS NULL
                            AND o2.id_usuario <> @uid
                      );", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                       
                        using (var cmd = new SqlCommand(@"
                    UPDATE obras SET eliminado = GETDATE()
                    WHERE id_usuario = @uid AND eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                       
                        using (var cmd = new SqlCommand(@"
                    UPDATE usuarios SET eliminado = GETDATE()
                    WHERE id_usuario = @uid AND eliminado IS NULL;", cn, tran))
                        { cmd.Parameters.Add("@uid", SqlDbType.Int).Value = idUsuario; cmd.ExecuteNonQuery(); }

                        tran.Commit();
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }


        public static void EditarNivelUsuario(int userId, int nuevoNivelId)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string query = "UPDATE usuarios SET id_nivel = @nuevoNivelId WHERE id_usuario = @userId";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nuevoNivelId", nuevoNivelId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static void ModificarUsuario(usuarioModif usuario, int id)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"UPDATE usuarios
                           SET telefono = @telefono, nombre = @nombre, apellido = @apellido where id_usuario = @id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = usuario.telefono;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.nombre;
                    cmd.Parameters.Add("@apellido", SqlDbType.VarChar).Value = usuario.apellido;

                    cmd.ExecuteNonQuery();
                }
                return;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static (bool esValido, int userId) ValidateExistente( string correo, string contra)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string ls_sql = @"SELECT id_usuario, contraseña, eliminado FROM usuarios 
                   WHERE email = @email";
                SqlCommand cmd = new SqlCommand(ls_sql, cn);
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = correo;
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(reader.GetOrdinal("id_usuario"));
                        string contraDB = reader.GetString(reader.GetOrdinal("contraseña"));
                        string Ccontra = Hasher.encriptar(contra);
                        DateTime? Ddate = reader.IsDBNull(reader.GetOrdinal("eliminado")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("eliminado"));



                        if (Ddate == null)
                        {
                            return (true, userId);
                        }
                    }
                }

                return (false, -1);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
        }


    }
}
