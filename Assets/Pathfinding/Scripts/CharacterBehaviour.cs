using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(Locomotion))]
    public class CharacterBehaviour: MonoBehaviour
    {
        protected Locomotion LocomotionController;
        protected AbstractPathMind PathController;
        public BoardManager BoardManager { get; set; }
        protected CellInfo currentTarget;
       
        void Awake()
        {
            PathController = GetComponentInChildren<AbstractPathMind>();
            PathController.Character = this;
            LocomotionController = GetComponent<Locomotion>();
            LocomotionController.SetCharacter(this);
        }

        void Update()
        {
            if (BoardManager == null) return;
            if (LocomotionController.MoveNeed)
            {
                var boardClone = (BoardInfo)BoardManager.boardInfo.Clone();
                LocomotionController.SetNewDirection(PathController.GetNextMove(boardClone,LocomotionController.CurrentEndPosition(), new[]{this.currentTarget}));
            }
        }

        public void SetCurrentTarget(CellInfo newTargetCell)
        {
            this.currentTarget = newTargetCell;
        }
    }
}

