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
* FlowKit - Transition API Property
* Created by Hollow1
* 
* Provides a single location for all position specific operations.
* 
* Version: 1.2.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using UnityEngine;

using FlowKit.Common;

namespace FlowKit
{
    public partial class FlowKitEngine
    {
        public TransitionModule Transition { get; private set; }

        public class TransitionModule
        {
            private readonly FlowKitEngine _engine;

            public TransitionModule(FlowKitEngine engine) 
            { 
                _engine = engine; 
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
                _engine.transitionImpl.Reset(target, occurrence, _engine.gameObject);
            }

            /// <summary>
            /// Immediately sets the target UI element's position to the specified position | <b>[NOT Animated]</b>
            ///  <list type="bullet">
            ///     <item>
            ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///     </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="position">Specifies the position where the element will appear on it's current panel (Based on anchor preset)</param>
            public void SetPosition(AnimationTarget target, int occurrence, Vector2 position)
            {
                occurrence -= 1;
                _engine.transitionImpl.SetPosition(target, occurrence, position);
            }

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
            public void FromTop(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionFromTop(target, occurrence, offset, easing, duration, delay);
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
            public void FromBottom(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionFromBottom(target, occurrence, offset, easing, duration, delay);
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
            public void FromLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionFromLeft(target, occurrence, offset, easing, duration, delay);
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
            public void FromRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionFromRight(target, occurrence, offset, easing, duration, delay);
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
            public void FromPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionFromPosition(target, occurrence, offset, easing, duration, delay);
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
            public void ToTop(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionToTop(target, occurrence, offset, easing, duration, delay);
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
            public void ToBottom(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionToBottom(target, occurrence, offset, easing, duration, delay);
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
            public void ToLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionToLeft(target, occurrence, offset, easing, duration, delay);
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
            public void ToRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionToRight(target, occurrence, offset, easing, duration, delay);
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
            public void ToPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.transitionImpl.TransitionToPosition(target, occurrence, offset, easing, duration, delay);
            }
        }
    }
}
