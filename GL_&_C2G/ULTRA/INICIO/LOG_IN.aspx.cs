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
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["usuario"] != null)
                {
                    lt_login.Visible = true;
                    Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");

                }
                else lt_login.Visible = false;
            }
        }


        //ACCIONES=====================================================================================
        protected void bt_login_Click(object sender, EventArgs e)
        {
      
          
            
            if (!validarForm()) return;
            (bool esValido, int id) = A_Userr.ValidarContrasenia(txtCorreo.Text, txtPass.Text);

            (bool esValidoo, int idd) = A_Userr.ValidateExistente(txtCorreo.Text, txtPass.Text);


            if (!esValido)
            {
                lb_mensaje.Text = "Contraseña o usuario incorrecto, pruebe otra vez.";
                lb_mensaje.Visible = true;
                return;
            };


            if (!esValidoo)
            {
                lb_mensaje.Text = "Este Usuario Fue eliminado";
                lb_mensaje.Visible = true;
                return;
            };
            estructuras.usuario usuario = A_Userr.traerUsuario(id);

            Session["usuario"] = usuario;
            Session["id"] = id;
            Session["nivel"] = usuario.nivel;

            if ((int)Session["nivel"] == 2)
            {
                Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL_DES");
                return;
            }

            else if ((int)Session["nivel"] == 3)
            {
                Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL_ADM");
                return;
            }

            else
                Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
            return;
        }

        protected void lbtn_register_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ULTRA/INICIO/REGISTER");
        }

        //=====================================================================================


        //VALIDACIONES=====================================================================================

        protected bool validarForm()
        {
            lblErrorPass.Visible = false;
            lblErrorCorreo.Visible = false;
            Validacion validador = new Validacion();
            formLogin input = new formLogin
            {
                email = txtCorreo.Text,
                contraseña = txtPass.Text,
            };
            errorFormLogin errores = validador.validarFormularioLogin(input);
            if (errores.Equals(default(errorFormLogin))) return true;
            lblErrorPass.Text = errores.contraseña;
            lblErrorPass.Visible = true;
            lblErrorCorreo.Text = errores.email;
            lblErrorCorreo.Visible = true;
            return false;
        }


        //=====================================================================================
       
    }
}