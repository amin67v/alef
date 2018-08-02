using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public partial class Renderer : ObjectBase
    {
        public event Action<int, int> OnReset;

        RenderTarget[,] tempRT;
        RenderTarget output;
        DebugMode debug;
        bool reset = true;

        public GBuffer GBuffer { get; private set; }

        public Lighting Lighting { get; private set; }

        public SSAO SSAO { get; private set; }

        public Shadow Shadow { get; private set; }

        public FXAA FXAA { get; private set; }

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

                GBuffer.Draw(scene);
                SSAO.Draw(scene);
                Shadow.Draw(scene);
                Lighting.Draw(scene);
                FXAA.Draw(scene);

                if (debug != DebugMode.Disabled)
                {
                    Graphics.RenderTarget = null;
                    Graphics.Clear(BufferMask.All);
                    switch (debug)
                    {
                        case DebugMode.Albedo:
                            Graphics.SetShader(GetShader("Debug"), GBuffer.AlbedoTex);
                            break;
                        case DebugMode.Surface:
                            Graphics.SetShader(GetShader("Debug"), GBuffer.SurfaceTex);
                            break;
                        case DebugMode.Normal:
                            Graphics.SetShader(GetShader("Debug"), GBuffer.NormalTex);
                            break;
                        case DebugMode.Depth:
                            Graphics.SetShader(GetShader("Debug"), GBuffer.DepthTex);
                            break;
                        case DebugMode.ShadowMap:
                            Graphics.SetShader(GetShader("Debug"), Shadow.ShadowMap[0]);
                            break;
                        case DebugMode.ShadowDepth:
                            Graphics.SetShader(GetShader("Debug"), Shadow.ShadowDepth[0]);
                            break;
                        case DebugMode.SSAO:
                            Graphics.SetShader(GetShader("Debug"), SSAO.Texture);
                            break;
                        case DebugMode.LightBuffer:
                            Graphics.SetShader(GetShader("Debug"), Lighting.LightTexture);
                            break;
                            //case DebugMode.UpsampleOffset:
                            //    Graphics.SetShader(GetShader("Debug"), gbuffer.DepthOffsetTex);
                            //    break;
                    }
                    Graphics.Draw(MeshBuffer.ScreenQuad);
                }
            }
        }

        internal void Init()
        {
            {
                var debug = BuildShader("Debug", "post-fx.vert", null, "debug.frag");
                var texId = debug.GetUniformID("InputTex");
                var singleId = debug.GetUniformID("SingleChannel");
                debug.OnSetUniforms = (object obj) =>
                {
                    var tex = obj as Texture2D;
                    debug.SetUniform(texId, 0, tex);
                    var singleChannel = tex.PixelFormat == PixelFormat.R8 ||
                                        tex.PixelFormat == PixelFormat.R16 ||
                                        tex.PixelFormat == PixelFormat.R32F;
                    debug.SetUniform(singleId, singleChannel ? 1 : 0);
                };
            }

            Window.OnResize += (w, h) =>
            {
                if (Output == null)
                    reset = true;
            };

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

            GBuffer = new GBuffer();
            Lighting = new Lighting();
            SSAO = new SSAO();
            Shadow = new Shadow();
            FXAA = new FXAA();
        }

        protected override void OnDestroy()
        {
            Destroy(GBuffer);
            Destroy(SSAO);
            Destroy(Lighting);
            Destroy(Shadow);
            Destroy(FXAA);

            // todo : dispose tmp RT
        }

        public RenderTarget getTempRT(int downsample, int index)
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
            //UpsampleOffset
        }
    }


}