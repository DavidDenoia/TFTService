using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TFTService
{
    public class Ordinales
    {
        public static string ConvertirNumEnteroOrdinal(string numero, string genero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (tamañoNumero == 1)
            {
                int numeroInt = int.Parse(numero);
                if (numeroInt >= 1 && numeroInt <= 4)
                {
                    switch (numeroInt)
                    {
                        case 1: resultado.Insert(0, genero == "M" ? "Primer" : "Primera"); break;
                        case 2: resultado.Insert(0, genero == "M" ? "Segon" : "Segona"); break;
                        case 3: resultado.Insert(0, genero == "M" ? "Tercer" : "Tecera"); break;
                        case 4: resultado.Insert(0, genero == "M" ? "Quart" : "Quarta"); break;
                    }
                }
                else
                {
                    string numCard = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "què" : numCard.Substring(0, numCard.Length - 1) + "quena");
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 2) + "vè" : numCard.Substring(0, numCard.Length - 2) + "vena");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n") || numCard.EndsWith("l"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard + "è" : numCard + "ena");
                    }
                    else if (numCard.EndsWith("u"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "sè" : numCard.Substring(0, numCard.Length - 1) + "sena");
                    }
                    else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "è" : numCard.Substring(0, numCard.Length - 1) + "ena");
                    }
                    else if (numCard.EndsWith("ns"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "è" : numCard.Substring(0, numCard.Length - 1) + "ena");
                    }
                }
            }
            else
            {
                string numCard = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                if (numCard.EndsWith("c"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "què" : numCard.Substring(0, numCard.Length - 1) + "quena");
                }
                else if (numCard.EndsWith("ou"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vè" : numCard.Substring(0, numCard.Length - 2) + "vena");
                }
                else if (numCard.EndsWith("ns"))
                {
                    //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "è" : numCard.Substring(0, numCard.Length - 1) + "ena");
                }
                else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n") || numCard.EndsWith("l"))
                {
                    resultado.Insert(0, genero == "M" ? numCard + "è" : numCard + "ena");
                }
                else if (numCard.EndsWith("u"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "sè" : numCard.Substring(0, numCard.Length - 1) + "sena");
                }
                else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "è" : numCard.Substring(0, numCard.Length - 1) + "ena");
                }
               
            }
            System.Diagnostics.Debug.WriteLine("Numero ORDINAL: " + resultado.ToString());
            return resultado.ToString();
        }
    }

}