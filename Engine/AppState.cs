using System;
using System.Numerics;

namespace Engine
{
    public abstract class AppState : Disposable
    {
        /// <summary>
        /// Called at the start of each frame.
        /// </summary>
        public virtual void OnFrame() { }

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        public virtual void OnKeyDown(KeyCode key) { }

        /// <summary>
        /// Called when a key is released.
        /// </summary>
        public virtual void OnKeyUp(KeyCode key) { }

        /// <summary>
        /// Called when a mouse key is pressed.
        /// </summary>
        public virtual void OnMouseDown(MouseButton btn, Vector2 pos) { }

        /// <summary>
        /// Called when a mouse key is released.
        /// </summary>
        public virtual void OnMouseUp(MouseButton btn, Vector2 pos) { }

        /// <summary>
        /// Called when mouse cursor moving.
        /// </summary>
        public virtual void OnMouseMove(Vector2 pos) { }

        /// <summary>
        /// Called when mouse wheel scrolls.
        /// </summary>
        public virtual void OnMouseScroll(Vector2 delta) { }

        /// <summary>
        /// Called when a te is pressed.
        /// </summary>
        public virtual void OnTextInput(string text) { }

        /// <summary>
        /// Called when window is resizing.
        /// </summary>
        public virtual void OnResize(int width, int height) { }

        /// <summary>
        /// Called when a file drops inside window rect.
        /// </summary>
        public virtual void OnFileDrop(string file) { }

        /// <summary>
        /// Called when a text drops inside window rect.
        /// </summary>
        public virtual void OnTextDrop(string text) { }

        /// <summary>
        /// Called when this state begins.
        /// </summary>
        public virtual void OnBegin() { }

        /// <summary>
        /// Called when quiting this state.
        /// </summary>
        public virtual void OnEnd() { }
    }
}