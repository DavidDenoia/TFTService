using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TFTService
{
    public class Multiplicativo
    {
        public static string ConvertirNumEnteroMultiplicativo(string numero)
        {
            int tamañoNumero = numero.Length;
            StringBuilder resultado = new StringBuilder();
            if (tamañoNumero <= 3)
            {

                int numeroInt = int.Parse(numero);
                if (numeroInt == 0 || numeroInt == 1)
                {
                    //resultado.Insert(0, "");
                    return null;
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
                else if (numeroInt == 100)
                {
                    resultado.Insert(0, "Cèntuple");
                }
                else
                {
                    string NumCompletoCard = Cardinales.ConvertirNumEnteroCardinal(numero,false);
                    resultado.Insert(0, NumCompletoCard + " vegades");
                }

            }
            else
            {
                string NumCompletoCard = Cardinales.ConvertirNumEnteroCardinal(numero, false);
                resultado.Insert(0, NumCompletoCard + " de vegades");
            }
            return resultado.ToString();
        }
    }
}