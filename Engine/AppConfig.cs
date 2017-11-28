using System;
using System.IO;
using System.Json;

namespace Engine
{
    public struct AppConfig
    {
        public string Title;
        public int Width;
        public int Height;
        public bool Resizable;
        public bool Fullscreen;
        public bool Vsync;

        public AppConfig(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            Resizable = false;
            Fullscreen = false;
            Vsync = false;
        }


        public static AppConfig FromFile(string file)
        {
            AppConfig cfg;
            var json = JsonObject.Parse(File.ReadAllText(file));
            cfg.Title = json["title"];
            cfg.Width = json["width"];
            cfg.Height = json["height"];
            cfg.Fullscreen = json["fullscreen"];
            cfg.Resizable = json["resizable"];
            cfg.Vsync = json["vsync"];
            return cfg;
        }

        public void Save(string file)
        {
            var jsonObj = new JsonObject();
            jsonObj.Add("title", Title);
            jsonObj.Add("width", Width);
            jsonObj.Add("height", Height);
            jsonObj.Add("fullscreen", Fullscreen);
            jsonObj.Add("resizable", Resizable);
            jsonObj.Add("vsync", Vsync);
            var json = JsonHelper.PrettifyJson(jsonObj.ToString());
            File.WriteAllText(file, json);
        }
    }
}