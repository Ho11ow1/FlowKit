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
* FlowKit - Base API Component
* Created by Hollow1
* 
* Provides a unified place for all tweening operations.
* 
* Version: 1.2.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlowKit
{
    public partial class FlowKitEngine : MonoBehaviour
    {
        // Class variables
        private Core.FlowKitQueue queueComponent;
        private UI.VisibilityImpl visibilityImpl;
        private UI.MovementImpl transitionImpl;
        private UI.RotationImpl rotateImpl;
        private UI.ScaleImpl scalingImpl;
        private UI.TextEffectImpl textEffectImpl;

        // Component variables
        private RectTransform panelRect;
        private CanvasGroup canvasGroup;
        private TextMeshProUGUI[] textsArr;
        private Image[] imagesArr;
        private Button[] buttonsArr;
        private TextMeshProUGUI[] buttonTextsArr;

        /// <summary>
        /// Read-only access to the panel's RectTransform.
        /// </summary>
        public RectTransform PanelRect => panelRect;
        /// <summary>
        /// Read-only access to the panel's CanvasGroup.
        /// </summary>
        public CanvasGroup CanvasGroup => canvasGroup;
        /// <summary>
        /// Read-only access to all TextMeshProUGUI components 
        /// found in the panel's direct chidren (not nested deeper).
        /// </summary>
        public IReadOnlyList<TextMeshProUGUI> Texts => textsArr;
        /// <summary>
        /// Read-only access to all Image components 
        /// found in the panel's direct chidren (not nested deeper).
        /// </summary>
        public IReadOnlyList<Image> Images => imagesArr;
        /// <summary>
        /// Read-only access to all Button components 
        /// found in the panel's direct chidren (not nested deeper).
        /// </summary>
        public IReadOnlyList<Button> Buttons => buttonsArr;
        /// <summary>
        /// Read-only access to all Button TextMeshProUGUI children components 
        /// found in the Button's direct chidren (not nested deeper).
        /// </summary>
        public IReadOnlyList<TextMeshProUGUI> ButtonTexts => buttonTextsArr;

        void Awake()
        {
            InitVariables();
            InitComponents();
            InitModules();
        }

        // ----------------------------------------------------- PRIVATE INITALIZERS -----------------------------------------------------

        private void InitVariables()
        {
            panelRect = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            textsArr = GetDirectChildren<TextMeshProUGUI>(child => !child.GetComponent<Button>());
            imagesArr = GetDirectChildren<Image>(child => !child.GetComponent<Button>());
            buttonsArr = GetDirectChildren<Button>();

            List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();
            foreach (var button in buttonsArr)
            {
                var textChild = button.GetComponentInChildren<TextMeshProUGUI>();
                if (textChild != null)
                {
                    buttonTexts.Add(textChild);
                }
            }
            buttonTextsArr = buttonTexts.ToArray();

            #if UNITY_EDITOR
            if (canvasGroup == null) { Debug.LogWarning($"[{gameObject.name}] No canvasGroup component found."); }
            if (textsArr == null) { Debug.LogWarning($"[{gameObject.name}] No Text component found in children. Parent: [{transform.parent.name ?? "none"}]"); }
            if (imagesArr == null) { Debug.LogWarning($"[{gameObject.name}] No Image component found in children. Parent: [{transform.parent.name ?? "none"}]"); }
            if (buttonsArr == null) { Debug.LogWarning($"[{gameObject.name}] No Button component found in children. Parent: [{transform.parent.name ?? "none"}]"); }
            if (panelRect == null) { Debug.LogWarning($"[{gameObject.name}] No RectTransform component found."); }
            #endif
        }

        private void InitComponents()
        {
            queueComponent = new Core.FlowKitQueue(this);
            visibilityImpl = new UI.VisibilityImpl(this, canvasGroup, textsArr, imagesArr, buttonsArr, buttonTextsArr);
            transitionImpl = new UI.MovementImpl(this, panelRect, textsArr, imagesArr, buttonsArr);
            rotateImpl = new UI.RotationImpl(this, panelRect, textsArr, imagesArr, buttonsArr);
            scalingImpl = new UI.ScaleImpl(this, panelRect, textsArr, imagesArr, buttonsArr);
            textEffectImpl = new UI.TextEffectImpl(this, textsArr);
        }

        private void InitModules()
        {
            Visibility = new VisibilityModule(this);
            Movement = new TransitionModule(this);
            Scale = new ScaleModule(this);
            Rotation = new RotateModule(this);
            Text = new TextModule(this);
        }

        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

        // System.Func<Transform, bool> name = null | Optional filter lambda such as (obj => !obj.GetComponent<T>())
        private T[] GetDirectChildren<T>(System.Func<Transform, bool> filter = null) where T : Component
        {
            var list = new List<T>();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<T>(out T component) && child.GetComponent<RectTransform>())
                {
                    // If filter is null or the child passes the filter
                    if (filter == null || filter(child))
                    {
                        list.Add(component);
                    }
                }
            }
            return list.ToArray();
        }

        private static bool IsOccurrenceValid(int occurrence)
        {
            return occurrence >= 1;
        }

        // ----------------------------------------------------- Queue API -----------------------------------------------------

        /// <summary>
        /// Queues a set of animation steps.
        /// </summary>
        /// <param name="steps">Array of <b>AnimationStep</b> methods to be called</param>
        /// <param name="name">Specifies the name of the queue</param>
        /// <param name="autoStart">Specifies if the queue should start on being created</param>
        public void Queue(AnimationStep[] steps, string name, bool autoStart = false)
        {
            queueComponent.Queue(steps, name, autoStart);
        }

        /// <summary>
        /// Starts a designated queue identified by it's name.
        /// </summary>
        /// <param name="name">Specifies the name of the target queue</param>
        public void StartQueue(string name)
        {
            queueComponent.Start(name);
        }

        /// <summary>
        /// Stops a designated queue identified by it's name.
        /// </summary>
        /// <param name="name">Specifies the name of the target queue</param>
        public void StopQueue(string name)
        {
            queueComponent.Stop(name);
        }

        /// <summary>
        /// Pauses a designated queue identified by it's name.
        /// </summary>
        /// <param name="name">Specifies the name of the target queue</param>
        public void PauseQueue(string name)
        {
            queueComponent.Pause(name);
        }

        /// <summary>
        /// Resumes a designated queue identified by it's name.
        /// </summary>
        /// <param name="name">Specifies the name of the target queue</param>
        public void ResumeQueue(string name)
        {
            queueComponent.Resume(name);
        }

    }
}
