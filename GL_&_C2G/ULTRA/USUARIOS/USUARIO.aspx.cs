using BIZ;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        
        private int IdUsuario
        {
            get
            {
                if (Session["id"] is int v) return v;
                int.TryParse(Session["id"]?.ToString(), out var x);
                return x; 
            }
        }

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null || IdUsuario == 0)
            {
                Response.Redirect("/ULTRA/INICIO/REGISTER");
                return;
            }


            if (!IsPostBack)
            {
                
                var usuario = A_Userr.traerUsuario(IdUsuario);
                tx_correo.Text = usuario.email;
                tx_nombre.Text = usuario.nombre;
                tx_apellido.Text = usuario.apellido;
                tx_tel.Text = usuario.telefono;

              
                lbl_errorNombre.Visible = lbl_errorApellido.Visible = lbl_errorTel.Visible = false;


                int nivel = 0;
                int.TryParse(Session["nivel"]?.ToString(), out nivel);
                bt_inicio.PostBackUrl = ResolveUrl(GetHomeUrlByNivel(nivel));

            }
        }
        #endregion

        #region Validaciones
        private bool validarCampos()
        {
           
            lbl_errorNombre.Visible = lbl_errorApellido.Visible = lbl_errorTel.Visible = false;

            var validador = new Validacion();
            var input = new formPerfil
            {
                telefono = tx_tel.Text,
                nombre = tx_nombre.Text,
                apellido = tx_apellido.Text,
            };
            var errores = validador.validarFormularioPerfil(input);

           
            if (errores.Equals(default(errorFormPerfil))) return true;

            
            if (!string.IsNullOrWhiteSpace(errores.nombre))
            {
                lbl_errorNombre.Text = errores.nombre;
                lbl_errorNombre.Visible = true;
            }
            if (!string.IsNullOrWhiteSpace(errores.apellido))
            {
                lbl_errorApellido.Text = errores.apellido;
                lbl_errorApellido.Visible = true;
            }
            if (!string.IsNullOrWhiteSpace(errores.telefono))
            {
                lbl_errorTel.Text = errores.telefono;
                lbl_errorTel.Visible = true;
            }
            return false;
        }

        private string GetHomeUrlByNivel(int nivel)
        {
        
            switch (nivel)
            {
                case 3: return "~/ULTRA/PAGINAS/PAG_PRINCIPAL_ADM";      
                case 2: return "~/ULTRA/PAGINAS/PAG_PRINCIPAL_DESIGNER";  
                case 1: return "~/ULTRA/PAGINAS/PAG_PRINCIPAL";          
                default: return "~/ULTRA/PAGINAS/PAG_PRINCIPAL";     
            }
        }

        #endregion

        #region Guardar
        protected void bt_guardar_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;

            var dto = new usuarioModif
            {
                telefono = tx_tel.Text?.Trim(),
                nombre = tx_nombre.Text?.Trim(),
                apellido = tx_apellido.Text?.Trim(),
            };

            
            A_Userr.ModificarUsuario(dto, IdUsuario);

            Response.Redirect(Request.RawUrl, false);
        }
        #endregion
    }
}
