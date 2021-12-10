using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    
    public class Traductor
    {
        static Token tActual = new Token();
        static Token tSiguiente = new Token();
        Logger log = new Logger();
        SegmentoDeCodigo segmentoDeCodigo = new SegmentoDeCodigo();
        TokenList Listado = new TokenList();
        public AnalizadorLexico analizlexico = new AnalizadorLexico();
        List<int> listaDeclaracion = new List<int> { 9, 10, 11 };
        List<int> listaInstruccion = new List<int> { 2, 3, 4, 5, 7, 8, 13 };
        List<int> listaAROP = new List<int> { 27, 28, 31, 16, 13, 42 };
        List<int> listaFACTOR = new List<int> { 31, 16, 13, 42 };
        List<int> listaCondicion = new List<int> { 16, 42, 13 };
        List<int> listaComparativos = new List<int> { 17, 18, 19, 20, 21, 22 };
        bool correcto = true;
        List<Token> listado = new List<Token>();
        Stack<string> verinstruccion = new Stack<string>();
        Stack<string> codigoASE = new Stack<string>();
        string[] dividirINCS = new string[2];

        string auxiliar;
        int contCodigo = 0, guardaSaltos, x=0;
        Stack<int> contadorEtiquetas = new Stack<int>();
        Queue<string> queFor = new Queue<string>();
        Queue<string> queIF = new Queue<string>();
        Dictionary<string, int> reserved = new Dictionary<string, int>() { { "NOP", 1 }, { "ADD", 1 }, { "SUB", 1 }, { "MULT", 1 }, { "DIV", 1 }, { "MOD", 1 }, { "INC", 3 }, { "DEC", 3 }, { "CMPEQ", 1 },
                { "CMPNE", 1 }, { "CMPLT", 1 }, { "CMPLE", 1 }, { "CMPGT", 1 }, { "CMPGE", 1 }, { "JMP", 3 }, { "JMPT", 3 }, { "JMPF", 3 }, { "SETIDX", 3 },
                { "SETIDXK", 5 }, { "PUSHI", 3 }, { "PUSHD", 3 }, { "PUSHS" , 3 }, { "PUSHAI" , 3 }, { "PUSHAD" , 3 }, { "PUSHAS" , 3 }, { "PUSHKI" , 5 },
                { "PUSHKD" , 9 }, { "PUSHKS" , 2 }, { "POPI" , 3 }, { "POPD" , 3 }, { "POPS" , 3 }, { "POPAI" , 3 }, { "POPAD" , 3 }, { "POPAS" , 3 },
                { "POPIDX" , 1 }, { "READI" , 3 }, { "READD" , 3 }, { "READS" , 3 }, { "READAI" , 3 }, { "READAD" , 3 }, { "READAS" , 3 }, { "PRTM" , 2 },
                { "PRTI" , 3 }, { "PRTD" , 3 }, { "PRTS" , 3 }, { "PRTAI" , 3 }, { "PRTAD" , 3 }, { "PRTAS" , 3 }, {"NL",1 },{ "HALT" , 1 }
            };
        Dictionary<string, int> reservedwithCode = new Dictionary<string, int>() { { "NOP", 0 }, { "ADD", 1 }, { "SUB", 2 }, { "MULT", 3 }, { "DIV", 4 }, { "MOD", 5 }, { "INC", 6 }, { "DEC", 7 }, { "CMPEQ", 8 },
                { "CMPNE", 9 }, { "CMPLT", 10 }, { "CMPLE", 11 }, { "CMPGT", 12 }, { "CMPGE", 13 }, { "JMP", 14 }, { "JMPT", 15 }, { "JMPF", 16 }, { "SETIDX", 17 },
                { "SETIDXK", 18 }, { "PUSHI", 19 }, { "PUSHD", 20 }, { "PUSHS" , 21 }, { "PUSHAI" , 22 }, { "PUSHAD" , 23 }, { "PUSHAS" , 24 }, { "PUSHKI" , 25 },
                { "PUSHKD" , 26 }, { "PUSHKS" , 27 }, { "POPI" , 28 }, { "POPD" , 29 }, { "POPS" , 30 }, { "POPAI" , 31 }, { "POPAD" , 32 }, { "POPAS" , 33 },
                { "POPIDX" , 34 }, { "READI" , 35 }, { "READD" , 36 }, { "READS" , 37 }, { "READAI" , 38 }, { "READAD" , 39 }, { "READAS" , 40 }, { "PRTM" , 41 },
                { "PRTI" , 42 }, { "PRTD" , 43 }, { "PRTS" , 44 }, { "PRTAI" , 45 }, { "PRTAD" , 46 }, { "PRTAS" , 47 }, { "NL",48 }, { "HALT" , 49 }
            };
        
        
public bool main()
        {
            analizlexico.reiniciarLector();
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
        void cflat()
        {
            espera(Listado.ListaTokens[0]);
            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && !listaInstruccion.Contains(tSiguiente.Codigo))
            {
                declaracion();
            }
            log.PrintTSNV(analizlexico.variables.Elementos);
            byte[] bytestsnv = log.tsnvEnBytes.ToArray();

            File.WriteAllBytes("./compiladoroutput2.tsnv", bytestsnv);

            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1)
            {
                instruccion();
            }
            espera(Listado.ListaTokens[1]);
            verinstruccion.Push("HALT");
            contCodigo += 1;
            while (verinstruccion.Count>0)
            {
                auxiliar = verinstruccion.Pop();
                if(auxiliar=="JMPT " || auxiliar == "JMPF ")
                {
                    if (contadorEtiquetas.Count > 0)
                    {

                        auxiliar = auxiliar + contadorEtiquetas.Pop();
                    }
                }
                if (auxiliar.Contains("PUSH"))
                {
                    if (verinstruccion.Count > 0)
                    {
                        if (verinstruccion.Peek() == "MULT" || verinstruccion.Peek() == "ADD" || verinstruccion.Peek() == "SUB" || verinstruccion.Peek() == "DIV")
                        {
                            codigoASE.Push(verinstruccion.Pop());
                        }
                    }
                }
                
                if (auxiliar.Contains("INCPUSH"))
                {
                    dividirINCS = auxiliar.Split(" ");
                    auxiliar = dividirINCS[0].Substring(0, 3) +" "+ dividirINCS[1];
                }
                codigoASE.Push(auxiliar);
            }
            auxiliar = "";
            Console.WriteLine(codigoASE.Count);
            while (codigoASE.Count>0)
            {
                auxiliar = codigoASE.Pop();
                ElementoSegmentoDeCodigo elemCodigo = new ElementoSegmentoDeCodigo();
                dividirINCS = auxiliar.Split(" ");
                elemCodigo.NumeroDeCodigo = reservedwithCode.Where(x => x.Key == dividirINCS[0]).Select(y => y.Value).FirstOrDefault();
                elemCodigo.PesoComando = reserved.Where(x => x.Key == dividirINCS[0]).Select(y => y.Value).FirstOrDefault();
                if (dividirINCS.Length == 2)
                {
                    if (analizlexico.variables.buscarVariable(dividirINCS[1]) != -1) {
                        x = analizlexico.variables.buscarVariable(dividirINCS[1]);
                        elemCodigo.DireccionVariable = analizlexico.variables.Elementos[x].Direccion;
                        elemCodigo.VariableType = analizlexico.variables.Elementos[x].VariableType;
                    }
                    else
                    {
                        if (elemCodigo.NumeroDeCodigo == 15 || elemCodigo.NumeroDeCodigo == 16)
                        {
                            elemCodigo.DireccionVariable = int.Parse(dividirINCS[1]);
                        }
                        if(elemCodigo.NumeroDeCodigo == 41)
                        {
                            elemCodigo.ValorConstante = dividirINCS[1];
                        }
                            elemCodigo.ValorConstante = dividirINCS[1];
                        
                    }
                }else if (elemCodigo.NumeroDeCodigo == 41)
                {
                    elemCodigo.ValorConstante = auxiliar.Substring(4,auxiliar.Length-4).Trim();
                }
                segmentoDeCodigo.Elementos.Add(elemCodigo);
            }
            x = analizlexico.variables.Elementos.Count;
            log.PrintMagicNumber();
            log.cambiarA2bytes(contCodigo);
            log.cambiarA2bytes(x);
            log.cambiarA2bytes(analizlexico.variables.Elementos[x-1].VectorString);
            log.PrintCodigodeTSN(segmentoDeCodigo.Elementos);
            byte[] ArregloTSN = log.codigoCompleto.ToArray();
            File.WriteAllBytes("./compiladoroutput2.tsn",ArregloTSN);

        }
        void espera(Token esperado)
        {
            if (esperado.Codigo == tSiguiente.Codigo)
            {
                lexico();
            }
            else
            {
                error("Se esperaba " + esperado.Lexema);
                correcto = false;
            }
        }
        void declaracion()
        {
            if (!listaDeclaracion.Contains(tSiguiente.Codigo))
            {
                error("Declaracion Incorrecta de Variable");
                correcto = false;
                while (tSiguiente.Codigo != 37 && !listaDeclaracion.Contains(tSiguiente.Codigo) && !listaInstruccion.Contains(tSiguiente.Codigo) && tSiguiente.Codigo != 45)
                {
                    lexico();
                }
                if (listaInstruccion.Contains(tSiguiente.Codigo))
                {
                    return;
                }
                if (tSiguiente.Codigo == 37)
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
        void instruccion()
        {
            if (!listaInstruccion.Contains(tSiguiente.Codigo))
            {
                error("No se reconoce tipo de Instruccion");
                correcto = false;
                while (tSiguiente.Codigo != 37 && !listaInstruccion.Contains(tSiguiente.Codigo) && tSiguiente.Codigo != 34 && tSiguiente.Codigo != 45)
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
                verinstruccion.Push(auxiliar);
                contCodigo += 3;
                
            }
        }
        private void instAsignacion()
        {
            switch (int.Parse(tSiguiente.Tipo))
            {
                case 1:
                    auxiliar="POPI " +tSiguiente.Lexema;
                    break;
                case 2:
                    auxiliar = ("POPD " + tSiguiente.Lexema);
                    break;
                case 3:
                    auxiliar = ("POPS " + tSiguiente.Lexema);
                    break;
                case 11:
                    auxiliar = ("POPAI " + tSiguiente.Lexema);
                    break;
                case 12:
                    auxiliar = ("POPAD " + tSiguiente.Lexema);
                    break;
                case 13:
                    auxiliar = ("POPAS " + tSiguiente.Lexema);
                    break;

            }
            lexico();
            if (tSiguiente.Codigo != 26)
            {
                correcto = false;
                error("No se encontro simbolo Igual");
                while (tSiguiente.Codigo != 37)
                {
                    lexico();
                }
                if (tSiguiente.Codigo == 37)
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
                while (tSiguiente.Codigo != 37 && tSiguiente.Codigo != 45)
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
            while (tSiguiente.Codigo == 27 || tSiguiente.Codigo == 28)
            {
                if (tSiguiente.Codigo == 27)
                {
                    verinstruccion.Push("ADD");
                    contCodigo += 1;
                }
                if (tSiguiente.Codigo == 28)
                {
                    verinstruccion.Push("SUB");
                    contCodigo += 1;
                }
                lexico();
                term();
            }


        }

        private void term()
        {
            factor();
            while (tSiguiente.Codigo == 29 || tSiguiente.Codigo == 30 || tSiguiente.Codigo == 44)
            {
                if (tSiguiente.Codigo == 29)
                {
                    verinstruccion.Push("MULT");
                    contCodigo += 1;
                }
                if (tSiguiente.Codigo == 30)
                {
                    verinstruccion.Push("DIV");
                    contCodigo += 1;
                }

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
            }
            else if (tSiguiente.Codigo == 16)
            {
                if(tSiguiente.Tipo == "1")
                {
                    verinstruccion.Push("PUSHKI "+tSiguiente.Valor);
                    contCodigo += 5;
                }
                if(tSiguiente.Tipo == "2")
                {
                    verinstruccion.Push("PUSHKD " + tSiguiente.Valor);
                    contCodigo += 9;
                }
                lexico();
            }
            else if (tSiguiente.Codigo == 42)
            {
                verinstruccion.Push("PUSHKS "+tSiguiente.Valor.Trim());
                contCodigo = tSiguiente.Valor.Trim().Length + 2;
                lexico();
            }
            else if (tSiguiente.Codigo == 13)
            {
                switch (int.Parse(tSiguiente.Tipo))
                {
                    case 1:
                        verinstruccion.Push("PUSHI " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 2:
                        verinstruccion.Push("PUSHD " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 3:
                        verinstruccion.Push("PUSHS " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 11:
                        verinstruccion.Push("PUSHAI " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 12:
                        verinstruccion.Push("PUSHAD " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 13:
                        verinstruccion.Push("PUSHAS " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;

                }
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
            contadorEtiquetas.Push(contCodigo);
            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo != 34)
            {
                instruccion();
            }
            contCodigo += 3;
            verinstruccion.Push("JMPT "+contadorEtiquetas.Pop());

            espera(Listado.ListaTokens[34]);
        }

        private void instFor()
        {
            lexico();
            
            espera(Listado.ListaTokens[31]);
            switch (int.Parse(tSiguiente.Tipo))
            {
                case 1:
                    queFor.Enqueue("PUSHI " + tSiguiente.Lexema);
                    //contCodigo += 3;
                    break;
                case 2:
                    queFor.Enqueue("PUSHD " + tSiguiente.Lexema);
                    //contCodigo += 3;
                    break;
                case 3:
                    queFor.Enqueue("PUSHS " + tSiguiente.Lexema);
                    //contCodigo += 3;
                    break;
                case 11:
                    queFor.Enqueue("PUSHAI " + tSiguiente.Lexema);
                    //contCodigo += 3;
                    break;
                case 12:
                    queFor.Enqueue("PUSHAD " + tSiguiente.Lexema);
                    //contCodigo += 3;
                    break;
                case 13:
                    queFor.Enqueue("PUSHAS " + tSiguiente.Lexema);
                    //contCodigo += 3;
                    break;

            }
            espera(Listado.ListaTokens[13]);
            espera(Listado.ListaTokens[38]);
            if (tSiguiente.Codigo == 16)
            {
                switch (int.Parse(tSiguiente.Tipo))
                {
                    case 1:
                        queFor.Enqueue("PUSHKI " + tSiguiente.Valor);
                       // contCodigo += 5;
                        break;
                    case 2:
                        queFor.Enqueue("PUSHKD " + tSiguiente.Valor);
                        //contCodigo += 39;
                        break;

                }
                lexico();
            }
            else if (tSiguiente.Codigo == 13)
            {
                switch (int.Parse(tSiguiente.Tipo))
                {
                    case 1:
                        queFor.Enqueue("PUSHI " + tSiguiente.Lexema);
                        break;
                    case 2:
                        queFor.Enqueue("PUSHD " + tSiguiente.Lexema);
                        break;
                    case 3:
                        queFor.Enqueue("PUSHS " + tSiguiente.Lexema);
                        break;
                    case 11:
                        queFor.Enqueue("PUSHAI " + tSiguiente.Lexema);
                        break;
                    case 12:
                        queFor.Enqueue("PUSHAD " + tSiguiente.Lexema);
                        break;
                    case 13:
                        queFor.Enqueue("PUSHAS " + tSiguiente.Lexema);
                        break;

                }
                lexico();
            }
            espera(Listado.ListaTokens[32]);
            espera(Listado.ListaTokens[33]);
            contadorEtiquetas.Push(contCodigo);
            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo != 34)
            {
                instruccion();
            }
            verinstruccion.Push("INC"+queFor.Peek());
            contCodigo += 3;
            while (queFor.Count > 0)
            {
                verinstruccion.Push(queFor.Dequeue());
                if (verinstruccion.Peek().Contains("PUSHI"))
                {
                    contCodigo += 3;
                }else if (verinstruccion.Peek().Contains("PUSHD"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("PUSHS"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("PUSHKI"))
                {
                    contCodigo += 5;
                }
                else if (verinstruccion.Peek().Contains("PUSHKD"))
                {
                    contCodigo += 9;
                }
            }
            verinstruccion.Push("CMPLT");
            contCodigo += 1;
            contCodigo += 3;
            verinstruccion.Push("JMPT " + contadorEtiquetas.Pop());
            
            espera(Listado.ListaTokens[34]);
        }

        private void instIF()
        {
            lexico();
            condicion();
            espera(Listado.ListaTokens[33]);
            
            while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo != 34)
            {
                
                verinstruccion.Push("JMPF ");
                contCodigo += 3;
                instruccion();
            }
            contCodigo += 3;
            contadorEtiquetas.Push(contCodigo);
            espera(Listado.ListaTokens[34]);
            while (queIF.Count > 0)
            {
                verinstruccion.Push(queIF.Dequeue());
                if (verinstruccion.Peek().Contains("PUSHI"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("PUSHD"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("PUSHS"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("PUSHKI"))
                {
                    contCodigo += 5;
                }
                else if (verinstruccion.Peek().Contains("PUSHKD"))
                {
                    contCodigo += 9;
                }
                else if (verinstruccion.Peek().Contains("PUSHAI"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("PUSHAD"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("PUSHAS"))
                {
                    contCodigo += 3;
                }
                else if (verinstruccion.Peek().Contains("CMP"))
                {
                    contCodigo += 1;
                }

            }

            verinstruccion.Push("JMPT ");

            

            if (tSiguiente.Codigo == 6)
            {
                //contCodigo += 3;
                espera(Listado.ListaTokens[33]);
                contadorEtiquetas.Push(contCodigo);
                while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 1 && tSiguiente.Codigo != 34)
                {
                    instruccion();
                }
                espera(Listado.ListaTokens[34]);
            }
            contadorEtiquetas.Push(contCodigo);
            
        }

        private void condicion()
        {
            if (!listaCondicion.Contains(tSiguiente.Codigo))
            {
                error("No se encontro condicion valida");
                correcto = false;
                while (tSiguiente.Codigo != 45 && tSiguiente.Codigo != 32 && tSiguiente.Codigo != 34)
                {
                    lexico();
                }
                lexico();
                return;
            }
            switch (tSiguiente.Codigo)
            {
                case 13:
                    switch (int.Parse(tSiguiente.Tipo))
                    {
                        case 1:
                            verinstruccion.Push("PUSHI " + tSiguiente.Lexema);
                            queIF.Enqueue("PUSHI " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 2:
                            verinstruccion.Push("PUSHD " + tSiguiente.Lexema);
                            queIF.Enqueue("PUSHD " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 3:
                            verinstruccion.Push("PUSHS " + tSiguiente.Lexema);
                            queIF.Enqueue("PUSHS " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 11:
                            verinstruccion.Push("PUSHAI " + tSiguiente.Lexema);
                            queIF.Enqueue("PUSHAI " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 12:
                            verinstruccion.Push("PUSHAD " + tSiguiente.Lexema);
                            queIF.Enqueue("PUSHAD " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 13:
                            verinstruccion.Push("PUSHAS " + tSiguiente.Lexema);
                            queIF.Enqueue("PUSHAS " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;

                    }
                    break;
                case 16:
                    switch (int.Parse(tSiguiente.Tipo))
                    {
                        case 1:
                            verinstruccion.Push("PUSHKI " + tSiguiente.Valor);
                            queIF.Enqueue("PUSHKI " + tSiguiente.Valor);
                            contCodigo += 5;
                            break;
                        case 2:
                            verinstruccion.Push("PUSHKD " + tSiguiente.Valor);
                            queIF.Enqueue("PUSHKD " + tSiguiente.Valor);
                            contCodigo += 9;
                            break;

                    }
                    break;
                case 42:
                    verinstruccion.Push("PUSHKS " + tSiguiente.Valor.Trim());
                    queIF.Enqueue("PUSHKS " + tSiguiente.Valor.Trim());
                    contCodigo += 2 + tSiguiente.Valor.Trim().Length;
                    break;
            }
            lexico();
            if (listaComparativos.Contains(tSiguiente.Codigo))
            {
                lexico();
            }
            if (listaCondicion.Contains(tSiguiente.Codigo))
            {
                switch (tSiguiente.Codigo)
                {
                    case 13:
                        switch (int.Parse(tSiguiente.Tipo))
                        {
                            case 1:
                                verinstruccion.Push("PUSHI " + tSiguiente.Lexema);
                                queIF.Enqueue("PUSHI " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 2:
                                verinstruccion.Push("PUSHD " + tSiguiente.Lexema);
                                queIF.Enqueue("PUSHD " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 3:
                                verinstruccion.Push("PUSHS " + tSiguiente.Lexema);
                                queIF.Enqueue("PUSHS " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 11:
                                verinstruccion.Push("PUSHAI " + tSiguiente.Lexema);
                                queIF.Enqueue("PUSHAI " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 12:
                                verinstruccion.Push("PUSHAD " + tSiguiente.Lexema);
                                queIF.Enqueue("PUSHAD " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 13:
                                verinstruccion.Push("PUSHAS " + tSiguiente.Lexema);
                                queIF.Enqueue("PUSHAS " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;

                        }
                        break;
                    case 16:
                        switch (int.Parse(tSiguiente.Tipo))
                        {
                            case 1:
                                verinstruccion.Push("PUSHKI " + tSiguiente.Valor);
                                queIF.Enqueue("PUSHKI " + tSiguiente.Valor);
                                contCodigo += 5;
                                break;
                            case 2:
                                verinstruccion.Push("PUSHKD " + tSiguiente.Valor);
                                queIF.Enqueue("PUSHKD " + tSiguiente.Valor);
                                contCodigo += 9;
                                break;

                        }
                        break;
                    case 42:
                        verinstruccion.Push("PUSHKS " + tSiguiente.Valor.Trim());
                        queIF.Enqueue("PUSHKS " + tSiguiente.Valor.Trim());
                        contCodigo += 2 + tSiguiente.Valor.Trim().Length;
                        break;
                }

                switch (tActual.Codigo)
                {
                    case 17:
                        verinstruccion.Push("CMPEQ");
                        queIF.Enqueue("CMPEQ");

                        contCodigo += 1;
                        break;
                    case 18:
                        verinstruccion.Push("CMPNE");
                        queIF.Enqueue("CMPNE");
                        contCodigo += 1;
                        break;
                    case 19:
                        verinstruccion.Push("CMPLE");
                        queIF.Enqueue("CMPLE");
                        contCodigo += 1;
                        break;
                    case 20:
                        verinstruccion.Push("CMPGE");
                        queIF.Enqueue("CMPGE");
                        contCodigo += 1;
                        break;
                    case 21:
                        verinstruccion.Push("CMPLT");
                        queIF.Enqueue("CMPLT");
                        contCodigo += 1;
                        break;
                    case 22:
                        verinstruccion.Push("CMPGT");
                        queIF.Enqueue("CMPGT");
                        contCodigo += 1;
                        break;
                }
                lexico();
            }
            if (tSiguiente.Codigo == 23 || tSiguiente.Codigo == 24)
            {
                switch (tSiguiente.Codigo)
                {
                    case 13:
                        switch (int.Parse(tSiguiente.Tipo))
                        {
                            case 1:
                                verinstruccion.Push("PUSHI " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 2:
                                verinstruccion.Push("PUSHD " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 3:
                                verinstruccion.Push("PUSHS " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 11:
                                verinstruccion.Push("PUSHAI " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 12:
                                verinstruccion.Push("PUSHAD " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;
                            case 13:
                                verinstruccion.Push("PUSHAS " + tSiguiente.Lexema);
                                contCodigo += 3;
                                break;

                        }
                        break;
                    case 16:
                        switch (int.Parse(tSiguiente.Tipo))
                        {
                            case 1:
                                verinstruccion.Push("PUSHKI " + tSiguiente.Valor);
                                contCodigo += 5;
                                break;
                            case 2:
                                verinstruccion.Push("PUSHKD " + tSiguiente.Valor);
                                contCodigo += 9;
                                break;

                        }
                        break;
                    case 42:
                        verinstruccion.Push("PUSHKS " + tSiguiente.Valor.Trim());
                        contCodigo += 2+tSiguiente.Valor.Trim().Length;
                        break;
                }

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
                    verinstruccion.Push("PRTM " + tSiguiente.Valor.Trim());
                    contCodigo += 2+ tSiguiente.Valor.Trim().Length;
                    lexico();
                }
                else if (tSiguiente.Codigo == 13)
                {
                    switch (int.Parse(tSiguiente.Tipo))
                    {
                        case 1:
                            verinstruccion.Push("PRTI " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 2:
                            verinstruccion.Push("PRTD " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 3:
                            verinstruccion.Push("PRTS " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 11:
                            verinstruccion.Push("PRTAI " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 12:
                            verinstruccion.Push("PRTAD " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 13:
                            verinstruccion.Push("PRTAS " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;

                    }
                    lexico();
                }
                else
                {
                    error("No esta correcto el PRTNL");
                    correcto = false;
                }
            } while (tSiguiente.Codigo == 39);
            verinstruccion.Push("NL");
            contCodigo += 1;
            espera(Listado.ListaTokens[37]);
        }

        private void instPrint()
        {
            do
            {
                lexico();
                if (tSiguiente.Codigo == 42)
                {
                    verinstruccion.Push("PRTM "+ tSiguiente.Valor.Trim());
                    contCodigo += 2+tSiguiente.Valor.Trim().Length;
                    lexico();
                }
                else if (tSiguiente.Codigo == 13)
                {
                    switch (int.Parse(tSiguiente.Tipo))
                    {
                        case 1:
                            verinstruccion.Push("PRTI " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 2:
                            verinstruccion.Push("PRTD " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 3:
                            verinstruccion.Push("PRTS " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 11:
                            verinstruccion.Push("PRTAI " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 12:
                            verinstruccion.Push("PRTAD " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;
                        case 13:
                            verinstruccion.Push("PRTAS " + tSiguiente.Lexema);
                            contCodigo += 3;
                            break;

                    }
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
                    verinstruccion.Push("PRTM " + tSiguiente.Valor.Trim());
                    contCodigo += 2+tSiguiente.Valor.Trim().Length;
                    lexico();
                }
                switch (int.Parse(tSiguiente.Tipo))
                {
                    case 1:
                        verinstruccion.Push("READI " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 2:
                        verinstruccion.Push("READD " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 3:
                        verinstruccion.Push("READS " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 11:
                        verinstruccion.Push("READAI " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 12:
                        verinstruccion.Push("READAD " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;
                    case 13:
                        verinstruccion.Push("READAS " + tSiguiente.Lexema);
                        contCodigo += 3;
                        break;

                }
                espera(Listado.ListaTokens[13]);
            } while (tSiguiente.Codigo == 39);
            espera(Listado.ListaTokens[37]);

        }
        public void error(string mensaje)
        {
            correcto = false;
            Console.WriteLine("En linea " + tSiguiente.Linea + " Columna " + tSiguiente.Columna + ": " + mensaje);
        }
    }
}
