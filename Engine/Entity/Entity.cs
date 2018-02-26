using System;

namespace Engine
{
    public partial class Entity
    {
        internal Entity next, prev;
        Action<float> node_update;
        Node root;
        string name;
        int name_hash;
        bool is_destroyed;

        /// <summary>
        /// Name of the entity
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                name_hash = name.GetFastHash();
            }
        }

        /// <summary>
        /// Gets or sets root node for this entity
        /// </summary>
        public Node RootNode => root;

        /// <summary>
        /// Returns true if the entity is destroyed and invalid, otherwise true
        /// </summary>
        public bool IsDestroyed => is_destroyed;

        /// <summary>
        /// Finds first entity whose name match the given value
        /// </summary>
        public static Entity Find(string name)
        {
            if (Scene.Active == null)
            {
                var msg = $"Failed to find entity because there is no active scene.";
                Log.Error(msg);
                Assert.IsTrue(false, msg);
                return null;
            }

            var hash = name.GetFastHash();
            var current = Scene.Active.head;
            while (current != null)
            {
                if (current.name_hash == hash && current.Name == name)
                    return current;

                current = current.next;
            }

            return null;
        }

        /// <summary>
        /// Spawns a new entity of type T inside active scene
        /// </summary>
        public static T Spawn<T>(string name) where T : Entity, new()
        {
            if (Scene.Active == null)
            {
                var msg = $"You can not spawn entity without an active scene.";
                Log.Error(msg);
                Assert.IsTrue(false, msg);
                return null;
            }

            if (Scene.Active.IsRendering)
            {
                var msg = $"You can not spawn entity inside render method.";
                Log.Error(msg);
                Assert.IsTrue(false, msg);
                return null;
            }

            var entity = new T();
            entity.Name = name;
            entity.next = Scene.Active.head;
            Scene.Active.head = entity;
            entity.OnBegin();
            return entity;
        }

        /// <summary>
        /// Destroys this entity
        /// </summary>
        public void Destroy()
        {
            if (Scene.Active == null)
            {
                var msg = $"You can not destroy entity without an active scene.";
                Log.Error(msg);
                Assert.IsTrue(false, msg);
                return;
            }

            if (Scene.Active.IsRendering)
            {
                var msg = $"You can not destroy entity inside render method.";
                Log.Error(msg);
                Assert.IsTrue(false, msg);
                return;
            }

            if (is_destroyed == false)
            {
                if (Scene.Active.head == this)
                    Scene.Active.head = next;

                RootNode?.Destroy();
                OnDestroy();
                if (prev != null)
                    prev.next = next;

                if (next != null)
                    next.prev = prev;

                is_destroyed = true;
            }
            else
            {
                Log.Warning($"Multiple destroy called on entity with name '{Name}'.");
            }
        }

        /// <summary>
        /// Creates root node of type T for this entity
        /// if root node already exist it will become child for the new root node
        /// </summary>
        public T CreateRootNode<T>() where T : Node, new()
        {
            return Node.CreateRoot<T>(this);
        }

        /// <summary>
        /// Called as soon as entity is spawned
        /// </summary>
        protected virtual void OnBegin() { }

        /// <summary>
        /// Called at the start of each frame
        /// </summary>
        protected virtual void OnUpdate(float dt) { }

        /// <summary>
        /// Called when entity is going to be destroyed
        /// </summary>
        protected virtual void OnDestroy() { }

        internal void update(float dt)
        {
            OnUpdate(dt);
            node_update?.Invoke(dt);
        }
    }
}