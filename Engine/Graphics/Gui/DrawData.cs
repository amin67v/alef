using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct DrawData
    {
        public byte Valid;
        public NativeDrawList** CmdLists;
        public int CmdListsCount;
        public int TotalVtxCount;
        public int TotalIdxCount;
    };
}
