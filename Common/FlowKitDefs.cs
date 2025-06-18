using UnityEngine.Events;

namespace FlowKit.Common
{
    public enum AnimationTarget
    {
        Panel,
        Text,
        Image,
        Button
    }

    public enum EasingType
    {
        Linear,
        Cubic,
        EaseIn, // Quadratic
        EaseOut, // Quadratic
        EaseInOut // Quadratic
    }

    public static class FlowKitEvents
    {
        public static event UnityAction FadeStart;
        public static event UnityAction FadeEnd;
        public static event UnityAction TransitionStart;
        public static event UnityAction TransitionEnd;
        public static event UnityAction ScaleStart;
        public static event UnityAction ScaleEnd;
        public static event UnityAction RotateStart;
        public static event UnityAction RotateEnd;
        public static event UnityAction TypeWriteStart;
        public static event UnityAction TypeWriteEnd;

        internal static void InvokeFadeStart() { FadeStart?.Invoke(); }
        internal static void InvokeFadeEnd() { FadeEnd?.Invoke(); }
        internal static void InvokeTransitionStart() { TransitionStart?.Invoke(); }
        internal static void InvokeTransitionEnd() { TransitionEnd?.Invoke(); }
        internal static void InvokeScaleStart() { ScaleStart?.Invoke(); }
        internal static void InvokeScaleEnd() { ScaleEnd?.Invoke(); }
        internal static void InvokeRotateStart() { RotateStart?.Invoke(); }
        internal static void InvokeRotateEnd() { RotateEnd?.Invoke(); }
        internal static void InvokeTypeWriteStart() { TypeWriteStart?.Invoke(); }
        internal static void InvokeTypeWriteEnd() { TypeWriteEnd?.Invoke(); }
    }

    public static class FlowKitConstants
    {
        public const int PanelIndex = 0;
        public const int TextIndex = 1;
        public const int ImageIndex = 2;
        public const int ButtonIndex = 3;
        public const float DefaultDuration = 0.5f;
    }
}