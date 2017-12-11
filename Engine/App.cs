using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Engine
{
    public static class App
    {
        static string exe_path = null;
        static Platform platform = Platform.Windows;
        static bool running = false;

        /// <summary>
        /// Path to the running app.
        /// </summary>
        public static string ExePath => exe_path;

        /// <summary>
        /// Current runtime platform.
        /// </summary>
        public static Platform Platform => platform;

        /// <summary>
        /// Quit the app.
        /// </summary>
        public static void Quit() => running = false;

        /// <summary>
        /// Running the app with the specified config and the initial state.
        /// </summary>
        public static void Run(AppConfig cfg, AppState state)
        {
            if (running)
            {
                Log.Warning("App is already running.");
                return;
            }
            running = true;

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

            Time.init();
            Log.init();
            Window.init(cfg);
            Graphics.init();
            Resource.init(false);
            AppState.init(state);

            while (running)
            {
                Time.process();
                AppState.Active.OnFrame();
                Window.swap_buffers();
                Window.do_events();
                AppState.process();
            }

            AppState.shut_down();
            Resource.shut_down();
            Graphics.shut_down();
            Window.shut_down();
            Log.shut_down();
            Time.shut_down();
        }
    }
}