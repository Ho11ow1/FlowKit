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
* Version: 1.0.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using FlowKit.UI;
using FlowKit.Common;

namespace FlowKit.Core
{
    public class Tween : MonoBehaviour
    {
        // Class variables
        private Fade fadeComponent;
        private Transition transitionComponent;
        private Rotate rotationComponent;
        private Scale scalingComponent;
        private TypeWrite typeWriterComponent;

        // Component variables
        private CanvasGroup _cg;
        private RectTransform _panel;
        private TextMeshProUGUI[] _texts;
        private Image[] _images;
        private Button[] _buttons;

        void Awake()
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

            fadeComponent = new Fade(_texts, _images, _buttons, _cg, this);
            transitionComponent = new Transition(_texts, _images, _buttons, _panel, this);
            rotationComponent = new Rotate(_texts, _images, _buttons, _panel, this);
            scalingComponent = new Scale(_texts, _images, _buttons, _panel, this);
            typeWriterComponent = new TypeWrite(_texts, this);
        }

        // ----------------------------------------------------- Fade API -----------------------------------------------------

        /// <summary>
        /// Immediately sets the UI panel visibility
        /// </summary>
        /// <param name="visible">Sets the panel visibility condition</param>
        public void SetPanelVisibility(bool visible)
        {
            fadeComponent.SetPanelVisibility(visible);
        }

        /// <summary>
        /// Fades in the UI element
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="duration">Time in seconds for the fading duration</param>
        /// <param name="delay">Time in seconds to wait before starting the fade</param>
        public void FadeIn(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            fadeComponent.FadeIn(target, occurrence, duration, delay);
        }

        /// <summary>
        /// Fades out the UI element
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="duration">Time in seconds for the fading duration</param>
        /// <param name="delay">Time in seconds to wait before starting the fade</param>
        public void FadeOut(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            fadeComponent.FadeOut(target, occurrence, duration, delay);
        }

        // ----------------------------------------------------- Transition API -----------------------------------------------------

        /// <summary>
        /// Transitions the UI element from a position offset upward back to its starting position
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element up</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionFromTop(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionFromTop(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from a position offset downward back to its starting position
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element down</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionFromBottom(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionFromBottom(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from a position offset to the left back to its starting position
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element to the left</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionFromLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionFromLeft(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from a position offset to the right back to its starting position
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element to the right</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionFromRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionFromRight(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from an offset position on both axes back to its starting position
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). determines the starting offset position to animate from, Positive values offset right and up</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionFromPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionFromPosition(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from its starting position to a position offset upward
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element up</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionToTop(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionToTop(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from its starting position to a position offset downward
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element down</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionToBottom(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionToBottom(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from its starting position to a position offset to the left
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element to the left</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionToLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionToLeft(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from its starting position to a position offset to the right
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). Positive values move the element to the right</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionToRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionToRight(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Transitions the UI element from its starting position to an offset position
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Animation always starts from the element's current position</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="offset">Offset in pixels (or units depending on canvas scaling and render mode). determines the final offset position to animate to, Positive values offset right and up</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the transition duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void TransitionToPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            transitionComponent.TransitionToPosition(target, occurrence, offset, easing, duration, delay);
        }

        /// <summary>
        /// Reverts the target UI elements' position back to it's original position | <b>[NOT Animated]</b>
        ///  <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        public void ResetPosition(AnimationTarget target, int occurrence)
        {
            occurrence -= 1;
            transitionComponent.Reset(target, occurrence, gameObject);
        }

        // ----------------------------------------------------- Rotation API -----------------------------------------------------

        /// <summary>
        /// Rotates the target UI element.
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Subsequent calls will start animationg from the current rotation</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to rotate (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="degrees">Degrees the rotation should rotate, positive values go counter-clockwise, negative clockwise</param>
        /// <param name="easing">Specifies the easing method the transition should use</param>
        /// <param name="duration">Time in seconds for the rotation duration</param>
        /// <param name="delay">Time in seconds to wait before starting the transition</param>
        public void Rotate(AnimationTarget target, int occurrence, float degrees, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            rotationComponent.Rotation(target, occurrence, degrees, easing, duration, delay);
        }

        /// <summary>
        /// Reverts the target UI elements' rotation back to it's original rotation | <b>[NOT Animated]</b>
        ///  <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to rotate (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        public void ResetRotation(AnimationTarget target, int occurrence)
        {
            occurrence -= 1;
            rotationComponent.Reset(target, occurrence, gameObject);
        }

        // ----------------------------------------------------- Scaling API -----------------------------------------------------

        /// <summary>
        /// Scales up the target UI element.
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Subsequant calls will start animating from current scale</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="multiplier">Scale multiplier. Values greater than 1 increase size, must be greater than 0. (Scale is based on the local scale of the parent)</param>
        /// <param name="easing">Specifies the easing method the scaling should use</param>
        /// <param name="duration">Time in seconds the scaling animation should take</param>
        /// <param name="delay">Time in seconds to wait before starting the scaling</param>
        public void ScaleUp(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            scalingComponent.ScaleUp(target, occurrence, multiplier, easing, duration, delay);
        }

        /// <summary>
        /// Scales down the target UI element.
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: Subsequant calls will start animating from current scale</description>
        ///     </item>
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="multiplier">Scale multiplier. Values greater than 1 decrease size, must be greater than 0. (Scale is based on the local scale of the parent)</param>
        /// <param name="easing">Specifies the easing method the scaling should use</param>
        /// <param name="duration">Time in seconds the scaling animation should take</param>
        /// <param name="delay">Time in seconds to wait before starting the scaling</param>
        public void ScaleDown(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            occurrence -= 1;
            scalingComponent.ScaleDown(target, occurrence, multiplier, easing, duration, delay);
        }

        /// <summary>
        /// Reverts the target UI elements' scale back to it's original scale | <b>[NOT Animated]</b>
        ///  <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        public void ResetScale(AnimationTarget target, int occurrence)
        {
            occurrence -= 1;
            scalingComponent.Reset(target, occurrence, gameObject);
        }

        // ----------------------------------------------------- TypeWriter API -----------------------------------------------------

        /// <summary>
        /// Applies a typeWriter effect to the TextMeshPro component.
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="delay">Time in seconds for the delay per character</param>
        public void TypeWriteWithDelay(int occurrence, float delay = 0.3f)
        {
            occurrence -= 1;
            typeWriterComponent.TypeWriterDelay(occurrence, delay);
        }

        /// <summary>
        /// Applies a typeWriter effect to the TextMeshPro component.
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
        /// <param name="duration">Time in seconds for the entire text to animate</param>
        public void TypeWriteWithDuration(int occurrence, float duration = 3f)
        {
            occurrence -= 1;
            typeWriterComponent.TypeWriterDuration(occurrence, duration);
        }
    }
}