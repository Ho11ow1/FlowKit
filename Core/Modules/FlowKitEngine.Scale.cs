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
* FlowKit - Scaling API Property
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
        public ScaleModule Scale { get; private set; }

        public class ScaleModule
        {
            private readonly FlowKitEngine _engine;

            public ScaleModule(FlowKitEngine engine) 
            { 
                _engine = engine; 
            }

            /// <summary>
            /// Reverts the target UI elements' scale back to it's original scale | <b>[NOT Animated]</b>
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            public void ResetScale(AnimationTarget target, int occurrence)
            {
                occurrence -= 1;
                _engine.scalingImpl.Reset(target, occurrence, _engine.gameObject);
            }

            /// <summary>
            /// Sets the scale of a target UI element immediately | <b>[NOT Animated]</b>
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="scale">Specifies the target scale of the element</param>
            public void SetScale(AnimationTarget target, int occurrence, float scale)
            {
                occurrence -= 1;
                _engine.scalingImpl.SetScale(target, occurrence, scale);
            }

            /// <summary>
            /// Scales up the target UI element.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="multiplier">Scale multiplier. Values greater than 1 increase size, must be greater than 0. (Scale is based on the local scale of the parent)</param>
            /// <param name="duration">Time in seconds the scaling animation should take</param>
            /// <param name="easing">Specifies the easing method the scaling should use</param>
            /// <param name="delay">Time in seconds to wait before starting the scaling</param>
            public void ScaleUp(AnimationTarget target, int occurrence, float multiplier, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.scalingImpl.ScaleFromTo(target, occurrence, null, multiplier, duration, easing, delay);
            }

            /// <summary>
            /// Scales down the target UI element.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="multiplier">Scale multiplier. Values greater than 1 decrease size, must be greater than 0. (Scale is based on the local scale of the parent)</param>
            /// <param name="duration">Time in seconds the scaling animation should take</param>
            /// <param name="easing">Specifies the easing method the scaling should use</param>
            /// <param name="delay">Time in seconds to wait before starting the scaling</param>
            public void ScaleDown(AnimationTarget target, int occurrence, float multiplier, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.scalingImpl.ScaleFromTo(target, occurrence, null, 1 / multiplier, duration, easing, delay);
            }

            /// <summary>
            /// Scales the target UI element from a specified scale to it's original scale
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="fromScale">Specifies the starting scale of the element</param>
            /// <param name="duration">Time in seconds the scaling animation should take</param>
            /// <param name="easing">Specifies the easing method the scaling should use</param>
            /// <param name="delay">Time in seconds to wait before starting the scaling</param>
            public void ScaleFrom(AnimationTarget target, int occurrence, float fromScale, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.scalingImpl.ScaleFromTo(target, occurrence, fromScale, null, duration, easing, delay);
            }

            /// <summary>
            /// Scales the target UI element from one scale to another
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to scale (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="fromScale">Specifies the starting scale of the element</param>
            /// <param name="toScale">Specifies the ending scale of the element</param>
            /// <param name="duration">Time in seconds the scaling animation should take</param>
            /// <param name="easing">Specifies the easing method the scaling should use</param>
            /// <param name="delay">Time in seconds to wait before starting the scaling</param>
            public void ScaleFromTo(AnimationTarget target, int occurrence, float fromScale, float toScale, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.scalingImpl.ScaleFromTo(target, occurrence, fromScale, toScale, duration, easing, delay);
            }
        }
    }
}
