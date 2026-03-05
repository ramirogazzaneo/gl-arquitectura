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
    public partial class WebForm6 : System.Web.UI.Page
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
                    return;
                }
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "MATERIALES_ADM"))
                {
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
                    return;
                }

                BindObras();

               
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
        // Variables / IDs
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
        // Binds
        //==========================
        #region
        private void BindObras()
        {
            var dt = BIZ.D_Materiales.obras_listar_combo();
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

                var rubros = BIZ.D_Materiales.rubros_listar_por_obra(IdObraSel);
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
        // Repeater / Grid
        //==========================
        #region
        
        protected void rptRubros_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var dr = (DataRowView)e.Item.DataItem;
            int idRubro = Convert.ToInt32(dr["id_rubro"]);

            var gv = (GridView)e.Item.FindControl("gvMateriales");
            gv.DataSource = BIZ.D_Materiales.materiales_listar_por_rubro(idRubro);
            gv.DataBind();
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

                var existentes = BIZ.D_Materiales.rubros_listar_por_obra(IdObraSel);
                bool existe = existentes.AsEnumerable()
                    .Any(r => string.Equals(r["descripcion"]?.ToString()?.Trim(),
                                            nombre, StringComparison.OrdinalIgnoreCase));
                if (existe)
                { ShowMsg("Ya existe un rubro con ese nombre en la obra.", false); upRubros.Update(); return; }

                BIZ.D_Materiales.rubro_crear(IdObraSel, nombre);
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

      
        protected void rptRubros_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            bool rebind = false;
            int idRubro = -1;

            // mantener contexto del acordeón
            RubroOpenIndex = e.Item.ItemIndex;

            try
            {
                ClearMsg();
                idRubro = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "RenombrarRubro")
                {
                    var txt = (TextBox)e.Item.FindControl("txtRenameRubro");
                    var nuevo = txt.Text?.Trim();

                    if (string.IsNullOrWhiteSpace(nuevo) || nuevo.Length < 2)
                    { ShowMsg("El nombre del rubro no puede estar vacío (mín. 2 caracteres).", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (nuevo.Length > 30)
                    { ShowMsg("El nombre del rubro debe contener menos de 30 caracteres.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    BIZ.D_Materiales.rubro_renombrar(idRubro, nuevo);
                    ShowMsg("Rubro actualizado.");
                    rebind = true;
                }
                else if (e.CommandName == "EliminarRubro")
                {
                    BIZ.D_Materiales.rubro_borrar(idRubro);
                    ShowMsg("Rubro eliminado.");
                    rebind = true;
                }
                else if (e.CommandName == "AgregarMaterial")
                {
                    var txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
                    var chkEntregado = (CheckBox)e.Item.FindControl("chkEntregado");
                    var txtCantidad = (TextBox)e.Item.FindControl("txtCantidad");

                    var desc = txtMaterial.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(desc))
                    { ShowMsg("La descripción del material es obligatoria.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (desc.Length > 30)
                    { ShowMsg("La descripción del material no puede contener mas de 30 caracteres.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                    if (string.IsNullOrWhiteSpace(txtCantidad.Text))
                    { ShowMsg("La cantidad es requerida.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                   
                    if (!int.TryParse(txtCantidad.Text, NumberStyles.None, CultureInfo.InvariantCulture, out var numero))
                    { ShowMsg("La cantidad solo puede contener dígitos (0-9) y no permite mayores a '2.147.483.647'.", false); KeepOpen(idRubro, e.Item.ItemIndex); return; }

                  
                    BIZ.D_Materiales.material_crear(new BIZ.estructuras.materialCrear
                    {
                        id_rubro = idRubro,
                        cantidad = numero,
                        descripcion = desc,
                        entrega = (chkEntregado != null && chkEntregado.Checked)
                    });

                    
                    txtMaterial.Text = string.Empty;
                    txtCantidad.Text = string.Empty;
                    if (chkEntregado != null) chkEntregado.Checked = false;

                    ShowMsg("Material agregado.");
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
                    BindRubros();                  // refrescamos
                    if (idRubro > 0) KeepOpen(idRubro, RubroOpenIndex); // y lo dejamos abierto
                }
            }
        }

        
        protected void gvMateriales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool rebind = false;

            try
            {
                ClearMsg();

                var gv = (GridView)sender;
                var rptItem = (RepeaterItem)gv.NamingContainer;
                RubroOpenIndex = rptItem.ItemIndex;

                if (e.CommandName == "BorrarMaterial")
                {
                    int idMaterial = Convert.ToInt32(e.CommandArgument);
                    BIZ.D_Materiales.material_borrar(idMaterial);
                    ShowMsg("Material eliminado.");
                    rebind = true;
                }
                else if (e.CommandName == "ToggleEntrega")
                {
                    var row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    bool entregaActual = Convert.ToBoolean(gv.DataKeys[row.RowIndex].Values["entrega"]);

                    int idMaterial = Convert.ToInt32(e.CommandArgument);
                    BIZ.D_Materiales.material_set_entrega(idMaterial, !entregaActual);

                    ShowMsg(!entregaActual ? "Material marcado como entregado." : "Material marcado como pendiente.");
                    rebind = true;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("No se pudo completar la acción: " + ex.Message, false);
            }
            finally
            {
                if (rebind)
                {
                    
                    var gv = (GridView)sender;
                    var rptItem = (RepeaterItem)gv.NamingContainer;
                    var dr = (DataRowView)rptItem.DataItem; 
                    int rubroId;

                  
                    var hid = (HiddenField)rptItem.FindControl("hidIdRubro");
                    if (hid != null && int.TryParse(hid.Value, out var hidVal))
                        rubroId = hidVal;
                    else
                        rubroId = -1;

                    BindRubros();
                    if (rubroId > 0) KeepOpen(rubroId, RubroOpenIndex);
                }
            }
        }
        #endregion
    }
}
