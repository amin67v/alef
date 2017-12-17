using System;
using System.Runtime.InteropServices;

namespace Engine
{
    public abstract class MeshBuffer : Disposable
    {
        /// <summary>
        /// Number of vertices this mesh buffer contains
        /// </summary>
        public abstract int VertexCount { get; }

        /// <summary>
        /// Number of indices this mesh buffer contains
        /// </summary>
        public abstract int IndexCount { get; }

        /// <summary>
        /// Does this mesh buffer use index buffer?
        /// </summary>
        public abstract bool HasIndexBuffer { get; }

        /// <summary>
        /// Draw this mesh buffer
        /// </summary>
        public void Draw(PrimitiveType type)
        {
            Draw(0, HasIndexBuffer ? IndexCount : VertexCount, type);
        }

        /// <summary>
        /// Draw this mesh buffer
        /// </summary>
        public abstract void Draw(int offset, int count, PrimitiveType type);

        /// <summary>
        /// Creates an empty mesh buffer
        /// </summary>
        public static MeshBuffer Create()
        {
            return Create(IntPtr.Zero, 0);
        }

        /// <summary>
        /// Creates an empty mesh buffer including index buffer
        /// </summary>
        public static MeshBuffer CreateIndexed()
        {
            return CreateIndexed(IntPtr.Zero, 0, IntPtr.Zero, 0);
        }

        /// <summary>
        /// Creates a mesh buffer with the provided vertex array
        /// </summary>
        public static MeshBuffer Create(Array<Vertex> vertices)
        {
            var pin = GCHandle.Alloc(vertices.Items, GCHandleType.Pinned);
            var mb = Create(pin.AddrOfPinnedObject(), vertices.Count);
            pin.Free();
            return mb;
        }

        /// <summary>
        /// Creates a mesh buffer with the provided vertex and index array
        /// </summary>
        public static MeshBuffer CreateIndexed(Array<Vertex> vertices, Array<ushort> indices)
        {
            var pin = GCHandle.Alloc(vertices.Items, GCHandleType.Pinned);
            var pin2 = GCHandle.Alloc(indices.Items, GCHandleType.Pinned);
            var mb = CreateIndexed(pin.AddrOfPinnedObject(), vertices.Count, pin2.AddrOfPinnedObject(), indices.Count);
            pin.Free();
            pin2.Free();
            return mb;
        }

        /// <summary>
        /// Creates a mesh buffer with the provided pointer to the vertex data
        /// </summary>
        public static MeshBuffer Create(IntPtr vtx_data, int vtx_count)
        {
            return App.Graphics.CreateMeshBuffer(vtx_data, vtx_count);
        }

        /// <summary>
        /// Creates a mesh buffer with the provided pointers to vertex and index data
        /// </summary>
        public static MeshBuffer CreateIndexed(IntPtr vtx_data, int vtx_count, IntPtr idx_data, int idx_count)
        {
            return App.Graphics.CreateMeshBuffer(vtx_data, vtx_count, idx_data, idx_count);
        }

        /// <summary>
        /// Updates vertex buffer with the provided vertex array
        /// </summary>
        public void UpdateVertices(Array<Vertex> vertices)
        {
            var pin = GCHandle.Alloc(vertices.Items, GCHandleType.Pinned);
            UpdateVertices(pin.AddrOfPinnedObject(), vertices.Count);
            pin.Free();
        }

        /// <summary>
        /// Updates vertex buffer with the provided pointer to vertex data
        /// </summary>
        public abstract void UpdateVertices(IntPtr data, int count);

        /// <summary>
        /// Updates index buffer with the provided index array
        /// </summary>
        public void UpdateIndices(Array<ushort> indices)
        {
            var pin = GCHandle.Alloc(indices.Items, GCHandleType.Pinned);
            UpdateVertices(pin.AddrOfPinnedObject(), indices.Count);
            pin.Free();
        }

        /// <summary>
        /// Updates index buffer with the provided pointer to index data
        /// </summary>
        public abstract void UpdateIndices(IntPtr data, int count);
    }
}