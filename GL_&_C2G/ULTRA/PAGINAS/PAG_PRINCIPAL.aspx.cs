using BIZ;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GL___C2G
{
    public partial class Contact : Page
    {
        //==========================
        //pageload
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["nivel"] == null)
            {
                Response.Redirect("/ULTRA/INICIO/REGISTER");
                return;
            }


            if (!IsPostBack)
                CargarDashboard();
            BindPagos();
        }
        #endregion
        //==========================
        //binds
        //==========================
        #region

        private void BindPagos()
        {
            int idCliente = Convert.ToInt32(Session["id"]?.ToString() ?? "0");
            var dt = D_pag.pagos_por_cliente(idCliente);
            gvPagos.DataSource = dt;
            gvPagos.DataBind();
        }

        protected void CargarDashboard()
        {
            
            int idCliente = Convert.ToInt32(Session["id"]?.ToString() ?? "0");

            var dtKpi = D_pag.reportes_kpis_obras(idCliente);
            if (dtKpi.Rows.Count > 0)
            {
                var r = dtKpi.Rows[0];

                int act = r["obras_activas"] == DBNull.Value ? 0 : Convert.ToInt32(r["obras_activas"]);
                int fin = r["obras_finalizadas"] == DBNull.Value ? 0 : Convert.ToInt32(r["obras_finalizadas"]);
                int espera = r["obras_en_espera"] == DBNull.Value ? 0 : Convert.ToInt32(r["obras_en_espera"]);

                kpi_activas.InnerText = act.ToString();
                kpi_proceso.InnerText = fin.ToString();
                kpi_espera.InnerText = espera.ToString();
            }

            var dtMontos = D_pag.reportes_montos_resumen(idCliente);
            if (dtMontos.Rows.Count > 0)
            {
                var r = dtMontos.Rows[0];
                decimal deudaTotal = r["total_deuda"] == DBNull.Value ? 0 : Convert.ToDecimal(r["total_deuda"]);
                kpi_deuda.InnerText = deudaTotal.ToString("N2");
            }
        }

        #endregion
        //==========================
        //tablas
        //==========================
        #region

        protected void gvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var dr = (System.Data.DataRowView)e.Row.DataItem;

            int estado = dr["estado_pago"] == DBNull.Value ? 0 : Convert.ToInt32(dr["estado_pago"]);
            bool vencido = dr["vencido"] != DBNull.Value && Convert.ToInt32(dr["vencido"]) == 1;

           
            var lit = (Literal)e.Row.FindControl("litEstado");
            if (estado == 1)
            {
                lit.Text = "<span class='badge bg-success'>Pagado</span>";
                e.Row.CssClass += " table-success";
            }
            else if (vencido)
            {
                lit.Text = "<span class='badge bg-danger'>Vencido</span>";
                e.Row.CssClass += " table-danger"; 
            }
            else
            {
                lit.Text = "<span class='badge bg-warning text-dark'>Pendiente</span>";
                e.Row.CssClass += " table-warning";
            }
        }

        protected void btnObrasActivas_Click(object sender, EventArgs e) 
        {
            Response.Redirect("/ULTRA/OBRAS/OBRAS");
        }
     
        #endregion
    }
}