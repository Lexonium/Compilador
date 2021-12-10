using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class Token
    {
        private string _lexema;
        private string _tipo;
        private int _indice;
        private string _descripcion;
        private int? _tamArreglo;
        private int _codigo;
        private string? _valor;
        private int _linea;
        private int _columna;
        private bool indexVar = false;

        public string Lexema
        {
            get { return _lexema; }
            set { _lexema = value; }
        }
        public string? Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }
        public bool IndexVar
        {
            get { return indexVar; }
            set { indexVar = value; }
        }
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
        public int Indice 
        {
            get { return _indice; }
            set { _indice = value; }
        }
        public int? TamanoArreglo
        {
            get { return _tamArreglo; }
            set { _tamArreglo = value; }
        }
        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
        public int Linea
        {
            get { return _linea; }
            set { _linea = value; }
        }
        public int Columna
        {
            get { return _columna; }
            set { _columna = value; }
        }
        public void imprimir()
        {
            Console.WriteLine(Lexema+" "+Codigo+" "+Tipo);
        }
        public Token(string Lexema, int Codigo,string Tipo) {
            this.Lexema = Lexema;
            this.Codigo = Codigo;
            this.Tipo = Tipo;
        }
        public Token()
        {

        }
        public Token(string Lexema, int Codigo, string Tipo, int TamanoArreglo) {
            this.Lexema = Lexema;
            this.Codigo = Codigo;
            this.Tipo = Tipo;
            this.TamanoArreglo = TamanoArreglo;
        }
    }
}
