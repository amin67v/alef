using System;
using System.Security;
using System.Runtime.InteropServices;

[SuppressUnmanagedCodeSecurity]
unsafe static class Glfw3
{
    const string lib = "glfw3";

    // key action
    public const int GLFW_PRESS = 1;
    public const int GLFW_RELEASE = 0;
    public const int GLFW_REPEAT = 2;

    // window hints
    public const int GLFW_RESIZABLE = 0x00020003;
    public const int GLFW_RED_BITS = 0x00021001;
    public const int GLFW_GREEN_BITS = 0x00021002;
    public const int GLFW_BLUE_BITS = 0x00021003;
    public const int GLFW_ALPHA_BITS = 0x00021004;
    public const int GLFW_DEPTH_BITS = 0x00021005;
    public const int GLFW_STENCIL_BITS = 0x00021006;
    public const int GLFW_SAMPLES = 0x0002100D;
    public const int GLFW_CONTEXT_VERSION_MAJOR = 0x00022002;
    public const int GLFW_CONTEXT_VERSION_MINOR = 0x00022003;

    // keycodes
    public const int GLFW_KEY_SPACE = 32;
    public const int GLFW_KEY_APOSTROPHE = 39;
    public const int GLFW_KEY_COMMA = 44;
    public const int GLFW_KEY_MINUS = 45;
    public const int GLFW_KEY_PERIOD = 46;
    public const int GLFW_KEY_SLASH = 47;
    public const int GLFW_KEY_0 = 48;
    public const int GLFW_KEY_1 = 49;
    public const int GLFW_KEY_2 = 50;
    public const int GLFW_KEY_3 = 51;
    public const int GLFW_KEY_4 = 52;
    public const int GLFW_KEY_5 = 53;
    public const int GLFW_KEY_6 = 54;
    public const int GLFW_KEY_7 = 55;
    public const int GLFW_KEY_8 = 56;
    public const int GLFW_KEY_9 = 57;
    public const int GLFW_KEY_SEMICOLON = 59;
    public const int GLFW_KEY_EQUAL = 61;
    public const int GLFW_KEY_A = 65;
    public const int GLFW_KEY_B = 66;
    public const int GLFW_KEY_C = 67;
    public const int GLFW_KEY_D = 68;
    public const int GLFW_KEY_E = 69;
    public const int GLFW_KEY_F = 70;
    public const int GLFW_KEY_G = 71;
    public const int GLFW_KEY_H = 72;
    public const int GLFW_KEY_I = 73;
    public const int GLFW_KEY_J = 74;
    public const int GLFW_KEY_K = 75;
    public const int GLFW_KEY_L = 76;
    public const int GLFW_KEY_M = 77;
    public const int GLFW_KEY_N = 78;
    public const int GLFW_KEY_O = 79;
    public const int GLFW_KEY_P = 80;
    public const int GLFW_KEY_Q = 81;
    public const int GLFW_KEY_R = 82;
    public const int GLFW_KEY_S = 83;
    public const int GLFW_KEY_T = 84;
    public const int GLFW_KEY_U = 85;
    public const int GLFW_KEY_V = 86;
    public const int GLFW_KEY_W = 87;
    public const int GLFW_KEY_X = 88;
    public const int GLFW_KEY_Y = 89;
    public const int GLFW_KEY_Z = 90;
    public const int GLFW_KEY_LEFT_BRACKET = 91;
    public const int GLFW_KEY_BACKSLASH = 92;
    public const int GLFW_KEY_RIGHT_BRACKET = 93;
    public const int GLFW_KEY_GRAVE_ACCENT = 96;
    public const int GLFW_KEY_WORLD_1 = 161;
    public const int GLFW_KEY_WORLD_2 = 162;
    public const int GLFW_KEY_ESCAPE = 256;
    public const int GLFW_KEY_ENTER = 257;
    public const int GLFW_KEY_TAB = 258;
    public const int GLFW_KEY_BACKSPACE = 259;
    public const int GLFW_KEY_INSERT = 260;
    public const int GLFW_KEY_DELETE = 261;
    public const int GLFW_KEY_RIGHT = 262;
    public const int GLFW_KEY_LEFT = 263;
    public const int GLFW_KEY_DOWN = 264;
    public const int GLFW_KEY_UP = 265;
    public const int GLFW_KEY_PAGE_UP = 266;
    public const int GLFW_KEY_PAGE_DOWN = 267;
    public const int GLFW_KEY_HOME = 268;
    public const int GLFW_KEY_END = 269;
    public const int GLFW_KEY_CAPS_LOCK = 280;
    public const int GLFW_KEY_SCROLL_LOCK = 281;
    public const int GLFW_KEY_NUM_LOCK = 282;
    public const int GLFW_KEY_PRINT_SCREEN = 283;
    public const int GLFW_KEY_PAUSE = 284;
    public const int GLFW_KEY_F1 = 290;
    public const int GLFW_KEY_F2 = 291;
    public const int GLFW_KEY_F3 = 292;
    public const int GLFW_KEY_F4 = 293;
    public const int GLFW_KEY_F5 = 294;
    public const int GLFW_KEY_F6 = 295;
    public const int GLFW_KEY_F7 = 296;
    public const int GLFW_KEY_F8 = 297;
    public const int GLFW_KEY_F9 = 298;
    public const int GLFW_KEY_F10 = 299;
    public const int GLFW_KEY_F11 = 300;
    public const int GLFW_KEY_F12 = 301;
    public const int GLFW_KEY_F13 = 302;
    public const int GLFW_KEY_F14 = 303;
    public const int GLFW_KEY_F15 = 304;
    public const int GLFW_KEY_F16 = 305;
    public const int GLFW_KEY_F17 = 306;
    public const int GLFW_KEY_F18 = 307;
    public const int GLFW_KEY_F19 = 308;
    public const int GLFW_KEY_F20 = 309;
    public const int GLFW_KEY_F21 = 310;
    public const int GLFW_KEY_F22 = 311;
    public const int GLFW_KEY_F23 = 312;
    public const int GLFW_KEY_F24 = 313;
    public const int GLFW_KEY_F25 = 314;
    public const int GLFW_KEY_KP_0 = 320;
    public const int GLFW_KEY_KP_1 = 321;
    public const int GLFW_KEY_KP_2 = 322;
    public const int GLFW_KEY_KP_3 = 323;
    public const int GLFW_KEY_KP_4 = 324;
    public const int GLFW_KEY_KP_5 = 325;
    public const int GLFW_KEY_KP_6 = 326;
    public const int GLFW_KEY_KP_7 = 327;
    public const int GLFW_KEY_KP_8 = 328;
    public const int GLFW_KEY_KP_9 = 329;
    public const int GLFW_KEY_KP_DECIMAL = 330;
    public const int GLFW_KEY_KP_DIVIDE = 331;
    public const int GLFW_KEY_KP_MULTIPLY = 332;
    public const int GLFW_KEY_KP_SUBTRACT = 333;
    public const int GLFW_KEY_KP_ADD = 334;
    public const int GLFW_KEY_KP_ENTER = 335;
    public const int GLFW_KEY_KP_EQUAL = 336;
    public const int GLFW_KEY_LEFT_SHIFT = 340;
    public const int GLFW_KEY_LEFT_CONTROL = 341;
    public const int GLFW_KEY_LEFT_ALT = 342;
    public const int GLFW_KEY_LEFT_SUPER = 343;
    public const int GLFW_KEY_RIGHT_SHIFT = 344;
    public const int GLFW_KEY_RIGHT_CONTROL = 345;
    public const int GLFW_KEY_RIGHT_ALT = 346;
    public const int GLFW_KEY_RIGHT_SUPER = 347;
    public const int GLFW_KEY_MENU = 348;

