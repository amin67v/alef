using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class EntityNode
    {
        string name;
        Entity entity;
        EntityNode parent;
        Array<EntityNode> childs = new Array<EntityNode>();
        Matrix3x2 matrix = Matrix3x2.Identity;
        Matrix3x2 inv_matrix = Matrix3x2.Identity;
        Vector2 pos = Vector2.Zero;
        Vector2 scale = Vector2.One;
        float rot = 0;
        int dirty = 0xff;
        bool is_destroyed;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool IsDestroyed => is_destroyed;

        public Entity Entity => entity;

        public EntityNode Parent
        {
            get => parent;
            set
            {
                // ignores set to same value
                if (parent != value)
                {
                    if (value.entity != this.entity)
                    {
                        Log.Error("You can not set node from other entity as parent to this node.");
                        return;
                    }

                    // ignores set parent to self childs !
                    if (value.IsSubChildOf(this))
                    {
                        Log.Error("You can not set node from sub childs as parent to this node.");
                        return;
                    }

                    parent?.childs.Remove(this);
                    parent = value;
                    parent.childs.Push(this);
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

        public Vector2 Scale
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
                    matrix.M11 *= scale.X;
                    matrix.M12 *= scale.X;
                    matrix.M21 *= scale.Y;
                    matrix.M22 *= scale.Y;
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

        /// <summary>
        /// Creates root node of type T for the given entity
        /// if root node already exist it will become child for the new root node
        /// </summary>
        public static T CreateRoot<T>(Entity entity) where T : EntityNode, new()
        {
            var node = new T();
            node.entity = entity;
            if (entity.RootNode != null)
                entity.RootNode.Parent = node;

            entity.RootNode = node;
            node.OnBegin();
            return node;
        }

        /// <summary>
        /// Creates a child node of type T
        /// </summary>
        public T CreateChild<T>(string name) where T : EntityNode, new()
        {
            var node = new T();
            node.name = name;
            node.entity = this.entity;
            node.parent = this;
            childs.Push(node);

            node.OnBegin();
            return node;
        }

        /// <summary>
        /// Is this node sub child of the given node
        /// </summary>
        public bool IsSubChildOf(EntityNode node)
        {
            var current = parent;
            while (current != null)
            {
                if (current == node)
                    return true;

                current = current.parent;
            }

            return false;
        }

        /// <summary>
        /// Destroys this node including sub childs
        /// </summary>
        public void Destroy()
        {
            void destroy(EntityNode node)
            {
                for (int i = 0; i < node.childs.Count; i++)
                    destroy(node.childs[i]);

                node.OnDestroy();
                node.is_destroyed = true;
            }

            if (is_destroyed == false)
            {
                if (parent == null) // root node
                    entity.RootNode = null;
                else
                    parent.childs.Remove(this);

                destroy(this);
            }
            else
            {
                Log.Warning($"Multiple destroy called on entity node with name '{Name}'.");
            }
        }

        public Vector2 LocalToWorld(Vector2 pos) => Vector2.Transform(pos, Matrix);

        public Vector2 WorldToLocal(Vector2 pos) => Vector2.Transform(pos, InvMatrix);

        public Vector2 LocalToWorldNormal(Vector2 normal) => Vector2.TransformNormal(normal, Matrix);

        public Vector2 WorldToLocalNormal(Vector2 normal) => Vector2.TransformNormal(normal, InvMatrix);

        /// <summary>
        /// Called as soon as node is created
        /// </summary>
        protected virtual void OnBegin() { }

        /// <summary>
        /// Called at the start of each frame
        /// </summary>
        protected virtual void OnUpdate(float dt) { }

        /// <summary>
        /// Called when node is going to be destroyed
        /// </summary>
        protected virtual void OnDestroy() { }

        internal void update(float dt)
        {
            OnUpdate(dt);
            for (int i = 0; i < childs.Count; i++)
                childs[i].update(dt);
        }

        void set_dirty()
        {
            dirty = 0xff;
            for (int i = 0; i < childs.Count; i++)
                childs[i].dirty = 0xff;
        }
    }
}