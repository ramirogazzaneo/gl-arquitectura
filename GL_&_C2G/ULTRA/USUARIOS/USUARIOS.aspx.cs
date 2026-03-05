using BIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class WebForm4 : System.Web.UI.Page
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
                {
                    Response.Redirect("/ULTRA/INICIO/REGISTER");
                }
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "USUARIOS"))
                {
                    Response.Redirect("/ULTRA/USUARIOS/USUARIOS");
                }
                BindGrid();
            }
        }
        #endregion
        //==========================
        //binds
        //==========================
        #region
        private void BindGrid()
        {
            List<usuario> usuarios = BIZ.A_Userr.traerUsuarios();
            GridView1.DataSource = usuarios;
            GridView1.DataBind();
        }
        #endregion
        //==========================
        //tablas
        //==========================
        #region
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();

            DropDownList ddlNivel = (DropDownList)GridView1.Rows[e.NewEditIndex].FindControl("ddlNivel");
            List<(int id_nivel, string descripcion)> niveles = BIZ.A_Userr.traerNiveles();
            ddlNivel.Items.Clear();

            foreach (var nivel in niveles)
            {
                ddlNivel.Items.Add(new ListItem(nivel.descripcion, nivel.id_nivel.ToString()));
            }

            string currentNivel = DataBinder.Eval(GridView1.DataSource as List<usuario>, "[" + e.NewEditIndex + "].nivel").ToString();
            if (!string.IsNullOrEmpty(currentNivel))
            {
                ddlNivel.SelectedValue = currentNivel;
            }

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            DropDownList ddlNivel = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlNivel");
            int newNivel = Convert.ToInt32(ddlNivel.SelectedValue);
            BIZ.A_Userr.EditarNivelUsuario(userId, newNivel);
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected string GetNivelDescripcion(int nivelId)
        {
            var niveles = BIZ.A_Userr.traerNiveles();
            var (id_nivel, descripcion) = niveles.FirstOrDefault(n => n.id_nivel == nivelId);
            return descripcion ?? "No definido";
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int codigo = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            A_Userr.eliminarUsuario(codigo);
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AgregarObra")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"/ULTRA/OBRAS/DIRECCION.aspx?usuarioId={usuarioId}");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int idTipo = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "nivel"));


                HyperLink lnk = (HyperLink)e.Row.FindControl("lnkAgregarObra");

                if (lnk != null)
                {

                    if (idTipo == 2 || idTipo == 3)
                    {
                        lnk.Visible = false;
                    }
                }
            }
        }

        #endregion

    }
}