using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class TokenList
    {
        public List<Token> ListaTokens { get; set; }
        
        public TokenList()
        {
            ListaTokens = new List<Token>();
            ListaTokens.Add(new Token("START", 0, "PRESERVADA"));
            ListaTokens.Add(new Token("END", 1, "PRESERVADA"));
            ListaTokens.Add(new Token("READ", 2, "PRESERVADA"));
            ListaTokens.Add(new Token("PRINT", 3, "PRESERVADA"));
            ListaTokens.Add(new Token("PRINTNL", 4, "PRESERVADA"));
            ListaTokens.Add(new Token("IF", 5, "CONDICIONAL"));
            ListaTokens.Add(new Token("ELSE", 6, "SINO"));
            ListaTokens.Add(new Token("FOR", 7, "PRESERVADA"));
            ListaTokens.Add(new Token("WHILE",8, "PRESERVADA"));
            ListaTokens.Add(new Token("INT", 9, "INT"));
            ListaTokens.Add(new Token("DOUBLE", 10, "DOUBLE"));
            ListaTokens.Add(new Token("STRING", 11, "STRING"));
            ListaTokens.Add(new Token("ARRAY", 12, "PRESERVADA"));
            ListaTokens.Add(new Token("VARIABLE", 13, "PRESERVADA"));
            ListaTokens.Add(new Token("IDENT", 14, "PRESERVADA"));
            ListaTokens.Add(new Token("COMENTARIO", 15, "PRESERVADA"));
            ListaTokens.Add(new Token("CONSTANTE", 16, "PRESERVADA"));
            ListaTokens.Add(new Token("==", 17, "PRESERVADA"));
            ListaTokens.Add(new Token("!=", 18, "PRESERVADA"));
            ListaTokens.Add(new Token("<=", 19, "PRESERVADA"));
            ListaTokens.Add(new Token(">=", 20, "PRESERVADA"));
            ListaTokens.Add(new Token("<", 21, "PRESERVADA"));
            ListaTokens.Add(new Token(">", 22, "PRESERVADA"));
            ListaTokens.Add(new Token("AND", 23, "PRESERVADA"));
            ListaTokens.Add(new Token("OR", 24, "PRESERVADA"));
            ListaTokens.Add(new Token("NOT", 25, "PRESERVADA"));
            ListaTokens.Add(new Token("=", 26, "PRESERVADA"));
            ListaTokens.Add(new Token("+", 27, "PRESERVADA"));
            ListaTokens.Add(new Token("-", 28, "PRESERVADA"));
            ListaTokens.Add(new Token("*", 29, "PRESERVADA"));
            ListaTokens.Add(new Token("/", 30, "PRESERVADA"));
            ListaTokens.Add(new Token("(", 31, "PRESERVADA"));
            ListaTokens.Add(new Token(")", 32, "PRESERVADA"));
            ListaTokens.Add(new Token("{", 33, "PRESERVADA"));
            ListaTokens.Add(new Token("}", 34, "PRESERVADA"));
            ListaTokens.Add(new Token("\"", 35, "PRESERVADA"));
            ListaTokens.Add(new Token("\"", 36, "PRESERVADA"));
            ListaTokens.Add(new Token(";", 37, "PRESERVADA"));
            ListaTokens.Add(new Token(":", 38, "CONDICIONALFOR"));
            ListaTokens.Add(new Token(",", 39, "COMA"));
            ListaTokens.Add(new Token("[", 40, "PRESERVADA"));
            ListaTokens.Add(new Token("]", 41, "PRESERVADA"));
            ListaTokens.Add(new Token("CADENA", 42, "Una cadena string"));
            ListaTokens.Add(new Token("FDD", 43, "FINAL DE DOCUMENTO"));
            ListaTokens.Add(new Token("%", 44, "FINAL DE DOCUMENTO"));
            ListaTokens.Add(new Token("NONE", 45, "CARACTER/TOKEN NO RECONOCIDO"));

        }
        public int buscarLexema(string palabra) {
            int i = 0;
            for (i=0;i<ListaTokens.Count;i++) {
                if (palabra == ListaTokens[i].Lexema) {
                    return ListaTokens[i].Codigo;
                }
            }
            return -1;
        }
    }
}
