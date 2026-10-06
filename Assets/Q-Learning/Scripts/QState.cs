using NavigationDJIA.World;
using System;

public class QState 
{
    private bool[] walkables = new bool[4]; //Stores if neighbor cells are walkable
    private int[] opponentRelativePos = new int[2]; //Stores relative position of the opponent
    private int distance; //Distance between the agent and the opponent
    public QState(CellInfo agentPosition, CellInfo otherPosition, WorldInfo worldInfo)
    {
        //State regarding neighbors' walkability:
        for(int i = 0; i < 4; i++)
        {
            CellInfo position = QMind.Utils.MoveAgent(i, agentPosition, worldInfo);
            walkables[i] = position.Walkable;
        }
        //State regarding the distance with the opponent:
        int manhattan = Math.Abs(agentPosition.x - otherPosition.x) + Math.Abs(agentPosition.y - otherPosition.y);
        if (manhattan > 10) distance = 0;
        else distance = 1;
        //State regarding the direction of the opponent:
        if(otherPosition.y > agentPosition.y) opponentRelativePos[0] = 1;
        else if (otherPosition.y < agentPosition.y) opponentRelativePos[0] = -1;
        else if (otherPosition.y == agentPosition.y) opponentRelativePos[0] = 0;
        else if (otherPosition.x > agentPosition.x) opponentRelativePos[1] = 1;
        else if (otherPosition.x < agentPosition.x) opponentRelativePos[1] = -1;
        else if (otherPosition.x == agentPosition.x) opponentRelativePos[1] = 0;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 * distance.GetHashCode();
        foreach (bool walkable in walkables)
        {
            hash = hash * 23 + walkable.GetHashCode();
        }
        foreach (int position in opponentRelativePos)
        {
            hash = hash * 23 + position.GetHashCode();
        }
        return hash;
    }
}
