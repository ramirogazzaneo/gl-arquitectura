using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GL___C2G
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        //==========================
        //pageload
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (idUsuario <= 0)
                {
                    Response.Redirect("/ULTRA/INICIO/LOG_IN");
                }

                BindObras();

                BindRubros();
            }
        }
        #endregion
        //==========================
        //variables, ids
        //==========================
        #region
        private int idUsuario => Convert.ToInt32(Session["id"]);

        private int idObra => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0;

        #endregion
        //==========================
        //binds
        //==========================
        #region
        private void BindObras()
        {
            
            DataTable dt = BIZ.D_Materiales.obras_listar_comboID(idUsuario);

          

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

            if (idObra <= 0)
            {
                lblMsg.Text = "Seleccioná una obra para ver sus rubros y materiales.";
                lblMsg.Visible = true;
                return;
            }

            if (!UsuarioTieneAccesoAObra(idObra))
            {
                lblMsg.Text = "No tenés acceso a esta obra.";
                lblMsg.Visible = true;
                return;
            }

            var rubros = BIZ.D_Materiales.rubros_listar_por_obra(idObra);
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
            //como tengo gaurdada la tabla, pasoa  filas las va recorriendo y busca que id = id
        }

        #endregion
        //==========================
        //dropdown
        //==========================
        #region

        protected void ddlObras_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            if (idObra > 0 && !UsuarioTieneAccesoAObra(idObra))
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
            int idRubro = Convert.ToInt32(drv["id_rubro"]);

            var gv = (GridView)e.Item.FindControl("gvMateriales");
           
            gv.DataSource = BIZ.D_Materiales.materiales_listar_por_rubro(idRubro);
            gv.DataBind();
        }

        #endregion
    }
}
