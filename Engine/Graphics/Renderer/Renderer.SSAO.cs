using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public enum SSAOQuality { Disabled, Normal, High }

    public partial class Renderer : ObjectBase
    {
        public class AmbientOcclusion : ObjectBase
        {
            Array<Vector3> kernel;
            RenderTarget rt;
            Texture2D inputTex;
            Texture2D noise;
            Material material;
            Vector2 blurDir;
            float radius = 4f;
            float power = 2f;
            SSAOQuality quality = SSAOQuality.Normal;

            internal AmbientOcclusion(Renderer r)
            {
                r.OnReset += (w, h) =>
                {
                    Destroy(rt);
                    rt = Graphics.CreateRenderTarget(w, h, false, PixelFormat.R8);
                    rt[0].FilterMode = FilterMode.Linear;
                };

                setupNoise();
                setupShaders(r);
            }

            public float Radius
            {
                get => radius;
                set => radius = value;
            }

            public float Power
            {
                get => power;
                set => power = value;
            }

            public SSAOQuality Quality
            {
                get => quality;
                set => quality = value;
            }

            public Texture2D Texture => Quality == SSAOQuality.Disabled ? Texture2D.White : rt[0];

            internal void InternalRender()
            {
                if (Quality == SSAOQuality.Disabled)
                    return;

                var tmpBuffer = Renderer.getTempRT(0, 0);
                tmpBuffer[0].FilterMode = FilterMode.Linear;

                Renderer.drawPass(rt, Renderer.getShader("SSAO.1stPass." + (int)Quality));

                // vertical blur
                inputTex = rt[0];
                blurDir = Vector2.UnitX;
                Renderer.drawPass(tmpBuffer, Renderer.getShader("SSAO.BlurPass"));

                // horizontal blur
                inputTex = tmpBuffer[0];
                blurDir = Vector2.UnitY;
                Renderer.drawPass(rt, Renderer.getShader("SSAO.BlurPass"));
            }

            protected override void OnDestroy()
            {
                Destroy(noise);
                Destroy(rt);
            }

            void setupNoise()
            {
                var rand = new Random(21);

                kernel = new Array<Vector3>(16);
                for (int i = 0; i < 16; i++)
                {
                    var length = rand.NextFloat(0.1f, 1f);
                    var sample = rand.NextDir3D() * length * length;
                    kernel.Push(sample);
                }

                Image noiseImage = new Image(8, 8);
                for (int i = 0; i < noiseImage.Width * noiseImage.Height; i++)
                {
                    var vec = rand.NextDir3D();
                    vec = vec * 0.5f + new Vector3(0.5f);
                    noiseImage[i] = new Color(vec.X, vec.Y, vec.Z);
                }
                
                noise = Graphics.CreateTexture2D(noiseImage, FilterMode.Point, WrapMode.Repeat, 0, false);
                noiseImage.Dispose();
            }

            void setupShaders(Renderer r)
            {
                r.buildShader("SSAO.1stPass.1", "post-fx.vert", null, "SSAO.1stPass.frag", "NUM_SAMPLES 8"); // ssao normal
                r.buildShader("SSAO.1stPass.2", "post-fx.vert", null, "SSAO.1stPass.frag", "NUM_SAMPLES 16"); // ssao high
                r.buildShader("SSAO.BlurPass", "post-fx.vert", null, "SSAO.BlurPass.frag");

                for (int i = 1; i <= 2; i++)
                {
                    var ssao = r.getShader("SSAO.1stPass." + i);
                    var normalId = ssao.GetUniformID("GBufferNormal");
                    var depthId = ssao.GetUniformID("GBufferDepth");
                    var noiseId = ssao.GetUniformID("NoiseTex");
                    var kernelId = ssao.GetUniformID("SampleKernel");
                    var radiusId = ssao.GetUniformID("Radius");

                    ssao.OnSetUniforms = (object obj) =>
                    {
                        ssao.SetUniform(normalId, 0, r.GBuffer.NormalTex);
                        ssao.SetUniform(depthId, 1, r.GBuffer.DepthTex);
                        ssao.SetUniform(noiseId, 2, noise);
                        ssao.SetUniform(kernelId, kernel);
                        ssao.SetUniform(radiusId, Radius);
                    };
                }

                {
                    var blur = r.getShader("SSAO.BlurPass");
                    var inputTexId = blur.GetUniformID("InputTex");
                    var depthId = blur.GetUniformID("GBufferDepth");
                    var normalId = blur.GetUniformID("GBufferNormal");
                    var dirId = blur.GetUniformID("Direction");
                    blur.OnSetUniforms = (object obj) =>
                    {
                        blur.SetUniform(inputTexId, 0, inputTex);
                        blur.SetUniform(depthId, 1, r.GBuffer.DepthTex);
                        blur.SetUniform(normalId, 2, r.GBuffer.NormalTex);
                        blur.SetUniform(dirId, blurDir);
                    };
                }
            }
        }
    }
}