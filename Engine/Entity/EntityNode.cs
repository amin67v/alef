using System;
using System.Numerics;

namespace Engine
{
    public class EntityNode
    {
        EntityNode parent;
        Array<EntityNode> childs = new Array<EntityNode>();
        Matrix3x2 matrix = Matrix3x2.Identity;
        Matrix3x2 inv_matrix = Matrix3x2.Identity;
        Vector2 pos = Vector2.Zero;
        float rot = 0;
        float scale = 1;
        int dirty = 0xff;

        public EntityNode Parent
        {
            get => parent;
            set
            {
                if (parent != value)
                {
                    parent?.childs.Remove(this);
                    parent = value;
                    parent?.childs.Push(this);
                }
            }
        }

        public Vector2 Position
        {
            get => pos;
            set
            {
                pos = value;
                set_dirty();
            }
        }

        public float Rotation
        {
            get => rot;
            set
            {
                rot = value;
                set_dirty();
            }
        }

        public float Scale
        {
            get => scale;
            set
            {
                scale = value;
                set_dirty();
            }
        }

        public Matrix3x2 Matrix
        {
            get
            {
                if ((dirty & 0x01) != 0)
                {
                    matrix = Matrix3x2.CreateRotation(rot);
                    matrix.Translation = pos;
                    matrix.M11 *= scale;
                    matrix.M22 *= scale;
                    dirty &= ~0x01;
                }

                return matrix;
            }
        }

        public Matrix3x2 InvMatrix
        {
            get
            {
                if ((dirty & 0x02) != 0)
                {
                    Matrix3x2.Invert(this.Matrix, out inv_matrix);
                    dirty &= ~0x02;
                }
                return inv_matrix;
            }
        }

        public Vector2 LocalToWorld(Vector2 pos) => Vector2.Transform(pos, Matrix);

        public Vector2 WorldToLocal(Vector2 pos) => Vector2.Transform(pos, InvMatrix);

        public Vector2 LocalToWorldNormal(Vector2 normal) => Vector2.TransformNormal(normal, Matrix);

        public Vector2 WorldToLocalNormal(Vector2 normal) => Vector2.TransformNormal(normal, InvMatrix);

        public void CreateChild<T>() where T : EntityNode, new()
        {
            var node = new T();
            node.Parent = this;
        }

        /// <summary>
        /// Called at the start of each frame
        /// </summary>
        protected virtual void OnUpdate(float dt) { }

        internal void process(float dt)
        {
            OnUpdate(dt);
            for (int i = 0; i < childs.Count; i++)
                childs[i].process(dt);
        }

        void set_dirty()
        {
            dirty = 0xff;
            for (int i = 0; i < childs.Count; i++)
                childs[i].dirty = 0xff;
        }
    }
}