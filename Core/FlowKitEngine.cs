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
        private UI.TransitionImpl transitionImpl;
        private UI.RotateImpl rotateImpl;
        private UI.ScaleImpl scalingImpl;
        private UI.TextEffectImpl textEffectImpl;

        // Component variables
        private CanvasGroup _cg;
        private RectTransform _panel;
        private TextMeshProUGUI[] _texts;
        private Image[] _images;
        private Button[] _buttons;

        void Awake()
        {
            InitVariables();
            InitComponents();
            InitModules();
        }

        // ----------------------------------------------------- PRIVATE INITALIZERS -----------------------------------------------------

        private void InitVariables()
        {
            _cg = GetComponent<CanvasGroup>();

            _texts = GetComponentsInChildren<TextMeshProUGUI>();

            var imageList = new List<Image>();
            foreach (RectTransform child in transform)
            {
                if (child.TryGetComponent<Image>(out var img))
                {
                    imageList.Add(img);
                }
            }
            _images = imageList.ToArray();

            _buttons = GetComponentsInChildren<Button>();

            _panel = GetComponent<RectTransform>();

            #if UNITY_EDITOR
            if (_cg == null) { Debug.LogWarning($"[{gameObject.name}] No CanvasGroup component found."); }
            if (_texts == null) { Debug.LogWarning($"[{gameObject.name}] No Text component found in children. Parent: [{transform.parent.name ?? "none"}]"); }
            if (_images == null) { Debug.LogWarning($"[{gameObject.name}] No Image component found in children. Parent: [{transform.parent.name ?? "none"}]"); }
            if (_buttons == null) { Debug.LogWarning($"[{gameObject.name}] No Button component found in children. Parent: [{transform.parent.name ?? "none"}]"); }
            if (_panel == null) { Debug.LogWarning($"[{gameObject.name}] No RectTransform component found."); }
            #endif
        }

        private void InitComponents()
        {
            queueComponent = new Core.FlowKitQueue(this);
            visibilityImpl = new UI.VisibilityImpl(_texts, _images, _buttons, _cg, this);
            transitionImpl = new UI.TransitionImpl(_texts, _images, _buttons, _panel, this);
            rotateImpl = new UI.RotateImpl(_texts, _images, _buttons, _panel, this);
            scalingImpl = new UI.ScaleImpl(_texts, _images, _buttons, _panel, this);
            textEffectImpl = new UI.TextEffectImpl(_texts, this);
        }

        private void InitModules()
        {
            Visibility = new VisibilityModule(this);
            Transition = new TransitionModule(this);
            Scale = new ScaleModule(this);
            Rotate = new RotateModule(this);
            Text = new TextModule(this);
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
