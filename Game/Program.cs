using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;

using System.IO.Compression;

using Engine;

namespace Game
{

    class Program
    {
        static void Main(string[] args)
        {
            var cfg = new AppConfig("Game App!!", 640, 480);
            //cfg.Vsync = true;
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
        SpriteSheet spr;
        Transform xform = new Transform();
        public int Layer => 0;

        public override void OnBegin()
        {
            spr = SpriteSheet.Load("sprites/test.json");

            //  var img = new Image(1024, 1024);
            //  for (int x = 0; x < 1024; x++)
            //  {
            //      for (int y = 0; y < 1024; y++)
            //      {
            //          byte r = (byte)(Noise.Perlin2D(x / 100f, y / 100f).Remap(-1, 1, 0, 255));
            //          img[x, y] = new Color(r, r, r);
            //      }
            //  }
            //  img.ToFile("perlin.jpg");
            //  img.Dispose();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    xform.Position = new Vector2(i, j) * 2;
                    StaticBatch.AddSprite(xform, spr, 0);
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

            if (App.Window.IsKeyDown(KeyCode.A))
                x -= dt;

            if (App.Window.IsKeyDown(KeyCode.D))
                x += dt;

            if (App.Window.IsKeyDown(KeyCode.S))
                y -= dt;

            if (App.Window.IsKeyDown(KeyCode.W))
                y += dt;

            if (App.Window.IsKeyDown(KeyCode.E))
                r += dt;

            if (App.Window.IsKeyDown(KeyCode.Q))
                r -= dt;

            if (App.Window.IsKeyDown(KeyCode.F))
                z += dt * 100;

            if (App.Window.IsKeyDown(KeyCode.R))
                z -= dt * 100;

            cam.Position += new Vector2(x, y) * cam.ViewSize * (App.Window.IsKeyDown(KeyCode.LShift) ? 4f : .2f);

            cam.Rotation += r;
            cam.ViewSize += z;
        }


    }
}
