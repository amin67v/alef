using System;
using System.Diagnostics;

namespace Engine
{
    public sealed class Time
    {
        Stopwatch sw = new Stopwatch();
        float ft;
        float speed = 1;
        float time;
        float utime;

        internal Time() { }

        /// <summary>
        /// Gets the total elapsed time since start of the app, in seconds.
        /// </summary>
        public float Total => time;

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
            set => speed = MathF.Max(0, value);
        }

        internal void process()
        {
            ft = (float)sw.Elapsed.TotalSeconds;
            time += ft * speed;
            sw.Restart();
        }

        internal void shutdown()
        {

        }
    }
}