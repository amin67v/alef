using System;
using System.Numerics;
using System.Runtime.InteropServices;
namespace Engine
{
    unsafe class IO
    {
        private NativeIO* _nativePtr;

        internal IO(NativeIO* nativePtr)
        {
            _nativePtr = nativePtr;
            MouseDown = new MouseDownStates(nativePtr);
            KeyMap = new KeyMap(_nativePtr);
            KeysDown = new KeyDownStates(_nativePtr);
            FontAtlas = new FontAtlas(_nativePtr->FontAtlas);
        }

        public NativeIO* GetNativePointer() => _nativePtr;

        public Vector2 DisplaySize
        {
            get { return _nativePtr->DisplaySize; }
            set { _nativePtr->DisplaySize = value; }
        }

        public float FontGlobalScale
        {
            get => _nativePtr->FontGlobalScale;
            set => _nativePtr->FontGlobalScale = value;
        }

        public float DeltaTime
        {
            get { return _nativePtr->DeltaTime; }
            set { _nativePtr->DeltaTime = value; }
        }

        public float IniSavingRate
        {
            get { return _nativePtr->IniSavingRate; }
            set { _nativePtr->IniSavingRate = value; }
        }

        public Vector2 DisplayFramebufferScale
        {
            get { return _nativePtr->DisplayFramebufferScale; }
            set { _nativePtr->DisplayFramebufferScale = value; }
        }

        public Vector2 MousePosition
        {
            get { return _nativePtr->MousePos; }
            set { _nativePtr->MousePos = value; }
        }

        public float MouseWheel
        {
            get { return _nativePtr->MouseWheel; }
            set { _nativePtr->MouseWheel = value; }
        }

        public MouseDownStates MouseDown { get; }
        public KeyMap KeyMap { get; }
        public KeyDownStates KeysDown { get; }
        public FontAtlas FontAtlas { get; }

        public bool FontAllowUserScaling
        {
            get { return _nativePtr->FontAllowUserScaling != 0; }
            set { _nativePtr->FontAllowUserScaling = value ? (byte)1 : (byte)0; }
        }

        public bool CtrlPressed
        {
            get { return _nativePtr->KeyCtrl == 1; }
            set { _nativePtr->KeyCtrl = value ? (byte)1 : (byte)0; }
        }

        public bool ShiftPressed
        {
            get { return _nativePtr->KeyShift == 1; }
            set { _nativePtr->KeyShift = value ? (byte)1 : (byte)0; }
        }

        public bool AltPressed
        {
            get { return _nativePtr->KeyAlt == 1; }
            set { _nativePtr->KeyAlt = value ? (byte)1 : (byte)0; }
        }
    }

    unsafe class KeyMap
    {
        private readonly NativeIO* _nativePtr;

        public KeyMap(NativeIO* nativePtr)
        {
            _nativePtr = nativePtr;
        }

        public int this[GuiKey key]
        {
            get
            {
                return _nativePtr->KeyMap[(int)key];
            }
            set
            {
                _nativePtr->KeyMap[(int)key] = value;
            }
        }
    }

    unsafe class MouseDownStates
    {
        private readonly NativeIO* _nativePtr;

        public MouseDownStates(NativeIO* nativePtr)
        {
            _nativePtr = nativePtr;
        }

        public bool this[int button]
        {
            get
            {
                if (button < 0 || button > 5)
                {
                    throw new ArgumentOutOfRangeException(nameof(button));
                }
                return _nativePtr->MouseDown[button] == 1;
            }
            set
            {
                if (button < 0 || button > 5)
                {
                    throw new ArgumentOutOfRangeException(nameof(button));
                }
                byte pressed = value ? (byte)1 : (byte)0;
                _nativePtr->MouseDown[button] = pressed;
            }
        }
    }

    unsafe class KeyDownStates
    {
        private readonly NativeIO* _nativePtr;

        public KeyDownStates(NativeIO* nativePtr)
        {
            _nativePtr = nativePtr;
        }

        public bool this[int key]
        {
            get
            {
                if (key < 0 || key > 512)
                {
                    throw new ArgumentOutOfRangeException(nameof(key));
                }
                return _nativePtr->KeysDown[key] == 1;
            }
            set
            {
                if (key < 0 || key > 512)
                {
                    throw new ArgumentOutOfRangeException(nameof(key));
                }
                byte pressed = value ? (byte)1 : (byte)0;
                _nativePtr->KeysDown[key] = pressed;
            }
        }
    }

    unsafe class FontAtlas
    {
        private readonly NativeFontAtlas* _atlasPtr;

        public FontAtlas(NativeFontAtlas* atlasPtr)
        {
            _atlasPtr = atlasPtr;
        }

        public FontTextureData GetTexDataAsAlpha8()
        {
            byte* pixels;
            int width, height;
            int bytesPerPixel;
            ImGui.ImFontAtlas_GetTexDataAsAlpha8(_atlasPtr, &pixels, &width, &height, &bytesPerPixel);
            return new FontTextureData(pixels, width, height, bytesPerPixel);
        }
        public FontTextureData GetTexDataAsRGBA32()
        {
            byte* pixels;
            int width, height;
            int bytesPerPixel;
            ImGui.ImFontAtlas_GetTexDataAsRGBA32(_atlasPtr, &pixels, &width, &height, &bytesPerPixel);
            return new FontTextureData(pixels, width, height, bytesPerPixel);
        }
        public void SetTexID(int textureID)
        {
            SetTexID(new IntPtr(textureID));
        }
        public void SetTexID(IntPtr textureID)
        {
            ImGui.ImFontAtlas_SetTexID(_atlasPtr, textureID.ToPointer());
        }
        public void Clear()
        {
            ImGui.ImFontAtlas_Clear(_atlasPtr);
        }
        public void ClearTexData()
        {
            ImGui.ImFontAtlas_ClearTexData(_atlasPtr);
        }
        public ImFont AddDefaultFont()
        {
            NativeFont* nativeFontPtr = ImGui.ImFontAtlas_AddFontDefault(_atlasPtr);
            return new ImFont(nativeFontPtr);
        }
        public ImFont AddFontFromFileTTF(string fileName, float pixelSize)
        {
            return new ImFont(ImGui.ImFontAtlas_AddFontFromFileTTF(_atlasPtr, fileName, pixelSize, IntPtr.Zero, null));
        }
        public ImFont AddFontFromMemoryTTF(IntPtr ttfData, int ttfDataSize, float pixelSize)
        {
            NativeFont* nativeFontPtr = ImGui.ImFontAtlas_AddFontFromMemoryTTF(_atlasPtr, ttfData.ToPointer(), ttfDataSize, pixelSize, IntPtr.Zero, null);
            return new ImFont(nativeFontPtr);
        }
        public ImFont AddFontFromMemoryTTF(IntPtr ttfData, int ttfDataSize, float pixelSize, IntPtr fontConfig)
        {
            NativeFont* nativeFontPtr = ImGui.ImFontAtlas_AddFontFromMemoryTTF(_atlasPtr, ttfData.ToPointer(), ttfDataSize, pixelSize, fontConfig, null);
            return new ImFont(nativeFontPtr);
        }
    }

    unsafe struct FontTextureData
    {
        public readonly byte* Pixels;
        public readonly int Width;
        public readonly int Height;
        public readonly int BytesPerPixel;
        public FontTextureData(byte* pixels, int width, int height, int bytesPerPixel)
        {
            Pixels = pixels;
            Width = width;
            Height = height;
            BytesPerPixel = bytesPerPixel;
        }
    }
}
