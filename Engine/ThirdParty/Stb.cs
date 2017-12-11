using System;
using System.Runtime.InteropServices;

internal static class Stb
{
    const string lib = "stb";

    static Stb()
    {
        SetFlipVerticallyOnLoad(true);
    }

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load")]
    public static extern IntPtr LoadFromFile([In] [MarshalAs(UnmanagedType.LPStr)] string file, ref int w, ref int h, ref int comp, int reqComp);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_memory")]
    public static extern IntPtr LoadFromMemory([In] IntPtr buffer, int len, ref int w, ref int h, ref int comp, int reqComp);

    public static IntPtr LoadFromMemory(byte[] buffer, ref int w, ref int h, ref int comp, int reqComp)
    {
        var hdl = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var r = LoadFromMemory(hdl.AddrOfPinnedObject(), buffer.Length, ref w, ref h, ref comp, reqComp);
        hdl.Free();
        return r;
    }

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_image_free")]
    public static extern void FreeImage(IntPtr data);

    public static void SetFlipVerticallyOnLoad(bool flip)
    {
        stbi_set_flip_vertically_on_load(flip ? 1 : 0);
    }

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    static extern void stbi_set_flip_vertically_on_load(int flip);
}
