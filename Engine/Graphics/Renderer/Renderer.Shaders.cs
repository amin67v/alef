using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        Shader getShader(string name) => ResourceManager.Get<Shader>("Shaders." + name);

        Shader buildShader(string name, string vertFiles, string geomFiles, string fragFiles, params string[] defines)
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

        void buildShaders()
        {
            buildShader("GBufferFill", "gfill.vert", null, "gfill.frag");
            buildShader("AmbientLight", "post-fx.vert", null, "ambient-light.frag");
            buildShader("PointLight", "point-light.vert", null, "point-light.frag");
            buildShader("SunLight", "post-fx.vert", null, "sun-light.frag");
            buildShader("ShadowDepthPass", "shadow-dpass.vert", null, "shadow-dpass.frag");
            buildShader("ShadowMap", "post-fx.vert", null, "shadow-collect.frag");
            buildShader("LightCompose", "post-fx.vert", null, "light-compose.frag");
            buildShader("DepthOffset", "post-fx.vert", null, "DepthOffset.frag");

            buildShader("FXAA", "post-fx.vert", null, "fxaa.frag", "FXAA_QUALITY__PRESET 12");
            buildShader("Debug", "post-fx.vert", null, "debug.frag");

            setUniforms();
        }

        void setUniforms()
        {
            {
                var gfill = getShader("GBufferFill");
                var albedoId = gfill.GetUniformID("Albedo");
                var surfaceId = gfill.GetUniformID("Surface");
                var colorId = gfill.GetUniformID("Color");
                var matrixId = gfill.GetUniformID("ModelMatrix");
                var reflectId = gfill.GetUniformID("ReflectionSource");

                gfill.OnSetUniforms = (object obj) =>
                {
                    var node = obj as IDrawNode;
                    gfill.SetUniform(albedoId, 0, node.Material.Albedo);
                    gfill.SetUniform(surfaceId, 1, node.Material.SurfaceMap);
                    gfill.SetUniform(reflectId, 2, Lighting.ReflectionSource);
                    gfill.SetUniform(colorId, node.Material.Color);
                    var matrix = node.Matrix;
                    gfill.SetUniform(matrixId, ref matrix);
                };
            }

            {
                var pointlight = getShader("PointLight");
                var albedoId = pointlight.GetUniformID("GBufferAlbedo");
                var surfaceId = pointlight.GetUniformID("GBufferSurface");
                var normalId = pointlight.GetUniformID("GBufferNormal");
                var depthId = pointlight.GetUniformID("GBufferDepth");
                var matrixId = pointlight.GetUniformID("ModelMatrix");
                var colorId = pointlight.GetUniformID("Color");
                var lightDataId = pointlight.GetUniformID("LightPosRadius");
                pointlight.OnSetUniforms = (object obj) =>
                {
                    var node = obj as LightNode;
                    pointlight.SetUniform(albedoId, 0, GBuffer.AlbedoTex);
                    pointlight.SetUniform(surfaceId, 1, GBuffer.SurfaceTex);
                    pointlight.SetUniform(normalId, 2, GBuffer.NormalTex);
                    pointlight.SetUniform(depthId, 3, GBuffer.DepthTex);
                    var matrix = node.Matrix;
                    pointlight.SetUniform(matrixId, ref matrix);
                    pointlight.SetUniform(colorId, node.Color.ToVector4() * node.Intensity);
                    pointlight.SetUniform(lightDataId, new Vector4(node.WorldPosition, node.Radius));
                };
            }

            {
                var sunlight = getShader("SunLight");
                var albedoId = sunlight.GetUniformID("GBufferAlbedo");
                var surfaceId = sunlight.GetUniformID("GBufferSurface");
                var normalId = sunlight.GetUniformID("GBufferNormal");
                var depthId = sunlight.GetUniformID("GBufferDepth");
                var colorId = sunlight.GetUniformID("Color");
                var lightDirId = sunlight.GetUniformID("LightDir");
                var depthOffsetId = sunlight.GetUniformID("DepthOffset");
                var shadowmapId = sunlight.GetUniformID("ShadowMap");

                sunlight.OnSetUniforms = (object obj) =>
                {
                    sunlight.SetUniform(albedoId, 0, GBuffer.AlbedoTex);
                    sunlight.SetUniform(surfaceId, 1, GBuffer.SurfaceTex);
                    sunlight.SetUniform(normalId, 2, GBuffer.NormalTex);
                    sunlight.SetUniform(depthId, 3, GBuffer.DepthTex);
                    sunlight.SetUniform(depthOffsetId, 4, GBuffer.DepthOffsetTex);
                    sunlight.SetUniform(shadowmapId, 5, Shadow.ShadowMap[0]);
                    sunlight.SetUniform(colorId, Lighting.SunColor.ToVector4() * lighting.SunIntensity);
                    sunlight.SetUniform(lightDirId, Lighting.SunDirection);
                };
            }

            {
                var shadowdpass = getShader("ShadowDepthPass");
                var matrixId = shadowdpass.GetUniformID("ModelMatrix");
                var biasId = shadowdpass.GetUniformID("Bias");
                shadowdpass.OnSetUniforms = (object obj) =>
                {
                    var node = obj as IDrawNode;
                    var matrix = node.Matrix;
                    shadowdpass.SetUniform(matrixId, ref matrix);
                    shadowdpass.SetUniform(biasId, Shadow.Bias / shadow.Range);
                };
            }

            {
                var doffset = getShader("DepthOffset");
                var depthId = doffset.GetUniformID("GBufferDepth");
                doffset.OnSetUniforms = (object obj) =>
                {
                    doffset.SetUniform(depthId, 0, GBuffer.DepthTex);
                };
            }

            {
                var ambient = getShader("AmbientLight");
                var albedoId = ambient.GetUniformID("GBufferAlbedo");
                var surfaceId = ambient.GetUniformID("GBufferSurface");
                var normalId = ambient.GetUniformID("GBufferNormal");
                var aoBufferId = ambient.GetUniformID("AoBuffer");
                var aoPowerId = ambient.GetUniformID("AoPower");
                var skyId = ambient.GetUniformID("SkyAmbient");
                var groundId = ambient.GetUniformID("GroundAmbient");

                ambient.OnSetUniforms = (object obj) =>
                {
                    ambient.SetUniform(albedoId, 0, GBuffer.AlbedoTex);
                    ambient.SetUniform(surfaceId, 1, GBuffer.SurfaceTex);
                    ambient.SetUniform(normalId, 2, GBuffer.NormalTex);
                    ambient.SetUniform(aoBufferId, 3, SSAO.Texture);
                    ambient.SetUniform(skyId, Lighting.SkyColor.ToVector3() * Lighting.SkyIntensity);
                    ambient.SetUniform(groundId, Lighting.GroundColor.ToVector3() * Lighting.GroundIntensity);
                    ambient.SetUniform(aoPowerId, SSAO.Power);
                };
            }

            {
                var compose = getShader("LightCompose");
                var depthId = compose.GetUniformID("GBufferDepth");
                var lbufferId = compose.GetUniformID("LightBuffer");
                var fogColorId = compose.GetUniformID("FogColor");
                var fogdensId = compose.GetUniformID("FogDensity");
                var exposureId = compose.GetUniformID("Exposure");

                compose.OnSetUniforms = (object obj) =>
                {
                    compose.SetUniform(depthId, 0, GBuffer.DepthTex);
                    compose.SetUniform(lbufferId, 1, Lighting.LightBuffer[0]);
                    compose.SetUniform(fogColorId, Color.Linear(Lighting.FogColor).ToVector4() * Lighting.FogIntensity);
                    compose.SetUniform(fogdensId, Lighting.FogDensity);
                    compose.SetUniform(exposureId, Lighting.Exposure);
                };
            }

            {
                var shadowMap = getShader("ShadowMap");
                var depthId = shadowMap.GetUniformID("GBufferDepth");
                var shadowDepthId = shadowMap.GetUniformID("ShadowDepth");

                shadowMap.OnSetUniforms = (object obj) =>
                {
                    shadowMap.SetUniform(depthId, 0, GBuffer.DepthTex);
                    shadowMap.SetUniform(shadowDepthId, 1, Shadow.ShadowDepth[0]);
                };
            }

            {
                var fxaa = getShader("FXAA");
                var texId = fxaa.GetUniformID("ScreenTexture");
                var rcpFrameId = fxaa.GetUniformID("RcpFrame");
                var subpixId = fxaa.GetUniformID("Subpix");
                var edgeThresholdId = fxaa.GetUniformID("EdgeThreshold");
                var edgeThresholdMinId = fxaa.GetUniformID("EdgeThresholdMin");

                fxaa.OnSetUniforms = (object obj) =>
                {
                    fxaa.SetUniform(texId, 0, Lighting.RenderTarget[0]);
                    fxaa.SetUniform(rcpFrameId, new Vector2(1f / Window.Width, 1f / Window.Height));
                    fxaa.SetUniform(subpixId, FXAA.SubPix);
                    fxaa.SetUniform(edgeThresholdId, FXAA.EdgeThreshold);
                    fxaa.SetUniform(edgeThresholdMinId, FXAA.EdgeThresholdMin);
                };
            }

            {
                var debug = getShader("Debug");
                var texId = debug.GetUniformID("InputTex");
                var singleId = debug.GetUniformID("SingleChannel");
                debug.OnSetUniforms = (object obj) =>
                {
                    var tex = obj as Texture2D;
                    debug.SetUniform(texId, 0, tex);
                    var singleChannel = tex.PixelFormat == PixelFormat.R8 ||
                                        tex.PixelFormat == PixelFormat.R16 ||
                                        tex.PixelFormat == PixelFormat.R32F;
                    debug.SetUniform(singleId, singleChannel ? 1 : 0);
                };
            }
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