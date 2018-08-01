using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    public class VertexFormat
    {
        static int[] VertexAttribSize = { 4, 8, 12, 16, 4 };

        VertexAttrib[] attribs;
        int size;

        public int AttribCount => attribs.Length;

        public int VertexSize => size;

        public VertexAttrib this[int index] => attribs[index];

        public VertexFormat(params VertexAttrib[] attribs)
        {
            this.attribs = attribs;
            for (int i = 0; i < attribs.Length; i++)
                size += VertexAttribSize[(int)attribs[i]];
        }
    }

    public enum VertexAttrib
    {
        Float,
        Vector2,
        Vector3,
        Vector4,
        Color
    }
}