using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TFTService
{
    public class Fraccionario
    {
        public static string ConvertirNumEnteroFracDenominador(string numero, string genero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (Regex.IsMatch(numero, @"^10*$"))
            {
                string denominador = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                if(denominador == "deu")
                {
                    denominador = genero == "M" ? "dècim" : "dècima";
                }
                else if(denominador== "cent")
                {
                    denominador = genero == "M" ? "centèsim" : "centèsima";

                }else if(denominador == "mil")
                {
                    denominador = genero == "M" ? "mil·lèsim" : "mil·lèsima";
                }
                else
                {
                    denominador = Cardinales.ConvertirNumDecimalCardinal(numero);
                }
                resultado.Insert(0,denominador);
                return resultado.ToString();
            }

            if (tamañoNumero <= 2)
            {

                int numeroInt = int.Parse(numero);
                if (numeroInt == 0) 
                {
                    resultado.Insert(0, "partit per zero");
                }else if(numeroInt == 1)
                {
                    resultado.Insert(0, "partit per un");
                }
                else if (numeroInt >= 2 && numeroInt <= 4 || numeroInt == 10)
                {
                    switch (numeroInt)
                    {
                        case 2: resultado.Insert(0, genero == "M" ? "mig" : "mitja"); break;
                        case 3: resultado.Insert(0, genero == "M" ? "terç" : "tercera"); break;
                        case 4: resultado.Insert(0, genero == "M" ? "quart" : "quarta"); break;
                        case 10: resultado.Insert(0, genero == "M" ? "dècim" : "dècima"); break;
                    }
                }
                else
                {
                    string numCard = Cardinales.ConvertirNumEnteroCardinal(numero,false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1)+"què" : numCard.Substring(0, numCard.Length - 1) + "quena");
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vè" : numCard.Substring(0, numCard.Length - 1) + "vena");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard + "è" : numCard + "ena");
                    }
                    else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "è" : numCard.Substring(0, numCard.Length - 1) + "ena");
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

        public static string ConvertirNumEnteroFraccionario(string numero, string genero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (Regex.IsMatch(numero, @"^10+$"))
            {
                string denominador = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                if (denominador == "deu")
                {
                    
                    denominador = "dècim";
                }
                else if (denominador == "cent")
                {
                    denominador = "centèsim";

                }
                else if (denominador == "mil")
                {
                    denominador = "mil·lèsim";
                }
                else
                {
                    denominador = Cardinales.ConvertirNumDecimalCardinal(numero);
                }
                resultado.Insert(0, genero == "M" ? denominador : denominador+"a");
                return resultado.ToString();
            }

            if (tamañoNumero <= 2)
            {

                int numeroInt = int.Parse(numero);
                
                if (numeroInt >= 0 && numeroInt <= 4)
                {
                    switch (numeroInt)
                    {
                        case 0: return null;
                        case 1: resultado.Insert(0, genero == "M" ? "unitat" : "unitat"); break;
                        case 2: resultado.Insert(0, genero == "M" ? "mig" : "mitja"); break;
                        case 3: resultado.Insert(0, genero == "M" ? "terç" : " terça"); break;
                        case 4: resultado.Insert(0, genero == "M" ? "quart" : "quarta"); break;
                            //case 10: resultado.Insert(0, "dècim"); break;
                    }
                }
                else
                {
                    string numCard = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "quesim" : numCard.Substring(0, numCard.Length - 1) + "quesima");
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                    }
                    else if (numCard.EndsWith("ns"))
                    {
                        //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "èsim" : numCard.Substring(0, numCard.Length - 1) + "èsima");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard + "èsim" : numCard + "èsima");
                    }
                    else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                    }

                }
            }
            else
            {
                string numCard = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                if (numCard.EndsWith("c"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "quesim" : numCard.Substring(0, numCard.Length - 1) + "quesima");
                }
                else if (numCard.EndsWith("ou"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                }
                else if (numCard.EndsWith("ns"))
                {
                    //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "èsim" : numCard.Substring(0, numCard.Length - 1) + "èsima");
                }
                else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                {
                    resultado.Insert(0, genero == "M" ? numCard + "èsim" : numCard + "èsima");
                }
                else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                }
                else if (numCard.EndsWith("mil")){
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "·lèsim" : numCard.Substring(0, numCard.Length - 1) + "·lèsima");
                }
            }

            return resultado.ToString();
        }

        public static string ConvertirNumEnteroFracDenominadorVal(string numero, string genero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (Regex.IsMatch(numero, @"^10*$"))
            {
                string denominador = Cardinales.ConvertirNumEnteroCardinalVal(numero, false);
                if (denominador == "deu")
                {
                    denominador = genero == "M" ? "dècim" : "dècima";
                }
                else if (denominador == "cent")
                {
                    denominador = genero == "M" ? "centèsim" : "centèsima";

                }
                else if (denominador == "mil")
                {
                    denominador = genero == "M" ? "mil·lèsim" : "mil·lèsima";
                }
                else
                {
                    denominador = Cardinales.ConvertirNumDecimalCardinalVal(numero);
                }
                resultado.Insert(0, denominador);
                return resultado.ToString();
            }

            if (tamañoNumero <= 2)
            {

                int numeroInt = int.Parse(numero);
                if (numeroInt == 0)
                {
                    resultado.Insert(0, "partit per zero");
                }
                else if (numeroInt == 1)
                {
                    resultado.Insert(0, "partit per un");
                }
                else if (numeroInt >= 2 && numeroInt <= 4 || numeroInt == 10)
                {
                    switch (numeroInt)
                    {
                        case 2: resultado.Insert(0, genero == "M" ? "mig" : "mitja"); break;
                        case 3: resultado.Insert(0, genero == "M" ? "terç" : "tercera"); break;
                        case 4: resultado.Insert(0, genero == "M" ? "quart" : "quarta"); break;
                        case 10: resultado.Insert(0, genero == "M" ? "dècim" : "dècima"); break;
                    }
                }
                else
                {
                    string numCard = Cardinales.ConvertirNumEnteroCardinalVal(numero, false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "què" : numCard.Substring(0, numCard.Length - 1) + "quena");
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vè" : numCard.Substring(0, numCard.Length - 1) + "vena");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard + "è" : numCard + "ena");
                    }
                    else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "è" : numCard.Substring(0, numCard.Length - 1) + "ena");
                    }

                }
            }
            else
            {
                string numCard = Cardinales.ConvertirNumEnteroCardinalVal(numero, false);
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

        public static string ConvertirNumEnteroFraccionarioVal(string numero, string genero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (Regex.IsMatch(numero, @"^10+$"))
            {
                string denominador = Cardinales.ConvertirNumEnteroCardinalVal(numero, false);
                if (denominador == "deu")
                {

                    denominador = "dècim";
                }
                else if (denominador == "cent")
                {
                    denominador = "centèsim";

                }
                else if (denominador == "mil")
                {
                    denominador = "mil·lèsim";
                }
                else
                {
                    denominador = Cardinales.ConvertirNumDecimalCardinalVal(numero);
                }
                resultado.Insert(0, genero == "M" ? denominador : denominador + "a");
                return resultado.ToString();
            }

            if (tamañoNumero <= 2)
            {

                int numeroInt = int.Parse(numero);

                if (numeroInt >= 0 && numeroInt <= 4)
                {
                    switch (numeroInt)
                    {
                        case 0: return null;
                        case 1: resultado.Insert(0, genero == "M" ? "unitat" : "unitat"); break;
                        case 2: resultado.Insert(0, genero == "M" ? "mig" : "mitja"); break;
                        case 3: resultado.Insert(0, genero == "M" ? "terç" : " terça"); break;
                        case 4: resultado.Insert(0, genero == "M" ? "quart" : "quarta"); break;
                            //case 10: resultado.Insert(0, "dècim"); break;
                    }
                }
                else
                {
                    string numCard = Cardinales.ConvertirNumEnteroCardinalVal(numero, false);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "quesim" : numCard.Substring(0, numCard.Length - 1) + "quesima");
                    }
                    else if (numCard.EndsWith("ou"))
                    {

                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                    }
                    else if (numCard.EndsWith("ns"))
                    {
                        //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "èsim" : numCard.Substring(0, numCard.Length - 1) + "èsima");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard + "èsim" : numCard + "èsima");
                    }
                    else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                    }

                }
            }
            else
            {
                string numCard = Cardinales.ConvertirNumEnteroCardinalVal(numero, false);
                if (numCard.EndsWith("c"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "quesim" : numCard.Substring(0, numCard.Length - 1) + "quesima");
                }
                else if (numCard.EndsWith("ou"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                }
                else if (numCard.EndsWith("ns"))
                {
                    //System.Diagnostics.Debug.WriteLine("Numero CARDINAL: " + numCard);
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "èsim" : numCard.Substring(0, numCard.Length - 1) + "èsima");
                }
                else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
                {
                    resultado.Insert(0, genero == "M" ? numCard + "èsim" : numCard + "èsima");
                }
                else if (numCard.EndsWith("a") || numCard.EndsWith("e"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vèsim" : numCard.Substring(0, numCard.Length - 1) + "vèsima");
                }
                else if (numCard.EndsWith("mil"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "·lèsim" : numCard.Substring(0, numCard.Length - 1) + "·lèsima");
                }
            }

            return resultado.ToString();
        }
    }
}
