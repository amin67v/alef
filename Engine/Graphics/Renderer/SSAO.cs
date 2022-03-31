using System;
using System.Numerics;

namespace Engine
{
    
    public enum SSAOQuality { Disabled, Normal, High }

    public class SSAO : RenderPass
    {
        const int k_NumKernel = 16;
        const int k_NormalSamples = 8;
        const int k_HighSamples = 16;

        Matrix4x4 prevViewProj;
        Matrix4x4 prevInvViewProj;
        Vector3 prevCameraPos;
        Array<Vector3> kernel;
        RenderTarget rt;
        RenderTarget temporalRT;
        Texture2D inputTex;
        Texture2D[] noise;
        Vector2 blurDir;
        int noiseIndex = 0;

        public float Radius { get; set; } = 4;

        public float Power { get; set; } = 4;

        public float BlurStride { get; set; } = 1.0f;

        public Texture2D Texture => (Quality == SSAOQuality.Disabled) ? Texture2D.White : rt[0];

        public SSAOQuality Quality { get; set; } = SSAOQuality.High;

        public override void Draw(Scene Scene)
        {
            if (Quality == SSAOQuality.Disabled)
                return;

            var blurTempRT = Renderer.getTempRT(0, 0);
            blurTempRT[0].FilterMode = FilterMode.Linear;

            DrawScreenEffect(rt, Renderer.GetShader("SSAO.1stPass." + (int)Quality));

            // vertical blur
            inputTex = rt[0];
            blurDir = Vector2.UnitX * BlurStride;
            DrawScreenEffect(blurTempRT, Renderer.GetShader("SSAO.BlurPass"));
            // horizontal blur
            inputTex = blurTempRT[0];
            blurDir = Vector2.UnitY * BlurStride;
            DrawScreenEffect(rt, Renderer.GetShader("SSAO.BlurPass"));

            Graphics.BlitRenderTarget(rt, temporalRT, 0, BufferMask.Color, FilterMode.Point);
        }

        protected override void OnBegin()
        {
            {
                var rand = new Random(87456);
                kernel = new Array<Vector3>(k_NumKernel);
                for (int i = 0; i < k_NumKernel; i++)
                {
                    var length = rand.NextFloat(0.1f, 1f);
                    var sample = rand.NextDir3D() * length * length;
                    kernel.Push(sample);
                }

                noise = new Texture2D[16];
                for (int j = 0; j < noise.Length; j++)
                {
                    Image noiseImage = new Image(128, 128);
                    for (int i = 0; i < noiseImage.Width * noiseImage.Height; i++)
                    {
                        var vec = rand.NextDir3D();
                        vec = vec * 0.5f + new Vector3(0.5f);
                        noiseImage[i] = new Color(vec.X, vec.Y, vec.Z);
                    }

                    noise[j] = Graphics.CreateTexture2D(noiseImage, FilterMode.Point, WrapMode.Repeat, 0, false);
                    noiseImage.Dispose();
                }

            }

            {
                Renderer.BuildShader("SSAO.1stPass.1", "post-fx.vert", null, "SSAO.1stPass.frag", $"NUM_SAMPLES {k_NormalSamples}", $"NUM_KERNEL {k_NumKernel}"); // ssao normal
                Renderer.BuildShader("SSAO.1stPass.2", "post-fx.vert", null, "SSAO.1stPass.frag", $"NUM_SAMPLES {k_HighSamples}", $"NUM_KERNEL {k_NumKernel}"); // ssao high

                for (int i = 1; i <= 2; i++)
                {
                    var ssao = Renderer.GetShader("SSAO.1stPass." + i);
                    var normalId = ssao.GetUniformID("GBufferNormal");
                    var depthId = ssao.GetUniformID("GBufferDepth");
                    var noiseId = ssao.GetUniformID("NoiseTex");
                    var temporalId = ssao.GetUniformID("TemporalTex");
                    var prevDepthId = ssao.GetUniformID("PrevDepthTex");
                    var kernelId = ssao.GetUniformID("SampleKernel");
                    var radiusId = ssao.GetUniformID("Radius");
                    var bufferSizeId = ssao.GetUniformID("BufferSize");
                    var prevViewProjId = ssao.GetUniformID("PrevViewProjMatrix");

                    ssao.OnSetUniforms = (object obj) =>
                    {
                        ssao.SetUniform(normalId, 0, GBuffer.NormalTex);
                        ssao.SetUniform(depthId, 1, GBuffer.DepthTex);
                        noiseIndex++;
                        noiseIndex %= noise.Length;
                        ssao.SetUniform(noiseId, 2, noise[noiseIndex]);
                        ssao.SetUniform(temporalId, 3, temporalRT[0]);
                        ssao.SetUniform(prevDepthId, 4, GBuffer.PrevHalfDepthTex);
                        ssao.SetUniform(kernelId, kernel);
                        ssao.SetUniform(radiusId, Radius);
                        ssao.SetUniform(bufferSizeId, new Vector2(Graphics.RenderTarget.Width, Graphics.RenderTarget.Height));
                        ssao.SetUniform(prevViewProjId, ref prevViewProj);
                        prevViewProj = Renderer.Constants.ViewProjMatrix;
                    };
                }
            }

            {
                var blur = Renderer.BuildShader("SSAO.BlurPass", "post-fx.vert", null, "SSAO.BlurPass.frag");
                var inputTexId = blur.GetUniformID("InputTex");
                var depthId = blur.GetUniformID("GBufferDepth");
                var normalId = blur.GetUniformID("GBufferNormal");
                var dirId = blur.GetUniformID("Direction");
                blur.OnSetUniforms = (object obj) =>
                {
                    blur.SetUniform(inputTexId, 0, inputTex);
                    blur.SetUniform(depthId, 1, GBuffer.DepthTex);
                    blur.SetUniform(normalId, 2, GBuffer.NormalTex);
                    blur.SetUniform(dirId, blurDir);
                };
            }
        }

        protected override void OnDestroy()
        {
            for (int i = 0; i < noise.Length; i++)
                Destroy(noise[i]);

            noise = null;
            Destroy(rt);
        }

        protected override void OnReset(int w, int h)
        {
            Destroy(rt);
            rt = Graphics.CreateRenderTarget(w, h, false, PixelFormat.R8);
            rt[0].FilterMode = FilterMode.Linear;

            Destroy(temporalRT);
            temporalRT = Graphics.CreateRenderTarget(w, h, false, PixelFormat.R8);
            temporalRT[0].FilterMode = FilterMode.Linear;
        }

    }
}