using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

using System.Json;

namespace Engine
{
    public abstract class Texture2D : Resource
    {
        /// <summary>
        /// Gets internal 1x1 white texture2d
        /// </summary>
        public static Texture2D White
        {
            get
            {
                var value = ResourceManager.Get<Texture2D>("Texture2D.White");
                if (value == null)
                {
                    var img = new Image(1, 1, Color.White);
                    value = Graphics.CreateTexture2D(img, FilterMode.Point, WrapMode.Clamp, 0, true);
                    ResourceManager.Add("Texture2D.White", value);
                    img.Dispose();
                }
                return value;
            }
        }

        /// <summary>
        /// Gets internal 1x1 black texture2d
        /// </summary>
        public static Texture2D Black
        {
            get
            {
                var value = ResourceManager.Get<Texture2D>("Texture2D.Black");
                if (value == null)
                {
                    var img = new Image(1, 1, Color.Black);
                    value = Graphics.CreateTexture2D(img, FilterMode.Point, WrapMode.Clamp, 0, true);
                    ResourceManager.Add("Texture2D.Black", value);
                    img.Dispose();
                }
                return value;
            }
        }

        /// <summary>
        /// The width of the texture
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// The height of the texture
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// Determines how to sample texture when coordinate is out of range 0, 1
        /// </summary>
        public abstract WrapMode WrapMode { get; set; }

        /// <summary>
        /// Determines how to sample texture
        /// </summary>
        public abstract FilterMode FilterMode { get; set; }

        /// <summary>
        /// Pixel format used to build this texture
        /// </summary>
        public abstract PixelFormat PixelFormat { get; }

        /// <summary>
        /// Updates pixels of the given mip level
        /// </summary>
        public abstract void UpdatePixels(Image img, int mipLevel);

        /// <summary>
        /// Generates mip level for this texture
        /// </summary>
        public abstract void GenerateMipmaps();

        /// <summary>
        /// Loads and cache texture from file
        /// </summary>
        public static Texture2D Load(string file)
        {
            Resource load_file(Stream stream)
            {
                var json = JsonValue.Load(stream);
                var imageStream = ResourceManager.GetFileStream(json["Image"]);
                var img = Image.FromFile(imageStream);
                imageStream.Dispose();
                img.FlipVertical();

                FilterMode filter = Enum.Parse<FilterMode>(json["FilterMode"]);
                WrapMode wrap = FilterMode.Parse<WrapMode>(json["WrapMode"]);
                int compress = json["Compress"];
                bool sRgb = json["sRgb"];
                bool genMips = json["GenerateMipmap"];

                var tex = Graphics.CreateTexture2D(img, filter, wrap, compress, sRgb);
                if (genMips)
                    tex.GenerateMipmaps();

                img.Dispose();
                return tex;
            }

            return ResourceManager.Load(file, load_file) as Texture2D;
        }
    }


}
