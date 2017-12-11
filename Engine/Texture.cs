using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using static OpenGL;

namespace Engine
{
    public class Texture : Resource
    {
        internal uint id = 0;
        FilterMode filter = FilterMode.Point;
        int width = 0;
        int height = 0;
        bool mipmap = false;
        bool repeat = false;

        Texture() { }

        /// <summary>
        /// The width of the texture
        /// </summary>
        public int Width => width;

        /// <summary>
        /// The Height of the texture
        /// </summary>
        public int Height => height;

        /// <summary>
        /// Does the texture contains mipmaps
        /// </summary>
        public bool HasMipmaps => mipmap;

        /// <summary>
        /// The texture size in vector2(width, height)
        /// </summary>
        public Vector2 Size => new Vector2(Width, Height);

        /// <summary>
        /// Determines how to sample texture when coordinate is out of range 0, 1
        /// 'True' means texture is repeated
        /// 'False' means the value is clamped within 0, 1 range
        /// </summary>
        public bool Repeat
        {
            get => repeat;
            set
            {
                if (repeat == value)
                    return;

                repeat = value;
                set_repeat(value);
            }
        }

        /// <summary>
        /// Determines how to sample texture
        /// 'Point' means sample the closest pixel to the given coordinate
        /// 'Bilinear' means linearly sample from the nearest mipmap
        /// 'Trilinear' means linearly sample from two nearest mipmap
        /// </summary>
        public FilterMode Filter
        {
            get => filter;
            set
            {
                if (filter == value)
                    return;

                filter = value;
                set_filter(value);
            }
        }

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
            var id = glGenTexture();

            glBindTexture(TextureTarget.Texture2D, id);
            glTexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            var tex = new Texture();
            tex.id = id;
            tex.width = width;
            tex.height = height;
            tex.filter = filter;
            tex.set_filter(filter);
            tex.repeat = repeat;
            tex.set_repeat(repeat);

            return tex;
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
                BinaryReader reader = new BinaryReader(stream);
                try
                {
                    byte[] buffer = reader.ReadBytes((int)stream.Length);
                    int w = 0, h = 0, c = 0;
                    IntPtr data = Stb.LoadFromMemory(buffer, ref w, ref h, ref c, 4);
                    var tex = Texture.Create(w, h, filter, repeat, data);
                    Stb.FreeImage(data);
                    return tex;
                }
                finally
                {
                    reader.Dispose();
                }

            }

            return FromCacheOrFile(file, load_file) as Texture;
        }

        protected override void OnDisposeUnmanaged()
        {
            base.OnDisposeUnmanaged();
            glDeleteTexture(id);
            id = 0;
        }

        void set_filter(FilterMode value)
        {
            glBindTexture(TextureTarget.Texture2D, id);

            if (value != FilterMode.Point && !HasMipmaps)
            {
                glGenerateMipmap(TextureTarget.Texture2D);
                mipmap = true;
            }

            if (value == FilterMode.Point)
            {
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureParameter.Nearest);
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureParameter.Nearest);
            }
            else if (value == FilterMode.Bilinear)
            {
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureParameter.Linear);
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureParameter.LinearMipMapNearest);
            }
            else if (value == FilterMode.Trilinear)
            {
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureParameter.Linear);
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureParameter.LinearMipMapLinear);
            }
        }

        void set_repeat(bool value)
        {
            glBindTexture(TextureTarget.Texture2D, id);
            if (value)
            {
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureParameter.Repeat);
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureParameter.Repeat);
            }
            else
            {
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureParameter.ClampToEdge);
                glTexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureParameter.ClampToEdge);
            }
        }
    }
}
