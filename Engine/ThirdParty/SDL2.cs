using System;
using System.Security;
using System.Runtime.InteropServices;

[SuppressUnmanagedCodeSecurity]
unsafe static class Sdl2
{
    const string lib = "SDL2";

    public const uint SDL_INIT_TIMER = 0x00000001u;
    public const uint SDL_INIT_AUDIO = 0x00000010u;
    public const uint SDL_INIT_VIDEO = 0x00000020u;
    public const uint SDL_INIT_JOYSTICK = 0x00000200u;
    public const uint SDL_INIT_HAPTIC = 0x00001000u;
    public const uint SDL_INIT_GAMECONTROLLER = 0x00002000u;
    public const uint SDL_INIT_EVENTS = 0x00004000u;
    public const uint SDL_INIT_NOPARACHUTE = 0x00100000u;

    public const int SDL_WINDOWPOS_CENTERED = 0x2FFF0000;

    public enum SDL_WindowFlags : uint
    {
        SDL_WINDOW_FULLSCREEN = 0x00000001,
        SDL_WINDOW_OPENGL = 0x00000002,
        SDL_WINDOW_SHOWN = 0x00000004,
        SDL_WINDOW_HIDDEN = 0x00000008,
        SDL_WINDOW_BORDERLESS = 0x00000010,
        SDL_WINDOW_RESIZABLE = 0x00000020,
        SDL_WINDOW_MINIMIZED = 0x00000040,
        SDL_WINDOW_MAXIMIZED = 0x00000080,
        SDL_WINDOW_INPUT_GRABBED = 0x00000100,
        SDL_WINDOW_INPUT_FOCUS = 0x00000200,
        SDL_WINDOW_MOUSE_FOCUS = 0x00000400,
        SDL_WINDOW_FULLSCREEN_DESKTOP = (SDL_WINDOW_FULLSCREEN | 0x00001000),
        SDL_WINDOW_FOREIGN = 0x00000800,
        SDL_WINDOW_ALLOW_HIGHDPI = 0x00002000,
        SDL_WINDOW_MOUSE_CAPTURE = 0x00004000,
        SDL_WINDOW_ALWAYS_ON_TOP = 0x00008000,
        SDL_WINDOW_SKIP_TASKBAR = 0x00010000,
        SDL_WINDOW_UTILITY = 0x00020000,
        SDL_WINDOW_TOOLTIP = 0x00040000,
        SDL_WINDOW_POPUP_MENU = 0x00080000,
        SDL_WINDOW_VULKAN = 0x10000000
    }

    public enum SDL_GLattr
    {
        SDL_GL_RED_SIZE,
        SDL_GL_GREEN_SIZE,
        SDL_GL_BLUE_SIZE,
        SDL_GL_ALPHA_SIZE,
        SDL_GL_BUFFER_SIZE,
        SDL_GL_DOUBLEBUFFER,
        SDL_GL_DEPTH_SIZE,
        SDL_GL_STENCIL_SIZE,
        SDL_GL_ACCUM_RED_SIZE,
        SDL_GL_ACCUM_GREEN_SIZE,
        SDL_GL_ACCUM_BLUE_SIZE,
        SDL_GL_ACCUM_ALPHA_SIZE,
        SDL_GL_STEREO,
        SDL_GL_MULTISAMPLEBUFFERS,
        SDL_GL_MULTISAMPLESAMPLES,
        SDL_GL_ACCELERATED_VISUAL,
        SDL_GL_RETAINED_BACKING,
        SDL_GL_CONTEXT_MAJOR_VERSION,
        SDL_GL_CONTEXT_MINOR_VERSION,
        SDL_GL_CONTEXT_EGL,
        SDL_GL_CONTEXT_FLAGS,
        SDL_GL_CONTEXT_PROFILE_MASK,
        SDL_GL_SHARE_WITH_CURRENT_CONTEXT,
        SDL_GL_FRAMEBUFFER_SRGB_CAPABLE,
        SDL_GL_CONTEXT_RELEASE_BEHAVIOR,
        SDL_GL_CONTEXT_RESET_NOTIFICATION,
        SDL_GL_CONTEXT_NO_ERROR
    }

