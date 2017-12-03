using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class Scene : AppState
    {
        Camera cam;
        Actor first;

        Scene()
        {
            cam = new Camera();
        }

        public Camera Camera => cam;

        /// <summary>
        /// Finds first actor whose name match the given value
        /// </summary>
        public Actor Find(string name)
        {
            var hash = name.KnuthHash();
            var current = first;
            while (current != null)
            {
                if (current.hash_name == hash && current.Name == name)
                    return current;

                current = current.next;
            }

            return null;
        }

        /// <summary>
        /// Spawns a new actor of type T
        /// </summary>
        public T Spawn<T>() where T : Actor, new()
        {
            var new_actor = new T();
            new_actor.owner = this;
            new_actor.next = first;
            first = new_actor;
            new_actor.OnBegin();
            return new_actor;
        }

        /// <summary>
        /// Destroys the given actor
        /// </summary>
        public void Destroy(Actor actor)
        {
            if (actor.is_destroyed == false)
            {
                if (first == actor)
                    first = actor.next;

                actor.OnDestroy();
                if (actor.prev != null)
                    actor.prev.next = actor.next;

                if (actor.next != null)
                    actor.next.prev = actor.prev;

                actor.is_destroyed = true;
            }
        }

        public void Unload()
        {
            var current = first;
            while (current != null)
            {
                Destroy(current);
                current = current.next;
            }
        }

        public override void OnNewFrame()
        {
            var current = first;
            while (current != null)
            {
                current.OnNewFrame();
                current = current.next;
            }
        }

        public override void OnUpdate(float dt)
        {
            var current = first;
            while (current != null)
            {
                current.OnUpdate(dt);
                current = current.next;
            }
        }

        public override void OnRender()
        {
            Graphics.Viewport(cam.Viewport);
            Graphics.Clear(cam.ClearColor);

            var current = first;
            while (current != null)
            {
                current.OnRender();
                current = current.next;
            }

            Graphics.Display();
        }
    }

}