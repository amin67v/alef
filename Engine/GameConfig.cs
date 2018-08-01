using System;

namespace Engine
{
    public class GameConfig
    {
        public string LogFile { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Resizable { get; set; }
        public bool Fullscreen { get; set; }
        public bool Vsync { get; set; }

        public GameConfig() : this(800, 600, false, false) { }

        public GameConfig(int width, int height, bool fullscreen, bool vsync)
        {
            LogFile = "log.txt";
            Width = width;
            Height = height;
            Resizable = false;
            Fullscreen = fullscreen;
            Vsync = vsync;
        }
    }
}