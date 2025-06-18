using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TFTService
{
    public class Multiplicativo
    {
        public static string ConvertirNumEnteroMultiplicativo(string numero, string genero)
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
                        case 2: resultado.Insert(0, genero == "M" ? "doble" : "doble"); break;
                        case 3: resultado.Insert(0, genero == "M" ? "triple" : "tripla"); break;
                    }
                }
                else if (numeroInt >= 4 && numeroInt <= 12)
                {
                    switch (numeroInt)
                    {
                        case 4: resultado.Insert(0, genero == "M" ? "quàdruple" : "quàdrupla"); break;
                        case 5: resultado.Insert(0, genero == "M" ? "quìntuple" : "quìntupla"); break;
                        case 6: resultado.Insert(0, genero == "M" ? "sèxtuple" : "sèxtupla"); break;
                        case 7: resultado.Insert(0, genero == "M" ? "sèptuple" : "sèptupla"); break;
                        case 8: resultado.Insert(0, genero == "M" ? "òctuple" : "òctupla"); break;
                        case 9: resultado.Insert(0, genero == "M" ? "nonùple" : "nonùpla"); break;
                        case 10: resultado.Insert(0, genero == "M" ? "dècuple" : "dècupla"); break;
                        case 11: resultado.Insert(0, genero == "M" ? "undècuple" : "undècupla"); break;
                        case 12: resultado.Insert(0, genero == "M" ? "duodècuple" : "duodècupla"); break;
                    }
                }
                else if (numeroInt == 100)
                {
                    resultado.Insert(0, genero == "M" ? "cèntuple" : "cèntupla");
                }
                else
                {
                    string NumCompletoCard = Cardinales.NuevoConvertirNumEnteroCardinal(numero,false);
                    resultado.Insert(0, NumCompletoCard + " vegades mes");
                }   

            }
            else
            {
                string NumCompletoCard = Cardinales.NuevoConvertirNumEnteroCardinal(numero, false);
                if (string.IsNullOrEmpty(NumCompletoCard))
                {
                    return null;
                }
                resultado.Insert(0, NumCompletoCard + " de vegades mes");
            }
            return resultado.ToString();
        }

        public static string ConvertirNumEnteroMultiplicativoVal(string numero, string genero)
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
                        case 2: resultado.Insert(0, genero == "M" ? "doble" : "doble"); break;
                        case 3: resultado.Insert(0, genero == "M" ? "triple" : "tripla"); break;
                    }
                }
                else if (numeroInt >= 4 && numeroInt <= 12)
                {
                    switch (numeroInt)
                    {
                        case 4: resultado.Insert(0, genero == "M" ? "quàdruple" : "quàdrupla"); break;
                        case 5: resultado.Insert(0, genero == "M" ? "quìntuple" : "quìntupla"); break;
                        case 6: resultado.Insert(0, genero == "M" ? "sèxtuple" : "sèxtupla"); break;
                        case 7: resultado.Insert(0, genero == "M" ? "sèptuple" : "sèptupla"); break;
                        case 8: resultado.Insert(0, genero == "M" ? "òctuple" : "òctupla"); break;
                        case 9: resultado.Insert(0, genero == "M" ? "nonùple" : "nonùpla"); break;
                        case 10: resultado.Insert(0, genero == "M" ? "dècuple" : "dècupla"); break;
                        case 11: resultado.Insert(0, genero == "M" ? "undècuple" : "undècupla"); break;
                        case 12: resultado.Insert(0, genero == "M" ? "duodècuple" : "duodècupla"); break;
                    }
                }
                else if (numeroInt == 100)
                {
                    resultado.Insert(0, genero == "M" ? "cèntuple" : "cèntupla");
                }
                else
                {
                    string NumCompletoCard = Cardinales.NuevoConvertirNumEnteroCardinalVal(numero, false);
                    resultado.Insert(0, NumCompletoCard + " vegades mes");
                }

            }
            else
            {
                string NumCompletoCard = Cardinales.NuevoConvertirNumEnteroCardinalVal(numero, false);
                if (string.IsNullOrEmpty(NumCompletoCard))
                {
                    return null;
                }
                resultado.Insert(0, NumCompletoCard + " de vegades mes");
            }
            return resultado.ToString();
        }
    }
}