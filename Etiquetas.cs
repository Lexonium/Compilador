using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class Etiquetas
    {
        private string _nombreEtiqueta;
        private int _direccionEtiqueta;

        public string NombreEtiqueta
        {
            get { return _nombreEtiqueta; }
            set { _nombreEtiqueta = value; }
        }
        public int DireccionEtiqueta
        {
            get { return _direccionEtiqueta; }
            set { _direccionEtiqueta = value; }
        }
        public Etiquetas()
        {

        }
        public Etiquetas(string NombreEtiqueta)
        {
            this._nombreEtiqueta = NombreEtiqueta;
        }
        public Etiquetas(string NombreEtiqueta, int Direccion)
        {
            this._nombreEtiqueta = NombreEtiqueta;
            this._direccionEtiqueta = Direccion;
        }
    }
}
