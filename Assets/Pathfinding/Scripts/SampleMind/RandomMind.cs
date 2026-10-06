using UnityEngine;

namespace Pathfinding
{
    public class RandomMind : AbstractPathMind 
    {
        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals = null)
        {
            Locomotion.MoveDirection chosenDir = (Locomotion.MoveDirection)Random.Range(0, 4);
            if (IsMovementAllowed(chosenDir, boardInfo, currentPos)) return chosenDir;
            return GetNextMove(boardInfo, currentPos);
        }

        private bool IsMovementAllowed(Locomotion.MoveDirection dir, BoardInfo boardInfo, CellInfo currentPos)
        {
            switch (dir)
            {
                case Locomotion.MoveDirection.Left:
                    if(currentPos.ColumnId <= 0) return false; //Out of bounds
                    return boardInfo.CellInfos[currentPos.ColumnId - 1, currentPos.RowId].Walkable;
                case Locomotion.MoveDirection.Right:
                    if (currentPos.ColumnId >= boardInfo.NumColumns - 1) return false; //Out of bounds
                    return boardInfo.CellInfos[currentPos.ColumnId + 1, currentPos.RowId].Walkable;
                case Locomotion.MoveDirection.Up:
                    if (currentPos.RowId >= boardInfo.NumRows - 1) return false; //Out of bounds
                    return boardInfo.CellInfos[currentPos.ColumnId, currentPos.RowId + 1].Walkable;
                case Locomotion.MoveDirection.Down:
                    if (currentPos.RowId <= 0) return false; //Out of bounds
                    return boardInfo.CellInfos[currentPos.ColumnId, currentPos.RowId - 1].Walkable;
                default:
                    return false;
            }
        }
    }
}