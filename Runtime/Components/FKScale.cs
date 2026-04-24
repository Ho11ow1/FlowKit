using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public sealed class FKScale : FKBase
    {
        #region FireAndForget
        // =============== Component Self =============== \\
        /// <summary>
        /// Uniformly scales this component's RectTransform to the target scale over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scale">Target uniform scale applied to both X and Y axes.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Scale(float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Scale(RectTransform, scale, duration, easing, delay);

        /// <summary>
        /// Scales this component's RectTransform on the X axis to the target scale over a given duration.
        /// <para>The Y axis is left unchanged.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scaleX">Target scale on the X axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void ScaleX(float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleX(RectTransform, scaleX, duration, easing, delay);
        /// <summary>
        /// Scales this component's RectTransform on the Y axis to the target scale over a given duration.
        /// <para>The X axis is left unchanged.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scaleY">Target scale on the Y axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void ScaleY(float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleY(RectTransform, scaleY, duration, easing, delay);
        /// <summary>
        /// Instantly sets this component's RectTransform to a uniform scale on both axes.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scale">Target uniform scale applied to both X and Y axes.</param>
        public void SetScale(float scale)
            => SetScale(RectTransform, scale);
        /// <summary>
        /// Instantly sets this component's RectTransform scale using individual X and Y values.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scale">Target scale as a Vector2, with X and Y set independently.</param>
        public void SetScale(Vector2 scale)
            => SetScale(RectTransform, scale);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Uniformly scales the specified RectTransform to the target scale over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="scale">Target uniform scale applied to both X and Y axes.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Scale(RectTransform obj, float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(Scale), gameObject.name);
                return;
            }

            StartCoroutine(ScaleImpl(obj, scale, scale, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        /// <summary>
        /// Scales the specified RectTransform on the X axis to the target scale over a given duration.
        /// <para>The Y axis is left unchanged.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="scaleX">Target scale on the X axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void ScaleX(RectTransform obj, float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleX), gameObject.name);
                return;
            }

            StartCoroutine(ScaleImpl(obj, scaleX, 1, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        /// <summary>
        /// Scales the specified RectTransform on the Y axis to the target scale over a given duration.
        /// <para>The X axis is left unchanged.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="scaleY">Target scale on the Y axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void ScaleY(RectTransform obj, float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleY), gameObject.name);
                return;
            }

            StartCoroutine(ScaleImpl(obj, 1, scaleY, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        /// <summary>
        /// Instantly sets the specified RectTransform to a uniform scale on both axes.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to rescale.</param>
        /// <param name="scale">Target uniform scale applied to both X and Y axes.</param>
        public void SetScale(RectTransform obj, float scale)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(SetScale), gameObject.name);
                return;
            }

            obj.localScale = new Vector2(scale, scale);
        }
        /// <summary>
        /// Instantly sets the specified RectTransform scale using individual X and Y values.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to rescale.</param>
        /// <param name="scale">Target scale as a Vector2, with X and Y set independently.</param>
        public void SetScale(RectTransform obj, Vector2 scale)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(SetScale), gameObject.name);
                return;
            }

            obj.localScale = scale;
        }
        #endregion
        #region Handles
        // =============== Component Self =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a uniform scale animation on this component's RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scale">Target uniform scale applied to both X and Y axes.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle ScaleHandle(float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleHandle(RectTransform, scale, duration, easing, delay);
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for an X axis scale animation on this component's RectTransform.
        /// <para>The animation does not start until the handle is played. The Y axis is left unchanged.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scaleX">Target scale on the X axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle ScaleXHandle(float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleXHandle(RectTransform, scaleX, duration, easing, delay);
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a Y axis scale animation on this component's RectTransform.
        /// <para>The animation does not start until the handle is played. The X axis is left unchanged.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="scaleY">Target scale on the Y axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle ScaleYHandle(float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleYHandle(RectTransform, scaleY, duration, easing, delay);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a uniform scale animation on the specified RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="scale">Target uniform scale applied to both X and Y axes.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
        public FKHandle ScaleHandle(RectTransform obj, float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleHandle), gameObject.name);
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, duration);
            return new FKHandle(this,
                () => ScaleImpl(obj, scale, scale, duration, easing, delay, eventData),
                eventData);
        }
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for an X axis scale animation on the specified RectTransform.
        /// <para>The animation does not start until the handle is played. The Y axis is left unchanged.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="scaleX">Target scale on the X axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
        public FKHandle ScaleXHandle(RectTransform obj, float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleXHandle), gameObject.name);
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, duration);
            return new FKHandle(this,
                () => ScaleImpl(obj, scaleX, 1, duration, easing, delay, eventData),
                eventData);
        }
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a Y axis scale animation on the specified RectTransform.
        /// <para>The animation does not start until the handle is played. The X axis is left unchanged.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="scaleY">Target scale on the Y axis.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the scale.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
        public FKHandle ScaleYHandle(RectTransform obj, float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleYHandle), gameObject.name);
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, duration);
            return new FKHandle(this,
                () => ScaleImpl(obj, 1, scaleY, duration, easing, delay, eventData),
                eventData);
        }
        #endregion
        // ============================== ACTUAL LOGIC ============================== \\

        private IEnumerator ScaleImpl(RectTransform obj, float scaleX, float scaleY, float duration, EasingType easing, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            var startScale = obj.localScale;
            var targetScale = new Vector2(startScale.x * scaleX, startScale.y * scaleY);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                var t = EasingUtils.Evaluate(easing, elapsedTime / duration);

                obj.localScale = Vector2.Lerp(startScale, targetScale, t);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            obj.localScale = targetScale;

            FlowKitEvents.InvokeEnd(data);
        }
    }
}
