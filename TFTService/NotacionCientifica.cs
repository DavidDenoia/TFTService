﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace TFTService
{
    public class NotacionCientifica
    {
        public static string ExpandirNotacionCientificaPositiva(string numero, int exponente)
        {
            string resultado = "";
            bool comaEncontrada = false;
            int expRestante = exponente;
            int posicionComa = -1;

            for (int i = 0; i < numero.Length; i++)
            {
                if (numero[i] == '.' || numero[i] == ',')
                {
                    comaEncontrada = true;
                    posicionComa = resultado.Length; 
                    continue; 
                }

                resultado += numero[i];

                if (comaEncontrada)
                {
                    expRestante--;
                }
            }

            if (expRestante > 0)
            {
                
                resultado += new string('0', expRestante);
            }
            else if (expRestante < 0)
            {
                
                int nuevaPosicionComa = resultado.Length + expRestante; 

                if (nuevaPosicionComa > 0)
                {
                    
                    resultado = resultado.Insert(nuevaPosicionComa, ".");
                }
                
            }

            System.Diagnostics.Debug.WriteLine("RESULTADO FINAL: " + resultado);
            return resultado;

        }
    }
}