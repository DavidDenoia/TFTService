using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TFTService
{
    public class Ordinales
    {
        public static string ConvertirNumEnteroOrdinal(string numero, string genero, bool plural)
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
                        case 0: return null;
                        case 1:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "primer" : "primera"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "primers" : "primeres"); break;
                            }
                        case 2:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "segon" : "segona"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "segons" : "segones"); break;
                            }
                        case 3:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "tercer" : "tecera"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "tercers" : "teceres"); break;
                            }
                        case 4:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "quart" : "quarta"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "quarts" : "quartes"); break;
                            }
                    }
                }
                else
                {
                    string numCard = Cardinales.NuevoConvertirNumEnteroCardinal(numero, false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "què" : numCard.Substring(0, numCard.Length - 1) + "quena");
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vè" : numCard.Substring(0, numCard.Length - 1) + "vena");
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
                string numCard = Cardinales.NuevoConvertirNumEnteroCardinal(numero, false);
                //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                if (string.IsNullOrEmpty(numCard))
                {
                    return null;
                }
                else if (numCard.EndsWith("c"))
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
            if (plural)
            {
                if (resultado.ToString().EndsWith("è"))
                {
                    resultado.Replace("è", "ens");
                }
                else if (resultado.ToString().EndsWith("ena"))
                {
                    resultado.Replace("ena", "enes");
                }
            }
            return resultado.ToString();
        }

        public static string ConvertirNumEnteroOrdinalVal(string numero, string genero, bool plural)
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
                        case 0: return null;
                        case 1:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "primer" : "primera"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "primers" : "primeres"); break;
                            }
                        case 2:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "segon" : "segona"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "segons" : "segones"); break;
                            }
                        case 3:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "tercer" : "tecera"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "tercers" : "teceres"); break;
                            }
                        case 4:
                            if (!plural)
                            {
                                resultado.Insert(0, genero == "M" ? "quart" : "quarta"); break;
                            }
                            else
                            {
                                resultado.Insert(0, genero == "M" ? "quarts" : "quartes"); break;
                            }
                    }
                }
                else
                {
                    string numCard = Cardinales.NuevoConvertirNumEnteroCardinalVal(numero, false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "qué" : numCard.Substring(0, numCard.Length - 1) + "quena");
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vé" : numCard.Substring(0, numCard.Length - 1) + "vena");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n") || numCard.EndsWith("l"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard + "é" : numCard + "ena");
                    }
                    else if (numCard.EndsWith("u"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "sé" : numCard.Substring(0, numCard.Length - 1) + "sena");
                    }
                    else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "é" : numCard.Substring(0, numCard.Length - 1) + "ena");
                    }
                    else if (numCard.EndsWith("ns"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "é" : numCard.Substring(0, numCard.Length - 1) + "ena");
                    }
                }
            }
            else
            {
                string numCard = Cardinales.NuevoConvertirNumEnteroCardinalVal(numero, false);
                //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                if (numCard.EndsWith("c"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "qué" : numCard.Substring(0, numCard.Length - 1) + "quena");
                }
                else if (numCard.EndsWith("ou"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vé" : numCard.Substring(0, numCard.Length - 2) + "vena");
                }
                else if (numCard.EndsWith("ns"))
                {
                    //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "é" : numCard.Substring(0, numCard.Length - 1) + "ena");
                }
                else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n") || numCard.EndsWith("l"))
                {
                    resultado.Insert(0, genero == "M" ? numCard + "é" : numCard + "ena");
                }
                else if (numCard.EndsWith("u"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "sé" : numCard.Substring(0, numCard.Length - 1) + "sena");
                }
                else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "é" : numCard.Substring(0, numCard.Length - 1) + "ena");
                }

            }

            if (plural)
            {
                if (resultado.ToString().EndsWith("é"))
                {
                    resultado.Replace("é", "ens");
                }
                else if (resultado.ToString().EndsWith("ena"))
                {
                    resultado.Replace("ena", "enes");
                }
            }
            System.Diagnostics.Debug.WriteLine("Numero ORDINAL VAL: " + resultado.ToString());
            return resultado.ToString();
        }
    }

}