using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    public abstract class GraphicsDriver : ObjectBase
    {
        public abstract string DriverInfo { get; }

        public abstract Rect Viewport { get; set; }

        public abstract Rect Scissor { get; set; }

        public abstract RenderTarget RenderTarget { get; set; }

        public abstract unsafe void Clear(BufferMask mask, int numColors = 1);

        public abstract void SetClearValue(int index, Vector4 value);

        public void SetClearValue(int index, float value) => SetClearValue(index, new Vector4(value, 0, 0, 0));

        public void SetClearValue(int index, Vector2 value) => SetClearValue(index, new Vector4(value, 0, 0));

        public void SetClearValue(int index, Vector3 value) => SetClearValue(index, new Vector4(value, 0));

        public void SetClearValue(int index, Color value) => SetClearValue(index, value.ToVector4());

        public abstract Vector4 GetClearValue(int index);

        public abstract MeshBuffer CreateMeshBuffer(VertexFormat format, PrimitiveType primitive, IntPtr vertexData, int vertexCount, IntPtr indexData, int indexCount);

        public abstract Texture2D CreateTexture2D(Image image, FilterMode filter, WrapMode wrap, int compress, bool sRgb);

        public abstract Texture2D CreateTexture2D(int width, int height, PixelFormat format, FilterMode filter, WrapMode wrap, IntPtr data);

        public abstract CubeMap CreateCubeMap(Image[] images, FilterMode filter, WrapMode wrap, int compress, bool sRgb);

        public abstract CubeMap CreateCubeMap(int faceSize, PixelFormat format, FilterMode filter, WrapMode wrap, IntPtr[] data);

        public RenderTarget CreateRenderTarget(int width, int height, params PixelFormat[] colorBuffers)
        {
            return CreateRenderTarget(width, height, true, colorBuffers);
        }

        public abstract RenderTarget CreateRenderTarget(int width, int height, bool depthStencil, params PixelFormat[] colorBuffers);

        public abstract Shader CreateShader(string vert, string geom, string frag);

        public abstract void SetStencil(StencilFunc compare, byte value, bool write);

        public abstract void SetStencil(bool enabled);

        public abstract void SetFillMode(FillMode value);

        public abstract void SetFaceCull(FaceCull value);

        public abstract void SetDepthWrite(bool value);

        public abstract void SetDepthTest(DepthTest value);

        public abstract void SetBlendMode(BlendMode value);
        
        public void BlitRenderTarget(RenderTarget from, RenderTarget to, int readIndex, BufferMask mask, FilterMode filter)
        {
            BlitRenderTarget(from, to, readIndex, mask, filter, new Rect(0, 0, from.Width, from.Height), new Rect(0, 0, to.Width, to.Height));
        }

        public abstract void BlitRenderTarget(RenderTarget from, RenderTarget to, int readIndex, BufferMask mask, FilterMode filter, Rect src, Rect dest);

        public abstract void SetUniformBlock(int index, IntPtr data, int size);

        public abstract void SetShader(Shader shader, object obj = null);

        public abstract void Draw(MeshBuffer mesh, int offset, int count);

        public void Draw(MeshBuffer mesh) => Draw(mesh, 0, mesh.IndexCount);
    }

    public enum BufferMask
    {
        Color = 0b0001,
        Depth = 0b0010,
        Stencil = 0b0100,
        All = 0b1111
    }

    public enum StencilFunc
    {
        Never,
        Less,
        Equal,
        Lequal,
        Greater,
        Notequal,
        Gequal,
        Always,
    }

    public enum WrapMode { Repeat, Clamp }

    public enum BlendMode { Disable, AlphaBlend, Additive, Modulative }

    public enum DepthTest { Disable, Always, Greater, Less, Never }

    public enum FaceCull { None, Back, Front }

    public enum FillMode { Point, Line, Fill }

    public enum PrimitiveType
    {
        Points,
        Lines,
        LineLoop,
        LineStrip,
        Triangles,
        TriangleStrip,
        TriangleFan,
    }

    public enum FilterMode
    {
        Point,
        Linear,
        Bilinear,
        Trilinear,
        Aniso4x,
        Aniso8x,
        Aniso16x
    }

    public enum PixelFormat
    {
        R8,
        R16,
        R16F,
        R32F,
        Rg8,
        Rg16,
        Rg16F,
        Rg32F,
        Rgb8,
        sRgb8,
        Rgb16,
        Rgb16F,
        Rgb32F,
        Rgba8,
        sRgba8,
        Rgba16,
        Rgba16F,
        Rgba32F,
        Dxt1,
        Dxt5,
        sRgbDxt1,
        sRgbDxt5,
        R11G11B10F,
        Depth16F,
        Depth32F,
        Depth24Stencil8,
    }
}