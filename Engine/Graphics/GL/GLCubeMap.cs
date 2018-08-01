using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;

    class GLCubeMap : CubeMap
    {
        public uint Id;
        PixelFormat format;
        FilterMode filter;
        WrapMode wrap;
        int faceSize;

        public GLCubeMap(int faceSize, PixelFormat format, FilterMode filter, WrapMode wrap, IntPtr[] data)
        {
            Id = glGenTexture();
            this.faceSize = faceSize;
            this.format = format;
            var glEnums = GLDriver.GetGLFormatEnums(format);

            glBindTexture(TextureTarget.TextureCubeMap, Id);
            for (int i = 0; i < 6; i++)
            {
                glTexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, glEnums.internalFormat,
                    faceSize, faceSize, 0, glEnums.readFormat, glEnums.readType, data[i]);
            }

            FilterMode = filter;
            WrapMode = wrap;
            this.filter = filter;
            GLDriver.SetTextureFilterMode(Id, TextureTarget.TextureCubeMap, filter);
            this.wrap = wrap;
            GLDriver.SetTextureWrapMode(Id, TextureTarget.TextureCubeMap, wrap);
        }

        public override int FaceSize => faceSize;

        public override PixelFormat PixelFormat => format;

        public override WrapMode WrapMode
        {
            get => wrap;
            set
            {
                if (wrap == value)
                    return;

                wrap = value;
                GLDriver.SetTextureWrapMode(Id, TextureTarget.TextureCubeMap, value);
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
                GLDriver.SetTextureFilterMode(Id, TextureTarget.TextureCubeMap, value);
            }
        }

        public override void UpdatePixels(Image image, Face face, int mipLevel)
        {
            var glEnums = GLDriver.GetGLFormatEnums(format);
            glBindTexture(TextureTarget.TextureCubeMap, Id);
            glTexImage2D(TextureTarget.TextureCubeMapPositiveX + (int)face, mipLevel, glEnums.internalFormat,
                faceSize, faceSize, 0, glEnums.readFormat, glEnums.readType, image.PixelData);
        }

        protected override void OnDestroy()
        {
            glDeleteTexture(Id);
            Id = 0;
        }
    }
}