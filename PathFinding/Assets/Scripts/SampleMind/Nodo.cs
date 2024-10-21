using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Nodo
{
    private Nodo parent; //Nodo padre 
    private float f = 0, g = 0, h = 0; //Valores de A*
    public int posX, posY, goalX, goalY; //Posiciones dentro de la matriz y posición de la meta 
    private Nodo next, previous; //Nodos siguiente y anterior. Utilizados cuando se añade el nodo a una lista

    public Nodo(int x, int y) //Constructor que inicializa la posición
    {
        this.posX = x;
        this.posY = y;
    }

    public Nodo GetNext() //Devuelve el siguiente nodo de la lista
    {
        return this.next;
    }

    public void SetNext(Nodo node) //Determina el siguiente nodo de la lista
    {
        this.next = node;
    }

    public Nodo GetPrevious() //Devuelve el anterior nodo de la lista
    {
        return this.previous;
    }

    public void SetPrevious(Nodo node) //Determina el anterior nodo de la lista
    {
        this.previous = node;
    }

    public void SetGoal(int x, int y) //Determina la posición del nodo meta
    {
        this.goalX = x;
        this.goalY = y;
    }

    public void SetParent(Nodo parent) //Añade un padre al nodo
    {
        this.parent = parent;
    }

    public Nodo GetParent() //Devuelve el padre del nodo
    {
        return this.parent;
    }

    public float GetG() //Devuelve g
    {
        return this.g;
    }

    public float GetF() //Devuelve f (útil para ordenar los nodos en la cola ordenada)
    {
        return this.f;
    }

    public void CalculateF() //Calcula f* a partir de la g del padre y la distancia mínima a la meta (sin tener en cuenta celdas no caminables)
    {
        g = parent.GetG() + 1;
        h = Mathf.Abs(goalX - this.posX) + Mathf.Abs(goalY - this.posY);
        f = g + h;
    }

    public bool isGoal() //Devuelve true si el nodo es el nodo meta y false si no lo es
    {
        if (posX == goalX && posY == goalY) return true;
        return false;
    }

}
