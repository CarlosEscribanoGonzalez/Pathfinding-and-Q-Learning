#region Copyright
// MIT License
// 
// Copyright (c) 2023 David María Arribas
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using NavigationDJIA.Algorithms.AStar;
using NavigationDJIA.Interfaces;
using NavigationDJIA.World;
using QMind.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Components.QLearning
{
    public class QMindTester : MonoBehaviour
    {
        public Movable agent;
        public Movable oponent;
        public float speed = 1.0f;
        public string qMindClass;

        private CellInfo _agentCell;
        private CellInfo _opponentCell;

        private INavigationAlgorithm _navigationAlgorithm;
        private IQMind _qMind;
        private WorldInfo _worldInfo;

        private int _steps = 0;
        private int _captures = 0;
        
        private void Start()
        {
            _worldInfo = WorldManager.Instance.WorldInfo;

            Type qMindType = System.Type.GetType(qMindClass);
            Assert.IsNotNull(qMindType);
            _qMind = (IQMind) Activator.CreateInstance(qMindType);
            _qMind.Initialize(_worldInfo);

            _navigationAlgorithm = new AStarNavigation();
            _navigationAlgorithm.Initialize(_worldInfo);

            _agentCell = _worldInfo.FromVector3(agent.transform.position);
            _opponentCell = _worldInfo.FromVector3(oponent.transform.position);
        }

        private void Update()
        {
            if (agent.DestinationReached && oponent.DestinationReached)
            {
                agent.speed = speed;
                oponent.speed = speed;

                MoveAgent();
                MoveOpponent();
                
                if(_agentCell == _opponentCell)
                {
                    _captures++;
                    Debug.Log($"Opponent captured agent in {_steps} steps");
                    EditorApplication.ExitPlaymode();
                }
                
                _steps++;
            }
        }

        private void MoveAgent()
        {
            CellInfo newAgentCell = _qMind.GetNextStep(_agentCell, _opponentCell);
            
            if(newAgentCell != null)
            {
                _agentCell = newAgentCell;
            }
            
            agent.destination = _worldInfo.ToWorldPosition(_agentCell);
        }

        private void MoveOpponent()
        {
            CellInfo[] path = _navigationAlgorithm.GetPath(_opponentCell, _agentCell, 1);
            if (path.Length > 0)
            {
                _opponentCell = path[0];    
            }
            oponent.destination = _worldInfo.ToWorldPosition(_opponentCell);
        }
        
        private void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.fontSize = 22;
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.normal.textColor = Color.black;
            
            GUI.Label(new Rect(10, 10, 300, 30), $"Steps: {_steps}", guiStyle);
            GUI.Label(new Rect(10, 40, 300, 30), $"Captures: {_captures}", guiStyle);
        }
    }
}