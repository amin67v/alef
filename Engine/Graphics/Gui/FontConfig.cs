using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct FontConfig
    {
        public IntPtr FontData;
        public int FontDataSize;
        [MarshalAs(UnmanagedType.I1)]
        public bool FontDataOwnedByAtlas;
        public int FontNo;
        public float SizePixels;
        public int OversampleH;
        public int OversampleV;
        [MarshalAs(UnmanagedType.I1)]
        public bool PixelSnapH;
        public Vector2 GlyphExtraSpacing;
        public Vector2 GlyphOffset;
        public IntPtr GlyphRanges;
        [MarshalAs(UnmanagedType.I1)]
        public bool MergeMode;
        public fixed byte Name[32];
        public IntPtr DstFont;
    };
}
