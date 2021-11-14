using System;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
            AnalizadorLexico lexico = new();
            lexico.Run();
            Console.ReadKey();
        }
    }
}
