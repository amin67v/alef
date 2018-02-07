using System;
using System.Numerics;
using static System.MathF;

namespace Engine
{
    public class Mesh : Disposable, IDrawable
    {
        Array<Vertex> vtx_arr = new Array<Vertex>();
        Array<ushort> idx_arr = new Array<ushort>();
        Transform xform;
        MeshBuffer<Vertex> mb;
        Texture main_tex;
        Shader shader;
        Rect bounds;
        BlendMode blend = BlendMode.Disabled;
        int layer;
        DirtyFlags dirty = DirtyFlags.All;

        public Mesh()
        {
            shader = DefaultShaders.ColorMult;
            mb = MeshBuffer<Vertex>.CreateIndexed();
        }

        public int VertexCount => vtx_arr.Count;

        public int IndexCount => idx_arr.Count;

        public Vertex GetVertex(int i) => vtx_arr[i];

        public ushort GetIndex(int i) => idx_arr[i];

        public int Layer
        {
            get => layer;
            set => layer = value;
        }

        /// <summary>
        /// BlendMode used to draw this mesh
        /// </summary>
        public BlendMode BlendMode
        {
            get => blend;
            set => blend = value;
        }

        /// <summary>
        /// Transform this mesh is attached to
        /// </summary>
        public Transform Transform
        {
            get => xform;
            set => xform = value;
        }

        /// <summary>
        /// Main texture used to draw this mesh
        /// </summary>
        public Texture MainTexture
        {
            get => main_tex;
            set => main_tex = value;
        }

        /// <summary>
        /// Shader used to draw this mesh
        /// </summary>
        public Shader Shader
        {
            get => shader;
            set => shader = value;
        }

        /// <summary>
        /// Bounding rect which all vertices are inside
        /// </summary>
        public Rect Bounds
        {
            get
            {
                // calc bound if needed
                if ((dirty & DirtyFlags.Bounds) != 0)
                {
                    float xmin = float.MaxValue;
                    float xmax = float.MinValue;
                    float ymin = float.MaxValue;
                    float ymax = float.MinValue;
                    for (int i = 0; i < vtx_arr.Count; i++)
                    {
                        var pos = vtx_arr[i].Position;
                        xmin = Min(xmin, pos.X);
                        xmax = Max(xmax, pos.X);
                        ymin = Min(ymin, pos.Y);
                        ymax = Max(ymax, pos.Y);
                    }
                    bounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
                    dirty &= ~DirtyFlags.Bounds;
                }
                return bounds;
            }
        }

        /// <summary>
        /// Modify vertex buffer at the given index
        /// </summary>
        public void SetVertex(int i, Vertex value)
        {
            vtx_arr[i] = value;
            dirty |= DirtyFlags.VertexArray | DirtyFlags.Bounds;
        }

        /// <summary>
        /// Modify index buffer at the given index
        /// </summary>
        public void SetIndex(int i, ushort value)
        {
            idx_arr[i] = value;
            dirty |= DirtyFlags.IndexArray;
        }

        /// <summary>
        /// Add new vertex to the vertex buffer
        /// </summary>
        public void AddVertex(Vertex value)
        {
            vtx_arr.Push(value);
            dirty |= DirtyFlags.VertexArray | DirtyFlags.Bounds;
        }

        /// <summary>
        /// Add new set of vertices to the vertex buffer
        /// </summary>
        public void AddVertex(params Vertex[] value)
        {
            vtx_arr.Push(value);
            dirty |= DirtyFlags.VertexArray | DirtyFlags.Bounds;
        }

        /// <summary>
        /// Add new index to the index buffer
        /// </summary>
        public void AddIndex(ushort value)
        {
            idx_arr.Push(value);
            dirty |= DirtyFlags.IndexArray;
        }

        /// <summary>
        /// Add new set of indices to the index buffer
        /// </summary>
        public void AddIndex(params ushort[] value)
        {
            idx_arr.Push(value);
            dirty |= DirtyFlags.IndexArray;
        }

        /// <summary>
        /// Clears all vertices inside vertex buffer
        /// </summary>
        public void ClearVertices()
        {
            vtx_arr.Clear(false);
            dirty |= DirtyFlags.VertexArray | DirtyFlags.Bounds;
        }

        /// <summary>
        /// Clears all indices inside index buffer
        /// </summary>
        public void ClearIndices()
        {
            idx_arr.Clear(false);
            dirty |= DirtyFlags.IndexArray;
        }

        public void Draw()
        {
            if (idx_arr.Count > 0)
            {
                var gfx = App.Graphics;
                gfx.SetBlendMode(blend);
                gfx.SetShader(shader);
                OnSetUniforms();

                if ((dirty & DirtyFlags.VertexArray) != 0)
                {
                    mb.UpdateVertices(vtx_arr);
                    dirty &= ~DirtyFlags.VertexArray;
                }

                if ((dirty & DirtyFlags.IndexArray) != 0)
                {
                    mb.UpdateIndices(idx_arr);
                    dirty &= ~DirtyFlags.IndexArray;
                }

                (App.ActiveState as Scene)?.DebugDraw.Rect(Bounds, Color.Red);
                mb.Draw(PrimitiveType.Triangles);
            }
        }

        /// <summary>
        /// Override to set custom uniforms
        /// </summary>
        protected virtual void OnSetUniforms()
        {
            shader.SetTexture("main_tex", 0, main_tex);
            shader.SetMatrix4x4("view_mat", App.Graphics.ViewMatrix);
            if (xform != null)
                shader.SetMatrix3x2("model_mat", xform.Matrix);
            else
                shader.SetMatrix3x2("model_mat", Matrix3x2.Identity);
        }

        protected override void OnDisposeManaged()
        {
            vtx_arr = null;
            idx_arr = null;
            xform = null;
            main_tex = null;
            shader = null;
            mb.Dispose();
        }

        enum DirtyFlags
        {
            VertexArray = 1 << 0,
            IndexArray = 1 << 1,
            Bounds = 1 << 2,

            All = VertexArray | IndexArray | Bounds
        }
    }
}