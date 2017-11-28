using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;

using Engine;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = new AppConfig("Game App!!", 800, 600);
            App.Run(cfg, new MyGame());
        }
    }

    public class MyGame : AppState
    {
        Shader shader;
        Text text;

        public override void OnResize(int width, int height)
        {
            Graphics.Viewport(0, 0, width, height);
        }

        public override void OnBegin()
        {
            var fnt = Font.Load("fonts/roboto_light_8.json");
            shader = Shader.Load("shaders/default.glsl");

            text = new Text("The quick brown fox jumps over the lazy dog.\n1234567890\n~!@#$%^&*()_+|", fnt, Color.Black);
            text.Color = Color.White;

            text.Transform.Position = new Vector2(200, 200);
        }

        public override void OnRender()
        {
            Graphics.Clear(Color.Black);

            var rect = new Rect(Vector2.Zero, Window.Size);
            Graphics.SetView(rect.X, rect.XMax, rect.YMax, rect.Y);
            text.Draw();
            Graphics.Display();
        }

        public override void OnKeyDown(Keys key, bool alt, bool ctrl, bool shift)
        {

        }

        public override void OnMouseMove(Vector2 pos)
        {

        }

        public override void OnKeyChar(string code)
        {
            if (code[0] == '\b' && text.Content != null && text.Content.Length > 0)
                text.Content = text.Content.Substring(0, text.Content.Length - 1);
            else if (code[0] == '\r')
                text.Content += '\n';
            else
                text.Content += code;
        }
    }
}
