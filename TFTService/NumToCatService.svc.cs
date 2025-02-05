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

       

    }
}
