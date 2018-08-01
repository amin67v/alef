using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    using static OpenGL;

    class GLMeshBuffer : MeshBuffer
    {
        public uint Vao, Vbo, Ibo;
        int vertexCount, indexCount;
        PrimitiveType primType;
        VertexFormat format;

        public override int VertexCount => vertexCount;

        public override int IndexCount => indexCount;

        public override PrimitiveType PrimitiveType => primType;

        public override VertexFormat VertexFormat => format;

        public GLMeshBuffer(VertexFormat format, PrimitiveType primitive, IntPtr vertexData, int vertexCount, IntPtr indexData, int indexCount)
        {
            this.format = format;
            this.vertexCount = vertexCount;
            this.primType = primitive;

            this.Vbo = glGenBuffer();
            glBindBuffer(BufferTarget.ArrayBuffer, this.Vbo);
            glBufferData(BufferTarget.ArrayBuffer, this.vertexCount * format.VertexSize, vertexData, BufferUsageHint.DynamicDraw);

            this.indexCount = indexCount;
            this.Ibo = glGenBuffer();
            glBindBuffer(BufferTarget.ElementArrayBuffer, this.Ibo);
            glBufferData(BufferTarget.ElementArrayBuffer, this.indexCount * sizeof(ushort), indexData, BufferUsageHint.DynamicDraw);

            this.Vao = glGenVertexArray();
            glBindVertexArray(this.Vao);
            glBindBuffer(BufferTarget.ArrayBuffer, this.Vbo);
            glBindBuffer(BufferTarget.ElementArrayBuffer, this.Ibo);

            IntPtr offset = IntPtr.Zero;
            for (uint i = 0; i < format.AttribCount; i++)
            {
                glEnableVertexAttribArray(i);
                var attrib = format[(int)i];
                switch (attrib)
                {
                    case VertexAttrib.Float:
                        glVertexAttribPointer(i, 1, VertexAttribPointerType.Float, false, format.VertexSize, offset);
                        offset += sizeof(float);
                        break;
                    case VertexAttrib.Vector2:
                        glVertexAttribPointer(i, 2, VertexAttribPointerType.Float, false, format.VertexSize, offset);
                        offset += sizeof(float) * 2;
                        break;
                    case VertexAttrib.Vector3:
                        glVertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, format.VertexSize, offset);
                        offset += sizeof(float) * 3;
                        break;
                    case VertexAttrib.Vector4:
                        glVertexAttribPointer(i, 4, VertexAttribPointerType.Float, false, format.VertexSize, offset);
                        offset += sizeof(float) * 4;
                        break;
                    case VertexAttrib.Color:
                        glVertexAttribPointer(i, 4, VertexAttribPointerType.UnsignedByte, true, format.VertexSize, offset);
                        offset += sizeof(int);
                        break;
                }
            }
        }

        public override void UpdateVertices(IntPtr data, int count)
        {
            var oldSize = format.VertexSize * vertexCount;
            var size = format.VertexSize * count;
            glBindBuffer(BufferTarget.ArrayBuffer, Vbo);
            if (size > oldSize)
                glBufferData(BufferTarget.ArrayBuffer, size, data, BufferUsageHint.DynamicDraw);
            else
                glBufferSubData(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(size), data);

            vertexCount = count;
        }

        public override void UpdateIndices(IntPtr data, int count)
        {
            var oldSize = indexCount * sizeof(ushort);
            var size = count * sizeof(ushort);
            glBindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
            if (size > oldSize)
                glBufferData(BufferTarget.ElementArrayBuffer, size, data, BufferUsageHint.DynamicDraw);
            else
                glBufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, new IntPtr(size), data);

            indexCount = count;
        }

        protected override void OnDestroy()
        {
            glDeleteVertexArray(Vao);
            glDeleteBuffer(Vbo);
            glDeleteBuffer(Ibo);
            Vao = Vbo = Ibo = 0;
        }
    }
}