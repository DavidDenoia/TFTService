using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using TFTService;
using System.Resources;
using System.Numerics;
using System.Web.Script.Serialization;
using System.Diagnostics.Eventing.Reader;



namespace TFTService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class TFTServicio : INumToCat
    {
        public PartesNumeros GetNumber(string value)
        {

            bool tieneComa = false;

            if (String.IsNullOrEmpty(value))
            {
                return new PartesNumeros { ParteEntera = "", ParteDecimal = "" };
            }

            for (int i = 0; i < value.Length; i++)
            {
                char caracter = value[i];

                if (!char.IsDigit(caracter) && caracter != ',')
                {
                    return new PartesNumeros { ParteEntera = "", ParteDecimal = "" };

                }

                if (caracter == ',')
                {
                    tieneComa = true;
                }

            }

            if (tieneComa)
            {
                string[] partes = value.Split(',');

                if (partes.Length != 2)
                {

                    return new PartesNumeros { ParteEntera = "", ParteDecimal = "" };
                }

                string parteEntera = partes[0];
                string parteDecimal = partes[1];

                return new PartesNumeros { ParteEntera = parteEntera, ParteDecimal = parteDecimal };

            }

            return new PartesNumeros { ParteEntera = value, ParteDecimal = "" };
        }

        public string CentenaresALetras(string numero)
        {
            StringBuilder resultado = new StringBuilder();
            string[] unidades = { "zero", "un", "dos", "tres", "quatre", "cinc", "sis", "set", "vuit", "nou" };
            string[] decenas = { "trenta", "qauranta", "cinquanta", "seixanta", "setanta", "vuitanta", "noranta" };
            string[] centenas = { "cent ", "dos-cents ", "tres-cents ", "quatre-cents ", "cinc-cents ", "sis-cents ", "set-cents ", "vuit-cents ", "nou-cents " };
            string[] decenasEspeciales = {"deu","onze","dotze","tretze","catorze", "quinze","setze","disset","divuit","dinou",
                "vint","vint-i-un","vint-i-dos","vint-i-tres","vint-i-quatre","vint-i-cinc", "vint-i-sis","vint-i-set","vint-i-vuit","vint-i-nou"};
            int numInt = int.Parse(numero);
            if (numInt == 0)
            {
                resultado.Append(unidades[0]);
            }

            int numIntCent = numInt / 100;
            int numIntDec = (numInt % 100) / 10;
            int numIntUni = (numInt % 10);
            int numIntDecUni = numInt % 100;

            if (numIntCent > 0)
            {
                resultado.Append(centenas[numIntCent - 1]);
            }
            if (numIntDecUni >= 10 && numIntDecUni <= 29)
            {
                resultado.Append(decenasEspeciales[numIntDecUni - 10]);
            }
            else
            {
                if (numIntDec >= 3)
                {
                    resultado.Append(decenas[numIntDec - 3]);
                    if (numIntUni > 0)
                    {
                        resultado.Append("-");
                    }
                }
                if (numIntUni > 0)
                {
                    resultado.Append(unidades[numIntUni]);
                }
            }
            return resultado.ToString().Trim();

        }

        public string NumCompletoALetraCard(string numero)
        {
            int contadorSufijos = 0;
            int contadorNumVeces = 0;
            StringBuilder numCentena = new StringBuilder();
            StringBuilder resultado = new StringBuilder();

            string[] sufijos = { "milions", "bilions", "trilions", "quadrilions", "quintilions", "sextilions", "septilions", "octilions", "nonilions", "decilions",
        "undecilions", "duodecilions", "tredecilions", "quatourdecilions", "quindecilions", "sexdecilions", "septendecilions", "octodecilions", "novendecilions", "vigintilions" };

            string[] sufijosEspeciales = { "milió", "bilió", "trilió", "quadrilió", "quintilió", "sextilió", "septilió", "octilió", "nonilió", "decilió",
        "undecilió", "duodecilió", "tredecilió", "quatuordecilió", "quindecilió", "sexdecilió", "septendecilió", "octodecilió", "novendecilió", "vigintilió" };

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                numCentena.Insert(0, numero[i]);
                contadorNumVeces++;

                if (contadorNumVeces == 3 || i == 0)
                {
                    string numCentenaLetra = CentenaresALetras(numCentena.ToString());
                    numCentena.Clear();
                    contadorNumVeces = 0;

                    if (!string.IsNullOrEmpty(numCentenaLetra) && !numCentenaLetra.Equals("zero"))
                    {
                        if (contadorSufijos == 0)
                        {
                            resultado.Insert(0, numCentenaLetra + " ");
                        }
                        else if (contadorSufijos % 2 != 0)
                        {
                            int sufijoIndice = (contadorSufijos / 2) - 1;
                            if (sufijoIndice >= 0)
                            {
                                if (numCentenaLetra != "un")
                                {
                                    resultado.Insert(0, numCentenaLetra + " mil " + sufijos[sufijoIndice] + " ");
                                }
                                else
                                {
                                    resultado.Insert(0, " mil " + sufijos[sufijoIndice] + " ");
                                }

                            }
                            else
                            {
                                if (numCentenaLetra != "un")
                                {
                                    resultado.Insert(0, numCentenaLetra + " mil ");
                                }
                                else
                                {
                                    resultado.Insert(0, " mil ");
                                }

                            }

                        }
                        else if (numCentenaLetra != "un")
                        {
                            int sufijoIndice = (contadorSufijos / 2) - 1;
                            resultado.Insert(0, numCentenaLetra + " " + sufijos[sufijoIndice] + " ");
                        }
                        else
                        {
                            int sufijoIndice = (contadorSufijos / 2) - 1;
                            resultado.Insert(0, numCentenaLetra + " " + sufijosEspeciales[sufijoIndice] + " ");
                        }

                    }
                    contadorSufijos++;

                }
            }
            if (string.IsNullOrEmpty(resultado.ToString()))
            {
                resultado.Insert(0, "zero");
            }
            return resultado.ToString().Trim();
        }

        public string NumCompletoALetraOrd(string numero, string genero)
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
                    string numCard = NumCompletoALetraCard(numero);
                    if (numCard.EndsWith("c"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "què" : numCard.Substring(0, numCard.Length - 1) + "quena");
                    }
                    else if (numCard.EndsWith("ou"))
                    {
                        resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 2) + "vè" : numCard.Substring(0, numCard.Length - 2) + "vena");
                    }
                    else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
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
            }
            else
            {
                string numCard = NumCompletoALetraCard(numero);
                if (numCard.EndsWith("c"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "què" : numCard.Substring(0, numCard.Length - 1) + "quena");
                }
                else if (numCard.EndsWith("ou"))
                {
                    resultado.Insert(0, genero == "M" ? numCard.Substring(0, numCard.Length - 1) + "vè" : numCard.Substring(0, numCard.Length - 2) + "vena");
                }
                else if (numCard.EndsWith("s") || numCard.EndsWith("t") || numCard.EndsWith("n"))
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
            return resultado.ToString();
        }

        public string NumCompletoALetraFrac(string numero)
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
                        case 2: resultado.Insert(0, "meitat/mig"); break;
                        case 3: resultado.Insert(0, "terç"); break;
                        case 4: resultado.Insert(0, "quart"); break;
                        case 10: resultado.Insert(0, "dècim"); break;
                    }
                }
                else
                {
                    string numCard = NumCompletoALetraCard(numero);
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
                string numCard = NumCompletoALetraCard(numero);
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

        public string NumCompletoALetraMult(string numero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (tamañoNumero <= 3)
            {

                int numeroInt = int.Parse(numero);
                if (numeroInt == 0 || numeroInt == 1)
                {
                    resultado.Insert(0, "");
                }
                else if (numeroInt == 2 || numeroInt == 3)
                {
                    switch (numeroInt)
                    {
                        case 2: resultado.Insert(0, "Doble"); break;
                        case 3: resultado.Insert(0, "Triple"); break;
                    }
                }
                else if (numeroInt >= 4 && numeroInt <= 12)
                {
                    switch (numeroInt)
                    {
                        case 4: resultado.Insert(0, "Quàdruple"); break;
                        case 5: resultado.Insert(0, "Quìntuple"); break;
                        case 6: resultado.Insert(0, "Sèxtuple"); break;
                        case 7: resultado.Insert(0, "Sèptuple"); break;
                        case 8: resultado.Insert(0, "Òctuple"); break;
                        case 9: resultado.Insert(0, "Nonùple"); break;
                        case 10: resultado.Insert(0, "Dècuple"); break;
                        case 11: resultado.Insert(0, "Undècuple"); break;
                        case 12: resultado.Insert(0, "Duodècuple"); break;
                    }
                }
                else if(numeroInt == 100)
                {
                    resultado.Insert(0, "Cèntuple");
                }
                else
                {
                    string NumCompletoCard = NumCompletoALetraCard(numero);
                    resultado.Insert(0, NumCompletoCard + " vegades");
                }

            }
            else
            {
                string NumCompletoCard = NumCompletoALetraCard(numero);
                resultado.Insert(0, NumCompletoCard + " de vegades");
            }
            return resultado.ToString();
        }

        public string NumCompletoALetraCardDec(string numero)
        {
            
            StringBuilder resultado = new StringBuilder();
            StringBuilder numCentena = new StringBuilder();

            Dictionary<int,string> sufijosDecimales = new Dictionary<int, string>
                {
                    { 1, "dècim" }, { 2, "centèsim" }, { 3, "mil·lèsim" }, { 6, "milionèsim" },
                    { 12, "bilionèsim" }, { 18, "trilionèsim" }, { 24, "quadrilionèsim" },
                    { 30, "quintilionèsim" }, { 36, "sextilionèsim" }, { 42, "septilionèsim" },
                    { 48, "octilionèsim" }, { 54, "nonilionèsim" }, { 60, "decilionèsim" },
                    { 66, "undecilionèsim" }, { 72, "duodecilionèsim" }, { 78, "tredecilionèsim" },
                    { 84, "quatuordecilionèsim" }, { 90, "quindecilionèsim" }, {96 , "sexdecilionèsim" },
                    { 102, "septendecilionèsim" }, { 108, "octodecilionèsim" }, { 114, "novendecilionèsim" },
                    { 120, "vigintilionèsim" }
                 };

            int longitudNumero = numero.Length;
            int claveSeleccionada = 1;

            foreach (var key in sufijosDecimales.Keys)
            {
                if (key <= longitudNumero)
                {
                    claveSeleccionada = key;
                }
                else
                {
                    break;
                }
            }

            string sufijoBase = sufijosDecimales[claveSeleccionada];
            StringBuilder sufijoFinal = new StringBuilder();    

            int diferencia = longitudNumero - claveSeleccionada;    
            if(diferencia == 1)
            {
                sufijoFinal.Append("deu ");
            }
            else if (diferencia == 2) // 5, 8, 14, etc.
            {
                sufijoFinal.Append("cent ");
            }
            else if (diferencia == 3) // 6, 9, 15, etc.
            {
                sufijoFinal.Append("mil ");
            }
            else if (diferencia == 4) // 10, 16, 22, etc.
            {
                sufijoFinal.Append("deu mil ");
            }
            else if (diferencia == 5) // 11, 17, 23, etc.
            {
                sufijoFinal.Append("cent mil ");
            }

            sufijoFinal.Append(sufijoBase);

            string NumCompleCard = NumCompletoALetraCard(numero);
            if(NumCompleCard == "un")
            {
                resultado.Insert(0, NumCompleCard + " " + sufijoFinal.ToString());
            }
            else
            {
                resultado.Insert(0, NumCompleCard + " "+sufijoFinal.ToString()+"es");
            }
          
            //return resultado.ToString().Trim();
            return resultado.ToString();
        }


        private CultureInfo language;
        List<Conversion> conversiones = new List<Conversion>();//Aqui vamos a meter todas las conversiones
        public List<Conversion> MainTraducir(string value, string lenguaje)
        {
            if (lenguaje != null)
            {
                if (lenguaje.IndexOf("-") > -1) lenguaje = lenguaje.Substring(0, lenguaje.IndexOf("-"));
                language = new CultureInfo(lenguaje);
            }
            else language = new CultureInfo("es");
            Thread.CurrentThread.CurrentUICulture = language;

            string numero = value.Trim();
            int longitudNumero = numero.Length;
            Boolean signo = false;
            //Boolean decima = false;
            //Comprobacion de signo 
           if (numero.StartsWith("-") && numero.Length > 1 && char.IsDigit(numero[1]))
           {
                signo = true;
                numero = numero.Substring(1);
           }

            //Comprobacion de si es decimal
            if (Regex.IsMatch(numero, @"^([+-]?)\d+([.,])\d+$"))
            {
                //decima = true;
                conversiones.Add(ConversionDecimal(numero, signo, value, false));
            }

            //Comprobacion de si es fraccionario
            if(Regex.IsMatch(numero, @"^([+-]?)\d+([/])\d+$"))
            {
                string[] partes = numero.Split('/');
                string numerador = partes[0];
                string denominador = partes[1];

                conversiones.Add(ConversionFraccion(numerador,denominador, signo, value));
                
                BigInteger.TryParse(numerador, out BigInteger Inumerador);
                BigInteger.TryParse(denominador, out BigInteger Idenominador);
                //BigInteger Inumerador = BigInteger.Parse(numerador);
                //BigInteger Idenominador = BigInteger.Parse(denominador);
                if(Idenominador != 0)
                {
                    BigInteger parteEntera = Inumerador / Idenominador;
                    BigInteger resto = Inumerador % Idenominador;
                    if (resto == 0)
                    {
                        if (signo == false)
                        {                            
                            conversiones.Add(ConversionCardinal(parteEntera.ToString(), signo, value, true));
                        }
                        else
                        {
                            string parteEnteraNoSigno = parteEntera.ToString().Replace("-","").Trim();
                            conversiones.Add(ConversionNegativo(parteEnteraNoSigno, signo, value, true));
                        }
                        
                    }
                    else
                    {
                        string numeroDecimal = parteEntera.ToString() + ".";
                        int contadorDecimales = 0;

                        while (resto != 0 && contadorDecimales < 15)
                        {
                            resto *= 10;
                            BigInteger cociente = resto / Idenominador;
                            numeroDecimal += cociente.ToString();
                            resto %= Idenominador;
                            contadorDecimales++;
                        }
                            
                        //System.Diagnostics.Debug.WriteLine("Valor de la parte entera: " + parteEntera.ToString());
                        //System.Diagnostics.Debug.WriteLine("Valor del resto: " + resto.ToString());
                        //System.Diagnostics.Debug.WriteLine("Numero decimal: " + numeroDecimal);

                        conversiones.Add(ConversionDecimal(numeroDecimal, signo, value, true));

                    }
                }
              
                
                

            }


            //Comprobacion de si es numero con anotacion cientifica
            //Match match = Regex.Match(numero, @"^([+-]?\d+(?:[.,]\d+)?)[Ee]([+-]?\d+)$");
            Match match = Regex.Match(numero, @"^([+-]?\d*[.,]?\d+)[Ee]([+-]?\d+)$");
            if (match.Success)
            {
                string[] partes = numero.Split(new char[] { 'E', 'e' });

                string baseNum = partes[0];  // Parte antes de la 'E'
                System.Diagnostics.Debug.WriteLine("BASENUM: " + baseNum);
                int exponente = int.Parse(partes[1]); // Exponente
                System.Diagnostics.Debug.WriteLine("EXPONENTE: " + exponente);
                if (exponente < -128 || exponente > 128)
                {
                    throw new ArgumentOutOfRangeException("Exponente fuera del rango");
                }
                else if (exponente >= 0)
                {
                    string numeroExpandido = NotacionCientifica.ExpandirNotacionCientificaPositiva(baseNum, exponente);
                    if (numeroExpandido.Contains('.'))
                    {
                        conversiones.Add(ConversionDecimal(numeroExpandido, signo, value, true));
                    }
                    else if (signo == true)
                    {
                        conversiones.Add(ConversionNegativo(numeroExpandido, signo, value, true));

                    }
                    else
                    {
                        conversiones.Add(ConversionCardinal(numeroExpandido, signo, value, true));
                        conversiones.Add(ConversionOrdinal(numeroExpandido, signo, value, false));
                        conversiones.Add(ConversionFraccionario(numeroExpandido, signo, value, false));
                        conversiones.Add(ConversionMultiplicativo(numeroExpandido, signo, value, false));
                    }
                }
                else
                {
                    string numeroExpandido = NotacionCientifica.ExpandirNotacionCientificaNegativa(baseNum, exponente);
                    if (numeroExpandido.Contains('.'))
                    {
                        conversiones.Add(ConversionDecimal(numeroExpandido, signo, value, true));
                    }
                    else if (signo == true)
                    {
                        conversiones.Add(ConversionNegativo(numeroExpandido, signo, value, true));
                    }
                    else
                    {
                        conversiones.Add(ConversionCardinal(numeroExpandido, signo, value, true));
                        conversiones.Add(ConversionOrdinal(numeroExpandido, signo, value, false));
                        conversiones.Add(ConversionFraccionario(numeroExpandido, signo, value, false));
                        conversiones.Add(ConversionMultiplicativo(numeroExpandido, signo, value, false));
                    }
                }
               
            }

            return conversiones;
        }

        public Conversion ConversionFraccion(string pNumerador,string pDenominador, bool signo, string numeroOriginal)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();
       
            string numerador = Cardinales.ConvertirNumEnteroCardinal(pNumerador, signo);
            string denominador = Fraccionario.ConvertirNumEnteroFracDenominador(pDenominador, "M");

            string denominadorAdj = "";
            string numeradorAdj = "";
            if(numerador == "un")
            {
                numeradorAdj = numerador + "a";
            }else if (numerador == "dos")
            {
                numeradorAdj = "dues";
                if(denominador == "terç")
                {
                    denominador = "tercos";
                }else if(denominador.EndsWith("è"))
                {
                    denominador = denominador.Substring(0, denominador.Length - 1) + "ens";
                }else if (denominador.EndsWith("n"))
                {
                    denominador = denominador + "ques";
                }
                else
                {
                    denominador = denominador + "s";
                }    
            }
            else
            {
                numeradorAdj = numerador;
                if (denominador == "terç")
                {
                    denominador = "tercos";
                }
                else if (denominador.EndsWith("è"))
                {
                    denominador = denominador.Substring(0, denominador.Length - 1) + "ens";
                }
                else if (denominador.EndsWith("n"))
                {
                    denominador = denominador + "ques";
                }
                else
                {
                    denominador = denominador + "s";
                }
            }

            if (numeradorAdj == "una")
            {
                denominadorAdj = Fraccionario.ConvertirNumEnteroFracDenominador(pDenominador, "F");
            }
            else
            {
                denominadorAdj = Fraccionario.ConvertirNumEnteroFracDenominador(pDenominador, "F");
                if(denominadorAdj == "mitja")
                {
                    denominadorAdj = "mitges";
                }else if (denominadorAdj.EndsWith("a"))
                {
                    denominadorAdj = denominadorAdj.Substring(0, denominadorAdj.Length - 1) + "es";
                }
            }

            string numCompletoLetras = numerador + " " + denominador;
            string numCompletoAdj = numeradorAdj + " " + denominadorAdj;

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "FraccionTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource","FraccionNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionNota2").ToString());
            //AÑADIR NOTAS BUENAS CUANDO ACABES !!!!!!!!!!!!!!!!!!!!!
            
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource","ReferenciasTitulo").ToString();
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!
            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioEjemplo1").ToString().Replace("...", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioEjemplo2").ToString().Replace("...", numCompletoLetras));
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion Sustantivo = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SustantivoOpcion").ToString());
            Sustantivo.Opciones = new List<string>();
            Sustantivo.Opciones.Add(numCompletoLetras);

            Opcion Adjetivo = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "AdjetivoOpcion").ToString());
            Adjetivo.Opciones = new List<string>();
            Adjetivo.Opciones.Add(numCompletoAdj);

            resultado.MasOpciones.Add(Sustantivo);
            resultado.MasOpciones.Add(Adjetivo);


            return resultado;
        }

        public Conversion ConversionDecimal(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string[] partes = Regex.Split(numero, @"[.,]");
            string parteEntera = Cardinales.ConvertirNumEnteroCardinal(partes[0], signo);
            string parteDecimal = Cardinales.ConvertirNumDecimalCardinal(partes[1]);

            //System.Diagnostics.Debug.WriteLine("Valor de la parte entera: " + parteEntera.ToString());
            //System.Diagnostics.Debug.WriteLine("Valor de la parte decimal: " + parteDecimal.ToString());

            string numCompletoLetras = parteEntera + " ambs " + parteDecimal;
            //System.Diagnostics.Debug.WriteLine("Numero decimal: " + numCompletoLetras);

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "DecimalTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!

            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);
            if(valorNum == true)
            {
                resultado.TitValorNumerico = HttpContext.GetGlobalResourceObject("Resource", "ValorNumericoTitulo").ToString();
                resultado.ValorNumerico = numero;
                resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
                resultado.MasOpciones = new List<Opcion>();

                Opcion Expresion = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "ExpresionTitulo").ToString());
                Expresion.Opciones = new List<string>();
                Expresion.Opciones.Add(numCompletoLetras);
                Expresion.Opciones.Add(parteEntera + " [coma/i/amb] " + parteDecimal);
            }

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalEjemplo1").ToString().Replace("...", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalEjemplo2").ToString().Replace("...", numCompletoLetras));

            return resultado;
        }
        
        public Conversion ConversionCardinal(string numero, bool signo, string numeroOrignal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras = Cardinales.ConvertirNumEnteroCardinal(numero, signo);

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "CardinalTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!

            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SusAdPro = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SusAdProTitulo").ToString());
            SusAdPro.Opciones = new List<string>();
            SusAdPro.Opciones.Add(numCompletoLetras);

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalEjemplo1").ToString().Replace("...", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalEjemplo2").ToString().Replace("...", numCompletoLetras));

            if (valorNum == true)
            {
                resultado.TitValorNumerico = HttpContext.GetGlobalResourceObject("Resource", "ValorNumericoTitulo").ToString();
                resultado.ValorNumerico = numero;
            }

                return resultado;
        }

        public Conversion ConversionNegativo(string numero, bool signo, string numeroOrignal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();
            //System.Diagnostics.Debug.WriteLine("Numero negativo: " + numero);
            
            string numCompletoLetras = Cardinales.ConvertirNumEnteroCardinal(numero, signo);

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "NegativoTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!

            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SusAdPro = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SusAdProTitulo").ToString());
            SusAdPro.Opciones = new List<string>();
            SusAdPro.Opciones.Add(numCompletoLetras);

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoEjemplo1").ToString().Replace("...", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoEjemplo2").ToString().Replace("...", numCompletoLetras));

            if (valorNum == true)
            {
                resultado.TitValorNumerico = HttpContext.GetGlobalResourceObject("Resource", "ValorNumericoTitulo").ToString();
                resultado.ValorNumerico = numero;
            }

            return resultado;
        }

        public Conversion ConversionOrdinal(string numero, bool signo, string numeroOrignal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras = Ordinales.ConvertirNumEnteroOrdinal(numero, "M");

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "OrdinalTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!

            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion FormFem = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemOpcion").ToString());
            FormFem.Opciones = new List<string>();
            FormFem.Opciones.Add(Ordinales.ConvertirNumEnteroOrdinal(numero, "F"));

            return resultado;
        }

        public Conversion ConversionFraccionario(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras = Fraccionario.ConvertirNumEnteroFraccionario(numero, "M");

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "FraccionarioTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!


            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            //AÑADIR EJEMPLOS
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SustantivoM = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SustantivoMOpcion").ToString());
            SustantivoM.Opciones = new List<string>();
            SustantivoM.Opciones.Add(numCompletoLetras);

            Opcion SustantivoF = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SustantivoFOpcion").ToString());
            SustantivoF.Opciones = new List<string>();
            SustantivoF.Opciones.Add(Fraccionario.ConvertirNumEnteroFraccionario(numero, "F"));

            Opcion AdjetivoPronF = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "AdjetivoPronombreFOpcion").ToString());
            AdjetivoPronF.Opciones = new List<string>();
            AdjetivoPronF.Opciones.Add(Fraccionario.ConvertirNumEnteroFraccionario(numero, "F"));

            resultado.MasOpciones.Add(SustantivoM);
            resultado.MasOpciones.Add(SustantivoF);
            resultado.MasOpciones.Add(AdjetivoPronF);

            return resultado;
        }
        
        public Conversion ConversionMultiplicativo(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras = Multiplicativo.ConvertirNumEnteroMultiplicativo(numero);

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!


            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            //AÑADIR EJEMPLOS
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion ExpreAdj = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "ExpresionAdjetivoOpcion").ToString());
            ExpreAdj.Opciones = new List<string>();
            ExpreAdj.Opciones.Add(numCompletoLetras);


            return resultado;

        }

    }
 

    
}
