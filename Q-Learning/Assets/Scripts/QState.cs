using Components;
using NavigationDJIA.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QState 
{
    public bool[] walkables = new bool[4]; // Array que indica para cada una de las celdas contiguas al agente son walkables
    public int distance; // Distancia entre el jugador y el agente
    private int[] otherRelativePosition = new int[2]; //Array que indica en qué direcciones se encuentra el jugador (arriba a la izquierda, abajo...)
    public QState(CellInfo agentPosition, CellInfo otherPosition, WorldInfo worldInfo)
    {
        //Estado respecto a las celdas vecinas caminables:
        for(int i = 0; i < 4; i++)
        {
            CellInfo position = QMind.Utils.MoveAgent(i, agentPosition, worldInfo);
            walkables[i] = position.Walkable;
        }
        //Estado respecto a la distancia con el jugador:
        int manhattan = Math.Abs(agentPosition.x - otherPosition.x) + Math.Abs(agentPosition.y - otherPosition.y);
        if (manhattan > 4) distance = 0; //Distancia media-alta
        else if (manhattan > 1) distance = 1; //Cerca (a dos casillas)
        else if (manhattan > 0) distance = 3; //Muy cerca (a una casilla)
        else distance = 4; //Pillado
        //Estado respecto a la dirección en la que se encuentra el jugador:
        if (otherPosition.y > agentPosition.y) otherRelativePosition[0] = 1;
        else if (otherPosition.y < agentPosition.y) otherRelativePosition[0] = -1;
        else if (otherPosition.y == agentPosition.y) otherRelativePosition[0] = 0;
        if (otherPosition.x > agentPosition.x) otherRelativePosition[1] = 1;
        else if (otherPosition.x < agentPosition.x) otherRelativePosition[1] = -1;
        else if (otherPosition.x == agentPosition.x) otherRelativePosition[1] = 0;
    }

    public QState(bool[] walkables, int distance, int[] otherRelativePosition)
    {
        this.walkables = walkables;
        this.distance = distance;
        this.otherRelativePosition = otherRelativePosition;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 * distance.GetHashCode();
        foreach (bool walkable in walkables)
        {
            hash = hash * 23 + walkable.GetHashCode();
        }
        foreach (int position in otherRelativePosition)
        {
            hash = hash * 23 + position.GetHashCode();
        }
        return hash;
    }
}
