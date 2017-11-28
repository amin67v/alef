using System;
using System.Numerics;
using System.Runtime.InteropServices;

using static OpenGL;

namespace Engine
{
    public class Graphics
    {
        static readonly Graphics instance = new Graphics();

        string driver_info = null;
        BlendMode blend_mode = BlendMode.Disabled;
        Matrix4x4 view_mx = Matrix4x4.Identity;
        float point_size = 1f;
        float line_width = 1f;
        bool scissor = false;

        public static string DriverInfo => instance.driver_info;

        public static Matrix4x4 ViewMatrix => instance.view_mx;

        public static BlendMode BlendMode
        {
            get => instance.blend_mode;
            set
            {
                if (instance.blend_mode == value)
                    return;

                instance.blend_mode = value;
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

        public static float LineWidth
        {
            get => instance.line_width;
            set
            {
                if (instance.line_width == value)
                    return;

                instance.line_width = value;
                glLineWidth(value);
            }
        }

        public static float PointSize
        {
            get => instance.point_size;
            set
            {
                if (instance.point_size == value)
                    return;

                instance.point_size = value;
                glPointSize(value);
            }
        }

        public static bool ScissorEnabled
        {
            get => instance.scissor;
            set
            {
                if (instance.scissor == value)
                    return;

                instance.scissor = value;
                if (value)
                    glEnable(EnableCap.ScissorTest);
                else
                    glDisable(EnableCap.ScissorTest);
            }
        }

        public static void Viewport(int x, int y, int w, int h)
        {
            glViewport(x, y, w, h);
        }

        public static void Scissor(int x, int y, int w, int h)
        {
            glScissor(x, y, w, h);
        }

        public static void SetView(Vector2 pos, float rot, Vector2 size)
        {
            var proj = Matrix4x4.CreateOrthographic(size.X, size.Y, -1, 1);
            var view = Matrix4x4.CreateFromYawPitchRoll(0, 0, rot);
            view.Translation = new Vector3(pos.X, pos.Y, 0);
            Matrix4x4.Invert(view, out view);
            instance.view_mx = view * proj;
        }

        public static void SetView(float left, float right, float bottom, float top)
        {
            instance.view_mx = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
        }

        public static void Clear(Color color)
        {
            var c = color.ToVector4();
            glClearColor(c.X, c.Y, c.Z, c.W);
            glClear(ClearBufferMask.ColorBufferBit);
        }

        public static void Display()
        {
            glFlush();
            ScissorEnabled = false;
#if DEBUG
            var err = glGetError();
            if (err != ErrorCode.NoError)
            {
                var msg = $"Opengl error at frame '{Time.FrameIndex}', {err.ToString()}.";
                Log.Error(msg);
                throw new Exception(msg);
            }
#endif
        }

        internal static void Init()
        {
            LoadFunctions();

            var vendor = glGetString(StringName.Vendor);
            var renderer = glGetString(StringName.Renderer);
            var version = glGetString(StringName.Version);
            instance.driver_info = $"{vendor} - {renderer} : {version} ";
            Log.Info(DriverInfo);

            // disable culling
            glDisable(EnableCap.CullFace);

            // no depth write
            glDepthMask(false);

            // no depth test
            glDisable(EnableCap.DepthTest);

            // disable blending
            glDisable(EnableCap.Blend);
        }

        internal static void Shutdown()
        {
            instance.driver_info = null;
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