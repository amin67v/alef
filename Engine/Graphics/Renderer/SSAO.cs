using System;
using System.Numerics;

namespace Engine
{
    public enum SSAOQuality { Disabled, Normal, High }

    public class SSAO : RenderPass
    {
        Array<Vector3> kernel;
        RenderTarget rt;
        Texture2D inputTex;
        Texture2D noise;
        Vector2 blurDir;

        public float Radius { get; set; } = 4;

        public float Power { get; set; } = 2;

        public Texture2D Texture => (Quality == SSAOQuality.Disabled) ? Texture2D.White : rt[0];

        public SSAOQuality Quality { get; set; } = SSAOQuality.Normal;

        public override void Draw(Scene Scene)
        {
            if (Quality == SSAOQuality.Disabled)
                return;

            var tmpBuffer = Renderer.getTempRT(0, 0);
            tmpBuffer[0].FilterMode = FilterMode.Linear;

            DrawScreenEffect(rt, Renderer.GetShader("SSAO.1stPass." + (int)Quality));

            // vertical blur
            inputTex = rt[0];
            blurDir = Vector2.UnitX;
            DrawScreenEffect(tmpBuffer, Renderer.GetShader("SSAO.BlurPass"));

            // horizontal blur
            inputTex = tmpBuffer[0];
            blurDir = Vector2.UnitY;
            DrawScreenEffect(rt, Renderer.GetShader("SSAO.BlurPass"));
        }

        protected override void OnBegin()
        {
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

            {
                Renderer.BuildShader("SSAO.1stPass.1", "post-fx.vert", null, "SSAO.1stPass.frag", "NUM_SAMPLES 8"); // ssao normal
                Renderer.BuildShader("SSAO.1stPass.2", "post-fx.vert", null, "SSAO.1stPass.frag", "NUM_SAMPLES 16"); // ssao high

                for (int i = 1; i <= 2; i++)
                {
                    var ssao = Renderer.GetShader("SSAO.1stPass." + i);
                    var normalId = ssao.GetUniformID("GBufferNormal");
                    var depthId = ssao.GetUniformID("GBufferDepth");
                    var noiseId = ssao.GetUniformID("NoiseTex");
                    var kernelId = ssao.GetUniformID("SampleKernel");
                    var radiusId = ssao.GetUniformID("Radius");

                    ssao.OnSetUniforms = (object obj) =>
                    {
                        ssao.SetUniform(normalId, 0, GBuffer.NormalTex);
                        ssao.SetUniform(depthId, 1, GBuffer.DepthTex);
                        ssao.SetUniform(noiseId, 2, noise);
                        ssao.SetUniform(kernelId, kernel);
                        ssao.SetUniform(radiusId, Radius);
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
            Destroy(noise);
            Destroy(rt);
        }

        protected override void OnReset(int w, int h)
        {
            Destroy(rt);
            rt = Graphics.CreateRenderTarget(w, h, false, PixelFormat.R8);
            rt[0].FilterMode = FilterMode.Linear;
        }

    }
}