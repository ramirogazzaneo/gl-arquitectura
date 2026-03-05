using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GL___C2G.ULTRA.DOCS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //==========================
        //page load
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["id"] == null)
                {
                    Response.Redirect("~/ULTRA/INICIO/LOG_IN");
                    return;
                }
                ClearMsg();
                BindObras();
                BindCarpetas();
                BindGrid();
            }
        }
        #endregion
        //==========================
        //ids
        //==========================
        #region
        private int id_usr => Convert.ToInt32(Session["id"].ToString());
        private int id_obra => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0;
        private int id_carpeta => int.TryParse(ddlCarpetas.SelectedValue, out var v) ? v : 0;
        #endregion
        //==========================
        //binds
        //==========================
        #region
        void BindObras()
        {
            var dt = BIZ.D_Presupuestos.obras_listar_comboID(id_usr);
            ddlObras.DataSource = dt;
            ddlObras.DataTextField = "nombre";
            ddlObras.DataValueField = "id_obra";
            ddlObras.DataBind();
            ddlObras.Items.Insert(0, new ListItem("-- Seleccione obra --", "0"));
        }

        void BindCarpetas()
        {
            ddlCarpetas.Items.Clear();

            if (id_obra <= 0)
            {
                ddlCarpetas.Items.Insert(0, new ListItem("-- Todas --", "0"));
                return;
            }

            var dt = BIZ.docs_carpetas.listar_por_obra(id_obra);
            ddlCarpetas.DataSource = dt;
            ddlCarpetas.DataTextField = "descripcion";
            ddlCarpetas.DataValueField = "id_carpeta";
            ddlCarpetas.DataBind();
            ddlCarpetas.Items.Insert(0, new ListItem("-- Todas --", "0"));
        }

        void BindGrid()
        {
            if (id_obra <= 0)
            {
                gvDocs.DataSource = null;
                gvDocs.DataBind();
                ShowMsg("Seleccioná una obra para ver sus documentos.", false);
                return;
            }

            int? obra = id_obra > 0 ? id_obra : (int?)null;
            int? carp = id_carpeta > 0 ? id_carpeta : (int?)null;

            var docs = BIZ.docs.listar_docs(obra, carp); 
            gvDocs.DataSource = docs;
            gvDocs.DataBind();

            if (docs == null || docs.Rows.Count == 0)
                ShowMsg("No hay documentos para los filtros seleccionados.", false);
            else
                ClearMsg();
        }
        #endregion
        //==========================
        //panel msj
        //==========================
        #region
        private void ShowMsg(string text, bool ok = true)
        {
            pnlMsg.Visible = true;
            pnlMsg.CssClass = "alert " + (ok ? "alert-success" : "alert-danger");
            litMsg.Text = Server.HtmlEncode(text);
        }
        private void ClearMsg()
        {
            pnlMsg.Visible = false;
            litMsg.Text = "";
        }
        #endregion
        //==========================
        //DropDown
        //==========================
        #region
        protected void ddlObras_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearMsg();
            BindCarpetas();
            BindGrid();
        }

        protected void ddlCarpetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearMsg();
            BindGrid();
        }
        #endregion
    }
}
