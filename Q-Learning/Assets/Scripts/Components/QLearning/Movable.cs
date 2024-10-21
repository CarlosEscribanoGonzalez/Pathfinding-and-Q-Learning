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
using Tools;
using UnityEngine;

namespace Components.QLearning
{
    public class Movable : MonoBehaviour
    {
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        [HideInInspector] public float speed;
        [HideInInspector] public Vector3 destination;

        public bool DestinationReached => transform.position.CloseTo(destination, 0.2f);
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            destination = transform.position;
        }

        private void Update()
        {
            Vector3 position = transform.position;
            Vector3 distanceVector = (destination - position);
            float moveDistance = speed * Time.deltaTime;

            if (distanceVector.magnitude < moveDistance)
            {
                moveDistance = distanceVector.magnitude;
            }
            
            Vector3 direction = distanceVector.normalized;
            Vector3 move = direction * moveDistance;
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z) + position;
            
            transform.LookAt(lookDirection);
            position += move;
            transform.position = position;
            
            _animator.speed = speed;
        }
    }
}