using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct ImVector
    {
        public int Size;
        public int Capacity;
        public void* Data;
    }
}
