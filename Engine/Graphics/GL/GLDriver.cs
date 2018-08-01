using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;

    class GLDriver : GraphicsDriver
    {
        Matrix4x4 mat4Identity = Matrix4x4.Identity;
        Vector4[] clearValue = new Vector4[16];
        GLRenderTarget boundRT;
        GLShader boundShader;
        BlendMode blendMode;
        FaceCull faceCull;
        FillMode fillMode;
        DepthTest depthTest;
        Rect viewport, scissor;
        int[] ubos;
        bool depthWrite;

        public GLDriver()
        {
            LoadFunctions();

            blendMode = BlendMode.Disable;
            glDisable(EnableCap.Blend);

            faceCull = FaceCull.None;
            glDisable(EnableCap.CullFace);
            fillMode = FillMode.Fill;
            glPolygonMode(MaterialFace.FrontAndBack, PolygonModeEnum.Fill);

            depthTest = DepthTest.Disable;
            glDisable(EnableCap.DepthTest);

            depthWrite = false;
            glDepthMask(depthWrite);

            glHint(HintTarget.GenerateMipmapHint, HintMode.Nicest);

            ubos = new int[16];
            for (int i = 0; i < ubos.Length; i++)
                ubos[i] = -1;
        }

        public static void SetTextureWrapMode(uint id, TextureTarget target, WrapMode value)
        {
            glBindTexture(target, id);
            if (value == WrapMode.Repeat)
            {
                glTexParameteri(target, TextureParameterName.TextureWrapS, (int)TextureParameter.Repeat);
                glTexParameteri(target, TextureParameterName.TextureWrapT, (int)TextureParameter.Repeat);
                glTexParameteri(target, TextureParameterName.TextureWrapR, (int)TextureParameter.Repeat);
            }
            else
            {
                glTexParameteri(target, TextureParameterName.TextureWrapS, (int)TextureParameter.ClampToEdge);
                glTexParameteri(target, TextureParameterName.TextureWrapT, (int)TextureParameter.ClampToEdge);
                glTexParameteri(target, TextureParameterName.TextureWrapR, (int)TextureParameter.ClampToEdge);
            }
        }

        public static void SetTextureFilterMode(uint id, TextureTarget target, FilterMode value)
        {
            glBindTexture(target, id);

            if (value == FilterMode.Point)
            {
                glTexParameteri(target, TextureParameterName.TextureMagFilter, (int)TextureParameter.Nearest);
                glTexParameteri(target, TextureParameterName.TextureMinFilter, (int)TextureParameter.Nearest);
                glTexParameteri(target, TextureParameterName.TextureMaxAnisotropyExt, 0);
            }
            else if (value == FilterMode.Linear)
            {
                glTexParameteri(target, TextureParameterName.TextureMagFilter, (int)TextureParameter.Linear);
                glTexParameteri(target, TextureParameterName.TextureMinFilter, (int)TextureParameter.Nearest);
                glTexParameteri(target, TextureParameterName.TextureMaxAnisotropyExt, 0);
            }
            else if (value == FilterMode.Bilinear)
            {
                glTexParameteri(target, TextureParameterName.TextureMagFilter, (int)TextureParameter.Linear);
                glTexParameteri(target, TextureParameterName.TextureMinFilter, (int)TextureParameter.LinearMipMapNearest);
                glTexParameteri(target, TextureParameterName.TextureMaxAnisotropyExt, 0);
            }
            else
            {
                glTexParameteri(target, TextureParameterName.TextureMagFilter, (int)TextureParameter.Linear);
                glTexParameteri(target, TextureParameterName.TextureMinFilter, (int)TextureParameter.LinearMipMapLinear);
                switch (value)
                {
                    case FilterMode.Aniso4x:
                        glTexParameteri(target, TextureParameterName.TextureMaxAnisotropyExt, 4);
                        break;
                    case FilterMode.Aniso8x:
                        glTexParameteri(target, TextureParameterName.TextureMaxAnisotropyExt, 8);
                        break;
                    case FilterMode.Aniso16x:
                        glTexParameteri(target, TextureParameterName.TextureMaxAnisotropyExt, 16);
                        break;
                }
            }
        }

        public static PixelFormat GetImageFormat(int compress, bool sRgb)
        {
            if (compress == 0)
                return sRgb ? PixelFormat.sRgba8 : PixelFormat.Rgba8;
            else if (compress == 1)
                return sRgb ? PixelFormat.sRgbDxt1 : PixelFormat.Dxt1;
            else
                return sRgb ? PixelFormat.sRgbDxt5 : PixelFormat.Dxt5;
        }

        public static (PixelInternalFormat internalFormat, PixelType readType, OpenGL.PixelFormat readFormat) GetGLFormatEnums(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R8:
                    return (PixelInternalFormat.R8, PixelType.UnsignedByte, OpenGL.PixelFormat.Red);
                case PixelFormat.R16:
                    return (PixelInternalFormat.R16, PixelType.UnsignedShort, OpenGL.PixelFormat.Red);
                case PixelFormat.R16F:
                    return (PixelInternalFormat.R16f, PixelType.Float, OpenGL.PixelFormat.Red);
                case PixelFormat.R32F:
                    return (PixelInternalFormat.R32f, PixelType.Float, OpenGL.PixelFormat.Red);
                case PixelFormat.Rg8:
                    return (PixelInternalFormat.Rg8, PixelType.UnsignedByte, OpenGL.PixelFormat.Rg);
                case PixelFormat.Rg16:
                    return (PixelInternalFormat.Rg16, PixelType.UnsignedShort, OpenGL.PixelFormat.Rg);
                case PixelFormat.Rg16F:
                    return (PixelInternalFormat.Rg16f, PixelType.Float, OpenGL.PixelFormat.Rg);
                case PixelFormat.Rg32F:
                    return (PixelInternalFormat.Rg32f, PixelType.Float, OpenGL.PixelFormat.Rg);
                case PixelFormat.Rgb8:
                    return (PixelInternalFormat.Rgb8, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgb);
                case PixelFormat.sRgb8:
                    return (PixelInternalFormat.Srgb8, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgb);
                case PixelFormat.Rgb16:
                    return (PixelInternalFormat.Rgb16, PixelType.UnsignedShort, OpenGL.PixelFormat.Rgb);
                case PixelFormat.Rgb16F:
                    return (PixelInternalFormat.Rgb16f, PixelType.Float, OpenGL.PixelFormat.Rgb);
                case PixelFormat.Rgb32F:
                    return (PixelInternalFormat.Rgb32f, PixelType.Float, OpenGL.PixelFormat.Rgb);
                case PixelFormat.Rgba8:
                    return (PixelInternalFormat.Rgba8, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgba);
                case PixelFormat.sRgba8:
                    return (PixelInternalFormat.Srgb8Alpha8, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgba);
                case PixelFormat.Rgba16:
                    return (PixelInternalFormat.Rgba16, PixelType.UnsignedShort, OpenGL.PixelFormat.Rgba);
                case PixelFormat.Rgba16F:
                    return (PixelInternalFormat.Rgba16f, PixelType.Float, OpenGL.PixelFormat.Rgba);
                case PixelFormat.Rgba32F:
                    return (PixelInternalFormat.Rgba32f, PixelType.Float, OpenGL.PixelFormat.Rgba);
                case PixelFormat.Dxt1:
                    return (PixelInternalFormat.CompressedRgbaS3tcDxt1Ext, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgba);
                case PixelFormat.Dxt5:
                    return (PixelInternalFormat.CompressedRgbaS3tcDxt5Ext, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgba);
                case PixelFormat.sRgbDxt1:
                    return (PixelInternalFormat.CompressedSrgbAlphaS3tcDxt1Ext, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgba);
                case PixelFormat.sRgbDxt5:
                    return (PixelInternalFormat.CompressedSrgbAlphaS3tcDxt5Ext, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgba);
                case PixelFormat.Depth16F:
                    return (PixelInternalFormat.DepthComponent16, PixelType.Float, OpenGL.PixelFormat.DepthComponent);
                case PixelFormat.Depth32F:
                    return (PixelInternalFormat.DepthComponent32f, PixelType.Float, OpenGL.PixelFormat.DepthComponent);
                case PixelFormat.Depth24Stencil8:
                    return (PixelInternalFormat.Depth24Stencil8, PixelType.UnsignedInt248, OpenGL.PixelFormat.DepthStencil);
                case PixelFormat.R11G11B10F:
                    return (PixelInternalFormat.R11fG11fB10f, PixelType.Float, OpenGL.PixelFormat.Rgb);

                default:
                    return (PixelInternalFormat.Rgba8, PixelType.UnsignedByte, OpenGL.PixelFormat.Rgba);
            }
        }

        public override string DriverInfo
        {
            get
            {
                var vendor = glGetString(StringName.Vendor);
                var renderer = glGetString(StringName.Renderer);
                var version = glGetString(StringName.Version);
                return $"{vendor} - {renderer} : {version} ";
            }
        }

        public override Rect Viewport
        {
            get => viewport;
            set
            {
                viewport = value;
                glViewport((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
            }
        }

        public override Rect Scissor
        {
            get => scissor;
            set
            {
                scissor = value;
                if (scissor.Width + scissor.Height == 0)
                {
                    glDisable(EnableCap.ScissorTest);
                }
                else
                {
                    glEnable(EnableCap.ScissorTest);
                    glScissor((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
                }
            }
        }

        public override RenderTarget RenderTarget
        {
            get => boundRT;
            set
            {
                if (boundRT == value)
                    return;

                boundRT = value as GLRenderTarget;
                glBindFramebuffer(FramebufferTarget.Framebuffer, (value as GLRenderTarget)?.Id ?? 0);
            }
        }

        public override unsafe void Clear(BufferMask mask, int numColors)
        {
            if ((mask & BufferMask.Color) != 0)
            {
                for (int i = 0; i < numColors; i++)
                {
                    var vec = GetClearValue(i);
                    glClearBufferfv(ClearBuffer.Color, i, (float*)&vec);

                }
            }

            ClearBufferMask glMask = 0x00;
            if ((mask & BufferMask.Depth) != 0)
            {
                SetDepthWrite(true);
                glMask |= ClearBufferMask.DepthBufferBit;
            }

            if ((mask & BufferMask.Stencil) != 0)
            {
                glStencilMask(0xFF);
                glMask |= ClearBufferMask.StencilBufferBit;
            }

            if (glMask != 0x00)
            {
                glClear(glMask);
            }
        }

        public override void SetClearValue(int index, Vector4 value) => clearValue[index] = value;

        public override Vector4 GetClearValue(int index) => clearValue[index];

        public override MeshBuffer CreateMeshBuffer(VertexFormat format, PrimitiveType primitive, IntPtr vertexData, int vertexCount, IntPtr indexData, int indexCount)
        {
            return new GLMeshBuffer(format, primitive, vertexData, vertexCount, indexData, indexCount);
        }

        public override Texture2D CreateTexture2D(Image image, FilterMode filter, WrapMode wrap, int compress, bool sRgb)
        {
            return new GLTexture2D(image.Width, image.Height, GetImageFormat(compress, sRgb), filter, wrap, image.PixelData);
        }

        public override Texture2D CreateTexture2D(int width, int height, PixelFormat format, FilterMode filter, WrapMode wrap, IntPtr data)
        {
            return new GLTexture2D(width, height, format, filter, wrap, data);
        }

        public override CubeMap CreateCubeMap(Image[] images, FilterMode filter, WrapMode wrap, int compress, bool sRgb)
        {
            IntPtr[] data =
            {
                images[0].PixelData,
                images[1].PixelData,
                images[2].PixelData,
                images[3].PixelData,
                images[4].PixelData,
                images[5].PixelData
            };
            return new GLCubeMap(images[0].Width, GetImageFormat(compress, sRgb), filter, wrap, data);
        }

        public override CubeMap CreateCubeMap(int faceSize, PixelFormat format, FilterMode filter, WrapMode wrap, IntPtr[] data)
        {
            return new GLCubeMap(faceSize, format, filter, wrap, data);
        }

        public override RenderTarget CreateRenderTarget(int width, int height, bool depthStencil, params PixelFormat[] colorBuffers)
        {
            return new GLRenderTarget(width, height, depthStencil, colorBuffers);
        }

        public override Shader CreateShader(string vert, string geom, string frag)
        {
            return new GLShader(vert, geom, frag);
        }

        public override void SetStencil(StencilFunc func, byte value, bool write)
        {
            glStencilMask(write ? 0xFFu : 0x00u);
            glStencilFunc(StencilFunction.Never + (int)func, value, 0xFF);
            glStencilOp(StencilOpEnum.Keep, StencilOpEnum.Replace, StencilOpEnum.Replace);
        }

        public override void SetStencil(bool enabled)
        {
            if (enabled)
                glEnable(EnableCap.StencilTest);
            else
                glDisable(EnableCap.StencilTest);
        }

        public override void BlitRenderTarget(RenderTarget from, RenderTarget to, int readIndex, BufferMask mask, FilterMode filter, Rect src, Rect dest)
        {
            glBindFramebuffer(FramebufferTarget.ReadFramebuffer, (from as GLRenderTarget).Id);
            glBindFramebuffer(FramebufferTarget.DrawFramebuffer, (to as GLRenderTarget).Id);
            var glFilter = filter == FilterMode.Point ? BlitFramebufferFilter.Nearest : BlitFramebufferFilter.Linear;

            ClearBufferMask glMask = 0x00;
            if ((mask & BufferMask.Color) != 0)
                glMask |= ClearBufferMask.ColorBufferBit;

            if ((mask & BufferMask.Depth) != 0)
                glMask |= ClearBufferMask.DepthBufferBit;

            if ((mask & BufferMask.Stencil) != 0)
                glMask |= ClearBufferMask.StencilBufferBit;

            glReadBuffer(ReadBufferMode.ColorAttachment0 + readIndex);

            glBlitFramebuffer((int)src.XMin, (int)src.YMin, (int)src.XMax, (int)src.YMax,
                (int)dest.XMin, (int)dest.YMin, (int)dest.XMax, (int)dest.YMax, glMask, glFilter);

            boundRT = to as GLRenderTarget;
        }

        public unsafe override void SetUniformBlock(int index, IntPtr data, int size)
        {
            if (ubos[index] == -1)
            {
                var uboId = glGenBuffer();

                glBindBuffer(BufferTarget.UniformBuffer, uboId);
                glBufferData(BufferTarget.UniformBuffer, size, data, BufferUsageHint.DynamicDraw);

                glBindBufferRange(BufferTarget.UniformBuffer, (uint)index, uboId, IntPtr.Zero, new IntPtr(size));
                ubos[index] = (int)uboId;
            }
            else
            {
                glBindBuffer(BufferTarget.UniformBuffer, (uint)ubos[index]);
                glBufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, new IntPtr(size), data);
            }
        }

        public override void SetFillMode(FillMode value)
        {
            if (fillMode != value)
            {
                fillMode = value;
                switch (fillMode)
                {
                    case FillMode.Fill:
                        glPolygonMode(MaterialFace.FrontAndBack, PolygonModeEnum.Fill);
                        break;
                    case FillMode.Line:
                        glPolygonMode(MaterialFace.FrontAndBack, PolygonModeEnum.Line);
                        break;
                    case FillMode.Point:
                        glPolygonMode(MaterialFace.FrontAndBack, PolygonModeEnum.Point);
                        break;
                }
            }
        }

        public override void SetFaceCull(FaceCull value)
        {
            if (faceCull != value)
            {
                faceCull = value;
                switch (faceCull)
                {
                    case FaceCull.None:
                        glDisable(EnableCap.CullFace);
                        break;
                    case FaceCull.Front:
                        glEnable(EnableCap.CullFace);
                        glCullFace(CullFaceMode.Front);
                        break;
                    case FaceCull.Back:
                        glEnable(EnableCap.CullFace);
                        glCullFace(CullFaceMode.Back);
                        break;
                }
            }
        }

        public override void SetDepthWrite(bool value)
        {
            if (depthWrite != value)
            {
                depthWrite = value;
                glDepthMask(depthWrite);
            }
        }

        public override void SetDepthTest(DepthTest value)
        {
            if (depthTest != value)
            {
                depthTest = value;
                switch (depthTest)
                {
                    case DepthTest.Disable:
                        glDisable(EnableCap.DepthTest);
                        break;
                    case DepthTest.Always:
                        glEnable(EnableCap.DepthTest);
                        glDepthFunc(DepthFunction.Always);
                        break;
                    case DepthTest.Greater:
                        glEnable(EnableCap.DepthTest);
                        glDepthFunc(DepthFunction.Greater);
                        break;
                    case DepthTest.Less:
                        glEnable(EnableCap.DepthTest);
                        glDepthFunc(DepthFunction.Less);
                        break;
                    case DepthTest.Never:
                        glEnable(EnableCap.DepthTest);
                        glDepthFunc(DepthFunction.Never);
                        break;
                }
            }
        }

        public override void SetBlendMode(BlendMode value)
        {
            if (blendMode != value)
            {
                blendMode = value;
                switch (blendMode)
                {
                    case BlendMode.Disable:
                        glDisable(EnableCap.Blend);
                        break;
                    case BlendMode.AlphaBlend:
                        glEnable(EnableCap.Blend);
                        glBlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                        break;
                    case BlendMode.Additive:
                        glEnable(EnableCap.Blend);
                        glBlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
                        break;
                    case BlendMode.Modulative:
                        glEnable(EnableCap.Blend);
                        glBlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero);
                        break;
                }
            }
        }

        public override void SetShader(Shader shader, object obj)
        {
            if (boundShader != shader)
            {
                boundShader = shader as GLShader;
                glUseProgram(boundShader.Id);
            }
            shader.OnSetUniforms?.Invoke(obj);
        }

        public override void Draw(MeshBuffer mesh, int offset, int count)
        {
            var glMesh = mesh as GLMeshBuffer;

            glBindVertexArray(glMesh.Vao);
            glBindBuffer(BufferTarget.ElementArrayBuffer, glMesh.Ibo);
            glDrawElements((BeginMode)glMesh.PrimitiveType, count, DrawElementsType.UnsignedShort, new IntPtr(offset));
        }
    }
}