using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

using Engine;

class Program
{
    static void Main(string[] args)
    {
        AppConfig cfg;
        cfg.Title = "Data Editor";
        cfg.Width = 1280;
        cfg.Height = 720;
        cfg.Vsync = true;
        cfg.Resizable = true;
        cfg.Fullscreen = false;
        cfg.LogFile = "data-editor-log.txt";
        App.Run(cfg, new DataEditor());
    }
}