﻿using System;
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
        private string idioma = "ca-CA";



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptResultados.DataSource = null;
                rptResultados.DataBind();
                
                panelBienvenida.Visible = true;

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
                    panelError.Visible = true;
                    rptResultados.Visible = false;
                    return;
                }

                using (var cliente = new NumToCatClient())
                {
                    List<Conversion> resultados = cliente.MainTraducir(numero, lenguaje);
                    
                    if (resultados != null && resultados.Count > 0)
                    {
                        rptResultados.DataSource = resultados;
                        rptResultados.DataBind();
                        lblResultado.Text = "Conversion hecha";

                        lblTitulo.Text = $"¿Cómo se escribe {numero} en letras en catalan?";
                        panelError.Visible = false;
                        rptResultados.Visible = true;

                    }
                    else
                    {
                        lblResultado.Text = "No se encontraron conversiones";
                        lblTitulo.Text = "";
                        panelError.Visible = true; 
                        rptResultados.Visible = false;
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

        protected void Leer_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string texto = btn.CommandArgument;
            string script = $@"
            <script>
                let speech = new SpeechSynthesisUtterance('{HttpUtility.JavaScriptStringEncode(texto)}');
                speech.lang = '{idioma}';
                window.speechSynthesis.speak(speech);
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "SpeechScript", script, false);
        } 

        protected void Copiar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string texto = btn.CommandArgument;
            string script = $@"
            <script>
                navigator.clipboard.writeText('{HttpUtility.JavaScriptStringEncode(texto)}');
            </script>    
            ";

            ClientScript.RegisterStartupScript(this.GetType(), "SpeechScript", script, false);
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