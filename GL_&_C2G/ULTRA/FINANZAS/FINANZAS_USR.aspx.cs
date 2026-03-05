using BIZ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GL___C2G
{
    public partial class WebForm11 : System.Web.UI.Page
    {
        //==========================
        //pageload
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
            {
                
                if (Session["id"] == null)
                {
                    Response.Redirect("~/ULTRA/INICIO/LOG_IN");
                    return;
                }

                if (!IsPostBack)
                {
                    BindObras();
                    BindGrid();
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
                var dt = D_Finanzas.obras_listar_por_usuario(idUsuario);
                ddlObras.DataSource = dt;
                ddlObras.DataTextField = "nombre";
                ddlObras.DataValueField = "id_obra";
                ddlObras.DataBind();
                ddlObras.Items.Insert(0, new ListItem("Todas mis obras", "0"));
            }

        private void BindGrid()
        {
            var dt = D_Finanzas.pagos_listar_usuario(idUsuario, idObra, chkSoloPend.Checked);
            gvPagos.DataSource = dt;
            gvPagos.DataBind();



            //esto me permite establecer el filtro de pendiente, y sumar aquello que esta pendiente===========
            decimal total = 0m;
            foreach (DataRow r in dt.Rows)
                if (!Convert.ToBoolean(r["estado_pago"]))
                    total += Convert.ToDecimal(r["monto"]);
            litTotal.Text = total.ToString("");
        }
        #endregion
        //==========================
        //panel msj
        //==========================
        #region

        protected void Filtro_Changed(object sender, EventArgs e)
            {
                ClearMsg();
                BindGrid();
            }

            private void ShowMsg(string text, bool ok = true)
            {
            
                pnlMsg.Visible = true;
                pnlMsg.CssClass = "alert " + (ok ? "alert-success" : "alert-danger");
                litMsg.Text = Server.HtmlEncode(text);
            }
            private void ClearMsg() 
            {
            pnlMsg.Visible = false; litMsg.Text = "";
            }

        #endregion
    }
}
