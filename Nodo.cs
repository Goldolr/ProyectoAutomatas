using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatasAFD
{
    class Nodo
    {
        String valor;
        int id;
        Nodo izq;
        Nodo der;
        Boolean estado;
        int posicion;
        String first;
        String last;

        public Nodo(String valor, int id)
        {
            this.valor = valor;
            this.id = id;
        }

        public String getValor()
        {
            return valor;
        }

        public void setValor(String valor)
        {
            this.valor = valor;
        }

        public Nodo getIzq()
        {
            return izq;
        }

        public void setIzq(Nodo izq)
        {
            this.izq = izq;
        }

        public Nodo getDer()
        {
            return der;
        }

        public void setDer(Nodo der)
        {
            this.der = der;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public Boolean getEstado()
        {
            return estado;
        }

        public void setEstado(Boolean estado)
        {
            this.estado = estado;
        }

        public int getPosicion()
        {
            return posicion;
        }

        public void setPosicion(int posicion)
        {
            this.posicion = posicion;
        }

        public String getFirst()
        {
            return first;
        }

        public void setFirst(String first)
        {
            this.first = first;
        }

        public String getLast()
        {
            return last;
        }

        public void setLast(String last)
        {
            this.last = last;
        }
    }
}
