using System;
using System.Numerics;
using static System.MathF;

namespace Engine
{
    public class DrawableMesh : Disposable, IDrawable
    {
        Array<Vertex> vtx_arr;
        Array<ushort> idx_arr;
        MeshBuffer<Vertex> buffer;
        Matrix3x2 xform;
        BlendMode blend;
        Texture main_tex;
        Shader shader;
        Rect bounds;
        int layer;

        public DrawableMesh()
        {
            vtx_arr = new Array<Vertex>();
            idx_arr = new Array<ushort>();
            buffer = MeshBuffer<Vertex>.CreateIndexed();
            xform = Matrix3x2.Identity;
            main_tex = Data.Get<Texture>("White.Texture");
            shader = Data.Get<Shader>("Mult.Shader");
        }

        /// <summary>
        /// Vertex array used to fill the mesh buffer
        /// Note: You should call UpdateVertices and UpdateBounds after modifying this array
        /// </summary>
        public Array<Vertex> VertexArray => vtx_arr;

        /// <summary>
        /// Index array used to fill the mesh buffer
        /// Note: You should call UpdateIndices after modifying this array
        /// </summary>
        public Array<ushort> IndexArray => idx_arr;

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
        /// Transform matrix for this mesh
        /// </summary>
        public Matrix3x2 Transform
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
        public Rect Bounds => bounds;

        public virtual void Draw()
        {
            if (buffer.IndexCount > 0)
            {
                var gfx = App.Graphics;
                gfx.SetBlendMode(blend);
                gfx.SetShader(shader);
                OnSetUniforms();

                buffer.Draw(PrimitiveType.Triangles);
            }
        }

        /// <summary>
        /// Updates Vertex/Index buffer and recalculate bounds
        /// </summary>
        public void UpdateAll()
        {
            UpdateVertices();
            UpdateIndices();
            UpdateBounds();
        }

        /// <summary>
        /// Recalculate bounds based on vertex array
        /// </summary>
        public void UpdateBounds()
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
        }

        /// <summary>
        /// Sends data from VertexArray to mesh buffer
        /// </summary>
        public void UpdateVertices() => buffer.UpdateVertices(vtx_arr);

        /// <summary>
        /// Sends data from IndexArray to mesh buffer
        /// </summary>
        public void UpdateIndices() => buffer.UpdateIndices(idx_arr);

        /// <summary>
        /// Override to set custom uniforms
        /// </summary>
        protected virtual void OnSetUniforms()
        {
            shader.SetTexture("main_tex", 0, main_tex);
            shader.SetMatrix4x4("view_mat", App.Graphics.ViewMatrix);
            shader.SetMatrix3x2("model_mat", xform);
        }

        protected override void OnDisposeManaged()
        {
            vtx_arr.Clear();
            idx_arr.Clear();
            vtx_arr = null;
            idx_arr = null;
            main_tex = null;
            shader = null;
            buffer.Dispose();
        }
    }
}