using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QTable
{
    private Dictionary<int, float[]> table = new();
    private const int NUM_ACTIONS = 4;

    public float GetValue(QState state, int action)
    {
        int key = state.GetHashCode();
        if (!table.ContainsKey(key))
        {
            table[key] = new float[NUM_ACTIONS]; //0 by default
            Debug.Log("New state created");
        }
        return table[key][action];
    }

    public void SetValue(QState state, int action, float value)
    {
        int key = state.GetHashCode();
        if (!table.ContainsKey(key))
        {
            table[key] = new float[NUM_ACTIONS]; 
        }
        table[key][action] = value;
    }

    public int GetBestAction(QState state)
    {
        int key = state.GetHashCode();
        if (!table.ContainsKey(key))
        {
            Debug.Log("State not found");
            return UnityEngine.Random.Range(0, NUM_ACTIONS);
        }
        return Array.IndexOf(table[key], table[key].Max());
    }

    public float GetBestValue(QState state)
    {
        int key = state.GetHashCode();
        if (!table.ContainsKey(key)) return 0;
        return table[key].Max();
    }

    public void SaveTable()
    {
        string dataPath = $"{Application.dataPath}/QTable";
        string csv = String.Join(
        Environment.NewLine,
        table.Select(d => $"{d.Key};{String.Join(";", d.Value)}"));
        System.IO.File.WriteAllText(dataPath, csv);
        Debug.Log($"QTable has been saved to {dataPath}");
    }

    public void GetTable(Dictionary<int, float[]> table)
    {
        this.table = table;
    }
}
