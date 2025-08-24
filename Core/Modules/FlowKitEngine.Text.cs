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
* FlowKit - Text API Property
* Created by Hollow1
* 
* Provides a single location for all text specific operations.
* 
* Version: 1.3.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using UnityEngine;

using FlowKit.Common;

namespace FlowKit
{
    public partial class FlowKitEngine
    {
        public TextModule Text { get; private set; }

        public class TextModule
        {
            private readonly FlowKitEngine _engine;

            public TextModule(FlowKitEngine engine) 
            { 
                _engine = engine; 
            }

            /// <summary>
            /// Applies a typeWriter effect to the TextMeshPro component.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="delay">Time in seconds for the Delay per character</param>
            public void TypeWriteWithDelay(int occurrence, float delay = FlowKitConstants.TypeWriter.PerCharacterDelay)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.textEffectImpl.DelayTypeWrite(occurrence, delay);
            }

            /// <summary>
            /// Applies a typeWriter effect to the TextMeshPro component.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="duration">Time in seconds for the entire text to animate</param>
            public void TypeWriteWithDuration(int occurrence, float duration = FlowKitConstants.TypeWriter.CompleteTextDuration)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.textEffectImpl.DurationTypeWrite(occurrence, duration);
            }

            /// <summary>
            /// Applies a color cycling effect to the TextMeshPro component.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            ///   <item>
            ///     <description><b>Note</b>: To create an infinitly looping effect specify <c>duration</c> as 0f</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="duration">Time in seconds the color cycling should last (0f for infinite)</param>
            /// <param name="delay">Delay between each color change</param>
            /// <param name="color">Specifies the color32 to cycle to and from</param>
            public void ColorCycle(int occurrence, float duration, float delay, Color32 color)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.textEffectImpl.ColorCycleTwo(occurrence, duration, delay, color);
            }

            /// <summary>
            /// Applies a color cycling effect to the TextMeshPro component.
            /// <list type="bullet">
            ///   <item>
            ///     <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///   </item>
            ///   <item>
            ///     <description><b>Note</b>: To create an infinitly looping effect specify <c>duration</c> as 0f</description>
            ///   </item>
            /// </list>
            /// </summary>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="duration">Time in seconds the color cycling should last (0f for infinite)</param>
            /// <param name="delay">Delay between each color change</param>
            /// <param name="colors">Specifies the color32's the text should cycle through</param>
            public void ColorCycle(int occurrence, float duration, float delay, Color32[] colors)
            {
                if (!IsOccurrenceValid(occurrence)) { return; }

                occurrence -= 1;
                _engine.textEffectImpl.ColorCycleMulti(occurrence, duration, delay, colors);
            }

        }
    }
}
