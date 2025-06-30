using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;

namespace TFTService
{
    public class Cardinales
    {

        public static string CentenaresALetras(string numero)
        {
            StringBuilder resultado = new StringBuilder();
            string[] unidades = { "zero", "un", "dos", "tres", "quatre", "cinc", "sis", "set", "vuit", "nou" };
            string[] decenas = { "trenta", "quaranta", "cinquanta", "seixanta", "setanta", "vuitanta", "noranta" };
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

        public static string ConvertirNumDecimalCardinal(string numero)
        {
            StringBuilder resultado = new StringBuilder();
            StringBuilder numCentena = new StringBuilder();

            Dictionary<int, string> sufijosDecimales = new Dictionary<int, string>
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
            if (diferencia == 1)
            {
                sufijoFinal.Append("deu ");
            }
            else if (diferencia == 2) // 5, 8, 14,......
            {
                sufijoFinal.Append("cent ");
            }
            else if (diferencia == 3) // 6, 9, 15,.......
            {
                sufijoFinal.Append("mil ");
            }
            else if (diferencia == 4) // 10, 16, 22,......
            {
                sufijoFinal.Append("deu mil ");
            }
            else if (diferencia == 5) // 11, 17, 23, etc.
            {
                sufijoFinal.Append("cent mil ");
            }

            sufijoFinal.Append(sufijoBase);

            string NumCompleCard = NuevoConvertirNumEnteroCardinal(numero, false);
            if (NumCompleCard == "un")
            {
                resultado.Insert(0, NumCompleCard + " " + sufijoFinal.ToString());
            }
            else
            {
                resultado.Insert(0, NumCompleCard + " " + sufijoFinal.ToString() + "es");
            }

            //return resultado.ToString().Trim();
            return resultado.ToString();
        }

        
        public static string CentenaresALetrasVal(string numero)
        {
            StringBuilder resultado = new StringBuilder();
            string[] unidades = { "zero", "un", "dos", "tres", "quatre", "cinc", "sis", "set", "huit", "nou" };
            string[] decenas = { "trenta", "qauranta", "cinquanta", "seixanta", "setanta", "huitanta", "noranta" };
            string[] centenas = { "cent ", "dos-cents ", "tres-cents ", "quatre-cents ", "cinc-cents ", "sis-cents ", "set-cents ", "huit-cents ", "nou-cents " };
            string[] decenasEspeciales = {"deu","onze","dotze","tretze","catorze", "quinze","setze","dèsset","dihuit","dènou",
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

        public static string ConvertirNumDecimalCardinalVal(string numero)
        {
            StringBuilder resultado = new StringBuilder();
            StringBuilder numCentena = new StringBuilder();

            Dictionary<int, string> sufijosDecimales = new Dictionary<int, string>
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
            if (diferencia == 1)
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

            string NumCompleCard = NuevoConvertirNumEnteroCardinalVal(numero, false);
            if (NumCompleCard == "un")
            {
                resultado.Insert(0, NumCompleCard + " " + sufijoFinal.ToString());
            }
            else
            {
                resultado.Insert(0, NumCompleCard + " " + sufijoFinal.ToString() + "es");
            }

            //return resultado.ToString().Trim();
            return resultado.ToString();
        }


        public static string NuevoConvertirNumEnteroCardinal(string numero, bool signo)
        {
            int contadorSufijos = 0;
            int contadorNumVeces = 0;
            StringBuilder numCentena = new StringBuilder();
            StringBuilder resultado = new StringBuilder();
            string ultimoSufijoZero = null;
            bool haySufijoZero = false;

            if (numero.Length >= 127)
            {
                return null;
            }

            Dictionary<int, (string plural, string singular)> sufijos = new Dictionary<int, (string, string)>()
            {
                { 1, ("milions", "milió") },{ 2, ("bilions", "bilió") },{ 3, ("trilions", "trilió") },{ 4, ("quadrilions", "quadrilió") },
                { 5, ("quintilions", "quintilió") },{ 6, ("sextilions", "sextilió") },{ 7, ("septilions", "septilió") },
                { 8, ("octilions", "octilió") },{ 9, ("nonilions", "nonilió") },{ 10, ("decilions", "decilió") },{ 11, ("undecilions", "undecilió") },
                { 12, ("duodecilions", "duodecilió") },{ 13, ("tredecilions", "tredecilió") },{ 14, ("quatuordecilions", "quatuordecilió") },
                { 15, ("quindecilions", "quindecilió") },{ 16, ("sexdecilions", "sexdecilió") },{ 17, ("septendecilions", "septendecilió") },
                { 18, ("octodecilions", "octodecilió") },{ 19, ("novendecilions", "novendecilió") },{ 20, ("vigintilions", "vigintilió") }
            };

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                numCentena.Insert(0, numero[i]);
                contadorNumVeces++;

                if (contadorNumVeces == 3 || i == 0)
                {
                    string numCentenaLetra = CentenaresALetras(numCentena.ToString());
                    numCentena.Clear();
                    contadorNumVeces = 0;

                    if (!string.IsNullOrEmpty(numCentenaLetra) && numCentenaLetra != "zero")
                    {
                        if (contadorSufijos == 0)
                        {
                            resultado.Insert(0, numCentenaLetra + " ");
                        }
                        else
                        {
                            int sufijoIndice = contadorSufijos / 2;

                            if (contadorSufijos % 2 != 0)
                            {
                                if (numCentenaLetra != "un")
                                {
                                    //resultado.Insert(0, numCentenaLetra + " mil ");

                                    if(haySufijoZero == true)
                                    {
                                        resultado.Insert(0, numCentenaLetra + " mil " + ultimoSufijoZero + " ");
                                        haySufijoZero = false;
                                    }
                                    else
                                    {
                                        resultado.Insert(0, numCentenaLetra + " mil ");
                                    }
                                }
                                else
                                {
                                    //resultado.Insert(0, " mil ");

                                    if (haySufijoZero == true)
                                    {
                                        resultado.Insert(0, " mil " + ultimoSufijoZero + " ");
                                        haySufijoZero = false;
                                    }
                                    else
                                    {
                                        resultado.Insert(0, " mil ");
                                    }
                                }
                            }
                            else
                            {
                                if (sufijos.ContainsKey(sufijoIndice))
                                {
                                    var sufijo = numCentenaLetra == "un" ? sufijos[sufijoIndice].singular : sufijos[sufijoIndice].plural;

                                    resultado.Insert(0, numCentenaLetra + " " + sufijo + " ");
                                }
                                else
                                {
                                    resultado.Insert(0, numCentenaLetra + " ");
                                }
                            }
                        }
                    }
                    else
                    {
                        
                        int sufijoIndice = contadorSufijos / 2;
                        if (sufijos.ContainsKey(sufijoIndice))
                        {
                            ultimoSufijoZero = sufijos[sufijoIndice].plural;
                            haySufijoZero = true;
                            System.Diagnostics.Debug.WriteLine($"Guardado sufijo por bloque zero: {ultimoSufijoZero}");
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"grupo: {numCentenaLetra}, Sufijo #: {contadorSufijos}, Resultado parcial: {resultado}");

                    contadorSufijos++; 
                }
            }


            System.Diagnostics.Debug.WriteLine($"ULTIMO SUFIJO: {ultimoSufijoZero}");

            /*if (!string.IsNullOrEmpty(ultimoSufijoZero) && resultado.ToString().Trim() != "zero")
            {
                resultado.Append(" " + ultimoSufijoZero);
            }*/

            int cerosFinales = 0;
            for (int i = numero.Length - 1; i >= 0 && numero[i] == '0'; i--)
            {
                cerosFinales++;
            }

            string last = resultado.ToString().Trim().Split(' ').LastOrDefault();
            System.Diagnostics.Debug.WriteLine($"LAST: {last}");
            if (!sufijos.Values.Any(s => s.plural == last || s.singular == last) && cerosFinales >=6)
            {
                resultado.Append(" " + ultimoSufijoZero);
            }

            if (resultado.Length == 0)
                resultado.Append("zero");

            if (signo)
                resultado.Insert(0, "minus ");

            return resultado.ToString().Trim();
        }

        public static string NuevoConvertirNumEnteroCardinalVal(string numero, bool signo)
        {
            int contadorSufijos = 0;
            int contadorNumVeces = 0;
            StringBuilder numCentena = new StringBuilder();
            StringBuilder resultado = new StringBuilder();
            string ultimoSufijoZero = null;
            bool haySufijoZero = false;

            if (numero.Length >= 127)
            {
                return null;
            }

            Dictionary<int, (string plural, string singular)> sufijos = new Dictionary<int, (string, string)>()
            {
                { 1, ("milions", "milió") },{ 2, ("bilions", "bilió") },{ 3, ("trilions", "trilió") },{ 4, ("quadrilions", "quadrilió") },
                { 5, ("quintilions", "quintilió") },{ 6, ("sextilions", "sextilió") },{ 7, ("septilions", "septilió") },
                { 8, ("octilions", "octilió") },{ 9, ("nonilions", "nonilió") },{ 10, ("decilions", "decilió") },{ 11, ("undecilions", "undecilió") },
                { 12, ("duodecilions", "duodecilió") },{ 13, ("tredecilions", "tredecilió") },{ 14, ("quatuordecilions", "quatuordecilió") },
                { 15, ("quindecilions", "quindecilió") },{ 16, ("sexdecilions", "sexdecilió") },{ 17, ("septendecilions", "septendecilió") },
                { 18, ("octodecilions", "octodecilió") },{ 19, ("novendecilions", "novendecilió") },{ 20, ("vigintilions", "vigintilió") }
            };

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                numCentena.Insert(0, numero[i]);
                contadorNumVeces++;

                if (contadorNumVeces == 3 || i == 0)
                {
                    string numCentenaLetra = CentenaresALetrasVal(numCentena.ToString());
                    numCentena.Clear();
                    contadorNumVeces = 0;

                    if (!string.IsNullOrEmpty(numCentenaLetra) && numCentenaLetra != "zero")
                    {
                        if (contadorSufijos == 0)
                        {
                            resultado.Insert(0, numCentenaLetra + " ");
                        }
                        else
                        {
                            int sufijoIndice = contadorSufijos / 2;

                            if (contadorSufijos % 2 != 0)
                            {
                                if (numCentenaLetra != "un")
                                {
                                    //resultado.Insert(0, numCentenaLetra + " mil ");

                                    if (haySufijoZero == true)
                                    {
                                        resultado.Insert(0, numCentenaLetra + " mil " + ultimoSufijoZero + " ");
                                        haySufijoZero = false;
                                    }
                                    else
                                    {
                                        resultado.Insert(0, numCentenaLetra + " mil ");
                                    }
                                }
                                else
                                {
                                    //resultado.Insert(0, " mil ");

                                    if (haySufijoZero == true)
                                    {
                                        resultado.Insert(0, " mil " + ultimoSufijoZero + " ");
                                        haySufijoZero = false;
                                    }
                                    else
                                    {
                                        resultado.Insert(0, " mil ");
                                    }
                                }
                            }
                            else
                            {
                                if (sufijos.ContainsKey(sufijoIndice))
                                {
                                    var sufijo = numCentenaLetra == "un" ? sufijos[sufijoIndice].singular : sufijos[sufijoIndice].plural;

                                    resultado.Insert(0, numCentenaLetra + " " + sufijo + " ");
                                }
                                else
                                {
                                    resultado.Insert(0, numCentenaLetra + " ");
                                }
                            }
                        }
                    }
                    else
                    {

                        int sufijoIndice = contadorSufijos / 2;
                        if (sufijos.ContainsKey(sufijoIndice))
                        {
                            ultimoSufijoZero = sufijos[sufijoIndice].plural;
                            haySufijoZero = true;
                            System.Diagnostics.Debug.WriteLine($"Guardado sufijo por bloque zero: {ultimoSufijoZero}");
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"grupo: {numCentenaLetra}, Sufijo #: {contadorSufijos}, Resultado parcial: {resultado}");

                    contadorSufijos++;
                }
            }


            System.Diagnostics.Debug.WriteLine($"ULTIMO SUFIJO: {ultimoSufijoZero}");

            /*if (!string.IsNullOrEmpty(ultimoSufijoZero) && resultado.ToString().Trim() != "zero")
            {
                resultado.Append(" " + ultimoSufijoZero);
            }*/

            int cerosFinales = 0;
            for (int i = numero.Length - 1; i >= 0 && numero[i] == '0'; i--)
            {
                cerosFinales++;
            }

            string last = resultado.ToString().Trim().Split(' ').LastOrDefault();
            System.Diagnostics.Debug.WriteLine($"LAST: {last}");
            if (!sufijos.Values.Any(s => s.plural == last || s.singular == last) && cerosFinales >= 6)
            {
                resultado.Append(" " + ultimoSufijoZero);
            }

            if (resultado.Length == 0)
                resultado.Append("zero");

            if (signo)
                resultado.Insert(0, "minus ");

            return resultado.ToString().Trim();
        }



    }
}