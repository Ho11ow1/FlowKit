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
            public float degreesPerTick => degrees / duration;
            public Quaternion targetRotation;
        }

        // -------------------------------------------------------------- TRANSITION --------------------------------------------------------------
        
        internal class TransitionVars
        {
            public TransitionDirection direction;
            public TransitionOffset offset;
            public float duration;
        }

        internal class TransitionOffset
        {
            public bool isVector;
            public Vector2 vectorOffset;
            public float floatOffset;

            public TransitionOffset(Vector2 vec)
            {
                isVector = true;
                vectorOffset = vec;
            }

            public TransitionOffset(float val)
            {
                isVector = false;
                floatOffset = val;
            }
        }

        internal enum TransitionDirection
        {
            FromTop,
            FromBottom,
            FromRight,
            FromLeft,
            FromPosition,
            ToTop,
            ToBottom,
            ToRight,
            ToLeft,
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
            public float alphaTarget;
            public float duration;
        }
    }
}