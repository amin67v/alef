using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    public abstract class Texture : Resource
    {
        /// <summary>
        /// The width of the texture
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// The Height of the texture
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// The texture size in vector2(width, height)
        /// </summary>
        public Vector2 Size => new Vector2(Width, Height);

        /// <summary>
        /// Does the texture contains mipmaps
        /// </summary>
        public abstract bool HasMipmaps { get; }

        /// <summary>
        /// Determines how to sample texture when coordinate is out of range 0, 1
        /// 'True' means texture is repeated
        /// 'False' means the value is clamped within 0, 1 range
        /// </summary>
        public abstract bool Repeat { get; set; }

        /// <summary>
        /// Determines how to sample texture
        /// 'Point' means sample the closest pixel to the given coordinate
        /// 'Bilinear' means linearly sample from the nearest mipmap
        /// 'Trilinear' means linearly sample from two nearest mipmap
        /// </summary>
        public abstract FilterMode Filter { get; set; }

        /// <summary>
        /// Creates texture with the provided parameters
        /// </summary>
        public static Texture Create(int width, int height, FilterMode filter, bool repeat, Color[] pixels)
        {
            var pin = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            var tex = Create(width, height, filter, repeat, pin.AddrOfPinnedObject());
            pin.Free();
            return tex;
        }

        /// <summary>
        /// Creates texture with the provided parameters
        /// </summary>
        public static Texture Create(int width, int height, FilterMode filter, bool repeat, IntPtr data)
        {
            return App.Graphics.CreateTexture(width, height, filter, repeat, data);
        }

        /// <summary>
        /// Loads and cache texture from file
        /// </summary>
        public static Texture Load(string file) => Load(file, FilterMode.Point, false);

        /// <summary>
        /// Loads and cache texture from file
        /// </summary>
        public static Texture Load(string file, FilterMode filter, bool repeat)
        {
            Resource load_file(Stream stream)
            {
                var img = Image.FromFile(stream);
                var tex = Texture.Create(img.Width, img.Height, filter, repeat, img.PixelData);
                img.Dispose();
                return tex;
            }

            return App.ResourceManager.FromCacheOrFile(file, load_file) as Texture;
        }
    }
}
