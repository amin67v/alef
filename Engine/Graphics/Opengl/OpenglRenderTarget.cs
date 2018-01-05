using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;
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
                textures[i] = Texture.Create(width, height, filter, WrapMode.Clamp, IntPtr.Zero);
                var tex_id = (textures[i] as OpenglTexture).id;
                glFramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, tex_id, 0);
            }
            glBindFramebuffer(FramebufferTarget.Framebuffer, current == null ? 0 : current.id);
        }

        public override Texture this[int i] => textures[i];

        protected override void OnDisposeManaged()
        {
            for (int i = 0; i < textures.Length; i++)
                textures[i].Dispose();

            textures = null;

        }

        protected override void OnDisposeUnmanaged()
        {
            glDeleteFramebuffer(id);
            id = 0;
        }
    }
}