using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        public event Action<int, int> OnReset;

        RenderTarget[,] tempRT;
        GBufferFill gbuffer;
        LightCompose lighting;
        AmbientOcclusion ssao;
        ShadowPass shadow;
        RenderTarget output;
        Fxaa fxaa;
        DebugMode debug;
        bool reset = true;

        public RenderTarget Output
        {
            get => output;
            set
            {
                if (output == value)
                    return;

                reset = true;
                output = value;
            }
        }

        public DebugMode Debug
        {
            get => debug;
            set => debug = value;
        }

        public int RenderWidth => Output?.Width ?? Window.Width;

        public int RenderHeight => Output?.Height ?? Window.Height;

        public AmbientOcclusion SSAO => ssao;

        public LightCompose Lighting => lighting;

        public GBufferFill GBuffer => gbuffer;

        public ShadowPass Shadow => shadow;

        public Fxaa FXAA => fxaa;

        public Renderer()
        {
            buildShaders();

            Window.OnResize += (w, h) =>
            {
                if (Output == null)
                    reset = true;
            };

            gbuffer = new GBufferFill(this);
            ssao = new AmbientOcclusion(this);
            lighting = new LightCompose(this);
            fxaa = new Fxaa(this);
            shadow = new ShadowPass(this);

            OnReset += (w, h) =>
            {
                if (tempRT == null)
                    tempRT = new RenderTarget[3, 2];

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 2; j++)
                    {
                        Destroy(tempRT[i, j]);
                        tempRT[i, j] = null;
                    }
            };
        }

        public void Render(Scene scene)
        {
            if (scene.MainCamera != null && !scene.MainCamera.IsDestroyed)
            {
                if (reset)
                {
                    OnReset?.Invoke(RenderWidth, RenderHeight);
                    reset = false;
                }

                var camera = scene.MainCamera;
                Graphics.Viewport = new Rect(0, 0, RenderWidth, RenderHeight);
                Graphics.Scissor = Rect.Zero;
                setShaderConstants(camera);

                GBuffer.InternalRender(scene);
                SSAO.InternalRender();
                Shadow.InternalRender(scene);
                Lighting.InternalRender(scene);
                FXAA.InternalRender();

                if (debug != DebugMode.Disabled)
                {
                    Graphics.RenderTarget = null;
                    Graphics.Clear(BufferMask.All);
                    switch (debug)
                    {
                        case DebugMode.Albedo:
                            Graphics.SetShader(getShader("Debug"), GBuffer.AlbedoTex);
                            break;
                        case DebugMode.Surface:
                            Graphics.SetShader(getShader("Debug"), GBuffer.SurfaceTex);
                            break;
                        case DebugMode.Normal:
                            Graphics.SetShader(getShader("Debug"), GBuffer.NormalTex);
                            break;
                        case DebugMode.Depth:
                            Graphics.SetShader(getShader("Debug"), GBuffer.DepthTex);
                            break;
                        case DebugMode.ShadowMap:
                            Graphics.SetShader(getShader("Debug"), Shadow.ShadowMap[0]);
                            break;
                        case DebugMode.ShadowDepth:
                            Graphics.SetShader(getShader("Debug"), Shadow.ShadowDepth[0]);
                            break;
                        case DebugMode.SSAO:
                            Graphics.SetShader(getShader("Debug"), SSAO.Texture);
                            break;
                        case DebugMode.LightBuffer:
                            Graphics.SetShader(getShader("Debug"), Lighting.RenderTarget[0]);
                            break;
                        case DebugMode.UpsampleOffset:
                            Graphics.SetShader(getShader("Debug"), gbuffer.DepthOffsetTex);
                            break;
                    }
                    Graphics.Draw(MeshBuffer.ScreenQuad);
                }
            }
        }

        protected override void OnDestroy()
        {
            GBuffer?.Dispose();
            Destroy(SSAO);
            Lighting?.Dispose();
            FXAA?.Dispose();

            // todo : dispose tmp RT
        }

        RenderTarget getTempRT(int downsample, int index)
        {
            if (tempRT[downsample, index] == null || tempRT[downsample, index].IsDestroyed)
            {
                int w = RenderWidth;
                int h = RenderHeight;
                if (downsample == 1) { w /= 2; h /= 2; }
                if (downsample == 2) { w /= 4; h /= 4; }
                tempRT[downsample, index] = Graphics.CreateRenderTarget(w, h, false, PixelFormat.Rgba8);
                tempRT[downsample, index][0].FilterMode = FilterMode.Linear;
            }
            return tempRT[downsample, index];
        }

        void drawPass(RenderTarget target, Shader shader, object obj = null)
        {
            Graphics.RenderTarget = target;
            Graphics.Viewport = new Rect(0, 0, target.Width, target.Height);
            Graphics.Clear(BufferMask.All);

            Graphics.SetDepthTest(DepthTest.Disable);
            Graphics.SetBlendMode(BlendMode.Disable);
            Graphics.SetFaceCull(FaceCull.None);
            Graphics.SetDepthWrite(false);

            Graphics.SetShader(shader, obj);
            Graphics.Draw(MeshBuffer.ScreenQuad);
        }

        public enum DebugMode
        {
            Disabled,
            Albedo,
            Surface,
            Normal,
            Depth,
            ShadowMap,
            ShadowDepth,
            SSAO,
            LightBuffer,
            UpsampleOffset
        }
    }


}