using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static OpenGL;

namespace Engine
{
    public class RenderTarget : Disposable
    {
        static RenderTarget active = null;

        Texture[] textures = null;
        uint id = 0;

        RenderTarget() { }

        /// <summary>
        /// Gets the attached texture with the specified index
        /// </summary>
        public Texture this[int i] => textures[i];

        /// <summary>
        /// Gets or sets the active render target
        /// </summary>
        public static RenderTarget Active
        {
            get => active;
            set
            {
                if (active == value)
                    return;

                active = value;

                glBindFramebuffer(FramebufferTarget.Framebuffer, value == null ? 0 : value.id);
            }
        }

        /// <summary>
        /// Creates a render target
        /// </summary>
        public static RenderTarget Create(Vector2 size)
        {
            return Create((int)size.X, (int)size.Y, FilterMode.Point, 1);
        }

        /// <summary>
        /// Creates a render target
        /// </summary>
        public static RenderTarget Create(int width, int height)
        {
            return Create(width, height, FilterMode.Point, 1);
        }

        /// <summary>
        /// Creates a render target
        /// </summary>
        public static RenderTarget Create(int width, int height, FilterMode filter, int count)
        {
            Assert.IsTrue(count > 0);
            var id = glGenFramebuffer();
            var texs = new Texture[count];

            glBindFramebuffer(FramebufferTarget.Framebuffer, id);
            for (int i = 0; i < count; i++)
            {
                texs[i] = Texture.Create(width, height, filter, false, IntPtr.Zero);
                glFramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, texs[i].Id, 0);
            }
            glBindFramebuffer(FramebufferTarget.Framebuffer, 0);

            var rt = new RenderTarget();
            rt.id = id;
            rt.textures = texs;
            glBindFramebuffer(FramebufferTarget.Framebuffer, Active == null ? 0 : active.id);
            return rt;
        }

        protected override void OnDisposeUnmanaged()
        {
            for (int i = 0; i < textures.Length; i++)
                textures[i].Dispose();

            textures = null;

            glDeleteFramebuffer(id);
            id = 0;
        }
    }
}