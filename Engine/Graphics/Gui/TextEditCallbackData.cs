using System;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TextEditCallbackData
    {
        public InputTextFlags EventFlag;
        public InputTextFlags Flags;
        public IntPtr UserData;
        private byte _ReadOnly;
        public bool ReadOnly { get { return _ReadOnly == 1; } }
        public ushort EventChar;
        public GuiKey EventKey;
        public IntPtr Buf;
        public int BufTextLen;
        public int BufSize;
        public byte BufDirty;
        public int CursorPos;
        public int SelectionStart;
        public int SelectionEnd;
        public bool HasSelection() { return SelectionStart != SelectionEnd; }
    }
}
