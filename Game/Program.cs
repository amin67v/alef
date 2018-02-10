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
            var cfg = new AppConfig("Game App!!", 1280, 720);
            App.Run(cfg, new MyGame());
        }
    }

    public class MyGame : Scene
    {
        SimpleEntity entity;
        public override void OnEnter()
        {
            base.OnEnter();
            entity = Entity.Spawn<SimpleEntity>("New Sprite entity !!!");
        }

        Vector2 btn_pos = Vector2.One * 100;
        void on_gui(Gui gui)
        {
            //var particle = entity.RootNode as ParticleSystem;
            //
            //gui.Text(particle.AliveCount.ToString());
            //gui.Combo<ParticleSortMode>("Sort Mode", ref particle.SortMode);
            //
            //gui.Combo<TweenType>("Rotation Tween", ref particle.RotateTween);
            //gui.InputFloat("Start Rotation", ref particle.RotateStart, 0, 0, 2, InputTextFlags.Default);
            //gui.InputFloat("End Rotation", ref particle.RotateEnd, 0, 0, 2, InputTextFlags.Default);
            //
            //gui.Combo<TweenType>("Size Tween", ref particle.SizeTween);
            //gui.InputFloat("Start Size", ref particle.SizeStart, 0, 0, 2, InputTextFlags.Default);
            //gui.InputFloat("End Size", ref particle.SizeEnd, 0, 0, 2, InputTextFlags.Default);
            //gui.Gradient("Color By Life", particle.ColorGradient);
            //
            //int emit_per_sec = particle.EmitPerSecond;
            //gui.InputInt("Emit Per Second", ref emit_per_sec, 0, 0, InputTextFlags.Default);
            //particle.EmitPerSecond = emit_per_sec;

        }

        protected override void OnRender()
        {
            MainCamera.Render(this);
            Gui.Render(on_gui);
        }
    }

    public class SimpleEntity : Entity
    {
        protected override void OnBegin()
        {
            CreateRootNode<EntityNode>();
            //CreateRootNode<ParticleSystem>();

            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    var sprite = RootNode.CreateChild<SpriteNode>("sprite1");
                    sprite.Frame = SpriteSheet.Load("sprites/BeardedMan.spr")["bearded-idle-1"];
                    sprite.Position = new Vector2(x, y);
                }
            }
        }

        protected override void OnUpdate(float dt)
        {
            var cam = Scene.Active.MainCamera;

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

            if (Input.IsKeyDown(KeyCode.Right))
                RootNode.Rotation += dt;

            if (Input.IsKeyDown(KeyCode.Left))
                RootNode.Rotation -= dt;
        }


    }
}
