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
using System.Collections.Generic;
using NavigationDJIA.Interfaces;
using QMind.Interfaces;
using Tools;
using UnityEditor;

namespace Components.QLearning.Editor
{
    [CustomEditor(typeof(QMindTester))]
    public class QMindTesterEditor : UnityEditor.Editor
    {
        private SerializedProperty _qMindClass;
        
        private List<string> _qMindClasses;
        
        private void OnEnable()
        {
            _qMindClass = serializedObject.FindProperty("qMindClass");
            _qMindClasses = typeof(IQMind).Subclasses();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawPropertiesExcluding(serializedObject, "m_Script", "qMindClass");
            
            _qMindClass.stringValue = ShowClassSelector( "QMind Agent", _qMindClasses, _qMindClass.stringValue);

            serializedObject.ApplyModifiedProperties();
        }
        
        private string ShowClassSelector(String label, List<String> classes, string currentClass)
        {
            string[] options = classes.ToArray();
            int classIndex = 0;

            if (currentClass != null)
            {
                classIndex = Array.IndexOf(options, currentClass);
                classIndex = classIndex < 0 ? 0 : classIndex;
            }

            classIndex = EditorGUILayout.Popup(label, classIndex, options);
            
            string selectedClass = options[classIndex];
            return selectedClass;
        }
    }
}