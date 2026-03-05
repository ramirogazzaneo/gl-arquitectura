using BIZ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class WebForm7 : System.Web.UI.Page
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
                if (!BIZ.Acceso.puedeUsarioIngresarPagina(int.Parse(Session["nivel"].ToString()), "DIRECCION_ADM"))
                {
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
                    return;
                }
                var uidQS = Session["usuarioIdParaObra"]?.ToString() ?? Request["usuarioId"];
                txIdU.Text = uidQS;

            }
        }
        #endregion
        //==========================
        //tablas
        //==========================
        #region

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!validarCampos()) return; 


            var direccion = new direccionCarga
            {
                calle = txCalle.Text,
                numero = txNumero.Text,
                barrio = txBarrio.Text,
                localidad = txLocalidad.Text,
                provincia = txProvincia.Text,
            };
            int idDireccion = B_Direccion.carga_direccion(direccion);

           
            var obr = new ObrasCarga
            {
                id_usuario = Convert.ToInt32(txIdU.Text),
                nombreO = txNombre.Text,
                metrosT = txM2T.Text,
            };
            B_obras.carga_obra(obr, idDireccion);

                Response.Redirect("/ULTRA/OBRAS/OBRAS_ADM?msg=obra_ok");
                return;
            }
            catch (Exception ex)
            {
                Label1.Text = "No se pudo registrar la obra: " + ex.Message;
                Label1.Visible = true;
            }
        }

        #endregion
        //==========================
        //validaciones
        //==========================
        #region

        private bool validarCampos()
        {
         
            limpiarMensajes();

            var validador = new Validacion();

            bool ok = true;

           
            var inputDir = new formDireccion
            {
                calle = txCalle.Text,
                numero = txNumero.Text,
                barrio = txBarrio.Text,
                localidad = txLocalidad.Text,
                provincia = txProvincia.Text,
            };
            var errDir = validador.validarDireccion(inputDir);
            if (!errDir.Equals(default(errorFormDireccion)))
            {
                lblCalle.Text = errDir.calle;
                lblCalle.Visible = !string.IsNullOrEmpty(errDir.calle);

                lblNumero.Text = errDir.numero;
                lblNumero.Visible = !string.IsNullOrEmpty(errDir.numero);

                lblBarrio.Text = errDir.barrio;
                lblBarrio.Visible = !string.IsNullOrEmpty(errDir.barrio);

                lblLocalidad.Text = errDir.localidad;
                lblLocalidad.Visible = !string.IsNullOrEmpty(errDir.localidad);

                lblProvincia.Text = errDir.provincia;
                lblProvincia.Visible = !string.IsNullOrEmpty(errDir.provincia);

                ok = false;
            }

           
            var inputObra = new formObra
            {
                nombreO = txNombre.Text,
                metrosT = txM2T.Text
            };
            var errObra = validador.validarObra(inputObra);
            if (!errObra.Equals(default(errorFormObra)))
            {
                lblNombre.Text = errObra.nombreO;   
                lblNombre.Visible = !string.IsNullOrEmpty(errObra.nombreO);

                lblMetros2.Text = errObra.metrosT;  
                lblMetros2.Visible = !string.IsNullOrEmpty(errObra.metrosT);

                ok = false;
            }

            return ok; 
        }

        #endregion
        //==========================
        //mensajes
        //==========================
        #region
        private void limpiarMensajes()
        {
            lblCalle.Text = lblNumero.Text = lblBarrio.Text = lblLocalidad.Text = lblProvincia.Text =
            lblNombre.Text = lblMetros2.Text = string.Empty;

            lblCalle.Visible = lblNumero.Visible = lblBarrio.Visible = lblLocalidad.Visible = lblProvincia.Visible =
            lblNombre.Visible = lblMetros2.Visible = false;
        }

        #endregion
    }
}