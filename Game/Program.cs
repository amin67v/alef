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
            //for (int i = 0; i < 50000; i++)
                DynamicBatch.Draw(xform, test_sheet, 0);
        }


    }
}
