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
            ListaTokens.Add(new Token("START", 1, "PRESERVADA"));
            ListaTokens.Add(new Token("END", 2, "PRESERVADA"));
            ListaTokens.Add(new Token("READ", 3, "PRESERVADA"));
            ListaTokens.Add(new Token("PRINT", 4, "PRESERVADA"));
            ListaTokens.Add(new Token("PRINTNL", 5, "PRESERVADA"));
            ListaTokens.Add(new Token("IF", 6, "CONDICIONAL"));
            ListaTokens.Add(new Token("ELSE", 7, "SINO"));
            ListaTokens.Add(new Token("FOR", 8, "PRESERVADA"));
            ListaTokens.Add(new Token("WHILE", 9, "PRESERVADA"));
            ListaTokens.Add(new Token("INT", 10, "PRESERVADA"));
            ListaTokens.Add(new Token("DOUBLE", 11, "PRESERVADA"));
            ListaTokens.Add(new Token("STRING", 12, "PRESERVADA"));
            ListaTokens.Add(new Token("ARRAY", 13, "PRESERVADA"));
            ListaTokens.Add(new Token("VARIABLE", 14, "PRESERVADA"));
            ListaTokens.Add(new Token("DEFINICION", 15, "PRESERVADA"));
            ListaTokens.Add(new Token("COMENTARIO", 16, "PRESERVADA"));
            ListaTokens.Add(new Token("CONSTANTE", 17, "PRESERVADA"));
            ListaTokens.Add(new Token("==", 18, "PRESERVADA"));
            ListaTokens.Add(new Token("!=", 19, "PRESERVADA"));
            ListaTokens.Add(new Token("<=", 20, "PRESERVADA"));
            ListaTokens.Add(new Token(">=", 21, "PRESERVADA"));
            ListaTokens.Add(new Token("<", 22, "PRESERVADA"));
            ListaTokens.Add(new Token(">", 23, "PRESERVADA"));
            ListaTokens.Add(new Token("AND", 24, "PRESERVADA"));
            ListaTokens.Add(new Token("OR", 25, "PRESERVADA"));
            ListaTokens.Add(new Token("NOT", 26, "PRESERVADA"));
            ListaTokens.Add(new Token("=", 27, "PRESERVADA"));
            ListaTokens.Add(new Token("+", 28, "PRESERVADA"));
            ListaTokens.Add(new Token("-", 29, "PRESERVADA"));
            ListaTokens.Add(new Token("*", 30, "PRESERVADA"));
            ListaTokens.Add(new Token("/", 31, "PRESERVADA"));
            ListaTokens.Add(new Token("(", 32, "PRESERVADA"));
            ListaTokens.Add(new Token(")", 33, "PRESERVADA"));
            ListaTokens.Add(new Token("{", 34, "PRESERVADA"));
            ListaTokens.Add(new Token("}", 35, "PRESERVADA"));
            ListaTokens.Add(new Token("\"", 36, "PRESERVADA"));
            ListaTokens.Add(new Token("\"", 37, "PRESERVADA"));
            ListaTokens.Add(new Token(";", 38, "PRESERVADA"));

        }
    }
}
