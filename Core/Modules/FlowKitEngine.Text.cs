using FlowKit.Common;

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
            public void TypeWriteWithDelay(int occurrence, float delay = 0.3f)
            {
                occurrence -= 1;
                _engine.typeWriteImpl.TypeWriterDelay(occurrence, delay);
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
                _engine.typeWriteImpl.TypeWriterDuration(occurrence, duration);
            }
        }
    }
}