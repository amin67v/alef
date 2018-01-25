using System;
using System.Numerics;

namespace Engine
{
    public sealed class Input
    {
        static Input instance;

        Vector2 mpos;
        Vector2 mscroll;
        bool[] keys;
        bool[] mouse;
        bool[] keys_last;
        bool[] mouse_last;
        char last_char;

        Input() { }

        public static Vector2 MouseScrollDelta => instance.mscroll;

        public static char GetLastKeyChar() => instance.last_char;

        public static bool IsKeyDown(KeyCode key) => instance.keys[(int)key];

        public static bool IsKeyPressed(KeyCode key) => instance.keys[(int)key] && !instance.keys_last[(int)key];

        public static bool IsKeyReleased(KeyCode key) => !instance.keys[(int)key] && instance.keys_last[(int)key];

        public static bool IsKeyDown(MouseButton button) => instance.mouse[(int)button];

        public static bool IsKeyPressed(MouseButton button) => instance.mouse[(int)button] && !(instance.mouse_last[(int)button]);

        public static bool IsKeyReleased(MouseButton button) => !instance.mouse[(int)button] && (instance.mouse_last[(int)button]);

        public static Vector2 MousePosition
        {
            get => instance.mpos;
            set
            {
                instance.mpos = value;
                App.Window.WrapMouse(value);
            }
        }

        internal static void init()
        {
            instance = new Input();
            instance.keys = new bool[512];
            instance.keys_last = new bool[512];
            instance.mouse = new bool[3];
            instance.mouse_last = new bool[3];

            App.Window.OnLostFocus += instance.lost_focus;
            App.Window.OnKeyPressed += instance.key_down;
            App.Window.OnKeyReleased += instance.key_up;
            App.Window.OnMouseButtonPressed += instance.mouse_down;
            App.Window.OnMouseButtonReleased += instance.mouse_up;
            App.Window.OnTextInput += instance.text_input;
            App.Window.OnMouseScroll += instance.mouse_scroll;
            App.Window.OnMouseMove += instance.mouse_pos;
        }

        internal static void shutdown()
        {
            App.Window.OnLostFocus -= instance.lost_focus;
            App.Window.OnKeyPressed -= instance.key_down;
            App.Window.OnKeyReleased -= instance.key_up;
            App.Window.OnMouseButtonPressed -= instance.mouse_down;
            App.Window.OnMouseButtonReleased -= instance.mouse_up;
            App.Window.OnTextInput -= instance.text_input;
            App.Window.OnMouseScroll -= instance.mouse_scroll;
            App.Window.OnMouseMove -= instance.mouse_pos;

            instance = null;
        }

        internal static void update()
        {
            Array.Copy(instance.keys, instance.keys_last, instance.keys.Length);
            Array.Copy(instance.mouse, instance.mouse_last, instance.mouse.Length);

            instance.mscroll = Vector2.Zero;
            instance.last_char = (char)0;
        }

        void lost_focus() => instance.reset_key_state();

        void key_down(KeyCode code) => instance.keys[(int)code] = true;

        void key_up(KeyCode code) => instance.keys[(int)code] = false;

        void mouse_down(MouseButton btn) => instance.mouse[(int)btn] = true;

        void mouse_up(MouseButton btn) => instance.mouse[(int)btn] = false;

        void text_input(char c) => instance.last_char = c;

        void mouse_scroll(Vector2 value) => instance.mscroll = value;

        void mouse_pos(Vector2 value) => instance.mpos = value;

        void reset_key_state()
        {
            for (int i = 0; i < instance.keys.Length; i++)
                instance.keys[i] = instance.keys_last[i] = false;

            for (int i = 0; i < instance.mouse.Length; i++)
                instance.mouse[i] = instance.mouse_last[i] = false;
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
        SemiColon = 51,
        Apostrophe = 52,
        Grave = 53,
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
}