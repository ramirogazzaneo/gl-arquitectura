using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Antlr.Runtime.Misc;
using BIZ;

namespace GL___C2G
{
    public partial class WebForm3 : System.Web.UI.Page
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

                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "PAG_PRINCIPAL_ADM"))
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");

                txtDesde.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd");
                txtHasta.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 30).ToString("yyyy-MM-dd");

                CargarObrasFiltro();
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
        //filtros
        //==========================
        #region
        //FILTRO============================================================================================================
        private int? IdObraFiltro
        {
            get
            {
                if (int.TryParse(ddlObraFiltro.SelectedValue, out var id) && id > 0)
                    return id;
                return null;
            }
        }
        #endregion
        //==========================
        //binds y graficos
        //==========================
        #region
        private void CargarObrasFiltro()
        {
            var dt = BIZ.D_Presupuestos.obras_listar_combo();
            ddlObraFiltro.DataSource = dt;
            ddlObraFiltro.DataTextField = "nombre";
            ddlObraFiltro.DataValueField = "id_obra";
            ddlObraFiltro.DataBind();
            ddlObraFiltro.Items.Insert(0, new ListItem("— Todas las obras —", "0"));
        }

        protected void CargarDashboard()
        {
            DateTime desde = DateTime.TryParse(txtDesde.Text, out var d) ? d : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime hasta = DateTime.TryParse(txtHasta.Text, out var h) ? h : DateTime.Today;

            var dtKpi = D_Reportes.reportes_kpis_obras(IdObraFiltro);
            if (dtKpi.Rows.Count > 0)
            {
                var r = dtKpi.Rows[0];
                kpi_activas.InnerText = Convert.ToInt32(r["obras_activas"]).ToString();
                kpi_proceso.InnerText = Convert.ToInt32(r["obras_finalizadas"]).ToString();
                kpi_espera.InnerText = Convert.ToInt32(r["obras_en_espera"]).ToString();
            }

            var dtEstados = D_Reportes.reportes_obras_por_estado(IdObraFiltro);

            var dtMontos = D_Reportes.reportes_montos_resumen(desde, hasta, IdObraFiltro);
            decimal cobrado = 0, esperado = 0, deudaTotal = 0;
            if (dtMontos.Rows.Count > 0)
            {
                var r = dtMontos.Rows[0];
                deudaTotal = r["total_deuda"] == DBNull.Value ? 0 : Convert.ToDecimal(r["total_deuda"]);
                cobrado = r["cobrado_rango"] == DBNull.Value ? 0 : Convert.ToDecimal(r["cobrado_rango"]);
                esperado = r["esperado_rango"] == DBNull.Value ? 0 : Convert.ToDecimal(r["esperado_rango"]);
                kpi_deuda.InnerText = deudaTotal.ToString("C0", new CultureInfo("es-AR"));
            }

            var dtLinea = D_Reportes.reportes_serie_diaria(desde, hasta, IdObraFiltro);

            gvDeudores.DataSource = D_Reportes.reportes_usuarios_deudores(IdObraFiltro);
            gvDeudores.DataBind();

            // Serializo para Chart.js
            var labelsEstados = new List<string>();
            var dataEstados = new List<int>();
            foreach (DataRow row in dtEstados.Rows)
            {
                labelsEstados.Add(row["estado"].ToString());
                dataEstados.Add(Convert.ToInt32(row["cantidad"]));
            }

            var labelsLinea = new List<string>();
            var dataCobrado = new List<decimal>();
            var dataEsperado = new List<decimal>();
            foreach (DataRow row in dtLinea.Rows)
            {
                labelsLinea.Add(Convert.ToDateTime(row["fecha"]).ToString("dd/MM"));
                dataCobrado.Add(row["cobrado_dia"] == DBNull.Value ? 0 : Convert.ToDecimal(row["cobrado_dia"]));
                dataEsperado.Add(row["esperado_dia"] == DBNull.Value ? 0 : Convert.ToDecimal(row["esperado_dia"]));
            }

            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(new
            {
                estados = new { labels = labelsEstados, data = dataEstados },
                mes = new { cobrado = cobrado, esperado = esperado },
                linea = new { labels = labelsLinea, cobrado = dataCobrado, esperado = dataEsperado }
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

  // ----- BARRAS: Cobrado vs Esperado -----
  new Chart($('chartMes'), {
    type: 'bar',
    data: {
      labels: ['Mes actual'],
      datasets: [
        { label: 'Cobrado',  data: [D.mes.cobrado],  borderWidth: 1 },
        { label: 'Esperado', data: [D.mes.esperado], borderWidth: 1 }
      ]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      layout: { padding: 4 },
      scales: {
        y: {
          beginAtZero: true,
          ticks: { callback: (v) => new Intl.NumberFormat('es-AR').format(v) }
        },
        x: { grid: { display: false } }
      },
      plugins: { legend: { position: 'bottom' } }
    }
  });

  // ----- LÍNEA: Serie diaria -----
  new Chart($('chartLinea'), {
    type: 'line',
    data: {
      labels: D.linea.labels,
      datasets: [
        { label: 'Cobrado',  data: safe(D.linea.cobrado),  borderWidth: 2, pointRadius: 0, tension: .3, fill: true },
        { label: 'Esperado', data: safe(D.linea.esperado), borderWidth: 2, pointRadius: 0, tension: .3, fill: true }
      ]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      layout: { padding: 4 },
      interaction: { mode: 'index', intersect: false },
      scales: {
        y: { beginAtZero: true, ticks: { callback: (v) => new Intl.NumberFormat('es-AR').format(v) } },
        x: { grid: { display: false }, ticks: { maxRotation: 0, autoSkip: true } }
      },
      plugins: { legend: { position: 'bottom' } }
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