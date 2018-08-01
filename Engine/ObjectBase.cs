using System;
using System.Runtime.CompilerServices;

namespace Engine
{
    public class ObjectBase
    {
        string name;
        int nameHash;

        ~ObjectBase()
        {
            Log.Error($"Potential memory leak, Destroy never called on object (Name: '{Name}' - Type: '{GetType().Name}')");
        }

        public static Log Log => Game.Current.Log;
        
        public static Time Time => Game.Current.Time;

        public static Profiler Profiler => Game.Current.Profiler;

        public static Window Window => Game.Current.Window;

        public static GraphicsDriver Graphics => Game.Current.Graphics;

        public static ResourceManager ResourceManager => Game.Current.ResourceManager;

        public static Renderer Renderer => Game.Current.Renderer;

        public static Input Input => Game.Current.Input;

        public bool IsDestroyed { get; internal set; }

        public string Name
        {
            get => name;
            set
            {
                name = value ?? "";
                nameHash = Hash.FromString(name);
            }
        }

        public static void Destroy(ObjectBase obj) => obj?.DestroyImp(false);

        protected virtual void OnBegin() { }

        protected virtual void OnDestroy() { }

        internal virtual void DestroyImp(bool destroying)
        {
            if (!IsDestroyed)
            {
                OnDestroy();
                IsDestroyed = true;
                GC.SuppressFinalize(this);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool CompareName(string name, int hash) => nameHash == hash && Name == name;
    }
}