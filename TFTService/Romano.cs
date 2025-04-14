using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TFTService
{
    public class Romano
    {
        public static string ConvertirNumRomanoEntero(string numero)
        {
            StringBuilder resultado = new StringBuilder();

            Dictionary<char, int> numeroRomano = new Dictionary<char, int>
                {
                    { 'I', 1 },
                    { 'V', 5 },
                    { 'X', 10 },
                    { 'L', 50 },
                    { 'C', 100 },
                    { 'D', 500 },
                    { 'M', 1000 }
                };

            int total = 0;
            int anterior = 0;

            string romano = numero.ToUpper();

            for(int i = romano.Length - 1; i>=0; i--)
            {
                char c = romano[i];

                if (!numeroRomano.ContainsKey(c))
                {
                    return null;
                }

                int actual = numeroRomano[c];

                if (actual < anterior)
                {
                    total -= actual;
                }
                else
                {
                    total += actual;
                }

                anterior = actual;
            }

            resultado.Append(total);
            return resultado.ToString();
        }
    }
}