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
    public partial class WebForm2 : System.Web.UI.Page
    {
        //==========================
        //pageload
        //==========================
        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["id"] == null)
                {
                    Response.Redirect("/ULTRA/INICIO/REGISTER");
                }
            }
        }
        #endregion
        //==========================
        //validaciones
        //==========================
        #region
        private bool validarCampos()
        {
            Validacion validador = new Validacion();
            formCambiarClave input = new formCambiarClave
            {
                claveActual = tx_claveActual.Text,
                claveConfirmacion = tx_claveConfirmacion.Text,
                claveNueva = tx_claveNueva.Text,
            };

            errorFormCambiarClave errores = validador.validarFormCambiarClave(input, ((usuario)Session["usuario"]).email);

         
            bool hayErrores = !string.IsNullOrWhiteSpace(errores.claveActual)
                           || !string.IsNullOrWhiteSpace(errores.claveNueva)
                           || !string.IsNullOrWhiteSpace(errores.claveConfirmacion);

            if (!hayErrores)
                return true;

            lbl_erroClaveConfirmacion.Text = errores.claveConfirmacion;
            lbl_erroClaveConfirmacion.Visible = true;

            lbl_erroClaveNueva.Text = errores.claveNueva;
            lbl_erroClaveNueva.Visible = true;

            lbl_errorClaveActual.Text = errores.claveActual;
            lbl_errorClaveActual.Visible = true;
            return false;
        }
        #endregion
        //==========================
        //btns
        //==========================
        #region
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ULTRA/USUARIOS/USUARIO");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;
            Debug.WriteLine("HELLO");
            A_Userr.actualizar_contraseña((int)Session["id"], Hasher.encriptar(tx_claveNueva.Text));
            lt_respuesta.Text = @"
            <div class='alert alert-success' role='alert'>
                <p>Su contraseña ha sido cambiada correctamente.</p>
                <hr>
                <p class='mb-0'><a href='/ULTRA/PAGINAS/PAG_PRINCIPAL.aspx' class='alert-link'>Haga clic aquí para ir a la página principal</a></p>
            </div>";

            Debug.WriteLine("VALIDACIÓN PASÓ");

            return;
        }

        #endregion
    }
}