using System;
using System.Runtime.InteropServices;

using static OpenGL;

namespace Engine
{
    public class MeshBuffer : Disposable
    {
        uint vao;
        uint vbo;
        uint ibo;
        int vtx_count;
        int idx_count;
        int vbo_size;
        int ibo_size;

        MeshBuffer() { }

        /// <summary>
        /// Number of vertices this mesh contains
        /// </summary>
        public int VertexCount => vtx_count;

        /// <summary>
        /// Number of indices this mesh contains
        /// </summary>
        public int IndexCount => idx_count;

        /// <summary>
        /// Does this mesh buffer use index buffer?
        /// </summary>
        public bool HasIndexBuffer => ibo != 0;

        /// <summary>
        /// Draw this mesh buffer, use this method only inside render callback
        /// </summary>
        public void Draw(PrimitiveType type)
        {
            Draw(0, ibo == 0 ? VertexCount : IndexCount, type);
        }

        /// <summary>
        /// Draw this mesh buffer, use this method only inside render callback
        /// </summary>
        public void Draw(int offset, int count, PrimitiveType type)
        {
            glBindVertexArray(vao);
            if (ibo == 0)
                glDrawArrays((BeginMode)type, offset, count);
            else
                glDrawElements((BeginMode)type, count, DrawElementsType.UnsignedShort, new IntPtr(offset));
        }

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
            var mb = new MeshBuffer();

            mb.vbo = glGenBuffer();
            glBindBuffer(BufferTarget.ArrayBuffer, mb.vbo);

            mb.vbo_size = vtx_count * Vertex.SizeInBytes;
            mb.vtx_count = vtx_count;
            glBufferData(BufferTarget.ArrayBuffer, mb.vbo_size, vtx_data, BufferUsageHint.DynamicDraw);

            mb.vao = glGenVertexArray();
            glBindVertexArray(mb.vao);
            glBindBuffer(BufferTarget.ArrayBuffer, mb.vbo);
            glVertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, new IntPtr(0));
            glVertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, new IntPtr(8));
            glVertexAttribPointer(2, 4, VertexAttribPointerType.UnsignedByte, true, Vertex.SizeInBytes, new IntPtr(16));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glEnableVertexAttribArray(2);

            return mb;
        }

        /// <summary>
        /// Creates a mesh buffer with the provided pointers to vertex and index data
        /// </summary>
        public static MeshBuffer CreateIndexed(IntPtr vtx_data, int vtx_count, IntPtr idx_data, int idx_count)
        {
            var mb = new MeshBuffer();

            mb.vbo = glGenBuffer();
            glBindBuffer(BufferTarget.ArrayBuffer, mb.vbo);

            mb.vbo_size = vtx_count * Vertex.SizeInBytes;
            mb.vtx_count = vtx_count;
            glBufferData(BufferTarget.ArrayBuffer, mb.vbo_size, vtx_data, BufferUsageHint.DynamicDraw);

            mb.ibo_size = idx_count * sizeof(ushort);
            mb.ibo = glGenBuffer();
            glBindBuffer(BufferTarget.ElementArrayBuffer, mb.ibo);
            glBufferData(BufferTarget.ElementArrayBuffer, mb.ibo_size, idx_data, BufferUsageHint.DynamicDraw);

            mb.vao = glGenVertexArray();
            glBindVertexArray(mb.vao);
            glBindBuffer(BufferTarget.ArrayBuffer, mb.vbo);
            glBindBuffer(BufferTarget.ElementArrayBuffer, mb.ibo);
            glVertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, new IntPtr(0));
            glVertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, new IntPtr(8));
            glVertexAttribPointer(2, 4, VertexAttribPointerType.UnsignedByte, true, Vertex.SizeInBytes, new IntPtr(16));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glEnableVertexAttribArray(2);

            return mb;
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
        public void UpdateVertices(IntPtr data, int count)
        {
            var size = count * Vertex.SizeInBytes;
            glBindBuffer(BufferTarget.ArrayBuffer, vbo);
            if (size > vbo_size)
            {
                vbo_size = size;
                glBufferData(BufferTarget.ArrayBuffer, size, data, BufferUsageHint.DynamicDraw);
            }
            else
            {
                glBufferSubData(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(size), data);
            }
            vtx_count = count;
        }

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
        public void UpdateIndices(IntPtr data, int count)
        {
            Assert.IsTrue(ibo != 0, "MeshBuffer does not contain index buffer.");
            var size = count * sizeof(ushort);
            glBindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            if (size > ibo_size)
            {
                ibo_size = size;
                glBufferData(BufferTarget.ElementArrayBuffer, size, data, BufferUsageHint.DynamicDraw);
            }
            else
            {
                glBufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, new IntPtr(size), data);
            }
            idx_count = count;
        }
    }
}