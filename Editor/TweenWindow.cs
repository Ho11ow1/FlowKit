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
* FlowKit - Tweening Editor window component
* Created by Hollow1
* 
* A window aimed to help users with code for their Animations
* 
* Version: 1.0.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

namespace FlowKit.Editor
{
    public class TweenWindow : EditorWindow
    {
        private static TweenWindow window;

        // Tween variables
        private readonly List<GameObject> _tweenList = new List<GameObject>();
        private Vector2 _scrollView;

        // Track which object is selected for code generation
        private RectTransform _selectedForCodeGen = null;

        // Styling variables
        private readonly GUIStyle _componentStyle = new GUIStyle();
        private readonly GUIStyle _childrenStyle = new GUIStyle();

        // Colour variables
        private Color _activeColour = new Color32(114, 144, 223, 255);
        private Color _inactiveColour = new Color32(255, 105, 130, 255);

        // Size variables
        private readonly GUILayoutOption _labelWidth = GUILayout.Width(52);
        private readonly GUILayoutOption _inputWidth = GUILayout.Width(30);
        private readonly GUILayoutOption _vectorWidth = GUILayout.Width(100);
        private const int _selectWidth = 55;
        private const int _childIndent = 15;
        private const int _dropdownWidth = 148;

        // Conveluted KeyValuePair finally works
        private readonly Dictionary<GameObject, Dictionary<int, Dictionary<RectTransform, AnimationData>>> _panelDataMap = 
                     new Dictionary<GameObject, Dictionary<int, Dictionary<RectTransform, AnimationData>>>();

        // Component index mathching Core
        internal const int _textIndex = 1;
        internal const int _imageIndex = 2;
        internal const int _buttonIndex = 3;

        [MenuItem("Window/FlowKit/Tween Helper")]
        public static void ShowWindow()
        {
            window = GetWindow<TweenWindow>("Tween Helper");
            window.position = new Rect(550, 250, 700, 550);
            window.Show();
        }

        void OnEnable()
        {
            _componentStyle.alignment = TextAnchor.MiddleLeft;
            _componentStyle.stretchWidth = true;
            _componentStyle.wordWrap = false;

            _childrenStyle.alignment = TextAnchor.MiddleLeft;
            _childrenStyle.margin = new RectOffset(0, 0, 0, 0);
            _childrenStyle.padding = new RectOffset(0, 0, 0, 0);
            _childrenStyle.margin.left = _childIndent;
            _childrenStyle.stretchWidth = true;
            _childrenStyle.wordWrap = false;

            FindPanels();
        }

