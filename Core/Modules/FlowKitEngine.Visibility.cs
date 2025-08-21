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
* FlowKit - TexVisibility API Property
* Created by Hollow1
* 
* Provides a single location for all visibility specific operations.
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
        public VisibilityModule Visibility { get; private set; }

        public class VisibilityModule
        {
            private readonly FlowKitEngine _engine;

            public VisibilityModule(FlowKitEngine engine) 
            { 
                _engine = engine;
            }

            /// <summary>
            /// Reverts the target UI elements' alpha value back to it's original value | <b>[NOT Animated]</b>
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to transition (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            public void ResetAlpha(AnimationTarget target, int occurrence)
            {
                occurrence -= 1;
                _engine.visibilityImpl.Reset(target, occurrence, _engine.gameObject);
            }

            /// <summary>
            /// Immediately sets the UI panel visibility condition | <b>[NOT Animated]</b>
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="visible">Sets the panel visibility condition</param>
            public void SetPanelVisibility(bool isVisible)
            {
                _engine.visibilityImpl.SetVisibility(AnimationTarget.Panel, 0, isVisible);
            }

            /// <summary>
            /// Immediately sets the visibility condition of a target UI element | <b>[NOT Animated]</b>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="isVisible">Specifies if the target element should be visible</param>
            public void SetVisibility(AnimationTarget target, int occurrence, bool isVisible)
            {
                occurrence -= 1;
                _engine.visibilityImpl.SetVisibility(target, occurrence, isVisible);
            }

            /// <summary>
            /// Fades in the UI element from transparent to fully opaque
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="easing">Specifies the easing method the fade should use</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeFrom0To1(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeFromTo(target, occurrence, FlowKitConstants.TransparentAlpha, FlowKitConstants.OpaqueAlpha, duration, easing, delay);
            }

            /// <summary>
            /// Fades out the UI element from fully opaque to transparent
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="easing">Specifies the easing method the fade should use</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeFrom1To0(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeFromTo(target, occurrence, FlowKitConstants.OpaqueAlpha, FlowKitConstants.TransparentAlpha, duration, easing, delay);
            }

            /// <summary>
            /// Fades the target UI element to a specified alpha value between 0-1
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="toAlpha">Specifies the target alpha value (Range between 0-1)</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="easing">Specifies the easing method the fade should use</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeTo(AnimationTarget target, int occurrence, float toAlpha, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeFromTo(target, occurrence, null, toAlpha, duration, easing, delay);
            }

            /// <summary>
            /// Fades the target UI element from a specified alpha value to it's original alpha value
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="fromAlpha">Specifies the starting alpha value (Range between 0-1)</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="easing">Specifies the easing method the fade should use</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeFrom(AnimationTarget target, int occurrence, float fromAlpha, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeFromTo(target, occurrence, fromAlpha, null, duration, easing, delay);
            }

            /// <summary>
            /// Fades the target UI element from one alpha value to another
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="fromAlpha">Specifies the starting alpha value (Range between 0-1)</param>
            /// <param name="toAlpha">Specifies the ending alpha value (Range between 0-1)</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="easing">Specifies the easing method the fade should use</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeFromTo(AnimationTarget target, int occurrence, float fromAlpha, float toAlpha, float duration = FlowKitConstants.DefaultDuration, EasingType easing = EasingType.Linear, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeFromTo(target, occurrence, fromAlpha, toAlpha, duration, easing, delay);
            }
        }
    }
}
