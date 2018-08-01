using System;
using System.Diagnostics;

namespace Engine
{

    /// <summary>
    /// Time related info about the app
    /// </summary>
    public sealed class Time
    {
        Stopwatch sw = new Stopwatch();
        float speed = 1;
        float ft;
        float time;

        /// <summary>
        /// Gets the total elapsed time since start of the app, in seconds.
        /// </summary>
        public float SinceStart => time;

        /// <summary>
        /// Total time spent on the last frame.
        /// </summary>
        public float FrameTime => ft;

        /// <summary>
        /// Gets or sets the speed of time.
        /// </summary>
        public float Speed
        {
            get => speed;
            set => speed = Math.Max(0, value);
        }

        internal void NewFrame()
        {
            ft = (float)sw.Elapsed.TotalSeconds;
            time += ft * speed;
            sw.Restart();
        }
    }
}