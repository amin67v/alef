using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Engine
{
    public sealed class App
    {
        internal static App instance;

        Platform platform = Platform.Windows;
        IAppState act_state = null;
        IAppState nxt_state = null;
        IGraphicsDevice gfx;
        Window cwindow;
        Action quit_override;
        string exe_path = null;
        bool is_running = false;

        App() { }

        /// <summary>
        /// Gets or sets the active state for the current app
        /// Note: changing active state does happen at the end of the frame and not immediately.
        /// </summary>
        public static IAppState ActiveState
        {
            get => instance.act_state;
            set => instance.nxt_state = value;
        }

        /// <summary>
        /// Gets graphics devices for the current app
        /// </summary>
        public static IGraphicsDevice Graphics => instance.gfx;

        /// <summary>
        /// Gets the window object for the current app
        /// </summary>
        public static Window Window => instance.cwindow;

        /// <summary>
        /// Path to the app
        /// </summary>
        public static string ExePath => instance.exe_path;

        /// <summary>
        /// Current runtime platform
        /// </summary>
        public static Platform Platform => instance.platform;

        /// <summary>
        /// Running the app with the specified config and the initial state.
        /// </summary>
        public static void Run(AppConfig cfg, IAppState state)
        {
            if (instance != null)
                throw new Exception("App is already running!");

            instance = new App();
            instance.run(cfg, state); // runs main loop
            instance = null;
        }

        /// <summary>
        /// Quit the app.
        /// </summary>
        public static void Quit()
        {
            if (instance.quit_override == null)
            {
                instance.is_running = false;
            }
            else
            {
                instance.quit_override();
            }
        }

        /// <summary>
        /// Overrides quit function
        /// </summary>
        public static void OverrideQuit(Action quit) => instance.quit_override = quit;

        /// <summary>
        /// Returns absolute path for the given file relative to executable path.
        /// </summary>
        public static string GetAbsolutePath(string file) => Path.Combine(ExePath, file);

        void run(AppConfig cfg, IAppState state)
        {
            is_running = true;

            // detect runtime platform
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                platform = Platform.Windows;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                platform = Platform.Linux;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                platform = Platform.OSX;
            else
                platform = Platform.Unknown;

            // determine and store executable path
            exe_path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Log.init(GetAbsolutePath(cfg.LogFile));
            cwindow = Window.init(cfg);
            Time.init();
            gfx = new OpenglDevice();
            Log.Info($"Graphics Driver:\n{gfx.DriverInfo}");
            DataCache.init();
            Input.init();

            if (state != null)
            {
                act_state = state;
                act_state.OnBegin();
            }
            else
            {
                throw new Exception("Initial state cant be null.");
            }

            while (is_running)
            {
                Time.update();
                Input.update();
                Window.DoEvents();
                ActiveState.OnFrame();
                Window.SwapBuffers();

                if (nxt_state != null)
                {
                    act_state.OnEnd();
                    act_state = nxt_state;
                    act_state.OnBegin();
                    nxt_state = null;
                }
            }

            ActiveState.OnEnd();
            nxt_state = act_state = null;
            Input.shutdown();
            Gui.shutdown();
            DataCache.shutdown();
            Window.ShutDown();
            Time.shutdown();
            Log.shutdown();
        }
    }
}