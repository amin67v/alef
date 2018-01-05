using System.Runtime.InteropServices;
using System;
using System.Numerics;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NativeFontAtlas
    {
        public void* TexID;
        public byte* TexPixelsAlpha8;
        public UIntPtr TexPixelsRGBA32;
        public IntPtr TexWidth;
        public IntPtr TexHeight;
        public IntPtr TexDesiredWidth;
        public Vector2 TexUvWhitePixel;
        public ImVector Fonts;
        public ImVector ConfigData;
    }
}
