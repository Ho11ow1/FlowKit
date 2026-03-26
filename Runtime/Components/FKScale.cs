using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public class FKScale : FKBase
    {
        // ============================== VOIDS ============================== \\

        // =============== Component Self =============== \\
        public void Scale(float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Scale(RectTransform, scale, duration, easing, delay);
        public void ScaleX(float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleX(RectTransform, scaleX, duration, easing, delay);
        public void ScaleY(float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleY(RectTransform, scaleY, duration, easing, delay);
        public void SetScale(float scale)
            => SetScale(RectTransform, scale);
        public void SetScale(Vector2 scale)
            => SetScale(RectTransform, scale);

        // =============== Monolith via Reference ===============\\
        public void Scale(RectTransform obj, float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(Scale), gameObject.name);
                return;
            }

            StartCoroutine(ScaleImpl(obj, scale, scale, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        public void ScaleX(RectTransform obj, float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleX), gameObject.name);
                return;
            }

            StartCoroutine(ScaleImpl(obj, scaleX, 1, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        public void ScaleY(RectTransform obj, float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleY), gameObject.name);
                return;
            }

            StartCoroutine(ScaleImpl(obj, 1, scaleY, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        public void SetScale(RectTransform obj, float scale)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(SetScale), gameObject.name);
                return;
            }

            obj.localScale = new Vector2(scale, scale);
        }
        public void SetScale(RectTransform obj, Vector2 scale)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(SetScale), gameObject.name);
                return;
            }

            obj.localScale = scale;
        }

        // ============================== ENUMERATORS ============================== \\

        // =============== Component Self =============== \\
        public IEnumerator ScaleRoutine(float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleRoutine(RectTransform, scale, duration, easing, delay);
        public IEnumerator ScaleXRoutine(float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleXRoutine(RectTransform, scaleX, duration, easing, delay);
        public IEnumerator ScaleYRoutine(float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleYRoutine(RectTransform, scaleY, duration, easing, delay);

        // =============== Monolith via Reference =============== \\
        public IEnumerator ScaleRoutine(RectTransform obj, float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleRoutine), gameObject.name);
                yield break;
            }

            yield return ScaleImpl(obj, scale, scale, duration, easing, delay, GenerateEventData(obj, duration));
        }
        public IEnumerator ScaleXRoutine(RectTransform obj, float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleXRoutine), gameObject.name);
                yield break;
            }

            yield return ScaleImpl(obj, scaleX, 1, duration, easing, delay, GenerateEventData(obj, duration));
        }
        public IEnumerator ScaleYRoutine(RectTransform obj, float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKScale>(nameof(ScaleYRoutine), gameObject.name);
                yield break;
            }

            yield return ScaleImpl(obj, 1, scaleY, duration, easing, delay, GenerateEventData(obj, duration));
        }

        // ============================== ACTUAL LOGIC ============================== \\

        private IEnumerator ScaleImpl(RectTransform obj, float scaleX, float scaleY, float duration, EasingType easing, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            Events.FlowKitEvents.InvokeStart(data);

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

            Events.FlowKitEvents.InvokeEnd(data);
        }

        private FKEventData GenerateEventData(RectTransform target, float duration)
        {
            return new FKEventData(gameObject, AnimationType.Scale, target, duration);
        }
    }
}
