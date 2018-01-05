using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct DrawCmd
    {
        public uint ElemCount;
        public Vector4 ClipRect;
        public IntPtr TextureId;
        public IntPtr UserCallback;
        public IntPtr UserCallbackData;
    };
}
