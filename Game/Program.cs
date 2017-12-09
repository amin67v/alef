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
            Camera.ViewSize = 10;
            Camera.SizeMode = Camera.ViewSizeMode.Height;
            Spawn<SimpleEntity>("New Sprite entity !!!");
        }
    }

    public class SimpleEntity : Entity, IDrawable
    {
        Texture heart;
        Shader shader;
        MeshBuffer mesh;

        public bool IsVisible() => true;

        public int SortKey => 0;

        public override void OnBegin()
        {
            heart = Texture.Load("heart.png");
            shader = Shader.Load("shaders/default.glsl");
            shader = Shader.Load("shaders/default.glsl");
            Array<Vertex> vertices = new Array<Vertex>(6);
            vertices.Push(new Vertex(-.5f, -.5f, 0, 0, Color.White));
            vertices.Push(new Vertex(0.5f, 0.5f, 1, 1, Color.White));
            vertices.Push(new Vertex(0.5f, -.5f, 1, 0, Color.White));
            vertices.Push(new Vertex(-.5f, -.5f, 0, 0, Color.White));
            vertices.Push(new Vertex(0.5f, 0.5f, 1, 1, Color.White));
            vertices.Push(new Vertex(-.5f, 0.5f, 0, 1, Color.White));
            mesh = MeshBuffer.Create(vertices);
        }

        public void Draw()
        {
            Graphics.BlendMode = BlendMode.AlphaBlend;
            Shader.Active = shader;
            shader.SetTexture("tex", 0, heart);
            shader.SetMatrix4x4("view", Graphics.ViewMatrix);
            mesh.Draw(PrimitiveType.Triangles);
        }
    }
}
