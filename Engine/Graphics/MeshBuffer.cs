using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Runtime.InteropServices;

using Assimp;

namespace Engine
{
    public abstract class MeshBuffer : Resource
    {
        /// <summary>
        /// Gets internal screen quad mesh buffer
        /// </summary>
        public static MeshBuffer ScreenQuad
        {
            get
            {
                MeshBuffer value = ResourceManager.Get<MeshBuffer>("MeshBuffer.ScreenQuad");
                if (value == null)
                {
                    Vertex[] vertices = new Vertex[4];
                    vertices[0].Position = new Vector3(-1, -1, 0);
                    vertices[0].Normal = new Vector3(0, 0, 0);
                    vertices[0].Texcoord = new Vector3(0, 0, 0);

                    vertices[1].Position = new Vector3(1, 1, 0);
                    vertices[1].Normal = new Vector3(0, 0, 0);
                    vertices[1].Texcoord = new Vector3(1, 1, 0);

                    vertices[2].Position = new Vector3(-1, 1, 0);
                    vertices[2].Normal = new Vector3(0, 0, 0);
                    vertices[2].Texcoord = new Vector3(0, 1, 0);

                    vertices[3].Position = new Vector3(1, -1, 0);
                    vertices[3].Normal = new Vector3(0, 0, 0);
                    vertices[3].Texcoord = new Vector3(1, 0, 0);

                    ushort[] indices = { 0, 1, 2, 0, 3, 1 };

                    value = MeshBuffer.Create(Vertex.Format, PrimitiveType.Triangles, vertices, indices);
                    ResourceManager.Add("MeshBuffer.ScreenQuad", value);
                }
                return value;
            }
        }

        /// <summary>
        /// Number of vertices this mesh buffer contains
        /// </summary>
        public abstract int VertexCount { get; }

        /// <summary>
        /// Number of indices this mesh buffer contains
        /// </summary>
        public abstract int IndexCount { get; }

        /// <summary>
        /// primitive type used to draw this mesh buffer
        /// </summary>
        public abstract PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// Vertex format used to build this mesh buffer
        /// </summary>
        public abstract VertexFormat VertexFormat { get; }

        /// <summary>
        /// Updates vertices with the given vertex data
        /// </summary>
        public abstract void UpdateVertices(IntPtr data, int count);

        /// <summary>
        /// Updates vertices with the given vertex array
        /// </summary>
        public void UpdateVertices<T>(Array<T> vertices)
        {

            var hdl = vertices.GetPinnedHandle();
            UpdateVertices(hdl.AddrOfPinnedObject(), vertices.Count);
            hdl.Free();
        }

        /// <summary>
        /// Updates indices with the given index data
        /// </summary>
        public abstract void UpdateIndices(IntPtr data, int count);

        // /// <summary>
        // /// Updates indices with the given index array
        // /// </summary>
        public void UpdateIndices(Array<ushort> indices)
        {
            var hdl = indices.GetPinnedHandle();
            UpdateIndices(hdl.AddrOfPinnedObject(), indices.Count);
            hdl.Free();
        }

        public static MeshBuffer Create<T>(VertexFormat format, PrimitiveType primitive, T[] vertices, ushort[] indices) where T : struct
        {
            var vertHdl = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            var indHdl = GCHandle.Alloc(indices, GCHandleType.Pinned);
            var buffer = Graphics.CreateMeshBuffer(format, primitive, vertHdl.AddrOfPinnedObject(), vertices.Length,
                                                                          indHdl.AddrOfPinnedObject(), indices.Length);
            vertHdl.Free();
            indHdl.Free();
            return buffer;
        }

        public static MeshBuffer Create<T>(VertexFormat format, PrimitiveType primitive, Array<T> vertices, Array<ushort> indices) where T : struct
        {
            var pinVertices = vertices.GetPinnedHandle();
            var pinIndices = indices.GetPinnedHandle();
            var buffer = Graphics.CreateMeshBuffer(format, primitive, pinVertices.AddrOfPinnedObject(), vertices.Count,
                                                                      pinIndices.AddrOfPinnedObject(), indices.Count);
            pinVertices.Free();
            pinIndices.Free();
            return buffer;
        }
    }
}