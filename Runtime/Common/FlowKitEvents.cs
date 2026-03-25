using System;

namespace FlowKit.Events
{
    public static class FlowKitEvents
    {
        public static event Action<FKEventData> OnAnimationStart;
        public static event Action<FKEventData> OnAnimationEnd;

        public static event Action<FKEventData> OnFadeStart;
        public static event Action<FKEventData> OnFadeEnd;
        public static event Action<FKEventData> OnMovementStart;
        public static event Action<FKEventData> OnMovementEnd;
        public static event Action<FKEventData> OnScaleStart;
        public static event Action<FKEventData> OnScaleEnd;
        public static event Action<FKEventData> OnRotateStart;
        public static event Action<FKEventData> OnRotateEnd;
        public static event Action<FKEventData> OnTextStart;
        public static event Action<FKEventData> OnTextEnd;

        internal static void InvokeStart(FKEventData data)
        {
            OnAnimationStart?.Invoke(data);
            switch (data.AnimationType)
            {
                case AnimationType.Movement:
                    OnMovementStart?.Invoke(data);
                    break;
                case AnimationType.Rotation:
                    OnRotateStart?.Invoke(data);
                    break;
                case AnimationType.Scale:
                    OnScaleStart?.Invoke(data);
                    break;
                case AnimationType.Fade:
                    OnFadeStart?.Invoke(data);
                    break;
                case AnimationType.Text:
                    OnTextStart?.Invoke(data);
                    break;
            }
        }

        internal static void InvokeEnd(FKEventData data)
        {
            OnAnimationEnd?.Invoke(data);
            switch (data.AnimationType)
            {
                case AnimationType.Movement:
                    OnMovementEnd?.Invoke(data);
                    break;
                case AnimationType.Rotation:
                    OnRotateEnd?.Invoke(data);
                    break;
                case AnimationType.Scale:
                    OnScaleEnd?.Invoke(data);
                    break;
                case AnimationType.Fade:
                    OnFadeEnd?.Invoke(data);
                    break;
                case AnimationType.Text:
                    OnTextEnd?.Invoke(data);
                    break;
            }
        }
    }
}
