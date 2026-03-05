using BIZ;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GL___C2G
{
    public partial class AF_TAREAS_ETAPAS_USR : System.Web.UI.Page
    {
        //==========================
        //pageload
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["id"] == null)
                {
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
                    ddlObras.Enabled = false;
                    return;
                }

                CargarObrasDelUsuario();   
                BindEtapas();              
            }
        }
        #endregion
        //==========================
        //variables,ids
        //==========================
        #region

        private int IdObraSeleccionada => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0;

        #endregion
        //==========================
        //binds
        //==========================
        #region

        private void CargarObrasDelUsuario()
        {

            int IdUsuarioActual = Convert.ToInt32(Session["id"]);
            DataTable dt = BIZ.C_EyT.obras_listar_comboID(IdUsuarioActual);

            ddlObras.DataSource = dt;
            ddlObras.DataTextField = "nombre";
            ddlObras.DataValueField = "id_obra";
            ddlObras.DataBind();
            ddlObras.Items.Insert(0, new ListItem("-- Seleccione obra --", "0"));

            ViewState["ObrasUsuario"] = dt;
        }
        private void BindEtapas()
        {
            lblMsg.Visible = false;
            rptEtapas.DataSource = null;
            rptEtapas.DataBind();

            if (IdObraSeleccionada <= 0)
            {
                lblMsg.Text = "Seleccioná una obra para ver sus etapas y tareas.";
                lblMsg.Visible = true;
                return;
            }


            if (!UsuarioTieneAccesoAObra(IdObraSeleccionada))
            {
                lblMsg.Text = "No tenés acceso a esta obra.";
                lblMsg.Visible = true;
                return;
            }

            var etapas = BIZ.C_EyT.etapas_listar_por_obra(IdObraSeleccionada);
            if (etapas == null || etapas.Rows.Count == 0)
            {
                lblMsg.Text = "Esta obra no tiene etapas cargadas.";
                lblMsg.Visible = true;
            }

            rptEtapas.DataSource = etapas;
            rptEtapas.DataBind();
        }

        #endregion
        //==========================
        //seguridad
        //==========================
        #region

        private bool UsuarioTieneAccesoAObra(int idObra)
        {
            var dt = ViewState["ObrasUsuario"] as DataTable;
            if (dt == null) return false;
            return dt.AsEnumerable().Any(r => r.Field<int>("id_obra") == idObra);
        }

        #endregion
        //==========================
        //dropdown
        //==========================
        #region

        protected void ddlObras_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (IdObraSeleccionada > 0 && !UsuarioTieneAccesoAObra(IdObraSeleccionada))
                ddlObras.SelectedIndex = 0;

            BindEtapas();
        }

        #endregion
        //==========================
        //tablas
        //==========================
        #region

        protected void rptEtapas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var drv = (DataRowView)e.Item.DataItem;
            int idEtapa = Convert.ToInt32(drv["id_etapa"]);

            var gv = (GridView)e.Item.FindControl("gvTareas");
            gv.DataSource = BIZ.C_EyT.tareas_listar_por_etapa(idEtapa);
            gv.DataBind();
        }

        #endregion

    }
}