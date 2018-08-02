using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public class FXAA : RenderPass
    {
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

        public override void Draw(Scene scene)
        {
            DrawScreenEffect(Renderer.Output, Renderer.GetShader("FXAA"));
        }

        protected override void OnBegin()
        {
            var fxaa = Renderer.BuildShader("FXAA", "post-fx.vert", null, "fxaa.frag", "FXAA_QUALITY__PRESET 12");
            var texId = fxaa.GetUniformID("ScreenTexture");
            var rcpFrameId = fxaa.GetUniformID("RcpFrame");
            var subpixId = fxaa.GetUniformID("Subpix");
            var edgeThresholdId = fxaa.GetUniformID("EdgeThreshold");
            var edgeThresholdMinId = fxaa.GetUniformID("EdgeThresholdMin");

            fxaa.OnSetUniforms = (object obj) =>
            {
                fxaa.SetUniform(texId, 0, Renderer.Lighting.LightComposeTexture);
                fxaa.SetUniform(rcpFrameId, new Vector2(1f / Window.Width, 1f / Window.Height));
                fxaa.SetUniform(subpixId, SubPix);
                fxaa.SetUniform(edgeThresholdId, EdgeThreshold);
                fxaa.SetUniform(edgeThresholdMinId, EdgeThresholdMin);
            };
        }
    }
}
