using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class AnalizadorLexico
    {
        TokenList Lista = new TokenList(); 
        string linea,temp, acum;
        StreamReader archivo = new StreamReader(PathRepository.CrearPath(@"\CflatProgram.txt"));
        int contLineas = 0, pActual,pAnterior,x=0;
        List<string> Variables = new List<string>();
        string[] defArreglo = new string[4];
        List<Token> tokens = new List<Token>();
        Token tokenTemp = new Token();
        public void Run()
        {
            foreach(Token token in Lista.ListaTokens)
            {
                token.imprimir();
            }
        }


        
    }
}
//foreach (string palabra in linea.Split())
//{
//    foreach (char letra in palabra)
//    {
//        acum += letra;
//        if (ListaTokens.ContainsKey(acum))
//        {
//            Console.Write(ListaTokens[acum] + "\t");
//            pAnterior = ListaTokens[acum];
//            tokenTemp.Lexema = acum;
//            tokenTemp.Codigo = ListaTokens[acum];
//            tokens.Add(tokenTemp);
//            acum = "";

//        }
//    }

//}
//Console.WriteLine("\n ===================");
//Dictionary<string, int> ListaTokens = new Dictionary<string, int> {
//            //INSTRUCCIONES 1-9
//            {"START",1 },
//            {"END",2},
//            {"READ", 3},
//            {"PRINT", 4},
//            {"PRINTNL", 5 },
//                //CONDICIONALES 6-7
//            {"IF",6 },
//            {"ELSE",7 },
//                //FIN CONDICIONALES
//                //CICLOS 8-9
//            {"FOR",8 },
//            {"WHILE",9 },
//                //FIN CICLOS
//            //FIN INSTRUCCIONES

//            //DECLARACION/DETECCION DE VARIABLES 10-17
//            {"INT",10 },
//            {"DOUBLE",11},
//            {"STRING",12},
//            {"ARRAY",13}, // => Tendria que revisar con un metodo, seria TIPO[CANTIDAD]
//            {"VARIABLE",14 }, // => para poner este token, seria revisar si esa variable especifica ha sido definida anteriormente.
//            {"DEFINICION",15 }, // => Despues revisar que despues del tipo sea palabras y no nomas letras o alguna condicional etc.
//            {"COMENTARIO",16}, //PONER un true que si se detecta $ todo lo que le siga hasta encontrar otro $ es comentario
//            {"CONSTANTE",17 }, //Revisar con un tryparse si se puede hacer en int o double.

//            //FIN DECLARACION/DETECCION VARIABLES
//            //OPERADORES RELACIONALES 18-26
//            {"==",18 },
//            {"!=",19},
//            {"<=",20 },
//            {">=",21 },
//            {"<",22 },
//            {">",23 },
//            {"AND",24},
//            {"OR",25},
//            {"NOT",26},
//            //FIN OPERADORES RELACIONALES

//            //OPERADORES ARITMETICOS Y CONTENEDORES 27-36
//            {"=",27 },
//            {"+",28 },
//            {"-",29 },
//            {"*",30 },
//            {"/",31 },
//            {"%",32 },
//            {"(",33 },
//            {")",34 },
//            {"{",35 },
//            {"}",36 },
//            {"“",47 },
//            {"”",48 },
//            {";",50 }

//            //FIN OPERADORES ARITMETICOS Y CONTENEDORES
//        };
//while ((linea = archivo.ReadLine()) != null)
//{
//    linea = linea.TrimEnd(';');
//    foreach (string palabra in linea.Split())
//    {
//        if (int.TryParse(palabra, out int output) || double.TryParse(palabra, out double salida)) //if revisa si es una constante
//        {
//            Console.Write(ListaTokens["CONSTANTE"] + "\t");
//            pAnterior = ListaTokens["CONSTANTE"];
//        }
//        else
//        {
//            if (ListaTokens.ContainsKey(palabra) || palabra.Contains("[")) //IF para revisar si es un arreglo, ya sea definicion de uno o la variable o una palabra reservada
//            {
//                if (palabra.Contains("["))
//                {
//                    if (Variables.Contains(palabra.Split('[')[0])) //IF para revisar si es una variable ya definida
//                    {
//                        Console.Write(ListaTokens["VARIABLE"] + "\t");
//                        pAnterior = ListaTokens["VARIABLE"];
//                    }
//                    else
//                    {
//                        if (ListaTokens.ContainsKey(palabra.Split('[', ']')[0]) && int.TryParse(palabra.Split('[', ']')[1], out int result))
//                        { //Este if es por si es un INT[] o DOUBLE[]
//                            Console.Write(ListaTokens["ARRAY"] + "\t");
//                            pAnterior = ListaTokens["ARRAY"];
//                        }

//                    }
//                }
//                else
//                { //ES una palabra reservada
//                    Console.Write(ListaTokens[palabra] + "\t");
//                    pAnterior = ListaTokens[palabra];
//                }
//            }
//            else
//            {
//                if (Variables.Contains(palabra)) //IF para revisar si es una variable ya definida
//                {
//                    Console.Write(ListaTokens["VARIABLE"] + "\t");
//                    pAnterior = ListaTokens["VARIABLE"];
//                }
//                else
//                {
//                    if (pAnterior >= 10 && pAnterior <= 13) //IF PARA REVISAR SI LA PALABRA ANTERIOR ERA PARA DEFINIR UNA VARIABLE
//                    {
//                        if (palabra[palabra.Length - 1] == ';')
//                        {
//                            //palabra.Remove(palabra.Length-1,1);
//                            temp = palabra.TrimEnd(';');
//                            Variables.Add(temp);
//                            Console.Write(ListaTokens["DEFINICION"] + "\t");
//                            pAnterior = ListaTokens["DEFINICION"];
//                        }
//                        else
//                        {
//                            Variables.Add(palabra);
//                            Console.Write(ListaTokens["DEFINICION"] + "\t");
//                            pAnterior = ListaTokens["DEFINICION"];
//                        }

//                    }
//                    else
//                    {
//                        Console.Write($"Palabra no reconocida en linea {contLineas}" + "\t");
//                    }
//                }

//            }
//        }
//    }
//    contLineas++;
//    Console.WriteLine(" ");
//}