    public enum SDL_EventType : uint
    {
        SDL_FIRSTEVENT = 0,
        SDL_QUIT = 0x100,
        SDL_WINDOWEVENT = 0x200,
        SDL_SYSWMEVENT,
        SDL_KEYDOWN = 0x300,
        SDL_KEYUP,
        SDL_TEXTEDITING,
        SDL_TEXTINPUT,
        SDL_MOUSEMOTION = 0x400,
        SDL_MOUSEBUTTONDOWN,
        SDL_MOUSEBUTTONUP,
        SDL_MOUSEWHEEL,
        SDL_JOYAXISMOTION = 0x600,
        SDL_JOYBALLMOTION,
        SDL_JOYHATMOTION,
        SDL_JOYBUTTONDOWN,
        SDL_JOYBUTTONUP,
        SDL_JOYDEVICEADDED,
        SDL_JOYDEVICEREMOVED,
        SDL_CONTROLLERAXISMOTION = 0x650,
        SDL_CONTROLLERBUTTONDOWN,
        SDL_CONTROLLERBUTTONUP,
        SDL_CONTROLLERDEVICEADDED,
        SDL_CONTROLLERDEVICEREMOVED,
        SDL_CONTROLLERDEVICEREMAPPED,
        SDL_FINGERDOWN = 0x700,
        SDL_FINGERUP,
        SDL_FINGERMOTION,
        SDL_DOLLARGESTURE = 0x800,
        SDL_DOLLARRECORD,
        SDL_MULTIGESTURE,
        SDL_CLIPBOARDUPDATE = 0x900,
        SDL_DROPFILE = 0x1000,
        SDL_DROPTEXT,
        SDL_DROPBEGIN,
        SDL_DROPCOMPLETE,
        SDL_AUDIODEVICEADDED = 0x1100,
        SDL_AUDIODEVICEREMOVED,
        SDL_RENDER_TARGETS_RESET = 0x2000,
        SDL_RENDER_DEVICE_RESET,
        SDL_USEREVENT = 0x8000,
        SDL_LASTEVENT = 0xFFFF
    }

