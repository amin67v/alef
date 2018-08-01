using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        public class ShadowPass : ObjectBase
        {
            Array<Vector2> shadowKernel;
            RenderTarget shadowDepth;
            RenderTarget shadowMap;
            Texture2D blurInput;
            Texture2D noise;
            Vector3 center;
            Vector2 blurDir;
            float range = 40f;
            float bias = 0.05f;
            float softness = 40f;
            int size = 1024;
            bool enabled = true;

            public IReadOnlyArray<Vector2> ShadowSampleKernel => shadowKernel;

            public Texture2D Noise => noise;

            public RenderTarget ShadowDepth => shadowDepth;

            public RenderTarget ShadowMap => shadowMap;

            public bool Enabled
            {
                get => enabled;
                set
                {
                    if (enabled == value)
                        return;

                    enabled = value;
                }
            }

            public Vector3 Center
            {
                get => center;
                set => center = value;
            }

            public float Softness
            {
                get => softness;
                set => softness = value;
            }

            public float Bias
            {
                get => bias;
                set => bias = value;
            }

            public float Range
            {
                get => range;
                set => range = value;
            }

            public int RenderSize
            {
                get => size;
                set
                {
                    value = System.Math.Max(128, value);
                    size = Math.NextPot(value);
                    buildShadowBuffer();
                }
            }

            internal ShadowPass(Renderer r)
            {
                r.OnReset += (w, h) =>
                {
                    Destroy(shadowMap);
                    shadowMap = Graphics.CreateRenderTarget(w, h, PixelFormat.R8);
                    shadowMap[0].FilterMode = FilterMode.Linear;
                };

                var blur = r.buildShader("Blur.Shadow", "post-fx.vert", null, "Blur.frag", "NUM_SAMPLES 4", "DEPTH_AWARE", "DEPTH_FUNC(d) (1.0 / (d + 1.0))");

                var depthId = blur.GetUniformID("GBufferDepth");
                var inputId = blur.GetUniformID("InputTex");
                var dirId = blur.GetUniformID("Direction");
                blur.OnSetUniforms = (obj) =>
                {
                    blur.SetUniform(depthId, 0, r.GBuffer.DepthTex);
                    blur.SetUniform(inputId, 1, blurInput);
                    blur.SetUniform(dirId, blurDir);
                };

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

                buildShadowBuffer();
            }

            internal void InternalRender(Scene scene)
            {
                if (Enabled && Renderer.Lighting.SunLight)
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

                        var shader = Renderer.getShader("ShadowDepthPass");
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

                        Graphics.SetShader(Renderer.getShader("ShadowMap"));
                        Graphics.Draw(MeshBuffer.ScreenQuad);
                    }

                    // blur
                    {
                        var tmpBlur = Renderer.getTempRT(0, 0);

                        Graphics.SetClearValue(0, Color.Black);
                        Graphics.SetBlendMode(BlendMode.Disable);
                        Graphics.SetDepthTest(DepthTest.Disable);
                        Graphics.SetDepthWrite(false);
                        var shader = Renderer.getShader("Blur.Shadow");


                        // horizontal
                        Graphics.Viewport = new Rect(0, 0, tmpBlur.Width, tmpBlur.Height);
                        Graphics.RenderTarget = tmpBlur;
                        blurDir = Vector2.UnitX * softness;
                        blurInput = shadowMap[0];
                        Graphics.SetShader(shader);
                        Graphics.Clear(BufferMask.Color);
                        Graphics.Draw(MeshBuffer.ScreenQuad);

                        // vertical
                        Graphics.RenderTarget = shadowMap;
                        Graphics.Viewport = new Rect(0, 0, shadowMap.Width, shadowMap.Height);
                        blurDir = Vector2.UnitY * softness;
                        blurInput = tmpBlur[0];
                        Graphics.SetShader(shader);
                        Graphics.Clear(BufferMask.Color);
                        Graphics.Draw(MeshBuffer.ScreenQuad);
                    }
                }
            }

            protected override void OnDestroy()
            {
                Destroy(shadowDepth);
                Destroy(shadowMap);
                Destroy(noise);
            }

            void buildShadowBuffer()
            {
                Destroy(shadowDepth);
                shadowDepth = Graphics.CreateRenderTarget(size, size, PixelFormat.R32F);
                shadowDepth[0].FilterMode = FilterMode.Linear;
            }
        }
    }
}