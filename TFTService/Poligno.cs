using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace TFTService
{
    public class Poligono
    {
        public static string ConvertirNumEnteroPoligno(string numero)
        {
            StringBuilder resultado = new StringBuilder();

            int numeroConvertido;
            if(!int.TryParse(numero, out numeroConvertido) || numeroConvertido<3 || numeroConvertido>10000)
            {
                System.Diagnostics.Debug.WriteLine("DENTRO DEL IF DE NULL");
                return null;
            }

            System.Diagnostics.Debug.WriteLine("FUERA DEL IF DE NULL");

            Dictionary<int, string> poligonosEspeciales = new Dictionary<int, string>
            {
                    {3, "triangle"}, {4, "quadrilàter"}, {5, "pentàgon"}, {6, "hexàgon"}, {7, "heptàgon"},
                    {8, "octàgon"}, {9, "enneàgon"}, {10, "decàgon"}, {11, "hendecàgon"}, {12, "dodecàgon"},
                    {13, "tridecàgon"}, {14, "tetradecàgon"}, {15, "pentadecàgon"}, {16, "hexadecàgon"},
                    {17, "heptadecàgon"}, {18, "octadecàgon"}, {19, "enneadecàgon"}, {20, "icosàgon"},
                    {30, "triacontàgon"}, {40, "tetracontàgon"}, {50, "pentacontàgon"},
                    {60, "hexacontàgon"}, {70, "heptacontàgon"}, {80, "octacontàgon"}, {90, "enneacontàgon"},
                    {100, "hectàgon"}, {1000, "quilìagon"}, {10000, "miriàgon"}
            };

            if (poligonosEspeciales.ContainsKey(numeroConvertido)) {
                return poligonosEspeciales[numeroConvertido];
            }

            string[] decenas =
            {
                 "", "", "icosa", "triaconta", "tetraconta", "pentaconta",
                 "hexaconta", "heptaconta", "octaconta", "enneaconta"
            };

            string[] unidades =
            {
                 "", "hena", "di", "tri", "tetra", "penta",
                "hexa", "hepta", "octa", "ennea"
            };

            int d = numeroConvertido / 10;
            int u = numeroConvertido % 10;

            if (d >= 2 && d <= 9 && u >= 1 && u <= 9)
            {
                return decenas[d] + "kai" + unidades[u] + "gon";
            }

            string numeroCardinal = Cardinales.ConvertirNumEnteroCardinal(numero, false);

            return resultado.Append("Polígon de " + numeroCardinal + " costats").ToString();

        }
    }
}