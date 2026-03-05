using BIZ;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class AF_TAREAS_ETAPAS_ADM : System.Web.UI.Page
    {
        //==========================
        // Page_Load
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["nivel"] == null)
                {
                    Response.Redirect("/ULTRA/INICIO/REGISTER.aspx");
                    return;
                }
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "TAREAS_ETAPAS_ADM"))
                {
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
                    return;
                }

                CargarObras();

                if (int.TryParse(Request.QueryString["id_obra"], out int idObraQS))
                {
                    var item = ddlObras.Items.FindByValue(idObraQS.ToString());
                    if (item != null) ddlObras.SelectedValue = idObraQS.ToString();
                }

                BindEtapas();
            }
        }
        #endregion

        //==========================
        // Variables / estado
        //==========================
        #region
        private int IdObraSeleccionada => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0;

        protected int RubroOpenIndex
        {
            get => ViewState["RubroOpenIndex"] as int? ?? -1;
            set => ViewState["RubroOpenIndex"] = value;
        }
        #endregion

        //==========================
        // Binds
        //==========================
        #region
        private void CargarObras()
        {
            var dt = BIZ.C_EyT.obras_listar_comboE();
            ddlObras.DataSource = dt;
            ddlObras.DataTextField = "nombre";
            ddlObras.DataValueField = "id_obra";
            ddlObras.DataBind();
            ddlObras.Items.Insert(0, new ListItem("-- Seleccione obra --", "0"));
        }

        private void BindEtapas()
        {
            try
            {
                if (IdObraSeleccionada <= 0)
                {
                    rptEtapas.DataSource = null;
                    rptEtapas.DataBind();
                    ShowMsg("Seleccioná una obra para ver sus etapas y tareas.", false);
                    upEtapas.Update();
                    return;
                }

                var etapas = BIZ.C_EyT.etapas_listar_por_obra_estado(IdObraSeleccionada);
                rptEtapas.DataSource = etapas;
                rptEtapas.DataBind();

                if (etapas == null || etapas.Rows.Count == 0)
                {
                    ShowMsg("Esta obra todavía no tiene etapas cargadas.", false);
                    upEtapas.Update();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("No fue posible cargar las etapas: " + ex.Message, false);
                upEtapas.Update();
            }
        }
        #endregion

        //==========================
        // Dropdown
        //==========================
        #region
        protected void ddlObras_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearMsg();
            RubroOpenIndex = -1;
            BindEtapas();
        }
        #endregion

        //==========================
        // Mensajes +  KeepOpen
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

        // Mantener abierto el acordeón de la etapa
        private void KeepOpen(int etapaId, int itemIndex)
        {
            RubroOpenIndex = itemIndex; // por las dudas

            string js =
                $"(function(){{" +
                $"  var el = document.getElementById('rubroBody_{etapaId}');" +
                $"  if (el) bootstrap.Collapse.getOrCreateInstance(el, {{toggle:false}}).show();" +
                $"}})();";

            ScriptManager.RegisterStartupScript(this, GetType(), "keepOpen_" + etapaId, js, true);
            upEtapas.Update();
        }
        #endregion

        //==========================
        // Repeater / Grid
        //==========================
        #region
        protected void rptEtapas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var drv = (DataRowView)e.Item.DataItem;
            int idEtapa = Convert.ToInt32(drv["id_etapa"]);

           
            var gv = (GridView)e.Item.FindControl("gvTareas");
            var dtTareas = BIZ.C_EyT.tareas_listar_por_etapa(idEtapa);
            gv.DataSource = dtTareas;
            gv.DataBind();

           
            int total = dtTareas?.Rows.Count ?? 0;
            int done = (dtTareas == null) ? 0 : dtTareas.AsEnumerable().Count(r => r["fecha_fin"] != DBNull.Value);
            int pct = (total == 0) ? 0 : (int)Math.Round(100.0 * done / total);

            var lit = (Literal)e.Item.FindControl("litAvanceText");
            var bar = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("barAvance");

            if (lit != null) lit.Text = $"{done}/{total} ({pct}%)";

            if (bar != null)
            {
                bar.Attributes["style"] = $"width:{pct}%";
                string cls = (pct >= 100) ? "bg-success" : (pct >= 50 ? "bg-info" : "bg-warning");
                bar.Attributes["class"] = $"progress-bar {cls}";
                bar.Attributes["aria-valuenow"] = pct.ToString();
                bar.Attributes["aria-valuemin"] = "0";
                bar.Attributes["aria-valuemax"] = "100";
            }
        }

        protected void rptEtapas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            bool rebind = false;
            int idEtapa = -1;

            try
            {
                RubroOpenIndex = e.Item.ItemIndex;
                idEtapa = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Renombrar")
                {
                    var txt = (TextBox)e.Item.FindControl("txtRename");

                    if (string.IsNullOrWhiteSpace(txt.Text) || txt.Text.Length < 2)
                    { ShowMsg("El nombre de la etapa debe tener al menos 2 caracteres.", false); KeepOpen(idEtapa, e.Item.ItemIndex); return; }

                    if (txt.Text.Length > 30)
                    { ShowMsg("El nombre de la etapa debe tener  menos de 30 caracteres.", false); KeepOpen(idEtapa, e.Item.ItemIndex); return; }

                    BIZ.C_EyT.etapa_renombrar(idEtapa, txt.Text);
                    ShowMsg("Etapa renombrada.");
                    rebind = true;
                }
                else if (e.CommandName == "EliminarEtapa")
                {
                    BIZ.C_EyT.etapa_borrar(idEtapa);
                    ShowMsg("Etapa eliminada.");
                    rebind = true;
                }
                else if (e.CommandName == "AgregarTarea")
                {
                    if (!ValidarCamposTarea(e))
                    { KeepOpen(idEtapa, e.Item.ItemIndex); return; }

                    var txtDesc = (TextBox)e.Item.FindControl("txtDesc");
                    var txtIni = (TextBox)e.Item.FindControl("txtIni");
                    var txtFin = (TextBox)e.Item.FindControl("txtFin");
                    var chkHito = (CheckBox)e.Item.FindControl("chkHito");

                    DateTime? fIni = DateTime.TryParseExact(txtIni.Text, "yyyy-MM-dd",
                                           CultureInfo.InvariantCulture, DateTimeStyles.None, out var d1) ? d1 : (DateTime?)null;


                    if (txtDesc.Text.Length > 30)
                    { ShowMsg("El nombre de la tarea debe tener menos de 30 caracteres.", false); KeepOpen(idEtapa, e.Item.ItemIndex); return; }

                    var cargaTarea = new estructuras.tareaCrear
                    {
                       
                        id_etapa = idEtapa,
                        descripcion = txtDesc.Text.Trim(),
                        fecha_inicio = fIni,
                        es_hito = chkHito.Checked
                    };
                    BIZ.C_EyT.tarea_crear(cargaTarea);

                    ShowMsg("Tarea creada correctamente.");

                   
                    txtDesc.Text = ""; txtIni.Text = ""; chkHito.Checked = false;

                    rebind = true;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Error: " + ex.Message, false);
                if (idEtapa > 0) KeepOpen(idEtapa, e.Item.ItemIndex);
            }
            finally
            {
                if (rebind)
                {
                    BindEtapas();
                    if (idEtapa > 0) KeepOpen(idEtapa, RubroOpenIndex);
                }
            }
        }

        protected void gvTareas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Finalizar" && e.CommandName != "Borrar") return;

            bool rebind = false;
            int etapaId = -1;

            try
            {
                var gv = (GridView)sender;
                var rptItem = (RepeaterItem)gv.NamingContainer;
                RubroOpenIndex = rptItem.ItemIndex;

               
                var hid = (HiddenField)rptItem.FindControl("hidIdEtapa");
                if (hid != null && int.TryParse(hid.Value, out var tmp)) etapaId = tmp;

                int idTarea = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Finalizar")
                {
                    BIZ.C_EyT.tarea_finalizar(idTarea, null);
                    ShowMsg("Tarea finalizada.");
                    rebind = true;
                }
                else if (e.CommandName == "Borrar")
                {
                    BIZ.C_EyT.tarea_borrar(idTarea);
                    ShowMsg("Tarea eliminada.");
                    rebind = true;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("No se pudo procesar la tarea: " + ex.Message, false);
            }
            finally
            {
                if (rebind)
                {
                    BindEtapas();
                    if (etapaId > 0) KeepOpen(etapaId, RubroOpenIndex);
                }
            }
        }

        protected void btnAgregarEtapa_OnClick(object sender, EventArgs e)
        {
            bool rebind = false;
            try
            {
                if (IdObraSeleccionada <= 0)
                { ShowMsg("Seleccioná una obra primero.", false); upEtapas.Update(); return; }

                var nombre = txtNuevaEtapa.Text?.Trim();
                if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2)
                { ShowMsg("El nombre de la etapa debe tener al menos 2 caracteres.", false); upEtapas.Update(); return; }

                if (nombre.Length > 30)
                { ShowMsg("El nombre de la etapa debe tener menos de 30 caracteres.", false); upEtapas.Update(); return; }

                var existentes = BIZ.C_EyT.etapas_listar_por_obra(IdObraSeleccionada);
                bool existe = existentes.AsEnumerable()
                    .Any(r => string.Equals(r["nombre"]?.ToString()?.Trim(),
                                            nombre, StringComparison.OrdinalIgnoreCase));
                if (existe)
                { ShowMsg("Ya existe una etapa con ese nombre en la obra.", false); upEtapas.Update(); return; }


                BIZ.C_EyT.etapa_crear(IdObraSeleccionada, nombre);
                txtNuevaEtapa.Text = string.Empty;
                ShowMsg("Etapa creada correctamente.");
                rebind = true;
            }
            catch (Exception ex)
            {
                ShowMsg("No se pudo crear la etapa: " + ex.Message, false);
                upEtapas.Update();
            }
            finally
            {
                if (rebind)
                {
                    BindEtapas();
                    upEtapas.Update();
                }
            }
        }
        #endregion

        //==========================
        // Validaciones
        //==========================
        #region
        private bool ValidarCamposTarea(RepeaterCommandEventArgs e)
        {
            var txtDesc = (TextBox)e.Item.FindControl("txtDesc");
            var txtIni = (TextBox)e.Item.FindControl("txtIni");
            var txtFin = (TextBox)e.Item.FindControl("txtFin");

            bool okIni = DateTime.TryParseExact(txtIni.Text, "yyyy-MM-dd",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out var fIni);


            var v = new Validacion();
            var input = new formTarea
            {
                descripcion = txtDesc.Text,
                fecha_inicio = okIni ? fIni : DateTime.MinValue,
            };
            var err = v.validarTarea(input);

            var msgs = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrWhiteSpace(err.descripcion)) msgs.Add(err.descripcion);
            if (!okIni) msgs.Add("La fecha de inicio es obligatoria.");
            else if (!string.IsNullOrWhiteSpace(err.fecha_inicio)) msgs.Add(err.fecha_inicio);


            if (msgs.Count > 0)
            {
                ShowMsg(string.Join(" | ", msgs), false);
                return false;
            }

            return true;
        }
        #endregion

        //==========================
        // Helpers
        //==========================
        #region
        protected bool PuedeFinalizar(object fecha_inicio, object fecha_fin)
        {
            if (fecha_fin != DBNull.Value && fecha_fin != null) return false;
            if (fecha_inicio == DBNull.Value || fecha_inicio == null) return false;

            DateTime ini = (DateTime)fecha_inicio;
            return DateTime.Today >= ini.Date;
        }

        protected string ClaseBotonFinalizar(object fecha_inicio, object fecha_fin)
        {
            bool ok = PuedeFinalizar(fecha_inicio, fecha_fin);
            return "btn btn-sm btn-success" + (ok ? "" : " disabled");
        }
        #endregion
    }
}
