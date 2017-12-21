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
            var cfg = new AppConfig("Game App!!", 640, 480);
            cfg.Vsync = true;
            App.Run(cfg, new MyGame());
        }
    }

    public class MyGame : Scene
    {
        public override void OnBegin()
        {
            base.OnBegin();
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

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    xform.Position = new Vector2(i, j) * 2;
                    StaticBatch.AddSprite(xform, test_sheet, 0);
                }
            }
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
                z += dt * 100;

            if (App.Window.IsKeyDown(Keys.R))
                z -= dt * 100;

            var t = App.Time.SinceStart;
            Vector2 noise;
            noise.X = Noise.Perlin1D(t);
            noise.Y = Noise.Perlin1D(t + 123);

            cam.Position += new Vector2(x, y) * cam.ViewSize * (App.Window.IsKeyDown(Keys.LShift) ? 4f : .2f) + noise * .025f;

            cam.Rotation += r;
            cam.ViewSize += z;
        }


    }
}
