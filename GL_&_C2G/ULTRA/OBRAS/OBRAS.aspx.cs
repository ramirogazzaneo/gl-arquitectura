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
    public partial class WebForm5 : System.Web.UI.Page
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
                }
                
                BindGrid();
            }
        }
        #endregion
        //==========================
        //binds
        //==========================
        #region
        private void BindGrid()
        {
            int clienteID = int.Parse(Session["id"].ToString());
            List<Obra> obraS = BIZ.B_obras.traerObra(clienteID);
            GridView1.DataSource = obraS;
            GridView1.DataBind();
        }

        #endregion
    }
}