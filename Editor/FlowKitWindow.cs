/* Copyright 2025 Hollow1
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* --------------------------------------------------------
* FlowKit - Editor window component
* Created by Hollow1
* 
* A window aimed to help users with code for their Animations
* 
* Version: 1.0.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FlowKit.Editor
{
    public class FlowKitWindow : EditorWindow
    {
        private static FlowKitWindow window;

        private readonly Dictionary<GameObject, List<GameObject>> _parentChildMap = new Dictionary<GameObject, List<GameObject>>();

        private readonly GUIStyle _instructionStyle = new GUIStyle();

        private GameObject selectedObject;
        // Easier to test with target 
        // Either move to target with button or go back to selected following original idea
        private GameObject targetObject;
        private float rotationSpeed = 90f;

        private bool isAnimating = false;
        private double startTime = 0d;

        [MenuItem("Window/FlowKit/Live Preview")]
        public static void ShowWindow()
        {
            window = GetWindow<FlowKitWindow>("Live Preview");
            window.minSize = new Vector2(500, 500);
        }

        void OnEnable()
        {
            _parentChildMap.Clear();

            InitParentsInMap();
            InitStyles();
        }

        private void OnDisable()
        {
            _parentChildMap.Clear();

            Undo.PerformUndo();
        }

        private void OnGUI()
        {
            LabelInstructions(); 
            DrawParents();
            GetSelectedObject();

            targetObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", targetObject, typeof(GameObject), true);
            rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", rotationSpeed);

            if (GUILayout.Button(isAnimating ? "Stop Animation" : "Start Animation"))
            {
                if (targetObject == null)
                {
                    Debug.LogWarning("No target object selected for animation.");

                    return;
                }

                isAnimating = !isAnimating;
                startTime = EditorApplication.timeSinceStartup;

                EditorUtility.SetDirty(targetObject); 
                Undo.RecordObject(targetObject.transform, $"Undo {targetObject.name} Animation");
            }
        }

        private void Update()
        {
            if (!isAnimating || targetObject == null) { return; }

            double currentTime = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(currentTime - startTime);
            startTime = currentTime;

            targetObject.transform.Rotate(Vector3.up, rotationSpeed * deltaTime, Space.World);

            SceneView.RepaintAll();
        }

        // -------------------------------------------------------------- INITIALIZERS --------------------------------------------------------------

        private void InitParentsInMap()
        {
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>(false);

            foreach (GameObject obj in gameObjects)
            {
                if (obj.layer == LayerMask.NameToLayer("UI"))
                {
                    if (obj.GetComponent<RectTransform>() && obj.GetComponent<Core.FlowKitEngine>())
                    {
                        _parentChildMap.Add(obj, new List<GameObject>());
                    }
                }
            }
        }

        private void InitStyles()
        {
            _instructionStyle.fontSize = 14;
            _instructionStyle.normal.textColor = Color.white;
            _instructionStyle.alignment = TextAnchor.MiddleCenter;
        }

        // -------------------------------------------------------------- DRAW GUI --------------------------------------------------------------

        // TODO: Make this look better
        private void LabelInstructions()
        {
            GUILayout.Label("To select a component for animation", _instructionStyle);
            GUILayout.Label("double click within this window or select in the hierarchy window", _instructionStyle);
            GUILayout.Label("(Object must be a child of a panel & be active in the hierarchy)", _instructionStyle);
        }

        private void DrawParents()
        {
            EditorGUILayout.BeginVertical();

            foreach (GameObject obj in _parentChildMap.Keys)
            {
                EditorGUILayout.ObjectField(obj, typeof(GameObject), true);

                DrawChildren(obj);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawChildren(GameObject parent)
        {
            EditorGUILayout.BeginHorizontal();

            foreach (RectTransform child in parent.transform)
            {
                EditorGUILayout.ObjectField(child.gameObject, typeof(GameObject), true);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void GetSelectedObject()
        {
            selectedObject = Selection.activeObject as GameObject;

            if (selectedObject == null || !selectedObject.activeInHierarchy) { return; }
            if (selectedObject.layer != LayerMask.NameToLayer("UI")) { return; }
            if (selectedObject.GetComponent<RectTransform>() == null) { return; }

            Debug.Log($"Selected object: {(selectedObject != null ? selectedObject.name : "null")}");
        }

        // -------------------------------------------------------------- ANIMATION TESTS --------------------------------------------------------------

        
    }
}