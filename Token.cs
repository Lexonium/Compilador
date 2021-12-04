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
        private int? _direccion;
        private string _descripcion;
        private int? _tamArreglo;
        private int? _codigo;
        private string? _valor;
        private int? _varType;

        public string Lexema
        {
            get { return _lexema; }
            set { _lexema = value; }
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
        public int? Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }
        public int? TamanoArreglo
        {
            get { return _tamArreglo; }
            set { _tamArreglo = value; }
        }
        public int? Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
        public void reiniciar()
        {
            _lexema = null; 
            _direccion = null;
            _tamArreglo = null;
            _codigo = null;
            _valor = null;
            _varType = null;
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
    }
}
