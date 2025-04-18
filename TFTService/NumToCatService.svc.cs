﻿using System;
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
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading.Tasks;



namespace TFTService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class TFTServicio : INumToCat
    {

        private CultureInfo language;
        List<Conversion> conversiones = new List<Conversion>();//Aqui vamos a meter todas las conversiones
        Cabecera cabecera;

        public (Cabecera, List<Conversion>) MainTraducir(string value, string lenguaje)
        {
            if (lenguaje != null)
            {
                if (lenguaje.IndexOf("-") > -1) lenguaje = lenguaje.Substring(0, lenguaje.IndexOf("-"));
                language = new CultureInfo(lenguaje);
            }
            else language = new CultureInfo("es");
            Thread.CurrentThread.CurrentUICulture = language;


            var cerrojo = new object();
            string numero = value.Replace(" ","").Trim();
            //System.Diagnostics.Debug.WriteLine("DESPUES DEL TRIM: " + numero);
            int longitudNumero = numero.Length;
            Boolean signo = false;
            //Boolean decima = false;
            //Comprobacion de signo 
            if (numero.StartsWith("-") && numero.Length > 1 && char.IsDigit(numero[1]))
            {
                signo = true;
                numero = numero.Substring(1);
            }
            else if (numero.Contains("/-"))
            {
                signo = true;
                numero = numero.Replace("/-", "/");
            }

            //Comprobacion de si es decimal
            if (Regex.IsMatch(numero, @"^([+-]?)\d+([.,])\d+$"))
            {
                //decima = true;
                conversiones.Add(ConversionDecimal(numero, signo, value, false));
            }

            //Comprobacion de si es fraccionario
            if (Regex.IsMatch(numero, @"^([+-]?)\d+([/])\d+$"))
            {
                string[] partes = numero.Split('/');
                string numerador = partes[0];
                string denominador = partes[1];

                conversiones.Add(ConversionFraccion(numerador, denominador, signo, value));

                BigInteger.TryParse(numerador, out BigInteger Inumerador);
                BigInteger.TryParse(denominador, out BigInteger Idenominador);
                //BigInteger Inumerador = BigInteger.Parse(numerador);
                //BigInteger Idenominador = BigInteger.Parse(denominador);
                if (Idenominador != 0)
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
                            string parteEnteraNoSigno = parteEntera.ToString().Replace("-", "").Trim();
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
            //Match match = Regex.Match(numero, @"^([+-]?\d+[.,]?\d+)[Ee]([+-]?\d+)$");
            Match match = Regex.Match(numero, @"^([+-]?\d+(?:[.,]?\d+)?)[Ee]([+-]?\d+)$");

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
                    System.Diagnostics.Debug.WriteLine("NUMERO EXPANDIDO: " + numeroExpandido);
                    if (numeroExpandido.Contains('.'))
                    {
                        cabecera = new Cabecera(numero, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                        conversiones.Add(ConversionDecimal(numeroExpandido, signo, value, true));
                    }
                    else if (signo == true)
                    {
                        cabecera = new Cabecera(numero, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                        conversiones.Add(ConversionNegativo(numeroExpandido, signo, value, true));

                    }
                    else
                    {
                        cabecera = new Cabecera(numero, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                        Parallel.Invoke(
                            () =>
                            {
                                lock (cerrojo) conversiones.Add(ConversionCardinal(numeroExpandido, signo, value, true));
                            },
                            () =>
                            {
                                var conversionOrdinal = ConversionOrdinal(numeroExpandido, signo, value, false);
                                if (conversionOrdinal != null) lock (cerrojo) conversiones.Add(conversionOrdinal);
                            },
                            () =>
                            {
                                var conversionFraccionario = ConversionFraccionario(numeroExpandido, signo, value, false);
                                if (conversionFraccionario != null) lock (cerrojo) conversiones.Add(conversionFraccionario);
                            },
                            () =>
                            {
                                var conversionMultiplicativo = ConversionMultiplicativo(numeroExpandido, signo, value, false);
                                if (conversionMultiplicativo != null) lock (cerrojo) conversiones.Add(conversionMultiplicativo);
                            },
                            () =>
                            {
                                if (numeroExpandido.Length <= 5)
                                {
                                    System.Diagnostics.Debug.WriteLine(numeroExpandido + " :PARA NUMERO CIENTIFICO");
                                    var conversionPoligono = ConversionPoligono(numeroExpandido, signo, value, false);
                                    if (conversionPoligono != null) lock (cerrojo) conversiones.Add(conversionPoligono);
                                }
                            }
                        );
                    }
                }
                else
                {
                    string numeroExpandido = NotacionCientifica.ExpandirNotacionCientificaNegativa(baseNum, exponente);
                    if (numeroExpandido.Contains('.'))
                    {
                        cabecera = new Cabecera(numero, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                        conversiones.Add(ConversionDecimal(numeroExpandido, signo, value, true));
                    }
                    else if (signo == true)
                    {
                        cabecera = new Cabecera(numero, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                        conversiones.Add(ConversionNegativo(numeroExpandido, signo, value, true));
                    }
                    else
                    {
                        cabecera = new Cabecera(numero, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                        Parallel.Invoke(
                            () =>
                            {
                                lock (cerrojo) conversiones.Add(ConversionCardinal(numeroExpandido, signo, value, true));
                            },
                            () =>
                            {
                                var conversionOrdinal = ConversionOrdinal(numeroExpandido, signo, value, false);
                                if (conversionOrdinal != null) lock (cerrojo) conversiones.Add(conversionOrdinal);
                            },
                            () =>
                            {
                                var conversionFraccionario = ConversionFraccionario(numeroExpandido, signo, value, false);
                                if (conversionFraccionario != null) lock (cerrojo) conversiones.Add(conversionFraccionario);
                            },
                            () =>
                            {
                                var conversionMultiplicativo = ConversionMultiplicativo(numeroExpandido, signo, value, false);
                                if (conversionMultiplicativo != null) lock (cerrojo) conversiones.Add(conversionMultiplicativo);
                            },
                            () =>
                            {
                                if (numeroExpandido.Length <= 5)
                                {
                                    System.Diagnostics.Debug.WriteLine(numeroExpandido + " :PARA NUMERO CIENTIFICO");
                                    var conversionPoligono = ConversionPoligono(numeroExpandido, signo, value, false);
                                    if (conversionPoligono != null) lock (cerrojo) conversiones.Add(conversionPoligono);
                                }
                            }
                        );
                    }
                }

            }

            //Comprobacion de si el numero es €
            if (Regex.IsMatch(numero, @"^(€\s?\d+([.,]?\d{1,2})?|[-+]?\d+([.,]?\d+)?\s?€)$"))
            {
                numero = numero.Replace("€", "");
                string numeroFormateado = FormateoNumero.FormatearNumero(numero);
                if (signo == true)
                {
                    numeroFormateado = "-" + numeroFormateado;
                }
                numeroFormateado = numeroFormateado + "€";
                cabecera = new Cabecera(numeroFormateado, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                //System.Diagnostics.Debug.WriteLine("SE CREO LA CABECERA!!!!!!!!!!!!");
                //System.Diagnostics.Debug.WriteLine("FORMATEADO:"+cabecera.Formateado);
                //System.Diagnostics.Debug.WriteLine("TITULO:" + cabecera.Titulo);
                if (numero.Contains(".") || numero.Contains(","))
                {
                    Parallel.Invoke(
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionEuro(numero, signo, value, false));
                        },
                        () =>
                        {

                            lock (cerrojo) conversiones.Add(ConversionDecimal(numero, signo, value, false));
                        }
                        );
                }
                else if (signo == true)
                {
                    Parallel.Invoke(
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionEuro(numero, signo, value, false));
                        },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionNegativo(numero, signo, value, false));
                        }
                    );
                }
                else
                {
                    Parallel.Invoke(
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionEuro(numero, signo, value, false));
                        },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionCardinal(numero, signo, value, false));
                        },
                        () =>
                        {
                            var conversionOrdinal = ConversionOrdinal(numero, signo, value, false);
                            if (conversionOrdinal != null) lock (cerrojo) conversiones.Add(conversionOrdinal);
                        },
                        () =>
                        {
                            System.Diagnostics.Debug.WriteLine("Valor de 'numero' antes del parse: '" + numero + "'");
                            var conversionFraccionario = ConversionFraccionario(numero, signo, value, false);
                            if (conversionFraccionario != null) lock (cerrojo) conversiones.Add(conversionFraccionario);
                        },
                        () =>
                        {
                            var conversionMultiplicativo = ConversionMultiplicativo(numero, signo, value, false);
                            if (conversionMultiplicativo != null) lock (cerrojo) conversiones.Add(conversionMultiplicativo);
                        }
                    );
                }
                numero += "€";
            }


            //Comprobacion de si el numero es $
            if (Regex.IsMatch(numero, @"^(\$\s?\d+([.,]?\d{1,2})?|[-+]?\d+([.,]?\d+)?\s?\$)$"))
            {
                numero = numero.Replace("$", "");
                string numeroFormateado = FormateoNumero.FormatearNumero(numero);
                if (signo == true)
                {
                    numeroFormateado = "-" + numeroFormateado;
                }
                numeroFormateado = numeroFormateado + "$";
                cabecera = new Cabecera(numeroFormateado, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                //System.Diagnostics.Debug.WriteLine("SE CREO LA CABECERA!!!!!!!!!!!!");
                //System.Diagnostics.Debug.WriteLine("FORMATEADO:"+cabecera.Formateado);
                //System.Diagnostics.Debug.WriteLine("TITULO:" + cabecera.Titulo);
                if (numero.Contains(".") || numero.Contains(","))
                {
                    Parallel.Invoke(
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionPeso(numero, signo, value, false));
                        },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionDolar(numero, signo, value, false));
                        },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionDecimal(numero, signo, value, false));
                        }
                    );
                }
                else if (signo == true)
                {
                    Parallel.Invoke(
                         () =>
                         {
                             lock (cerrojo) conversiones.Add(ConversionPeso(numero, signo, value, false));
                         },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionDolar(numero, signo, value, false));
                        },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionNegativo(numero, signo, value, false));
                        }
                    );
                }
                else
                {

                    Parallel.Invoke(
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionPeso(numero, signo, value, false));
                        },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionDolar(numero, signo, value, false));
                        },
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionCardinal(numero, signo, value, false));
                        },
                        () =>
                        {
                            var conversionOrdinal = ConversionOrdinal(numero, signo, value, false);
                            if (conversionOrdinal != null) lock (cerrojo) conversiones.Add(conversionOrdinal);
                        },
                        () =>
                        {
                            var conversionFraccionario = ConversionFraccionario(numero, signo, value, false);
                            if (conversionFraccionario != null) lock (cerrojo) conversiones.Add(conversionFraccionario);
                        },
                        () =>
                        {
                            var conversionMultiplicativo = ConversionMultiplicativo(numero, signo, value, false);
                            if (conversionMultiplicativo != null) lock (cerrojo) conversiones.Add(conversionMultiplicativo);
                        }
                    );
                }
                numero += "$";

            }

            //Comprobacion numerica
            if (Regex.IsMatch(numero, @"^\d+$"))
            {
                if (signo == true)
                {
                    string numeroFormateado = "-" + FormateoNumero.FormatearNumero(numero);
                    cabecera = new Cabecera(numeroFormateado, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());

                    conversiones.Add(ConversionNegativo(numero, signo, value, false));
                }
                else
                {
                    Parallel.Invoke(
                        () =>
                        {
                            lock (cerrojo) conversiones.Add(ConversionCardinal(numero, signo, value, false));
                        },
                        () =>
                        {
                            var conversionOrdinal = ConversionOrdinal(numero, signo, value, false);
                            if (conversionOrdinal != null) lock (cerrojo) conversiones.Add(conversionOrdinal);
                        },
                        () =>
                        {
                            var conversionFraccionario = ConversionFraccionario(numero, signo, value, false);
                            if (conversionFraccionario != null) lock (cerrojo) conversiones.Add(conversionFraccionario);
                        },
                        () =>
                        {
                            var conversionMultiplicativo = ConversionMultiplicativo(numero, signo, value, false);
                            if (conversionMultiplicativo != null) lock (cerrojo) conversiones.Add(conversionMultiplicativo);
                        },
                        () =>
                        {
                            if (numero.Length <= 5)
                            {
                                var conversionPoligono = ConversionPoligono(numero, signo, value, false);
                                if (conversionPoligono != null) lock (cerrojo) conversiones.Add(conversionPoligono);
                            }
                        }

                    );
                }
            }

            //Comprobacion de si es numero romano
            if (Regex.IsMatch(numero, @"^[IVXLCDMivxlcdm]+$") && signo != true)
            {

                string numeroUpper = numero.ToUpper();
                if (numeroUpper != numero)
                {
                    cabecera = new Cabecera(numeroUpper, HttpContext.GetGlobalResourceObject("Resource", "NumeroFormateadoTitulo").ToString());
                }

                string numeroEntero = Romano.ConvertirNumRomanoEntero(numero);

                Parallel.Invoke(
                    () =>
                    {
                        lock (cerrojo) conversiones.Add(ConversionCardinal(numeroEntero, signo, value, false));
                    },
                    () =>
                    {
                        var conversionOrdinal = ConversionOrdinal(numeroEntero, signo, value, false);
                        if (conversionOrdinal != null) lock (cerrojo) conversiones.Add(conversionOrdinal);
                    },
                    () =>
                    {
                        var conversionFraccionario = ConversionFraccionario(numeroEntero, signo, value, false);
                        if (conversionFraccionario != null) lock (cerrojo) conversiones.Add(conversionFraccionario);
                    },
                    () =>
                    {
                        var conversionMultiplicativo = ConversionMultiplicativo(numeroEntero, signo, value, false);
                        if (conversionMultiplicativo != null) lock (cerrojo) conversiones.Add(conversionMultiplicativo);
                    },
                    () =>
                    {
                        if (numero.Length <= 5)
                        {
                            var conversionPoligono = ConversionPoligono(numeroEntero, signo, value, false);
                            if (conversionPoligono != null) lock (cerrojo) conversiones.Add(conversionPoligono);
                        }
                    }
                );
            }


            return (cabecera, conversiones);
        }

        public Conversion ConversionFraccion(string pNumerador, string pDenominador, bool signo, string numeroOriginal)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numerador = Cardinales.ConvertirNumEnteroCardinal(pNumerador, signo);
            string denominador = Fraccionario.ConvertirNumEnteroFracDenominador(pDenominador, "M");

            string denominadorAdj = "";
            string numeradorAdj = "";
            if (numerador == "un")
            {
                numeradorAdj = numerador + "a";
            }
            else if (numerador == "dos")
            {
                numeradorAdj = "dues";
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
                if (denominadorAdj == "mitja")
                {
                    denominadorAdj = "mitges";
                }
                else if (denominadorAdj.EndsWith("a"))
                {
                    denominadorAdj = denominadorAdj.Substring(0, denominadorAdj.Length - 1) + "es";
                }
            }

            string numCompletoLetras = numerador + " " + denominador;
            string numCompletoAdj = numeradorAdj + " " + denominadorAdj;

            string numeradorVal = Cardinales.ConvertirNumEnteroCardinalVal(pNumerador, signo);
            string denominadorVal = Fraccionario.ConvertirNumEnteroFracDenominadorVal(pDenominador, "M");


            string denominadorAdjVal = "";
            string numeradorAdjVal = "";

            if (numeradorVal == "un")
            {
                numeradorAdjVal = numeradorVal + "a";
            }
            else if (numeradorVal == "dos")
            {
                numeradorAdjVal = "dues";
                if (denominadorVal == "terç")
                {
                    denominadorVal = "tercos";
                }
                else if (denominadorVal.EndsWith("è"))
                {
                    denominadorVal = denominadorVal.Substring(0, denominadorVal.Length - 1) + "ens";
                }
                else if (denominadorVal.EndsWith("n"))
                {
                    denominadorVal = denominadorVal + "ques";
                }
                else
                {
                    denominadorVal = denominadorVal + "s";
                }
            }
            else
            {
                numeradorAdjVal = numeradorVal;
                if (denominadorVal == "terç")
                {
                    denominadorVal = "tercos";
                }
                else if (denominadorVal.EndsWith("è"))
                {
                    denominadorVal = denominadorVal.Substring(0, denominadorVal.Length - 1) + "ens";
                }
                else if (denominadorVal.EndsWith("n"))
                {
                    denominadorVal = denominadorVal + "ques";
                }
                else
                {
                    denominadorVal = denominadorVal + "s";
                }
            }

            if (numeradorAdjVal == "una")
            {
                denominadorAdjVal = Fraccionario.ConvertirNumEnteroFracDenominadorVal(pDenominador, "F");
            }
            else
            {
                denominadorAdjVal = Fraccionario.ConvertirNumEnteroFracDenominadorVal(pDenominador, "F");
                if (denominadorAdjVal == "mitja")
                {
                    denominadorAdjVal = "mitges";
                }
                else if (denominadorAdjVal.EndsWith("a"))
                {
                    denominadorAdjVal = denominadorAdjVal.Substring(0, denominadorAdjVal.Length - 1) + "es";
                }
            }

            string numCompletoLetrasVal = numeradorVal + " " + denominadorVal;
            string numCompletoAdjVal = numeradorAdjVal + " " + denominadorAdjVal;



            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "FraccionTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();

            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionNota2").ToString());


            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionReferencia1").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionReferencia2").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionReferencia3").ToString());


            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionEjemplo4").ToString().Replace("|", numCompletoLetras));

            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion Sustantivo = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SustantivoOpcion").ToString());
            Sustantivo.Opciones = new List<string>();
            Sustantivo.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            Sustantivo.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal);

            Opcion Adjetivo = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "AdjetivoOpcion").ToString());
            Adjetivo.Opciones = new List<string>();
            Adjetivo.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoAdj);
            Adjetivo.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoAdjVal);

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

            string parteEnteraVal = Cardinales.ConvertirNumEnteroCardinalVal(partes[0], signo);
            string parteDecimalVal = Cardinales.ConvertirNumDecimalCardinalVal(partes[1]);

            string numCompletoLetrasVal = parteEnteraVal + " ambs " + parteDecimalVal;

            //System.Diagnostics.Debug.WriteLine("Numero decimal: " + numCompletoLetras);

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "DecimalTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalNota2").ToString());

            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalReferencia1").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalReferencia2").ToString());


            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);
            if (valorNum == true)
            {
                resultado.TitValorNumerico = HttpContext.GetGlobalResourceObject("Resource", "ValorNumericoTitulo").ToString();
                string numeroFormateado = FormateoNumero.FormatearNumero(numero);
                resultado.ValorNumerico = numeroFormateado;

            }
            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion Expresion = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "ExpresionTitulo").ToString());
            Expresion.Opciones = new List<string>();
            Expresion.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            Expresion.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal);
            Expresion.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + parteEntera + " [coma/i/amb] " + parteDecimal);
            Expresion.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + parteEnteraVal + " [coma/i/amb] " + parteDecimalVal);

            resultado.MasOpciones.Add(Expresion);

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "DecimalEjemplo4").ToString().Replace("|", numCompletoLetras));

            return resultado;
        }

        public Conversion ConversionCardinal(string numero, bool signo, string numeroOrignal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras = Cardinales.ConvertirNumEnteroCardinal(numero, signo);
            if (string.IsNullOrEmpty(numCompletoLetras))
            {
                return null;
            }
            
            //System.Diagnostics.Debug.WriteLine("NUMERO CARDINAL: " + numCompletoLetras);

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "CardinalTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalNota3").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalNota4").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalNota5").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalNota6").ToString());


            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalReferencia1").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalReferencia2").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalReferencia3").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalReferencia4").ToString());


            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SusAdPro = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SusAdProTitulo").ToString());
            SusAdPro.Opciones = new List<string>();
            SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Cardinales.ConvertirNumEnteroCardinalVal(numero, signo));

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalEjemplo4").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "CardinalEjemplo5").ToString().Replace("|", numCompletoLetras));

            if (valorNum == true)
            {
                resultado.TitValorNumerico = HttpContext.GetGlobalResourceObject("Resource", "ValorNumericoTitulo").ToString();
                string numeroFormateado = FormateoNumero.FormatearNumero(numero);
                resultado.ValorNumerico = numeroFormateado;
            }

            resultado.MasOpciones.Add(SusAdPro);

            if (numCompletoLetras == "un")
            {
                Opcion FormFem = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemOpcion").ToString());
                FormFem.Opciones = new List<string>();
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + "una");
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + "una");
                resultado.MasOpciones.Add(FormFem);
            }
            else if (numCompletoLetras == "dos")
            {
                Opcion FormFem = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemOpcion").ToString());
                FormFem.Opciones = new List<string>();
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + "dues");
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + "dues");
                resultado.MasOpciones.Add(FormFem);
            }
            else if (numCompletoLetras == "disset")
            {
                SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "BalearTipo").ToString() + "desset");
            }
            else if (numCompletoLetras == "divuit")
            {
                SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "BalearTipo").ToString() + "devuit");
            }
            else if (numCompletoLetras == "dinou")
            {
                SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "BalearTipo").ToString() + "denou");
            }

            return resultado;
        }

        public Conversion ConversionNegativo(string numero, bool signo, string numeroOrignal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();
            //System.Diagnostics.Debug.WriteLine("Numero negativo: " + numero);

            string numCompletoLetras = Cardinales.ConvertirNumEnteroCardinal(numero, signo);
            if (string.IsNullOrEmpty(numCompletoLetras))
            {
                return null;
            }

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "NegativoTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoNota3").ToString());

            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoReferencia1").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoReferencia2").ToString());


            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SusAdPro = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SusAdProTitulo").ToString());
            SusAdPro.Opciones = new List<string>();
            SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Cardinales.ConvertirNumEnteroCardinalVal(numero, signo));

            resultado.MasOpciones.Add(SusAdPro);

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "NegativoEjemplo4").ToString().Replace("|", numCompletoLetras));

            if (valorNum == true)
            {
                resultado.TitValorNumerico = HttpContext.GetGlobalResourceObject("Resource", "ValorNumericoTitulo").ToString();
                string numeroFormateado = FormateoNumero.FormatearNumero(numero);
                resultado.ValorNumerico = numeroFormateado;
            }

            if (numCompletoLetras == "minus un")
            {
                Opcion FormFem = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemOpcion").ToString());
                FormFem.Opciones = new List<string>();
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + "minus una");
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + "minus una");
                resultado.MasOpciones.Add(FormFem);
            }
            else if (numCompletoLetras == "minus dos")
            {
                Opcion FormFem = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemOpcion").ToString());
                FormFem.Opciones = new List<string>();
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + "minus dues");
                FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + "minus dues");
                resultado.MasOpciones.Add(FormFem);
            }
            else if (numCompletoLetras == "minus disset")
            {
                SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "BalearTipo").ToString() + "minus desset");
            }
            else if (numCompletoLetras == "minus divuit")
            {
                SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "BalearTipo").ToString() + "minus devuit");
            }
            else if (numCompletoLetras == "minus dinou")
            {
                SusAdPro.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "BalearTipo").ToString() + "minus denou");
            }

            return resultado;
        }

        public Conversion ConversionOrdinal(string numero, bool signo, string numeroOrignal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras = Ordinales.ConvertirNumEnteroOrdinal(numero, "M", false);
            //System.Diagnostics.Debug.WriteLine("NUMERO ORDINAL: " + numCompletoLetras);
            if (string.IsNullOrEmpty(numCompletoLetras))
            {
                return null;
            }


            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "OrdinalTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalNota3").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalNota4").ToString());

            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalReferencia1").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalReferencia2").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalReferencia3").ToString());


            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "OrdinalEjemplo4").ToString().Replace("|", numCompletoLetras));

            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion FormFemSin = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemSinOpcion").ToString());
            FormFemSin.Opciones = new List<string>();
            FormFemSin.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + Ordinales.ConvertirNumEnteroOrdinal(numero, "F", false));
            FormFemSin.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Ordinales.ConvertirNumEnteroOrdinalVal(numero, "F", false));

            Opcion FormMasSin = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormMasSinOpcion").ToString());
            FormMasSin.Opciones = new List<string>();
            FormMasSin.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            FormMasSin.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Ordinales.ConvertirNumEnteroOrdinalVal(numero, "M", false));

            Opcion FormMasPlu = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormMasPluOpcion").ToString());
            FormMasPlu.Opciones = new List<string>();
            FormMasPlu.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + Ordinales.ConvertirNumEnteroOrdinal(numero, "M", true));
            FormMasPlu.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Ordinales.ConvertirNumEnteroOrdinalVal(numero, "M", true));


            Opcion FormFemPlu = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemPluOpcion").ToString());
            FormFemPlu.Opciones = new List<string>();
            FormFemPlu.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + Ordinales.ConvertirNumEnteroOrdinal(numero, "F", true));
            FormFemPlu.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Ordinales.ConvertirNumEnteroOrdinalVal(numero, "F", true));

            resultado.MasOpciones.Add(FormFemSin);
            resultado.MasOpciones.Add(FormMasSin);
            resultado.MasOpciones.Add(FormMasPlu);
            resultado.MasOpciones.Add(FormFemPlu);
            //System.Diagnostics.Debug.WriteLine("NUMERO ORDINAL: FEMENINO " + Ordinales.ConvertirNumEnteroOrdinal(numero, "F"));
            return resultado;
        }

        public Conversion ConversionFraccionario(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();
            System.Diagnostics.Debug.WriteLine("NUMERO FRACCIONARIO ANTES DEL TRIM: " + numero);
            numero = numero.TrimStart('0');
           
            System.Diagnostics.Debug.WriteLine("NUMERO FRACCIONARIO DESPUES DEL TRIM: " + numero);
            if (string.IsNullOrEmpty(numero)) numero = "0";
            string numCompletoLetras = Fraccionario.ConvertirNumEnteroFraccionario(numero, "M");
            System.Diagnostics.Debug.WriteLine("NUMERO FRACCIONARIO: " + numCompletoLetras);
            if (string.IsNullOrEmpty(numCompletoLetras))
            {
                return null;
            }

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "FraccionarioTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioNota1").ToString());


            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioReferencia1").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioReferencia2").ToString());



            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioEjemplo4").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "FraccionarioEjemplo5").ToString().Replace("|", numCompletoLetras));
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SustantivoM = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SustantivoMOpcion").ToString());
            SustantivoM.Opciones = new List<string>();
            SustantivoM.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            SustantivoM.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Fraccionario.ConvertirNumEnteroFraccionarioVal(numero, "M"));

            Opcion SustantivoF = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SustantivoFOpcion").ToString());
            SustantivoF.Opciones = new List<string>();
            SustantivoF.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + Fraccionario.ConvertirNumEnteroFraccionario(numero, "F"));
            SustantivoF.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Fraccionario.ConvertirNumEnteroFraccionarioVal(numero, "F"));

            Opcion AdjetivoPronF = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "AdjetivoPronombreFOpcion").ToString());
            AdjetivoPronF.Opciones = new List<string>();
            AdjetivoPronF.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + Fraccionario.ConvertirNumEnteroFraccionario(numero, "F"));

            resultado.MasOpciones.Add(SustantivoM);
            resultado.MasOpciones.Add(SustantivoF);
            resultado.MasOpciones.Add(AdjetivoPronF);

            return resultado;
        }

        public Conversion ConversionMultiplicativo(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();
            System.Diagnostics.Debug.WriteLine("NUMERO MULTIPLICATIVO ANTES DEL TRIM: " + numero);
            numero = numero.TrimStart('0');
            System.Diagnostics.Debug.WriteLine("NUMERO MULTIPLICATIVO DESPUES DEL TRIM: " + numero);
            if (string.IsNullOrEmpty(numero)) numero = "0";
            string numCompletoLetras = Multiplicativo.ConvertirNumEnteroMultiplicativo(numero, "M");
            if (string.IsNullOrEmpty(numCompletoLetras))
            {
                return null;
            }
            System.Diagnostics.Debug.WriteLine("NUMERO MULTIPLICATIVO: " + numCompletoLetras);
            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoNota3").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoNota4").ToString());
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!
            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoReferencia1").ToString());
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoReferencia2").ToString());
            //AÑADIR REFERENCIAS BUENAS CUANDO ACABES!!!!!!!!!!!!!!!!!!


            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "MultiplicativoEjemplo4").ToString().Replace("|", numCompletoLetras));
            //AÑADIR EJEMPLOS
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion ExpreAdj = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "ExpresionAdjetivoOpcion").ToString());
            ExpreAdj.Opciones = new List<string>();
            ExpreAdj.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            ExpreAdj.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Multiplicativo.ConvertirNumEnteroMultiplicativoVal(numero, "M"));

            resultado.MasOpciones.Add(ExpreAdj);

            int tamañoNumero = numero.Length;
            if (tamañoNumero <= 3)
            {
                int numeroInt = int.Parse(numero);
                if (numeroInt >= 2 && numeroInt <= 12 || numeroInt == 100)
                {
                    Opcion FormFem = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "FormFemOpcion").ToString());
                    FormFem.Opciones = new List<string>();
                    FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + Multiplicativo.ConvertirNumEnteroMultiplicativo(numero, "F"));
                    FormFem.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + Multiplicativo.ConvertirNumEnteroMultiplicativoVal(numero, "F"));

                    resultado.MasOpciones.Add(FormFem);
                }
            }

            return resultado;

        }

        public Conversion ConversionEuro(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras;
            string numCompletoLetrasVal;
            string numeroNoSimbolo = numero.Replace("€", "");
            string[] partes = Regex.Split(numeroNoSimbolo, @"[.,]");

            string parteEuros = Cardinales.ConvertirNumEnteroCardinal(partes[0], signo);
            string parteEurosVal = Cardinales.ConvertirNumEnteroCardinalVal(partes[0], signo);
            string parteCentimos = "";
            string parteCentimosVal = "";
            if (partes.Length > 1)
            {
                if (partes[1].Length == 1)
                {
                    partes[1] += "0";
                }
                else if (partes[1].Length > 2)
                {
                    partes[1] = partes[1].Substring(0, 2);
                }
                parteCentimos = Cardinales.ConvertirNumEnteroCardinal(partes[1], false);
                parteCentimosVal = Cardinales.ConvertirNumEnteroCardinalVal(partes[1], false);
            }
            if (partes[0] == "1")
            {
                numCompletoLetras = parteEuros + " " + HttpContext.GetGlobalResourceObject("Resource", "Euro").ToString();
                numCompletoLetrasVal = parteEurosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Euro").ToString();
            }
            else if (partes[0].Length <= 6)
            {
                numCompletoLetras = parteEuros + " " + HttpContext.GetGlobalResourceObject("Resource", "Euros").ToString();
                numCompletoLetrasVal = parteEurosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Euros").ToString();
            }
            else
            {
                numCompletoLetras = parteEuros + " " + HttpContext.GetGlobalResourceObject("Resource", "DeEuros").ToString();
                numCompletoLetrasVal = parteEurosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "DeEuros").ToString();
            }
            if (!string.IsNullOrEmpty(parteCentimos))
            {
                if (partes[1] == "01")
                {
                    numCompletoLetras += " i " + parteCentimos + " " + HttpContext.GetGlobalResourceObject("Resource", "Centimo").ToString();
                    numCompletoLetrasVal += " i " + parteCentimosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Centimo").ToString();
                }
                else
                {
                    numCompletoLetras += " i " + parteCentimos + " " + HttpContext.GetGlobalResourceObject("Resource", "Centimos").ToString();
                    numCompletoLetrasVal += " i " + parteCentimosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Centimos").ToString();
                }
            }



            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "EurosTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "EuroNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "EuroNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "EuroNota3").ToString());
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "EuroEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "EuroEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "EuroEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "EuroEjemplo4").ToString().Replace("|", numCompletoLetras));
            //AÑADIR EJEMPLOS
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);


            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SintagmaNom = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SintagmaNominalOpcion").ToString());
            SintagmaNom.Opciones = new List<string>();
            SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras);
            SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal);
            if (numero.Contains(".") || numero.Contains(","))
            {
                SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras.Replace(" i ", " ambs "));
                SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " ambs "));
            }


            /*Opcion NoApropiado = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "NoApropiadoOpcion").ToString());
            NoApropiado.Opciones = new List<string>();
            NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString()+numCompletoLetras.Replace(" i ", " punt "));
            NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValenacianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " punt "));*/


            resultado.MasOpciones.Add(SintagmaNom);
            if (numero.Contains(".") || numero.Contains(","))
            {
                Opcion NoApropiado = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "NoApropiadoOpcion").ToString());
                NoApropiado.Opciones = new List<string>();
                NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras.Replace(" i ", " punt "));
                NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " punt "));

                resultado.MasOpciones.Add(NoApropiado);
            }
            return resultado;
        }

        public Conversion ConversionPeso(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras;
            string numCompletoLetrasVal;
            string numeroNoSimbolo = numero.Replace("$", "");
            string[] partes = Regex.Split(numeroNoSimbolo, @"[.,]");

            string partePesos = Cardinales.ConvertirNumEnteroCardinal(partes[0], signo);
            string partePesosVal = Cardinales.ConvertirNumEnteroCardinalVal(partes[0], signo);
            string parteCentavos = "";
            string parteCentavosVal = "";
            if (partes.Length > 1)
            {
                if (partes[1].Length == 1)
                {
                    partes[1] += "0";
                }
                else if (partes[1].Length > 2)
                {
                    partes[1] = partes[1].Substring(0, 2);
                }
                parteCentavos = Cardinales.ConvertirNumEnteroCardinal(partes[1], false);
                parteCentavosVal = Cardinales.ConvertirNumEnteroCardinalVal(partes[1], false);
            }
            if (partes[0] == "1")
            {
                numCompletoLetras = partePesos + " " + HttpContext.GetGlobalResourceObject("Resource", "Peso").ToString();
                numCompletoLetrasVal = partePesosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Peso").ToString();
            }
            else if (partes[0].Length <= 6)
            {
                numCompletoLetras = partePesos + " " + HttpContext.GetGlobalResourceObject("Resource", "Pesos").ToString();
                numCompletoLetrasVal = partePesosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Pesos").ToString();
            }
            else 
            {
                numCompletoLetras = partePesos + " " + HttpContext.GetGlobalResourceObject("Resource", "DePesos").ToString();
                numCompletoLetrasVal = partePesosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "DePesos").ToString();
            }
            if (!string.IsNullOrEmpty(parteCentavos))
            {
                if (partes[1] == "01")
                {
                    numCompletoLetras += " i " + parteCentavos + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavo").ToString();
                    numCompletoLetrasVal += " i " + parteCentavosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavo").ToString();
                }
                else
                {
                    numCompletoLetras += " i " + parteCentavos + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavos").ToString();
                    numCompletoLetrasVal += " i " + parteCentavosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavos").ToString();
                }
            }

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "PesoTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoNota3").ToString());
            //AÑADIR NOTAS CUANDO ACABES BUENAS!!!!!!!!!!!!!!!

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo4").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo5").ToString().Replace("|", numCompletoLetras));
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);


            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SintagmaNom = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SintagmaNominalOpcion").ToString());
            SintagmaNom.Opciones = new List<string>();
            SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString()+numCompletoLetras);
            SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal);

            if (numero.Contains(".") || numero.Contains(","))
            {
                SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString()+numCompletoLetras.Replace(" i ", " ambs "));
                SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " ambs "));
            }


            /*Opcion NoApropiado = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "NoApropiadoOpcion").ToString());
            NoApropiado.Opciones = new List<string>();
            NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString()+numCompletoLetras.Replace(" i ", " punt "));
            NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " punt "));*/


            resultado.MasOpciones.Add(SintagmaNom);
            if (numero.Contains(".") || numero.Contains(","))
            {
                Opcion NoApropiado = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "NoApropiadoOpcion").ToString());
                NoApropiado.Opciones = new List<string>();
                NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString() + numCompletoLetras.Replace(" i ", " punt "));
                NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " punt "));
                resultado.MasOpciones.Add(NoApropiado);
            }
            return resultado;
        }

        public Conversion ConversionDolar(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras;
            string numCompletoLetrasVal;
            string numeroNoSimbolo = numero.Replace("$", "");
            string[] partes = Regex.Split(numeroNoSimbolo, @"[.,]");

            string parteDolar = Cardinales.ConvertirNumEnteroCardinal(partes[0], signo);
            string parteDolarVal = Cardinales.ConvertirNumEnteroCardinalVal(partes[0], signo);
            string parteCentavos = "";
            string parteCentavosVal = "";
            if (partes.Length > 1)
            {
                if (partes[1].Length == 1)
                {
                    partes[1] += "0";
                }
                else if (partes[1].Length > 2)
                {
                    partes[1] = partes[1].Substring(0, 2);
                }
                parteCentavos = Cardinales.ConvertirNumEnteroCardinal(partes[1], false);
                parteCentavosVal = Cardinales.ConvertirNumEnteroCardinalVal(partes[1], false);
            }
            if (partes[0] == "1")
            {
                numCompletoLetras = parteDolar + " " + HttpContext.GetGlobalResourceObject("Resource", "Dolar").ToString();
                numCompletoLetrasVal = parteDolarVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Dolar").ToString();
            }
            else if (partes[0].Length <= 6)
            {
                numCompletoLetras = parteDolar + " " + HttpContext.GetGlobalResourceObject("Resource", "Dolares").ToString();
                numCompletoLetrasVal = parteDolarVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Dolares").ToString();
            }
            else
            {
                numCompletoLetras = parteDolar + " " + HttpContext.GetGlobalResourceObject("Resource", "DeDolares").ToString();
                numCompletoLetrasVal = parteDolarVal + " " + HttpContext.GetGlobalResourceObject("Resource", "DeDolares").ToString();
            }
            if (!string.IsNullOrEmpty(parteCentavos))
            {
                if (partes[1] == "01")
                {
                    numCompletoLetras += " i " + parteCentavos + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavo").ToString();
                    numCompletoLetrasVal += " i " + parteCentavosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavo").ToString();
                }
                else
                {
                    numCompletoLetras += " i " + parteCentavos + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavos").ToString();
                    numCompletoLetrasVal += " i " + parteCentavosVal + " " + HttpContext.GetGlobalResourceObject("Resource", "Centavos").ToString();
                }
            }

            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "DolarTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "DolarNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "DolarNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "DolarNota3").ToString());
            

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo4").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PesoEjemplo5").ToString().Replace("|", numCompletoLetras));
           
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);


            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SintagmaNom = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SintagmaNominalOpcion").ToString());
            SintagmaNom.Opciones = new List<string>();
            SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString()+numCompletoLetras);
            SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal);
            if (numero.Contains(".") || numero.Contains(","))
            {
                SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString()+numCompletoLetras.Replace(" i ", " ambs "));
                SintagmaNom.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " ambs "));
            }


            /*Opcion NoApropiado = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "NoApropiadoOpcion").ToString());
            NoApropiado.Opciones = new List<string>();
            NoApropiado.Opciones.Add(numCompletoLetras.Replace(" i ", " punt "));*/


            resultado.MasOpciones.Add(SintagmaNom);
            if (numero.Contains(".") || numero.Contains(","))
            {
                Opcion NoApropiado = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "NoApropiadoOpcion").ToString());
                NoApropiado.Opciones = new List<string>();
                NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "CatalanTipo").ToString()+numCompletoLetras.Replace(" i ", " punt "));
                NoApropiado.Opciones.Add(HttpContext.GetGlobalResourceObject("Resource", "ValencianoTipo").ToString() + numCompletoLetrasVal.Replace(" i ", " punt "));
                resultado.MasOpciones.Add(NoApropiado);
            }
            return resultado;
        }

        public Conversion ConversionPoligono(string numero, bool signo, string numeroOriginal, bool valorNum)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Conversion resultado = new Conversion();

            string numCompletoLetras = Poligono.ConvertirNumEnteroPoligno(numero);
            if (string.IsNullOrEmpty(numCompletoLetras))
            {
                return null;
            }


            resultado.Tipo = HttpContext.GetGlobalResourceObject("Resource", "PoligonoTipo").ToString();
            resultado.TitNotas = HttpContext.GetGlobalResourceObject("Resource", "NotasTitulo").ToString();
            resultado.Notas = new List<string>();
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoNota1").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoNota2").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoNota3").ToString());
            resultado.Notas.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoNota4").ToString());
            

            resultado.TitReferencias = HttpContext.GetGlobalResourceObject("Resource", "ReferenciasTitulo").ToString();
            resultado.Referencias = new List<string>();
            resultado.Referencias.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoReferencia1").ToString());
            

            resultado.TitEjemplos = HttpContext.GetGlobalResourceObject("Resource", "EjemplosTitulo").ToString();
            resultado.Ejemplos = new List<string>();
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoEjemplo1").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoEjemplo2").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoEjemplo3").ToString().Replace("|", numCompletoLetras));
            resultado.Ejemplos.Add(HttpContext.GetGlobalResourceObject("Resource", "PoligonoEjemplo4").ToString().Replace("|", numCompletoLetras));
         
            resultado.Respuestas = new List<string>();
            resultado.Respuestas.Add(numCompletoLetras);

            resultado.TitOpciones = HttpContext.GetGlobalResourceObject("Resource", "TituloOpciones").ToString();
            resultado.MasOpciones = new List<Opcion>();

            Opcion SustantivoAdjetivo = new Opcion(HttpContext.GetGlobalResourceObject("Resource", "SustantivoAdjetivoOpcion").ToString());
            SustantivoAdjetivo.Opciones = new List<string>();
            SustantivoAdjetivo.Opciones.Add(numCompletoLetras);

            resultado.MasOpciones.Add(SustantivoAdjetivo);

            return resultado;
        }


    }



}
