using System;
using System.Numerics;

namespace Engine
{
    public sealed class Input : ObjectBase
    {
        Vector2 mousePos;
        Vector2 lastMousePos;
        Vector2 deltaMousePos;
        Vector2 mouseScrollDelta;
        bool[] keys;
        bool[] mouse;
        bool[] lastFrameKeys;
        bool[] lastFrameMouse;
        char lastChar;
        bool lockMouse;

        internal Input()
        {
            keys = new bool[512];
            lastFrameKeys = new bool[512];
            mouse = new bool[3];
            lastFrameMouse = new bool[3];

            lastMousePos = mousePos = Window.GetMousePosition();

            Window.OnFocusChanged += focusChanged;
            Window.OnKeyPressed += keyDown;
            Window.OnKeyReleased += keyUp;
            Window.OnMouseButtonPressed += mouseDown;
            Window.OnMouseButtonReleased += mouseUp;
            Window.OnTextInput += textInput;
            Window.OnMouseScroll += mouseScroll;
        }

        public char GetLastKeyChar() => lastChar;

        public bool IsKeyDown(KeyCode key) => keys[(int)key];

        public bool IsKeyPressed(KeyCode key) => keys[(int)key] && !lastFrameKeys[(int)key];

        public bool IsKeyReleased(KeyCode key) => !keys[(int)key] && lastFrameKeys[(int)key];

        public bool IsKeyDown(MouseButton button) => mouse[(int)button];

        public bool IsKeyPressed(MouseButton button) => mouse[(int)button] && !(lastFrameMouse[(int)button]);

        public bool IsKeyReleased(MouseButton button) => !mouse[(int)button] && (lastFrameMouse[(int)button]);

        public Vector2 MouseScrollDelta => mouseScrollDelta;

        public bool LockCursor
        {
            get => lockMouse;
            set => lockMouse = value;
        }

        public Vector2 MousePosition
        {
            get => mousePos;
            set
            {
                mousePos = value;
                Window.SetMousePosition(value);
            }
        }

        public Vector2 MouseDeltaPosition => deltaMousePos;

        protected override void OnDestroy()
        {
            Window.OnFocusChanged -= focusChanged;
            Window.OnKeyPressed -= keyDown;
            Window.OnKeyReleased -= keyUp;
            Window.OnMouseButtonPressed -= mouseDown;
            Window.OnMouseButtonReleased -= mouseUp;
            Window.OnTextInput -= textInput;
            Window.OnMouseScroll -= mouseScroll;
        }

        internal void PreWindowEvents()
        {
            Array.Copy(keys, lastFrameKeys, keys.Length);
            Array.Copy(mouse, lastFrameMouse, mouse.Length);
            mouseScrollDelta = Vector2.Zero;
            lastChar = (char)0;
        }

        internal void PostWindowEvents()
        {
            mousePos = Window.GetMousePosition();
            deltaMousePos = MousePosition - lastMousePos;
            if (lockMouse && Window.IsFocused)
                MousePosition = new Vector2(Window.Width, Window.Height) * .5f;
            lastMousePos = MousePosition;
        }

        void focusChanged(bool isFocused) => resetKeyState();

        void keyDown(KeyCode code) => keys[(int)code] = true;

        void keyUp(KeyCode code) => keys[(int)code] = false;

        void mouseDown(MouseButton btn) => mouse[(int)btn] = true;

        void mouseUp(MouseButton btn) => mouse[(int)btn] = false;

        void textInput(char c) => lastChar = c;

        void mouseScroll(Vector2 value) => mouseScrollDelta = value;

        void resetKeyState()
        {
            for (int i = 0; i < keys.Length; i++)
                keys[i] = lastFrameKeys[i] = false;

            for (int i = 0; i < mouse.Length; i++)
                mouse[i] = lastFrameMouse[i] = false;
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