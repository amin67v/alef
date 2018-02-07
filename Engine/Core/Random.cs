using System;
using System.Numerics;
using static System.MathF;

namespace Engine
{
    /// <summary>
    /// Implements a simple pseudo-random number generator.
    /// </summary>
    public class Random
    {
        const float inv_max_uint = 1f / (float)uint.MaxValue;
        static readonly Random g_rand = new Random((int)DateTime.Now.ToBinary());

        int seed = 0;

        /// <summary>
        /// Creates an instance of random with the given seed number.
        /// </summary>
        public Random(int seed = 0) => this.seed = seed;

        /// <summary>
        /// Return a random int number.
        /// </summary>
        public int NextInt() => seed = 214013 * seed + 2531011;

        /// <summary>
        /// Return a random int number between min and max.
        /// </summary>
        public int NextInt(int min, int max) => (int)Floor(NextFloat(min, max));

        /// <summary>
        /// Returns a random float number between 0 and 1.
        /// </summary>
        public float NextFloat() => (uint)NextInt() * inv_max_uint;

        /// <summary>
        /// Returns a random float number between min and max.
        /// </summary>
        public float NextFloat(float min, float max) => NextFloat() * (max - min) + min;

        /// <summary>
        /// Return a random int number.
        /// </summary>
        public static int Int() => g_rand.NextInt();

        /// <summary>
        /// Return a random int number between min and max.
        /// </summary>
        public static int Int(int min, int max) => g_rand.NextInt(min, max);

        /// <summary>
        /// Returns a random float number between 0 and 1.
        /// </summary>
        public static float Float() => g_rand.NextFloat();

        /// <summary>
        /// Returns a random float number between min and max.
        /// </summary>
        public static float Float(float min, float max) => g_rand.NextFloat(min, max);

        /// <summary>
        /// Returns a random opaque color.
        /// </summary>
        public static Color Color() => Engine.Color.FromRgb(Int());

    }
}