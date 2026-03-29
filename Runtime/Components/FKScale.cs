using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public sealed class FKScale : FKBase
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
        public FKHandle ScaleHandle(float scale, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleHandle(RectTransform, scale, duration, easing, delay);
        public FKHandle ScaleXHandle(float scaleX, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleXHandle(RectTransform, scaleX, duration, easing, delay);
        public FKHandle ScaleYHandle(float scaleY, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => ScaleYHandle(RectTransform, scaleY, duration, easing, delay);

        // =============== Monolith via Reference =============== \\
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
