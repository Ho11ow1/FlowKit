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

namespace FlowKit
{
    public partial class FlowKitEngine
    {
        public RotateModule Rotation { get; private set; }

        public class RotateModule
        {
            private readonly FlowKitEngine _engine;

            public RotateModule(FlowKitEngine engine) 
            { 
                _engine = engine; 
            }

            /// <summary>
            /// Reverts the target UI elements' rotation back to it's original rotation | <b>[NOT Animated]</b>
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to rotate (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            public void ResetRotation(AnimationTarget target, int occurrence)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.rotateImpl.Reset(target, occurrence, _engine.gameObject);
            }

            /// <summary>
            /// Immediately sets the rotation of the target UI element to the specified degrees | <b>[NOT Animated]</b>
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///    </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to rotate (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="degrees">Degrees the rotation should rotate, positive values go counter-clockwise, negative clockwise</param>
            public void SetRotation(AnimationTarget target, int occurrence, float degrees)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.rotateImpl.SetRotation(target, occurrence, degrees);
            }

            /// <summary>
            /// Rotates the target UI element.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to rotate (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="degrees">Degrees the rotation should rotate, positive values go counter-clockwise, negative clockwise</param>
            /// <param name="duration">Time in seconds for the rotation duration</param>
            /// <param name="easing">Specifies the easing method the transition should use</param>
            /// <param name="delay">Time in seconds to wait before starting the transition</param>
            public void Rotate(AnimationTarget target, int occurrence, float degrees, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.rotateImpl.Rotation(target, occurrence, degrees, duration, easing, delay);
            }

            /// <summary>
            /// Rotates the target UI element forever.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to rotate (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="degreesPerSecond">Degrees the rotation should rotate every second, positive values go counter-clockwise, negative clockwise</param>
            /// <param name="delay">Time in seconds to wait before starting the transition</param>
            public void RotateForever(AnimationTarget target, int occurrence, float degreesPerSecond, float delay = 0f)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.rotateImpl.RotationForever(target, occurrence, degreesPerSecond, delay);
            }
        }
    }
}
