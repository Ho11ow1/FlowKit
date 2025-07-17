using FlowKit.Common;
using UnityEngine;

namespace FlowKit.Core
{
    public partial class FlowKitEngine
    {
        public TextModule Text { get; private set; }

        public class TextModule
        {
            private readonly FlowKitEngine _engine;

            public TextModule(FlowKitEngine engine) { _engine = engine; }

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
            public void TypeWriteWithDelay(int occurrence, float delay = FlowKitConstants.TypeWriter.PerCharacterDelay)
            {
                occurrence -= 1;
                _engine.textEffectImpl.DelayTypeWrite(occurrence, delay);
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
            public void TypeWriteWithDuration(int occurrence, float duration = FlowKitConstants.TypeWriter.CompleteTextDuration)
            {
                occurrence -= 1;
                _engine.textEffectImpl.DurationTypeWrite(occurrence, duration);
            }

            /// <summary>
            /// Applies a color cycling effect to the TextMeshPro component.
            /// <list type="bullet">
            ///     <item>
            ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///     </item>
            ///     <item>
            ///         <description><b>Note</b>: To create an infinitly looping effect specify <c>duration</c> as 0f</description>
            ///     </item>
            /// </list>
            /// </summary>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="duration">Time in seconds the color cycling should last (0f for infinite)</param>
            /// <param name="delay">Delay between each color change</param>
            /// <param name="color">Specifies the color32 to cycle to and from</param>
            public void ColorCycle(int occurrence, float duration, float delay, Color32 color)
            {
                occurrence -= 1;
                _engine.textEffectImpl.ColorCycle(occurrence, duration, delay, color);
            }
        }
    }
}