using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public class Shadow : RenderPass
    {
        Array<Vector2> shadowKernel;
        RenderTarget shadowDepth;
        RenderTarget shadowMap;
        Texture2D blurInput;
        Texture2D noise;
        Vector2 blurDir;
        int size = 2048;

        public IReadOnlyArray<Vector2> ShadowSampleKernel => shadowKernel;

        public Texture2D Noise => noise;

        public RenderTarget ShadowDepth => shadowDepth;

        public RenderTarget ShadowMap => shadowMap;

        public Vector3 Center { get; set; }

        public float Softness { get; set; } = 30;

        public float Bias { get; set; } = 0.05f;

        public float Range { get; set; } = 50;

        public int RenderSize
        {
            get => size;
            set
            {
                value = System.Math.Max(128, value);
                size = Math.NextPot(value);
                buildShadowDepthRT();
            }
        }

        public override void Draw(Scene scene)
        {
            if (Renderer.Lighting.SunLight)
            {
                var camera = scene.MainCamera;

                // render shadow depth
                {
                    Graphics.RenderTarget = shadowDepth;
                    Graphics.Viewport = new Rect(0, 0, shadowDepth.Width, shadowDepth.Height);
                    Graphics.SetClearValue(0, float.MaxValue);
                    Graphics.Clear(BufferMask.All);

                    Graphics.SetBlendMode(BlendMode.Disable);
                    Graphics.SetDepthTest(DepthTest.Less);
                    Graphics.SetDepthWrite(true);

                    var shader = Renderer.GetShader("ShadowDepthPass");
                    var drawArr = scene.GetNodesByGroup("Node.Draw");
                    if (drawArr != null)
                    {
                        for (int i = 0; i < drawArr.Count; i++)
                        {
                            var node = drawArr.GetItemAt(i) as IDrawNode;
                            Graphics.SetShader(shader, node);
                            Graphics.Draw(node.MeshBuffer);
                        }
                    }
                }

                // render shadow map
                {
                    Graphics.RenderTarget = shadowMap;
                    Graphics.Viewport = new Rect(0, 0, shadowMap.Width, shadowMap.Height);
                    Graphics.SetClearValue(0, Color.White);
                    Graphics.Clear(BufferMask.Color);

                    Graphics.SetDepthTest(DepthTest.Disable);
                    Graphics.SetDepthWrite(false);

                    Graphics.SetShader(Renderer.GetShader("ShadowMap"));
                    Graphics.Draw(MeshBuffer.ScreenQuad);
                }

                // blur
                {
                    var tmpBlur = Renderer.getTempRT(0, 0);

                    Graphics.SetClearValue(0, Color.Black);
                    Graphics.SetBlendMode(BlendMode.Disable);
                    Graphics.SetDepthTest(DepthTest.Disable);
                    Graphics.SetDepthWrite(false);
                    var shader = Renderer.GetShader("Blur.Shadow");


                    // horizontal
                    Graphics.Viewport = new Rect(0, 0, tmpBlur.Width, tmpBlur.Height);
                    Graphics.RenderTarget = tmpBlur;
                    blurDir = Vector2.UnitX * Softness;
                    blurInput = shadowMap[0];
                    Graphics.SetShader(shader);
                    Graphics.Clear(BufferMask.Color);
                    Graphics.Draw(MeshBuffer.ScreenQuad);

                    // vertical
                    Graphics.RenderTarget = shadowMap;
                    Graphics.Viewport = new Rect(0, 0, shadowMap.Width, shadowMap.Height);
                    blurDir = Vector2.UnitY * Softness;
                    blurInput = tmpBlur[0];
                    Graphics.SetShader(shader);
                    Graphics.Clear(BufferMask.Color);
                    Graphics.Draw(MeshBuffer.ScreenQuad);
                }
            }
        }

        protected override void OnBegin()
        {
            {
                var dpass = Renderer.BuildShader("ShadowDepthPass", "shadow-dpass.vert", null, "shadow-dpass.frag");
                var matrixId = dpass.GetUniformID("ModelMatrix");
                var biasId = dpass.GetUniformID("Bias");
                dpass.OnSetUniforms = (object obj) =>
                {
                    var node = obj as IDrawNode;
                    var matrix = node.Matrix;
                    dpass.SetUniform(matrixId, ref matrix);
                    dpass.SetUniform(biasId, Bias / Range);
                };
            }

            {
                var shadowMap = Renderer.BuildShader("ShadowMap", "post-fx.vert", null, "shadow-collect.frag");
                var depthId = shadowMap.GetUniformID("GBufferDepth");
                var shadowDepthId = shadowMap.GetUniformID("ShadowDepth");

                shadowMap.OnSetUniforms = (object obj) =>
                {
                    shadowMap.SetUniform(depthId, 0, GBuffer.DepthTex);
                    shadowMap.SetUniform(shadowDepthId, 1, ShadowDepth[0]);
                };
            }

            {
                var blur = Renderer.BuildShader("Blur.Shadow", "post-fx.vert", null, "Blur.frag", "NUM_SAMPLES 4", "DEPTH_AWARE", "DEPTH_FUNC(d) (1.0 / (d + 1.0))");

                var depthId = blur.GetUniformID("GBufferDepth");
                var inputId = blur.GetUniformID("InputTex");
                var dirId = blur.GetUniformID("Direction");
                blur.OnSetUniforms = (obj) =>
                {
                    blur.SetUniform(depthId, 0, Renderer.GBuffer.DepthTex);
                    blur.SetUniform(inputId, 1, blurInput);
                    blur.SetUniform(dirId, blurDir);
                };
            }

            Random rand = new Random(21);

            shadowKernel = new Array<Vector2>(32);
            for (int i = 0; i < 32; i++)
            {
                var length = rand.NextFloat(0.1f, 1f);
                var sample = rand.NextDir2D();
                shadowKernel.Push(sample);
            }

            Image noiseImage = new Image(4, 4);
            for (int i = 0; i < noiseImage.Width * noiseImage.Height; i++)
            {
                var vec = rand.NextDir2D() * 0.5f + new Vector2(0.5f);
                noiseImage[i] = new Color(vec.X, vec.Y, 1);
            }

            noise = Graphics.CreateTexture2D(noiseImage, FilterMode.Point, WrapMode.Repeat, 0, false);
            noiseImage.Dispose();

            buildShadowDepthRT();
        }

        protected override void OnDestroy()
        {
            Destroy(shadowDepth);
            Destroy(shadowMap);
            Destroy(noise);
        }

        protected override void OnReset(int w, int h)
        {
            Destroy(shadowMap);
            shadowMap = Graphics.CreateRenderTarget(w, h, PixelFormat.R8);
            shadowMap[0].FilterMode = FilterMode.Linear;
        }

        void buildShadowDepthRT()
        {
            Destroy(shadowDepth);
            shadowDepth = Graphics.CreateRenderTarget(size, size, PixelFormat.R32F);
            shadowDepth[0].FilterMode = FilterMode.Linear;
        }
    }

}