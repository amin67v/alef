using System;
using System.Numerics;

namespace Engine
{
    using static Sdl2;

    public abstract class Window
    {
        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        public event KeyHandler OnKeyPressed;

        /// <summary>
        /// Called when a key is released.
        /// </summary>
        public event KeyHandler OnKeyReleased;

        /// <summary>
        /// Called when a mouse key is pressed.
        /// </summary>
        public event MouseButtonHandler OnMouseButtonPressed;

        /// <summary>
        /// Called when a mouse key is released.
        /// </summary>
        public event MouseButtonHandler OnMouseButtonReleased;

        /// <summary>
        /// Called when mouse cursor is moving.
        /// </summary>
        public event MouseMoveHandler OnMouseMove;

        /// <summary>
        /// Called when mouse wheel scrolls.
        /// </summary>
        public event MouseScrollHandler OnMouseScroll;

        /// <summary>
        /// Called when a char key is pressed.
        /// </summary>
        public event TextInputHandler OnTextInput;

        /// <summary>
        /// Called when window is resizing.
        /// </summary>
        public event ResizeHandler OnResize;

        /// <summary>
        /// Called when a file drops inside window rect.
        /// </summary>
        public event FileDropHandler OnFileDrop;

        /// <summary>
        /// Called when a text drops inside window rect.
        /// </summary>
        public event TextDropHandler OnTextDrop;

        /// <summary>
        /// Called when window lose focus.
        /// </summary>
        public event LostFocusHandler OnLostFocus;

        /// <summary>
        /// Gets or sets window title 
        /// </summary>
        public abstract string Title { get; set; }

        /// <summary>
        /// Gets or set window client size
        /// </summary>
        public abstract Vector2 Size { get; set; }

        /// <summary>
        /// Sets window icon
        /// </summary>
        public abstract void SetIcon(Image value);

        /// <summary>
        /// Sets cursor to the given value
        /// </summary>
        public abstract void SetCursor(SystemCursor value);

        protected internal abstract void SwapBuffers();

        protected internal abstract void WrapMouse(Vector2 value);

        protected internal abstract void DoEvents();

        protected internal abstract void ShutDown();

        protected void RaiseKeyPressed(KeyCode key) => OnKeyPressed?.Invoke(key);

        protected void RaiseKeyReleased(KeyCode key) => OnKeyReleased?.Invoke(key);

        protected void RaiseMouseButtonPressed(MouseButton btn) => OnMouseButtonPressed?.Invoke(btn);

        protected void RaiseMouseButtonReleased(MouseButton btn) => OnMouseButtonReleased?.Invoke(btn);

        protected void RaiseMouseMove(Vector2 pos) => OnMouseMove?.Invoke(pos);

        protected void RaiseMouseScroll(Vector2 delta) => OnMouseScroll?.Invoke(delta);

        protected void RaiseTextInput(string text) => OnTextInput?.Invoke(text);

        protected void RaiseResize(int width, int height) => OnResize?.Invoke(width, height);

        protected void RaiseFileDrop(string text) => OnFileDrop?.Invoke(text);

        protected void RaiseTextDrop(string text) => OnTextDrop?.Invoke(text);

        protected void RaiseLostFocus() => OnLostFocus?.Invoke();

        internal static Window init(AppConfig cfg)
        {
            return new Sdl2Window(cfg);
        }
    }

    public delegate void KeyHandler(KeyCode key);
    public delegate void MouseButtonHandler(MouseButton btn);
    public delegate void MouseMoveHandler(Vector2 pos);
    public delegate void MouseScrollHandler(Vector2 delta);
    public delegate void TextInputHandler(string text);
    public delegate void ResizeHandler(int width, int height);
    public delegate void FileDropHandler(string text);
    public delegate void TextDropHandler(string text);
    public delegate void LostFocusHandler();

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
        SizeAll,
        No,
        Hand,
        Count
    }
}