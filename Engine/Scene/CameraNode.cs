using System;
using System.Numerics;

using static Engine.Math;
using static System.MathF;

namespace Engine
{
    public class CameraNode : Node
    {
        float fov = 75f;
        float near = 0.1f;
        float far = 100f;

        /// <summary>
        /// Gets or sets cameras near clip
        /// </summary>
        public float NearClip
        {
            get => near;
            set => near = value;
        }

        /// <summary>
        /// Gets or sets cameras far clip
        /// </summary>
        public float FarClip
        {
            get => far;
            set => far = value;
        }

        /// <summary>
        /// Gets or sets cameras field of view in degree
        /// </summary>
        public float FieldOfView
        {
            get => fov;
            set => fov = Math.Clamp(value, 0, 180);
        }
    }
}