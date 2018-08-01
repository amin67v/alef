using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;

    class GLTexture2D : Texture2D
    {
        public uint Id;
        PixelFormat format;
        FilterMode filter;
        WrapMode wrap;
        int width, height;

        public GLTexture2D(int width, int height, PixelFormat format, FilterMode filter, WrapMode wrap, IntPtr data)
        {
            Id = glGenTexture();
            this.width = width;
            this.height = height;
            this.format = format;

            var glEnums = GLDriver.GetGLFormatEnums(format);
            glBindTexture(TextureTarget.Texture2D, Id);
            glTexImage2D(TextureTarget.Texture2D, 0, glEnums.internalFormat,
                width, height, 0, glEnums.readFormat, glEnums.readType, data);

            this.filter = filter;
            GLDriver.SetTextureFilterMode(Id, TextureTarget.Texture2D, filter);
            this.wrap = wrap;
            GLDriver.SetTextureWrapMode(Id, TextureTarget.Texture2D, wrap);
        }

        public override int Width => width;

        public override int Height => height;

        public override PixelFormat PixelFormat => format;

        public override WrapMode WrapMode
        {
            get => wrap;
            set
            {
                if (wrap == value)
                    return;

                wrap = value;
                GLDriver.SetTextureWrapMode(Id, TextureTarget.Texture2D, value);
            }
        }

        public override FilterMode FilterMode
        {
            get => filter;
            set
            {
                if (filter == value)
                    return;

                filter = value;
                GLDriver.SetTextureFilterMode(Id, TextureTarget.Texture2D, value);
            }
        }

        public override void UpdatePixels(Image img, int mipLevel)
        {
            var glEnums = GLDriver.GetGLFormatEnums(format);
            glBindTexture(TextureTarget.Texture2D, Id);
            glTexImage2D(TextureTarget.Texture2D, mipLevel, glEnums.internalFormat, width, height, 0, glEnums.readFormat, glEnums.readType, img.PixelData);
        }

        public override void GenerateMipmaps()
        {
            glBindTexture(TextureTarget.Texture2D, Id);
            glGenerateMipmap(TextureTarget.Texture2D);
        }

        protected override void OnDestroy()
        {
            glDeleteTexture(Id);
            Id = 0;
        }
    }
}
