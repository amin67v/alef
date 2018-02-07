using System;
using System.Numerics;
using static System.MathF;

namespace Engine
{
    public class Camera
    {
        ViewSizeMode size_mode = ViewSizeMode.Height;
        RenderTarget target;
        Matrix4x4 inv_matrix = Matrix4x4.Identity;
        Matrix4x4 matrix = Matrix4x4.Identity;
        Vector2 pos = Vector2.Zero;
        Color color = new Color(230, 230, 230, 255);
        float rot = 0;
        float size = 10;
        Rect viewport = new Rect(0, 0, 1, 1);
        Rect view_rect;
        bool dirty = true;

        /// <summary>
        /// Gets or sets color used to clear background
        /// </summary>
        public Color ClearColor
        {
            get => color;
            set => color = value;
        }

        /// <summary>
        /// Gets or sets position for the camera
        /// </summary>
        public Vector2 Position
        {
            get => pos;
            set
            {
                pos = value;
                dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets rotation for the camera in radian
        /// </summary>
        public float Rotation
        {
            get => rot;
            set
            {
                rot = value;
                dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets size of the view for the camera
        /// </summary>
        public float ViewSize
        {
            get => size;
            set
            {
                size = Max(0, value);
                dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets size mode where view size relys on
        /// </summary>
        public ViewSizeMode SizeMode
        {
            get => size_mode;
            set => size_mode = value;
        }

        /// <summary>
        /// Gets or sets viewport where camera render,
        /// </summary>
        public Rect Viewport
        {
            get => viewport;
            set
            {
                value.X = Max(0, value.X);
                value.Y = Max(0, value.Y);
                value.Width = Min(1, value.Width);
                value.Height = Min(1, value.Height);
                viewport = value;
                dirty = true;
            }
        }

        /// <summary>
        /// Gets the axis aligned view rect in world space
        /// </summary>
        public Rect ViewRect
        {
            get
            {
                calc_matrix();
                return view_rect;
            }
        }

        /// <summary>
        /// Gets viewport in pixel coordinate
        /// </summary>
        public Rect PixelViewport => Viewport * App.Window.Size;

        /// <summary>
        /// Render target that camera draws into, if null draws into screen instead
        /// </summary>
        public RenderTarget Target
        {
            get => target;
            set => target = value;
        }

        /// <summary>
        /// View matrix for the camera
        /// </summary>
        public Matrix4x4 ViewMatrix
        {
            get
            {
                calc_matrix();
                return matrix;
            }
        }

        /// <summary>
        /// Invert of view matrix for the camera
        /// </summary>
        public Matrix4x4 InvViewMatrix
        {
            get
            {
                calc_matrix();
                return inv_matrix;
            }
        }

        public Vector2 ScreenToWorld(Vector2 pos)
        {
            return Vector2.Zero;
            //var pixelvp = PixelViewport;
            //var ndc = pos / App.Window.Size;
            //ndc.X = ndc.X * 2 - 1;
            //ndc.Y = ndc.Y * 2 - 1;
            //return Vector2.Transform(ndc, InvViewMatrix);
        }

        public Vector2 WorldToScreen(Vector2 pos)
        {
            return Vector2.Zero;
            //var ndc = Vector2.Transform(pos, ViewMatrix);
            //ndc.X = ndc.X * .5f + .5f;
            //ndc.Y = ndc.Y * .5f + .5f;
            //return ndc * App.Window.Size;
        }

        public Vector2 WorldToNdc(Vector2 pos)
        {
            return Vector2.Transform(pos, ViewMatrix);
        }

        public void Render(Scene scene)
        {
            if (scene.IsRendering == false)
                throw new Exception("You can only call cameras render inside scene OnRender method.");

            var gfx = App.Graphics;
            gfx.SetRenderTarget(target);
            var pixelvp = PixelViewport;
            gfx.SetViewport(pixelvp);
            gfx.SetScissor(pixelvp);
            gfx.Clear(ClearColor);

            gfx.ViewMatrix = ViewMatrix;

            for (int i = 0; i < scene.draw_list.Count; i++)
            {
                var item = scene.draw_list[i];
                if (ViewRect.Overlap(item.Bounds))
                    item.Draw();
            }

            scene.DebugDraw.DrawAll();
        }

        void calc_matrix()
        {
            if (dirty)
            {
                // calculate view matrix and inv view matrix
                {
                    var pixelvp = PixelViewport;
                    Vector2 size_xy;
                    if (SizeMode == ViewSizeMode.Width)
                    {
                        size_xy.X = ViewSize;
                        size_xy.Y = size_xy.X * (pixelvp.Height / pixelvp.Width);
                    }
                    else
                    {
                        size_xy.Y = ViewSize;
                        size_xy.X = size_xy.Y * (pixelvp.Width / pixelvp.Height);
                    }

                    var proj = Matrix4x4.CreateOrthographic(size_xy.X, size_xy.Y, -1, 1);
                    var view = Matrix4x4.CreateFromYawPitchRoll(0, 0, rot);
                    view.Translation = new Vector3(pos.X, pos.Y, 0);
                    Matrix4x4.Invert(view, out view);
                    matrix = view * proj;
                    Matrix4x4.Invert(matrix, out inv_matrix);
                }

                // calculate view rect
                {
                    const float s = 1;

                    var r = new Rect(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue);
                    var a = Vector2.Transform(new Vector2(-s, -s), inv_matrix);
                    var b = Vector2.Transform(new Vector2(s, s), inv_matrix);
                    var c = Vector2.Transform(new Vector2(-s, s), inv_matrix);
                    var d = Vector2.Transform(new Vector2(s, -s), inv_matrix);
                    r.XMin = Min(Min(a.X, b.X), Min(c.X, d.X));
                    r.XMax = Max(Max(a.X, b.X), Max(c.X, d.X));
                    r.YMin = Min(Min(a.Y, b.Y), Min(c.Y, d.Y));
                    r.YMax = Max(Max(a.Y, b.Y), Max(c.Y, d.Y));
                    view_rect = r;
                }

                dirty = false;
            }
        }

        public enum ViewSizeMode { Width, Height }
    }
}