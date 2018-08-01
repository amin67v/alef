using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

using System.Json;

namespace Engine
{
    public abstract class CubeMap : Resource
    {
        /// <summary>
        /// The size of the cubemap faces
        /// </summary>
        public abstract int FaceSize { get; }

        /// <summary>
        /// Pixel format used to build this texture
        /// </summary>
        public abstract PixelFormat PixelFormat { get; }

        /// <summary>
        /// Determines how to sample texture when coordinate is out of range 0, 1
        /// </summary>
        public abstract WrapMode WrapMode { get; set; }

        /// <summary>
        /// Determines how to sample texture
        /// </summary>
        public abstract FilterMode FilterMode { get; set; }

        /// <summary>
        /// Updates pixels of the given face and mip level
        /// </summary>
        public abstract void UpdatePixels(Image img, Face face, int mipLevel);

        /// <summary>
        /// Generates mip level for this cubemap
        /// </summary>
        public void GenerateMipmaps(Image[] images)
        {

        }

        /// <summary>
        /// Loads and cache cubemap from file
        /// </summary>
        public static CubeMap Load(string file)
        {
            Resource load_file(Stream stream)
            {
                var json = JsonValue.Load(stream);


                var imageStream = ResourceManager.GetFileStream(json["Image"]["Back"]);
                var back = Image.FromFile(imageStream);
                imageStream.Dispose();

                imageStream = ResourceManager.GetFileStream(json["Image"]["Bottom"]);
                var bottom = Image.FromFile(imageStream);
                imageStream.Dispose();

                imageStream = ResourceManager.GetFileStream(json["Image"]["Front"]);
                var front = Image.FromFile(imageStream);
                imageStream.Dispose();

                imageStream = ResourceManager.GetFileStream(json["Image"]["Left"]);
                var left = Image.FromFile(imageStream);
                imageStream.Dispose();

                imageStream = ResourceManager.GetFileStream(json["Image"]["Right"]);
                var right = Image.FromFile(imageStream);
                imageStream.Dispose();

                imageStream = ResourceManager.GetFileStream(json["Image"]["Top"]);
                var top = Image.FromFile(imageStream);
                imageStream.Dispose();

                FilterMode filter = Enum.Parse<FilterMode>(json["FilterMode"]);
                WrapMode wrap = FilterMode.Parse<WrapMode>(json["WrapMode"]);
                int compress = json["Compress"];
                bool sRgb = json["sRgb"];
                bool genMips = json["GenerateMipmap"];

                var imgArr = new Image[] { right, left, top, bottom, back, front };
                var tex = Graphics.CreateCubeMap(imgArr, filter, wrap, compress, sRgb);

                if (genMips)
                    tex.GenerateMipmaps(imgArr);

                right.Dispose();
                left.Dispose();
                top.Dispose();
                bottom.Dispose();
                back.Dispose();
                front.Dispose();

                return tex;
            }

            return ResourceManager.Load(file, load_file) as CubeMap;
        }

        public enum Face { Right, Left, Top, Bottom, Back, Front }
    }
}
