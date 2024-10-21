using Components;
using NavigationDJIA.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class QTable2
{
    private Dictionary<int, float[]> QTable = new();
    private const int NUM_ACTIONS = 4;

    public bool Contains(QState state)
    {
        int key = state.GetHashCode();
        if (QTable.ContainsKey(key)) return true;
        return false;
    }
    public float GetValue(QState state, int action)
    {
        int key = state.GetHashCode();
        if (!QTable.ContainsKey(key))
        {
            QTable[key] = new float[NUM_ACTIONS]; //Por defecto a 0
        }
        return QTable[key][action];
    }

    public void SetValue(QState state, int action, float value)
    {
        int key = state.GetHashCode();
        if (!QTable.ContainsKey(key))
        {
            QTable[key] = new float[NUM_ACTIONS]; 
        }
        QTable[key][action] = value;
    }

    public int GetBestAction(QState state)
    {
        int key = state.GetHashCode();
        if (!QTable.ContainsKey(key))
        {
            Debug.Log("Estado no encontrado");
            return UnityEngine.Random.Range(0, NUM_ACTIONS);
        }
        return Array.IndexOf(QTable[key], QTable[key].Max());
    }

    public float GetBestValue(QState state)
    {
        int key = state.GetHashCode();
        if (!QTable.ContainsKey(key)) return 0;
        return QTable[key].Max();
    }

    public void SaveTable()
    {
        string dataPath = $"{Application.dataPath}/QTable";
        string csv = String.Join(
        Environment.NewLine,
        QTable.Select(d => $"{d.Key};{String.Join(";", d.Value)}"));
        System.IO.File.WriteAllText(dataPath, csv);
        Debug.Log("La tabla ha sido guardada");
    }

    public void GetTable(Dictionary<int, float[]> table)
    {
        QTable = table;
    }
}
