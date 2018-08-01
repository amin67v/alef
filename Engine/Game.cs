using System;
using System.IO;

namespace Engine
{
    public abstract class Game
    {
        Scene activeScene;

        public static Game Current { get; private set; }

        public abstract string Name { get; }

        public Scene ActiveScene
        {
            get => activeScene;
            set
            {
                if (activeScene != value)
                {
                    activeScene = value;
                    activeScene.OnBecameActive();
                }
            }
        }

        public bool IsRunning { get; private set; }

        public string ExePath { get; private set; }

        public Log Log { get; private set; }

        public Time Time { get; private set; }

        public Profiler Profiler { get; private set; }

        public Window Window { get; private set; }

        public GraphicsDriver Graphics { get; private set; }

        public ResourceManager ResourceManager { get; private set; }

        public Renderer Renderer { get; private set; }

        public Input Input { get; private set; }

        public Gui Gui { get; private set; }

        public static void Run<T>(GameConfig cfg) where T : Game, new() => new T().run(cfg);

        /// <summary>
        /// Returns absolute path for the given file relative to executable path.
        /// </summary>
        public string GetAbsolutePath(string file) => Path.Combine(ExePath, file);

        /// <summary>
        /// Quits the game
        /// </summary>
        public void Quit() => IsRunning = false;

        protected virtual void Load() { }

        protected virtual void OnUpdate(float dt) { }

        protected virtual void OnRender() => Renderer.Render(ActiveScene);

        protected virtual void Unload() { }

        void run(GameConfig cfg)
        {
            Current = this;
            IsRunning = true;
            
            ExePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Log = new Log(GetAbsolutePath(cfg.LogFile));
            Time = new Time();
            Profiler = new Profiler();
            Window = new SDLWindow(cfg);
            Graphics = new GLDriver();
            ResourceManager = new ResourceManager();
            Renderer = new Renderer();
            Input = new Input();
            Gui = new Gui();

            Load();
            while (IsRunning)
            {
                Time.NewFrame();
                Profiler.NewFrame();
                Input.PreWindowEvents();
                Window.DoEvents();
                Input.PostWindowEvents();
                OnUpdate(Time.FrameTime);
                ActiveScene?.NewFrame();
                OnRender();
                Window.SwapBuffers();
            }
            Unload();

            ObjectBase.Destroy(Gui);
            ObjectBase.Destroy(Input);
            ObjectBase.Destroy(Renderer);
            ObjectBase.Destroy(ResourceManager);
            ObjectBase.Destroy(Graphics);
            ObjectBase.Destroy(Window);
            ObjectBase.Destroy(Log);

            Current = null;
            IsRunning = false;
        }
    }
}