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
            cfg.Vsync = true;
            App.Run(cfg, new MyGame());
        }
    }

    public class MyGame : Scene
    {
        Camera cam2;
        MeshBuffer mb;

        public override void OnBegin()
        {
            base.OnBegin();
            Spawn<SimpleEntity>("New Sprite entity !!!");
            MainCamera.Viewport = new Rect(0, 0, .25f, .5f);
            cam2 = new Camera();
            cam2.Viewport = new Rect(.5f, .5f, .5f, .25f);
            cam2.ClearColor = Color.Green;
            cam2.Target = RenderTarget.Create(cam2.PixelViewport.Size);

            Array<Vertex> a = new Array<Vertex>(6);
            a.Push(new Vertex(0, 0, 0, 0, Color.White));
            a.Push(new Vertex(1, 1, 0, 0, Color.White));
            a.Push(new Vertex(1, 0, 0, 0, Color.White));

            a.Push(new Vertex(0, 0, 0, 0, Color.White));
            a.Push(new Vertex(1, 1, 0, 0, Color.White));
            a.Push(new Vertex(0, 1, 0, 0, Color.White));

            mb = MeshBuffer.Create(a);
        }

        protected override void OnRender()
        {
            base.OnRender();
            cam2.Render(this);

            var gfx = App.Graphics;
            gfx.ViewMatrix = MainCamera.ViewMatrix;
            gfx.SetViewport(MainCamera.PixelViewport);
            gfx.SetShader(DefaultShaders.ColorMult);
            DefaultShaders.ColorMult.SetTexture("main_tex", 0, cam2.Target[0]);
            DefaultShaders.ColorMult.SetMatrix4x4("view_mat", gfx.ViewMatrix);
            mb.Draw(PrimitiveType.Triangles);
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

           // Scene.DebugDraw.FillRect(new Rect(0, 0, 1, 1), Color.Red);
           // Scene.DebugDraw.FillRect(new Rect(2, 2, 1, 2), Color.White);
            //DynamicBatch.Draw(xform, test_sheet, 0);
        }


    }
}
