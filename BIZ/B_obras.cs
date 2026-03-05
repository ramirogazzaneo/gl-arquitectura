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
    public class B_obras
    {

        private static int idParaObra()
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COALESCE(Max(id_obra), 100)+1 from obras", conn);
                int id = int.Parse(cmd.ExecuteScalar().ToString());
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
        }


        public static void carga_obra(estructuras.ObrasCarga obras, int idD)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string sql = @"INSERT INTO obras 
                           (id_obra , id_usuario, nombre, id_direccion , m2_totales, id_estado) 
                           VALUES 
                           (@id_obra , @id_usuario, @nombre, @id_direccion , @m2_totales, @id_estado)";

                int idO = idParaObra();
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {

                    cmd.Parameters.Add("@id_obra", SqlDbType.Int).Value = idO;
                    cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = obras.id_usuario;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = obras.nombreO;
                    cmd.Parameters.Add("@id_direccion", SqlDbType.VarChar).Value = idD;
                    cmd.Parameters.Add("@m2_totales", SqlDbType.VarChar).Value = obras.metrosT;
                    cmd.Parameters.Add("@id_estado", SqlDbType.VarChar).Value = 3; //en espera por default

                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static List<Obra> traerObra(int clienteId)
        {
            SqlConnection cn = A_conexion.conexionDB();
            List<Obra> obra = new List<Obra>();
            try
            {
                string query = @"SELECT	O.id_obra, O.nombre, D.barrio, D.calle, D.localidad,D.provincia, O.m2_totales, E.descripcion, O.eliminado
                                FROM obras O
                                LEFT JOIN estado E ON E.id_estado = O.id_estado
                                LEFT JOIN direccion D ON D.id_direccion = O.id_direccion
                                 where O.eliminado IS NULL and O.id_usuario = @clienteID
                                 ORDER BY O.nombre
                                ";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {

                    cmd.Parameters.Add("@clienteID", SqlDbType.Int).Value = clienteId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Obra obraS = new Obra
                            {
                                id = reader.GetInt32(0),
                                nombre = reader.GetString(1),
                                barrio = reader.GetString(2),
                                calle = reader.GetString(3),
                                localidad = reader.GetString(4),
                                provincia = reader.GetString(5),
                                metrosT = reader.GetString(6),
                                descripcion = reader.GetString(7),
                            };
                            obra.Add(obraS);
                        }
                    }
                }
                return obra;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static List<Obras> traerObras()
        {
            SqlConnection cn = A_conexion.conexionDB();
            List<Obras> obras = new List<Obras>();
            try
            {
                string query = @"SELECT	O.id_obra, U.nombre,U.apellido,  O.nombre, D.barrio, D.calle, D.localidad,D.provincia, O.m2_totales, E.id_estado
                                FROM obras O
                                LEFT JOIN estado E ON E.id_estado = O.id_estado
                                LEFT JOIN direccion D ON D.id_direccion = O.id_direccion
								LEFT JOIN usuarios U ON U.id_usuario = O.id_usuario
                                 where O.eliminado IS NULL
                                 ORDER BY O.id_obra
                                ";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Obras obraas = new Obras
                            {
                                id = reader.GetInt32(0),
                                nombre = reader.GetString(1),
                                apellido = reader.GetString(2),
                                nombreO = reader.GetString(3),
                                barrio = reader.GetString(4),
                                calle = reader.GetString(5),
                                localidad = reader.GetString(6),
                                provincia = reader.GetString(7),
                                metrosT = reader.GetString(8),
                                id_estado = reader.GetInt32(9),
                            };
                            obras.Add(obraas);
                        }
                    }
                }
                return obras;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static List<Obras> traerObrass(int id)
        {
            SqlConnection cn = A_conexion.conexionDB();
            List<Obras> obras = new List<Obras>();
            try
            {
                string query = @"SELECT	O.id_obra, U.nombre,U.apellido,  O.nombre, D.barrio, D.calle, D.localidad,D.provincia, O.m2_totales, E.id_estado
                                FROM obras O
                                LEFT JOIN estado E ON E.id_estado = O.id_estado
                                LEFT JOIN direccion D ON D.id_direccion = O.id_direccion
								LEFT JOIN usuarios U ON U.id_usuario = O.id_usuario
                                 where O.eliminado IS NULL and E.id_estado = @id 
                                 ORDER BY O.id_obra
                                ";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Obras obraas = new Obras
                            {
                                id = reader.GetInt32(0),
                                nombre = reader.GetString(1),
                                apellido = reader.GetString(2),
                                nombreO = reader.GetString(3),
                                barrio = reader.GetString(4),
                                calle = reader.GetString(5),
                                localidad = reader.GetString(6),
                                provincia = reader.GetString(7),
                                metrosT = reader.GetString(8),
                                id_estado = reader.GetInt32(9),
                            };
                            obras.Add(obraas);
                        }
                    }
                }
                return obras;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static List<(int id_estado, string descripcion)> traerEstado()
        {
            SqlConnection cn = A_conexion.conexionDB();
            List<(int id_estado, string descripcion)> estado = new List<(int id_estado, string descripcion)>();
            try
            {
                string query = "SELECT id_estado, descripcion FROM estado ORDER BY id_estado";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        estado.Add((
                            reader.GetInt32(0),
                            reader.GetString(1)
                        ));
                    }
                }
                return estado;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cn.Close(); }
        }

        public static void EditarEstadoObra(int userId, int nuevoNivelId)
        {
            SqlConnection cn = A_conexion.conexionDB();
            try
            {
                string query = "UPDATE obras SET id_estado = @nuevoNivelId WHERE id_obra = @userId";
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

        public static void eliminarObra(int codigo)
        {
            using (SqlConnection cn = A_conexion.conexionDB())
            using (SqlTransaction tran = cn.BeginTransaction())
            {
                try
                {
                   
                    using (var cmd = new SqlCommand(@"
                UPDATE p SET eliminado = GETDATE()
                FROM presupuestos p
                INNER JOIN rubroR rr ON rr.id_rubroR = p.id_rubroR
                WHERE rr.id_obra = @id AND p.eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                   
                    using (var cmd = new SqlCommand(@"
                UPDATE rubroR SET eliminado = GETDATE()
                WHERE id_obra = @id AND eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                  
                    using (var cmd = new SqlCommand(@"
                UPDATE m SET eliminado = GETDATE()
                FROM materiales m
                INNER JOIN rubro r ON r.id_rubro = m.id_rubro
                WHERE r.id_obra = @id AND m.eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                    
                    using (var cmd = new SqlCommand(@"
                UPDATE rubro SET eliminado = GETDATE()
                WHERE id_obra = @id AND eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                   
                    using (var cmd = new SqlCommand(@"
                UPDATE t SET eliminado = GETDATE()
                FROM tarea t
                INNER JOIN etapas e ON e.id_etapa = t.id_etapa
                WHERE e.id_obra = @id AND t.eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                 
                    using (var cmd = new SqlCommand(@"
                UPDATE etapas SET eliminado = GETDATE()
                WHERE id_obra = @id AND eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                  
                    using (var cmd = new SqlCommand(@"
                UPDATE pagos SET eliminado = GETDATE()
                WHERE id_obra = @id AND eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                   
                    
                    using (var cmd = new SqlCommand(@"
                UPDATE d SET d.eliminado = GETDATE()
                FROM documentos_tecnicos d
                INNER JOIN carpetas_docs c ON c.id_carpeta = d.id_carpeta
                WHERE c.id_obra = @id
                  AND d.eliminado IS NULL
                  AND c.eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                    
                    using (var cmd = new SqlCommand(@"
                UPDATE carpetas_docs SET eliminado = GETDATE()
                WHERE id_obra = @id AND eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }
                

                
                    using (var cmd = new SqlCommand(@"
                UPDATE d SET eliminado = GETDATE()
                FROM direccion d
                INNER JOIN obras o ON o.id_direccion = d.id_direccion
                WHERE o.id_obra = @id
                  AND d.eliminado IS NULL
                  AND NOT EXISTS (
                        SELECT 1
                        FROM obras o2
                        WHERE o2.id_direccion = d.id_direccion
                          AND o2.eliminado IS NULL
                          AND o2.id_obra <> @id
                  );", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                  
                    using (var cmd = new SqlCommand(@"
                UPDATE obras SET eliminado = GETDATE()
                WHERE id_obra = @id AND eliminado IS NULL;", cn, tran))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = codigo;
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

    }
}

