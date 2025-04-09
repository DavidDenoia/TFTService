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
    public interface INumToCat
    {
        [OperationContract]
        //List<Conversion> MainTraducir(string value, string lenguaje);
        (Cabecera, List<Conversion>) MainTraducir(string value, string lenguaje);

        [OperationContract]
        PartesNumeros GetNumber(string value);

        [OperationContract]
        string CentenaresALetras(string numero);

        [OperationContract]
        string NumCompletoALetraCard(string numero);

        [OperationContract]
        string NumCompletoALetraCardDec(string numero);

        [OperationContract]
        string NumCompletoALetraOrd(string numero, string genero);

        [OperationContract]
        string NumCompletoALetraFrac(string numero);

        [OperationContract]
        string NumCompletoALetraMult(string numero);
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

    [DataContract]
    public class Conversion
    {
        private string tipo = "";
        private string titulo = "";
        private List<string> respuestas = null;
        private List<string> respuestasMayuscula = null;
        private string titValorNumerico = "";
        private string valorNumerico = "";
        private string titOpciones = "";
        private List<Opcion> masOpciones = null;
        private List<Opcion> masOpcionesMayuscula = null;
        private string titEjemplos = "";
        private List<string> ejemplos = null;
        private List<string> ejemplosMayuscula = null;
        private string titNotas = "";
        private List<string> notas = null;
        private string titReferencias = "";
        private List<string> referencias = null;
        private bool errorRomano = false;

        [DataMember]
        public string Tipo { get => tipo; set => tipo = value; }
        [DataMember]
        public string Titulo { get => titulo; set => titulo = value; }
        [DataMember]
        public List<string> Respuestas { get => respuestas; set => respuestas = value; }
        [DataMember]
        public List<string> RespuestasMayuscula { get => respuestasMayuscula; set => respuestasMayuscula = value; }
        [DataMember]
        public string TitValorNumerico { get => titValorNumerico; set => titValorNumerico = value; }
        [DataMember]
        public string ValorNumerico { get => valorNumerico; set => valorNumerico = value; }
        [DataMember]
        public string TitOpciones { get => titOpciones; set => titOpciones = value; }
        [DataMember]
        public List<Opcion> MasOpciones { get => masOpciones; set => masOpciones = value; }
        [DataMember]
        public List<Opcion> MasOpcionesMayuscula { get => masOpcionesMayuscula; set => masOpcionesMayuscula = value; }
        [DataMember]
        public string TitEjemplos { get => titEjemplos; set => titEjemplos = value; }
        [DataMember]
        public List<string> Ejemplos { get => ejemplos; set => ejemplos = value; }
        [DataMember]
        public List<string> EjemplosMayuscula { get => ejemplosMayuscula; set => ejemplosMayuscula = value; }
        [DataMember]
        public string TitNotas { get => titNotas; set => titNotas = value; }
        [DataMember]
        public List<string> Notas { get => notas; set => notas = value; }
        [DataMember]
        public string TitReferencias { get => titReferencias; set => titReferencias = value; }
        [DataMember]
        public List<string> Referencias { get => referencias; set => referencias = value; }
        [DataMember]
        public bool ErrorRomano { get => errorRomano; set => errorRomano = value; }
    }

    [DataContract]
    public class Opcion
    {
        private string titulo = "";
        private List<string> opciones = null;

        public Opcion(string titulo)
        {
            this.titulo = titulo;
        }

        [DataMember]
        public string Titulo { get => titulo; set => titulo = value; }
        [DataMember]
        public List<string> Opciones { get => opciones; set => opciones = value; }
    }

    [DataContract]
    public class Cabecera
    {
        private string formateado = "";
        private string titulo = "";

        public Cabecera(string formateado, string titulo)
        {
            this.formateado = formateado;
            this.titulo = titulo;
        }

        [DataMember]
        public string Formateado { get => formateado; set => formateado = value; }
        [DataMember]
        public string Titulo { get => titulo; set => titulo = value; }
    }
}
