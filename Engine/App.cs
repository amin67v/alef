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
        static AppState act_state = null;
        static AppState nxt_state = null;
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
        /// Gets or sets the active state of the app.
        /// Note that setting active state does happen at the end of the frame and not immediately.
        /// </summary>
        public static AppState ActiveState
        {
            get => act_state;
            set => nxt_state = value;
        }

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
                throw new Exception("App is already running.");

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

            Time.Init();
            Log.Init();
            Window.Init(cfg);
            Graphics.Init();
            Resource.Init();

            if (state != null)
            {
                act_state = state;
                act_state.OnBegin();
            }
            else
            {
                throw new Exception("Initial state cant be null.");
            }

            float accum = 0;
            while (running)
            {
                Time.Process();
                ActiveState.OnBeginFrame();

                accum += Time.FrameTime;
                var update_dt = Time.UpdateDeltaTime;
                while (accum >= update_dt)
                {
                    var dt = update_dt * Time.Speed;
                    ActiveState.OnUpdate(dt);
                    accum -= update_dt;
                }
                ActiveState.OnRender();

                Window.SwapBuffers();
                Window.DoEvents();

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

            Resource.Shutdown();
            Graphics.Shutdown();
            Window.Shutdown();
            Log.Shutdown();
            Time.Shutdown();
        }
    }
}