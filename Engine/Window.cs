using System;
using System.Numerics;

namespace Engine
{
    using static Sdl2;
    public sealed class Window
    {
        string title;
        IntPtr wnd;
        IntPtr ctx;
        Vector2 mpos;
        Vector2 mwheel;
        IntPtr[] sys_cursors;
        bool[] keys;
        bool[] mouse;

        internal Window(AppConfig cfg)
        {
            SDL_SetMainReady();
            SDL_Init(SDL_INIT_GAMECONTROLLER | SDL_INIT_VIDEO);
            SDL_SetHint("SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING", "1");

            SDL_WindowFlags flags = 0x00;
            if (cfg.Fullscreen)
                flags |= SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;

            if (cfg.Resizable)
                flags |= SDL_WindowFlags.SDL_WINDOW_RESIZABLE;

            flags |= SDL_WindowFlags.SDL_WINDOW_OPENGL;

            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_RED_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_BLUE_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_ALPHA_SIZE, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DEPTH_SIZE, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_STENCIL_SIZE, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);

            wnd = SDL_CreateWindow(cfg.Title, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, cfg.Width, cfg.Height, flags);
            if (wnd == IntPtr.Zero)
                throw new Exception($"Failed to create sdl window.\n{SDL_GetError()}");

            ctx = SDL_GL_CreateContext(wnd);
            if (ctx == IntPtr.Zero)
                throw new Exception($"Failed to create opengl context.\n{SDL_GetError()}");

            if (SDL_GL_MakeCurrent(wnd, ctx) != 0)
                throw new Exception($"Failed to make opengl context current.\n{SDL_GetError()}");

            SDL_GL_SetSwapInterval(cfg.Vsync ? 1 : 0);

            keys = new bool[512];
            mouse = new bool[3];

