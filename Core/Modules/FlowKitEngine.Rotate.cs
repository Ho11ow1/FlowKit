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
* FlowKit - Rotation API Property
* Created by Hollow1
* 
* Provides a single location for all specific operations.
* 
* Version: 1.2.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using FlowKit.Common;

namespace FlowKit.Core
{
    public partial class FlowKitEngine
    {
        public RotateModule Rotate { get; private set; }

        public class RotateModule
        {
            private readonly FlowKitEngine _engine;

            public RotateModule(FlowKitEngine engine) 
            { 
                _engine = engine; 
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
                _engine.rotateImpl.Reset(target, occurrence, _engine.gameObject);
            }

            /// <summary>
            /// Immediately sets the rotation of the target UI element to the specified degrees | <b>[NOT Animated]</b>
            ///  <list type="bullet">
            ///     <item>
            ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///     </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to rotate (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="degrees">Degrees the rotation should rotate, positive values go counter-clockwise, negative clockwise</param>
            public void SetRotation(AnimationTarget target, int occurrence, float degrees)
            {
                occurrence -= 1;
                _engine.rotateImpl.SetRotation(target, occurrence, degrees);
            }

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
                _engine.rotateImpl.Rotation(target, occurrence, degrees, easing, duration, delay);
            }
        }
    }
}