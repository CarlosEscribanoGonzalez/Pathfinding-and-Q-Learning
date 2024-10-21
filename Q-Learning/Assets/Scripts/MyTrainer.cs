using Components.QLearning;
using Components;
using NavigationDJIA.Interfaces;
using NavigationDJIA.World;
using QMind.Interfaces;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class MyTrainer2 : IQMindTrainer
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

    private QTable2 table = new();
    private bool restart = false;
    private CellInfo initialOtherPosition;

    public void Initialize(QMind.QMindTrainerParams qMindTrainerParams, WorldInfo worldInfo, INavigationAlgorithm navigationAlgorithm)
    {
        _navigationAlgorithm = QMind.Utils.InitializeNavigationAlgo(navigationAlgorithm, worldInfo);
        AgentPosition = worldInfo.RandomCell();
        OtherPosition = worldInfo.RandomCell();
        OnEpisodeStarted?.Invoke(this, EventArgs.Empty);
        LoadTable();
    }

    public void DoStep(bool train)
    {
        //Estado inicial:
        QState initialState = new QState(AgentPosition, OtherPosition, WorldManager.Instance.WorldInfo);
        initialOtherPosition = OtherPosition;
        //Aumento del contador:
        CurrentStep = counter;
        counter += 1;
        //Movmiento del jugador:
        CellInfo otherCell = QMind.Utils.MoveOther(_navigationAlgorithm, OtherPosition, AgentPosition);
        if (otherCell != null) OtherPosition = otherCell;
        //Movimiento del agente:
        if (train) //Si está entrenando
        {
            int reward;
            int direction;
            //Miramos si vamos a una posición aleatoria o no según el exploration rate:
            if (NextMoveIsRandom())
            {
                //Movemos al agente aleatoriamente:
                direction = UnityEngine.Random.Range(0, 4);
                AgentPosition = QMind.Utils.MoveAgent(direction, AgentPosition, WorldManager.Instance.WorldInfo);
            }
            else
            {
                //Movemos al agente a la mejor opción:
                direction = MoveToBestOption(initialState);
            }
            //Asignación de recompensas dependiendo de la acción:
            reward = CalculateReward();
            //Calculamos la nueva Q:
            CalculateNewQ(initialState, direction, reward);

            if (restart) Restart();
        }
        else
        {
            MoveToBestOption(initialState);
        }
    }

    private int CalculateReward()
    {
        if (!AgentPosition.Walkable) //Si se sale del mapa o camina contra una pared
        {
            restart = true;
            Debug.Log("Acción no permitida");
            return -1000;
        }
        else if (AgentPosition == OtherPosition || AgentPosition == initialOtherPosition) //Si es pillado por el jugador
        {
            restart = true;
            Debug.Log("Atrapado");
            return -900;
        }
        else return 0;
    }
    private void CalculateNewQ(QState initialState, int action, int reward)
    {
        float currentQ = table.GetValue(initialState, action);
        QState newState = new QState(AgentPosition, OtherPosition, WorldManager.Instance.WorldInfo);
        float alpha = GameObject.FindObjectOfType<QMindTrainer>().algorithmParams.alpha;
        float gamma = GameObject.FindObjectOfType<QMindTrainer>().algorithmParams.gamma;
        float newQ = (1 - alpha) * currentQ + alpha * (reward + gamma * table.GetBestValue(newState));
        table.SetValue(initialState, action, newQ);
    }

    private bool NextMoveIsRandom()
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        if (random <= GameObject.FindObjectOfType<QMindTrainer>().algorithmParams.epsilon) return true;
        return false;
    }

    private int MoveToBestOption(QState initialState)
    {
        int bestAction = table.GetBestAction(initialState);
        AgentPosition = QMind.Utils.MoveAgent(bestAction, AgentPosition, WorldManager.Instance.WorldInfo);
        return bestAction;
    }


    private void LoadTable()
    {
        bool[] walkables = new bool[4];
        int[] otherPos = new int[2];
        for(int distance = 0; distance < 5; distance++)
        {
            for (int otherPos1 = -1; otherPos1 < 2; otherPos1++)
            {
                otherPos[0] = otherPos1;
                for(int otherPos2 = -1; otherPos2 < 2; otherPos2++)
                {
                    otherPos[1] = otherPos2;
                    for(int w1 = 0; w1 < 2; w1++)
                    {
                        if (w1 == 0) walkables[0] = true;
                        else walkables[0] = false;
                        
                        for (int w2 = 0; w2 < 2; w2++)
                        {
                            if (w2 == 0) walkables[1] = true;
                            else walkables[1] = false;
                            
                            for (int w3 = 0; w3 < 2; w3++)
                            {
                                if (w3 == 0) walkables[2] = true;
                                else walkables[2] = false;
                                
                                for (int w4 = 0; w4 < 2; w4++)
                                {
                                    if (w4 == 0) walkables[3] = true;
                                    else walkables[3] = false;
                                    QState state = new QState(walkables, distance, otherPos);
                                    CalculateInitialValues(state);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void CalculateInitialValues(QState state)
    {
        for(int i = 0; i < 4; i++)
        {
            int action = i;
            if (!state.walkables[action])  //Si la celda no es caminable
            {
                table.SetValue(state, action, -1000);
            }
            else if (state.distance == 4)  // Si es pillado
            {
                table.SetValue(state, action, -900);
            }
            else
            {
                table.SetValue(state, action, 0);
            }
        }
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
        //Guardado de la tabla en un CSV:
        if (CurrentEpisode % GameObject.FindObjectOfType<QMindTrainer>().algorithmParams.episodesBetweenSaves == 0)
        {
            table.SaveTable();
        }
    }
}
