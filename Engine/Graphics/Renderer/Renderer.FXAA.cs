using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        public class Fxaa : DisposeBase
        {
            Material material;

            float subPix = 0.75f;
            float edgeThreshold = 0.166f;
            float edgeThresholdMin = 0.0625f;

            public float SubPix
            {
                get => subPix;
                set => subPix = Math.Clamp(value, 0, 1);
            }

            public float EdgeThreshold
            {
                get => edgeThreshold;
                set => edgeThreshold = Math.Clamp(value, 0.063f, 0.333f);
            }

            public float EdgeThresholdMin
            {
                get => edgeThresholdMin;
                set => edgeThresholdMin = Math.Clamp(value, 0.0312f, 0.0833f);
            }

            internal Fxaa(Renderer renderer)
            {
                material = new Material();
                material.BlendMode = BlendMode.Disable;
            }

            internal void InternalRender()
            {
                Graphics.RenderTarget = Renderer.Output;
                Graphics.Clear(BufferMask.All);
                Graphics.SetShader(Renderer.getShader("FXAA"));
                Graphics.SetBlendMode(BlendMode.Disable);
                Graphics.Draw(MeshBuffer.ScreenQuad);
            }
        }
    }
}