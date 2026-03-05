using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GL___C2G.Z_DOCUMENTOS
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
                if (Session["nivel"] == null) { Response.Redirect("/ULTRA/INICIO/REGISTER"); return; }
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "DOCS_ADM"))
                { Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL"); return; }

                HideAlert();
                BindObras();
                BindAcordeon();
            }
        }
        #endregion
        //==========================
        //ids
        //==========================
        #region
        private int id_obra => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0;
        #endregion
        //==========================
        //alerts
        //==========================
        #region
        void HideAlert()
        {
            pnlAlert.Visible = false;
            lblAlert.Text = string.Empty;
        }
        void AlertOk(string msg)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = "alert alert-success";
            lblAlert.Text = msg;
        }
        void AlertErr(string msg)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = "alert alert-danger";
            lblAlert.Text = msg;
        }
        #endregion
        //==========================
        //binds
        //==========================
        #region
        void BindObras()
        {
            var dt = BIZ.D_Presupuestos.obras_listar_combo();
            ddlObras.DataSource = dt;
            ddlObras.DataTextField = "nombre";
            ddlObras.DataValueField = "id_obra";
            ddlObras.DataBind();
            ddlObras.Items.Insert(0, new ListItem("-- Seleccione obra --", "0"));
        }

        void BindAcordeon()
        {
            lblMsgTop.Text = "";
            pnlAcordeon.Visible = (id_obra > 0);

            if (id_obra <= 0)
            {
                rptCarpetas.DataSource = null;
                rptCarpetas.DataBind();
                return;
            }

            var dt = BIZ.docs_carpetas.listar_por_obra(id_obra);
            rptCarpetas.DataSource = dt;
            rptCarpetas.DataBind();
        }
        #endregion
        //==========================
        //tablas
        //==========================
        #region
        protected void btnCrearCarpeta_Click(object sender, EventArgs e)
        {
            HideAlert();
            if (id_obra <= 0) { AlertErr("Elegí una obra primero."); return; }

            var nombre = (txtNuevaCarpeta.Text ?? "").Trim();
            if (string.IsNullOrEmpty(nombre) || nombre.Length < 2) { AlertErr("El nombre de la carpeta debe tener al menos 2 caracteres."); return; }
            if (nombre.Length > 30) { AlertErr("El nombre de la carpeta debe tener menos de 30 caracteres."); return; }
            var existentes = BIZ.docs_carpetas.listar_por_obra(id_obra);

            bool existe = existentes.AsEnumerable()
                .Any(r => string.Equals(r["descripcion"]?.ToString()?.Trim(),
                                        nombre, StringComparison.OrdinalIgnoreCase));
            if (existe)
            { AlertErr("Ya existe una carpeta con ese nombre en la obra."); return; }
            try
            {
                int nuevaId = BIZ.docs_carpetas.crear(id_obra, nombre);
                hfOpenCarpetaId.Value = nuevaId.ToString();  //aca dejo abierta la carpeta.
                txtNuevaCarpeta.Text = "";
                BindAcordeon();
                AlertOk("Carpeta creada.");
            }
            catch (Exception ex) { AlertErr(ex.Message); }
        }

        protected void gvDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del_doc")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    BIZ.docs.eliminar_doc(id);

                    var gv = (GridView)sender;
                    var repItem = (RepeaterItem)gv.NamingContainer;
                    var hfCarp = (HiddenField)repItem.FindControl("hfIdCarpeta");
                    if (hfCarp != null) hfOpenCarpetaId.Value = hfCarp.Value;

                    BindAcordeon();
                    AlertOk("Documento eliminado.");
                }
                catch (Exception ex) { AlertErr(ex.Message); }
            }
        }

        #endregion
        //==========================
        //dropdown
        //==========================
        #region
        protected void ddlObras_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideAlert();
            hfOpenCarpetaId.Value = "";   // aca voy a reseter el hf, cuando cambio de carpeta.
            BindAcordeon();
        }
        #endregion
        //==========================
        //repeater (carpetas)
        //==========================
        #region
        protected void rptCarpetas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            int idCarp = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_carpeta"));

         
            var gv = (GridView)e.Item.FindControl("gvDocs");
            DataTable dtDocs = BIZ.docs.listar_docs(id_obra, idCarp);
            gv.DataSource = dtDocs;
            gv.DataBind();

          
            if (hfOpenCarpetaId.Value == idCarp.ToString())
            {
             
                var collapseDiv = (HtmlGenericControl)e.Item.FindControl("col_" + idCarp);
                if (collapseDiv != null)
                {
                    string cls = collapseDiv.Attributes["class"] ?? "";
                    if (!cls.Contains("show")) collapseDiv.Attributes["class"] = cls + " show";
                }

              
                var btnToggler = (HtmlGenericControl)e.Item.FindControl("btnToggler");
                if (btnToggler != null)
                {
                    string cls = btnToggler.Attributes["class"] ?? "";
                    btnToggler.Attributes["class"] = cls.Replace("collapsed", "").Trim();
                    btnToggler.Attributes["aria-expanded"] = "true";
                }
            }
        }

        protected void rptCarpetas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HideAlert();

            int idCarp = Convert.ToInt32(e.CommandArgument);
            var lblRow = (Label)e.Item.FindControl("lblMsgRow");

            try
            {
                switch (e.CommandName)
                {
                    case "rename":
                        {
                            var txt = (TextBox)e.Item.FindControl("txtNombreCarpeta");
                            string nuevo = (txt.Text ?? "").Trim();
                            if (string.IsNullOrEmpty(nuevo) || nuevo.Length < 2)
                            { lblRow.Text = "Nombre inválido."; AlertErr("El nombre debe tener al menos 2 caracteres."); return; }
                            if (txt.Text.Length > 30) { AlertErr("El nombre de la carpeta debe tener menos de 30 caracteres."); return; }
                            BIZ.docs_carpetas.RenombrarCarpeta(idCarp, nuevo);
                            hfOpenCarpetaId.Value = idCarp.ToString();  
                            BindAcordeon();
                            AlertOk("Carpeta renombrada.");
                            break;
                        }

                    case "del_carp":
                        {
                            
                            BIZ.docs_carpetas.eliminar_logico(idCarp);
                            hfOpenCarpetaId.Value = ""; 
                            BindAcordeon();
                            AlertOk("Carpeta eliminada.");
                            break;
                        }

                    case "upload":
                        {
                            var fu = (FileUpload)e.Item.FindControl("fuPdf");
                            var txtTipo = (TextBox)e.Item.FindControl("txtTipo");
                            var txtVer = (TextBox)e.Item.FindControl("txtVersion");

                            if (fu == null || !fu.HasFile) { lblRow.Text = "Elegí un PDF."; AlertErr("Elegí un PDF."); return; }
                            var fn = fu.FileName;
                            var ct = fu.PostedFile.ContentType ?? "";
                            if (!fn.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) &&
                                !ct.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                            { lblRow.Text = "Solo PDF."; AlertErr("Solo PDF."); return; }

                            byte[] bin = fu.FileBytes;
                            const int Max = 20 * 1024 * 1024;
                            if (bin.Length == 0 || bin.Length > Max) { lblRow.Text = "Máximo 20MB."; AlertErr("Máximo 20MB."); return; }

                            string tipo = string.IsNullOrWhiteSpace(txtTipo.Text) ? "PDF" : txtTipo.Text.Trim();

                            int version;
                                version = BIZ.docs.proxima_version(idCarp, tipo);

                            var u = (BIZ.estructuras.usuario)Session["usuario"];
                            int idDoc = BIZ.docs.insertar_doc(idCarp, tipo, fn, version, DateTime.Today, u.nombre, bin);

                            hfOpenCarpetaId.Value = idCarp.ToString();  
                            BindAcordeon();
                            AlertOk($"Documento guardado.");
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                hfOpenCarpetaId.Value = idCarp.ToString();
                BindAcordeon();
                AlertErr(ex.Message);
            }
        }

       
        #endregion
    }
}
