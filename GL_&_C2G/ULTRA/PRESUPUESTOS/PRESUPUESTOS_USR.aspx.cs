using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GL___C2G
{
    public partial class WebForm10 : System.Web.UI.Page
    {
        //==========================
        //pageload
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (id_usuarioA <= 0)
                {
                    ddlObras.Enabled = false;
                    lblMsg.Text = "Iniciá sesión para ver tus obras.";
                    lblMsg.Visible = true;
                    return;
                }

                CargarObrasDelUsuario();

                BindRubros();
            }
        }
        #endregion
        //==========================
        //variables,ids
        //==========================
        #region
        private int id_usuarioA => (Session["id"] != null && int.TryParse(Session["id"].ToString(), out var id)) ? id : 0;

        private int id_obraA => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0;
        #endregion
        //==========================
        //binds
        //==========================
        #region
        private void CargarObrasDelUsuario()
        {

            DataTable dt = BIZ.D_Presupuestos.obras_listar_comboID(id_usuarioA);



            ddlObras.DataSource = dt;
            ddlObras.DataTextField = "nombre";
            ddlObras.DataValueField = "id_obra";
            ddlObras.DataBind();
            ddlObras.Items.Insert(0, new ListItem("-- Seleccione obra --", "0"));


            ViewState["ObrasUsuario"] = dt;
        }

        private void BindRubros()
        {
            lblMsg.Visible = false;
            rptRubros.DataSource = null;
            rptRubros.DataBind();

            if (id_obraA <= 0)
            {
                lblMsg.Text = "Seleccioná una obra para ver sus rubros y materiales.";
                lblMsg.Visible = true;
                return;
            }

            if (!UsuarioTieneAccesoAObra(id_obraA))
            {
                lblMsg.Text = "No tenés acceso a esta obra.";
                lblMsg.Visible = true;
                return;
            }

            var rubros = BIZ.D_Presupuestos.rubrosR_listar_por_obra(id_obraA);
            if (rubros == null || rubros.Rows.Count == 0)
            {
                lblMsg.Text = "Esta obra no tiene rubros cargados.";
                lblMsg.Visible = true;
            }

            rptRubros.DataSource = rubros;
            rptRubros.DataBind();
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
        //seguridad
        //==========================
        #region
        protected void ddlObras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (id_obraA > 0 && !UsuarioTieneAccesoAObra(id_obraA))
                ddlObras.SelectedIndex = 0;

            BindRubros();
        }
        #endregion
        //==========================
        //tablas
        //==========================
        #region
        protected void rptRubros_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var drv = (DataRowView)e.Item.DataItem;
            int idRubro = Convert.ToInt32(drv["id_rubroR"]);

            var gv = (GridView)e.Item.FindControl("gvMateriales");
            gv.DataSource = BIZ.D_Presupuestos.presupuestos_listar_por_rubro(idRubro);
            gv.DataBind();
        }

        #endregion
    }
}
