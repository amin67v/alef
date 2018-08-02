using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        public Shader GetShader(string name) => ResourceManager.Get<Shader>("Shaders." + name);

        public Shader BuildShader(string name, string vertFiles, string geomFiles, string fragFiles, params string[] defines)
        {
            string readFiles(string files)
            {
                var arr = files.Split(' ');
                string content = "";
                for (int i = 0; i < arr.Length; i++)
                    content += File.ReadAllText(Game.Current.GetAbsolutePath($"Shaders/{arr[i]}"));

                return content;
            }

            var common = readFiles("common.glsl");
            string def = "";
            for (int i = 0; i < defines.Length; i++)
                def += $"#define {defines[i]}\n";

            string start = $"#version 330 core\n{def}\n{common}\n";

            var vert = start + readFiles(vertFiles);

            string geom = null;
            if (geomFiles != null)
                geom = start + readFiles(geomFiles);

            var frag = start + readFiles(fragFiles);

            var shader = Graphics.CreateShader(vert, geom, frag);
            shader.SetUniformBlock("Constants", 0);

            ResourceManager.Add("Shaders." + name, shader);
            return shader;
        }

        unsafe void setShaderConstants(CameraNode camera)
        {
            ShaderConstants constants;
            constants.InvViewMatrix = camera.Matrix;
            Matrix4x4.Invert(camera.Matrix, out constants.ViewMatrix);

            constants.FieldOfView = camera.FieldOfView * Math.DegToRad;
            constants.WindowSize = new Vector2(Window.Width, Window.Height);

            constants.RenderSize = new Vector2(RenderWidth, RenderHeight);

            var aspect = constants.WindowSize.X / (float)constants.WindowSize.Y;
            constants.NearClip = camera.NearClip;
            constants.FarClip = camera.FarClip;
            constants.ProjMatrix = Matrix4x4.CreatePerspectiveFieldOfView(constants.FieldOfView, aspect, constants.NearClip, constants.FarClip);
            Matrix4x4.Invert(constants.ProjMatrix, out constants.InvProjMatrix);

            constants.ViewProjMatrix = constants.ViewMatrix * constants.ProjMatrix;
            constants.InvViewProjMatrix = constants.InvProjMatrix * constants.InvViewMatrix;

            constants.CameraPos = new Vector4(camera.WorldPosition, 1);
            constants.CameraDir = new Vector4(camera.Forward, 0);

            constants.Time = Time.SinceStart;

            var shadowCamPos = Shadow.Center - Lighting.SunDirection * Shadow.Range * 2;
            var shadowViewMatrix = Matrix4x4.CreateLookAt(shadowCamPos, Shadow.Center, Shadow.Center == Vector3.UnitY ? Vector3.UnitZ : Vector3.UnitY);
            var shadowProMatrix = Matrix4x4.CreateOrthographic(Shadow.Range, Shadow.Range, 0, Shadow.Range * 4);
            constants.ShadowMatrix = shadowViewMatrix * shadowProMatrix;

            var ptr = new IntPtr(&constants);
            Graphics.SetUniformBlock(0, ptr, Marshal.SizeOf<ShaderConstants>());
        }

        [StructLayout(LayoutKind.Sequential)]
        struct ShaderConstants
        {
            public Matrix4x4 ViewMatrix;
            public Matrix4x4 ProjMatrix;
            public Matrix4x4 ViewProjMatrix;
            public Matrix4x4 InvViewMatrix;
            public Matrix4x4 InvProjMatrix;
            public Matrix4x4 InvViewProjMatrix;
            public Matrix4x4 ShadowMatrix;

            public Vector4 CameraPos;
            public Vector4 CameraDir;
            public Vector2 WindowSize;
            public Vector2 RenderSize;
            public float FieldOfView;
            public float FarClip;
            public float NearClip;
            public float Time;
        }
    }
}