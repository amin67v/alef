using System;

namespace Engine
{
    public class GBuffer : RenderPass
    {
        RenderTarget gbufferRT;
        RenderTarget halfDepthRT;
        RenderTarget halfNormalRT;

        public Texture2D AlbedoTex => gbufferRT[0];

        public Texture2D SurfaceTex => gbufferRT[1];

        public Texture2D NormalTex => gbufferRT[2];

        public Texture2D DepthTex => gbufferRT[3];

        public Texture2D HalfDepthTex => halfDepthRT[0];

        public Texture2D HalfNormalTex => halfNormalRT[0];

        public void BlitDepth(RenderTarget target)
        {
            Graphics.BlitRenderTarget(gbufferRT, target, 3, BufferMask.Depth, FilterMode.Point);
        }

        public void BlitNormal(RenderTarget target)
        {
            Graphics.BlitRenderTarget(gbufferRT, target, 2, BufferMask.Depth, FilterMode.Point);
        }

        public override void Draw(Scene scene)
        {
            Graphics.RenderTarget = gbufferRT;
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
                var shader = Renderer.GetShader("GBufferFill");
                for (int i = 0; i < drawArr.Count; ++i)
                {
                    var node = drawArr.GetItemAt(i) as IDrawNode;
                    Graphics.SetShader(shader, node);
                    Graphics.Draw(node.MeshBuffer);
                }
            }

            BlitDepth(halfDepthRT);
            BlitNormal(halfNormalRT);
        }

        protected override void OnBegin()
        {
            var gfill = Renderer.BuildShader("GBufferFill", "gfill.vert", null, "gfill.frag");
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
                gfill.SetUniform(reflectId, 2, Renderer.Lighting.ReflectionSource);
                gfill.SetUniform(colorId, node.Material.Color);
                var matrix = node.Matrix;
                gfill.SetUniform(matrixId, ref matrix);
            };
        }

        protected override void OnDestroy()
        {
            Destroy(gbufferRT);
            Destroy(halfDepthRT);
            Destroy(halfNormalRT);
        }

        protected override void OnReset(int w, int h)
        {
            Destroy(gbufferRT);
            gbufferRT = Graphics.CreateRenderTarget(w, h, PixelFormat.Rgba8, PixelFormat.Rgba8, PixelFormat.Rg16F, PixelFormat.R32F);

            Destroy(halfDepthRT);
            halfDepthRT = Graphics.CreateRenderTarget(w / 2, h / 2, PixelFormat.R32F);

            Destroy(halfNormalRT);
            halfNormalRT = Graphics.CreateRenderTarget(w / 2, h / 2, PixelFormat.Rg16F);
        }
    }
}