using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    public abstract class RenderTarget : ObjectBase
    {
        /// <summary>
        /// The width of the renderTarget
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// The height of the renderTarget
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// Gets texture at the given index
        /// </summary>
        public abstract Texture2D this[int index] { get; }
    }
}