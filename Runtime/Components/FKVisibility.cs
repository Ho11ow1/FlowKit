using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public class FKVisibility : FKBase
    {
        // ============================== VOIDS ============================== \\

        // =============== Component Self =============== \\
        public void Fade(float fromAlpha, float toAlpha, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Fade(RectTransform, fromAlpha, toAlpha, duration, easing, delay);
        public void SetVisibility(bool isVisible)
            => SetVisibility(RectTransform, isVisible ? 1f : 0f);
        public void SetVisibility(float alpha)
            => SetVisibility(RectTransform, alpha);

        // =============== Monolith via Reference ===============\\
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

        // ============================== ENUMERATORS ============================== \\

        // =============== Component Self =============== \\
        public FKHandle FadeHandle(float fromAlpha, float toAlpha, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => FadeHandle(RectTransform, fromAlpha, toAlpha, duration, easing, delay);

        // =============== Monolith via Reference =============== \\
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

            return new FKHandle(this, 
                () => FadeImpl(cg, fromAlpha, toAlpha, duration, easing, delay, GenerateEventData(obj, duration)),
                null);
        }

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

        private FKEventData GenerateEventData(RectTransform target, float duration)
        {
            return new FKEventData(gameObject, AnimationType.Visibility, target, duration);
        }
    }
}
