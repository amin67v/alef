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
        static readonly Random gRandom = new Random(System.DateTime.Now.Millisecond);
        const float invMaxUint = 1f / (float)uint.MaxValue;
        int seed = 0;

        /// <summary>
        /// Returns a global instance of random class
        /// </summary>
        public static Random Global => gRandom;

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
        public float NextFloat() => (uint)NextInt() * invMaxUint;

        /// <summary>
        /// Returns a random float number between min and max.
        /// </summary>
        public float NextFloat(float min, float max) => NextFloat() * (max - min) + min;

        /// <summary>
        /// Returns a random color
        /// </summary>
        /// <param name="randAlpha">randomize alpha if true</param>
        public Color NextColor(bool randAlpha = false)
        {
            var r = NextInt(0, 256);
            var g = NextInt(0, 256);
            var b = NextInt(0, 256);
            var a = randAlpha ? NextInt(0, 256) : 255;
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Returns a normalized 3d direction vector
        /// </summary>
        public Vector3 NextDir3D()
        {
            var x = NextFloat() * 2 - 1;
            var y = NextFloat() * 2 - 1;
            var z = NextFloat() * 2 - 1;
            return Vector3.Normalize(new Vector3(x, y, z));
        }

        /// <summary>
        /// Returns a normalized 2d direction vector
        /// </summary>
        public Vector2 NextDir2D()
        {
            var x = NextFloat() * 2 - 1;
            var y = NextFloat() * 2 - 1;
            return Vector2.Normalize(new Vector2(x, y));
        }
    }
}