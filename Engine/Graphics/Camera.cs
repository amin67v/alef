using System;
using System.Numerics;

namespace Engine
{
    public class Camera
    {
        ViewSizeMode size_mode = ViewSizeMode.Height;
        Color clear_color = Color.DeepSkyBlue;
        Vector2 pos = Vector2.Zero;
        float rot = 0;
        float size = 10;
        //Rect viewport = new Rect(0, 0, 1, 1);
        Matrix4x4 matrix = Matrix4x4.Identity;
        bool dirty = true;

        public ViewSizeMode SizeMode
        {
            get => size_mode;
            set => size_mode = value;
        }

        public Color ClearColor
        {
            get => clear_color;
            set => clear_color = value;
        }

        public Vector2 Position
        {
            get => pos;
            set
            {
                pos = value;
                dirty = true;
            }
        }

        public float Rotation
        {
            get => rot;
            set
            {
                rot = value;
                dirty = true;
            }
        }

        public float ViewSize
        {
            get => size;
            set
            {
                size = value;
                dirty = true;
            }
        }

        // public Rect Viewport
        // {
        //     get => viewport;
        //     set
        //     {
        //         viewport = value;
        //         dirty = true;
        //     }
        // }

        public enum ViewSizeMode
        {
            Width = 0,
            Height = 1
        }

        public Matrix4x4 ViewMatrix
        {
            get
            {
                if (dirty)
                {
                    Vector2 view_size;
                    var wnd_size = App.Window.Size;
                    //var viewport_size = Viewport * wnd_size;
                    if (SizeMode == ViewSizeMode.Width)
                    {
                        view_size.X = ViewSize;
                        view_size.Y = view_size.X * (wnd_size.Y / wnd_size.X);
                    }
                    else
                    {
                        view_size.Y = ViewSize;
                        view_size.X = view_size.Y * (wnd_size.X / wnd_size.Y);
                    }

                    var proj = Matrix4x4.CreateOrthographic(view_size.X, view_size.Y, -1, 1);
                    var view = Matrix4x4.CreateFromYawPitchRoll(0, 0, rot);
                    view.Translation = new Vector3(pos.X, pos.Y, 0);
                    Matrix4x4.Invert(view, out view);
                    matrix = view * proj;
                    dirty = false;
                }
                return matrix;
            }
        }

        //public Vector2 ScreenToWorld(Vector2 pos)
        //{
        //
        //}

        // public Vector2 WorldToScreen(Vector2 pos)
        // {
        // 
        // }

        public void Render(Scene scene)
        {
            if (scene.IsRendering == false)
                throw new Exception("You can only call cameras render inside scene OnRender method.");

            var gfx = App.Graphics;

            //gfx.Viewport(viewport);
            gfx.Clear(ClearColor);

            gfx.ViewMatrix = ViewMatrix;
            for (int i = 0; i < scene.draw_list.Count; i++)
                scene.draw_list[i].Draw();
        }
    }
}