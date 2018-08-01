using System;

using static System.MathF;

namespace Engine
{
    public class LightNode : Node
    {
        LightType type = LightType.Point;
        Color color = Color.White;
        float intensity = 1f;
        float angle = 45f;

        public LightType LightType
        {
            get => type;
            set
            {
                if (type == value)
                    return;
                
                Unregister($"Renderer.{type.ToString()}Light");
                type = value;
                Register($"Renderer.{value.ToString()}Light");
            }
        }

        public float Angle
        {
            get => angle;
            set => angle = value;
        }

        public float Radius
        {
            get => Scale;
            set => Scale = Max(0, value);
        }

        public Color Color
        {
            get => color;
            set => color = value;
        }

        public float Intensity
        {
            get => intensity;
            set => intensity = Max(0, value);
        }

        protected override void OnBegin() => Register($"Renderer.{type.ToString()}Light");

        protected override void OnDestroy() => Unregister($"Renderer.{type.ToString()}Light");
    }

    public enum LightType { Point, Spot }
}