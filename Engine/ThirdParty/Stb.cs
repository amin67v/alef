using System;
using System.Runtime.InteropServices;

namespace Engine
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal static unsafe class Stb
    {
        const string lib = "stb";

        static Stb()
        {
            stbi_set_flip_vertically_on_load(1);
        }

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* stbi_load_from_memory([In] IntPtr buffer, int len, ref int w, ref int h, ref int comp, int req_comp);

        public static Color* stbi_load_from_memory(byte[] buffer, ref int w, ref int h)
        {
            var hdl = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            int comp = 0;
            var r = stbi_load_from_memory(hdl.AddrOfPinnedObject(), buffer.Length, ref w, ref h, ref comp, 4);
            hdl.Free();
            return (Color*)r;
        }

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void stbi_image_free(void* data);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int stbi_write_png([In] [MarshalAs(UnmanagedType.LPStr)] string filename, int w, int h, int comp, [In] void* data, int stride_in_bytes);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int stbi_write_tga([In] [MarshalAs(UnmanagedType.LPStr)] string filename, int w, int h, int comp, [In] void* data);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int stbi_write_jpg([In] [MarshalAs(UnmanagedType.LPStr)] string filename, int w, int h, int comp, [In] void* data, int quality);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void stbi_set_flip_vertically_on_load(int flip);

    }
}