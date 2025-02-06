using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TFTService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface NumToCat
    {

        [OperationContract]
        PartesNumeros GetNumber(string value);

        [OperationContract]
        string CentenaresALetras(string numero);
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class PartesNumeros
    {
        string parteEntera = "";
        string parteDecimal = "";

        [DataMember]
        public string ParteEntera
        {
            get { return parteEntera; }
            set { parteEntera = value; }
        }

        [DataMember]
        public string ParteDecimal
        {
            get { return parteDecimal; }
            set { parteDecimal = value; }
        }
    }
}
