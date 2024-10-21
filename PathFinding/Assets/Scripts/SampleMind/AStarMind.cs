using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AStarMind
{
    public class AStarMind : AbstractPathMind {

        private Nodo[,] nodeMatrix; //Matriz de nodos correspondientes a la matriz de celdas
        private SortedQueue queue = new SortedQueue(); //Cola ordenada en función de f*
        private LinkedList<Nodo> path = new LinkedList<Nodo>(); //Array de los nodos correspondientes al camino encontrado
        private bool pathFound = false; //Indica si se ha encontrado el camino para buscarlo 

        public override void Repath()
        {
            
        }

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            if (!pathFound) //Si no hay camino encontrado se debe encontrar. Para ello llamamos a SearchPath()
            {
                SearchPath(boardInfo, currentPos, goals);
            }

            if(path.Count == 0 || currentPos.Walkable == false) //Si la celda en la que empieza el jugador no es caminable
                                                                //o no existe un camino que lleve hasta la meta se detiene la ejecución
            {
                Debug.Log("No se ha encontrado ningún camino hacia la meta. Posible semilla incorrecta.");
                UnityEditor.EditorApplication.isPlaying = false;
            }
            
            Nodo nextCell = path.First.Value;
            path.RemoveFirst(); //Similar al método Pop() de la cola. Se coge el siguiente nodo del camino a seguir y se elimina.

            if (nextCell.posX < currentPos.ColumnId) return Locomotion.MoveDirection.Left;
            else if (nextCell.posX > currentPos.ColumnId) return Locomotion.MoveDirection.Right;
            else if (nextCell.posY > currentPos.RowId) return Locomotion.MoveDirection.Up;
            return Locomotion.MoveDirection.Down;
        }

        private void SearchPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals) //Función encargada de comenzar a buscar el camino
        {
            nodeMatrix = new Nodo[boardInfo.NumColumns,boardInfo.NumRows]; //Se inicializa nodeMatrix
            CreateNodeMatrix(boardInfo); //Se crea la matriz de nodos correspondiente a la matriz de celdas de la simulación
            AssignGoals(boardInfo, goals); //A todos los nodos de nodeMatrix se les asigna la posición de la meta
            Nodo node = nodeMatrix[currentPos.ColumnId, currentPos.RowId]; //Nodo actual del jugador
            node.SetParent(node); //Ponemos que es su propio padre para que no se añada a la sorted queue de nuevo más adelante
            queue.Add(node); //Hay que añadir la primera celda del jugador a la cola ordenada
            SearchGoal(boardInfo); //Comienza la búsqueda del camino
        }

        private void CreateNodeMatrix(BoardInfo boardInfo) //Función encargada de crear la matriz de nodos correspondiente a la matriz de celdas
        {
            int numColumns = boardInfo.NumColumns;
            int numRows = boardInfo.NumRows;
            for(int i = 0; i < numColumns; i++)
            {
                for(int j = 0; j < numRows; j++)
                {
                    nodeMatrix[i,j] = new Nodo(i, j);
                }
            }
        }

        private void AssignGoals(BoardInfo boardInfo, CellInfo[] goals) //Función que asigna la posición de la meta a todos los nodos
        {
            for (int i = 0; i < boardInfo.NumColumns; i++)
            {
                for (int j = 0; j < boardInfo.NumRows; j++)
                {
                    nodeMatrix[i,j].SetGoal(goals[0].ColumnId, goals[0].RowId);
                }
            }
        }

        private void SearchGoal(BoardInfo boardInfo) //Función encargada de encontrar el nodo meta
        {
            Nodo currentNode = queue.Pop(); //Se obtiene el primer elemento de la cola
            if (currentNode.isGoal()) //Si es meta se rellena la lista con el camino que lleva hasta este nodo
            {
                path.AddFirst(currentNode);
                AddParentsToPath(currentNode);
                Debug.Log("Meta encontrada");
            }
            else //Si no es meta se expande. Los nodos al inicializarse no tienen padre. Si los vecinos no cuentan con padre,
                 //se calcula su f* y se les añade a la cola. Si ya tienen padre ya han sido añadidos previamente, así que no se hace nada con ellos
            {
                CellInfo currentCell = boardInfo.CellInfos[currentNode.posX, currentNode.posY]; //Celda correspondiente al nodo a expandir
                CellInfo[] neighbours = currentCell.WalkableNeighbours(boardInfo);
                foreach(CellInfo neighbour in neighbours)
                {
                    if (neighbour != null) //Neighbours.Length es siempre 4, pero no todas las celdas tienen 4 vecinos
                    {
                        Nodo neighbourNode = nodeMatrix[neighbour.ColumnId,neighbour.RowId]; //Nodo correspondiente a la celda vecina que se está tratando
                        if(neighbourNode.GetParent() == null) //Si no tiene padre significa que no se ha añadido a la lista, pues el padre se le asigna cuando se le añade
                        {
                            neighbourNode.SetParent(currentNode);
                            neighbourNode.CalculateF();
                            queue.Add(neighbourNode);
                        }
                    }
                        
                }
                if(queue.first != null) SearchGoal(boardInfo); //Si no se ha encontrado el camino se sigue buscando una vez se han añadido los hijos a la cola.
                                                               //Importante: si la cola está vacía significa que no hay camino existente hasta la meta
            }
        }

        private void AddParentsToPath(Nodo node) //Crea el camino que lleva hasta la meta
        {
            if(node.GetParent().GetParent() != node.GetParent()) //Si el padre de la celda inicial no es él mismo, es decir, si el padre no es la celda inicial en la que
                                                                 //se sitúa el jugador, se añaden al camino. La celda inicial no forma parte del camino.
            {
                path.AddFirst(node.GetParent()); //Se añaden al principio de la lista. De lo contrario daría el camino desde la meta hasta el jugador
                AddParentsToPath(node.GetParent());
            }
            else
            {
                pathFound = true; //Ya se tiene el camino a recorrer. El algoritmo ha finalizado.
            }
        }
    }
}
