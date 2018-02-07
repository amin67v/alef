using System;
using System.Numerics;

namespace Engine
{
    public interface IGraphicsDevice
    {
        string DriverInfo { get; }
        Matrix4x4 ViewMatrix { get; set; }

        void Clear(Color color);
        void Display();

        Texture CreateTexture(int width, int height, FilterMode filter, WrapMode wrap, IntPtr data);
        MeshBuffer<T> CreateMeshBuffer<T>(IntPtr vtx_data, int vtx_count) where T : struct, IVertex;
        MeshBuffer<T> CreateMeshBuffer<T>(IntPtr vtx_data, int vtx_count, IntPtr idx_data, int idx_count) where T : struct, IVertex;
        RenderTarget CreateRenderTarget(int width, int height, FilterMode filter, int count);
        Shader CreateShader(string vert, string geom, string frag);

        void SetBlendMode(BlendMode value);
        void SetRenderTarget(RenderTarget value);
        void SetShader(Shader value);
        void SetViewport(int x, int y, int w, int h);
        void SetViewport(Rect rect);
        void SetScissor(int x, int y, int w, int h);
        void SetScissor(Rect rect);
        void SetScissorOff();
        void SetPointSize(float value);
        void SetLineWidth(float value);
    }

    public enum WrapMode { Repeat, Clamp }

    public enum BlendMode
    {
        Disabled,
        AlphaBlend,
        Additive,
        Modulative
    }

    public enum PrimitiveType
    {
        Points = 0,
        Lines = 1,
        LineLoop = 2,
        LineStrip = 3,
        Triangles = 4,
        TriangleStrip = 5,
        TriangleFan = 6
    }

    public enum FilterMode { Point, Bilinear, Trilinear }
}