            sys_cursors = new IntPtr[(int)SystemCursor.Count];
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                SDL_SetWindowTitle(wnd, value);
            }
        }

        public Vector2 MousePosition
        {
            get => mpos;
            set => SDL_WarpMouseInWindow(wnd, (int)value.X, (int)value.Y);
        }

        public Vector2 MouseScrollDelta => mwheel;

        public Vector2 Size
        {
            get
            {
                int w, h;
                SDL_GetWindowSize(wnd, out w, out h);
                return new Vector2(w, h);
            }
            set
            {
                SDL_SetWindowSize(wnd, (int)value.X, (int)value.Y);
            }
        }

        public bool IsKeyDown(KeyCode key) => keys[(int)key];

        public bool IsMouseDown(MouseButton button) => mouse[(int)button];

        public void SetIcon(Image img)
        {
            IntPtr surface;

            if (BitConverter.IsLittleEndian)
                surface = SDL_CreateRGBSurfaceFrom(img.PixelData, img.Width, img.Height, 32, img.Width * 4, 0x000000ffu, 0x0000ff00u, 0x00ff0000u, 0xff000000u);
            else
                surface = SDL_CreateRGBSurfaceFrom(img.PixelData, img.Width, img.Height, 32, img.Width * 4, 0xff000000u, 0x00ff0000u, 0x0000ff00u, 0x000000ffu);

            SDL_SetWindowIcon(wnd, surface);
            SDL_FreeSurface(surface);
        }

        public void SetCursor(SystemCursor cursor)
        {
            if (sys_cursors[(int)cursor] == IntPtr.Zero)
                sys_cursors[(int)cursor] = SDL_CreateSystemCursor((SDL_SystemCursor)cursor);

            SDL_SetCursor(sys_cursors[(int)cursor]);
        }

        internal void swap_buffers() => SDL_GL_SwapWindow(wnd);

        internal void do_events()
        {
            mwheel = Vector2.Zero;
            SDL_Event e;
            while (SDL_PollEvent(out e) != 0)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        App.Quit();
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT:
                        switch (e.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
                                App.ActiveState.OnResize(e.window.data1, e.window.data2);
                                break;

                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                // set all key state to false after losing focus
                                for (int i = 0; i < keys.Length; i++)
                                    keys[i] = false;
                                for (int i = 0; i < mouse.Length; i++)
                                    mouse[i] = false;
                                break;
                        }
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        {
                            var code = (KeyCode)e.key.keysym.scancode;
                            if (e.key.repeat == 0)
                            {
                                keys[(int)code] = true;
                                App.ActiveState.OnKeyDown(code);
                            }
                        }
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        {
                            var code = (KeyCode)e.key.keysym.scancode;
                            keys[(int)code] = false;
                            App.ActiveState.OnKeyUp(code);
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        {
                            int button = e.button.button - 1;
                            if ((uint)button < 3)
                            {
                                mouse[button] = true;
                                App.ActiveState.OnMouseDown((MouseButton)button, new Vector2(e.button.x, e.button.x));
                            }
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        {
                            int button = e.button.button - 1;
                            if ((uint)button < 3)
                            {
                                mouse[button] = false;
                                App.ActiveState.OnMouseUp((MouseButton)button, new Vector2(e.button.x, e.button.x));
                            }
                        }
                        break;
                    case SDL_EventType.SDL_TEXTINPUT:
                        unsafe
                        {
                            var text = UTF8_ToManaged(new IntPtr(e.text.text));
                            App.ActiveState.OnTextInput(text);
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEWHEEL:
                        mwheel = new Vector2(e.wheel.x, e.wheel.y);
                        App.ActiveState.OnMouseScroll(mwheel);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION:
                        mpos = new Vector2(e.motion.x, e.motion.y);
                        App.ActiveState.OnMouseMove(mpos);
                        break;
                    case SDL_EventType.SDL_DROPFILE:
                        var dropfile = UTF8_ToManaged(e.drop.file);
                        App.ActiveState.OnFileDrop(dropfile);
                        SDL_free(e.drop.file);
                        break;
                    case SDL_EventType.SDL_DROPTEXT:
                        var droptext = UTF8_ToManaged(e.drop.file);
                        App.ActiveState.OnFileDrop(droptext);
                        SDL_free(e.drop.file);
                        break;
                    case SDL_EventType.SDL_DROPBEGIN:
                        SDL_free(e.drop.file);
                        break;
                    case SDL_EventType.SDL_DROPCOMPLETE:
                        SDL_free(e.drop.file);
                        break;
                }
            }
        }

        internal void shutdown()
        {
            for (int i = 0; i < (int)SystemCursor.Count; i++)
            {
                if (sys_cursors[i] != IntPtr.Zero)
                    SDL_FreeCursor(sys_cursors[i]);
            }
            sys_cursors = null;
            
            SDL_GL_DeleteContext(ctx);
            SDL_DestroyWindow(wnd);
            SDL_Quit();
            wnd = IntPtr.Zero;
        }
    }

    public enum MouseButton { Left, Middle, Right }

    public enum KeyCode
    {
        A = 4,
        B = 5,
        C = 6,
        D = 7,
        E = 8,
        F = 9,
        G = 10,
        H = 11,
        I = 12,
        J = 13,
        K = 14,
        L = 15,
        M = 16,
        N = 17,
        O = 18,
        P = 19,
        Q = 20,
        R = 21,
        S = 22,
        T = 23,
        U = 24,
        V = 25,
        W = 26,
        X = 27,
        Y = 28,
        Z = 29,
        Num1 = 30,
        Num2 = 31,
        Num3 = 32,
        Num4 = 33,
        Num5 = 34,
        Num6 = 35,
        Num7 = 36,
        Num8 = 37,
        Num9 = 38,
        Num0 = 39,
        Enter = 40,
        Escape = 41,
        BackSpace = 42,
        Tab = 43,
        Space = 44,
        Minus = 45,
        Equals = 46,
        LeftBracket = 47,
        RightBracket = 48,
        BackSlash = 49,
        // HashTilde = 50, // no use
        SemiColon = 51,
        DoubleQuate = 52,
        BackQuote = 53,
        Comma = 54,
        Period = 55,
        Slash = 56,
        CapsLock = 57,
        F1 = 58,
        F2 = 59,
        F3 = 60,
        F4 = 61,
        F5 = 62,
        F6 = 63,
        F7 = 64,
        F8 = 65,
        F9 = 66,
        F10 = 67,
        F11 = 68,
        F12 = 69,
        PrintScreen = 70,
        ScrollLock = 71,
        Pause = 72,
        Insert = 73,
        Home = 74,
        PageUp = 75,
        Delete = 76,
        End = 77,
        PageDown = 78,
        Right = 79,
        Left = 80,
        Down = 81,
        Up = 82,
        NumLock = 83,
        KpDivide = 84,
        KpMultiply = 85,
        KpMinus = 86,
        KpPlus = 87,
        KpEnter = 88,
        Kp1 = 89,
        Kp2 = 90,
        Kp3 = 91,
        Kp4 = 92,
        Kp5 = 93,
        Kp6 = 94,
        Kp7 = 95,
        Kp8 = 96,
        Kp9 = 97,
        Kp0 = 98,
        KpPeriod = 99,
        LCtrl = 224,
        LShift = 225,
        LAlt = 226,
        RCtrl = 228,
        RShift = 229,
        RAlt = 230
    }

    public enum SystemCursor
    {
        Arrow,
        IBeam,
        Wait,
        Crosshair,
        WaitArrow,
        SizeNWSE,
        SizeNESW,
        SizeWE,
        SizeNS,
        SizeALL,
        No,
        Hand,
        Count
    }
}