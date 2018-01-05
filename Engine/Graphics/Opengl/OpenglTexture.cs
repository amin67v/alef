using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;
    class OpenglTexture : Texture
    {
        internal uint id = 0;
        FilterMode filter = FilterMode.Point;
        WrapMode wrap = WrapMode.Clamp;
        int width;
        int height;
        bool mipmap;

        public OpenglTexture(int width, int height, FilterMode filter, WrapMode wrap, IntPtr data)
        {
            id = glGenTexture();

            glBindTexture(TextureTarget.Texture2D, id);
            glTexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            this.width = width;
            this.height = height;
            this.filter = filter;
            this.set_filter(filter);
            this.wrap = WrapMode.Clamp;
            this.set_wrap(wrap);
        }

        public override int Width => width;

        public override int Height => height;

        public override bool HasMipmaps => mipmap;

        public override WrapMode Wrap
        {
            get => wrap;
            set
            {
                if (wrap == value)
                    return;

                wrap = value;
                set_wrap(value);
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

        void set_wrap(WrapMode value)
        {
            glBindTexture(TextureTarget.Texture2D, id);
            if (value == WrapMode.Repeat)
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

        protected override void OnDisposeUnmanaged()
        {
            glDeleteTexture(id);
            id = 0;
        }
    }
}
