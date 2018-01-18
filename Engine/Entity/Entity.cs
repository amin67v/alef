namespace Engine
{
    public abstract class Entity
    {
        string name;
        internal int name_hash;
        internal Entity next, prev;
        internal bool is_destroyed = false;


        /// <summary>
        /// Gets the active scene
        /// </summary>
        public static Scene Scene => App.ActiveState as Scene;

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
        /// Returns true if the entity is destroyed and invalid, otherwise true
        /// </summary>
        public bool IsDestroyed => is_destroyed;

        /// <summary>
        /// Called as soon as entity is spawned
        /// </summary>
        public virtual void OnBegin() { }

        /// <summary>
        /// Called at the start of each frame
        /// </summary>
        public virtual void OnUpdate(float dt) { }

        /// <summary>
        /// Called when entity is going to be destroyed
        /// </summary>
        public virtual void OnDestroy() { }
    }
}