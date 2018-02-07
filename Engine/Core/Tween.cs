using System;
using System.Runtime.CompilerServices;

namespace Engine
{
    public static class Tween
    {
        static TweenHandler[] tweens = { Linear, EaseIn, EaseOut, EaseInOut };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Linear(float value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseIn(float value) => value * value * value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOut(float value)
        {
            var f = value - 1;
            return f * f * f + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOut(float value)
        {
            if (value < 0.5f)
            {
                return 4 * value * value * value;
            }
            else
            {
                float f = ((2 * value) - 2);
                return 0.5f * f * f * f + 1;
            }
        }

        public static float Invoke(TweenType type, float value) => tweens[(int)type].Invoke(value);
    }

    public delegate float TweenHandler(float value);

    public enum TweenType { Linear, EaseIn, EaseOut, EaseInOut }
}