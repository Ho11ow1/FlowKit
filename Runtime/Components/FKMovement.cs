using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public class FKMovement : FKBase
    {
        // ============================== VOIDS ============================== \\

        // =============== Component Self =============== \\
        public void Move(Vector2 to, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Move(RectTransform, to, duration, easing, delay);
        public void Move(Direction direction, float offset, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Move(RectTransform, direction, offset, duration, easing, delay);
        public void SetPosition(Vector2 position)
            => SetPosition(RectTransform, position);

        // =============== Monolith via Reference ===============\\
        public void Move(RectTransform obj, Vector2 to, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKMovement>(nameof(Move), gameObject.name);
                return;
            }

            StartCoroutine(MoveImpl(obj, obj.localPosition, to, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        public void Move(RectTransform obj, Direction direction, float offset, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKMovement>(nameof(Move), gameObject.name);
                return;
            }
            if (!CalculateOffsetPosition(obj.localPosition, direction, offset, out var from, out var to))
            {
                return;
            }

            StartCoroutine(MoveImpl(obj, from, to, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        public void SetPosition(RectTransform obj, Vector2 position)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKMovement>(nameof(SetPosition), gameObject.name);
                return;
            }

            obj.anchoredPosition = position;
        }

        // ============================== ENUMERATORS ============================== \\

        // =============== Component Self =============== \\
        public FKHandle MoveHandle(Vector2 to, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => MoveHandle(RectTransform, to, duration, easing, delay);
        public FKHandle MoveHandle(Direction direction, float offset, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => MoveHandle(RectTransform, direction, offset, duration, easing, delay);

        // =============== Monolith via Reference ===============\\
        public FKHandle MoveHandle(RectTransform obj, Vector2 to, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKMovement>(nameof(MoveHandle), gameObject.name);
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, duration);
            return new FKHandle(this,
                () => MoveImpl(obj, obj.localPosition, to, duration, easing, delay, eventData),
                eventData);
        }
        public FKHandle MoveHandle(RectTransform obj, Direction direction, float offset, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKMovement>(nameof(MoveHandle), gameObject.name);
                return FKHandle.Invalid;
            }
            if (!CalculateOffsetPosition(obj.localPosition, direction, offset, out var from, out var to))
            {
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, duration);
            return new FKHandle(this,
                () => MoveImpl(obj, from, to, duration, easing, delay, eventData),
                eventData);
        }

        // ============================== ACTUAL LOGIC ============================== \\

        private IEnumerator MoveImpl(RectTransform obj, Vector2 from, Vector2 to, float duration, EasingType easing, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                var t = EasingUtils.Evaluate(easing, elapsedTime / duration);

                obj.anchoredPosition = Vector2.Lerp(from, to, t);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            obj.anchoredPosition = to;

            FlowKitEvents.InvokeEnd(data);
        }

        private bool CalculateOffsetPosition(Vector2 currentPos, Direction dir, float offset, out Vector2 from, out Vector2 to)
        {
            switch (dir)
            {
                case Direction.ToUp:
                    from = currentPos;
                    to = new Vector2(currentPos.x, currentPos.y + offset);
                    return true;
                case Direction.ToRight:
                    from = currentPos;
                    to = new Vector2(currentPos.x + offset, currentPos.y);
                    return true;
                case Direction.ToBottom:
                    from = currentPos;
                    to = new Vector2(currentPos.x, currentPos.y - offset);
                    return true;
                case Direction.ToLeft:
                    from = currentPos;
                    to = new Vector2(currentPos.x - offset, currentPos.y);
                    return true;

                case Direction.FromUp:
                    from = new Vector2(currentPos.x, currentPos.y + offset);
                    to = currentPos;
                    return true;
                case Direction.FromRight:
                    from = new Vector2(currentPos.x + offset, currentPos.y);
                    to = currentPos;
                    return true;
                case Direction.FromBottom:
                    from = new Vector2(currentPos.x, currentPos.y - offset);
                    to = currentPos;
                    return true;
                case Direction.FromLeft:
                    from = new Vector2(currentPos.x - offset, currentPos.y);
                    to = currentPos;
                    return true;

                default:
                    FKLogger.UnknownDirection<FKMovement>(nameof(Move), dir.ToString(), gameObject.name);
                    from = Vector2.zero;
                    to = Vector2.zero;
                    return false;
            }
        }

        private FKEventData GenerateEventData(RectTransform target, float duration)
        {
            return new FKEventData(gameObject, AnimationType.Movement, target, duration);
        }
    }
}
