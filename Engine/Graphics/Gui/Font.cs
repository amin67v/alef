using System;
using System.Runtime.InteropServices;
using System.Numerics;

namespace Engine
{
    unsafe class ImFont
    {
        public ImFont(NativeFont* nativePtr)
        {
            NativeFont = nativePtr;
        }

        public NativeFont* NativeFont { get; }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NativeFont
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Glyph
        {
            public ushort Codepoint;
            public float XAdvance;
            public float X0, Y0, X1, Y1;
            public float U0, V0, U1, V1;     // Texture coordinates
        };

        public float FontSize;
        public float Scale;
        public Vector2 DisplayOffset;
        public ImVector Glyphs;
        public ImVector IndexXAdvance;
        public ImVector IndexLookup;
        public Glyph* FallbackGlyph;
        public float FallbackXAdvance;
        public ushort FallbackChar;
        public int ConfigDataCount;
        public IntPtr ConfigData;
        public IntPtr ContainerAtlas;
        public float Ascent, Descent;
    };
}
