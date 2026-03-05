using BIZ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GL___C2G.ULTRA.PAGINAS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //==========================
        //pageload
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["nivel"] == null)
                    Response.Redirect("/ULTRA/INICIO/REGISTER");

                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "PAG_PRINCIPAL_DES"))
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");

               
                CargarDashboard();
            }
        }
        #endregion
        //==========================
        //variables, ids
        //==========================
        #region
        private int id_nivel => int.TryParse(Session["nivel"].ToString(), out var v) ? v : 0;

        #endregion
        //==========================
        //binds y graficos
        //==========================
        #region

        protected void CargarDashboard()
        {

            var dtKpi = D_Reportes.reportes_kpis_obrasS();
            if (dtKpi.Rows.Count > 0)
            {
                var r = dtKpi.Rows[0];
                kpi_activas.InnerText = Convert.ToInt32(r["obras_activas"]).ToString();
                kpi_proceso.InnerText = Convert.ToInt32(r["obras_finalizadas"]).ToString();
                kpi_espera.InnerText = Convert.ToInt32(r["obras_en_espera"]).ToString();
            }

            var dtEstados = D_Reportes.reportes_obras_por_estadoO();


            // Serializo para Chart.js
            var labelsEstados = new List<string>();
            var dataEstados = new List<int>();
            foreach (DataRow row in dtEstados.Rows)
            {
                labelsEstados.Add(row["estado"].ToString());
                dataEstados.Add(Convert.ToInt32(row["cantidad"]));
            }

            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(new
            {
                estados = new { labels = labelsEstados, data = dataEstados },
            });

            //ACA CONSTRUYO LA PLANTILLA EN JS TOMANDO LOS DATOS ANTERIORMENTE ESTANDARIZADOS
            var js = @"
window.__DASH__ = __JSON__;
(function(d){
  const D = window.__DASH__;
  const $ = id => d.getElementById(id);

  // helper por si vienen arrays vacíos
  const safe = arr => (Array.isArray(arr) && arr.length ? arr : [0]);

  // ----- DONA: Obras por estado -----
  new Chart($('chartEstados'), {
    type: 'doughnut',
    data: {
      labels: D.estados.labels,
      datasets: [{ data: safe(D.estados.data), borderWidth: 2 }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      cutout: '58%',
      layout: { padding: 4 },
      plugins: {
        legend: { position: 'bottom', labels: { boxWidth: 14, usePointStyle: true } },
        tooltip: { callbacks: { label: (ctx) => `${ctx.label}: ${ctx.formattedValue}` } }
      }
    }
  });
})(document);";

            //INYECTO EL JSON AL JS CPN LOS DATOS
            js = js.Replace("__JSON__", json);

            //EJECUTAMOS SCRIPT PARA QUE FUNCIONE
            ScriptManager.RegisterStartupScript(this, GetType(), "dashInit", js, true);
        }
        #endregion

        //==========================
        //dropdown
        //==========================
        #region

        protected void ddlObraFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDashboard();
        }
        #endregion

        //==========================
        //acciones
        //==========================
        #region
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarDashboard();
        }
       
        #endregion
    }
}