    public struct GLFWimage
    {
        public int width;
        public int height;
        public IntPtr pixels;
    }

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int glfwInit();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwTerminate();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwWindowHint(int target, int hint);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr glfwGetPrimaryMonitor();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr glfwCreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwDestroyWindow(IntPtr window);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwSetWindowTitle(IntPtr window, string title);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwMakeContextCurrent(IntPtr window);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwSwapInterval(int interval);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwSwapBuffers(IntPtr window);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwSetWindowSize(IntPtr window, int width, int height);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwSetCursorPos(IntPtr window, double xpos, double ypos);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwPollEvents();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr glfwCreateCursor(GLFWimage* image, int xhot, int yhot);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwSetCursor(IntPtr window, IntPtr cursor);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwDestroyCursor(IntPtr cursor);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void glfwSetWindowIcon(IntPtr window, int count, GLFWimage* image);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWwindowclosefun glfwSetWindowCloseCallback(IntPtr window, GLFWwindowclosefun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWwindowsizefun glfwSetWindowSizeCallback(IntPtr window, GLFWwindowsizefun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWwindowfocusfun glfwSetWindowFocusCallback(IntPtr window, GLFWwindowfocusfun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWkeyfun glfwSetKeyCallback(IntPtr window, GLFWkeyfun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWmousebuttonfun glfwSetMouseButtonCallback(IntPtr window, GLFWmousebuttonfun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWcharfun glfwSetCharCallback(IntPtr window, GLFWcharfun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWscrollfun glfwSetScrollCallback(IntPtr window, GLFWscrollfun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWcursorposfun glfwSetCursorPosCallback(IntPtr window, GLFWcursorposfun cbfun);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern GLFWdropfun glfwSetDropCallback(IntPtr window, GLFWdropfun cbfun);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWmousebuttonfun(IntPtr window, int button, int action, int mods);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWcursorposfun(IntPtr window, double xoffset, double yoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWscrollfun(IntPtr window, double xpos, double ypos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWkeyfun(IntPtr window, int key, int scancode, int action, int mods);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWcharfun(IntPtr window, uint code);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowsizefun(IntPtr window, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowclosefun(IntPtr window);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWwindowfocusfun(IntPtr window, int focused);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWdropfun(IntPtr window, int count, IntPtr paths);
}