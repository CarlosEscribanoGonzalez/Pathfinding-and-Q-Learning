using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Randommind
{
    public class RandomMind : AbstractPathMind
    {
        public override void Repath()
        {

        }

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        { 
            int random = Random.Range(0,4);
            if (random == 0) return Locomotion.MoveDirection.Up;
            if (random == 1) return Locomotion.MoveDirection.Left;
            if (random == 2) return Locomotion.MoveDirection.Down;
            return Locomotion.MoveDirection.Right;
        }
    }
}