using Components.QLearning;
using Components;
using NavigationDJIA.Interfaces;
using NavigationDJIA.World;
using QMind.Interfaces;
using System;
using UnityEngine;

public class MyTrainer : IQMindTrainer
{
    public int CurrentEpisode { get; private set; }
    public int CurrentStep { get; private set; }
    public CellInfo AgentPosition { get; private set; }
    public CellInfo OtherPosition { get; private set; }
    public float Return { get; }
    public float ReturnAveraged { get; }
    public event EventHandler OnEpisodeStarted;
    public event EventHandler OnEpisodeFinished;
    private INavigationAlgorithm _navigationAlgorithm;
    private int counter = 0;

    private QTable table = new();
    private bool restart = false;

    public void Initialize(QMind.QMindTrainerParams qMindTrainerParams, WorldInfo worldInfo, INavigationAlgorithm navigationAlgorithm)
    {
        _navigationAlgorithm = QMind.Utils.InitializeNavigationAlgo(navigationAlgorithm, worldInfo);

        AgentPosition = worldInfo.RandomCell();
        OtherPosition = worldInfo.RandomCell();
        OnEpisodeStarted?.Invoke(this, EventArgs.Empty);
    }

    public void DoStep(bool train)
    {
        //Init state:
        QState initialState = new QState(AgentPosition, OtherPosition, WorldManager.Instance.WorldInfo);
        CellInfo initialOtherPosition = OtherPosition;
        //Step increase:
        CurrentStep = counter;
        counter += 1;
        //Player movement:
        CellInfo otherCell = QMind.Utils.MoveOther(_navigationAlgorithm, OtherPosition, AgentPosition);
        if (otherCell != null) OtherPosition = otherCell;
        //Agent movement:
        if (train) //If it is training
        {
            int reward;
            int direction;
            //Depending on exploration rate, the next movement can be randomly chosen or not
            if (NextMoveIsRandom())
            {
                direction = UnityEngine.Random.Range(0, 4);
                AgentPosition = QMind.Utils.MoveAgent(direction, AgentPosition, WorldManager.Instance.WorldInfo);
            }
            else
            {
                direction = MoveToBestOption(initialState);
            }
            //Reward if calculated
            reward = CalculateReward(initialOtherPosition);
            //New Q value is calculated:
            CalculateNewQ(initialState, direction, initialOtherPosition, reward);

            if (restart) Restart();
        }
        else
        {
            MoveToBestOption(initialState);
        }
    }

    private int CalculateReward(CellInfo otherInitialPosition)
    {
        if (!AgentPosition.Walkable) //If the chosen position was out of bounds or a wall
        {
            restart = true;
            Debug.Log("Unallowed action");
            return -1000;
        }
        else if (AgentPosition == OtherPosition || AgentPosition == otherInitialPosition) //If is caught by the opponent
        {
            restart = true;
            Debug.Log("Caught");
            return -750;
        }
        else return 0;
    }

    private void CalculateNewQ(QState initialState, int action, CellInfo otherInitialPosition, int reward)
    {
        float currentQ = table.GetValue(initialState, action);
        QState newState = new QState(AgentPosition, otherInitialPosition, WorldManager.Instance.WorldInfo);
        float alpha = GameObject.FindFirstObjectByType<QMindTrainer>().algorithmParams.alpha;
        float gamma = GameObject.FindFirstObjectByType<QMindTrainer>().algorithmParams.gamma;
        float newQ = (1 - alpha) * currentQ + alpha * (reward + gamma * table.GetBestValue(newState));
        table.SetValue(initialState, action, newQ);
    }

    private bool NextMoveIsRandom()
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        if (random <= GameObject.FindFirstObjectByType<QMindTrainer>().algorithmParams.epsilon) return true;
        return false;
    }

    private int MoveToBestOption(QState initialState)
    {
        int bestAction = table.GetBestAction(initialState);
        AgentPosition = QMind.Utils.MoveAgent(bestAction, AgentPosition, WorldManager.Instance.WorldInfo);
        return bestAction;
    }

    private void Restart()
    {
        OnEpisodeFinished?.Invoke(this, EventArgs.Empty);
        CurrentEpisode++;
        AgentPosition = WorldManager.Instance.WorldInfo.RandomCell();
        OtherPosition = WorldManager.Instance.WorldInfo.RandomCell();
        counter = 0;
        CurrentStep = counter;
        OnEpisodeStarted?.Invoke(this, EventArgs.Empty);
        restart = false;
        //Qtable is stored in a .csv file:
        if (CurrentEpisode % GameObject.FindFirstObjectByType<QMindTrainer>().algorithmParams.episodesBetweenSaves == 0)
            table.SaveTable();
    }
}
