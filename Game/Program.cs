using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;

using System.IO.Compression;

using Engine;
using Random = Engine.Random;
using static Engine.Math;

namespace Game
{

    class Program
    {
        static void Main(string[] args)
        {
            Engine.Game.Run<MyGame>(new GameConfig(1280, 720, false, false));
        }
    }

    class MyGame : Engine.Game
    {
        CameraNode camera;
        Vector3 camRot;

        public override string Name => "My Test Game !";

        protected override void Load()
        {
            ActiveScene = new Scene();
            ActiveScene.MainCamera = camera = ActiveScene.Spawn<CameraNode>("Camera");
            camera.Position = new Vector3(12f, 18f, 20f);
            camera.Rotation = FromEuler(new Vector3(-32, 40, 0) * DegToRad);

            Renderer.Lighting.ReflectionSource = CubeMap.Load("DaySkybox.json");

            var modelNode = ActiveScene.Spawn<ModelNode>("Model");
            modelNode.Model = Model.Load("TestModel2.dae");

            camRot = ToEuler(camera.Rotation) * RadToDeg;
        }

        protected override void OnUpdate(float dt)
        {
            if (Input.IsKeyDown(MouseButton.Right))
            {
                camRot.Y -= Input.MouseDeltaPosition.X * 0.1f;
                camRot.X -= Input.MouseDeltaPosition.Y * 0.1f;
            }

            camera.Rotation = FromEuler(camRot * DegToRad);
            camRot = ToEuler(camera.Rotation) * RadToDeg;

            float zMove = 0;
            float xMove = 0;

            if (Input.IsKeyDown(KeyCode.W))
                zMove -= 0.5f;

            if (Input.IsKeyDown(KeyCode.S))
                zMove += 0.5f;

            if (Input.IsKeyDown(KeyCode.A))
                xMove -= 0.5f;

            if (Input.IsKeyDown(KeyCode.D))
                xMove += 0.5f;

            if (Input.IsKeyDown(KeyCode.LShift))
            {
                zMove *= 10f;
                xMove *= 10f;
            }

            camera.Position += camera.Forward * zMove * dt * 4f;
            camera.Position += camera.Right * xMove * dt * 4f;

            if (Input.IsKeyPressed(KeyCode.Escape))
                Quit();
        }

        protected override void OnRender()
        {
            Renderer.Render(ActiveScene);
            Gui.Render(onGui);
        }

        void onGui(Gui gui)
        {
            gui.TextLabel("Frame Time", (Time.FrameTime * 1000).ToString("0.00 ms"));
            Renderer.Debug = gui.EnumEdit("Debug View", Renderer.Debug);

            if (gui.Header("Environment Light"))
            {
                Renderer.Lighting.Exposure = gui.FloatEdit("Exposure", Renderer.Lighting.Exposure);

                Renderer.Lighting.SkyColor = gui.ColorEdit("SkyColor", Renderer.Lighting.SkyColor);
                Renderer.Lighting.GroundColor = gui.ColorEdit("GroundColor", Renderer.Lighting.GroundColor);
                Renderer.Lighting.SkyIntensity = gui.FloatEdit("SkyIntensity", Renderer.Lighting.SkyIntensity);
                Renderer.Lighting.GroundIntensity = gui.FloatEdit("GroundIntensity", Renderer.Lighting.GroundIntensity);

                Renderer.SSAO.Radius = gui.FloatEdit("SSAO Radius", Renderer.SSAO.Radius);
                Renderer.SSAO.Quality = gui.EnumEdit<SSAOQuality>("SSAO Quality", Renderer.SSAO.Quality);

                Renderer.SSAO.Power = gui.FloatEdit("SSAO Power", Renderer.SSAO.Power);

                Renderer.Lighting.FogColor = gui.ColorEdit("Fog Color", Renderer.Lighting.FogColor);
                Renderer.Lighting.FogIntensity = gui.FloatEdit("Fog Intensity", Renderer.Lighting.FogIntensity);
                Renderer.Lighting.FogDensity = gui.FloatEdit("Fog Density", Renderer.Lighting.FogDensity);
            }

            if (gui.Header("Sun Light"))
            {
                Renderer.Lighting.SunLight = gui.BoolEdit("Enabled", Renderer.Lighting.SunLight);
                Renderer.Lighting.SunColor = gui.ColorEdit("Color", Renderer.Lighting.SunColor);
                Renderer.Lighting.SunIntensity = gui.FloatEdit("Intensity", Renderer.Lighting.SunIntensity);
                Renderer.Lighting.SunDirection = gui.Vector3Edit("Direction", Renderer.Lighting.SunDirection);
                Renderer.Shadow.Center = gui.Vector3Edit("Shadow Center", Renderer.Shadow.Center);
                Renderer.Shadow.Range = gui.FloatEdit("Shadow Volume", Renderer.Shadow.Range);
                Renderer.Shadow.Bias = gui.FloatEdit("Shadow Bias", Renderer.Shadow.Bias);
                Renderer.Shadow.Softness = gui.FloatEdit("Shadow Softness", Renderer.Shadow.Softness);
            }
        }
    }

}
