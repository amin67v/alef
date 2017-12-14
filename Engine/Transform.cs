using System;
using System.Numerics;

namespace Engine
{
    public class Transform
    {
        Matrix3x2 matrix = Matrix3x2.Identity;
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
                if (dirty)
                {
                    matrix = Matrix3x2.CreateRotation(rot);
                    matrix.Translation = pos;
                    matrix.M11 *= scale;
                    matrix.M22 *= scale;
                    dirty = false;
                }
                return matrix;
            }
        }

        public Vector2 LocalToWorld(Vector2 pos)
        {
            return Vector2.Transform(pos, Matrix);
        }

        public Vector2 WorldToLocal(Vector2 pos)
        {
            Matrix3x2 inv_mat;
            if (Matrix3x2.Invert(Matrix, out inv_mat))
            {
                return Vector2.Transform(pos,inv_mat);
            }
            else
            {
                return pos;
            }
        }

        public Vector2 LocalToWorldNormal(Vector2 pos)
        {
            return Vector2.TransformNormal(pos, Matrix);
        }

        public Vector2 WorldToLocalNormal(Vector2 pos)
        {
            Matrix3x2 inv_mat;
            if (Matrix3x2.Invert(Matrix, out inv_mat))
            {
                return Vector2.TransformNormal(pos,inv_mat);
            }
            else
            {
                return pos;
            }
        }
    }
}