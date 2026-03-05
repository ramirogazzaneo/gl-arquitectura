using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static BIZ.estructuras;

namespace GL___C2G
{
    public partial class SiteMaster : MasterPage
    {

        private bool EsPaginaGeneral()
        {
            // Cubre AA_REGISTER, AA_LOGIN y cualquier otra dentro de /Z_INICIO/
            var path = Request.Path ?? string.Empty;
            return path.IndexOf("/ULTRA/INICIO/", StringComparison.OrdinalIgnoreCase) >= 0;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                liUserMenu.Visible = false;
                liPerfil.Visible = false;
                liObras.Visible = false;
                liPresupuestos.Visible = false;
                liMateriales.Visible = false;
                LiTareas.Visible = false;
                LiPagos.Visible = false;
                LiUser.Visible = false;
                liLogout.Visible = false;


                sbUsuarios.Visible = false;
                sbObras.Visible = false;
                sbPresupuestos.Visible = false;
                sbMateriales.Visible = false;
                sbPagos.Visible = false;
                sbTareas.Visible = false;


                if (EsPaginaGeneral())
                {
                    appSidebar.Visible = false;
                    appMain.Attributes["style"] = "margin-left:0";
                    return;

                }



                var u = Session["usuario"];
                if (u != null)
                {
                    var x = (usuario)u;


                    liUserMenu.Visible = true;
                    liPerfil.Visible = true;
                    liLogout.Visible = true;


                    bool esAdmin = x.nivel == 3;
                    bool esDesigner = x.nivel == 2;
                    sbUsuarios.Visible = esAdmin;
                    sbObras.Visible = true;
                    sbPresupuestos.Visible = true;
                    sbMateriales.Visible = true;
                    sbPagos.Visible = true;
                    sbTareas.Visible = true;


                    LiUser.Visible = false;
                    liObras.Visible = false;
                    liPresupuestos.Visible = false;
                    liMateriales.Visible = false;
                    LiTareas.Visible = false;
                    LiPagos.Visible = false;

                    ltUserName.Text = x.nombre.ToUpper();
                    hlHome.NavigateUrl = ResolveUrl(

                        esAdmin ? "~/ULTRA/PAGINAS/PAG_PRINCIPAL_ADM" : esDesigner ? "~/ULTRA/PAGINAS/PAG_PRINCIPAL_DES" : "~/ULTRA/PAGINAS/PAG_PRINCIPAL");
                }
            }

            MarcarActivoSidebar();

        }
       
        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/ULTRA/INICIO/REGISTER");
        }

        protected void lbtnProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ULTRA/USUARIOS/USUARIO");
        }

        protected void lbtnObras_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["nivel"]) == 1)
                Response.Redirect("/ULTRA/OBRAS/OBRAS");
            else
                Response.Redirect("/ULTRA/OBRAS/OBRAS_ADM");
        }

        protected void lbtnPresupuestos_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["nivel"]) == 1)
                Response.Redirect("/ULTRA/PRESUPUESTOS/PRESUPUESTOS_USR");
            else
                Response.Redirect("/ULTRA/PRESUPUESTOS/PRESUPUESTOS_ADM");
        }

        protected void lbtnUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ULTRA/USUARIOS/USUARIOS");
        }

        protected void lbtnTareas_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["nivel"]) == 1)
                Response.Redirect("/ULTRA/TAREAS/TAREAS_ETAPAS_USR");
            else
                Response.Redirect("/ULTRA/TAREAS/TAREAS_ETAPAS_ADM");
        }

        protected void lbtnMateriales_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["nivel"]) == 1)
                Response.Redirect("/ULTRA/MATERIALES/MATERIALES_USR");
            else
                Response.Redirect("/ULTRA/MATERIALES/MATERIALES_ADM");
        }

        protected void lbtnPagos_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["nivel"]) == 1)
                Response.Redirect("/ULTRA/FINANZAS/FINANZAS_USR");
            else
                Response.Redirect("/ULTRA/FINANZAS/FINANZAS_ADM");
        }

        protected void lbtnDocs_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["nivel"]) == 1)
                Response.Redirect("/ULTRA/DOCS/DOCS_USR");
            else
                Response.Redirect("/ULTRA/DOCS/DOCS_ADM");
        }

        private void MarcarActivoSidebar()
        {
            var path = (Request.Path ?? string.Empty).ToLowerInvariant();

            void Activar(HtmlGenericControl li)
            {
                if (li == null) return;
                var cls = li.Attributes["class"] ?? string.Empty;
                if (!cls.Contains("active"))
                    li.Attributes["class"] = (cls + " active").Trim();
            }


            if (path.Contains("/ultra/obras/")) Activar(sbObras);
            else if (path.Contains("/ultra/presupuestos/")) Activar(sbPresupuestos);
            else if (path.Contains("/ultra/materiales/")) Activar(sbMateriales);
            else if (path.Contains("/ultra/finanzas/")) Activar(sbPagos);
            else if (path.Contains("/ultra/tareas/")) Activar(sbTareas);
            else if (path.Contains("/ultra/docs/")) Activar(sbDocumentos);
            else if (path.Contains("/ultra/usuarios/")) Activar(sbUsuarios);
        }

    }
}


