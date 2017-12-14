using System;
using static CSFML;

namespace Engine
{
    public sealed class Time
    {
        IntPtr start_clock;
        float frame_time;
        float last_time;
        float speed;

        internal Time()
        {
            start_clock = sfClock_create();
            frame_time = 0;
            last_time = 0;
        }

        /// <summary>
        /// Gets the total elapsed time since start of the app, in seconds.
        /// </summary>
        public float SinceStart => sfClock_getElapsedTime(start_clock) /* <- microseconds */ * 0.000001f;

        /// <summary>
        /// Total time spent on the last last frame.
        /// </summary>
        public float FrameTime => frame_time;

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
            var now = SinceStart;
            frame_time = now - last_time;
            last_time = now;
        }

        internal void shutdown()
        {
            sfClock_destroy(start_clock);
            start_clock = IntPtr.Zero;
        }
    }
}