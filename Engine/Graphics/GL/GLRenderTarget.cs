using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;
    class GLRenderTarget : RenderTarget
    {
        public uint Id;
        uint depthBuffer;
        int width, height;
        Array<GLTexture2D> textures;

        public override int Width => width;

        public override int Height => height;

        public GLRenderTarget(int width, int height, bool depthStencil, params PixelFormat[] colorBuffers)
        {
            Id = glGenFramebuffer();
            this.width = width;
            this.height = height;
            textures = new Array<GLTexture2D>();

            var boundRT = (Graphics as GLDriver).RenderTarget as GLRenderTarget;
            glBindFramebuffer(FramebufferTarget.Framebuffer, Id);

            if (depthStencil)
            {
                depthBuffer = glGenRenderbuffer();
                glBindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
                glRenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorageEnum.Depth24Stencil8, width, height);
                glBindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
                glFramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
            }

            DrawBuffersEnum[] attachments = new DrawBuffersEnum[colorBuffers.Length];
            for (int i = 0; i < colorBuffers.Length; i++)
            {
                attachments[i] = DrawBuffersEnum.ColorAttachment0 + i;
                var glTex = new GLTexture2D(Width, Height, colorBuffers[i], FilterMode.Point, WrapMode.Clamp, IntPtr.Zero);
                glFramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachments[i], TextureTarget.Texture2D, glTex.Id, 0);
                textures.Push(glTex);
            }
            glDrawBuffers(attachments.Length, attachments);

            if (glCheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Log.Error("Failed to setup frameBuffer correctly.");

            glBindFramebuffer(FramebufferTarget.Framebuffer, boundRT?.Id ?? 0);
        }

        public override Texture2D this[int index] => ((uint)index < textures.Count) ? textures[index] : null;

        protected override void OnDestroy()
        {
            for (int i = 0; i < textures.Count; i++)
                Destroy(textures[i]);

            textures = null;

            if (depthBuffer != 0)
                glDeleteRenderbuffer(depthBuffer);
            glDeleteFramebuffer(Id);
            Id = 0;
        }
    }
}