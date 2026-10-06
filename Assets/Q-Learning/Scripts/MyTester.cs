using NavigationDJIA.World;
using QMind.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MyTester : IQMind
{
    private WorldInfo worldInfo;
    private readonly QTable table = new();

    public void Initialize(WorldInfo worldInfo)
    {
        this.worldInfo = worldInfo;
        LoadQTable();
    }

    public CellInfo GetNextStep(CellInfo currentPosition, CellInfo otherPosition)
    {
        QState state = CalculateState(currentPosition, otherPosition);
        CellInfo agentCell = null;
        int action = GetAction(state);
        agentCell = QMind.Utils.MoveAgent(action, currentPosition, worldInfo);
        Debug.Log("Action = " + action);

        return agentCell;
    }

    private int GetAction(QState state)
    {
        return table.GetBestAction(state);
    }

    private void LoadQTable()
    {
        string pathToCsv = $"{Application.dataPath}/QTable";
        if (File.Exists($"{Application.dataPath}/QTable"))
        {
            string[] lines = System.IO.File.ReadAllLines(pathToCsv);
            Dictionary<int, float[]> loadedQTable = new Dictionary<int, float[]>();
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                int key = int.Parse(parts[0]);
                float[] values = parts.Skip(1).Select(float.Parse).ToArray();
                loadedQTable[key] = values;
            }
            table.GetTable(loadedQTable);
        }
    }

    private QState CalculateState(CellInfo currentPosition, CellInfo otherPosition)
    {
        return new QState(currentPosition, otherPosition, worldInfo);
    }
}
