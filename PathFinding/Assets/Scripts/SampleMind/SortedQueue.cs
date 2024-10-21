using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SortedQueue
{
    public Nodo first, last; //"Punteros" al primer y �ltimo nodo de la cola
    private float length = 0; //N�mero de elementos en la cola
    
    public Nodo Pop() //Elimina el primer elemento de la cola y devuelve su valor
    {
        Nodo aux = this.first;
        if(first.GetNext() != null) this.first = this.first.GetNext(); //Si no existe ning�n otro nodo en la cola no hay first
        else first = null;
        if(this.first != null) first.SetPrevious(null);
        length--;
        return aux;
    }

    public void Add(Nodo node) //A�ade nodos a la cola ordenados por su f*. Los primeros ser�n los que tengan un f* menor
    { 
        Nodo currentNode = this.first;
        bool added = false;
        for(int i = 0; i < length && !added; i++)
        {
            if(node.GetF() < currentNode.GetF()) //Si es menor la f* del nodo a a�adir que la del siguiente nodo se a�ade en esa posici�n
            {
                if (i == 0) first = node; //Si es el valor m�s peque�o hasta el momento se pone como first
                node.SetNext(currentNode);
                node.SetPrevious(currentNode.GetPrevious());
                if(currentNode.GetPrevious() != null) currentNode.GetPrevious().SetNext(node);
                currentNode.SetPrevious(node);
                added = true;
            }

            if (i == length - 1) last = currentNode; //Es importante tener actualizado el valor last

            currentNode = currentNode.GetNext();
        }
        if (!added) //Si no se ha a�adido, es decir, el valor es el m�s grande de todos o no hay ning�n elemento en la cola, se a�ade igualmente
        {
            if(length == 0) //Si no hay valores first y last ser�n el puntero a�adido
            {
                first = node;
                last = node;
            }
            else //Si el valor a a�adir es el m�s alto se convierte en el nuevo last
            {
                last.SetNext(node);
                node.SetPrevious(last);
                last = node;
            }
        }
        length++;
    }
}
