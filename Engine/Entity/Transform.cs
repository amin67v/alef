using System;
using System.Numerics;

namespace Engine
{
    public class Transform
    {
        Matrix3x2 matrix = Matrix3x2.Identity;
        Matrix3x2 inv_matrix = Matrix3x2.Identity;

        Vector2 pos = Vector2.Zero;
        float rot = 0;
        float scale = 1;
        bool dirty = true;

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

        public float Scale
        {
            get => scale;
            set
            {
                scale = value;
                dirty = true;
            }
        }

        public Matrix3x2 Matrix
        {
            get
            {
                calc_matrix();
                return matrix;
            }
        }

        public Matrix3x2 InvMatrix
        {
            get
            {
                calc_matrix();
                return inv_matrix;
            }
        }

        public Vector2 LocalToWorld(Vector2 pos) => Vector2.Transform(pos, Matrix);

        public Vector2 WorldToLocal(Vector2 pos) => Vector2.Transform(pos, InvMatrix);

        public Vector2 LocalToWorldNormal(Vector2 pos) => Vector2.TransformNormal(pos, Matrix);

        public Vector2 WorldToLocalNormal(Vector2 pos) => Vector2.TransformNormal(pos, InvMatrix);

        void calc_matrix()
        {
            if (dirty)
            {
                matrix = Matrix3x2.CreateRotation(rot);
                matrix.Translation = pos;
                matrix.M11 *= scale;
                matrix.M22 *= scale;
                Matrix3x2.Invert(matrix, out inv_matrix);
                dirty = false;
            }
        }
    }
}