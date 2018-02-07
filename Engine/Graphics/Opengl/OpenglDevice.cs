using System;
using System.Numerics;

namespace Engine
{
    using static OpenGL;
    class OpenglDevice : IGraphicsDevice
    {
        string info;
        Matrix4x4 view;
        BlendMode blend;
        bool scissor = false;

        public OpenglDevice()
        {
            LoadFunctions();
            glDisable(EnableCap.CullFace);
            glDepthMask(false);
            glDisable(EnableCap.DepthTest);
            glDisable(EnableCap.Blend);
        }

        public string DriverInfo
        {
            get
            {
                if (info == null)
                {
                    var vendor = glGetString(StringName.Vendor);
                    var renderer = glGetString(StringName.Renderer);
                    var version = glGetString(StringName.Version);
                    info = $"{vendor} - {renderer} : {version} ";
                }
                return info;
            }
        }

        public Matrix4x4 ViewMatrix
        {
            get => view;
            set => view = value;
        }

        public void SetViewport(int x, int y, int w, int h) => glViewport(x, y, w, h);

        public void SetViewport(Rect rect) => glViewport((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        public void SetScissor(int x, int y, int w, int h)
        {
            if (scissor == false)
            {
                scissor = true;
                glEnable(EnableCap.ScissorTest);
            }
            glScissor(x, y, w, h);
        }

        public void SetScissor(Rect rect) => SetScissor((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        public void SetScissorOff()
        {
            scissor = false;
            glDisable(EnableCap.ScissorTest);
        }

        public void SetBlendMode(BlendMode value)
        {
            if (blend == value)
                return;

            blend = value;
            switch (value)
            {
                case BlendMode.Disabled:
                    glDisable(EnableCap.Blend);
                    break;
                case BlendMode.AlphaBlend:
                    glEnable(EnableCap.Blend);
                    glBlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    break;
                case BlendMode.Additive:
                    glEnable(EnableCap.Blend);
                    glBlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
                    break;
                case BlendMode.Modulative:
                    glEnable(EnableCap.Blend);
                    glBlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero);
                    break;
            }
        }

        public void Clear(Color color)
        {
            var c = color.ToVector4();
            glClearColor(c.X, c.Y, c.Z, c.W);
            glClear(ClearBufferMask.ColorBufferBit);
        }

        public void Display()
        {
            glFlush();
            SetScissorOff();
#if DEBUG
            var err = glGetError();
            if (err != ErrorCode.NoError)
            {
                var msg = $"Opengl error at frame '0', {err.ToString()}.";
                Log.Error(msg);
                throw new Exception(msg);
            }
#endif
        }

        public Texture CreateTexture(int width, int height, FilterMode filter, WrapMode wrap, IntPtr data)
        {
            return new OpenglTexture(width, height, filter, wrap, data);
        }

        public MeshBuffer<T> CreateMeshBuffer<T>(IntPtr vtx_data, int vtx_count) where T : struct, IVertex
        {
            return new OpenglMeshBuffer<T>(vtx_data, vtx_count);
        }

        public MeshBuffer<T> CreateMeshBuffer<T>(IntPtr vtx_data, int vtx_count, IntPtr idx_data, int idx_count) where T : struct, IVertex
        {
            return new OpenglMeshBuffer<T>(vtx_data, vtx_count, idx_data, idx_count);
        }

        public RenderTarget CreateRenderTarget(int width, int height, FilterMode filter, int count)
        {
            return new OpenglRenderTarget(width, height, filter, count);
        }

        public Shader CreateShader(string vert, string geom, string frag)
        {
            return new OpenglShader(vert, geom, frag);
        }

        public void SetRenderTarget(RenderTarget value)
        {
            if (OpenglRenderTarget.current == value)
                return;

            OpenglRenderTarget.current = value as OpenglRenderTarget;
            glBindFramebuffer(FramebufferTarget.Framebuffer, value == null ? 0 : OpenglRenderTarget.current.id);
        }

        public void SetShader(Shader value)
        {
            if (OpenglShader.current == value)
                return;

            OpenglShader.current = value as OpenglShader;
            glUseProgram(value == null ? 0 : OpenglShader.current.id);
        }

        public void SetPointSize(float value)
        {
            glPointSize(value);
        }

        public void SetLineWidth(float value)
        {
            glLineWidth(value);
        }

    }
}