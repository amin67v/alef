using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2D : IEquatable<Vertex2D>
    {
        public static readonly VertexFormat Format = 
            new VertexFormat(VertexAttrib.Vector2, VertexAttrib.Vector2, VertexAttrib.Color);

        public Vector2 Position;
        public Vector2 Texcoord;
        public Color Color;

        public override bool Equals(object obj)
        {
            if (obj is Vertex2D)
                return Equals((Vertex2D)obj);
            else
                return false;
        }

        public bool Equals(Vertex2D other)
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

        public static bool operator ==(Vertex2D a, Vertex2D b) => a.Equals(b);

        public static bool operator !=(Vertex2D a, Vertex2D b) => !a.Equals(b);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex : IEquatable<Vertex>
    {
        public static readonly VertexFormat Format = 
            new VertexFormat(VertexAttrib.Vector3, VertexAttrib.Vector3, VertexAttrib.Vector3, VertexAttrib.Vector3);

        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector3 Texcoord;

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
                   Normal == other.Normal &&
                   Texcoord == other.Texcoord;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int prime = 486187739;
                int hash = prime;
                hash = (hash + Position.GetHashCode()) * prime;
                hash = (hash + Normal.GetHashCode()) * prime;
                hash = (hash + Texcoord.GetHashCode()) * prime;
                return hash;
            }
        }

        public static bool operator ==(Vertex a, Vertex b) => a.Equals(b);

        public static bool operator !=(Vertex a, Vertex b) => !a.Equals(b);
    }
}