using System;
using System.Security;
using System.Numerics;
using System.Runtime.InteropServices;

[SuppressUnmanagedCodeSecurity]
internal unsafe static class CSFML
{
    const string system_lib = "csfml-system-2";
    const string window_lib = "csfml-window-2";

    /*****************************************************************
     *                          System                               *
     *****************************************************************/

    [DllImport(system_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr sfClock_create();

    [DllImport(system_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfClock_destroy(IntPtr ptr);

    [DllImport(system_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern long sfClock_getElapsedTime(IntPtr ptr);

    [DllImport(system_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern long sfClock_restart(IntPtr ptr);


    /*****************************************************************
     *                          window                               *
     *****************************************************************/
    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr sfWindow_create(VideoMode mode, [MarshalAs(UnmanagedType.LPStr)] string title, Styles style, ref ContextSettings @params);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern VideoMode sfVideoMode_getDesktopMode();

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool sfWindow_isOpen(IntPtr ptr);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_destroy(IntPtr ptr);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_close(IntPtr ptr);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool sfWindow_pollEvent(IntPtr ptr, out Event e);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_display(IntPtr ptr);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern Vec2u sfWindow_getSize(IntPtr ptr);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setSize(IntPtr ptr, Vec2u size);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setTitle(IntPtr ptr, string title);

    public static void sfWindow_setIcon(IntPtr ptr, uint width, uint height, byte[] pixels)
    {
        fixed (byte* pixel_ptr = pixels)
        {
            sfWindow_setIcon(ptr, width, height, pixel_ptr);
        }
    }

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setIcon(IntPtr ptr, uint width, uint height, byte* pixels);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setMouseCursorVisible(IntPtr ptr, bool show);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setMouseCursorGrabbed(IntPtr ptr, bool grabbed);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setVerticalSyncEnabled(IntPtr ptr, bool enable);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setKeyRepeatEnabled(IntPtr ptr, bool enable);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfWindow_setJoystickThreshold(IntPtr ptr, float threshold);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool sfWindow_hasFocus(IntPtr ptr);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern Vec2i sfMouse_getPosition(IntPtr ptr);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void sfMouse_setPosition(Vec2i position, IntPtr ptr);

    public static VideoMode[] sfVideoMode_getFullscreenModes()
    {
        uint count;
        VideoMode* modes_ptr = sfVideoMode_getFullscreenModes(out count);
        VideoMode[] modes = new VideoMode[count];
        for (uint i = 0; i < count; ++i)
            modes[i] = modes_ptr[i];

        return modes;
    }

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern VideoMode* sfVideoMode_getFullscreenModes(out uint count);

    [DllImport(window_lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool sfVideoMode_isValid(VideoMode mode);


    /*****************************************************************
     *                          Types                                *
     *****************************************************************/
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoMode
    {
        public uint Width;
        public uint Height;
        public uint BitsPerPixel;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ContextSettings
    {
        public uint DepthBits;
        public uint StencilBits;
        public uint AntialiasingLevel;
        public uint MajorVersion;
        public uint MinorVersion;
        public uint AttributeFlags;
        public bool SRgbCapable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vec2u
    {
        public uint X, Y;

        public Vec2u(uint x, uint y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vec2i
    {
        public int X, Y;

        public Vec2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct Event
    {
        [FieldOffset(0)]
        public EventType Type;
        [FieldOffset(4)]
        public SizeEvent Size;
        [FieldOffset(4)]
        public KeyEvent Key;
        [FieldOffset(4)]
        public TextEvent Text;
        [FieldOffset(4)]
        public MouseMoveEvent MouseMove;
        [FieldOffset(4)]
        public MouseButtonEvent MouseButton;
        [FieldOffset(4)]
        public MouseWheelScrollEvent MouseWheelScroll;
        [FieldOffset(4)]
        public JoystickMoveEvent JoystickMove;
        [FieldOffset(4)]
        public JoystickButtonEvent JoystickButton;
        [FieldOffset(4)]
        public JoystickConnectEvent JoystickConnect;
        [FieldOffset(4)]
        public TouchEvent Touch;
        [FieldOffset(4)]
        public SensorEvent Sensor;
    }

    public enum EventType
    {
        Closed,
        Resized,
        LostFocus,
        GainedFocus,
        TextEntered,
        KeyPressed,
        KeyReleased,
        MouseWheelMoved, // deprecated
        MouseWheelScrolled,
        MouseButtonPressed,
        MouseButtonReleased,
        MouseMoved,
        MouseEntered,
        MouseLeft,
        JoystickButtonPressed,
        JoystickButtonReleased,
        JoystickMoved,
        JoystickConnected,
        JoystickDisconnected,
        TouchBegan,
        TouchMoved,
        TouchEnded,
        SensorChanged
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyEvent
    {
        public int Code;
        public int Alt;
        public int Control;
        public int Shift;
        public int System;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TextEvent
    {
        public uint Unicode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseMoveEvent
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseButtonEvent
    {
        public int Button;
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseWheelScrollEvent
    {
        public int Wheel;
        public float Delta;
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JoystickMoveEvent
    {
        public uint JoystickId;
        public int Axis;
        public float Position;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JoystickButtonEvent
    {
        public uint JoystickId;
        public uint Button;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JoystickConnectEvent
    {
        public uint JoystickId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SizeEvent
    {
        public uint Width;
        public uint Height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TouchEvent
    {
        public uint Finger;
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SensorEvent
    {
        public SensorType Type;
        public float X;
        public float Y;
        public float Z;
    }

    public enum Styles
    {
        None = 0,
        Titlebar = 1 << 0,
        Resize = 1 << 1,
        Close = 1 << 2,
        Fullscreen = 1 << 3,
        Default = Titlebar | Resize | Close
    }

    public enum SensorType
    {
        Accelerometer,
        Gyroscope,
        Magnetometer,
        Gravity,
        UserAcceleration,
        Orientation,
        TypeCount
    };
}
