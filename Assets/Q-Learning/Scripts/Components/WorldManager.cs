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

using NavigationDJIA.World;
using UnityEngine;

namespace Components
{
    public class WorldManager : MonoBehaviour
    {
        public WorldInfo WorldInfo
        {
            get;
            private set;
        }
        
        public float worldResolution = 1.0f;
        public bool debugWorld = false;

        private static WorldManager _instance;
        public static WorldManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            
            WorldInfo = new WorldInfo(gameObject, AllowedDirections.FourDirections, worldResolution);
        }

        private void Start()
        {
            DebugWorld();                   
        }
        
        private GameObject _debugObject;
        
        public void DebugWorld()
        {
            if(!debugWorld) return;
            
            if (_debugObject != null)
            {
                Destroy(_debugObject);
            }

            _debugObject = new GameObject();
            _debugObject.transform.SetParent(transform);
            _debugObject.name = "Debug";
            
            for (int x = 0; x < WorldInfo.WorldSize.x; x++)
            {
                for (int y = 0; y < WorldInfo.WorldSize.y; y++)
                {
                    CellInfo cell = WorldInfo[x, y];
                    
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
                    go.transform.SetParent(_debugObject.transform);
                    Vector3 cellPosition = WorldInfo.ToWorldPosition(cell);
                    cellPosition.y = cell.Type == CellInfo.CellType.Floor ? 0.0f : 1.0f;
                    go.transform.position = cellPosition;
                    go.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    go.GetComponent<MeshRenderer>().material.color = (x==0 && y==0) ? Color.black : cell.CellColor();
                    Destroy(go.GetComponent<SphereCollider>());
                }
            }
        }
    }
}