using System;
using System.Diagnostics;

namespace Engine
{
    /// <summary>
    /// Time related info about the current app
    /// </summary>
    public sealed class Time
    {
        static Time instance;

        Stopwatch sw = new Stopwatch();
        float speed = 1;
        float ft;
        float time;

        Time() { }

        /// <summary>
        /// Gets the total elapsed time since start of the app, in seconds.
        /// </summary>
        public static float SinceStart => instance.time;

        /// <summary>
        /// Total time spent on the last frame.
        /// </summary>
        public static float FrameTime => instance.ft;

        /// <summary>
        /// Gets or sets the speed of time.
        /// </summary>
        public static float Speed
        {
            get => instance.speed;
            set => instance.speed = MathF.Max(0, value);
        }

        internal static void update()
        {
            instance.ft = (float)instance.sw.Elapsed.TotalSeconds;
            instance.time += instance.ft * instance.speed;
            instance.sw.Restart();
        }

        internal static void init()
        {
            instance = new Time();
        }

        internal static void shutdown()
        {
            instance = null;
        }
    }
}