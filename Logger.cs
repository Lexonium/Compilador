using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    class Logger
    {
        string magicnumber = "ICCTSN";
        public List<byte> codigoCompleto = new List<byte>();
        public List<byte> tsnvEnBytes = new List<byte>();
        public void PrintMagicNumber()
        {
            byte[] letrasmagic = new byte[6];
            letrasmagic = Encoding.ASCII.GetBytes(magicnumber);
            int i = 0;
            for (i = 0; i < letrasmagic.Length; i++)
            {
                codigoCompleto.Add(letrasmagic[i]);
            }
        }
        public void cambiarA2bytes(int numero)//, ref int contadorTSN) {
        {
            int i;
            string losbytes;
            string[] twobytes = new string[2];
            byte[] dosbytes = new byte[2];
            dosbytes = BitConverter.GetBytes(numero);
            for (i = 0; i < 2; i++)
            {
                codigoCompleto.Add(dosbytes[i]);
                //twobytes[i] = Convert.ToString(dosbytes[i]);
            }
            //losbytes = string.Join("-", twobytes);
            //Console.Write(losbytes);
            //contadorTSN += 2;
        }
        public void PrintSegmentoDeCodigo(List<ElementoSegmentoDeCodigo> listaSegmentos)
        {
            foreach (var x in listaSegmentos)
            {
                Console.Write("Nombre: " + x.CommandName + ", Size: " + x.PesoComando + ", Codigo: " + x.NumeroDeCodigo + ", DireccionVariable: " + x.DireccionVariable + ", Valor Constante: " + x.ValorConstante);
            }
        }
        public void PrintTSNV(List<ElementoSegmentoDeDatos> listaSegmentos)
        {
            int i = 0, temp = 0;
            string[] thirtybytes = new string[30], twobytes = new string[2];
            byte[] unbyte = new byte[1];
            byte[] treintabytes = new byte[30];
            byte[] dosbytes = new byte[2];
            //Console.Write(listaSegmentos.Count);
            unbyte = BitConverter.GetBytes(listaSegmentos.Count);
            tsnvEnBytes.Add(unbyte[0]);
            foreach (var x in listaSegmentos)
            {
                i = 0;

                treintabytes = Encoding.ASCII.GetBytes(x.VariableName);
                for (i = 0; i < treintabytes.Length; i++)
                {
                    tsnvEnBytes.Add(treintabytes[i]);
                }
                if (treintabytes.Length < 30)
                {
                    i = x.VariableName.Length;
                    while (i < 30)
                    {
                        tsnvEnBytes.Add(BitConverter.GetBytes(temp)[0]);
                        i++;
                    }
                }
                dosbytes = BitConverter.GetBytes(x.Direccion);
                for (i = 0; i < 2; i++)
                {
                    tsnvEnBytes.Add(dosbytes[i]);
                }
                unbyte = BitConverter.GetBytes(x.VariableType);
                tsnvEnBytes.Add(unbyte[0]);
                dosbytes = BitConverter.GetBytes(Convert.ToInt32(x.NumElementos));
                for (i = 0; i < 2; i++)
                {
                    tsnvEnBytes.Add(dosbytes[i]);
                }
                dosbytes = BitConverter.GetBytes(Convert.ToInt32(x.VectorString));
                for (i = 0; i < 2; i++)
                {
                    tsnvEnBytes.Add(dosbytes[i]);
                } 
            }
        }
        public void PrintCodigodeTSN(List<ElementoSegmentoDeCodigo> listaSegmentos)
        {
            int i = 0, xint;
            double xdoub;
            string losbytes;
            byte[] byteinstruccion = new byte[1];
            foreach (var x in listaSegmentos)
            {
                byteinstruccion = BitConverter.GetBytes(x.NumeroDeCodigo);
                codigoCompleto.Add(byteinstruccion[0]);
                
                if (x.NumeroDeCodigo != 27 && x.NumeroDeCodigo != 41)
                {
                    x.PesoComando--;
                    switch (x.PesoComando)
                    {
                        case 0:
                            break;
                        case 2:
                            string[] twobytes = new string[2];
                            byte[] dosbytes = new byte[2];
                            xint = Convert.ToInt32(x.DireccionVariable);
                            dosbytes = BitConverter.GetBytes(xint);
                            for (i = 0; i < 2; i++)
                            {
                                codigoCompleto.Add(dosbytes[i]);
                                
                            }
                            
                            break;
                        case 4:
                            string[] fourbytes = new string[4];
                            byte[] cuatrobytes = new byte[4];
                            xint = Convert.ToInt32(x.ValorConstante);
                            cuatrobytes = BitConverter.GetBytes(xint);
                            for (i = 0; i < cuatrobytes.Length; i++)
                            {
                                codigoCompleto.Add(cuatrobytes[i]);
                            }
                            break;
                        case 8:
                            string[] eightbytes = new string[8];
                            byte[] ochobytes = new byte[8];
                            
                            xdoub = Convert.ToDouble(x.ValorConstante);
                            ochobytes = BitConverter.GetBytes(xdoub);
                            for (i = 0; i < 8; i++)
                            {
                                codigoCompleto.Add(ochobytes[i]);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    byteinstruccion = BitConverter.GetBytes(x.ValorConstante.Trim().Count());
                    codigoCompleto.Add(byteinstruccion[0]);
                    byte[] mensaje = Encoding.ASCII.GetBytes(x.ValorConstante.Trim());
                    for (i = 0; i < mensaje.Length; i++)
                    {
                        codigoCompleto.Add(mensaje[i]);
                    }
                }
            }
        }

    }

}
