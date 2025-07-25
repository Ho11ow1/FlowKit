using FlowKit.Common;

namespace FlowKit.Core
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
                _engine.scalingImpl.Reset(target, occurrence, _engine.gameObject);
            }

            /// <summary>
            /// Sets the scale of a target UI element immediately | <b>[NOT Animated]</b>
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
            public void Up(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.scalingImpl.ScaleUp(target, occurrence, multiplier, easing, duration, delay);
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
            public void Down(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.scalingImpl.ScaleDown(target, occurrence, multiplier, easing, duration, delay);
            }
        }
    }
}