using System;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
            bool estaBien;
            AnalizadorSintactico analizasintaxis = new();
            estaBien = analizasintaxis.main();
            Console.WriteLine("Termino de Leer el programa");
            if (estaBien)
            {
                Console.WriteLine("No se detecto algun error");
                Traductor traductor = new();
                traductor.main();
                Console.WriteLine("Traduccion terminada");
            }
            Console.ReadKey();
        }
    }
}
