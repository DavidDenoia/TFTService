using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TFTService
{
    public class Fraccionario
    {
        public static string ConvertirNumEnteroFrac(string numero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (tamañoNumero <= 2)
            {

                int numeroInt = int.Parse(numero);
                if (numeroInt == 0 || numeroInt == 1)
                {
                    resultado.Insert(0, "");
                }
                else if (numeroInt >= 2 && numeroInt <= 4 || numeroInt == 10)
                {
                    switch (numeroInt)
                    {
                        case 2: resultado.Insert(0, "mig"); break;
                        case 3: resultado.Insert(0, "terç"); break;
                        case 4: resultado.Insert(0, "quart"); break;
                        case 10: resultado.Insert(0, "dècim"); break;
                    }
                }
                else
                {
                    string numCard = Cardinales.ConvertirNumEnteroCardinal(numero,false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, numCard.Substring(0, numCard.Length - 1));
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        resultado.Insert(0, numCard.Substring(0, numCard.Length - 1) + "vè");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                    {
                        resultado.Insert(0, numCard + "è");
                    }
                    else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                    {
                        resultado.Insert(0, numCard.Substring(0, numCard.Length - 1) + "è");
                    }

                }
            }
            else
            {
                string numCard = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                if (numCard.EndsWith("c"))
                {
                    resultado.Insert(0, numCard.Substring(0, numCard.Length - 1));
                }
                else if (numCard.EndsWith("ou"))
                {
                    resultado.Insert(0, numCard.Substring(0, numCard.Length - 1) + "vè");
                }
                else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                {
                    resultado.Insert(0, numCard + "è");
                }
                else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                {
                    resultado.Insert(0, numCard.Substring(0, numCard.Length - 1) + "è");
                }
            }

            return resultado.ToString();
        }
    }
}