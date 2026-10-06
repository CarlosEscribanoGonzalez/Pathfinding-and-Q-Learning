using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class HorizonMind : AbstractPathMind
    {
        private Node[,] nodeMatrix;
        private SortedQueue queue = new();
        private readonly LinkedList<Node> path = new();
        private int depth;

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            queue = new();
            path.Clear();
            depth = 3;
            EnemyBehaviour closestEnemy = FindClosestEnemy(currentPos);
            boardInfo.Exit.Walkable = closestEnemy == null;
            if (closestEnemy != null) goals[0] = closestEnemy.CurrentPosition();
            SearchPath(boardInfo, currentPos, goals);

            path.RemoveFirst();
            Node nextCell = path.First.Value;
            path.RemoveFirst();

            if (nextCell.posX < currentPos.ColumnId) return Locomotion.MoveDirection.Left;
            else if (nextCell.posX > currentPos.ColumnId) return Locomotion.MoveDirection.Right;
            else if (nextCell.posY > currentPos.RowId) return Locomotion.MoveDirection.Up;
            return Locomotion.MoveDirection.Down;
        }

        private void SearchPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            nodeMatrix = new Node[boardInfo.NumColumns, boardInfo.NumRows];
            CreateNodeMatrix(boardInfo);
            AssignGoal(boardInfo, goals);
            Node node = nodeMatrix[currentPos.ColumnId, currentPos.RowId];
            node.SetParent(node); //So it isn't added to the sorted queue later
            queue.Add(node);
            SearchGoal(boardInfo);
        }

        private void CreateNodeMatrix(BoardInfo boardInfo)
        {
            int numColumns = boardInfo.NumColumns;
            int numRows = boardInfo.NumRows;
            for (int i = 0; i < numColumns; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    nodeMatrix[i, j] = new Node(i, j);
                }
            }
        }

        private void AssignGoal(BoardInfo boardInfo, CellInfo[] goals)
        {
            for (int i = 0; i < boardInfo.NumColumns; i++)
            {
                for (int j = 0; j < boardInfo.NumRows; j++)
                {
                    nodeMatrix[i, j].SetParent(null);
                    nodeMatrix[i, j].SetGoal(goals[0].ColumnId, goals[0].RowId);
                }
            }
        }

        private void SearchGoal(BoardInfo boardInfo)
        {
            Node currentNode = queue.Pop();
            if (currentNode.IsGoal() || depth == 0)
            {
                path.AddFirst(currentNode);
                AddParentsToPath(currentNode);
            }
            else
            {
                CellInfo currentCell = boardInfo.CellInfos[currentNode.posX, currentNode.posY];
                CellInfo[] neighbors = currentCell.WalkableNeighbours(boardInfo);
                for (int i = 0; i < neighbors.Length; i++)
                {
                    if (neighbors[i] != null) //Neighbours.Length is always 4, but not all nodes have 4 neighbors
                    {
                        Node neighborNode = nodeMatrix[neighbors[i].ColumnId, neighbors[i].RowId];
                        if (neighborNode.GetParent() == null) //If it doesn't have a parent it means it hasn't been visited yet
                        {
                            neighborNode.SetParent(currentNode);
                            neighborNode.CalculateF();
                            queue.Add(neighborNode);
                        }
                    }
                }
                depth--;
                SearchGoal(boardInfo);
            }
        }

        private void AddParentsToPath(Node node)
        {
            if (!node.IsFirstNode())
            {
                path.AddFirst(node.GetParent());
                AddParentsToPath(node.GetParent());
            }
        }

        private EnemyBehaviour FindClosestEnemy(CellInfo currentPos)
        {
            EnemyBehaviour[] enemies = FindObjectsByType<EnemyBehaviour>(FindObjectsSortMode.None);
            if (enemies.Length == 0) return null;
            int closestManhattan = 10000;
            int closestEnemyIdx = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                int offset = Mathf.Abs(currentPos.ColumnId - enemies[i].CurrentPosition().ColumnId);
                offset += Mathf.Abs(currentPos.RowId - enemies[i].CurrentPosition().RowId);
                if (offset < closestManhattan)
                {
                    closestManhattan = offset;
                    closestEnemyIdx = i;
                }
            }
            return enemies[closestEnemyIdx];
        }
    }
}