using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace TFTService
{
    public class FormateoNumero
    {
        public static string FormatearNumero(string numero)
        {
            string[] partes = numero.Split('.');
            string parteEntera = partes[0];
            if (string.IsNullOrEmpty(parteEntera)) parteEntera = "0";

            string parteEnteraFormateada = "";
            int contador = 0;

            for (int i = parteEntera.Length - 1; i >= 0; i--)
            {
                parteEnteraFormateada = parteEntera[i] + parteEnteraFormateada;
                contador++;
                if (contador % 3 == 0 && i != 0)
                {
                    parteEnteraFormateada = " " + parteEnteraFormateada;
                }
            }

            if (partes.Length > 1)
            {
                string parteDecimal = partes[1];
                string parteDecimalFormateada = "";
                for (int i = 0; i < parteDecimal.Length; i++)
                {
                    parteDecimalFormateada += parteDecimal[i];
                    if ((i + 1) % 3 == 0 && i != parteDecimal.Length - 1)
                    {
                        parteDecimalFormateada += " ";
                    }
                }

                return parteEnteraFormateada + "." + parteDecimalFormateada;
            }
            else
            {
                return parteEnteraFormateada;
            }
        }

    }
}