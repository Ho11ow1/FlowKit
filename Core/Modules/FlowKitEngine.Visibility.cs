using FlowKit.Common;

namespace FlowKit.Core
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
            ///  <list type="bullet">
            ///     <item>
            ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///     </item>
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
            /// Immediately sets the UI panel visibility
            /// </summary>
            /// <param name="visible">Sets the panel visibility condition</param>
            public void SetPanelVisibility(bool visible)
            {
                _engine.visibilityImpl.SetPanelVisibility(visible);
            }

            /// <summary>
            /// Fades in the UI element from transparent to fully opaque
            /// <list type="bullet">
            ///     <item>
            ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///     </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeIn(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeIn(target, occurrence, duration, delay);
            }

            /// <summary>
            /// Fades out the UI element from fully opaque to transparent
            /// <list type="bullet">
            ///     <item>
            ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///     </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeOut(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeOut(target, occurrence, duration, delay);
            }

            /// <summary>
            /// Fades the target UI element to a specified alpha value between 0-1
            /// <list type="bullet">
            ///     <item>
            ///         <description><b>Note</b>: The <c>occurrence</c> is using 1-based indexing, meaning the first element is 1, not 0.</description>
            ///     </item>
            /// </list>
            /// </summary>
            /// <param name="target">Target component to fade (Panel, Text, Image, Button)</param>
            /// <param name="occurrence">Specifies the instance of the target element (1-based index)</param>
            /// <param name="alpha">Specifies the target alpha value (Range between 0-1)</param>
            /// <param name="duration">Time in seconds for the fading duration</param>
            /// <param name="delay">Time in seconds to wait before starting the fade</param>
            public void FadeTo(AnimationTarget target, int occurrence, float alpha, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
            {
                occurrence -= 1;
                _engine.visibilityImpl.FadeTo(target, occurrence, alpha, duration, delay);
            }
        }
    }
}