        private void OnDestroy()
        {
            _panelDataMap.Clear();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            EditorGUILayout.LabelField("Tween Objects", EditorStyles.boldLabel);
            if (_tweenList.Count == 0)
            {
                EditorGUILayout.HelpBox("No UI panels with `Tween` component found.", MessageType.Info);
            }

            _scrollView = EditorGUILayout.BeginScrollView(_scrollView);
            EditorGUILayout.BeginVertical("box");

            for (int i = 0; i < _tweenList.Count; i++)
            {
                GameObject obj = _tweenList[i];
                if (obj != null)
                {
                    EditorGUILayout.BeginHorizontal(_componentStyle);

                    if (!obj.activeInHierarchy)
                    {
                        GUI.backgroundColor = _inactiveColour;
                        EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                    }
                    else
                    {
                        GUI.backgroundColor = _activeColour;
                        EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                    }

                    GUI.backgroundColor = _activeColour;
                    if (!obj.activeInHierarchy)
                    { GUI.backgroundColor = _inactiveColour; }
                    if (GUILayout.Button("Find", GUILayout.Width(_selectWidth)))
                    {
                        Selection.activeGameObject = obj;
                        EditorGUIUtility.PingObject(obj);
                    }

                    EditorGUILayout.EndHorizontal();

                    DrawChildren(obj);
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        // -------------------------------------------------------------- SET PANEL DATA --------------------------------------------------------------

        private void FindPanels()
        {
            _tweenList.Clear();
            _panelDataMap.Clear();

            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>(true);

            foreach (GameObject obj in gameObjects)
            {
                if (obj.layer == LayerMask.NameToLayer("UI"))
                {
                    if (obj.GetComponent<FlowKit.Core.Tween>() != null)
                    {
                        _tweenList.Add(obj);
                        InitializePanelData(obj);
                    }
                }
            }

            Debug.Log($"Found {_tweenList.Count} UI panels with `Tween` scripts (including inactive)");
        }

        private void InitializePanelData(GameObject panel)
        {
            if (!_panelDataMap.ContainsKey(panel))
            {
                _panelDataMap[panel] = new Dictionary<int, Dictionary<RectTransform, AnimationData>>
            {
                { _textIndex, new Dictionary<RectTransform, AnimationData>() },
                { _imageIndex, new Dictionary<RectTransform, AnimationData>() },
                { _buttonIndex, new Dictionary<RectTransform, AnimationData>() }
            };
            }
        }

        // -------------------------------------------------------------- DRAW GUI --------------------------------------------------------------

        private void DrawChildren(GameObject gameObject)
        {
            List<RectTransform> rects = new List<RectTransform>();

            foreach (RectTransform child in gameObject.transform)
            {
                rects.Add(child);
            }

            foreach (RectTransform rect in rects)
            {
                EditorGUILayout.BeginHorizontal(_childrenStyle);

                if (!rect.gameObject.activeInHierarchy)
                {
                    GUI.backgroundColor = _inactiveColour;
                    EditorGUILayout.ObjectField(rect.gameObject, typeof(GameObject), true);
                }
                else
                {
                    EditorGUILayout.ObjectField(rect.gameObject, typeof(GameObject), true);
                }

                GUI.backgroundColor = _activeColour;
                if (!rect.parent.gameObject.activeInHierarchy)
                { GUI.backgroundColor = _inactiveColour; }
                if (GUILayout.Button("Code", GUILayout.Width(_selectWidth)))
                {
                    _selectedForCodeGen = (_selectedForCodeGen == rect) ? null : rect;
                    Repaint();
                }

                EditorGUILayout.EndHorizontal();

                DrawAnimationControls(rect);

                if (_selectedForCodeGen == rect)
                {
                    (AnimationData dataPoint, int indexPoint, string type) = AnimationData.GetTupleAnimationData(rect, _panelDataMap);
                    if (dataPoint != null)
                    {
                        GenerateTweenCode(dataPoint, indexPoint, type);
                    }
                }
            }
        }

        private void DrawAnimationControls(RectTransform rect)
        {
            AnimationData data = AnimationData.GetAnimationData(rect, _panelDataMap);
            GUILayout.BeginHorizontal(_childrenStyle);

            GUIContent content = new GUIContent(data.animation, EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow").image);

            bool clicked = EditorGUILayout.DropdownButton(content, FocusType.Passive, GUILayout.Width(_dropdownWidth));
            var button = GUILayoutUtility.GetLastRect();

            Rect menuPos = SetMenuPosition(button);

            if (clicked)
            {
                GenericMenu menu = new GenericMenu();
                // Fade
                menu.AddItem(new GUIContent(AnimationData._fadeText + "/FadeIn"), data.animation == "FadeIn", (userData) => OnSelected((RectTransform)userData, "FadeIn"), rect);
                menu.AddItem(new GUIContent(AnimationData._fadeText + "/FadeOut"), data.animation == "FadeOut", (userData) => OnSelected((RectTransform)userData, "FadeOut"), rect);
                // Transition
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionFromUp"), data.animation == "TransitionFromUp", (userData) => OnSelected((RectTransform)userData, "TransitionFromUp"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionFromDown"), data.animation == "TransitionFromDown", (userData) => OnSelected((RectTransform)userData, "TransitionFromDown"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionFromLeft"), data.animation == "TransitionFromLeft", (userData) => OnSelected((RectTransform)userData, "TransitionFromLeft"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionFromRight"), data.animation == "TransitionFromRight", (userData) => OnSelected((RectTransform)userData, "TransitionFromRight"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionFromPosition"), data.animation == "TransitionFromPosition", (userData) => OnSelected((RectTransform)userData, "TransitionFromPosition"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionToUp"), data.animation == "TransitionToUp", (userData) => OnSelected((RectTransform)userData, "TransitionToUp"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionToDown"), data.animation == "TransitionToDown", (userData) => OnSelected((RectTransform)userData, "TransitionToDown"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionToLeft"), data.animation == "TransitionToLeft", (userData) => OnSelected((RectTransform)userData, "TransitionToLeft"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionToRight"), data.animation == "TransitionToRight", (userData) => OnSelected((RectTransform)userData, "TransitionToRight"), rect);
                menu.AddItem(new GUIContent(AnimationData._transitionText + "/TransitionToPosition"), data.animation == "TransitionToPosition", (userData) => OnSelected((RectTransform)userData, "TransitionToPosition"), rect);
                // Rotate
                menu.AddItem(new GUIContent(AnimationData._rotateText), data.animation == AnimationData._rotateText, (userData) => OnSelected((RectTransform)userData, AnimationData._rotateText), rect);
                // Scale
                menu.AddItem(new GUIContent(AnimationData._scaleText + "/ScaleUp"), data.animation == "ScaleUp", (userData) => OnSelected((RectTransform)userData, "ScaleUp"), rect);
                menu.AddItem(new GUIContent(AnimationData._scaleText + "/ScaleDown"), data.animation == "ScaleDown", (userData) => OnSelected((RectTransform)userData, "ScaleDown"), rect);
                // TypeWrite
                if (rect.gameObject.GetComponent<TextMeshProUGUI>())
                {
                    menu.AddItem(new GUIContent(AnimationData._typeWriteText), data.animation == AnimationData._typeWriteText, (userData) => OnSelected((RectTransform)userData, AnimationData._typeWriteText), rect);
                }

                menu.DropDown(menuPos);
            }

            if (data.animation != "" && data.animation != AnimationData._animationText)
            {
                EditorGUILayout.LabelField("Duration:", _labelWidth);
                data.duration = EditorGUILayout.FloatField(data.duration, _inputWidth);

                EditorGUILayout.LabelField("Delay:", _labelWidth);
                data.delay = EditorGUILayout.FloatField(data.delay, _inputWidth);
            }

            if (data.animation.StartsWith(AnimationData._transitionText))
            {
                if (data.animation == "TransitionFromPosition" || data.animation == "TransitionToPosition")
                {
                    EditorGUILayout.LabelField("Offset:", _labelWidth);
                    data.vector = EditorGUILayout.Vector2Field("", data.vector, _vectorWidth);
                }
                else
                {
                    EditorGUILayout.LabelField("Offset:", _labelWidth);
                    data.offset = EditorGUILayout.FloatField(data.offset, _inputWidth);
                }
            }
            else if (data.animation == AnimationData._rotateText)
            {
                EditorGUILayout.LabelField("Degrees:", _labelWidth);
                data.degrees = EditorGUILayout.FloatField(data.degrees, _inputWidth);
            }
            else if (data.animation.StartsWith(AnimationData._scaleText))
            {
                EditorGUILayout.LabelField("Multiplier:", _labelWidth);
                data.multiplier = EditorGUILayout.FloatField(data.multiplier, _inputWidth);
            }

            GUILayout.EndHorizontal();
        }

        private Rect SetMenuPosition(Rect buttonRect)
        {
            Vector2 mousePos = Event.current.mousePosition;
            return new Rect(mousePos.x, mousePos.y, buttonRect.width, 0);
        }

        private void OnSelected(RectTransform rect, string animation)
        {
            if (rect != null)
            {
                AnimationData data = AnimationData.GetAnimationData(rect, _panelDataMap);
                data.animation = animation;

                Repaint();
            }
        }

        // -------------------------------------------------------------- CODE GENERATION --------------------------------------------------------------

        private void GenerateTweenCode(AnimationData data, int occurrence, string type)
        {
            if (data.animation == AnimationData._animationText || string.IsNullOrEmpty(data.animation))
            {
                EditorGUILayout.HelpBox("Please select an animation type first", MessageType.Warning);
                return;
            }

            string animationString = AnimationData.GetAnimationString(data);

            GUI.backgroundColor = Color.black;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            float codeWidth = 650f;
            string[] labels =
            {
            "[SerializeField] private GameObject panel;",
            "private Tween tween;\n",
            "void Awake()",
            "    tween = panel.GetComponent<Tween>();",
            "private void PlayAnimation()",
        };

            EditorGUILayout.LabelField("// Generated Code", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField(labels[0], GUILayout.Width(codeWidth));
            EditorGUILayout.LabelField(labels[1], GUILayout.Width(codeWidth));
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField(labels[2], GUILayout.Width(codeWidth));
            EditorGUILayout.LabelField("{", GUILayout.Width(codeWidth));
            EditorGUILayout.LabelField(labels[3], GUILayout.Width(codeWidth));
            EditorGUILayout.LabelField("}", GUILayout.Width(codeWidth));
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField(labels[4], GUILayout.Width(codeWidth));
            EditorGUILayout.LabelField("{", GUILayout.Width(codeWidth));

            string parameters = $"{type}, {occurrence}";
            if (data.animation == AnimationData._typeWriteText)
            {
                parameters = $"{occurrence}";
            }

            switch (data.animation)
            {
                case AnimationData._typeWriteText:
                    if (data.delay > 0)
                        parameters += $", delay: {data.delay}f";
                    if (data.duration > 0)
                        parameters += $", duration: {data.duration}f";
                    break;

                case "TransitionFromPosition":
                case "TransitionToPosition":
                    if (data.vector != Vector2.zero)
                        parameters += $", new Vector2({data.vector.x}, {data.vector.y})";
                    if (data.duration > 0)
                        parameters += $", duration: {data.duration}f";
                    if (data.delay > 0)
                        parameters += $", delay: {data.delay}f";
                    break;

                default:
                    if (data.duration > 0)
                        parameters += $", duration: {data.duration}f";
                    if (data.delay > 0)
                        parameters += $", delay: {data.delay}f";

                    if (data.animation.StartsWith(AnimationData._transitionText) && data.offset != 0)
                        parameters += $", offset: {data.offset}f";

                    else if (data.animation == AnimationData._rotateText && data.degrees != 0)
                        parameters += $", degrees: {data.degrees}f";

                    else if (data.animation.StartsWith(AnimationData._scaleText) && data.multiplier != 0)
                        parameters += $", multiplier: {data.multiplier}f";
                    break;
            }

            GUIStyle codeStyle = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold };

            EditorGUILayout.LabelField($"    tween.{animationString}({parameters});", codeStyle, GUILayout.Width(codeWidth));
            EditorGUILayout.LabelField("}", GUILayout.Width(codeWidth));
            EditorGUILayout.Space(5);

            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Copy to Clipboard", GUILayout.Width(150)))
            {
                var code = $"tween.{animationString}({parameters});";

                EditorGUIUtility.systemCopyBuffer = code;
                Debug.Log("Code copied to clipboard!");
            }

            EditorGUILayout.EndVertical();
        }
    }
}