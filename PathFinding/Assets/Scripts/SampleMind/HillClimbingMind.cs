using Assets.Scripts.AStarMind;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.HillClimbingMind
{
    public class HillClimbingMind : AbstractPathMind {
        private EnemyBehaviour[] enemyBehaviours; //Lista de enemigos, los cuales son detectados al tener todos el mismo script
        private GameObject currentTarget; //Enemigo al que persigue el jugador
        private CellInfo nextCell; //Siguiente celda a la que moverse
        private AbstractPathMind AStarBehaviour; //Cuando se han eliminado todos los enemigos el jugador avanza hasta la meta con el A* ya implementado

        private void Start()
        {
            AStarBehaviour = GameObject.Find("A*").GetComponent<AbstractPathMind>();
            AStarBehaviour.gameObject.SetActive(false);
        }

        public override void Repath()
        {
            
        }

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            enemyBehaviours = FindObjectsOfType<EnemyBehaviour>(); //Se añaden los enemigos a la lista
            if (enemyBehaviours.Length != 0) //Si hay enemigos se ejecuta el ascenso de colinas
            {
                goals[0].ChangeToNoWalkable(); //Previene que el personaje llegue a la meta sin eliminar los enemigos
                FindNearestEnemy(currentPos); //Se encuentra el enemigo más cercano
                //Una vez tenemos al enemigo más cercano localizado procedemos a perseguirle. Para ello, expandimos el nodo actual
                //y miramos cuál de los nodos contiguos están más cerca del objetivo
                FindBestOption(boardInfo, currentPos);
            }
            else //En el momento en el que se han eliminado los enemigos la meta se desbloquea y el personaje llega hasta ella mediante
                 //el algoritmo ya implementado de A*
            {
                goals[0].ChangeToWalkable(); //La meta se vuelve de nuevo caminable
                GameObject.Find("CharacterEnemyScene").GetComponent<CharacterBehaviour>().SetPathController(AStarBehaviour); //Se cambia el tipo de algoritmo implementado
            }

            if (nextCell.ColumnId > currentPos.ColumnId) return Locomotion.MoveDirection.Right;
            else if (nextCell.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;
            if (nextCell.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
            return Locomotion.MoveDirection.Down;
        }

        private void FindNearestEnemy(CellInfo currentPos) //Función que se encarga de encontrar el enemigo más cercano y lo almacena en "currentTarget"
        {
            int minDistance = 1000; //Hay que encontrar el enemigo que esté a una menor distancia gracias a esta variable
            foreach (EnemyBehaviour enemy in enemyBehaviours)
            {
                int distance = CalculateDistance(enemy, currentPos);
                if (distance < minDistance)
                {
                    currentTarget = enemy.gameObject; //El enemigo al que perseguirá el personaje será el más cercano
                    minDistance = distance;
                }
            }
        }

        private void FindBestOption(BoardInfo boardInfo, CellInfo currentPos) //Función que determina qué celda es más óptima 
        {
            CellInfo[] neighbours = currentPos.WalkableNeighbours(boardInfo);
            //Repetimos el mismo procedimiento de antes para ver la celda más cercana a currentTarget:
            int minDistance = 1000;
            foreach (CellInfo neighbour in neighbours)
            {
                if (neighbour != null)
                {
                    int distance = CalculateDistance(currentTarget.GetComponent<EnemyBehaviour>(), neighbour);
                    if (distance < minDistance)
                    {
                        nextCell = neighbour;
                        minDistance = distance;
                    }
                }
            }
        }

        private int CalculateDistance(EnemyBehaviour enemy, CellInfo position) //Función que calcula la distancia entre el enemigo y una celda
        {
            int distance = (int)Mathf.Abs(Mathf.Round(enemy.transform.position.x) - position.ColumnId); //Distancia respecto al eje x
            distance += (int)Mathf.Abs(Mathf.Round(enemy.transform.position.y) - position.RowId); //Se le añade la distancia respecto al eje y
            return distance;
        }
    }
}
