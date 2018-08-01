using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        public class GBufferFill : DisposeBase
        {
            RenderTarget gBuffer;
            RenderTarget depthHalf;
            RenderTarget normalHalf;
            RenderTarget depthOffset;
            float depthOffsetBias = 0.02f;

            public RenderTarget GBufferRT => gBuffer;

            public Texture2D AlbedoTex => gBuffer[0];

            public Texture2D SurfaceTex => gBuffer[1];

            public Texture2D NormalTex => gBuffer[2];

            public Texture2D DepthTex => gBuffer[3];

            public Texture2D HalfDepthTex => depthHalf[0];

            public Texture2D HalfNormalTex => normalHalf[0];

            public Texture2D DepthOffsetTex => depthOffset[0];

            public float DepthOffsetBias
            {
                get => depthOffsetBias;
                set => depthOffsetBias = value;
            }

            internal GBufferFill(Renderer r)
            {
                r.OnReset += (w, h) =>
                {
                    Destroy(gBuffer);
                    gBuffer = Graphics.CreateRenderTarget(w, h, PixelFormat.Rgba8, PixelFormat.Rgba8, PixelFormat.Rg16F, PixelFormat.R32F);

                    Destroy(depthHalf);
                    depthHalf = Graphics.CreateRenderTarget(w / 2, h / 2, PixelFormat.R32F);

                    Destroy(normalHalf);
                    normalHalf = Graphics.CreateRenderTarget(w / 2, h / 2, PixelFormat.Rg16F);

                    Destroy(depthOffset);
                    depthOffset = Graphics.CreateRenderTarget(w, h, PixelFormat.Rg8);
                };

            }

            internal void InternalRender(Scene scene)
            {
                Graphics.RenderTarget = gBuffer;
                Graphics.SetClearValue(0, Color.Black);
                Graphics.SetClearValue(1, Color.Black);
                Graphics.SetClearValue(2, Color.Black);
                Graphics.SetClearValue(3, -1000000000);

                Graphics.Clear(BufferMask.All, 4);
                Graphics.SetBlendMode(BlendMode.Disable);
                Graphics.SetDepthTest(DepthTest.Less);
                Graphics.SetDepthWrite(true);

                // todo : frustum cull drawables
                // todo : split solid and transparent objects
                // todo : sort front to back ?

                var drawArr = scene.GetNodesByGroup("Node.Draw");
                if (drawArr != null)
                {
                    var shader = Renderer.getShader("GBufferFill");
                    for (int i = 0; i < drawArr.Count; ++i)
                    {
                        var node = drawArr.GetItemAt(i) as IDrawNode;
                        Graphics.SetShader(shader, node);
                        Graphics.Draw(node.MeshBuffer);
                    }
                }

                Graphics.BlitRenderTarget(gBuffer, depthHalf, 3, BufferMask.Color, FilterMode.Point);
                Graphics.BlitRenderTarget(gBuffer, normalHalf, 2, BufferMask.Color, FilterMode.Point);
                Renderer.drawPass(depthOffset, Renderer.getShader("DepthOffset"));
            }

            protected override void OnDispose(bool manual)
            {
                Destroy(gBuffer);
            }
        }

    }
}