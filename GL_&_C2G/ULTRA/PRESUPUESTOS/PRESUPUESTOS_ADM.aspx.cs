using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class WebForm9 : System.Web.UI.Page
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
                    Response.Redirect("/ULTRA/INICIO/REGISTER");
                }
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "PRESUPUESTOS_ADM"))
                {
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
                }

                CargarObras();
                if (int.TryParse(Request.QueryString["id_obra"], out int idQS))
                {
                    var item = ddlObras.Items.FindByValue(idQS.ToString());
                    if (item != null) ddlObras.SelectedValue = idQS.ToString();
                }
                BindRubros();
            }
        }
        #endregion

        //==========================
        // Ids, variables
        //==========================
        #region
        private int IdObraSel => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0;

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
            var dt = BIZ.D_Presupuestos.obras_listar_combo();
            ddlObras.DataSource = dt;
            ddlObras.DataTextField = "nombre";
            ddlObras.DataValueField = "id_obra";
            ddlObras.DataBind();
            ddlObras.Items.Insert(0, new ListItem("-- Seleccione obra --", "0"));
        }

        private void BindRubros()
        {
            try
            {
                if (IdObraSel <= 0)
                {
                    rptRubros.DataSource = null;
                    rptRubros.DataBind();
                    ShowMsg("Seleccioná una obra para gestionar sus rubros y materiales.", false);
                    return;
                }
                var rubros = BIZ.D_Presupuestos.rubrosR_listar_por_obra(IdObraSel);
                rptRubros.DataSource = rubros;
                rptRubros.DataBind();

                if (rubros == null || rubros.Rows.Count == 0)
                {
                    ShowMsg("Esta obra todavía no tiene rubros cargados.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMsg("No fue posible cargar los rubros: " + ex.Message, false);
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
            BindRubros();
        }
        #endregion

        //==========================
        // Panel mensajes +  KeepOpen
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

        
        private void KeepOpen(int rubroId, int itemIndex)
        {
            RubroOpenIndex = itemIndex; 

            string js =
                $"(function(){{" +
                $"  var el = document.getElementById('rubroBody_{rubroId}');" +
                $"  if (el) bootstrap.Collapse.getOrCreateInstance(el, {{toggle:false}}).show();" +
                $"}})();";

            ScriptManager.RegisterStartupScript(this, GetType(), "keepOpen_" + rubroId, js, true);
            upRubros.Update(); 
        }
        #endregion

        //==========================
        // Tablas / Repeater / Grid
        //==========================
        #region
        protected void rptRubros_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var drv = (DataRowView)e.Item.DataItem;
            int idRubro = Convert.ToInt32(drv["id_rubroR"]);

            var gv = (GridView)e.Item.FindControl("gvPres");
            gv.DataSource = BIZ.D_Presupuestos.presupuestos_listar_por_rubro(idRubro);
            gv.DataBind();

            var ddlTipo = (DropDownList)e.Item.FindControl("ddlTipoNuevo");
            if (ddlTipo != null)
            {
                var dtTipos = BIZ.D_Presupuestos.Tipo_listar();
                ddlTipo.DataSource = dtTipos;
                ddlTipo.DataTextField = "descripcion";
                ddlTipo.DataValueField = "id_tipo";
                ddlTipo.DataBind();
                ddlTipo.Items.Insert(0, new ListItem("-- Tipo --", ""));
            }
        }

        protected void rptRubros_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            bool rebind = false;
            int idRubro = -1;

            try
            {
                RubroOpenIndex = e.Item.ItemIndex; 
                idRubro = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "RenombrarRubro")
                {
                    var txt = (TextBox)e.Item.FindControl("txtRenameRubro");

                    if (string.IsNullOrWhiteSpace(txt.Text))
                    { ShowMsg("El nombre debe tener al menos 2 caracteres", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (txt.Text.Length > 30)
                    { ShowMsg("El nombre debe tener menos de 30 caracteres", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    BIZ.D_Presupuestos.rubroR_renombrar(new RubroActualizar { id_rubroR = idRubro, descripcion = txt.Text });
                    ShowMsg("Rubro actualizado.");
                    rebind = true;
                }
                else if (e.CommandName == "EliminarRubro")
                {
                    BIZ.D_Presupuestos.rubroR_borrar(idRubro);
                    ShowMsg("Rubro eliminado.");
                    rebind = true;
                }
                else if (e.CommandName == "AgregarPres")
                {
                    var txtMonto = (TextBox)e.Item.FindControl("txtMontoNuevo");
                    var txtDesc = (TextBox)e.Item.FindControl("txtDescNuevo");
                    var ddlTipo = (DropDownList)e.Item.FindControl("ddlTipoNuevo");
                    var txtValor = (TextBox)e.Item.FindControl("txtValorNuevo");

                    
                    if (string.IsNullOrWhiteSpace(txtDesc.Text))
                    { ShowMsg("La descripción es obligatoria.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (txtDesc.Text.Length > 30)
                    { ShowMsg("La descripción no puede contener mas de 30 caracteres.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (!int.TryParse(ddlTipo?.SelectedValue, out int idTipo))
                    { ShowMsg("Seleccioná un tipo (Horas/Cantidad/Peso).", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (!TryParseMontoPositivo(txtValor.Text, out decimal valorNum))
                    { ShowMsg("El valor debe ser un número > 0 (ej: horas, cantidad o peso).", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (valorNum > 9999999999)
                    { ShowMsg("El valor no puede contener mas de 10 dígitos.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (!TryParseMontoPositivo(txtMonto.Text, out decimal monto))
                    { ShowMsg("Monto inválido. Usá 12345,67 y > 0.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (txtMonto.Text.Length > 12)
                    { ShowMsg("Monto inválido. Máx. 12 caracteres.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                   
                    BIZ.D_Presupuestos.presupuesto_crear(new PresupuestoCrear
                    {
                        id_rubroR = idRubro,
                        monto_real = monto,
                        descripcion = txtDesc.Text,
                        id_tipo = idTipo,
                        valor = valorNum.ToString(CultureInfo.InvariantCulture)
                    });

                    ShowMsg($"Presupuesto agregado ($ {monto:N2}).");

                  
                    txtMonto.Text = string.Empty;
                    txtDesc.Text = string.Empty;
                    txtValor.Text = string.Empty;
                    ddlTipo.SelectedIndex = 0;

                    rebind = true;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Error: " + ex.Message, false);
                if (idRubro > 0) KeepOpen(idRubro, e.Item.ItemIndex);
            }
            finally
            {
                if (rebind)
                {
                    BindRubros();           
                    if (idRubro > 0) KeepOpen(idRubro, RubroOpenIndex); 
                }
            }
        }

        protected void btnAgregarRubro_OnClick(object sender, EventArgs e)
        {
            bool rebind = false;
            try
            {
                ClearMsg();
                if (IdObraSel <= 0) { ShowMsg("Seleccioná una obra primero.", false); return; }

                var nombre = txtNuevoRubro.Text?.Trim();
                if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2)
                { ShowMsg("El nombre del rubro debe tener al menos 2 caracteres.", false); upRubros.Update(); return; }

                if (nombre.Length > 30)
                { ShowMsg("El nombre del rubro debe tener menos de 30 caracteres.", false); upRubros.Update(); return; }

                
                var existentes = BIZ.D_Presupuestos.rubrosR_listar_por_obra(IdObraSel);
                bool existe = existentes.AsEnumerable()
                    .Any(r => string.Equals(r["descripcion"]?.ToString()?.Trim(),
                                            nombre, StringComparison.OrdinalIgnoreCase));
                if (existe)
                { ShowMsg("Ya existe un rubro con ese nombre en la obra.", false); upRubros.Update(); return; }

                BIZ.D_Presupuestos.rubroR_crear(IdObraSel, nombre);
                txtNuevoRubro.Text = string.Empty;
                ShowMsg("Rubro creado correctamente.");
                rebind = true;
            }
            catch (Exception ex)
            {
                ShowMsg("No se pudo crear el rubro: " + ex.Message, false);
                upRubros.Update();
            }
            finally
            {
                if (rebind) BindRubros();
            }
        }

        protected void gvPres_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idPres = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Guardar" || e.CommandName == "GuardarMonto")
                {
                    var btn = (LinkButton)e.CommandSource;
                    var row = (GridViewRow)btn.NamingContainer;
                    var txtM = (TextBox)row.FindControl("txtMonto");
                    var txtD = (TextBox)row.FindControl("txtDesc");

                    if (!TryParseMontoPositivo(txtM.Text, out decimal monto))
                    {
                        ShowMsg("Monto inválido. Usá 12345,67 y > 0.", false);
                        return;
                    }

                    BIZ.D_Presupuestos.presupuesto_actualizar(new PresupuestoActualizar
                    {
                        id_presupuesto = idPres,
                        monto_real = monto,
                        descripcion = txtD.Text
                    });
                    ShowMsg("Presupuesto actualizado.");
                }
                else if (e.CommandName == "BorrarPres")
                {
                    BIZ.D_Presupuestos.presupuesto_borrar(idPres);
                    ShowMsg("Presupuesto eliminado.");
                }
            }
            catch (Exception ex) { ShowMsg("Error: " + ex.Message, false); }
            finally { BindRubros(); }
        }
        #endregion

        //==========================
        // Validaciones
        //==========================
        #region
       
        private bool TryParseMontoPositivo(string s, out decimal monto)
        {
            monto = 0m;
            var esAR = new CultureInfo("es-AR");
            s = s?.Trim();

            if (decimal.TryParse(s, NumberStyles.Number, esAR, out var m) ||
                decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out m))
            {
                if (m > 0) { monto = m; return true; }
            }
            return false;
        }
        #endregion
    }
}
