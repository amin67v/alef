using System;

namespace Engine
{
    public class AppConfig
    {
        public string Title;
        public string LogFile;
        public int Width;
        public int Height;
        public bool Resizable;
        public bool Fullscreen;
        public bool Vsync;

        public AppConfig() { }

        public AppConfig(string title, int width, int height)
        {
            Title = title;
            LogFile = "log.txt";
            Width = width;
            Height = height;
            Resizable = false;
            Fullscreen = false;
            Vsync = false;
        }
    }
}