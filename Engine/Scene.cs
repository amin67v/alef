using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class Scene : AppState
    {
        Camera cam;
        Entity root;
        Array<IDrawable> draw_list;
        bool inside_draw_loop;

        public Scene()
        {
            cam = new Camera();
            draw_list = new Array<IDrawable>(50);
        }

        public Camera Camera => cam;

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
            if (inside_draw_loop)
                throw new Exception("You can't spawn entity inside draw method");

            var entity = new T();
            entity.owner = this;
            entity.Name = name;
            entity.next = root;
            root = entity;
            entity.OnBegin();
            if (entity is IDrawable)
                draw_list.Push(entity as IDrawable);
            return entity;
        }

        /// <summary>
        /// Destroys the given entity
        /// </summary>
        public void Destroy(Entity entity)
        {
            if (inside_draw_loop)
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

                if (entity is IDrawable)
                    draw_list.Remove(entity as IDrawable);

                entity.set_destroyed();
            }
        }

        public void Unload()
        {
            var current = root;
            while (current != null)
            {
                Destroy(current);
                current = current.next;
            }
        }

        public override void OnFrame()
        {
            float dt = Time.FrameTime;

            // update all entities
            var current = root;
            while (current != null)
            {
                current.OnUpdate(dt);
                current = current.next;
            }

            // sort and render drawables
            inside_draw_loop = true;
            var wnd_size = Window.Size;
            var viewport = cam.Viewport * wnd_size;
            Graphics.Viewport(viewport);
            Graphics.Clear(cam.ClearColor);

            Vector2 view_size;
            if (cam.SizeMode == Camera.ViewSizeMode.Width)
            {
                view_size.X = cam.ViewSize;
                view_size.Y = view_size.X * (viewport.Height / viewport.Width);
            }
            else
            {
                view_size.Y = cam.ViewSize;
                view_size.X = view_size.Y * (viewport.Width / viewport.Height);
            }
            Graphics.SetView(cam.Position, cam.Rotation, view_size);

            int comparsion(IDrawable x, IDrawable y) => x.SortKey - y.SortKey;
            draw_list.Sort(comparsion);

            for (int i = 0; i < draw_list.Count; i++)
            {
                if (draw_list[i].IsVisible())
                    draw_list[i].Draw();
            }

            Graphics.Display();
            inside_draw_loop = false;
        }
    }

}