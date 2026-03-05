using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BIZ;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class _Default : Page
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
                else
                {
                    lt_login.Visible = false;
                }
            }
        }

        //ACCIONES=====================================================================================
        protected void bt_regist_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) { return; }
            usuarioCarga usuario = new usuarioCarga
            {
                nombre = txtNombre.Text,
                apellido = txtApellido.Text,
                email = txtCorreo.Text,
                contraseña = Hasher.encriptar(txtPass.Text),
                telefono = txtTel.Text
            };

            int userId = A_Userr.cargar_usuario(usuario);
            estructuras.usuario usuarioCargado = A_Userr.traerUsuario(userId);

            Session["usuario"] = usuarioCargado;
            Session["id"] = userId;
            Session["nivel"] = usuarioCargado.nivel;

            if (Session["usuario"] != null)
            {
                Response.Redirect("/ULTRA/PAGINAS/PAG_PRINCIPAL");
            }

        }

        protected void lbtn_login_Click1(object sender, EventArgs e)
        {
            Response.Redirect("/ULTRA/INICIO/LOG_IN");
        }
        //=====================================================================================


        //VALIDACIONES=====================================================================================
        private bool validarCampos()
        {
            Validacion validador = new Validacion();
            formRegistrar input = new formRegistrar
            {
                email = txtCorreo.Text,
                telefono = txtTel.Text,
                nombre = txtNombre.Text,
                apellido = txtApellido.Text,
                contraseña = txtPass.Text,
            };
            errorFormRegistrar errores = validador.validarFormularioRegistrar(input);
            if (errores.Equals(default(errorFormRegistrar))) return true;

            lblErrorCorreo.Text = errores.email;
            lblErrorCorreo.Visible = true;
            lblErrorTel.Text = errores.telefono;
            lblErrorTel.Visible = true;
            lblErrorNombre.Text = errores.nombre;
            lblErrorNombre.Visible = true;
            lblErrorApellido.Text = errores.apellido;
            lblErrorApellido.Visible = true;
            lblErrorPass.Text = errores.contraseña;
            lblErrorPass.Visible = true;

            return false;
        }
        //=====================================================================================
       
    }
}