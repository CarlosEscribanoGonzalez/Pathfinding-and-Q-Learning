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
using NavigationDJIA.World;
using QMind;
using QMind.Interfaces;
using Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Components.QLearning
{
    public class QMindTrainer : MonoBehaviour
    {
        public Movable agent;
        public Movable other;

        public string qLearningTrainerClass;
        public bool showSimulation = false;
        public float agentSpeed = 1f;
        public bool train;
        public QMindTrainerParams algorithmParams;
        
        private WorldInfo _worldInfo;
        private IQMindTrainer _qMindTrainer;
        private CellInfo _agentCell;
        private CellInfo _oponentCell;

        public void Start()
        {
            Assert.IsNotNull(other);
            Assert.IsNotNull(agent);

            _worldInfo = WorldManager.Instance.WorldInfo;
            
            Type qMindTrainerType = System.Type.GetType(qLearningTrainerClass);
            Assert.IsNotNull(qMindTrainerType);
            _qMindTrainer = (IQMindTrainer) Activator.CreateInstance(qMindTrainerType);
            _qMindTrainer.OnEpisodeStarted += EpisodeStarted;
            _qMindTrainer.OnEpisodeFinished += EpisodeFinished;

            _qMindTrainer.Initialize(algorithmParams, _worldInfo, new AStarNavigation());
        }

        private void EpisodeStarted(object sender, EventArgs e)
        {
            _agentCell = _qMindTrainer.AgentPosition;
            _oponentCell = _qMindTrainer.OtherPosition;
            
            agent.transform.position  = _worldInfo.ToWorldPosition(_agentCell);
            other.transform.position = _worldInfo.ToWorldPosition(_oponentCell);
        }
        
        private void EpisodeFinished(object sender, EventArgs e)
        {
            if (algorithmParams.episodes != -1 && _qMindTrainer.CurrentEpisode >= algorithmParams.episodes)
            {
                Debug.Log($"Max episodes reached, stopping simulation");
                EditorApplication.ExitPlaymode();
            }
        }

        private bool _started = false;
        
        public void Update()
        {
            if (!showSimulation)
            {
                _qMindTrainer.DoStep(train);
            }
            else
            {
                if (!_started || (agent.DestinationReached && other.DestinationReached))
                {
                    agent.speed = agentSpeed;
                    other.speed = agentSpeed;
                    
                    _started = true;
                    _qMindTrainer.DoStep(train);
        
                    _agentCell = _qMindTrainer.AgentPosition;
                    _oponentCell = _qMindTrainer.OtherPosition;
                    
                    agent.destination = _worldInfo.ToWorldPosition(_agentCell);
                    other.destination = _worldInfo.ToWorldPosition(_oponentCell);
                }
            }
        }
        
        private void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.fontSize = 22;
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.normal.textColor = Color.black;

            GUI.Label(new Rect(10, 10, 300, 30), $"Episodes: {_qMindTrainer.CurrentEpisode}[{_qMindTrainer.CurrentStep}]", guiStyle);
            GUI.Label(new Rect(10, 40, 300, 30), $"Averaged reward: {_qMindTrainer.ReturnAveraged}", guiStyle);
            GUI.Label(new Rect(10, 70, 300, 30), $"Total reward: {_qMindTrainer.Return}", guiStyle);
        }
    }
}