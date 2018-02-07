using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using static System.MathF;

namespace Engine
{
    public static class MathF
    {
        public const float DegToRad = 0.0174532925199432957692369076849f;
        public const float RadToDeg = 57.295779513082320876798154814105f;

        /// <summary>
        /// Linear interpolation between a and b with the given t
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float a, float b, float t) => (b - a) * t + a;

        /// <summary>
        /// Clamps float value between min and max
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float value, float min, float max) => Min(max, Max(min, value));

        /// <summary>
        /// Normalize this float value from [min, max] to [0, 1]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(this float value, float min, float max) => (value - min) / (max - min);

        /// <summary>
        /// Denormalize this float value from [0, 1] to [min, max]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Denormalize(this float value, float min, float max) => Lerp(min, max, value);

        /// <summary>
        /// Remap this float value from [a1, b1] to [a2, b2]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(this float value, float a1, float b1, float a2, float b2) => value.Normalize(a1, b1).Denormalize(a2, b2);

    }
}