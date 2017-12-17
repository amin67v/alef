using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Engine
{
    public class App
    {
        static App current;

        AppState act_state = null;
        AppState nxt_state = null;
        Platform platform = Platform.Windows;
        string exe_path = null;
        ResourceManager res_mgr;
        IGraphicsDevice gfx;
        Window window;
        Time time;
        Log log;
        bool is_running = false;

        App() { }

        /// <summary>
        /// Gets or sets the active state for the current app
        /// Note: changing active state does happen at the end of the frame and not immediately.
        /// </summary>
        public static AppState ActiveState
        {
            get => current.act_state;
            set => current.nxt_state = value;
        }

        /// <summary>
        /// Gets the window for the current app
        /// </summary>
        public static Window Window => current.window;

        /// <summary>
        /// Gets the resource manager for the current app
        /// </summary>
        public static ResourceManager ResourceManager => current.res_mgr;

        /// <summary>
        /// Gets time info for the current app
        /// </summary>
        public static Time Time => current.time;

        /// <summary>
        /// Logger object used to log current app
        /// </summary>
        public static Log Log => current.log;

        /// <summary>
        /// Gets graphics devices for the current app
        /// </summary>
        public static IGraphicsDevice Graphics => current.gfx;

        /// <summary>
        /// Path to the app
        /// </summary>
        public static string ExePath => current.exe_path;

        /// <summary>
        /// Current runtime platform
        /// </summary>
        public static Platform Platform => current.platform;

        /// <summary>
        /// Running the app with the specified config and the initial state.
        /// </summary>
        public static void Run(AppConfig cfg, AppState state)
        {
            if (current != null)
                throw new Exception("App is already running!");

            current = new App();
            current.run(cfg, state); // runs main loop
            current = null;
        }

        /// <summary>
        /// Quit the app.
        /// </summary>
        public static void Quit() => current.is_running = false;

        void run(AppConfig cfg, AppState state)
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

            log = new Log("log.txt");
            time = new Time();
            window = new Window(cfg);
            gfx = new OpenglDevice();
            App.Log.Info($"Graphics Driver:\n{gfx.DriverInfo}\n");
            res_mgr = new ResourceManager();
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
                time.process();
                ActiveState.OnFrame();
                window.swap_buffers();
                window.do_events();
                
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
            ResourceManager.shutdown();
            //Graphics.shutdown();
            window.shutdown();
            time.shutdown();
            Log.Dispose();
        }
    }
}