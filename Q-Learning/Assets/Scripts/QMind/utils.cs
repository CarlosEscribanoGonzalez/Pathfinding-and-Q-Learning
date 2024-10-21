using NavigationDJIA.Interfaces;
using NavigationDJIA.World;

namespace QMind
{
    public static class Utils
    {   
        // Initialize the navigation algorithm in a given world.
        public static INavigationAlgorithm InitializeNavigationAlgo(INavigationAlgorithm navigationAlgorithm, WorldInfo worldInfo)
        {
            navigationAlgorithm.Initialize(worldInfo);
            return navigationAlgorithm;
        }
        
        // Return the next cell where the the agent needs to be moved. Action is an integer in range [0, 3].
        public static CellInfo MoveAgent(int action, CellInfo agentPosition, WorldInfo worldInfo)
        {
            CellInfo nextCell = worldInfo.NextCell(agentPosition, worldInfo.AllowedMovements.FromIntValue(action));
            return nextCell;
        }
        
        // Return the next cell where the the player needs to be moved using an A*. If the movement is invalid it returns null.
        public static CellInfo MoveOther(INavigationAlgorithm navigationAlgorithm, CellInfo playerPosition, CellInfo agentPosition)
        {
            CellInfo[] path = navigationAlgorithm.GetPath(playerPosition, agentPosition, 1);
            if (path != null && path.Length > 0)
            {
                return path[0];    
            }
            else
            {
                return null;
            }
        }
    }
}