using FlowKit.Common;

namespace FlowKit.Core
{
    public partial class FlowKitEngine
    {
        public RotateModule Rotate { get; private set; }

        public class RotateModule
        {
            private readonly FlowKitEngine _engine;

            public RotateModule(FlowKitEngine engine) { _engine = engine; }

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
        }
    }
}