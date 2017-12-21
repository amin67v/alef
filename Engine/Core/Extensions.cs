using System;
using System.Text;
using System.Json;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine
{
    public static class Extensions
    {
        /// <summary>
        /// Generates hash code for the given string
        /// </summary>
        public static int GetFastHash(this string str)
        {
            unchecked
            {
                const int prime = 486187739;
                int h = prime;
                int len = str.Length;
                for (int i = 0; i < len; i++)
                {
                    h += str[i];
                    h *= prime;
                }
                return h;
            }
        }

        /// <summary>
        /// Generates hash code for the given byte array
        /// </summary>
        public static int GetFastHash(this byte[] bytes)
        {
            unchecked
            {
                const int prime = 486187739;
                int h = prime;
                int len = bytes.Length;
                for (int i = 0; i < len; i++)
                {
                    h += bytes[i];
                    h *= prime;
                }
                return h;
            }
        }

        /// <summary>
        /// Remap this value from (min1, max1) to (min2, max2) range
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(this float value, float min1, float max1, float min2, float max2)
        {
            var t = (value - min1) / (max1 - min1);
            return min2.Blend(max2, t);

        }

        /// <summary>
        /// Blend this value to the other value with the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Blend(this float value, float other, float amount)
        {
            amount = amount.Clamp(0, 1);
            return (other - value) * amount + value;
        }

        /// <summary>
        /// Clamp this value between min and max
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float value, float min, float max)
        {
            return MathF.Min(max, MathF.Max(min, value));
        }


    }
}