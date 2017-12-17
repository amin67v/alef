using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
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
    }
}