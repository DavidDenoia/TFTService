using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TFTService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class TFTServicio : NumToCat
    {
        public PartesNumeros GetNumber(string value)
        {

            bool tieneComa=false;

            if (String.IsNullOrEmpty(value))
            {
                return new PartesNumeros { ParteEntera= "", ParteDecimal=""};
            }

            for(int i=0; i<value.Length; i++)
            {
                char caracter = value[i];

                if(!char.IsDigit(caracter) && caracter != ',')
                {
                    return new PartesNumeros { ParteEntera = "", ParteDecimal = "" };

                }

                if(caracter == ',')
                {
                    tieneComa = true;
                }

            }

            if (tieneComa)
            {
                string[] partes = value.Split(',');

                if (partes.Length != 2) {

                    return new PartesNumeros { ParteEntera = "", ParteDecimal = "" };
                }

                string parteEntera = partes[0];
                string parteDecimal = partes[1];

                return new PartesNumeros { ParteEntera = parteEntera, ParteDecimal = parteDecimal };

            }

            return new PartesNumeros { ParteEntera = value, ParteDecimal=""};
        }

        public string CentenaresALetras(string numero)
        {
            StringBuilder resultado = new StringBuilder();
            string[] unidades = { "zero", "un", "dos", "tres", "quatre", "cinc", "sis", "set", "vuit", "nou" };
            string[] decenas = {"trenta","qauranta","cinquanta","seixanta","setanta","vuitanta","noranta"};
            string[] centenas = { "cent ", "dos-cents ", "tres-cents ", "quatre-cents ", "cinc-cents ", "sis-cents ", "set-cents ", "vuit-cents ", "nou-cents " };
            string[] decenasEspeciales = {"deu","onze","dotze","tretze","catorze", "quinze","setze","disset","divuit","dinou",
                "vint","vint-i-un","vint-i-dos","vint-i-tress","vint-i-quatre","vint-i-cinc", "vint-i-sis","vint-i-set","vint-i-vuit","vint-i-nou"};
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
            if(numIntDecUni >= 10 && numIntDecUni <= 29)
            {
                resultado.Append(decenasEspeciales[numIntDecUni - 10]);
            }
            else
            {
                if(numIntDec >= 3)
                {
                    resultado.Append(decenas[numIntDec-3]);
                    if(numIntUni > 0)
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


        /*public string NumCompletoALetra(string numero)
        {
            
            int contadorSufijos = -1;
            int contadorNumVeces = 0;
            StringBuilder numCentena = new StringBuilder();
            StringBuilder resultado = new StringBuilder();

            string[] sufijos = { "", "mil", "milions", "bilions", "trilions", "quadrilions", "quintilions", "sextilions", "septilions", "octilions", "nonilions","decilions",
                "undecilions", "duodecilions", "tredecilions", "quatourdecilions", "quindecilions", "sexdecilions", "septendecilions", "octodecilions", "novendecilions", 
                "vigintilions" };

            string[] sufijosEspeciales = { "","", "milió", "bilió", "trilió", "quadrilió", "quintilió", "sextilió", "septilió", "octilió", "nonilió", "decilió", 
                "undecilió", "duodecilió", "tredecilió", "quatuordecilió", "quindecilió", "sexdecilió", "septendecilió", "octodecilió", "novendecilió", "vigintilió" };

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                numCentena.Insert(0, numero[i]); 
                contadorNumVeces++;

                if (contadorNumVeces == 3 || i == 0)
                {

                    string numCentenaLetra = CentenaresALetras(numCentena.ToString());
                    if (!string.IsNullOrEmpty(numCentenaLetra))
                    {
                        contadorSufijos++;

                        if (contadorSufijos == 1 && numCentenaLetra == "un")
                        {
                            resultado.Insert(0, sufijos[contadorSufijos]);
                        }
                        else if (contadorSufijos > 1 && numCentenaLetra == "un")
                        {
                            resultado.Insert(0, sufijosEspeciales[contadorSufijos] + " ");
                        }
                        else
                        {
                            if (contadorSufijos > 0)
                            {
                                resultado.Insert(0, numCentenaLetra + " " + sufijos[contadorSufijos] + " ");
                            }
                            else
                            {
                                resultado.Insert(0, numCentenaLetra + " ");
                            }
                        }
                    }
                    contadorNumVeces = 0;
                    numCentena.Clear();
                    //resultado.Insert(0, numCentena); 
                }
            }
            /*if (numCentena.Length > 0)
            {
                //resultado.Insert(0, numCentena);
            }
            return resultado.ToString();
        }*/
        public string NumCompletoALetra(string numero)
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

                    if (!string.IsNullOrEmpty(numCentenaLetra))
                    {
                        if(contadorSufijos == 0)
                        {
                            resultado.Insert(0, numCentenaLetra+" ");
                        }else if(contadorSufijos % 2 == 1)
                        {
                            resultado.Insert(0, numCentenaLetra + "mil ");
                        }
                        else
                        {
                            int sufijoIndice = (contadorSufijos / 2) - 1;
                            resultado.Insert(0, numCentenaLetra + " " + sufijos[sufijoIndice] + " ");
                        }

                    }
                    contadorSufijos++;
                       
                }
            }
            return resultado.ToString().Trim();
        }

    }
}
