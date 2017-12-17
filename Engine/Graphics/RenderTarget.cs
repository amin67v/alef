using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    public abstract class RenderTarget : Disposable
    {
        Texture[] textures = null;

        /// <summary>
        /// Gets the attached texture with the specified index
        /// </summary>
        public Texture this[int i] => textures[i];

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
            return App.Graphics.CreateRenderTarget(width, height, filter, count);
        }

        protected override void OnDisposeManaged()
        {
            for (int i = 0; i < textures.Length; i++)
                textures[i].Dispose();

            textures = null;
        }
    }
}