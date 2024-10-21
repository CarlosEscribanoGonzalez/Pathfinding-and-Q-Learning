using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SortedQueue
{
    public Nodo first, last; //"Punteros" al primer y último nodo de la cola
    private float length = 0; //Número de elementos en la cola
    
    public Nodo Pop() //Elimina el primer elemento de la cola y devuelve su valor
    {
        Nodo aux = this.first;
        if(first.GetNext() != null) this.first = this.first.GetNext(); //Si no existe ningún otro nodo en la cola no hay first
        else first = null;
        if(this.first != null) first.SetPrevious(null);
        length--;
        return aux;
    }

    public void Add(Nodo node) //Añade nodos a la cola ordenados por su f*. Los primeros serán los que tengan un f* menor
    { 
        Nodo currentNode = this.first;
        bool added = false;
        for(int i = 0; i < length && !added; i++)
        {
            if(node.GetF() < currentNode.GetF()) //Si es menor la f* del nodo a añadir que la del siguiente nodo se añade en esa posición
            {
                if (i == 0) first = node; //Si es el valor más pequeño hasta el momento se pone como first
                node.SetNext(currentNode);
                node.SetPrevious(currentNode.GetPrevious());
                if(currentNode.GetPrevious() != null) currentNode.GetPrevious().SetNext(node);
                currentNode.SetPrevious(node);
                added = true;
            }

            if (i == length - 1) last = currentNode; //Es importante tener actualizado el valor last

            currentNode = currentNode.GetNext();
        }
        if (!added) //Si no se ha añadido, es decir, el valor es el más grande de todos o no hay ningún elemento en la cola, se añade igualmente
        {
            if(length == 0) //Si no hay valores first y last serán el puntero añadido
            {
                first = node;
                last = node;
            }
            else //Si el valor a añadir es el más alto se convierte en el nuevo last
            {
                last.SetNext(node);
                node.SetPrevious(last);
                last = node;
            }
        }
        length++;
    }
}
