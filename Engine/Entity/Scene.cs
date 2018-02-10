using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class Scene : IAppState
    {
        internal Array<IDrawable> draw_list;
        internal Entity head;
        Comparer<IDrawable> draw_comparer;
        DebugDraw debug_draw;
        Camera main_cam;
        bool rendering;

        public event Action OnPreRender;
        public event Action OnPostRender;

        /// <summary>
        /// Gets the active scene
        /// </summary>
        public static Scene Active => App.ActiveState as Scene;

        /// <summary>
        /// Gets scene main camera
        /// </summary>
        public Camera MainCamera => main_cam;

        /// <summary>
        /// Returns true if we are inside rendering function, otherwide false
        /// </summary>
        public bool IsRendering => rendering;

        /// <summary>
        /// Use to draw visual stuff for debugging purposes
        /// </summary>
        public DebugDraw DebugDraw => debug_draw;


        public virtual void OnEnter()
        {
            main_cam = new Camera();
            draw_list = new Array<IDrawable>(50);
            debug_draw = new DebugDraw();
            draw_comparer = Comparer<IDrawable>.Create((IDrawable x, IDrawable y) => x.Layer - y.Layer);
        }

        public virtual void OnExit()
        {
            var current = head;
            while (current != null)
            {
                current.Destroy();
                current = current.next;
            }

            OnPostRender = null;
            OnPreRender = null;

            draw_list.Clear();
            draw_list = null;

            head = null;
        }

        public void OnFrame()
        {
            float dt = Time.FrameTime;

            OnUpdate(dt);

            // update all entities
            var current = head;
            while (current != null)
            {
                current.update(dt);
                current = current.next;
            }

            // sort and render drawables
            rendering = true;
            OnPreRender?.Invoke();
            // sort draw list based on their layer
            draw_list.Sort(draw_comparer);
            OnRender();
            draw_list.Clear();
            App.Graphics.Display();
            OnPostRender?.Invoke();
            DebugDraw.Clear();
            rendering = false;
        }

        public void AddDrawable(IDrawable drawable) => draw_list.Push(drawable);

        /// <summary>
        /// Called at the start of each frame
        /// </summary>
        protected virtual void OnUpdate(float dt) { }

        /// <summary>
        /// Called when scene needs to be rendered
        /// </summary>
        protected virtual void OnRender() => MainCamera.Render(this);
    }

}