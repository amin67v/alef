using System;
using System.Buffers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Engine
{
    unsafe struct DrawList
    {
        private readonly NativeDrawList* _nativeDrawList;
        public DrawList(NativeDrawList* nativeDrawList)
        {
            _nativeDrawList = nativeDrawList;
        }

        public static DrawList GetForCurrentWindow()
        {
            return new DrawList(ImGuiNative.igGetWindowDrawList());
        }

        public void AddLine(Vector2 a, Vector2 b, uint color, float thickness)
        {
            ImGuiNative.ImDrawList_AddLine(_nativeDrawList, a, b, color, thickness);
        }

        public void AddRect(Vector2 a, Vector2 b, uint color, float rounding, int rounding_corners, float thickness)
        {
            ImGuiNative.ImDrawList_AddRect(_nativeDrawList, a, b, color, rounding, rounding_corners, thickness);
        }

        public void AddRectFilled(Vector2 a, Vector2 b, uint color, float rounding, int rounding_corners = ~0)
        {
            ImGuiNative.ImDrawList_AddRectFilled(_nativeDrawList, a, b, color, rounding, rounding_corners);
        }

        public void AddRectFilledMultiColor(
            Vector2 a,
            Vector2 b,
            uint colorUpperLeft,
            uint colorUpperRight,
            uint colorBottomRight,
            uint colorBottomLeft)
        {
            ImGuiNative.ImDrawList_AddRectFilledMultiColor(
                _nativeDrawList,
                a,
                b,
                colorUpperLeft,
                colorUpperRight,
                colorBottomRight,
                colorBottomLeft);
        }

        public void AddCircle(Vector2 center, float radius, uint color, int numSegments, float thickness)
        {
            ImGuiNative.ImDrawList_AddCircle(_nativeDrawList, center, radius, color, numSegments, thickness);
        }

        public unsafe void AddText(Vector2 position, string text, uint color)
        {
            // Consider using stack allocation if a newer version of Encoding is used (with byte* overloads).
            int bytes = Encoding.UTF8.GetByteCount(text);
            byte[] tempBytes = ArrayPool<byte>.Shared.Rent(bytes);
            Encoding.UTF8.GetBytes(text, 0, text.Length, tempBytes, 0);
            fixed (byte* bytePtr = &tempBytes[0])
            {
                ImGuiNative.ImDrawList_AddText(_nativeDrawList, position, color, bytePtr, bytePtr + bytes);
            }
            ArrayPool<byte>.Shared.Return(tempBytes);
        }

        public void PushClipRect(Vector2 min, Vector2 max, bool intersectWithCurrentClipRect)
        {
            ImGuiNative.ImDrawList_PushClipRect(_nativeDrawList, min, max, intersectWithCurrentClipRect ? (byte)1 : (byte)0);
        }

        public void PushClipRectFullScreen()
        {
            ImGuiNative.ImDrawList_PushClipRectFullScreen(_nativeDrawList);
        }

        public void PopClipRect()
        {
            ImGuiNative.ImDrawList_PopClipRect(_nativeDrawList);
        }

        public void AddDrawCmd()
        {
            ImGuiNative.ImDrawList_AddDrawCmd(_nativeDrawList);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NativeDrawList
    {
        public ImVector CmdBuffer;
        public ImVector IdxBuffer;
        public ImVector VtxBuffer;
        public IntPtr _OwnerName;
        public uint _VtxCurrentIdx;
        public IntPtr _VtxWritePtr;
        public IntPtr _IdxWritePtr;
        public ImVector _ClipRectStack;
        public ImVector _TextureIdStack;
        public ImVector _Path;
        public int _ChannelsCurrent;
        public int _ChannelsCount;
        public ImVector _Channels;
    }
}
