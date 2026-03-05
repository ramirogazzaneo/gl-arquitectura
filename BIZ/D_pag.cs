using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class D_pag
    {
        
        public static DataTable reportes_kpis_obras(int idCliente)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT
  ISNULL(SUM(CASE WHEN o.id_estado = 1 THEN 1 ELSE 0 END),0) AS obras_activas,
  ISNULL(SUM(CASE WHEN o.id_estado = 2 THEN 1 ELSE 0 END),0) AS obras_finalizadas,
  ISNULL(SUM(CASE WHEN o.id_estado = 3 THEN 1 ELSE 0 END),0) AS obras_en_espera
FROM dbo.obras o
WHERE o.eliminado IS NULL
  AND o.id_usuario = @cliente;", cn))
            {
                cmd.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

       
        public static DataTable reportes_montos_resumen(int idCliente)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT
  ISNULL((
    SELECT SUM(p.monto)
    FROM dbo.pagos p
    WHERE p.eliminado IS NULL
      AND p.estado_pago = 0         
      AND p.id_obra IN (SELECT o.id_obra FROM dbo.obras o WHERE o.eliminado IS NULL AND o.id_usuario = @cliente)
  ),0) AS total_deuda,

  ISNULL((
    SELECT SUM(p.monto)
    FROM dbo.pagos p
    WHERE p.eliminado IS NULL
      AND p.estado_pago = 1         
      AND p.id_obra IN (SELECT o.id_obra FROM dbo.obras o WHERE o.eliminado IS NULL AND o.id_usuario = @cliente)
  ),0) AS total_cobrado_hist;", cn))
            {
                cmd.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }


        public static DataTable pagos_por_cliente(int idCliente)
        {
            using (var cn = A_conexion.conexionDB())
            using (var cmd = new SqlCommand(@"
SELECT
    p.id_pago,
    p.id_obra,
    o.nombre            AS obra,
    p.descripcion,
    p.monto,
    p.fecha_ven,
    p.estado_pago,                  
    CASE
        WHEN p.estado_pago = 0
         AND p.fecha_ven IS NOT NULL
         AND CAST(p.fecha_ven AS date) < CAST(GETDATE() AS date)
        THEN 1 ELSE 0
    END AS vencido
FROM dbo.pagos p
JOIN dbo.obras o ON o.id_obra = p.id_obra
WHERE p.eliminado IS NULL
  AND o.eliminado IS NULL
  AND o.id_usuario = @cliente
ORDER BY
    CASE WHEN p.estado_pago = 0 THEN 0 ELSE 1 END,   
    p.fecha_ven ASC;", cn))
            {
                cmd.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}