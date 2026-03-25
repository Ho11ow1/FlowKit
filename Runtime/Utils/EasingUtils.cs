namespace FlowKit.Utils
{
    public static class EasingUtils
    {
        public static float Evaluate(EasingType easing, float time)
        {
            return easing switch
            {
                EasingType.Linear => time,
                EasingType.Cubic => time * time * time,
                EasingType.EaseIn => time * time,
                EasingType.EaseOut => time * (2 - time),
                EasingType.EaseInOut => time < 0.5f ? 2 * time * time : -1 + (4 - 2 * time) * time,
                _ => time
            };
        }
    }
}