    public enum SDL_WindowEventID : byte
    {
        SDL_WINDOWEVENT_NONE,
        SDL_WINDOWEVENT_SHOWN,
        SDL_WINDOWEVENT_HIDDEN,
        SDL_WINDOWEVENT_EXPOSED,
        SDL_WINDOWEVENT_MOVED,
        SDL_WINDOWEVENT_RESIZED,
        SDL_WINDOWEVENT_SIZE_CHANGED,
        SDL_WINDOWEVENT_MINIMIZED,
        SDL_WINDOWEVENT_MAXIMIZED,
        SDL_WINDOWEVENT_RESTORED,
        SDL_WINDOWEVENT_ENTER,
        SDL_WINDOWEVENT_LEAVE,
        SDL_WINDOWEVENT_FOCUS_GAINED,
        SDL_WINDOWEVENT_FOCUS_LOST,
        SDL_WINDOWEVENT_CLOSE,
        SDL_WINDOWEVENT_TAKE_FOCUS,
        SDL_WINDOWEVENT_HIT_TEST
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_WindowEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public SDL_WindowEventID @event;
        public byte padding1;
        public byte padding2;
        public byte padding3;
        public int data1;
        public int data2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Keysym
    {
        public int scancode;
        public int sym;
        public ushort mod;
        public uint unicode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_KeyboardEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public byte state;
        public byte repeat;
        public byte padding2;
        public byte padding3;
        public SDL_Keysym keysym;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_TextEditingEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public fixed byte text[32];
        public int start;
        public int length;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_TextInputEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public fixed byte text[32];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_MouseMotionEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public uint which;
        public byte state;
        public byte padding1;
        public byte padding2;
        public byte padding3;
        public int x;
        public int y;
        public int xrel;
        public int yrel;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_MouseButtonEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public uint which;
        public byte button;
        public byte state;
        public byte clicks;
        public byte padding1;
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_MouseWheelEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public uint which;
        public int x;
        public int y;
        public uint direction;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_JoyAxisEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
        public byte axis;
        public byte padding1;
        public byte padding2;
        public byte padding3;
        public short @value;
        public ushort padding4;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_JoyBallEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
        public byte ball;
        public byte padding1;
        public byte padding2;
        public byte padding3;
        public short xrel;
        public short yrel;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_JoyHatEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
        public byte hat;
        public byte @Value;
        public byte padding1;
        public byte padding2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_JoyButtonEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
        public byte button;
        public byte state;
        public byte padding1;
        public byte padding2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_JoyDeviceEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_ControllerAxisEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
        public byte axis;
        public byte padding1;
        public byte padding2;
        public byte padding3;
        public short @Value;
        public ushort padding4;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_ControllerButtonEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
        public byte button;
        public byte state;
        public byte padding1;
        public byte padding2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_ControllerDeviceEvent
    {
        public uint type;
        public uint timestamp;
        public int which;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_AudioDeviceEvent
    {
        public uint type;
        public uint timestamp;
        public uint which;
        public byte iscapture;
        public byte padding1;
        public byte padding2;
        public byte padding3;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_QuitEvent
    {
        public uint type;
        public uint timestamp;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_UserEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public int code;
        public IntPtr data1;
        public IntPtr data2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMEvent
    {
        public uint type;
        public uint timestamp;
        public IntPtr msg;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_TouchFingerEvent
    {
        public uint type;
        public uint timestamp;
        public long touchId;
        public long fingerId;
        public float x;
        public float y;
        public float dx;
        public float dy;
        public float pressure;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_MultiGestureEvent
    {
        public uint type;
        public uint timestamp;
        public long touchId;
        public float dTheta;
        public float dDist;
        public float x;
        public float y;
        public ushort numFingers;
        public ushort padding;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_DollarGestureEvent
    {
        public uint type;
        public uint timestamp;
        public long touchId;
        public long gestureId;
        public uint numFingers;
        public float error;
        public float x;
        public float y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_DropEvent
    {
        public uint type;
        public uint timestamp;
        public IntPtr file;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct SDL_Event
    {
        [FieldOffset(0)]
        public SDL_EventType type;
        [FieldOffset(0)]
        public SDL_WindowEvent window;
        [FieldOffset(0)]
        public SDL_KeyboardEvent key;
        [FieldOffset(0)]
        public SDL_TextEditingEvent edit;
        [FieldOffset(0)]
        public SDL_TextInputEvent text;
        [FieldOffset(0)]
        public SDL_MouseMotionEvent motion;
        [FieldOffset(0)]
        public SDL_MouseButtonEvent button;
        [FieldOffset(0)]
        public SDL_MouseWheelEvent wheel;
        [FieldOffset(0)]
        public SDL_JoyAxisEvent jaxis;
        [FieldOffset(0)]
        public SDL_JoyBallEvent jball;
        [FieldOffset(0)]
        public SDL_JoyHatEvent jhat;
        [FieldOffset(0)]
        public SDL_JoyButtonEvent jbutton;
        [FieldOffset(0)]
        public SDL_JoyDeviceEvent jdevice;
        [FieldOffset(0)]
        public SDL_ControllerAxisEvent caxis;
        [FieldOffset(0)]
        public SDL_ControllerButtonEvent cbutton;
        [FieldOffset(0)]
        public SDL_ControllerDeviceEvent cdevice;
        [FieldOffset(0)]
        public SDL_AudioDeviceEvent adevice;
        [FieldOffset(0)]
        public SDL_QuitEvent quit;
        [FieldOffset(0)]
        public SDL_UserEvent user;
        [FieldOffset(0)]
        public SDL_SysWMEvent syswm;
        [FieldOffset(0)]
        public SDL_TouchFingerEvent tfinger;
        [FieldOffset(0)]
        public SDL_MultiGestureEvent mgesture;
        [FieldOffset(0)]
        public SDL_DollarGestureEvent dgesture;
        [FieldOffset(0)]
        public SDL_DropEvent drop;
    }

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_SetMainReady();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_Init(uint flags);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_Quit();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_malloc(IntPtr size);
    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_free(IntPtr memblock);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_SetHint(string name, string value);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_GL_SetAttribute(SDL_GLattr attr, int value);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_CreateWindow(string title, int x, int y, int w, int h, SDL_WindowFlags flags);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_DestroyWindow(IntPtr window);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_GL_CreateContext(IntPtr window);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_GL_DeleteContext(IntPtr context);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_GL_MakeCurrent(IntPtr window, IntPtr context);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_GL_SetSwapInterval(int interval);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_SetWindowTitle(IntPtr window, string title);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_SetWindowSize(IntPtr window, int w, int h);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_WarpMouseInWindow(IntPtr window, int x, int y);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_CreateRGBSurfaceFrom(IntPtr pixels, int width, int height, int depth, int pitch,
                                                         uint Rmask, uint Gmask, uint Bmask, uint Amask);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_FreeSurface(IntPtr surface);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_SetWindowIcon(IntPtr window, IntPtr icon);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_CreateColorCursor(IntPtr surface, int hot_x, int hot_y);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_SetCursor(IntPtr cursor);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_FreeCursor(IntPtr cursor);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_GL_SwapWindow(IntPtr window);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_PollEvent(out SDL_Event @event);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_GetError();

}