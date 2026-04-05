using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public sealed class FKVisibility : FKBase
    {
        #region FireAndForget
        // =============== Component Self =============== \\
        /// <summary>
        /// Fades this component's <see cref="CanvasGroup"/> from one alpha value to another over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="CanvasGroup"/> is found on this component.</para>
        /// </summary>
        /// <param name="fromAlpha">Starting alpha value, between 0 and 1.</param>
        /// <param name="toAlpha">Target alpha value, between 0 and 1.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the fade.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Fade(float fromAlpha, float toAlpha, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Fade(RectTransform, fromAlpha, toAlpha, duration, easing, delay);
        /// <summary>
        /// Instantly shows or hides this component's <see cref="CanvasGroup"/> by setting its alpha, interactability, and raycast blocking.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="CanvasGroup"/> is found on this component.</para>
        /// </summary>
        /// <param name="isVisible">If true, sets alpha to 1 and enables interaction. If false, sets alpha to 0 and disables interaction.</param>
        public void SetVisibility(bool isVisible)
            => SetVisibility(RectTransform, isVisible ? 1f : 0f);
        /// <summary>
        /// Instantly sets the alpha of this component's <see cref="CanvasGroup"/>, and toggles interactability and raycast blocking based on the value.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="CanvasGroup"/> is found on this component.</para>
        /// </summary>
        /// <param name="alpha">Target alpha value, between 0 and 1. Values above 0 enable interaction and raycasts.</param>
        public void SetVisibility(float alpha)
            => SetVisibility(RectTransform, alpha);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Fades the <see cref="CanvasGroup"/> on the specified RectTransform from one alpha value to another over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="CanvasGroup"/> is found on <paramref name="obj"/>.</para>
        /// </summary>
        /// <param name="obj">RectTransform whose <see cref="CanvasGroup"/> will be animated.</param>
        /// <param name="fromAlpha">Starting alpha value, between 0 and 1.</param>
        /// <param name="toAlpha">Target alpha value, between 0 and 1.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the fade.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Fade(RectTransform obj, float fromAlpha, float toAlpha, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKVisibility>(nameof(Fade), gameObject.name);
                return;
            }
            if (!obj.TryGetComponent<CanvasGroup>(out var cg))
            {
                FKLogger.MissingComponent<FKVisibility>(typeof(CanvasGroup), obj.name);
                return;
            }

            StartCoroutine(FadeImpl(cg, fromAlpha, toAlpha, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        /// <summary>
        /// Instantly sets the alpha of the <see cref="CanvasGroup"/> on the specified RectTransform, and toggles interactability and raycast blocking based on the value.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="CanvasGroup"/> is found on <paramref name="obj"/>.</para>
        /// </summary>
        /// <param name="obj">RectTransform whose <see cref="CanvasGroup"/> will be updated.</param>
        /// <param name="alpha">Target alpha value, between 0 and 1. Values above 0 enable interaction and raycasts.</param>
        public void SetVisibility(RectTransform obj, float alpha)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKVisibility>(nameof(SetVisibility), gameObject.name);
                return;
            }
            if (!obj.TryGetComponent<CanvasGroup>(out var cg))
            {
                FKLogger.MissingComponent<FKVisibility>(typeof(CanvasGroup), obj.name);
                return;
            }

            cg.alpha = alpha;
            cg.interactable = alpha > 0f;
            cg.blocksRaycasts = alpha > 0f;
        }
        #endregion
        #region Handles
        // =============== Component Self =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a fade animation on this component's <see cref="CanvasGroup"/>.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null or no <see cref="CanvasGroup"/> is found on this component.</para>
        /// </summary>
        /// <param name="fromAlpha">Starting alpha value, between 0 and 1.</param>
        /// <param name="toAlpha">Target alpha value, between 0 and 1.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the fade.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle FadeHandle(float fromAlpha, float toAlpha, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => FadeHandle(RectTransform, fromAlpha, toAlpha, duration, easing, delay);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a fade animation on the <see cref="CanvasGroup"/> of the specified RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null or no <see cref="CanvasGroup"/> is found on <paramref name="obj"/>.</para>
        /// </summary>
        /// <param name="obj">RectTransform whose <see cref="CanvasGroup"/> will be animated.</param>
        /// <param name="fromAlpha">Starting alpha value, between 0 and 1.</param>
        /// <param name="toAlpha">Target alpha value, between 0 and 1.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the fade.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
        public FKHandle FadeHandle(RectTransform obj, float fromAlpha, float toAlpha, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKVisibility>(nameof(FadeHandle), gameObject.name);
                return FKHandle.Invalid;
            }
            if (!obj.TryGetComponent<CanvasGroup>(out var cg))
            {
                FKLogger.MissingComponent<FKVisibility>(typeof(CanvasGroup), obj.name);
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, duration);
            return new FKHandle(this, 
                () => FadeImpl(cg, fromAlpha, toAlpha, duration, easing, delay, eventData),
                eventData);
        }
        #endregion
        // ============================== ACTUAL LOGIC ============================== \\

        private IEnumerator FadeImpl(CanvasGroup cg, float from, float to, float duration, EasingType easing, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            cg.alpha = from;
            cg.interactable = from > 0f;
            cg.blocksRaycasts = from > 0f;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                var t = EasingUtils.Evaluate(easing, elapsedTime / duration);

                cg.alpha = Mathf.Lerp(from, to, t);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            cg.alpha = to;
            cg.interactable = to > 0f;
            cg.blocksRaycasts = to > 0f;

            FlowKitEvents.InvokeEnd(data);
        }
    }
}
