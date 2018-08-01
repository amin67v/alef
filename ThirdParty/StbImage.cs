using System;
using System.Runtime.InteropServices;

[System.Security.SuppressUnmanagedCodeSecurity]
public unsafe static class StbImage
{
    const string lib = "stb_image";

    // stb_image
    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void* stbi_load_from_memory([In] IntPtr buffer, int len, ref int w, ref int h, ref int comp, int req_comp);

    public static void* stbi_load_from_memory(byte[] buffer, ref int w, ref int h)
    {
        var hdl = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        int comp = 0;
        var r = stbi_load_from_memory(hdl.AddrOfPinnedObject(), buffer.Length, ref w, ref h, ref comp, 4);
        hdl.Free();
        return r;
    }

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void stbi_image_free(void* data);

    // stb_image_write
    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int stbi_write_png([In] [MarshalAs(UnmanagedType.LPStr)] string filename, int w, int h, int comp, [In] void* data, int stride_in_bytes);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int stbi_write_tga([In] [MarshalAs(UnmanagedType.LPStr)] string filename, int w, int h, int comp, [In] void* data);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int stbi_write_jpg([In] [MarshalAs(UnmanagedType.LPStr)] string filename, int w, int h, int comp, [In] void* data, int quality);
}
