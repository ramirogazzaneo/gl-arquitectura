using System;
using System.Data;
using System.Data.SqlClient;

namespace BIZ
{
    public class D_Reportes
    {
        public static DataTable reportes_kpis_obras(int? idObra)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT
  SUM(CASE WHEN o.id_estado = 1 THEN 1 ELSE 0 END) AS obras_activas,
  SUM(CASE WHEN o.id_estado = 2 THEN 1 ELSE 0 END) AS obras_finalizadas,
  SUM(CASE WHEN o.id_estado = 3 THEN 1 ELSE 0 END) AS obras_en_espera
FROM dbo.obras o
WHERE o.eliminado IS NULL
  AND (@obra IS NULL OR o.id_obra = @obra);", cn))
            {
                cmd.Parameters.Add("@obra", SqlDbType.Int).Value = (object)idObra ?? DBNull.Value;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable reportes_kpis_obrasS()
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT
  SUM(CASE WHEN o.id_estado = 1 THEN 1 ELSE 0 END) AS obras_activas,
  SUM(CASE WHEN o.id_estado = 2 THEN 1 ELSE 0 END) AS obras_finalizadas,
  SUM(CASE WHEN o.id_estado = 3 THEN 1 ELSE 0 END) AS obras_en_espera
FROM dbo.obras o
WHERE o.eliminado IS NULL;", cn))
            {
           
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable reportes_obras_por_estado(int? idObra)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT e.descripcion AS estado, COUNT(*) AS cantidad
FROM dbo.obras o
JOIN dbo.estado e ON e.id_estado = o.id_estado
WHERE o.eliminado IS NULL
  AND (@obra IS NULL OR o.id_obra = @obra)
GROUP BY e.descripcion
ORDER BY cantidad DESC;", cn))
            {
                cmd.Parameters.Add("@obra", SqlDbType.Int).Value = (object)idObra ?? DBNull.Value;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable reportes_obras_por_estadoO()
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT e.descripcion AS estado, COUNT(*) AS cantidad
FROM dbo.obras o
JOIN dbo.estado e ON e.id_estado = o.id_estado
WHERE o.eliminado IS NULL
GROUP BY e.descripcion
ORDER BY cantidad DESC;", cn))
            {
               
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable reportes_usuarios_deudores(int? idObra)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT u.id_usuario, u.nombre AS Usuario, SUM(p.monto) AS Deuda
FROM dbo.pagos p
JOIN dbo.usuarios u ON u.id_usuario = p.id_usuario
WHERE p.eliminado IS NULL
  AND p.estado_pago = 0
  AND (@obra IS NULL OR p.id_obra = @obra)
GROUP BY u.id_usuario, u.nombre
ORDER BY Deuda DESC;", cn))
            {
                cmd.Parameters.Add("@obra", SqlDbType.Int).Value = (object)idObra ?? DBNull.Value;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable reportes_montos_resumen(DateTime desde, DateTime hasta, int? idObra)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT
  (SELECT SUM(p.monto) FROM dbo.pagos p
    WHERE p.eliminado IS NULL AND p.estado_pago = 0
      AND (@obra IS NULL OR p.id_obra = @obra)) AS total_deuda,
  (SELECT SUM(p.monto) FROM dbo.pagos p
    WHERE p.eliminado IS NULL AND p.estado_pago = 1
      AND (@obra IS NULL OR p.id_obra = @obra)) AS total_cobrado_hist,
  (SELECT SUM(p.monto) FROM dbo.pagos p
    WHERE p.eliminado IS NULL AND p.estado_pago = 1
      AND p.fecha_ven BETWEEN @d AND @h
      AND (@obra IS NULL OR p.id_obra = @obra)) AS cobrado_rango,
  (SELECT SUM(p.monto) FROM dbo.pagos p
    WHERE p.eliminado IS NULL AND p.estado_pago = 0
      AND p.fecha_ven BETWEEN @d AND @h
      AND (@obra IS NULL OR p.id_obra = @obra)) AS esperado_rango;", cn))
            {
                cmd.Parameters.AddWithValue("@d", desde.Date);
                cmd.Parameters.AddWithValue("@h", hasta.Date);
                cmd.Parameters.Add("@obra", SqlDbType.Int).Value = (object)idObra ?? DBNull.Value;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable reportes_serie_diaria(DateTime desde, DateTime hasta, int? idObra)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
WITH dias AS (
  SELECT CAST(@d AS date) AS fecha
  UNION ALL
  SELECT DATEADD(DAY,1,fecha) FROM dias WHERE fecha < @h
)
SELECT 
  d.fecha,
  ISNULL((
    SELECT SUM(p.monto) FROM dbo.pagos p
    WHERE p.eliminado IS NULL AND p.estado_pago = 1
      AND CAST(p.fecha_ven AS date) = d.fecha
      AND (@obra IS NULL OR p.id_obra = @obra)
  ),0) AS cobrado_dia,
  ISNULL((
    SELECT SUM(p.monto) FROM dbo.pagos p
    WHERE p.eliminado IS NULL AND p.estado_pago = 0
      AND CAST(p.fecha_ven AS date) = d.fecha
      AND (@obra IS NULL OR p.id_obra = @obra)
  ),0) AS esperado_dia
FROM dias d
OPTION (MAXRECURSION 0);", cn))
            {
                cmd.Parameters.AddWithValue("@d", desde.Date);
                cmd.Parameters.AddWithValue("@h", hasta.Date);
                cmd.Parameters.Add("@obra", SqlDbType.Int).Value = (object)idObra ?? DBNull.Value;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}

