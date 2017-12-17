using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using static OpenGL;

namespace Engine
{
    class OpenglTexture : Texture
    {
        internal uint id = 0;
        FilterMode filter = FilterMode.Point;
        int width = 0;
        int height = 0;
        bool mipmap = false;
        bool repeat = false;

        public OpenglTexture(int width, int height, FilterMode filter, bool repeat, IntPtr data)
        {
            id = glGenTexture();

            glBindTexture(TextureTarget.Texture2D, id);
            glTexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            this.width = width;
            this.height = height;
            this.filter = filter;
            this.set_filter(filter);
            this.repeat = repeat;
            this.set_repeat(repeat);
        }

        public override int Width => width;

        public override int Height => height;

        public override bool HasMipmaps => mipmap;

        public override bool Repeat
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

        public override FilterMode Filter
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
