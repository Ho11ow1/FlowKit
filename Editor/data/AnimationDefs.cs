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