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

    public class SimpleEntity : Entity, IDrawable
    {
        MeshBuffer mb;
        Texture tex;
        Shader shader;

        public int Layer => 0;

        public Rect Bounds => new Rect(0, 0, 1, 1);

        public override void OnBegin()
        {
            Array<Vertex> verts = new Array<Vertex>();
            verts.Push(new Vertex(0, 0, 0, 0, Color.Red));
            verts.Push(new Vertex(1, 0, 0, 0, Color.Red));
            verts.Push(new Vertex(0, 1, 0, 0, Color.Red));
            verts.Push(new Vertex(1, 1, 0, 0, Color.Red));
            mb = MeshBuffer.Create(verts);
            tex = Texture.Create(1, 1, FilterMode.Point, WrapMode.Clamp, new Color[] { Color.White });
            shader = Shader.Load("shaders/geom_test.glsl");
            App.Graphics.SetPointSize(5);
        }

        public void Draw()
        {
            var gfx = App.Graphics;
            gfx.SetShader(shader);
            shader.SetMatrix3x2("model_mat", Matrix3x2.Identity);
            shader.SetMatrix4x4("view_mat", gfx.ViewMatrix);
            shader.SetTexture("main_tex", 0, tex);
            shader.SetFloat("size", .1f);
            mb.Draw(PrimitiveType.Points);
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

            Scene.RegisterForDraw(this);
        }


    }
}
