using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TFTWebApplication.ServiceReference1;

namespace TFTWebApplication
{
    public partial class _Default : Page
    {

        private string idioma;



        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {



              
                panelBienvenida.Visible = true;
                panelError.Visible = false;
                rptResultados.Visible = false;




            }

        }
        protected override void InitializeCulture()
        {
            
            idioma = Session["idioma"]?.ToString() ?? "es-ES";
            CultureInfo cultura = new CultureInfo(idioma);
            Thread.CurrentThread.CurrentCulture = cultura;
            Thread.CurrentThread.CurrentUICulture = cultura;
            base.InitializeCulture();
        }



        protected void Traducir_Click(object sender, EventArgs e)
        {
            try
            {
                string numero = txtNumero.Text;
                


                if (string.IsNullOrEmpty(numero))
                {
                    lblResultado.Text = "Introducir numero valido";
                    panelError.Visible = true;
                    rptResultados.Visible = false;
                    return;
                }

              

                using (var cliente = new NumToCatClient())
                {
                    //List<Conversion> resultado = cliente.MainTraducir(numero, lenguaje);
                    var resultado = cliente.MainTraducir(numero, idioma);
                    //System.Diagnostics.Debug.WriteLine("RESULTADO RECIBIDO:" + resultado);
                    var cabecera = resultado.Item1;

                    var resultados = resultado.Item2;




                    if (resultados != null && resultados.Count > 0)
                    {
                        rptResultados.DataSource = resultados;
                        rptResultados.DataBind();
                        //lblResultado.Text = "Conversion hecha";

                        if (cabecera != null)
                        {
                            lblResultado.Text = cabecera.Titulo + ": " + cabecera.Formateado;
                        }
                        else
                        {
                            if (idioma == "es-ES")
                            {
                                lblResultado.Text = "Conversion hecha";
                            }
                            else
                            {
                                lblResultado.Text = "Conversió feta";
                            }
                            
                        }
                        
                        if(idioma == "es-ES")
                        {
                            lblTitulo.Text = $"¿Cómo se escribe {numero} en letras en catalan?";
                        }
                        else
                        {
                            lblTitulo.Text = $"Com s'escriu {numero} en lletres en catalan?";
                        }

                        
                        panelError.Visible = false;
                        rptResultados.Visible = true;
                        panelBienvenida.Visible = false;

                    }
                    else
                    {
                        if (cabecera != null)
                        {
                            lblResultado.Text = cabecera.Titulo + ": " + cabecera.Formateado;
                        }
                        else
                        {
                            if (idioma == "es-ES")
                            {
                                lblResultado.Text = "No se encontraron conversiones";
                            }
                            else
                            {
                                lblResultado.Text = "No es van trobar conversions";
                            }
                        }
                        //lblResultado.Text = "No se encontraron conversiones";
                        lblTitulo.Text = "";
                        panelError.Visible = true; 
                        rptResultados.Visible = false;
                        panelBienvenida.Visible = false;
                    }

                    
                    
                }
            }
            catch (Exception ex)
            {
                lblResultado.Text = "Error: " + ex.Message;
                panelError.Visible = true;
                rptResultados.Visible = false;
            }
        }


        protected void Limpiar_Click(object sender, EventArgs e)
        {
            txtNumero.Text = "";
        }

       protected void Ejemplo_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            txtNumero.Text = boton.CommandArgument;
            Traducir_Click(sender, e);

        }
    }
}