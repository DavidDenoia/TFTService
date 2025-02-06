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

            string[] unidades = {"zero","un", "dos", "tres", "quatre", "cinc", "sis", "set", "vuit", "nou"};

            string[] decenas = {"deu","onze","dotze","tretze","catorze", "quinze","setze","disset","divuit","dinou",
                "vint","vint-i-un","vint-i-dos","vint-i-tress","vint-i-quatre","vint-i-cinc", "vint-i-sis","vint-i-set","vint-i-vuit","vint-i-nou",
                "trenta","qauranta","cinquanta","seixanta","setanta","vuitanta","noranta"};

            string[] centenas = { "cent ", "dos-cents ", "tres-cents ", "quatre-cents ", "cinc-cents ", "sis-cents ", "set-cents ", "vuit-cents ", "nou-cents " };

            int numInt = int.Parse(numero);

            if (numInt == 0)
            {
                resultado.Append(unidades[0]);
            }

            int numIntCent = numInt / 100;
            int numIntDec = (numInt % 100) / 10;
            int numIntUni = (numInt % 10);

            switch (numIntCent)
            {
                case 0: break;
                case 1: resultado.Append(centenas[0]); break;
                case 2: resultado.Append(centenas[1]); break;
                case 3: resultado.Append(centenas[2]); break;
                case 4: resultado.Append(centenas[3]); break;
                case 5: resultado.Append(centenas[4]); break;
                case 6: resultado.Append(centenas[5]); break;
                case 7: resultado.Append(centenas[6]); break;
                case 8: resultado.Append(centenas[7]); break;
                case 9: resultado.Append(centenas[8]); break;
            }

            switch (numIntDec)
            {
                case 0: break;
                case 1:
                    if (numIntUni == 0) { resultado.Append(decenas[0]); }
                    else if (numIntUni == 1) { resultado.Append(decenas[1]); }
                    else if (numIntUni == 2) { resultado.Append(decenas[2]); }
                    else if (numIntUni == 3) { resultado.Append(decenas[3]); }
                    else if (numIntUni == 4) { resultado.Append(decenas[4]); }
                    else if (numIntUni == 5) { resultado.Append(decenas[5]); }
                    else if (numIntUni == 6) { resultado.Append(decenas[6]); }
                    else if (numIntUni == 7) { resultado.Append(decenas[7]); }
                    else if (numIntUni == 8) { resultado.Append(decenas[8]); }
                    else if (numIntUni == 9) { resultado.Append(decenas[9]); }
                    break;
                case 2:
                    if (numIntUni == 0) { resultado.Append(decenas[10]); }
                    else if (numIntUni == 1) {resultado.Append(decenas[11]); }
                    else if (numIntUni == 2) { resultado.Append(decenas[12]); }
                    else if (numIntUni == 3) { resultado.Append(decenas[13]); }
                    else if (numIntUni == 4) { resultado.Append(decenas[14]); }
                    else if (numIntUni == 5) { resultado.Append(decenas[15]); }
                    else if (numIntUni == 6) { resultado.Append(decenas[16]); }
                    else if (numIntUni == 7) { resultado.Append(decenas[17]); }
                    else if (numIntUni == 8) { resultado.Append(decenas[18]); }
                    else if (numIntUni == 9) { resultado.Append(decenas[19]); }
                    break;
                case 3: resultado.Append(decenas[20]); break;
                case 4: resultado.Append(decenas[21]); break;
                case 5: resultado.Append(decenas[22]); break;
                case 6: resultado.Append(decenas[23]); break;
                case 7: resultado.Append(decenas[24]); break;
                case 8: resultado.Append(decenas[25]); break;
                case 9: resultado.Append(decenas[26]); break;
            }  
            
            if(numIntDec > 2 && numIntUni > 0)
            {
                resultado.Append("-");
            }

            switch (numIntUni)
            {
                case 0: break;
                case 1: resultado.Append(unidades[1]); break;
                case 2: resultado.Append(unidades[2]); break;
                case 3: resultado.Append(unidades[3]); break;
                case 4: resultado.Append(unidades[4]); break;
                case 5: resultado.Append(unidades[5]); break;
                case 6: resultado.Append(unidades[6]); break;
                case 7: resultado.Append(unidades[7]); break;
                case 8: resultado.Append(unidades[8]); break;
                case 9: resultado.Append(unidades[9]); break;
            }

            return resultado.ToString();
        }

    }
}
