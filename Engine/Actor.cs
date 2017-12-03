namespace Engine
{
    public abstract class Actor
    {
        internal Scene owner;
        internal Actor next, prev;
        internal uint hash_name;
        internal bool is_destroyed = false;

        string name;

        /// <summary>
        /// Name of the actor
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                hash_name = name.KnuthHash();
            }
        }

        /// <summary>
        /// Returns true if the actor is destroyed and invalid, otherwise true
        /// </summary>
        public bool IsDestroyed => is_destroyed;

        /// <summary>
        /// The scene which actor exist inside.
        /// </summary>
        public Scene Scene => owner;

        /// <summary>
        /// Called as soon as actor is spawned
        /// </summary>
        public virtual void OnBegin() { }

        /// <summary>
        /// Called at the start of each frame
        /// </summary>
        public virtual void OnNewFrame() { }

        /// <summary>
        /// Called when actor needs to get updated
        /// </summary>
        public virtual void OnUpdate(float dt) { }

        /// <summary>
        /// Called when actor needs to be rendered
        /// </summary>
        public virtual void OnRender() { }

        /// <summary>
        /// Called when actor is going to be destroyed
        /// </summary>
        public virtual void OnDestroy() { }

    }
}