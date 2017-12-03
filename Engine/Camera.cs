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
        Rect viewport = new Rect(0, 0, 1, 1);

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
            set => pos = value;
        }

        public float Rotation
        {
            get => rot;
            set => rot = value;
        }

        public float ViewSize
        {
            get => size;
            set => size = value;
        }

        public Rect Viewport
        {
            get => viewport;
            set => viewport = value;
        }

        public enum ViewSizeMode
        {
            Width = 0,
            Height = 1
        }
    }
}