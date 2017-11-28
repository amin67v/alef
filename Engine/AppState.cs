using System;
using System.Numerics;

namespace Engine
{
    public abstract class AppState
    {
        /// <summary>
        /// Called at the beginning of each frame.
        /// </summary>
        public virtual void OnBeginFrame() { }

        /// <summary>
        /// Called each time the game needs to be updated.
        /// </summary>
        public virtual void OnUpdate(float dt) { }

        /// <summary>
        /// Called when the game needs to be rendered.
        /// </summary>
        public virtual void OnRender() { }

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        public virtual void OnKeyDown(Keys key, bool alt, bool ctrl, bool shift) { }

        /// <summary>
        /// Called when a key is released.
        /// </summary>
        public virtual void OnKeyUp(Keys key, bool alt, bool ctrl, bool shift) { }

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
        /// Called when a keychar is pressed.
        /// </summary>
        public virtual void OnKeyChar(string code) { }

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