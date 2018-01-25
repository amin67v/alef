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
        Shader fxaa;
        MeshBuffer mb;

        public override void OnBegin()
        {
            base.OnBegin();
            Spawn<SimpleEntity>("New Sprite entity !!!");
            
        }
    }

    public class SimpleEntity : Entity
    {
        public override void OnBegin()
        {

        }

        public override void OnUpdate(float dt)
        {
            var cam = Scene.MainCamera;

            float x = 0;
            float y = 0;
            float r = 0;
            float z = 0;

            if (Input.IsKeyDown(KeyCode.A))
                x -= dt;

            if (Input.IsKeyDown(KeyCode.D))
                x += dt;

            if (Input.IsKeyDown(KeyCode.S))
                y -= dt;

            if (Input.IsKeyDown(KeyCode.W))
                y += dt;

            if (Input.IsKeyDown(KeyCode.E))
                r += dt;

            if (Input.IsKeyDown(KeyCode.Q))
                r -= dt;

            if (Input.IsKeyDown(KeyCode.F))
                z += dt * 100;

            if (Input.IsKeyDown(KeyCode.R))
                z -= dt * 100;

            cam.Position += new Vector2(x, y) * cam.ViewSize * (Input.IsKeyDown(KeyCode.LShift) ? 4f : .2f);

            cam.Rotation += r;
            cam.ViewSize += z;
        }


    }
}
