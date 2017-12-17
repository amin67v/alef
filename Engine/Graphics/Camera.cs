using System;
using System.Numerics;

namespace Engine
{
    public class Camera
    {
        ViewSizeMode size_mode = ViewSizeMode.Height;
        Color color = new Color(230, 230, 230, 255);
        Vector2 pos = Vector2.Zero;
        float rot = 0;
        float size = 5;
        Rect viewport = new Rect(0, 0, 1, 1);
        Matrix4x4 matrix = Matrix4x4.Identity;
        RenderTarget target;
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
                size = value;
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
                //value.X = MathF.Max(0, value.X);
                //value.Y = MathF.Max(0, value.Y);
                //value.Width = MathF.Min(1, value.Width);
                //value.Height = MathF.Min(1, value.Height);
                viewport = value;
                dirty = true;
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
                if (dirty)
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
                    dirty = false;
                }
                return matrix;
            }
        }

        public Vector2 ScreenToWorld(Vector2 pos)
        {
            var ndc = pos / App.Window.Size;
            ndc.X = ndc.X * 2 - 1;
            ndc.Y = ndc.Y * 2 - 1;
            return NdcToWorld(ndc);
        }

        public Vector2 WorldToScreen(Vector2 pos)
        {
            var ndc = WorldToNdc(pos);
            ndc.X = ndc.X * .5f + .5f;
            ndc.Y = ndc.Y * .5f + .5f;
            return ndc * App.Window.Size;
        }

        public Vector2 NdcToWorld(Vector2 pos)
        {
            Matrix4x4 inv;
            Matrix4x4.Invert(ViewMatrix, out inv);
            return Vector2.Transform(pos, inv);
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
                scene.draw_list[i].Draw();

            scene.DebugDraw.DrawAll();
        }

        public enum ViewSizeMode { Width, Height }
    }
}