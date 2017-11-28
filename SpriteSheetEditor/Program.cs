using System;
using System.Numerics;

using Engine;
using Editor;

namespace SpriteSheetEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            AppConfig cfg;
            cfg.Title = "Sprite Sheet Editor";
            cfg.Height = 600;
            cfg.Width = 800;
            cfg.Vsync = false;
            cfg.Resizable = true;
            cfg.Fullscreen = false;
            App.Run(cfg, new SpriteSheetEditor());
        }
    }

    class SpriteSheetEditor : EditorBase
    {

    }
}
