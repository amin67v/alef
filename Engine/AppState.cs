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
        public virtual void OnMouseDown(int key, Vector2 pos) { }

        /// <summary>
        /// Called when a mouse key is released.
        /// </summary>
        public virtual void OnMouseUp(int key, Vector2 pos) { }

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
        /// Called when this state begins.
        /// </summary>
        public virtual void OnBegin() { }

        /// <summary>
        /// Called when quiting this state.
        /// </summary>
        public virtual void OnEnd() { }
    }
}