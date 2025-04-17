using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TFTWebApplication
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["idioma"] == null)
            {
                Session["idioma"] = "es-ES";
            }
        }

        protected void btnEspanol_Click(object sender, EventArgs e)
        {
            Session["idioma"] = "es-ES";
            Response.Redirect("~/Default.aspx");

        }

        protected void btnCatalan_Click(object sender, EventArgs e)
        {
            Session["idioma"] = "ca-ES";
            Response.Redirect("~/Default.aspx");

        }

        





    }
}
