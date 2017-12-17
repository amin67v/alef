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

    public class MyGame : Scene
    {
        public override void OnBegin()
        {
            base.OnBegin();
            MainCamera.ViewSize = 10;
            MainCamera.SizeMode = Camera.ViewSizeMode.Height;
            Spawn<SimpleEntity>("New Sprite entity !!!");
        }
    }

    public class SimpleEntity : Entity
    {
        SpriteSheet test_sheet;
        Transform xform = new Transform();
        public int Layer => 0;

        public override void OnBegin()
        {
            test_sheet = SpriteSheet.Load("sprites/test.json");
        }

        public override void OnUpdate(float dt)
        {
            var cam = Scene.MainCamera;
            float x = 0;
            float y = 0;
            float r = 0;
            float z = 0;

            if (App.Window.IsKeyDown(Keys.A))
                x -= dt;

            if (App.Window.IsKeyDown(Keys.D))
                x += dt;

            if (App.Window.IsKeyDown(Keys.S))
                y -= dt;

            if (App.Window.IsKeyDown(Keys.W))
                y += dt;

            if (App.Window.IsKeyDown(Keys.E))
                r += dt;

            if (App.Window.IsKeyDown(Keys.Q))
                r -= dt;

            if (App.Window.IsKeyDown(Keys.F))
                z += dt;

            if (App.Window.IsKeyDown(Keys.R))
                z -= dt;

            cam.Position += new Vector2(x, y);
            cam.Rotation += r;
            cam.ViewSize += z;

            Scene.DebugDraw.FillRect(new Rect(0, 0, 1, 1), Color.Red);
            Scene.DebugDraw.FillRect(new Rect(2, 2, 1, 2), Color.White);
            DynamicBatch.Draw(xform, test_sheet, 0);
        }


    }
}
