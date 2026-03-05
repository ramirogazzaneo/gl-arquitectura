using BIZ;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class WebForm12 : System.Web.UI.Page
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
                    Response.Redirect("/ULTRA/INICIO/REGISTER.aspx");
                    return;
                }
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "FINANZAS_ADM"))
                {
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
                    return;
                }

                BindObras();

                
                if (int.TryParse(Request.QueryString["id_obra"], out int qs))
                {
                    var it = ddlObras.Items.FindByValue(qs.ToString());
                    if (it != null) ddlObras.SelectedValue = qs.ToString();
                }

                BindPagos(); 
            }
        }
        #endregion
        //==========================
        //variables, ids
        //==========================
        #region

        private int idObraa => int.TryParse(ddlObras.SelectedValue, out var v) ? v : 0; 
        #endregion
        //==========================
        //binds
        //==========================
        #region
        private void BindObras() 
        {
            var dt = BIZ.D_Presupuestos.obras_listar_combo();
            ddlObras.DataSource = dt;
            ddlObras.DataTextField = "nombre";
            ddlObras.DataValueField = "id_obra";
            ddlObras.DataBind();
            ddlObras.Items.Insert(0, new ListItem("-- Seleccione obra --", "0"));
        }

        private void BindPagos() 
        {
            try
            {
                if (idObraa <= 0)
                {
                    pnlLista.Visible = false;
                    gvPagos.DataSource = null;
                    gvPagos.DataBind();

                    ShowMsg("Seleccioná una obra para gestionar sus pagos.", false); 
                    return;
                }
                pnlLista.Visible = true;


                var pagos = BIZ.D_Finanzas.pagos_listar_por_obra(idObraa); 
                gvPagos.DataSource = pagos;
                gvPagos.DataBind();

                if (pagos == null || pagos.Rows.Count == 0) 
                {
                    ShowMsg("No hay pagos cargados para esta obra.", false);
                    pnlLista.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ShowMsg("No fue posible cargar los pagos: " + ex.Message, false);
            }
        }

        #endregion
        //==========================
        //dropdown
        //==========================
        #region

        protected void ddlObras_SelectedIndexChanged(object sender, EventArgs e) 
        {
            ClearMsg();
            BindPagos();
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
        //tablas
        //==========================
        #region
        protected void btnAgregar_OnClick(object sender, EventArgs e) 
        {
            try
            {
                ClearMsg();

                if (idObraa <= 0) { ShowMsg("Seleccioná una obra.", false); updPagos.Update(); return; }

                DateTime fecha;
                if (!DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out fecha))
                { ShowMsg("Fecha inválida.", false); updPagos.Update(); return; }

                
                if (fecha < DateTime.Today)
                { ShowMsg("La fecha debe ser hoy o posterior.", false); updPagos.Update(); return; }

                var desc = (txtDescripcion.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(desc))
                { ShowMsg("La descripción es requerida.", false); updPagos.Update(); return; }
                if (desc.Length > 30)
                { ShowMsg("La descripción no puede tener más de 30 caracteres.", false); updPagos.Update(); return; }

                var textoMonto = (txtMonto.Text ?? "").Trim();
                var rxMonto = new System.Text.RegularExpressions.Regex(@"^\d{1,10}(,\d{1,2})?$");
                if (!rxMonto.IsMatch(textoMonto))
                { ShowMsg("Monto inválido. Usá 12345,67 (hasta 10 enteros y 2 decimales).", false); updPagos.Update(); return; }

                var culturaAr = new CultureInfo("es-AR");
                decimal monto = decimal.Parse(textoMonto, NumberStyles.Number, culturaAr);
                if (monto <= 0)
                { ShowMsg("El monto debe ser mayor que cero.", false); updPagos.Update(); return; }

                var pago = new pagoCrear
                {
                    id_obra = idObraa,
                    descripcion = desc,
                    fecha = fecha,
                    monto = monto,
                    estado_pago = chkPagadoNuevo.Checked
                };
                BIZ.D_Finanzas.pago_crear(pago);
                ShowMsg("Pago creado. Vence a los 15 días de la fecha seleccionada.");

                txtDescripcion.Text = "";
                txtMonto.Text = "";
                txtFecha.Text = "";
                chkPagadoNuevo.Checked = false;

                BindPagos();
                updPagos.Update();
            }
            catch (Exception ex)
            {
                ShowMsg("No se pudo crear el pago: " + ex.Message, false);
                updPagos.Update();
            }
        }


        protected void gvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                ClearMsg();

                
                if (e.CommandName == "Borrar")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    BIZ.D_Finanzas.pago_eliminar(id);
                    ShowMsg("Pago eliminado.");
                    BindPagos();
                    updPagos.Update();
                    return;
                }

                
                var btn = (Control)e.CommandSource;
                var row = btn.NamingContainer as GridViewRow;

                
                if (e.CommandName == "CambiarEstado")
                {
                    string[] args = e.CommandArgument.ToString().Split('|');
                    int idPago = Convert.ToInt32(args[0]);
                    bool estadoActual = Convert.ToBoolean(args[1]);

                    bool nuevoEstado = !estadoActual;
                    BIZ.D_Finanzas.pago_cambiar_estado(idPago, nuevoEstado);

                    ShowMsg(nuevoEstado ? "Pago marcado como PAGADO." : "Pago marcado como PENDIENTE.");
                    BindPagos();
                    updPagos.Update();
                    return;
                }

                
                if (e.CommandName == "Guardar")
                {
                    int idPago = Convert.ToInt32(e.CommandArgument);
                    var txtDesc = row.FindControl("txtDesc") as TextBox;
                    var txtMontoEdit = row.FindControl("txtMontoEdit") as TextBox;

                    var culturaAr = new CultureInfo("es-AR");
                    var textoMonto = (txtMontoEdit?.Text ?? "").Trim();

                    var rxMonto = new System.Text.RegularExpressions.Regex(@"^\d{1,10}(,\d{1,2})?$");
                    if (!rxMonto.IsMatch(textoMonto))
                    { ShowMsg("Monto inválido. Usá 12345,67 (hasta 10 enteros y 2 decimales).", false); updPagos.Update(); return; }

                    decimal monto = decimal.Parse(textoMonto, NumberStyles.Number, culturaAr);
                    if (monto <= 0)
                    { ShowMsg("El monto debe ser mayor que cero.", false); updPagos.Update(); return; }

                    if ((txtDesc?.Text ?? "").Length > 30)
                    { ShowMsg("La descripción no puede tener más de 30 caracteres.", false); updPagos.Update(); return; }

                    var pago = new BIZ.estructuras.formPago
                    {
                        id_pago = idPago,
                        monto = monto,
                        descripcion = txtDesc.Text.Trim()
                    };

                    
                    BIZ.D_Finanzas.pago_actualizar(pago);

                    ShowMsg("Pago actualizado.");
                    BindPagos();
                    updPagos.Update();
                    return;
                }

            }
            catch (Exception ex)
            {
                ShowMsg("No se pudo completar la acción: " + ex.Message, false);
                updPagos.Update();
            }
        }

        protected void gvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var dr = (System.Data.DataRowView)e.Row.DataItem;

            bool pagado = dr["estado_pago"] != DBNull.Value && Convert.ToBoolean(dr["estado_pago"]);
            bool vencido = dr.DataView.Table.Columns.Contains("vencido")
                           && dr["vencido"] != DBNull.Value && Convert.ToInt32(dr["vencido"]) == 1;

            
            if (pagado)
                e.Row.CssClass += " table-success";
            else if (vencido)
                e.Row.CssClass += " table-danger";
            else
                e.Row.CssClass += " table-warning";
        }

        #endregion
    }
}
