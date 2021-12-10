using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class SegmentoDeDatos
    {
        public List<ElementoSegmentoDeDatos> Elementos { get; set; }

        public SegmentoDeDatos()
        {
            Elementos = new List<ElementoSegmentoDeDatos>();
        }
        public int buscarVariable(string palabra)
        {
            int i = 0;
            for (i = 0; i < Elementos.Count; i++)
            {
                if (palabra == Elementos[i].VariableName)
                {
                    return i;
                }
            }
            return -1;
        }
        public void imprimir() {
            int i = 0;
            for (i = 0; i < Elementos.Count; i++)
            {
                Console.WriteLine(Elementos[i].VariableName + "-" + Elementos[i].VariableType + "-" + Elementos[i].Direccion + "-" + Elementos[i].NumElementos);
            }
        }
    }
}
