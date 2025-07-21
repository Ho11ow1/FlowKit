using UnityEngine;

namespace FlowKit.Editor
{
    internal class AnimationDefs
    {
        // -------------------------------------------------------------- ROTATION --------------------------------------------------------------
        internal class RotateVars
        {
            public float degrees;
            public float duration;
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
        }

        // -------------------------------------------------------------- FADE --------------------------------------------------------------

        internal class FadeVars
        {
            public FadeType type;
            public float duration;
        }

        internal enum FadeType
        {
            FadeIn,
            FadeOut
        }
    }
}