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
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlowKit.Editor
{
    public class FlowKitWindow : EditorWindow
    {
        #region Variables
        private static FlowKitWindow window;

        private readonly Dictionary<GameObject, List<GameObject>> _parentChildMap = new Dictionary<GameObject, List<GameObject>>();
        private readonly Dictionary<GameObject, Vector2> _positionMap = new Dictionary<GameObject, Vector2>();
        private readonly Dictionary<GameObject, Quaternion> _rotationMap = new Dictionary<GameObject, Quaternion>();
        private readonly Dictionary<GameObject, Vector2> _scaleMap = new Dictionary<GameObject, Vector2>();
        private readonly Dictionary<GameObject, float> _alphaMap = new Dictionary<GameObject, float>();

        private readonly GUIStyle _instructionStyle = new GUIStyle();
        private const int animationControlSpacing = 5;
        private const int parentChildGroupSpacing = 3;

        private bool isAnimating;
        private double startTime;
        private double lastFrameTime;
        private GameObject selectedObject;

        private AnimationDefs.FadeVars fade;
        private AnimationDefs.TransitionVars transition;
        private AnimationDefs.ScaleVars scale;
        private AnimationDefs.RotateVars rotate;
        private AnimationDefs.AnimationType animationType = AnimationDefs.AnimationType.None;
        #endregion

        [MenuItem("Window/FlowKit/Live Preview")]
        public static void ShowWindow()
        {
            window = GetWindow<FlowKitWindow>("Live Preview");
            window.minSize = new Vector2(500, 500);
        }

        void OnEnable()
        {
            InitData();
            InitParentsInMap();
            InitStyles();
        }

        private void OnDisable()
        {
            StopPreview();
            ResetAllTransforms();
            ClearSavedData();
        }

        private void OnGUI()
        {
            LabelInstructions();
            
            DrawParents();
            DrawAnimationSelection();
            DrawSelectedControls();

            GetSelectedObject();
        }

        private void EditorUpdateRotate()
        {
            if (selectedObject == null || !isAnimating) { return; }

            double currentTime = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(currentTime - lastFrameTime);
            lastFrameTime = currentTime;
            float time = (float)(currentTime - startTime) / rotate.duration;

            if ((float)(currentTime - startTime) >= rotate.duration) 
            { 
                isAnimating = false;
                selectedObject.transform.localRotation = rotate.targetRotation;
                SceneView.RepaintAll();
                return; 
            }

            selectedObject.transform.localRotation = Quaternion.Lerp(selectedObject.transform.localRotation, rotate.targetRotation, time * deltaTime);

            SceneView.RepaintAll();
        }

        private void EditorUpdateTransition()
        {
            if (selectedObject == null || !isAnimating) { return; }

            double currentTime = EditorApplication.timeSinceStartup;
            lastFrameTime = currentTime;
            float time = (float)(currentTime - startTime) / transition.duration;

            if ((float)(currentTime - startTime) >= transition.duration)
            {
                isAnimating = false;
                selectedObject.transform.localPosition = transition.targetPos;
                SceneView.RepaintAll();
                return;
            }

            Vector3 newPos = Vector3.Lerp(transition.startPos, transition.targetPos, time);
            selectedObject.transform.localPosition = newPos;

            SceneView.RepaintAll();
        }

        private void EditorUpdateScale()
        {
            if (selectedObject == null || !isAnimating) { return; }

            double currentTime = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(currentTime - lastFrameTime);
            lastFrameTime = currentTime;
            float time = (float)(currentTime - startTime) / scale.duration;

            if ((float)(currentTime - startTime) >= scale.duration)
            {
                isAnimating = false;
                selectedObject.transform.localScale = scale.targetScale;
                SceneView.RepaintAll();
                return;
            }

            selectedObject.transform.localScale = Vector2.Lerp(selectedObject.transform.localScale, scale.targetScale, time * deltaTime);

            SceneView.RepaintAll();
        }

        private void EditorUpdateAlpha()
        {
            if (selectedObject == null || !isAnimating) { return; }

            double currentTime = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(currentTime - lastFrameTime);
            lastFrameTime = currentTime;
            float time = (float)(currentTime - startTime) / fade.duration;

            if ((float)(currentTime - startTime) >= fade.duration)
            {
                isAnimating = false;
                HandleAlphaReset();
                Canvas.ForceUpdateCanvases();
                SceneView.RepaintAll();
                return;
            }

            HandleAlphaLerp(time * deltaTime);

            Canvas.ForceUpdateCanvases();
            SceneView.RepaintAll();
        }

        // -------------------------------------------------------------- INITIALIZERS --------------------------------------------------------------

        private void InitData()
        {
            fade = new AnimationDefs.FadeVars();
            transition = new AnimationDefs.TransitionVars();
            scale = new AnimationDefs.ScaleVars();
            rotate = new AnimationDefs.RotateVars();
        }

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
                        SaveTransform(obj);
                        InitChildrenInMap(obj);
                    }
                }
            }
        }

        private void InitChildrenInMap(GameObject parent)
        {
            List<GameObject> children = new List<GameObject>();

            foreach (Transform childTransform in parent.transform)
            {
                GameObject child = childTransform.gameObject;

                if (child.layer == LayerMask.NameToLayer("UI"))
                {
                    if (child.GetComponent<RectTransform>())
                    {
                        children.Add(child);
                        SaveTransform(child);
                    }
                }
            }

            _parentChildMap[parent] = children;
        }

        private void InitStyles()
        {
            _instructionStyle.fontSize = 14;
            _instructionStyle.normal.textColor = Color.white;
            _instructionStyle.alignment = TextAnchor.MiddleCenter;
        }

        // -------------------------------------------------------------- DRAW GUI --------------------------------------------------------------

        private void LabelInstructions()
        {
            string[] instructions =
            {
                "To animate a component, double-click it in this window",
                "or select it from the Hierarchy panel.",
                "(Ensure the object is active and a child of a panel.)"
            };

            foreach (string line in instructions)
            {
                GUILayout.Label(line, _instructionStyle);
            }
        }

        private void DrawParents()
        {
            EditorGUILayout.BeginVertical();

            foreach (GameObject obj in _parentChildMap.Keys)
            {
                EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                DrawChildren(obj);
                EditorGUILayout.Space(parentChildGroupSpacing);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawChildren(GameObject parent)
        {
            EditorGUILayout.BeginHorizontal();

            foreach (GameObject child in _parentChildMap[parent])
            {
                EditorGUILayout.ObjectField(child, typeof(GameObject), true);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawAnimationSelection()
        {
            EditorGUILayout.BeginVertical();

            animationType = (AnimationDefs.AnimationType)EditorGUILayout.EnumPopup("Animation Type: ", animationType);

            EditorGUILayout.EndVertical();
        }

        private void DrawSelectedControls()
        {
            EditorGUILayout.Space(animationControlSpacing);

            switch (animationType)
            {
                case AnimationDefs.AnimationType.Rotate:
                    rotate.degrees = EditorGUILayout.FloatField("Degrees", rotate.degrees);
                    rotate.duration = EditorGUILayout.FloatField("Duration", rotate.duration);
                    break;
                case AnimationDefs.AnimationType.Transition:
                    transition.direction = (AnimationDefs.TransitionDirection)EditorGUILayout.EnumPopup("Direction", transition.direction);

                    if (transition.direction == AnimationDefs.TransitionDirection.FromPosition || transition.direction == AnimationDefs.TransitionDirection.ToPosition)
                    {
                        transition.offset.vectorOffset = EditorGUILayout.Vector2Field("Offset", transition.offset.vectorOffset);
                    }
                    else
                    {
                        transition.offset.floatOffset = EditorGUILayout.FloatField("Offset", transition.offset.floatOffset);
                    }

                    transition.duration = EditorGUILayout.FloatField("Duration", transition.duration);
                    break;
                case AnimationDefs.AnimationType.Scale:
                    scale.multiplier = EditorGUILayout.FloatField("Multiplier", scale.multiplier);
                    scale.duration = EditorGUILayout.FloatField("Duration", scale.duration);
                    break;
                case AnimationDefs.AnimationType.Fade:
                    fade.alphaTarget = EditorGUILayout.IntField("Alpha target", fade.alphaTarget);
                    fade.duration = EditorGUILayout.FloatField("Duration", fade.duration);
                    break;
            }

            if (GUILayout.Button(isAnimating ? "Stop Animation" : "Start Animation"))
            {
                isAnimating = !isAnimating;

                if (isAnimating)
                {
                    SubscribeToUpdate(animationType);
                }
                else
                {
                    StopPreview();
                }
            }
        }

        // -------------------------------------------------------------- Internal functionality --------------------------------------------------------------

        private void StopPreview()
        {
            isAnimating = false;
            selectedObject = null;

            EditorApplication.update -= EditorUpdateRotate;
            EditorApplication.update -= EditorUpdateTransition;
            EditorApplication.update -= EditorUpdateScale;
            EditorApplication.update -= EditorUpdateAlpha;
        }

        private void GetSelectedObject()
        {
            selectedObject = Selection.activeObject as GameObject;

            if (selectedObject == null || !selectedObject.activeInHierarchy)
            {
                selectedObject = null;
                return;
            }
            if (selectedObject.layer != LayerMask.NameToLayer("UI"))
            {
                selectedObject = null;
                return;
            }
            if (selectedObject.GetComponent<RectTransform>() == null)
            {
                selectedObject = null;
                return;
            }
        }

        private void SubscribeToUpdate(AnimationDefs.AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationDefs.AnimationType.Rotate:
                    EditorApplication.update -= EditorUpdateTransition;
                    EditorApplication.update -= EditorUpdateScale;
                    EditorApplication.update -= EditorUpdateAlpha;

                    EditorApplication.update += EditorUpdateRotate;
                    startTime = EditorApplication.timeSinceStartup;
                    lastFrameTime = startTime;
                    rotate.targetRotation = selectedObject.transform.localRotation * Quaternion.Euler(0, 0, -rotate.degrees);
                    break;
                case AnimationDefs.AnimationType.Transition:
                    EditorApplication.update -= EditorUpdateRotate;
                    EditorApplication.update -= EditorUpdateScale;
                    EditorApplication.update -= EditorUpdateAlpha;

                    EditorApplication.update += EditorUpdateTransition;
                    startTime = EditorApplication.timeSinceStartup;
                    lastFrameTime = startTime;
                    HandleTransitionPosSet();
                break;
                case AnimationDefs.AnimationType.Scale:
                    EditorApplication.update -= EditorUpdateRotate;
                    EditorApplication.update -= EditorUpdateTransition;
                    EditorApplication.update -= EditorUpdateAlpha;

                    EditorApplication.update += EditorUpdateScale;
                    startTime = EditorApplication.timeSinceStartup;
                    lastFrameTime = startTime;
                    scale.targetScale = selectedObject.transform.localScale * scale.multiplier;
                break;
                case AnimationDefs.AnimationType.Fade:
                    EditorApplication.update -= EditorUpdateRotate;
                    EditorApplication.update -= EditorUpdateTransition;
                    EditorApplication.update -= EditorUpdateScale;

                    EditorApplication.update += EditorUpdateAlpha;
                    startTime = EditorApplication.timeSinceStartup;
                    lastFrameTime = startTime;
                break;
            }
        }

        // -------------------------------------------------------------- PRIVATE HANDLERS --------------------------------------------------------------

        private void HandleTransitionPosSet()
        {
            if (transition.direction.ToString().StartsWith("From"))
            {
                if (transition.direction == AnimationDefs.TransitionDirection.FromPosition)
                {
                    transition.targetPos = selectedObject.transform.localPosition;
                    transition.startPos = selectedObject.transform.localPosition - (Vector3)transition.offset.vectorOffset;
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.FromTop)
                {
                    transition.targetPos = selectedObject.transform.localPosition;
                    transition.startPos = selectedObject.transform.localPosition + new Vector3(0, transition.offset.floatOffset, 0);
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.FromBottom)
                {
                    transition.targetPos = selectedObject.transform.localPosition;
                    transition.startPos = selectedObject.transform.localPosition - new Vector3(0, transition.offset.floatOffset, 0);
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.FromLeft)
                {
                    transition.targetPos = selectedObject.transform.localPosition;
                    transition.startPos = selectedObject.transform.localPosition - new Vector3(transition.offset.floatOffset, 0, 0);
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.FromRight)
                {
                    transition.targetPos = selectedObject.transform.localPosition;
                    transition.startPos = selectedObject.transform.localPosition + new Vector3(transition.offset.floatOffset, 0, 0);
                }
            }
            else
            {
                if (transition.direction == AnimationDefs.TransitionDirection.ToPosition)
                {
                    transition.startPos = selectedObject.transform.localPosition;
                    transition.targetPos = selectedObject.transform.localPosition + (Vector3)transition.offset.vectorOffset;
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.ToTop)
                {
                    transition.startPos = selectedObject.transform.localPosition;
                    transition.targetPos = selectedObject.transform.localPosition + new Vector3(0, transition.offset.floatOffset, 0);
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.ToBottom)
                {
                    transition.startPos = selectedObject.transform.localPosition;
                    transition.targetPos = selectedObject.transform.localPosition - new Vector3(0, transition.offset.floatOffset, 0);
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.ToLeft)
                {
                    transition.startPos = selectedObject.transform.localPosition;
                    transition.targetPos = selectedObject.transform.localPosition - new Vector3(transition.offset.floatOffset, 0, 0);
                }
                else if (transition.direction == AnimationDefs.TransitionDirection.ToRight)
                {
                    transition.startPos = selectedObject.transform.localPosition;
                    transition.targetPos = selectedObject.transform.localPosition + new Vector3(transition.offset.floatOffset, 0, 0);
                }
            }
        }

        private void HandleAlphaLerp(float time)
        {
            if (selectedObject.TryGetComponent<Button>(out Button btn) && btn.image != null)
            {
                Color btnColor = btn.image.color;
                btnColor.a = Mathf.Lerp(btn.image.color.a, fade.alphaTarget, time);
                btn.image.color = btnColor;
            }
            else if (selectedObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                text.alpha = Mathf.Lerp(text.alpha, fade.alphaTarget, time);
            }
            else if (selectedObject.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
            {
                cg.alpha = Mathf.Lerp(cg.alpha, fade.alphaTarget, time);
            }
            else if (selectedObject.TryGetComponent<Image>(out Image img))
            {
                Color imgColor = img.color;
                imgColor.a = Mathf.Lerp(img.color.a, fade.alphaTarget, time);
                img.color = imgColor;
            }
        }

        private void HandleAlphaReset()
        {
            if (selectedObject.TryGetComponent<Button>(out Button btn) && btn.image != null)
            {
                Color btnColor = btn.image.color;
                btnColor.a = fade.alphaTarget;
                btn.image.color = btnColor;
            }
            else if (selectedObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                text.alpha = fade.alphaTarget;
            }
            else if (selectedObject.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
            {
                cg.alpha = fade.alphaTarget;
            }
            else if (selectedObject.TryGetComponent<Image>(out Image img))
            {
                Color imgColor = img.color;
                imgColor.a = fade.alphaTarget;
                img.color = imgColor;
            }
        }

        // -------------------------------------------------------------- TRANSFORM SETTERS --------------------------------------------------------------
        
        #region Transform Methods
        private void SaveTransform(GameObject obj)
        {
            SavePosition(obj);
            SaveRotation(obj);
            SaveScale(obj);
            SaveAlpha(obj);
        }

        private void SavePosition(GameObject obj)
        {
            _positionMap.Add(obj, obj.transform.localPosition);
        }

        private void SaveRotation(GameObject obj)
        {
            _rotationMap.Add(obj, obj.transform.localRotation);
        }

        private void SaveScale(GameObject obj)
        {
            _scaleMap.Add(obj, obj.transform.localScale);
        }

        private void SaveAlpha(GameObject obj)
        {
            if (obj.TryGetComponent<Button>(out Button btn))
            {
                _alphaMap.Add(obj, btn.image.color.a);
            }
            else if (obj.TryGetComponent<Image>(out Image img))
            {
                _alphaMap.Add(obj, img.color.a);
            }
            else if (obj.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                _alphaMap.Add(obj, text.color.a);
            }
            else if (obj.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
            {
                _alphaMap.Add(obj, cg.alpha);
            }
        }

        // -------------------------------------------------------------- TRANSFORM RESETS --------------------------------------------------------------

        private void ClearSavedData()
        {
            _parentChildMap.Clear();
            _positionMap.Clear();
            _rotationMap.Clear();
            _scaleMap.Clear();
            _alphaMap.Clear();
        }

        private void ResetAllTransforms()
        {
            ResetPosition();
            ResetRotation();
            ResetScale();
            ResetAlpha();
        }

        private void ResetPosition()
        {
            foreach (var kvp in _positionMap)
            {
                GameObject obj = kvp.Key;
                if (obj == null) { continue; }

                Vector2 originalPos = kvp.Value;

                obj.transform.localPosition = originalPos;
            }
        }

        private void ResetRotation()
        {
            foreach (var kvp in _rotationMap)
            {
                GameObject obj = kvp.Key;
                if (obj == null) { continue; }

                Quaternion originalRotation = kvp.Value;

                obj.transform.localRotation = originalRotation;
            }
        }

        private void ResetScale()
        {
            foreach (var kvp in _scaleMap)
            {
                GameObject obj = kvp.Key;
                if (obj == null) { continue; }

                Vector2 originalScale = kvp.Value;

                obj.transform.localScale = originalScale;
            }
        }

        private void ResetAlpha()
        {
            foreach (var kvp in _alphaMap)
            {
                GameObject obj = kvp.Key;
                if (obj == null) { continue; }

                float originalAlpha = kvp.Value;

                if (obj.TryGetComponent<Button>(out Button btn))
                {
                    Color color = btn.image.color;
                    btn.image.color = new Color(color.r, color.g, color.b, originalAlpha);
                }
                else if (obj.TryGetComponent<Image>(out Image img))
                {
                    Color color = img.color;
                    img.color = new Color(color.r, color.g, color.b, originalAlpha);
                }
                else if (obj.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
                {
                    Color color = text.color;
                    text.color = new Color(color.r, color.g, color.b, originalAlpha);
                }
                else if (obj.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
                {
                    cg.alpha = originalAlpha;
                }
            }
        }
    }
    #endregion
}