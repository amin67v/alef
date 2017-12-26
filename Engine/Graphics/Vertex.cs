using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex : IEquatable<Vertex>
    {
        public static readonly int SizeInBytes = Marshal.SizeOf<Vertex>();

        public Vector2 Position;
        public Vector2 Texcoord;
        public Color Color;

        public Vertex(float x, float y, float tx, float ty, Color c)
        {
            Position = new Vector2(x, y);
            Texcoord = new Vector2(tx, ty);
            Color = c;
        }

        public Vertex(Vector2 pos, Vector2 texcoord, Color c)
        {
            Position = pos;
            Texcoord = texcoord;
            Color = c;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vertex)
                return Equals((Vertex)obj);
            else
                return false;
        }

        public bool Equals(Vertex other)
        {
            return Position == other.Position &&
                   Texcoord == other.Texcoord &&
                   Color == other.Color;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int prime = 486187739;
                int hash = prime;
                hash = (hash + Position.GetHashCode()) * prime;
                hash = (hash + Texcoord.GetHashCode()) * prime;
                hash = (hash + Color.GetHashCode()) * prime;
                return hash;
            }
        }

        public static bool operator ==(Vertex a, Vertex b) => a.Equals(b);

        public static bool operator !=(Vertex a, Vertex b) => !a.Equals(b);
    }
}