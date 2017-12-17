using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class Scene : AppState
    {
        internal Array<IDrawable> draw_list;
        Camera cam;
        Entity root;
        bool is_render;
        DebugDraw debug_draw;

        public event Action OnPreRender;
        public event Action OnPostRender;

        /// <summary>
        /// Gets scene main camera
        /// </summary>
        public Camera MainCamera => cam;

        /// <summary>
        /// Returns true if we are inside rendering function, otherwide false
        /// </summary>
        public bool IsRendering => is_render;

        /// <summary>
        /// Use to draw visual stuff for debugging purposes
        /// </summary>
        public DebugDraw DebugDraw => debug_draw;

        /// <summary>
        /// Finds first entity whose name match the given value
        /// </summary>
        public Entity Find(string name)
        {
            var hash = name.KnuthHash();
            var current = root;
            while (current != null)
            {
                if (current.get_name_hash() == hash && current.Name == name)
                    return current;

                current = current.next;
            }

            return null;
        }

        /// <summary>
        /// Spawns a new entity of type T
        /// </summary>
        public T Spawn<T>(string name) where T : Entity, new()
        {
            if (is_render)
                throw new Exception("You can't spawn entity inside draw method");

            var entity = new T();
            entity.Name = name;
            entity.next = root;
            root = entity;
            entity.OnBegin();
            return entity;
        }

        /// <summary>
        /// Destroys the given entity
        /// </summary>
        public void Destroy(Entity entity)
        {
            if (is_render)
                throw new Exception("You can't destroy entity inside draw method");

            if (entity.IsDestroyed == false)
            {
                if (root == entity)
                    root = entity.next;

                entity.OnDestroy();
                if (entity.prev != null)
                    entity.prev.next = entity.next;

                if (entity.next != null)
                    entity.next.prev = entity.prev;

                entity.set_destroyed();
            }
        }

        public override void OnBegin()
        {
            cam = new Camera();
            draw_list = new Array<IDrawable>(50);
            debug_draw = new DebugDraw();
        }

        /// <summary>
        /// Note: dont override this method, use OnUpdate and OnRender instead
        /// </summary>
        public override void OnFrame()
        {
            float dt = App.Time.FrameTime;

            OnUpdate(dt);

            // update all entities
            var current = root;
            while (current != null)
            {
                current.OnUpdate(dt);
                current = current.next;
            }

            // sort and render drawables
            is_render = true;
            OnPreRender?.Invoke();
            // sort draw list based on their layer
            int comparsion(IDrawable x, IDrawable y) => x.Layer - y.Layer;
            draw_list.Sort(comparsion);
            OnRender();
            draw_list.Clear();
            App.Graphics.Display();
            OnPostRender?.Invoke();
            DebugDraw.Clear();
            is_render = false;
        }

        public void RegisterForDraw(IDrawable drawable) => draw_list.Push(drawable);

        /// <summary>
        /// Called at the start of each frame
        /// </summary>
        protected virtual void OnUpdate(float dt) { }

        /// <summary>
        /// Called when scene needs to be rendered
        /// </summary>
        protected virtual void OnRender() => MainCamera.Render(this);

        protected override void OnDisposeManaged()
        {
            var current = root;
            while (current != null)
            {
                Destroy(current);
                current = current.next;
            }

            OnPostRender = null;
            OnPreRender = null;

            draw_list.Clear();
            draw_list = null;

            root = null;
        }
    }

}