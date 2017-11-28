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
    public static extern IntPtr LoadImage([In] [MarshalAs(UnmanagedType.LPStr)] string file, ref int w, ref int h, ref int comp, int reqComp);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_image_free")]
    public static extern void FreeImage(IntPtr data);

    public static void SetFlipVerticallyOnLoad(bool flip)
    {
        stbi_set_flip_vertically_on_load(flip ? 1 : 0);
    }

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    static extern void stbi_set_flip_vertically_on_load(int flip);
}
