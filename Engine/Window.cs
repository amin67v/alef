using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public delegate void KeyHandler(KeyCode key);
    public delegate void MouseButtonHandler(MouseButton btn);
    public delegate void MouseMoveHandler(Vector2 pos);
    public delegate void MouseScrollHandler(Vector2 delta);
    public delegate void TextInputHandler(char text);
    public delegate void ResizeHandler(int width, int height);
    public delegate void FileDropHandler(string[] files);
    public delegate void FocusChangedHandler(bool isFocused);

    public abstract class Window : ObjectBase
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
        /// Called when window lose focus.
        /// </summary>
        public event FocusChangedHandler OnFocusChanged;

        public abstract string Title { get; set; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract bool IsFocused { get; }

        public abstract void SetSize(int width, int height, bool fullScreen);
        public abstract Vector2 GetMousePosition();
        public abstract void SetMousePosition(Vector2 value);
        public abstract void SetIcon(Image image);
        public abstract IntPtr CreateCursor(Image image, Vector2 hotpos);
        public abstract void SetCursor(IntPtr cursor);
        public abstract void DestroyCursor(IntPtr cursor);
        public abstract void SwapBuffers();
        public abstract void DoEvents();

        protected void RaiseOnKeyPressed(KeyCode key) => OnKeyPressed?.Invoke(key);
        protected void RaiseOnKeyReleased(KeyCode key) => OnKeyReleased?.Invoke(key);
        protected void RaiseOnMouseButtonPressed(MouseButton btn) => OnMouseButtonPressed?.Invoke(btn);
        protected void RaiseOnMouseButtonReleased(MouseButton btn) => OnMouseButtonReleased?.Invoke(btn);
        protected void RaiseOnMouseMove(Vector2 pos) => OnMouseMove?.Invoke(pos);
        protected void RaiseOnMouseScroll(Vector2 delta) => OnMouseScroll?.Invoke(delta);
        protected void RaiseOnTextInput(char text) => OnTextInput?.Invoke(text);
        protected void RaiseOnResize(int width, int height) => OnResize?.Invoke(width, height);
        protected void RaiseOnFileDrop(string[] files) => OnFileDrop?.Invoke(files);
        protected void RaiseOnFocusChanged(bool isFocused) => OnFocusChanged?.Invoke(isFocused);
    }
}