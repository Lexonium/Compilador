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
        public TokenList Lista = new TokenList();
        string linea, temp, acum, temp2, mensaje,acum2;
        static StreamReader archivo = new StreamReader(PathRepository.CrearPath(@"\CflatProgram.txt"));
        int contLineas = 1, contColumna = 1, pActual, pAnterior = -1, x = 0, contComillas = 0, tipoVar = 0, direccion = 0, dirtemp = 0,arrsize=0;
        List<string> Variables = new List<string>();
        string[] defArreglo = new string[4];
        public SegmentoDeDatos variables = new SegmentoDeDatos();
        static char caracter = (char)archivo.Read();
        public List<Token> listaATraducir = new List<Token>();
        public Token siguiente()
        {
            Token tokenTemp = new Token();
            while (caracter == ' ' || caracter=='\n' || caracter == '\t') {
                //While para saltar los espacios en blanco
                sigChar();
            }
            if (caracter == '$') {
                sigChar();
                while (caracter != '$') {
                    sigChar();
                }
                sigChar();
                while (caracter == ' ' || caracter == '\n' || caracter == '\t')
                {
                    //While para saltar los espacios en blanco
                    sigChar();
                }
            }
            tokenTemp.Linea = contLineas;
            tokenTemp.Columna = contColumna;
            //ASIGNACION DE LINEA Y COLUMNA DEL TOKEN QUE SE VA IDENTIFICAR
            if (char.IsLetter(caracter))
            {   //LO MANDA A UN METODO QUE REVISA QUE TIPO DE PALABRA ES (RESERVADA/VARIABLE/IDENTIFIER)
                esPalabra(tokenTemp);
            }
            else if (char.IsNumber(caracter))
            {   //LO MANDA PARA REVISAR QUE TIPO DE CONSTANTE ES INT o DOUBLE
                tokenTemp.Codigo = 16;
                pAnterior = 16;
                esNumero(tokenTemp);
            }
            else switch (caracter) {
                    case '=':
                        sigChar();
                        if (caracter == '=')
                        { //es un condicional igual
                            tokenTemp.Lexema = "==";
                            tokenTemp.Codigo = 17;
                            pAnterior = 17;
                        }
                        else {
                            //es un igual de asignacion
                            tokenTemp.Lexema = "=";
                            tokenTemp.Codigo = 26;
                            pAnterior = 26;
                        }
                        break;
                    case '<':
                        sigChar();
                        if (caracter == '=')
                        {
                            //es un menorigual que
                            tokenTemp.Lexema = "<=";
                            tokenTemp.Codigo = 19;
                            pAnterior = 19;
                        }
                        else {
                            //es un menor que
                            tokenTemp.Lexema = "<";
                            tokenTemp.Codigo = 21;
                            pAnterior = 21;
                        }
                        break;
                    case '>':
                        sigChar();
                        if (caracter == '=')
                        {
                            //es un mayor igual que
                            tokenTemp.Lexema = ">=";
                            tokenTemp.Codigo = 20;
                            pAnterior = 20;

                        }
                        else {
                            //es un mayor que
                            tokenTemp.Lexema = ">";
                            tokenTemp.Codigo = 22;
                            pAnterior = 22;
                        }
                        break;
                    case '!':
                        sigChar();
                        if (caracter == '=')
                        {
                            //es un diferente que
                            tokenTemp.Lexema = "!=";
                            tokenTemp.Codigo = 18;
                            pAnterior = 18;
                        }
                        else { //que hace una exclamacion solito en el codigo?
                            errorLexico("No hay un comando con este simbolo [!]",tokenTemp);
                        }
                        break;
                    case '"':
                        sigChar();
                        while (caracter!= '"') {
                            //WHILE PARA AGARRAR TODA LA CADENA
                            if (caracter == '\n')
                            { //REVISA QUE NO SE HAGA UN SALTO DE LINEA DENTRO DE LA CADENA
                                errorLexico("Una cadena no puede tomar mas de una linea", tokenTemp);
                                acum = "";
                                break;
                            }
                            else
                            {   //VA ACUMULANDO LA CADENA
                                acum += caracter;
                            }
                            sigChar();
                        }
                        if (caracter =='"') {
                            //CREA EL TOKEN DE LA CADENA
                            acum += caracter;
                            tokenTemp.Codigo = 42;
                            pAnterior = 42;
                            tokenTemp.Lexema = "CADENA";
                            tokenTemp.Valor = acum;
                            tokenTemp.Tipo = "3";
                            acum = "";
                        }
                        sigChar();
                        break;
                    case '+': //TOKEN DEL OPERADOR MAS
                        tokenTemp.Lexema = "+";
                        tokenTemp.Codigo = 27;
                        pAnterior = 27;
                        sigChar();
                        break;
                    case '-': //TOKEN DEL OPERADOR MENOS
                        tokenTemp.Lexema = "-";
                        tokenTemp.Codigo = 28;
                        pAnterior = 28;
                        sigChar();
                        break;
                    case '*': //TOKEN DEL OPERADOR POR
                        tokenTemp.Lexema = "*";
                        tokenTemp.Codigo = 29;
                        pAnterior = 29;
                        sigChar();
                        break;
                    case '/': //TOKEN DEL OPERADOR ENTRE
                        tokenTemp.Lexema = "/";
                        tokenTemp.Codigo = 30;
                        pAnterior = 30;
                        sigChar();
                        break;
                    case '(': //TOKEN DE ABRIR PARENTESIS
                        tokenTemp.Lexema = "(";
                        tokenTemp.Codigo = 31;
                        pAnterior = 31;
                        sigChar();
                        break;
                    case ')': //TOKEN DE CERRAR PARENTESIS
                        tokenTemp.Lexema = ")";
                        tokenTemp.Codigo = 32;
                        pAnterior = 32;
                        sigChar();
                        break;
                    case '{': // TOKEN DE ABRIR CORCHETES
                        tokenTemp.Lexema = "{";
                        tokenTemp.Codigo = 33;
                        pAnterior = 33;
                        sigChar();
                        break;
                    case '}': //TOKEN DE CERRRAR CORCHETES
                        tokenTemp.Lexema = "}";
                        tokenTemp.Codigo = 34;
                        pAnterior = 34;
                        sigChar();
                        break;
                    case ';': //TOKEN PUNTOCOMA
                        tokenTemp.Lexema = ";";
                        tokenTemp.Codigo = 37;
                        pAnterior = 37;
                        sigChar();
                        break;
                    case ':': //TOKEN 2 PUNTOS (PARA IF)
                        tokenTemp.Lexema = ":";
                        tokenTemp.Codigo = 38;
                        pAnterior = 38;
                        sigChar();
                        break;
                    case ',': //TOKEN COMA
                        tokenTemp.Lexema = ",";
                        tokenTemp.Codigo = 39;
                        //pAnterior = 39;
                        sigChar();
                        break;
                    case '%':
                        tokenTemp.Lexema = "%";
                        tokenTemp.Codigo = 44;
                        pAnterior = 44;
                        sigChar();
                        break;
                    case '\u0080':
                        tokenTemp.Lexema = "FDD";
                        tokenTemp.Codigo = 43;
                        sigChar();
                        break;
                    default:
                        tokenTemp.Lexema = "NONE";
                        tokenTemp.Codigo = 45;
                        sigChar();
                        break;
                }
            listaATraducir.Add(tokenTemp);
            return tokenTemp;
        }
        public void sigChar()
        {
            caracter = (char)archivo.Read();
            contColumna++;
            if (caracter == '\n')
            {
                contLineas++;
                contColumna = 0;
                //sigChar();
            }
            else if (caracter == '\uffff')
            {
                caracter = '\u0080';
            }
            else if (caracter == '\r') {
                sigChar();
            }
        }
        public void reiniciarLector()
        {
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            archivo.BaseStream.Position = 0;
            caracter = (char)archivo.Read();
            sigChar();
        }

        public void esPalabra (Token tokenTemp){
            while (char.IsLetter(caracter) || char.IsNumber(caracter)) {
                acum += caracter;
                sigChar();
                //VA IR ACUMULANDO LAS LETRAS O NUMEROS, YA QUE VARIABLES PUEDEN INCORPORAR NUMEROS
            }
            tokenTemp.Lexema = acum;
            if (Lista.buscarLexema(acum) != -1) //REVISA SI EL ACUMULADO ES UNA PALABRA YA EN LOS TOKENS
            {
                x = Lista.buscarLexema(acum);
                tokenTemp.Codigo = Lista.ListaTokens[x].Codigo;
                pAnterior = x;
            }
            else {
                //ver si es un IDENT o VARIABLE
                if (variables.buscarVariable(acum) != -1)
                {  //YA SE HABIA DECLARADO ANTERIORMENTE ESTA VARIABLE
                    x = variables.buscarVariable(acum);
                    tokenTemp.Codigo = 13;
                    pAnterior = 13;
                    tokenTemp.TamanoArreglo = variables.Elementos[x].NumElementos;
                    tokenTemp.Tipo = variables.Elementos[x].VariableType.ToString();
                    //sigChar();
                    acum = "";
                    if (caracter == '[') { //SI LA VARIABLE ES UN ARREGLO
                        sigChar();
                        if (char.IsNumber(caracter))
                        {
                            while (char.IsNumber(caracter))
                            {
                                acum += caracter;
                                sigChar();
                            }
                            if (caracter == ']')
                            {
                                tokenTemp.Indice = int.Parse(acum);
                                sigChar();
                            }
                            else
                            {
                                if (caracter == '.') { 
                                errorLexico("Double dentro de indice", tokenTemp);
                                }
                                errorLexico("No se cerro corchete", tokenTemp);
                            }
                        }
                        else if (char.IsLetter(caracter)) {
                            while (char.IsLetter(caracter) || char.IsNumber(caracter))
                            { //VA IR LEYENDO LA VARIABLE QUE SE PUSO DENTRO DE LOS CORCHETES
                                acum += caracter;
                                sigChar();
                            }
                            if (caracter == ']')
                            {
                                sigChar();
                            }
                            else {
                                errorLexico("No se cerro corchete", tokenTemp);
                            }
                            if (variables.buscarVariable(acum) != -1)
                            {   //BUSCA QUE YA HAYA SIDO DECLARADA
                                x = variables.buscarVariable(acum);
                                if(variables.Elementos[x].VariableType == 1) {
                                    tokenTemp.Indice = variables.Elementos[x].Direccion;
                                    tokenTemp.IndexVar = true;
                                }
                                else{
                                    errorLexico("La variable no es entera", tokenTemp);
                                }
                                
                            }
                            else
                            {
                                errorLexico("No se encuentra declarada esta variable",tokenTemp);
                            }

                        }
                    }
                }
                else {
                    // NO se ha declarado la variable anteriormente
                    if (caracter == '[')
                    {
                        //Es arreglo la variable que se declaro
                        sigChar();
                        if (!char.IsNumber(caracter))
                        {
                            errorLexico("Solo se puede definir arreglos con numeros enteros", tokenTemp);
                        }
                        else {
                            while (char.IsNumber(caracter))
                            {
                                acum2 += caracter;
                                sigChar();
                            }
                            if (caracter == '.')
                            {
                                errorLexico("Solo se puede definir arreglos con numeros enteros", tokenTemp);
                            }
                            if (caracter == ']')
                            {
                                arrsize = int.Parse(acum2);
                                tipoVar = 10;
                                switch (pAnterior)
                                {
                                    case 9:
                                        tipoVar++;
                                        dirtemp = 4 * arrsize;
                                        break;
                                    case 10:
                                        tipoVar += 2;
                                        dirtemp = 8 * arrsize;
                                        break;
                                    case 11:
                                        tipoVar += 3;
                                        dirtemp = 2 * arrsize;
                                        break;
                                    default:
                                        break;
                                }
                                variables.Elementos.Add(new ElementoSegmentoDeDatos(acum, tipoVar, direccion, arrsize));
                                direccion += dirtemp;
                                tokenTemp.Codigo = 14;
                                //pAnterior = 14;
                                tokenTemp.TamanoArreglo = arrsize;
                                tokenTemp.Tipo = tipoVar.ToString();
                                sigChar();
                            }
                            else {
                                errorLexico("No se cerro corchete", tokenTemp);
                            }

                        }

                    }
                    else {
                        //No es arreglo la variable
                        switch (pAnterior)
                        {
                            case 9:
                                tipoVar = 1;
                                dirtemp += 4;
                                break;
                            case 10:
                                tipoVar = 2;
                                dirtemp += 8;
                                break;
                            case 11:
                                tipoVar = 3;
                                dirtemp += 2;
                                break;
                            default:
                                errorLexico($"No se reconoce esta palabra,{acum} si es variable falto poner el tipo antes",tokenTemp);
                                break;
                        }
                        variables.Elementos.Add(new ElementoSegmentoDeDatos(acum,tipoVar,direccion,0));
                        direccion += dirtemp;
                        tokenTemp.Codigo = 14;
                        //pAnterior = 14;
                        tokenTemp.TamanoArreglo = 0;
                        tokenTemp.Tipo = tipoVar.ToString();
                    }

                }
            }
            acum = "";
            acum2 = "";
        }

        public void esNumero(Token tokentemp) {
            while (char.IsNumber(caracter)) {
                //LEO CHARS HASTA QUE ME TOPE A ALGO QUE NO ES NUMERO
                acum += caracter;
                sigChar();
            }
            acum2 += acum; //GUARDO EL VALOR QUE LLEVO
            acum = "";
            if (caracter == '.')
            { //VEO SI LO QUE PONE ES UN DOUBLE
                sigChar();
                if (char.IsNumber(caracter))
                {   //EN EFECTO ES UN DOUBLE
                    acum2 += '.';
                    while (char.IsNumber(caracter))
                    {
                        //LEO TODOS LOS DECIMALES QUE PUEDO
                        acum += caracter;
                        sigChar();
                    }
                    acum2 += acum;
                    tokentemp.Lexema = "Constante Double";
                    tokentemp.Tipo = "2";
                    tokentemp.Valor = acum2;

                }
            }
            else {
                tokentemp.Lexema = "Constante Entero";
                tokentemp.Tipo = "1";
                tokentemp.Valor = acum2;
            }
            acum = "";
            acum2 = "";
            
        }
        public void errorLexico(string msg, Token tokenTemp) {
            Console.WriteLine("En linea " + tokenTemp.Linea + " Columna " + tokenTemp.Columna + ": " + msg);
        }



    }
}

