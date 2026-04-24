using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public sealed class FKMovement : FKBase
    {
        #region FireAndForget
        // =============== Component Self =============== \\
        /// <summary>
        /// Moves this component's RectTransform to the specified position over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="to">Target anchored position to move to.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Move(Vector2 to, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Move(RectTransform, to, duration, easing, delay);
        /// <summary>
        /// Moves this component's RectTransform in a given direction by a set offset over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="direction">Direction of movement.</param>
        /// <param name="offset">Distance in units to move in the specified direction.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Move(Direction direction, float offset, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Move(RectTransform, direction, offset, duration, easing, delay);
        /// <summary>
        /// Instantly sets this component's RectTransform to the specified anchored position.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="position">Target anchored position.</param>
        public void SetPosition(Vector2 position)
            => SetPosition(RectTransform, position);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Moves the specified RectTransform to the target position over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="to">Target anchored position to move to.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Move(RectTransform obj, Vector2 to, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKMovement>(nameof(Move), gameObject.name);
                return;
            }

            StartCoroutine(MoveImpl(obj, obj.localPosition, to, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        /// <summary>
        /// Moves the specified RectTransform in a given direction by a set offset over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null, or if the offset position could not be calculated.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="direction">Direction of movement.</param>
        /// <param name="offset">Distance in units to move in the specified direction.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
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
        /// <summary>
        /// Instantly sets the anchored position of the specified RectTransform. 
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to reposition.</param>
        /// <param name="position">Target anchored position.</param>
        public void SetPosition(RectTransform obj, Vector2 position)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKMovement>(nameof(SetPosition), gameObject.name);
                return;
            }

            obj.anchoredPosition = position;
        }
        #endregion
        #region Handles
        // =============== Component Self =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a movement animation on this component's RectTransform to the specified position.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="to">Target anchored position to move to.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle MoveHandle(Vector2 to, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => MoveHandle(RectTransform, to, duration, easing, delay);
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a directional movement animation on this component's RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="direction">Direction of movement.</param>
        /// <param name="offset">Distance in units to move in the specified direction.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle MoveHandle(Direction direction, float offset, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => MoveHandle(RectTransform, direction, offset, duration, easing, delay);

        // =============== Monolith via Reference ===============\\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a movement animation on the specified RectTransform to the target position.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="to">Target anchored position to move to.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
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
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a directional movement animation on the specified RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null or the offset position could not be calculated.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="direction">Direction of movement.</param>
        /// <param name="offset">Distance in units to move in the specified direction.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the movement.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
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
        #endregion
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
    }
}
