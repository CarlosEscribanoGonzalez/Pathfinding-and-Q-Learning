using UnityEngine;

namespace Pathfinding
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } = null;
        public BoardManager BoardManager { get; set; }
        public int seed = 2016;
        public bool ForPlanner = false;
        public int numEnemies;
        
        void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            this.BoardManager = GetComponent<BoardManager>();
        }

        public void Start()
        {
            var character = GameObject.Find("Character").GetComponent<CharacterBehaviour>();
            character.BoardManager = BoardManager;
            character.SetCurrentTarget(BoardManager.boardInfo.Exit);
        }

        //Initializes the game for each level.
        public void InitGame()
        {
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            BoardManager.SetupScene(this.seed, this.ForPlanner,numEnemies);
            BoardManager.GenerateMap();
        }
    }
}
