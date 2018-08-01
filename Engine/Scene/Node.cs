using System;
using System.Numerics;

namespace Engine
{
    public class Node : ObjectBase
    {
        Scene scene;
        Array<Node> childs;
        Node parent = null;
        Vector3 pos = Vector3.Zero;
        float scale = 1f;
        Quaternion rot = Quaternion.Identity;
        Matrix4x4 matrix = Matrix4x4.Identity;
        bool dirty = true;

        /// <summary>
        /// Gets child at index
        /// </summary>
        public Node this[int index] => childs?[index];

        /// <summary>
        /// Returns number of childs this node has
        /// </summary>
        public int ChildCount => childs?.Count ?? 0;

        /// <summary>
        /// Gets world position
        /// </summary>
        public Vector3 WorldPosition => Matrix.Translation;

        /// <summary>
        /// Gets world rotation 
        /// </summary>
        public Quaternion WorldRotation
        {
            get
            {
                Quaternion wrot = Rotation;
                if (Parent != null)
                    wrot = Quaternion.Concatenate(Parent.WorldRotation, wrot);

                return wrot;
            }
        }

        /// <summary>
        /// Gets world scale
        /// </summary>
        public float WorldScale
        {
            get
            {
                float wscale = Scale;
                if (Parent != null)
                    wscale *= Parent.WorldScale;

                return wscale;
            }
        }

        /// <summary>
        /// Gets or sets local position relative to parent
        /// </summary>
        public Vector3 Position
        {
            get => pos;
            set
            {
                pos = value;
                setDirty();
            }
        }

        /// <summary>
        /// Gets or sets rotation relative to parent
        /// </summary>
        public Quaternion Rotation
        {
            get => rot;
            set
            {
                rot = value;
                setDirty();
            }
        }

        /// <summary>
        /// Gets or sets scale relative to parent
        /// </summary>
        public float Scale
        {
            get => scale;
            set
            {
                scale = value;
                setDirty();
            }
        }

        /// <summary>
        ///Gets model matrix, built from position, rotation and scale
        /// </summary>
        public Matrix4x4 Matrix
        {
            get
            {
                if (dirty)
                {
                    matrix = Matrix4x4.CreateFromQuaternion(rot);
                    matrix.Translation = pos;
                    matrix.M11 *= scale;
                    matrix.M12 *= scale;
                    matrix.M13 *= scale;
                    matrix.M21 *= scale;
                    matrix.M22 *= scale;
                    matrix.M23 *= scale;
                    matrix.M31 *= scale;
                    matrix.M32 *= scale;
                    matrix.M33 *= scale;

                    if (parent != null)
                        matrix *= parent.Matrix;

                    dirty = false;
                }
                return matrix;
            }
        }

        public Vector3 Forward => Vector3.Normalize(new Vector3(Matrix.M31, Matrix.M32, Matrix.M33));

        public Vector3 Right => Vector3.Normalize(new Vector3(Matrix.M11, Matrix.M12, Matrix.M13));

        public Vector3 Up => Vector3.Normalize(new Vector3(Matrix.M21, Matrix.M22, Matrix.M23));

        public Scene Scene => scene;

        public Node Parent
        {
            get => parent;
            set
            {
                if (parent != value)
                {
                    if (value.IsChildOf(this))
                    {
                        Log.Error($"Failed to set Node parent, child Node can not be set as parent.");
                        return;
                    }

                    if (value == this)
                    {
                        Log.Error($"Failed to set Node parent, you can not set parent to itself.");
                        return;
                    }

                    parent?.childs?.SwapAndPop(this);
                    parent = value;
                    if (parent != null)
                    {
                        if (parent.childs == null)
                            parent.childs = new Array<Node>();

                        parent.childs.Push(this);
                    }
                }
            }
        }

        public void Register(string group) => Scene.RegisterNode(group, this);

        public void Unregister(string group) => Scene.UnregisterNode(group, this);

        public bool IsChildOf(Node other)
        {
            var current = parent;
            while (current != null)
            {
                if (current == other)
                    return true;

                current = current.parent;
            }

            return false;
        }

        public Node Find(string name)
        {
            if (childs != null)
            {
                int hash = Hash.FromString(name);
                for (int i = 0; i < ChildCount; i++)
                {
                    if (childs[i].CompareName(name, hash))
                        return childs[i];
                }
            }

            return null;
        }

        public T AddChild<T>() where T : Node, new()
        {
            var node = new T();
            node.Name = typeof(T).Name;
            node.InternalBegin(this.Scene, this);
            return node;
        }

        internal override void DestroyImp(bool destroying)
        {
            if (destroying)
            {
                if (!IsDestroyed)
                {
                    if (childs != null)
                    {
                        for (int i = ChildCount - 1; i >= 0; --i)
                            childs[i].DestroyImp(true);
                    }

                    OnDestroy();

                    if (this is IDrawNode)
                        Unregister("Node.Draw");

                    if (this is IUpdateNode)
                        Unregister("Node.Update");

                    parent?.childs?.SwapAndPop(this);

                    if (parent == null)
                        Scene.RemoveNode(this);
                    else
                        parent.childs.SwapAndPop(this);

                    IsDestroyed = true;
                    GC.SuppressFinalize(this);
                }
            }
            else
            {
                Register("Node.Destroy");
            }
        }

        internal void InternalBegin(Scene scene, Node parent)
        {
            this.scene = scene;
            this.Parent = parent;

            if (this is IDrawNode)
                Register("Node.Draw");

            if (this is IUpdateNode)
                Register("Node.Update");

            OnBegin();
        }

        void setDirty()
        {
            dirty = true;

            for (int i = ChildCount - 1; i >= 0; --i)
                childs[i].setDirty();
        }
    }
}