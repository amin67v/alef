using System;
using System.Numerics;

namespace Engine
{
    public abstract class AppState : Disposable
    {
        static AppState act_state = null;
        static AppState nxt_state = null;

        /// <summary>
        /// Gets or sets the active state of the app.
        /// Note that setting active state does happen at the end of the frame and not immediately.
        /// </summary>
        public static AppState Active
        {
            get => act_state;
            set => nxt_state = value;
        }

        /// <summary>
        /// Called at the start of each frame.
        /// </summary>
        public virtual void OnFrame() { }

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

        internal static void init(AppState state)
        {
            if (state != null)
            {
                act_state = state;
                act_state.OnBegin();
            }
            else
            {
                throw new Exception("Initial state cant be null.");
            }
        }

        internal static void shut_down()
        {
            Active.OnEnd();
            nxt_state = act_state = null;
        }

        internal static void process()
        {
            if (nxt_state != null)
            {
                act_state.OnEnd();
                act_state = nxt_state;
                act_state.OnBegin();
                nxt_state = null;
            }
        }
    }
}