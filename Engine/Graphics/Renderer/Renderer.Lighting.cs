using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        public class LightCompose : DisposeBase
        {
            RenderTarget composeRT;
            RenderTarget lightRT;
            CubeMap reflection;
            MeshBuffer pointLightMesh;
            MeshBuffer[] skyboxMeshes;
            Vector3 sunDir = Vector3.Normalize(new Vector3(-0.85f, -0.28f, -0.45f));
            Color skyColor = new Color(27, 234, 255, 255);
            Color groundColor = new Color(80, 80, 70, 255);
            Color sunColor = new Color(255, 213, 170, 255);
            Color fogColor = new Color(80, 72, 75, 255);
            float fogIntensity = 1f;
            float fogDensity = 0.01f;
            float sunIntensity = 15f;
            float skyIntensity = 1.5f;
            float groundIntensity = 0.4f;
            float exposure = 1f;
            bool sunLight = true;

            public RenderTarget LightBuffer => lightRT;

            public RenderTarget RenderTarget => composeRT;

            public CubeMap ReflectionSource
            {
                get => reflection;
                set => reflection = value;
            }

            public Color FogColor
            {
                get => fogColor;
                set => fogColor = value;
            }

            public Color SunColor
            {
                get => sunColor;
                set => sunColor = value;
            }

            public Color SkyColor
            {
                get => skyColor;
                set => skyColor = value;
            }

            public Color GroundColor
            {
                get => groundColor;
                set => groundColor = value;
            }

            public float FogDensity
            {
                get => fogDensity;
                set => fogDensity = value;
            }

            public float FogIntensity
            {
                get => fogIntensity;
                set => fogIntensity = value;
            }

            public float SunIntensity
            {
                get => sunIntensity;
                set => sunIntensity = value;
            }

            public float SkyIntensity
            {
                get => skyIntensity;
                set => skyIntensity = value;
            }

            public float GroundIntensity
            {
                get => groundIntensity;
                set => groundIntensity = value;
            }

            public float Exposure
            {
                get => exposure;
                set => exposure = value;
            }

            public bool SunLight
            {
                get => sunLight;
                set
                {
                    if (sunLight == value)
                        return;

                    sunLight = value;
                }
            }

            public Vector3 SunDirection
            {
                get => sunDir;
                set => sunDir = Vector3.Normalize(value);
            }

            internal LightCompose(Renderer r)
            {
                r.OnReset += onReset;

                pointLightMesh = Model.Load("Engine/PointLight.obj")[0].MeshBuffer;
                // skyboxMeshes = new MeshBuffer[6];
                // skyboxMeshes[0] = Model.Load("Engine/SkyBox.obj")["Right"].MeshBuffer;
                // skyboxMeshes[1] = Model.Load("Engine/SkyBox.obj")["Left"].MeshBuffer;
                // skyboxMeshes[2] = Model.Load("Engine/SkyBox.obj")["Top"].MeshBuffer;
                // skyboxMeshes[3] = Model.Load("Engine/SkyBox.obj")["Bottom"].MeshBuffer;
                // skyboxMeshes[4] = Model.Load("Engine/SkyBox.obj")["Back"].MeshBuffer;
                // skyboxMeshes[5] = Model.Load("Engine/SkyBox.obj")["Front"].MeshBuffer;
            }

            internal void InternalRender(Scene scene)
            {
                var camera = scene.MainCamera;

                Graphics.BlitRenderTarget(Renderer.GBuffer.GBufferRT, lightRT, 0, BufferMask.Depth, FilterMode.Point);

                // render point lights
                Graphics.RenderTarget = lightRT;
                Graphics.Viewport = new Rect(0, 0, lightRT.Width, lightRT.Height);
                Graphics.SetClearValue(0, Color.Black);
                Graphics.Clear(BufferMask.All);

                Graphics.SetBlendMode(BlendMode.Additive);
                Graphics.SetDepthTest(DepthTest.Disable);
                Graphics.SetDepthWrite(false);

                // ambient light
                Graphics.SetShader(Renderer.getShader("AmbientLight"));
                Graphics.Draw(MeshBuffer.ScreenQuad);

                // sun light
                if (SunLight)
                {
                    Graphics.SetShader(Renderer.getShader("SunLight"));
                    Graphics.Draw(MeshBuffer.ScreenQuad);
                }
                Graphics.SetDepthTest(DepthTest.Greater);

                // point lights
                var pointLightArr = scene.GetNodesByGroup("Renderer.PointLight");
                if (pointLightArr != null)
                {
                    var shader = Renderer.getShader("PointLight");
                    for (int i = 0; i < pointLightArr.Count; i++)
                    {
                        var node = pointLightArr.GetItemAt(i) as LightNode;
                        Graphics.SetShader(shader, node);
                        Graphics.Draw(pointLightMesh);
                    }
                }

                // light combine pass
                Graphics.RenderTarget = composeRT;
                Graphics.Viewport = new Rect(0, 0, composeRT.Width, composeRT.Height);
                Graphics.SetClearValue(0, Color.Black);
                Graphics.Clear(BufferMask.All);
                Graphics.SetBlendMode(BlendMode.Disable);
                Graphics.SetDepthTest(DepthTest.Disable);
                Graphics.SetDepthWrite(false);
                Graphics.SetShader(Renderer.getShader("LightCompose"));
                Graphics.Draw(MeshBuffer.ScreenQuad);
            }

            protected override void OnDispose(bool manual)
            {
                Destroy(lightRT);
                Destroy(composeRT);
            }

            void onReset(int w, int h)
            {
                Destroy(lightRT);
                lightRT = Graphics.CreateRenderTarget(w, h, PixelFormat.R11G11B10F);

                Destroy(composeRT);
                composeRT = Graphics.CreateRenderTarget(w, h, PixelFormat.Rgba8);
                composeRT[0].FilterMode = FilterMode.Linear;
            }
        }
    }
}
