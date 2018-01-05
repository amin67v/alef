using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;
    class OpenglMeshBuffer : MeshBuffer
    {
        uint vao;
        uint vbo;
        uint ibo;
        int vtx_count;
        int idx_count;
        int vbo_size;
        int ibo_size;

        public OpenglMeshBuffer(IntPtr vtx_data, int vtx_count) :
            this(vtx_data, vtx_count, IntPtr.Zero, 0, false)
        { }

        public OpenglMeshBuffer(IntPtr vtx_data, int vtx_count, IntPtr idx_data, int idx_count, bool has_index = true)
        {
            this.vbo_size = vtx_count * Vertex.SizeInBytes;
            this.vtx_count = vtx_count;
            this.vbo = glGenBuffer();
            glBindBuffer(BufferTarget.ArrayBuffer, this.vbo);
            glBufferData(BufferTarget.ArrayBuffer, this.vbo_size, vtx_data, BufferUsageHint.DynamicDraw);

            if (has_index)
            {
                this.ibo_size = idx_count * sizeof(ushort);
                this.idx_count = idx_count;
                this.ibo = glGenBuffer();
                glBindBuffer(BufferTarget.ElementArrayBuffer, this.ibo);
                glBufferData(BufferTarget.ElementArrayBuffer, this.ibo_size, idx_data, BufferUsageHint.DynamicDraw);
            }

            this.vao = glGenVertexArray();
            glBindVertexArray(this.vao);
            glBindBuffer(BufferTarget.ArrayBuffer, this.vbo);
            if (has_index)
                glBindBuffer(BufferTarget.ElementArrayBuffer, this.ibo);
            glVertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, new IntPtr(0));
            glVertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, new IntPtr(8));
            glVertexAttribPointer(2, 4, VertexAttribPointerType.UnsignedByte, true, Vertex.SizeInBytes, new IntPtr(16));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glEnableVertexAttribArray(2);
        }

        public override int VertexCount => vtx_count;

        public override int IndexCount => idx_count;

        public override bool HasIndexBuffer => ibo != 0;

        public override void Draw(int offset, int count, PrimitiveType type)
        {
            glBindVertexArray(vao);
            if (ibo == 0)
            {
                glDrawArrays((BeginMode)type, offset, count);
            }
            else
            {
                glBindBuffer(BufferTarget.ElementArrayBuffer, ibo);
                glDrawElements((BeginMode)type, count, DrawElementsType.UnsignedShort, new IntPtr(offset));
            }
        }

        public override void UpdateVertices(IntPtr data, int count)
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

        public override void UpdateIndices(IntPtr data, int count)
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

        protected override void OnDisposeUnmanaged()
        {
            glDeleteVertexArray(vao);
            glDeleteBuffer(vbo);
            glDeleteBuffer(ibo);
            vao = vbo = ibo = 0;
        }
    }
}