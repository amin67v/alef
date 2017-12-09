using System;
using static CSFML;

namespace Engine
{
    public static class Time
    {
        const float to_seconds = 0.000001f;

        static IntPtr start_clock;
        static float frame_time;
        static float last_time;
        static float speed;
        static float update_dt;
        static uint frame_idx;

        /// <summary>
        /// Gets the total elapsed time since start of the app, in seconds.
        /// </summary>
        public static float SinceStart => sfClock_getElapsedTime(start_clock) * to_seconds;

        /// <summary>
        /// Total time spent on the last last frame.
        /// </summary>
        public static float FrameTime => frame_time;

        /// <summary>
        /// The index of the current frame.
        /// </summary>
        public static uint FrameIndex => frame_idx;

        /// <summary>
        /// Gets or sets the speed of time.
        /// </summary>
        public static float Speed
        {
            get => speed;
            set => speed = MathF.Max(0, value);
        }

        public static float UpdateDeltaTime
        {
            get => update_dt;
            set => update_dt = MathF.Max(0, value);
        }

        internal static void init()
        {
            start_clock = sfClock_create();
            frame_time = 0;
            last_time = 0;
            frame_idx = 0;
            update_dt = 1f / 60f;
        }

        internal static void process()
        {
            var now = SinceStart;
            frame_time = now - last_time;
            last_time = now;
            frame_idx++;
        }

        internal static void shut_down()
        {
            sfClock_destroy(start_clock);
            start_clock = IntPtr.Zero;
        }
    }
}