using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class AnalizadorSintactico
    {
        static Token tActual = new Token();
        static Token tSiguiente = new Token();
        TokenList Listado = new TokenList();
        public AnalizadorLexico analizlexico = new AnalizadorLexico();
        List<int> listaDeclaracion = new List<int> { 9, 10, 11 };
        List<int> listaInstruccion = new List<int> { 2, 3, 4, 5, 7, 8, 13 };
        List<int> listaAROP = new List<int> { 27, 28, 31, 16, 13, 42 };
        List<int> listaFACTOR = new List<int> {31, 16, 13, 42 };
        List<int> listaCondicion = new List<int> { 16, 42, 13 };
        List<int> listaComparativos = new List<int> {17,18,19,20,21,22};
        bool correcto = true;
        List<Token> listado = new List<Token>();
    public bool main() {
            lexico();
            cflat();
            return correcto;
        }
        void lexico()
        {
            tActual = tSiguiente;
            listado.Add(tActual);
            tSiguiente = analizlexico.siguiente();
        }
        void cflat() {           
            espera(Listado.ListaTokens[0]);
            while (tSiguiente.Codigo!=45 && tSiguiente.Codigo !=1 && !listaInstruccion.Contains(tSiguiente.Codigo)) {
                declaracion();
            }
            while (tSiguiente.Codigo!=45 && tSiguiente.Codigo !=1 )
            {
                instruccion();
            }
            espera(Listado.ListaTokens[1]);
            
        }
        void espera(Token esperado) {
            if (esperado.Codigo == tSiguiente.Codigo)
            {
                lexico();
            }
            else {
                error("Se esperaba " + esperado.Lexema);
                correcto=false;
            }
        }
        void declaracion() {
            if (!listaDeclaracion.Contains(tSiguiente.Codigo)) {
                error("Declaracion Incorrecta de Variable");
                correcto = false;
                while(tSiguiente.Codigo != 37 && !listaDeclaracion.Contains(tSiguiente.Codigo) && !listaInstruccion.Contains(tSiguiente.Codigo) && tSiguiente.Codigo != 45)
                {
                    lexico();
                }
                if (listaInstruccion.Contains(tSiguiente.Codigo)){
                    return ;
                }
                if(tSiguiente.Codigo == 37)
                {
                    lexico();
                    return;
                }
            }
            lexico();
            espera(Listado.ListaTokens[14]);
            while (tSiguiente.Codigo == Listado.ListaTokens[39].Codigo)
            {
                lexico();
                espera(Listado.ListaTokens[14]);
            }
            espera(Listado.ListaTokens[37]);
        }
        void instruccion() {
            if (!listaInstruccion.Contains(tSiguiente.Codigo))
            {
                error("No se reconoce tipo de Instruccion");
                correcto = false;
                while (tSiguiente.Codigo != 37  && !listaInstruccion.Contains(tSiguiente.Codigo) && tSiguiente.Codigo!=34 && tSiguiente.Codigo != 45)
                {
                    lexico();
                }
                if (tSiguiente.Codigo == 37)
                {
                    lexico();
                    return;
                }
                if (tSiguiente.Codigo == 34)
                {
                    return;
                }
            }
            if (tSiguiente.Codigo == Listado.ListaTokens[2].Codigo)
            {
                instRead();
            }
            else if (tSiguiente.Codigo == Listado.ListaTokens[3].Codigo)
            {
                instPrint();
            }
            else if (tSiguiente.Codigo == Listado.ListaTokens[4].Codigo)
            {
                instPrintNL();
            }
            else if (tSiguiente.Codigo == Listado.ListaTokens[5].Codigo)
            {
                instIF();
            }
            else if (tSiguiente.Codigo == Listado.ListaTokens[7].Codigo)
            {
                instFor();
            }
            else if (tSiguiente.Codigo == Listado.ListaTokens[8].Codigo)
            {
                instWhile();
            }
            else if (tSiguiente.Codigo == Listado.ListaTokens[13].Codigo)
            {
                instAsignacion();
            }
        }
        private void instAsignacion()
        {
            lexico();
            if (tSiguiente.Codigo != 26)
            {
                correcto = false;
                error("No se encontro simbolo Igual");
                while (tSiguiente.Codigo != 37)
                {
                    lexico();
                }
                if(tSiguiente.Codigo == 37)
                {
                    lexico();
                    return;
                }
            }
            espera(Listado.ListaTokens[26]);
            arop();
            espera(Listado.ListaTokens[37]);
        }

        private void arop()
        {
            if (!listaAROP.Contains(tSiguiente.Codigo))
            {
                correcto = false;
                error("Se planteo incorrectamente la operacion");
                while (tSiguiente.Codigo != 37 && tSiguiente.Codigo!= 45)
                {
                    lexico();
                }
                lexico();
                return;
            }
            if (tSiguiente.Codigo == 27)
            {
                lexico();
            }
            else if (tSiguiente.Codigo == 28)
            {
                lexico();
            }
            
            term();
            while(tSiguiente.Codigo == 27 || tSiguiente.Codigo == 28)
             {
                lexico();
                term();
             }
                
            
        }

        private void term()
        {
            factor();
            while(tSiguiente.Codigo == 29 || tSiguiente.Codigo == 30 || tSiguiente.Codigo == 44)
            {
                lexico();
                factor();
            }
        }

        private void factor()
        {
            if (!listaFACTOR.Contains(tSiguiente.Codigo))
            {
                correcto = false;
                error("Se planteo incorrectamente la operacion (Factor)");
                while (tSiguiente.Codigo != 37 && tSiguiente.Codigo != 45)
                {
                    lexico();
                }
                lexico();
                return;
            }
            if (tSiguiente.Codigo == 31)
            {
                lexico();
                arop();
                espera(Listado.ListaTokens[32]);
            }else if(tSiguiente.Codigo == 16)
            {
                lexico();
            }else if(tSiguiente.Codigo == 42)
            {
                lexico();
            }else if(tSiguiente.Codigo == 13)
            {
                lexico();
            }
        }

        private void instWhile()
        {
            lexico();
            espera(Listado.ListaTokens[31]);
            condicion();
            espera(Listado.ListaTokens[32]);
            espera(Listado.ListaTokens[33]);
            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo != 34)
            {
                instruccion();
            }
            espera(Listado.ListaTokens[34]);
        }

        private void instFor()
        {
            lexico();
            espera(Listado.ListaTokens[31]);
            espera(Listado.ListaTokens[13]);
            espera(Listado.ListaTokens[38]);
            if(tSiguiente.Codigo==16)
            {
                lexico();
            }else if (tSiguiente.Codigo == 13)
            {
                lexico();
            }
            espera(Listado.ListaTokens[32]);
            espera(Listado.ListaTokens[33]);
            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo!=34)
            {
                instruccion();
            }
            espera(Listado.ListaTokens[34]);
        }

        private void instIF()
        {
            lexico();
            condicion();
            espera(Listado.ListaTokens[33]);
            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo != 34)
            {
                instruccion();
            }
            espera(Listado.ListaTokens[34]);
            if (tSiguiente.Codigo == 6)
            {
                espera(Listado.ListaTokens[33]);
                while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo != 34  )
                {
                    instruccion();
                }
                espera(Listado.ListaTokens[34]);
            }
        }

        private void condicion()
        {
            if (!listaCondicion.Contains(tSiguiente.Codigo))
            {
                error("No se encontro condicion valida");
                correcto = false;
                while (tSiguiente.Codigo!=45 && tSiguiente.Codigo != 32 && tSiguiente.Codigo!=34)
                {
                    lexico();
                }
                lexico();
                return;
            }
                lexico();
                if (listaComparativos.Contains(tSiguiente.Codigo))
                {
                    lexico();
                }
                if (listaCondicion.Contains(tSiguiente.Codigo))
                {
                    lexico();
                }
                if(tSiguiente.Codigo==23 || tSiguiente.Codigo == 24)
                {
                    lexico();
                    condicion2();
                }
            
        }

        private void condicion2()
        {
            
            if (!listaCondicion.Contains(tSiguiente.Codigo))
            {
                correcto = false;
                error("No se encontro condicion2 valida");
                while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 32 && tSiguiente.Codigo != 34)
                {
                    lexico();
                }
                lexico();
                return;
            }
                lexico();
                if (listaComparativos.Contains(tSiguiente.Codigo))
                {
                    lexico();
                }
                if (listaCondicion.Contains(tSiguiente.Codigo))
                {
                    lexico();
                }
            
        }

        private void instPrintNL()
        {
            do
            {
                lexico();
                if (tSiguiente.Codigo == 42)
                {
                    lexico();
                }
                else if (tSiguiente.Codigo == 13)
                {
                    lexico();
                }
                else
                {
                    error("No esta correcto el PRINTNL");
                    correcto = false;
                }
            } while (tSiguiente.Codigo == 39);
            espera(Listado.ListaTokens[37]);
        }

        private void instPrint()
        {
            do
            {
                lexico();
                if (tSiguiente.Codigo == 42)
                {
                    lexico();
                }
                else if (tSiguiente.Codigo == 13)
                {
                    lexico();
                }
                else
                {
                    error("No esta correcto el PRINT");
                    correcto = false;
                }
            } while (tSiguiente.Codigo == 39);
            espera(Listado.ListaTokens[37]);
        }

        void instRead()
        {
            //READ "Texto" variable, variable2
            
            do
            {
                lexico();
                if (tSiguiente.Codigo == 42)
                {
                    lexico();
                }
                espera(Listado.ListaTokens[13]);
            } while (tSiguiente.Codigo == 39);
            espera(Listado.ListaTokens[37]);

        }
        public void error(string mensaje) {
            correcto = false;
            Console.WriteLine("En linea " + tSiguiente.Linea + " Columna " + tSiguiente.Columna + ": "+mensaje);
        }
    }
}
