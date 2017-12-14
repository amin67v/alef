using System;
using System.Numerics;
using System.Runtime.InteropServices;

using static OpenGL;

namespace Engine
{
    public sealed class Graphics
    {
        string driver_info = null;
        BlendMode blend_mode = BlendMode.Disabled;
        Matrix4x4 view_mx = Matrix4x4.Identity;
        float point_size = 1f;
        float line_width = 1f;
        bool scissor = false;

        internal Graphics()
        {
            LoadFunctions();

            var vendor = glGetString(StringName.Vendor);
            var renderer = glGetString(StringName.Renderer);
            var version = glGetString(StringName.Version);
            driver_info = $"{vendor} - {renderer} : {version} ";
            App.Log.Info($"Graphics Driver:\n{DriverInfo}\n");

            // disable culling
            glDisable(EnableCap.CullFace);

            // no depth write
            glDepthMask(false);

            // no depth test
            glDisable(EnableCap.DepthTest);

            // disable blending
            glDisable(EnableCap.Blend);
        }

        public string DriverInfo => driver_info;

        public Matrix4x4 ViewMatrix => view_mx;

        public BlendMode BlendMode
        {
            get => blend_mode;
            set
            {
                if (blend_mode == value)
                    return;

                blend_mode = value;
                switch (value)
                {
                    case BlendMode.Disabled:
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

        public float LineWidth
        {
            get => line_width;
            set
            {
                if (line_width == value)
                    return;

                line_width = value;
                glLineWidth(value);
            }
        }

        public float PointSize
        {
            get => point_size;
            set
            {
                if (point_size == value)
                    return;

                point_size = value;
                glPointSize(value);
            }
        }

        public bool ScissorEnabled
        {
            get => scissor;
            set
            {
                if (scissor == value)
                    return;

                scissor = value;
                if (value)
                    glEnable(EnableCap.ScissorTest);
                else
                    glDisable(EnableCap.ScissorTest);
            }
        }

        public void Viewport(Rect rect) => glViewport((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        public void Viewport(int x, int y, int w, int h) => glViewport(x, y, w, h);

        public void Scissor(int x, int y, int w, int h) => glScissor(x, y, w, h);

        public void SetView(Vector2 pos, float rot, Vector2 size)
        {
            var proj = Matrix4x4.CreateOrthographic(size.X, size.Y, -1, 1);
            var view = Matrix4x4.CreateFromYawPitchRoll(0, 0, rot);
            view.Translation = new Vector3(pos.X, pos.Y, 0);
            Matrix4x4.Invert(view, out view);
            view_mx = view * proj;
        }

        public void SetView(float left, float right, float bottom, float top)
        {
            view_mx = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
        }

        public void Clear(Color color)
        {
            var c = color.ToVector4();
            glClearColor(c.X, c.Y, c.Z, c.W);
            glClear(ClearBufferMask.ColorBufferBit);
        }

        public void Display()
        {
            glFlush();
            ScissorEnabled = false;
#if DEBUG
            var err = glGetError();
            if (err != ErrorCode.NoError)
            {
                var msg = $"Opengl error at frame '0', {err.ToString()}.";
                App.Log.Error(msg);
                throw new Exception(msg);
            }
#endif
        }
        internal void shutdown()
        {
            driver_info = null;
        }
    }

    public enum BlendMode
    {
        Disabled = 0,
        AlphaBlend = 1,
        Additive = 2,
        Modulative = 3
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

    public enum FilterMode
    {
        Point = 0,
        Bilinear = 1,
        Trilinear = 2
    }
}