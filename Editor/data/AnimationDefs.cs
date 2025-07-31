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
*/
using UnityEngine;

namespace FlowKit.Editor
{
    internal class AnimationDefs
    {
        internal enum AnimationType
        {
            None,
            Rotate,
            Transition,
            Scale,
            Fade
        }

        // -------------------------------------------------------------- ROTATION --------------------------------------------------------------
        internal class RotateVars
        {
            public float degrees;
            public float duration;
            public Quaternion targetRotation;
        }

        // -------------------------------------------------------------- TRANSITION --------------------------------------------------------------
        
        internal class TransitionVars
        {
            public TransitionDirection direction;
            public TransitionOffset offset;
            public float duration;
            public Vector2 targetPos;
            public Vector2 startPos;

            public TransitionVars()
            {
                offset = new TransitionOffset();
            }
        }

        internal class TransitionOffset
        {
            public Vector2 vectorOffset;
            public float floatOffset;
        }

        internal enum TransitionDirection
        {
            FromTop,
            FromLeft,
            FromRight,
            FromBottom,
            FromPosition,
            ToTop,
            ToLeft,
            ToRight,
            ToBottom,
            ToPosition
        }

        // -------------------------------------------------------------- SCALE --------------------------------------------------------------

        internal class ScaleVars
        {
            public float multiplier;
            public float duration;
            public Vector2 targetScale;
        }

        // -------------------------------------------------------------- FADE --------------------------------------------------------------

        internal class FadeVars
        {
            public int alphaTarget;
            public float duration;
            public Color currentColor;
        }
    }
}
