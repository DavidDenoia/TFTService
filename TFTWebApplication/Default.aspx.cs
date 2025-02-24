using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TFTWebApplication.ServiceReference1;
namespace TFTWebApplication
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptResultados.DataSource = null;
                rptResultados.DataBind();
            }

        }

        protected void Traducir_Click(object sender, EventArgs e)
        {
            try
            {
                string numero = txtNumero.Text;
                string lenguaje = "ca-CA";

                if (string.IsNullOrEmpty(numero))
                {
                    lblResultado.Text = "Introducir numero valido";
                    return;
                }

                using (var cliente = new NumToCatClient())
                {
                    List<Conversion> resultados = cliente.MainTraducir(numero, lenguaje).ToList();
                    if (resultados != null && resultados.Count > 0)
                    {
                        rptResultados.DataSource = resultados;
                        rptResultados.DataBind();
                        lblResultado.Text = "Conversion hecha";

                    }
                    else
                    {
                        lblResultado.Text = "No se encontraron conversiones";
                    }
                }
            }
            catch (Exception ex)
            {
                lblResultado.Text = "Error: " + ex.Message;
            }
        }
    }
}