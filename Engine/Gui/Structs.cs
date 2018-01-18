using System;
using System.Text;
using System.Buffers;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Int2
    {
        public readonly int X, Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Int3
    {
        public readonly int X, Y, Z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Int4
    {
        public readonly int X, Y, Z, W;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct DrawCmd
    {
        public uint ElemCount;
        public Vector4 ClipRect;
        public IntPtr TextureId;
        public IntPtr UserCallback;
        public IntPtr UserCallbackData;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct DrawData
    {
        public byte Valid;
        public NativeDrawList** CmdLists;
        public int CmdListsCount;
        public int TotalVtxCount;
        public int TotalIdxCount;
    }

    unsafe struct DrawList
    {
        readonly NativeDrawList* native_draw_list;

        public DrawList(NativeDrawList* nativeDrawList)
        {
            native_draw_list = nativeDrawList;
        }

        public static DrawList GetForCurrentWindow()
        {
            return new DrawList(ImGui.igGetWindowDrawList());
        }

        public void AddLine(Vector2 a, Vector2 b, uint color, float thickness)
        {
            ImGui.ImDrawList_AddLine(native_draw_list, a, b, color, thickness);
        }

        public void AddRect(Vector2 a, Vector2 b, uint color, float rounding, int rounding_corners, float thickness)
        {
            ImGui.ImDrawList_AddRect(native_draw_list, a, b, color, rounding, rounding_corners, thickness);
        }

        public void AddRectFilled(Vector2 a, Vector2 b, uint color, float rounding, int rounding_corners = ~0)
        {
            ImGui.ImDrawList_AddRectFilled(native_draw_list, a, b, color, rounding, rounding_corners);
        }

        public void AddRectFilledMultiColor(
            Vector2 a,
            Vector2 b,
            uint colorUpperLeft,
            uint colorUpperRight,
            uint colorBottomRight,
            uint colorBottomLeft)
        {
            ImGui.ImDrawList_AddRectFilledMultiColor(
                native_draw_list,
                a,
                b,
                colorUpperLeft,
                colorUpperRight,
                colorBottomRight,
                colorBottomLeft);
        }

        public void AddCircle(Vector2 center, float radius, uint color, int numSegments, float thickness)
        {
            ImGui.ImDrawList_AddCircle(native_draw_list, center, radius, color, numSegments, thickness);
        }

        public unsafe void AddText(Vector2 position, string text, uint color)
        {
            // Consider using stack allocation if a newer version of Encoding is used (with byte* overloads).
            int bytes = Encoding.UTF8.GetByteCount(text);
            byte[] tempBytes = ArrayPool<byte>.Shared.Rent(bytes);
            Encoding.UTF8.GetBytes(text, 0, text.Length, tempBytes, 0);
            fixed (byte* bytePtr = &tempBytes[0])
            {
                ImGui.ImDrawList_AddText(native_draw_list, position, color, bytePtr, bytePtr + bytes);
            }
            ArrayPool<byte>.Shared.Return(tempBytes);
        }

        public void PushClipRect(Vector2 min, Vector2 max, bool intersectWithCurrentClipRect)
        {
            ImGui.ImDrawList_PushClipRect(native_draw_list, min, max, intersectWithCurrentClipRect ? (byte)1 : (byte)0);
        }

        public void PushClipRectFullScreen()
        {
            ImGui.ImDrawList_PushClipRectFullScreen(native_draw_list);
        }

        public void PopClipRect()
        {
            ImGui.ImDrawList_PopClipRect(native_draw_list);
        }

        public void AddDrawCmd()
        {
            ImGui.ImDrawList_AddDrawCmd(native_draw_list);
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

    unsafe class ImFont
    {
        public ImFont(NativeFont* nativePtr)
        {
            NativeFont = nativePtr;
        }

        public NativeFont* NativeFont { get; }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NativeFont
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Glyph
        {
            public ushort Codepoint;
            public float XAdvance;
            public float X0, Y0, X1, Y1;
            public float U0, V0, U1, V1; // Texture coordinates
        };

        public float FontSize;
        public float Scale;
        public Vector2 DisplayOffset;
        public ImVector Glyphs;
        public ImVector IndexXAdvance;
        public ImVector IndexLookup;
        public Glyph* FallbackGlyph;
        public float FallbackXAdvance;
        public ushort FallbackChar;
        public int ConfigDataCount;
        public IntPtr ConfigData;
        public IntPtr ContainerAtlas;
        public float Ascent, Descent;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NativeFontAtlas
    {
        public void* TexID;
        public byte* TexPixelsAlpha8;
        public UIntPtr TexPixelsRGBA32;
        public IntPtr TexWidth;
        public IntPtr TexHeight;
        public IntPtr TexDesiredWidth;
        public Vector2 TexUvWhitePixel;
        public ImVector Fonts;
        public ImVector ConfigData;
    }

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
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct ImVector
    {
        public int Size;
        public int Capacity;
        public void* Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct Storage
    {
        /// <summary>
        /// A vector of Storage.Pair values.
        /// </summary>
        public ImVector Data;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Pair
        {
            public uint Key;
            private OverlappedDataItem _overlappedData;

            public float FloatData
            {
                get { return _overlappedData.FloatData; }
                set { _overlappedData.FloatData = value; }
            }

            public int IntData
            {
                get { return _overlappedData.IntData; }
                set { _overlappedData.IntData = value; }
            }

            public IntPtr PtrData
            {
                get { return _overlappedData.PtrData; }
                set { _overlappedData.PtrData = value; }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private unsafe struct OverlappedDataItem
        {
            [FieldOffset(0)]
            public float FloatData;
            [FieldOffset(0)]
            public int IntData;
            [FieldOffset(0)]
            public IntPtr PtrData;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NativeStyle
    {
        public float Alpha;
        public Vector2 WindowPadding;
        public Vector2 WindowMinSize;
        public float WindowRounding;
        public Vector2 WindowTitleAlign;
        public float ChildWindowRounding;
        public Vector2 FramePadding;
        public float FrameRounding;
        public Vector2 ItemSpacing;
        public Vector2 ItemInnerSpacing;
        public Vector2 TouchExtraPadding;
        public float IndentSpacing;
        public float ColumnsMinSpacing;
        public float ScrollbarSize;
        public float ScrollbarRounding;
        public float GrabMinSize;
        public float GrabRounding;
        public Vector2 ButtonTextAlign;
        public Vector2 DisplayWindowPadding;
        public Vector2 DisplaySafeAreaPadding;
        public byte AntiAliasedLines;
        public byte AntiAliasedShapes;
        public float CurveTessellationTol;
        public fixed float Colors[(int)ColorTarget.Count * 4];
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct NativeIO
    {
        public Vector2 DisplaySize;
        public float DeltaTime;
        public float IniSavingRate;
        public IntPtr IniFilename;
        public IntPtr LogFilename;
        public float MouseDoubleClickTime;
        public float MouseDoubleClickMaxDist;
        public float MouseDragThreshold;
        public fixed int KeyMap[(int)GuiKey.Count];
        public float KeyRepeatDelay;
        public float KeyRepeatRate;
        public IntPtr UserData;
        public NativeFontAtlas* FontAtlas;
        public float FontGlobalScale;
        public byte FontAllowUserScaling;
        public NativeFont* FontDefault;
        public Vector2 DisplayFramebufferScale;
        public Vector2 DisplayVisibleMin;
        public Vector2 DisplayVisibleMax;
        public byte OSXBehaviors;
        public IntPtr RenderDrawListsFn;
        public IntPtr GetClipboardTextFn;
        public IntPtr SetClipboardTextFn;
        public IntPtr ClipboardUserData;
        public IntPtr MemAllocFn;
        public IntPtr MemFreeFn;
        public IntPtr ImeSetInputScreenPosFn;
        public IntPtr ImeWindowHandle;
        public Vector2 MousePos;
        public fixed byte MouseDown[5];
        public float MouseWheel;
        public byte MouseDrawCursor;
        public byte KeyCtrl;
        public byte KeyShift;
        public byte KeyAlt;
        public byte KeySuper;
        public fixed byte KeysDown[512];
        public fixed ushort InputCharacters[16 + 1];
        public byte WantCaptureMouse;
        public byte WantCaptureKeyboard;
        public byte WantTextInput;
        public float Framerate;
        public int MetricsAllocs;
        public int MetricsRenderVertices;
        public int MetricsRenderIndices;
        public int MetricsActiveWindows;
        public Vector2 MouseDelta;
        public Vector2 MousePosPrev;
        public fixed byte MouseClicked[5];
        public Vector2 MouseClickedPos0;
        public Vector2 MouseClickedPos1;
        public Vector2 MouseClickedPos2;
        public Vector2 MouseClickedPos3;
        public Vector2 MouseClickedPos4;
        public fixed float MouseClickedTime[5];
        public fixed byte MouseDoubleClicked[5];
        public fixed byte MouseReleased[5];
        public fixed byte MouseDownOwned[5];
        public fixed float MouseDownDuration[5];
        public fixed float MouseDownDurationPrev[5];
        public fixed float MouseDragMaxDistanceSqr[5];
        public fixed float KeysDownDuration[512];
        public fixed float KeysDownDurationPrev[512];
    }
}