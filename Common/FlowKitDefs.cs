/* Copyright 2025 Hollow1
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* --------------------------------------------------------
* FlowKit - Common definitions
* Created by Hollow1
* 
* Provides a unified space for all internal and public
* commonly used properties / values
* 
* Version: 1.0.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
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

    internal class FlowKitConstants
    {
        internal const int PanelIndex = 0;
        internal const int TextIndex = 1;
        internal const int ImageIndex = 2;
        internal const int ButtonIndex = 3;
        internal const float DefaultDuration = 0.5f;
    }
}