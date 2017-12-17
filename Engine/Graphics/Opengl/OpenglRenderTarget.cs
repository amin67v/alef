using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using static OpenGL;

namespace Engine
{
    class OpenglRenderTarget : RenderTarget
    {
        internal static OpenglRenderTarget current = null;

        internal uint id;
        Texture[] textures = null;

        public OpenglRenderTarget(int width, int height, FilterMode filter, int count)
        {
            Assert.IsTrue(count > 0);
            id = glGenFramebuffer();
            textures = new Texture[count];

            glBindFramebuffer(FramebufferTarget.Framebuffer, id);
            for (int i = 0; i < count; i++)
            {
                textures[i] = Texture.Create(width, height, filter, false, IntPtr.Zero);
                var tex_id = (textures[i] as OpenglTexture).id;
                glFramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, tex_id, 0);
            }
            glBindFramebuffer(FramebufferTarget.Framebuffer, current.id);
        }

        protected override void OnDisposeUnmanaged()
        {
            glDeleteFramebuffer(id);
            id = 0;
        }
    }
}