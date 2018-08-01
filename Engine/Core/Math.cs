using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using static System.MathF;

namespace Engine
{
    public static class Math
    {
        public const float PI = 3.1415926535897932384626433832795f;
        public const float E = 2.7182818284590452353602874713527f;
        public const float DegToRad = 0.0174532925199432957692369076849f;
        public const float RadToDeg = 57.295779513082320876798154814105f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(float x) => System.MathF.Abs(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acos(float x) => System.MathF.Acos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asin(float x) => System.MathF.Asin(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan(float x) => System.MathF.Atan(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan2(float y, float x) => System.MathF.Atan2(y, x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceiling(float x) => System.MathF.Ceiling(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float x) => System.MathF.Cos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cosh(float x) => System.MathF.Cosh(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Exp(float x) => System.MathF.Exp(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(float x) => System.MathF.Floor(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float IEEERemainder(float x, float y) => System.MathF.IEEERemainder(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(float x, float y) => System.MathF.Log(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(float x) => System.MathF.Log(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10(float x) => System.MathF.Log10(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float x, float y) => System.MathF.Max(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float x, float y) => System.MathF.Min(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(float x, float y) => System.MathF.Pow(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x) => System.MathF.Round(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x, int digits) => System.MathF.Round(x, digits);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x, int digits, MidpointRounding mode) => System.MathF.Round(x, digits, mode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x, MidpointRounding mode) => System.MathF.Round(x, mode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(float x) => System.MathF.Sign(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float x) => System.MathF.Sin(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sinh(float x) => System.MathF.Sinh(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(float x) => System.MathF.Sqrt(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(float x) => System.MathF.Tan(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tanh(float x) => System.MathF.Tanh(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Truncate(float x) => System.MathF.Truncate(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(int value) => System.Math.Sign(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Min(byte val1, byte val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Min(uint val1, uint val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Min(short val1, short val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Min(ushort val1, ushort val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long val1, long val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Min(ulong val1, ulong val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2) => System.Math.Min(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Max(byte val1, byte val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Max(uint val1, uint val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Max(short val1, short val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Max(ushort val1, ushort val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Max(long val1, long val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Max(ulong val1, ulong val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2) => System.Math.Max(val1, val2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Clamp(byte value, byte min, byte max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Clamp(uint value, uint min, uint max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Clamp(ushort value, ushort min, ushort max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Clamp(short value, short min, short max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Clamp(ulong value, ulong min, ulong max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long value, long min, long max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(double value, double min, double max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max) => System.Math.Clamp(value, min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Abs(decimal value) => System.Math.Abs(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(double value) => System.Math.Abs(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Abs(short value) => System.Math.Abs(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Abs(int value) => System.Math.Abs(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Abs(long value) => System.Math.Abs(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Abs(sbyte value) => System.Math.Abs(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float a, float b, float t) => (b - a) * t + a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NextPot(this int value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value++;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Repeat(this float value, float max)
        {
            var r = value % max;
            if (r < 0)
                r += max;

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Repeat(int value, int max)
        {
            var r = value % max;
            if (r < 0)
                r += max;

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float value, float min, float max) => (value - min) / (max - min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Denormalize(float value, float min, float max) => Lerp(min, max, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(float value, float a1, float b1, float a2, float b2) => Denormalize(Normalize(value, a1, b1), a2, b2);

        public static Quaternion FromEuler(Vector3 value)
        {
            double yawOver2 = value.Y * 0.5f;
            float cosYawOver2 = (float)System.Math.Cos(yawOver2);
            float sinYawOver2 = (float)System.Math.Sin(yawOver2);
            double pitchOver2 = value.Z * 0.5f;
            float cosPitchOver2 = (float)System.Math.Cos(pitchOver2);
            float sinPitchOver2 = (float)System.Math.Sin(pitchOver2);
            double rollOver2 = value.X * 0.5f;
            float cosRollOver2 = (float)System.Math.Cos(rollOver2);
            float sinRollOver2 = (float)System.Math.Sin(rollOver2);

            var c1 = Cos(value.Y * .5f);
            var c2 = Cos(value.Z * .5f);
            var c3 = Cos(value.X * .5f);
            var s1 = Sin(value.Y * .5f);
            var s2 = Sin(value.Z * .5f);
            var s3 = Sin(value.X * .5f);

            Quaternion q;
            q.W = c1 * c2 * c3 - s1 * s2 * s3;
            q.X = s1 * s2 * c3 + c1 * c2 * s3;
            q.Y = s1 * c2 * c3 + c1 * s2 * s3;
            q.Z = c1 * s2 * c3 - s1 * c2 * s3;

            return q;
        }

        public static Vector3 ToEuler(Quaternion value)
        {
            Vector3 vec;
            float test = value.X * value.Y + value.Z * value.W;
            if (test > 0.499f)
            {
                vec.Y = 2 * Atan2(value.X, value.W);
                vec.Z = PI * .5f;
                vec.X = 0;
                return vec;
            }

            if (test < -0.499f)
            {
                vec.Y = -2 * Atan2(value.X, value.W);
                vec.Z = -PI * .5f;
                vec.X = 0;
                return vec;
            }

            float sqx = value.X * value.X;
            float sqy = value.Y * value.Y;
            float sqz = value.Z * value.Z;

            vec.Y = Atan2(2 * value.Y * value.W - 2 * value.X * value.Z, 1 - 2 * sqy - 2 * sqz);
            vec.Z = Asin(2 * test);
            vec.X = Atan2(2 * value.X * value.W - 2 * value.Y * value.Z, 1 - 2 * sqx - 2 * sqz);

            return vec;
        }

    }
}