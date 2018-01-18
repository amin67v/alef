using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine
{
    public struct Rect : IEquatable<Rect>
    {
        Vector4 vec;

        /// <summary>
        /// Create rect with the specified (x, y, w, h).
        /// </summary>
        public Rect(float x, float y, float w, float h)
        {
            vec.X = x;
            vec.Y = y;
            vec.Z = MathF.Max(0, w);
            vec.W = MathF.Max(0, h);
        }

        /// <summary>
        /// Create rect with the specified pos and size.
        /// </summary>.
        public Rect(Vector2 pos, Vector2 size)
        {
            vec.X = pos.X;
            vec.Y = pos.Y;
            vec.Z = MathF.Max(0, size.X);
            vec.W = MathF.Max(0, size.Y);
        }

        /// <summary>
        /// Create rect with the specified vector 4.
        /// </summary>.
        public Rect(Vector4 vec)
        {
            this.vec = vec;
        }

        /// <summary>
        /// The x coord of the rect.
        /// </summary>
        public float X
        {
            get => vec.X;
            set => vec.X = value;
        }

        /// <summary>
        /// The y coord of the rect.
        /// </summary>
        public float Y
        {
            get => vec.Y;
            set => vec.Y = value;
        }


        /// <summary>
        /// The min x coord of the rect.
        /// </summary>
        public float XMin
        {
            get => vec.X;
            set
            {
                Width += vec.X - value;
                vec.X = value;
            }
        }

        /// <summary>
        /// The min y coord of the rect.
        /// </summary>
        public float YMin
        {
            get => vec.Y;
            set
            {
                Height += vec.Y - value;
                vec.Y = value;
            }
        }

        /// <summary>
        /// The max x coord of the rect.
        /// </summary>
        public float XMax
        {
            get => vec.X + vec.Z;
            set => Width = value - vec.X;
        }

        /// <summary>
        /// The max y coord of the rect.
        /// </summary>
        public float YMax
        {
            get => vec.Y + vec.W;
            set => Height = value - vec.Y;
        }

        /// <summary>
        /// The width of the rect.
        /// </summary>
        public float Width
        {
            get => vec.Z;
            set => vec.Z = MathF.Max(0, value);
        }

        /// <summary>
        /// The Height of the rect.
        /// </summary>
        public float Height
        {
            get => vec.W;
            set => vec.W = MathF.Max(0, value);
        }

        /// <summary>
        /// The center position of this rect.
        /// </summary>
        public Vector2 Center
        {
            get => new Vector2(XHalf, YHalf);
            set
            {
                XHalf = value.X;
                YHalf = value.Y;
            }
        }

        /// <summary>
        /// The half value between xmin and xmax.
        /// </summary>
        public float XHalf
        {
            get => vec.X + Width * .5f;
            set => vec.X = value - Width * .5f;
        }

        /// <summary>
        /// The half value between ymin and ymax.
        /// </summary>
        public float YHalf
        {
            get => vec.Y + Height * .5f;
            set => vec.Y = value - Height * .5f;
        }

        /// <summary>
        /// The position of this rect.
        /// </summary>
        public Vector2 Position
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// The size of this rect.
        /// </summary>
        public Vector2 Size
        {
            get => new Vector2(vec.Z, vec.W);
            set
            {
                vec.Z = value.X;
                vec.W = value.Y;
            }
        }

        /// <summary>
        /// The vector2 represents (XMin, YMin)
        /// </summary>
        public Vector2 XMinYMin
        {
            get => new Vector2(XMin, YMin);
            set
            {
                XMin = value.X;
                YMin = value.Y;
            }
        }

        /// <summary>
        /// The vector2 represents (XMax, YMin)
        /// </summary>
        public Vector2 XMaxYMin
        {
            get => new Vector2(XMax, YMin);
            set
            {
                XMax = value.X;
                YMin = value.Y;
            }
        }

        /// <summary>
        /// The vector2 represents (XMin, YMax)
        /// </summary>
        public Vector2 XMinYMax
        {
            get => new Vector2(XMin, YMax);
            set
            {
                XMin = value.X;
                YMax = value.Y;
            }
        }

        /// <summary>
        /// The vector2 represents (XMax, YMax)
        /// </summary>
        public Vector2 XMaxYMax
        {
            get => new Vector2(XMax, YMax);
            set
            {
                XMax = value.X;
                YMax = value.Y;
            }
        }

        /// <summary>
        /// Area of this rect
        /// </summary>
        public float Area => Width * Height;

        /// <summary>
        /// Checks if point lies within this rect.
        /// </summary>
        public bool Contains(Vector2 point)
        {
            return point.X > X && point.X < XMax &&
                   point.Y > Y && point.Y < YMax;
        }

        /// <summary>
        /// Checks if this rect overlap the other rect.
        /// </summary>
        public bool Overlap(Rect other)
        {
            return X < other.XMax && XMax > other.X &&
                   Y < other.YMax && YMax > other.Y;
        }

        /// <summary>
        /// Inflate this rect by x, y
        /// </summary>
        public void Inflate(float x, float y)
        {
            X -= x;
            XMax += y;
            Width += 2 * x;
            Height += 2 * y;
        }

        /// <summary>
        /// Extends this rect to contain the point.
        /// </summary>
        public void Extend(Vector2 point)
        {
            XMin = MathF.Min(XMin, point.X);
            XMax = MathF.Max(XMax, point.X);
            YMin = MathF.Min(YMin, point.Y);
            YMax = MathF.Max(YMax, point.Y);
        }

        /// <summary>
        /// Extends this rect to contain the other rect.
        /// </summary>
        public void Extend(Rect rect)
        {
            XMin = MathF.Min(XMin, rect.XMin);
            XMax = MathF.Max(XMax, rect.XMax);
            YMin = MathF.Min(YMin, rect.YMin);
            YMax = MathF.Max(YMax, rect.YMax);
        }

        /// <summary>
        /// Converts this rect to vector4
        /// </summary>
        public Vector4 ToVector4() => vec;

        public override string ToString()
        {
            return $"({vec.X}, {vec.Y}, {vec.Z}, {vec.W})";
        }

        public override int GetHashCode() => vec.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is Rect)
                return Equals((Rect)obj);
            else
                return false;
        }

        public bool Equals(Rect other) => vec == other.vec;

        public static Rect operator *(Rect l, float r)
        {
            l.vec *= r;
            return l;
        }

        public static Rect operator /(Rect l, float r)
        {
            l.vec /= r;
            return l;
        }

        public static Rect operator *(Rect l, Vector2 r)
        {
            return new Rect(l.vec.X * r.X, l.vec.Y * r.Y, l.vec.Z * r.X, l.vec.W * r.Y);
        }

        public static Rect operator /(Rect l, Vector2 r)
        {
            return new Rect(l.vec.X / r.X, l.vec.Y / r.Y, l.vec.Z / r.X, l.vec.W / r.Y);
        }

        public static bool operator ==(Rect a, Rect b) => a.Equals(b);

        public static bool operator !=(Rect a, Rect b) => !a.Equals(b);

    }
}