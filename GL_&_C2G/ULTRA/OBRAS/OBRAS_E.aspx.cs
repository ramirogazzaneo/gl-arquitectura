using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G.ULTRA.OBRAS
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
                {
                    Response.Redirect("/ULTRA/INICIO/REGISTER");
                    return;
                }
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "OBRAS_ADM"))
                {

                    Response.Redirect("/ULTRA/PAGINAS/PAGINA_PRINCIPAL");
                    return;
                }


                IdEstado = 0;
                if (int.TryParse(Request.QueryString[0], out var est))
                    IdEstado = est;


                Estados = BIZ.B_obras.traerEstado();
                BindGrid();
            }
        }
        #endregion

        //==========================
        //variables 
        //==========================
        #region
        private int IdEstado
        {
            get => ViewState["__ID_ESTADO__"] is int v ? v : 0;
            set => ViewState["__ID_ESTADO__"] = value;
        }

        #endregion

        //==========================
        //binds
        //==========================
        #region
        private void BindGrid()
        {
            List<Obras> obras = BIZ.B_obras.traerObrass(IdEstado) ?? new List<Obras>();
            GridView1.DataSource = obras;
            GridView1.DataBind();
        }

        private List<(int id_estado, string descripcion)> Estados
        {
            get
            {
                if (ViewState["__ESTADOS__"] is List<(int id_estado, string descripcion)> list)
                    return list;

                var est = BIZ.B_obras.traerEstado() ?? new List<(int, string)>();
                ViewState["__ESTADOS__"] = est;
                return est;
            }
            set { ViewState["__ESTADOS__"] = value; }
        }

        protected string GetNivelDescripcion(int estadoId)
        {
            var est = Estados.FirstOrDefault(x => x.id_estado == estadoId);

            return string.IsNullOrWhiteSpace(est.descripcion) ? "No definido" : est.descripcion;
        }

        private void LlenoDropEstados(DropDownList ddl, int estadoActual)
        {
            ddl.Items.Clear();
            foreach (var (id_estado, descripcion) in Estados)
            {
                ddl.Items.Add(new ListItem(descripcion, id_estado.ToString()));
            }
            var item = ddl.Items.FindByValue(estadoActual.ToString());
            if (item != null) item.Selected = true;
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

            DropDownList ddEstado = (DropDownList)GridView1.Rows[e.NewEditIndex].FindControl("ddEstado");
            List<(int id_estado, string descripcion)> estados = BIZ.B_obras.traerEstado();
            ddEstado.Items.Clear();

            foreach (var estado in estados)
            {
                ddEstado.Items.Add(new ListItem(estado.descripcion, estado.id_estado.ToString()));
            }

            string currentNivel = DataBinder.Eval(GridView1.DataSource as List<Obras>, "[" + e.NewEditIndex + "].id_estado").ToString();
            if (!string.IsNullOrEmpty(currentNivel))
            {
                ddEstado.SelectedValue = currentNivel;
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int obraId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            var row = GridView1.Rows[e.RowIndex];
            var ddl = (DropDownList)row.FindControl("ddEstado");
            if (ddl == null || !int.TryParse(ddl.SelectedValue, out int nuevoEstado))
            {

                GridView1.EditIndex = -1;
                BindGrid();
                return;
            }

            BIZ.B_obras.EditarEstadoObra(obraId, nuevoEstado);

            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int obraId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            BIZ.B_obras.eliminarObra(obraId);
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int idEstado = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id_estado"));


                HyperLink lnk = (HyperLink)e.Row.FindControl("lnkTareas");

                if (lnk != null)
                {

                    if (idEstado == 2 || idEstado == 3)
                    {
                        lnk.Visible = false;
                    }
                }
            }
        }
    }
    #endregion
}
