using System;
using System.Diagnostics;

namespace Engine
{
    /// <summary>
    /// Simple frame and memory profiling
    /// </summary>
    public class Profiler
    {
        Stopwatch sw = new Stopwatch();
        Array<Sample> samples = new Array<Sample>();

        public IReadOnlyArray<Sample> Samples => samples;

        internal Profiler() { }

        internal void NewFrame()
        {
            samples.Clear();
            sw.Restart();
        }

        public void BeginSample(string name)
        {
            Sample s = new Sample();
            s.Name = name;
            s.Begin = (float)sw.Elapsed.TotalMilliseconds;
            s.Length = 0;
            samples.Push(s);
        }

        public void EndSample()
        {
            samples.Last.Length = (float)sw.Elapsed.TotalMilliseconds - samples.Last.Begin;
        }

        public struct Sample
        {
            public string Name;
            public float Begin;
            public float Length;
        }
    }
}