using UnityEngine;

namespace Pathfinding
{
    [DisallowMultipleComponent]
    public abstract class AbstractPathMind : MonoBehaviour
    {
        public CharacterBehaviour Character { get; set; }
        
        public abstract Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, 
                                                        CellInfo currentPos, CellInfo[] goals = null);
    }
}
