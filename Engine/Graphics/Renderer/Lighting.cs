using System;
using System.Numerics;

namespace Engine
{
    public class Lighting : RenderPass
    {
        RenderTarget composeRT;
        RenderTarget lightRT;
        MeshBuffer pointLightMesh;
        MeshBuffer[] skyboxMeshes;
        Vector3 sunDir = Vector3.Normalize(new Vector3(-1.11f, -1.22f, -1.33f));

        public Texture2D LightTexture => lightRT[0];

        public Texture2D LightComposeTexture => composeRT[0];

        public CubeMap ReflectionSource { get; set; }

        public Color FogColor { get; set; } = new Color(50, 90, 150, 255);

        public Color SunColor { get; set; } = new Color(255, 213, 170, 255);

        public Color SkyColor { get; set; } = new Color(130, 200, 255, 255);

        public Color GroundColor { get; set; } = new Color(80, 80, 70, 255);

        public float FogDensity { get; set; } = 0.003f;

        public float FogIntensity { get; set; } = 1;

        public float SunIntensity { get; set; } = 15;

        public float SkyIntensity { get; set; } = 2;

        public float GroundIntensity { get; set; } = 0.2f;

        public float Exposure { get; set; } = 1;

        public bool SunLight { get; set; } = true;

        public Vector3 SunDirection
        {
            get => sunDir;
            set => sunDir = Vector3.Normalize(value);
        }

        public override void Draw(Scene scene)
        {
            var camera = scene.MainCamera;

            GBuffer.BlitDepth(lightRT);

            // render point lights
            Graphics.RenderTarget = lightRT;
            Graphics.Viewport = new Rect(0, 0, lightRT.Width, lightRT.Height);
            Graphics.SetClearValue(0, Color.Black);
            Graphics.Clear(BufferMask.All);

            Graphics.SetBlendMode(BlendMode.Additive);
            Graphics.SetDepthTest(DepthTest.Disable);
            Graphics.SetDepthWrite(false);

            // ambient light
            Graphics.SetShader(Renderer.GetShader("AmbientLight"));
            Graphics.Draw(MeshBuffer.ScreenQuad);

            // sun light
            if (SunLight)
            {
                Graphics.SetShader(Renderer.GetShader("SunLight"));
                Graphics.Draw(MeshBuffer.ScreenQuad);
            }
            Graphics.SetDepthTest(DepthTest.Greater);

            // point lights
            var pointLightArr = scene.GetNodesByGroup("Renderer.PointLight");
            if (pointLightArr != null)
            {
                var shader = Renderer.GetShader("PointLight");
                for (int i = 0; i < pointLightArr.Count; i++)
                {
                    var node = pointLightArr.GetItemAt(i) as LightNode;
                    Graphics.SetShader(shader, node);
                    Graphics.Draw(pointLightMesh);
                }
            }

            // light combine pass
            DrawScreenEffect(composeRT, Renderer.GetShader("LightCompose"));
        }

        protected override void OnBegin()
        {
            pointLightMesh = Model.Load("Engine/PointLight.obj")[0].MeshBuffer;
            // skyboxMeshes = new MeshBuffer[6];
            // skyboxMeshes[0] = Model.Load("Engine/SkyBox.obj")["Right"].MeshBuffer;
            // skyboxMeshes[1] = Model.Load("Engine/SkyBox.obj")["Left"].MeshBuffer;
            // skyboxMeshes[2] = Model.Load("Engine/SkyBox.obj")["Top"].MeshBuffer;
            // skyboxMeshes[3] = Model.Load("Engine/SkyBox.obj")["Bottom"].MeshBuffer;
            // skyboxMeshes[4] = Model.Load("Engine/SkyBox.obj")["Back"].MeshBuffer;
            // skyboxMeshes[5] = Model.Load("Engine/SkyBox.obj")["Front"].MeshBuffer;

            {
                var pointlight = Renderer.BuildShader("PointLight", "point-light.vert", null, "point-light.frag");
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
                var sunlight = Renderer.BuildShader("SunLight", "post-fx.vert", null, "sun-light.frag");
                var albedoId = sunlight.GetUniformID("GBufferAlbedo");
                var surfaceId = sunlight.GetUniformID("GBufferSurface");
                var normalId = sunlight.GetUniformID("GBufferNormal");
                var depthId = sunlight.GetUniformID("GBufferDepth");
                var colorId = sunlight.GetUniformID("Color");
                var lightDirId = sunlight.GetUniformID("LightDir");
                var shadowmapId = sunlight.GetUniformID("ShadowMap");

                sunlight.OnSetUniforms = (object obj) =>
                {
                    sunlight.SetUniform(albedoId, 0, GBuffer.AlbedoTex);
                    sunlight.SetUniform(surfaceId, 1, GBuffer.SurfaceTex);
                    sunlight.SetUniform(normalId, 2, GBuffer.NormalTex);
                    sunlight.SetUniform(depthId, 3, GBuffer.DepthTex);
                    sunlight.SetUniform(shadowmapId, 4, Renderer.Shadow.ShadowMap[0]);
                    sunlight.SetUniform(colorId, SunColor.ToVector4() * SunIntensity);
                    sunlight.SetUniform(lightDirId, SunDirection);
                };
            }

            {
                var ambient = Renderer.BuildShader("AmbientLight", "post-fx.vert", null, "ambient-light.frag");
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
                    ambient.SetUniform(aoBufferId, 3, Renderer.SSAO.Texture);
                    ambient.SetUniform(skyId, SkyColor.ToVector3() * SkyIntensity);
                    ambient.SetUniform(groundId, GroundColor.ToVector3() * GroundIntensity);
                    ambient.SetUniform(aoPowerId, Renderer.SSAO.Power);
                };
            }

            {
                var compose = Renderer.BuildShader("LightCompose", "post-fx.vert", null, "light-compose.frag");
                var depthId = compose.GetUniformID("GBufferDepth");
                var lbufferId = compose.GetUniformID("LightBuffer");
                var fogColorId = compose.GetUniformID("FogColor");
                var fogdensId = compose.GetUniformID("FogDensity");
                var exposureId = compose.GetUniformID("Exposure");

                compose.OnSetUniforms = (object obj) =>
                {
                    compose.SetUniform(depthId, 0, GBuffer.DepthTex);
                    compose.SetUniform(lbufferId, 1, lightRT[0]);
                    compose.SetUniform(fogColorId, Color.Linear(FogColor).ToVector4() * FogIntensity);
                    compose.SetUniform(fogdensId, FogDensity);
                    compose.SetUniform(exposureId, Exposure);
                };
            }
        }

        protected override void OnDestroy()
        {
            Destroy(lightRT);
            Destroy(composeRT);
        }

        protected override void OnReset(int w, int h)
        {
            Destroy(lightRT);
            lightRT = Graphics.CreateRenderTarget(w, h, PixelFormat.R11G11B10F);

            Destroy(composeRT);
            composeRT = Graphics.CreateRenderTarget(w, h, PixelFormat.Rgba8);
            composeRT[0].FilterMode = FilterMode.Linear;
        }
    }
}