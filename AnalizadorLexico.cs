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
        Dictionary<string, int> ListaTokens = new Dictionary<string, int> {
            //INSTRUCCIONES 1-9
            {"START",1 },
            {"END",2},
            {"READ", 3},
            {"PRINT", 4},
            {"PRINTNL", 5 },
                //CONDICIONALES 6-7
            {"IF",6 },
            {"ELSE",7 },
                //FIN CONDICIONALES
                //CICLOS 8-9
            {"FOR",8 },
            {"WHILE",9 },
                //FIN CICLOS
            //FIN INSTRUCCIONES

            //DECLARACION/DETECCION DE VARIABLES 10-17
            {"INT",10 },
            {"DOUBLE",11},
            {"STRING",12},
            {"ARRAY",13}, // => Tendria que revisar con un metodo, seria TIPO[CANTIDAD]
            {"VARIABLE",14 }, // => para poner este token, seria revisar si esa variable especifica ha sido definida anteriormente.
            {"DEFINICION",15 }, // => Despues revisar que despues del tipo sea palabras y no nomas letras o alguna condicional etc.
            {"COMENTARIO",16}, //PONER un true que si se detecta $ todo lo que le siga hasta encontrar otro $ es comentario
            {"constante",17 }, //Revisar con un tryparse si se puede hacer en int o double.

            //FIN DECLARACION/DETECCION VARIABLES
            //OPERADORES RELACIONALES 18-26
            {"==",18 },
            {"!=",19},
            {"<=",20 },
            {">=",21 },
            {"<",22 },
            {">",23 },
            {"AND",24},
            {"OR",25},
            {"NOT",26},
            //FIN OPERADORES RELACIONALES

            //OPERADORES ARITMETICOS Y CONTENEDORES 27-36
            {"=",27 },
            {"+",28 },
            {"-",29 },
            {"*",30 },
            {"/",31 },
            {"%",32 },
            {"(",33 },
            {")",34 },
            {"{",35 },
            {"}",36 },
            //FIN OPERADORES ARITMETICOS Y CONTENEDORES
        };
        string linea, palabra;
        StreamReader archivo = new StreamReader(PathRepository.CrearPath(@"\CflatProgram.txt"));
        int contLineas = 0, pActual,pAnterior;
        List<string> Variables = new List<string>();
        string[] defArreglo = new string[4];
        public void Run()
        {
            while ((linea= archivo.ReadLine())!=null)
            {
                
                foreach(string palabra in linea.Split())
                {
                    if (ListaTokens.ContainsKey(palabra)) //IF para revisar si es un comando/reserved word
                    {
                        if (palabra.Contains('['))
                        {
                            defArreglo = palabra.Split('[',']');
                            if(ListaTokens[defArreglo[0]]>=10 && ListaTokens[defArreglo[0]] <= 13)
                            {
                                if (defArreglo[1] == "[" && defArreglo[3] == "]")
                                {
                                    if (int.TryParse(defArreglo[2],out _))
                                    {
                                        Console.WriteLine(ListaTokens["Array"] + "\t");
                                        pAnterior = ListaTokens["Array"];
                                    }
                                } 
                            }
                        }
                        Console.Write(ListaTokens[palabra]+"\t"); 
                        pAnterior = ListaTokens[palabra];
                    }
                    else
                    {
                        if (Variables.Contains(palabra)) //IF para revisar si es una variable ya definida
                        {
                            Console.Write(ListaTokens["VARIABLE"]+"\t");
                        }
                        else
                        {
                            if(pAnterior>=10 && pAnterior <= 13) //IF PARA REVISAR
                            {
                                Variables.Add(palabra);
                                Console.Write(ListaTokens["DEFINICION"] + "\t");
                            }
                            else
                            {
                                Console.Write($"Palabra no reconocida en linea {contLineas}" + "\t");
                            }
                        }

                    }
                }
                contLineas++;
                Console.WriteLine(" ");
            }
        }


        
    }
}
