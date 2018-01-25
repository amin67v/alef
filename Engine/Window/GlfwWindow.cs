using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    using static Glfw3;

    class GlfwWindow : Window
    {
        string title; // window title
        IntPtr ptr; // window pointer
        Vector2 size; // window size
        Dictionary<Image, IntPtr> cursors;

        KeyCode[] keymap;

        public GlfwWindow(AppConfig cfg)
        {
            if (glfwInit() == 0)
                throw new Exception("Failed to initialize glfw.");

            glfwWindowHint(GLFW_RED_BITS, 8);
            glfwWindowHint(GLFW_GREEN_BITS, 8);
            glfwWindowHint(GLFW_BLUE_BITS, 8);
            glfwWindowHint(GLFW_ALPHA_BITS, 0);
            glfwWindowHint(GLFW_DEPTH_BITS, 0);
            glfwWindowHint(GLFW_STENCIL_BITS, 0);
            glfwWindowHint(GLFW_SAMPLES, 0);
            glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
            glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
            glfwWindowHint(GLFW_RESIZABLE, cfg.Resizable ? 1 : 0);

            var monitor = cfg.Fullscreen ? glfwGetPrimaryMonitor() : IntPtr.Zero;
            ptr = glfwCreateWindow(cfg.Width, cfg.Height, cfg.Title, monitor, IntPtr.Zero);
            size = new Vector2(cfg.Width, cfg.Height);
            title = cfg.Title;

            if (ptr == IntPtr.Zero)
                throw new Exception("Failed to create glfw window.");

            glfwMakeContextCurrent(ptr);

            glfwSwapInterval(cfg.Vsync ? 1 : 0);

            setup_keymap();

            glfwSetWindowCloseCallback(ptr, (w) => App.Quit());
            glfwSetWindowSizeCallback(ptr, (w1, w, h) =>
            {
                size = new Vector2(w, h);
                RaiseResize(w, h);
            });
            glfwSetWindowFocusCallback(ptr, (w, focus) =>
            {
                if (focus == 0)
                    RaiseLostFocus();
            });

            glfwSetKeyCallback(ptr, (w, key, scan, act, mods) =>
            {
                var code = keymap[(int)key];
                if (code > 0)
                {
                    if (act == GLFW_PRESS)
                        RaiseKeyPressed(code);
                    else if (act == GLFW_RELEASE)
                        RaiseKeyReleased(code);
                }
            });

            glfwSetMouseButtonCallback(ptr, (w, btn, act, mods) =>
            {
                switch (btn)
                {
                    case 0:
                        if (act == GLFW_PRESS)
                            RaiseMouseButtonPressed(MouseButton.Left);
                        else if (act == GLFW_RELEASE)
                            RaiseMouseButtonReleased(MouseButton.Left);
                        break;
                    case 1:
                        if (act == GLFW_PRESS)
                            RaiseMouseButtonPressed(MouseButton.Right);
                        else if (act == GLFW_RELEASE)
                            RaiseMouseButtonReleased(MouseButton.Right);
                        break;
                    case 2:
                        if (act == GLFW_PRESS)
                            RaiseMouseButtonPressed(MouseButton.Middle);
                        else if (act == GLFW_RELEASE)
                            RaiseMouseButtonReleased(MouseButton.Middle);
                        break;
                }
            });

            glfwSetCharCallback(ptr, (w, c) => RaiseTextInput(Convert.ToChar(c)));
            glfwSetScrollCallback(ptr, (w, x, y) => RaiseMouseScroll(new Vector2((float)x, (float)y)));
            glfwSetCursorPosCallback(ptr, (w, x, y) => RaiseMouseMove(new Vector2((float)x, (float)y)));
            glfwSetDropCallback(ptr, (w, c, p) =>
            {
                unsafe
                {
                    var ptr = (IntPtr*)p.ToPointer();
                    var paths = new string[c];
                    for (int i = 0; i < c; i++)
                        paths[i] = Marshal.PtrToStringAnsi(ptr[i]);
                        
                    RaiseFileDrop(paths);
                }
            });
        }

        public override string Title
        {
            get => title;
            set
            {
                title = value;
                glfwSetWindowTitle(ptr, value);
            }
        }

        public override Vector2 Size
        {
            get => size;
            set
            {
                size = value;
                glfwSetWindowSize(ptr, (int)value.X, (int)value.Y);
            }
        }

        protected internal override void WrapMouse(Vector2 value)
        {
            glfwSetCursorPos(ptr, value.X, value.Y);
        }

        public override void SetIcon(Image image)
        {
            GLFWimage img;
            img.width = image.Width;
            img.height = image.Height;
            img.pixels = image.PixelData;
            unsafe { glfwSetWindowIcon(ptr, 1, &img); }
        }

        public override IntPtr CreateCursor(Image image, Vector2 hotpos)
        {
            GLFWimage img;
            img.width = image.Width;
            img.height = image.Height;
            img.pixels = image.PixelData;
            unsafe { return glfwCreateCursor(&img, (int)hotpos.X, (int)hotpos.Y); }
        }

        public override void SetCursor(IntPtr cursor)
        {
            glfwSetCursor(ptr, cursor);
        }

        public override void DestroyCursor(IntPtr cursor)
        {
            glfwDestroyCursor(cursor);
        }

        protected internal override void SwapBuffers()
        {
            glfwSwapBuffers(ptr);
        }

        protected internal override void DoEvents()
        {
            glfwPollEvents();
        }

        protected internal override void ShutDown()
        {
            glfwDestroyWindow(ptr);
            glfwTerminate();
        }

        void setup_keymap()
        {
            keymap = new KeyCode[512];

            keymap[GLFW_KEY_SPACE] = KeyCode.Space;
            keymap[GLFW_KEY_APOSTROPHE] = KeyCode.Apostrophe;
            keymap[GLFW_KEY_COMMA] = KeyCode.Comma;
            keymap[GLFW_KEY_MINUS] = KeyCode.Minus;
            keymap[GLFW_KEY_PERIOD] = KeyCode.Period;
            keymap[GLFW_KEY_SLASH] = KeyCode.Slash;
            keymap[GLFW_KEY_0] = KeyCode.Num0;
            keymap[GLFW_KEY_1] = KeyCode.Num1;
            keymap[GLFW_KEY_2] = KeyCode.Num2;
            keymap[GLFW_KEY_3] = KeyCode.Num3;
            keymap[GLFW_KEY_4] = KeyCode.Num4;
            keymap[GLFW_KEY_5] = KeyCode.Num5;
            keymap[GLFW_KEY_6] = KeyCode.Num6;
            keymap[GLFW_KEY_7] = KeyCode.Num7;
            keymap[GLFW_KEY_8] = KeyCode.Num8;
            keymap[GLFW_KEY_9] = KeyCode.Num9;
            keymap[GLFW_KEY_SEMICOLON] = KeyCode.SemiColon;
            keymap[GLFW_KEY_EQUAL] = KeyCode.Equals;
            keymap[GLFW_KEY_A] = KeyCode.A;
            keymap[GLFW_KEY_B] = KeyCode.B;
            keymap[GLFW_KEY_C] = KeyCode.C;
            keymap[GLFW_KEY_D] = KeyCode.D;
            keymap[GLFW_KEY_E] = KeyCode.E;
            keymap[GLFW_KEY_F] = KeyCode.F;
            keymap[GLFW_KEY_G] = KeyCode.G;
            keymap[GLFW_KEY_H] = KeyCode.H;
            keymap[GLFW_KEY_I] = KeyCode.I;
            keymap[GLFW_KEY_J] = KeyCode.J;
            keymap[GLFW_KEY_K] = KeyCode.K;
            keymap[GLFW_KEY_L] = KeyCode.L;
            keymap[GLFW_KEY_M] = KeyCode.M;
            keymap[GLFW_KEY_N] = KeyCode.N;
            keymap[GLFW_KEY_O] = KeyCode.O;
            keymap[GLFW_KEY_P] = KeyCode.P;
            keymap[GLFW_KEY_Q] = KeyCode.Q;
            keymap[GLFW_KEY_R] = KeyCode.R;
            keymap[GLFW_KEY_S] = KeyCode.S;
            keymap[GLFW_KEY_T] = KeyCode.T;
            keymap[GLFW_KEY_U] = KeyCode.U;
            keymap[GLFW_KEY_V] = KeyCode.V;
            keymap[GLFW_KEY_W] = KeyCode.W;
            keymap[GLFW_KEY_X] = KeyCode.X;
            keymap[GLFW_KEY_Y] = KeyCode.Y;
            keymap[GLFW_KEY_Z] = KeyCode.Z;
            keymap[GLFW_KEY_LEFT_BRACKET] = KeyCode.LeftBracket;
            keymap[GLFW_KEY_BACKSLASH] = KeyCode.BackSlash;
            keymap[GLFW_KEY_RIGHT_BRACKET] = KeyCode.RightBracket;
            keymap[GLFW_KEY_GRAVE_ACCENT] = KeyCode.Grave;
            keymap[GLFW_KEY_WORLD_1] = 0;
            keymap[GLFW_KEY_WORLD_2] = 0;
            keymap[GLFW_KEY_ESCAPE] = KeyCode.Escape;
            keymap[GLFW_KEY_ENTER] = KeyCode.Enter;
            keymap[GLFW_KEY_TAB] = KeyCode.Tab;
            keymap[GLFW_KEY_BACKSPACE] = KeyCode.BackSpace;
            keymap[GLFW_KEY_INSERT] = KeyCode.Insert;
            keymap[GLFW_KEY_DELETE] = KeyCode.Delete;
            keymap[GLFW_KEY_RIGHT] = KeyCode.Right;
            keymap[GLFW_KEY_LEFT] = KeyCode.Left;
            keymap[GLFW_KEY_DOWN] = KeyCode.Down;
            keymap[GLFW_KEY_UP] = KeyCode.Up;
            keymap[GLFW_KEY_PAGE_UP] = KeyCode.PageUp;
            keymap[GLFW_KEY_PAGE_DOWN] = KeyCode.PageDown;
            keymap[GLFW_KEY_HOME] = KeyCode.Home;
            keymap[GLFW_KEY_END] = KeyCode.End;
            keymap[GLFW_KEY_CAPS_LOCK] = KeyCode.CapsLock;
            keymap[GLFW_KEY_SCROLL_LOCK] = KeyCode.ScrollLock;
            keymap[GLFW_KEY_NUM_LOCK] = KeyCode.NumLock;
            keymap[GLFW_KEY_PRINT_SCREEN] = KeyCode.PrintScreen;
            keymap[GLFW_KEY_PAUSE] = KeyCode.Pause;
            keymap[GLFW_KEY_F1] = KeyCode.F1;
            keymap[GLFW_KEY_F2] = KeyCode.F2;
            keymap[GLFW_KEY_F3] = KeyCode.F3;
            keymap[GLFW_KEY_F4] = KeyCode.F4;
            keymap[GLFW_KEY_F5] = KeyCode.F5;
            keymap[GLFW_KEY_F6] = KeyCode.F6;
            keymap[GLFW_KEY_F7] = KeyCode.F7;
            keymap[GLFW_KEY_F8] = KeyCode.F8;
            keymap[GLFW_KEY_F9] = KeyCode.F9;
            keymap[GLFW_KEY_F10] = KeyCode.F10;
            keymap[GLFW_KEY_F11] = KeyCode.F11;
            keymap[GLFW_KEY_F12] = KeyCode.F12;
            keymap[GLFW_KEY_F13] = 0;
            keymap[GLFW_KEY_F14] = 0;
            keymap[GLFW_KEY_F15] = 0;
            keymap[GLFW_KEY_F16] = 0;
            keymap[GLFW_KEY_F17] = 0;
            keymap[GLFW_KEY_F18] = 0;
            keymap[GLFW_KEY_F19] = 0;
            keymap[GLFW_KEY_F20] = 0;
            keymap[GLFW_KEY_F21] = 0;
            keymap[GLFW_KEY_F22] = 0;
            keymap[GLFW_KEY_F23] = 0;
            keymap[GLFW_KEY_F24] = 0;
            keymap[GLFW_KEY_F25] = 0;
            keymap[GLFW_KEY_KP_0] = KeyCode.Kp0;
            keymap[GLFW_KEY_KP_1] = KeyCode.Kp1;
            keymap[GLFW_KEY_KP_2] = KeyCode.Kp2;
            keymap[GLFW_KEY_KP_3] = KeyCode.Kp3;
            keymap[GLFW_KEY_KP_4] = KeyCode.Kp4;
            keymap[GLFW_KEY_KP_5] = KeyCode.Kp5;
            keymap[GLFW_KEY_KP_6] = KeyCode.Kp6;
            keymap[GLFW_KEY_KP_7] = KeyCode.Kp7;
            keymap[GLFW_KEY_KP_8] = KeyCode.Kp8;
            keymap[GLFW_KEY_KP_9] = KeyCode.Kp9;
            keymap[GLFW_KEY_KP_DECIMAL] = KeyCode.KpPeriod;
            keymap[GLFW_KEY_KP_DIVIDE] = KeyCode.KpDivide;
            keymap[GLFW_KEY_KP_MULTIPLY] = KeyCode.KpMultiply;
            keymap[GLFW_KEY_KP_SUBTRACT] = KeyCode.KpMinus;
            keymap[GLFW_KEY_KP_ADD] = KeyCode.KpPlus;
            keymap[GLFW_KEY_KP_ENTER] = KeyCode.KpEnter;
            keymap[GLFW_KEY_KP_EQUAL] = 0;
            keymap[GLFW_KEY_LEFT_SHIFT] = KeyCode.LShift;
            keymap[GLFW_KEY_LEFT_CONTROL] = KeyCode.LCtrl;
            keymap[GLFW_KEY_LEFT_ALT] = KeyCode.LAlt;
            keymap[GLFW_KEY_LEFT_SUPER] = 0;
            keymap[GLFW_KEY_RIGHT_SHIFT] = KeyCode.RShift;
            keymap[GLFW_KEY_RIGHT_CONTROL] = KeyCode.RCtrl;
            keymap[GLFW_KEY_RIGHT_ALT] = KeyCode.RAlt;
            keymap[GLFW_KEY_RIGHT_SUPER] = 0;
            keymap[GLFW_KEY_MENU] = 0;
        }
    }
}