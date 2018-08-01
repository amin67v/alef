using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Engine
{
    using static OpenGL;
    class GLShader : Shader
    {
        public uint Id;

        public GLShader(string vert, string geom, string frag)
        {
            uint program;

            uint vertId = glCreateShader(ShaderType.VertexShader);
            uint fragId = glCreateShader(ShaderType.FragmentShader);
            uint geomId = 0;
            if (geom != null)
                geomId = glCreateShader(ShaderType.GeometryShader);

            // Compile vertex shader
            glShaderSource(vertId, vert);
            glCompileShader(vertId);

            if (glGetShaderiv(vertId, ShaderParameter.CompileStatus) == 0)
            {
                var msg = glGetShaderInfoLog(vertId);
                Log.Error(msg);
                throw new Exception(msg);
            }

            // Compile geometry shader if available
            if (geom != null)
            {
                glShaderSource(geomId, geom);
                glCompileShader(geomId);

                if (glGetShaderiv(geomId, ShaderParameter.CompileStatus) == 0)
                {
                    var msg = glGetShaderInfoLog(geomId);
                    Log.Error(msg);
                    throw new Exception(msg);
                }
            }

            // Compile fragment shader
            glShaderSource(fragId, frag);
            glCompileShader(fragId);

            if (glGetShaderiv(fragId, ShaderParameter.CompileStatus) == 0)
            {
                var msg = glGetShaderInfoLog(fragId);
                Log.Error(msg);
                throw new Exception(msg);
            }

            // Create, attach and link shaderProgram
            try
            {
                program = glCreateProgram();
                glAttachShader(program, vertId);
                glAttachShader(program, fragId);
                if (geom != null)
                    glAttachShader(program, geomId);

                glLinkProgram(program);

                if (glGetProgramiv(program, ProgramParameter.LinkStatus) == 0)
                {
                    var msg = glGetProgramInfoLog(program);
                    glDeleteProgram(program);
                    Log.Error(msg);
                    throw new Exception(msg);
                }

            }
            finally
            {
                // Delete shader objects
                glDeleteShader(vertId);
                glDeleteShader(fragId);
                glDeleteShader(geomId);
            }

            Id = program;
        }

        public override void SetUniformBlock(string name, uint index)
        {
            var blockIndex = glGetUniformBlockIndex(Id, name);
            if (blockIndex != 0xFFFFFFFFu)
                glUniformBlockBinding(Id, blockIndex, index);
        }

        public override void SetUniform(int id, Color value)
        {
            SetUniform(id, value.ToVector4());
        }

        public override void SetUniform(int id, float value)
        {
            glUniform1f(id, value);
        }

        public override void SetUniform(int id, int value)
        {
            glUniform1i(id, value);
        }

        public override unsafe void SetUniform(int id, ref Matrix4x4 value)
        {
            fixed (Matrix4x4* ptr = &value)
            {
                float* f = (float*)ptr;
                glUniformMatrix4fv(id, 1, false, f);
            }
        }

        public override unsafe void SetUniform(int id, ref Matrix3x2 value)
        {
            fixed (Matrix3x2* ptr = &value)
            {
                float* f = (float*)ptr;
                glUniformMatrix3x2fv(id, 1, false, f);
            }
        }

        public override void SetUniform(int id, int index, Texture2D value)
        {
            glActiveTexture(TextureUnit.Texture0 + index);
            glBindTexture(TextureTarget.Texture2D, (value as GLTexture2D).Id);
            SetUniform(id, index);
        }

        public override void SetUniform(int id, int index, CubeMap value)
        {
            glActiveTexture(TextureUnit.Texture0 + index);
            glBindTexture(TextureTarget.TextureCubeMap, (value as GLCubeMap).Id);
            SetUniform(id, index);
        }

        public override void SetUniform(int id, Vector2 value)
        {
            glUniform2f(id, value.X, value.Y);
        }

        public override void SetUniform(int id, Vector3 value)
        {
            glUniform3f(id, value.X, value.Y, value.Z);
        }

        public override void SetUniform(int id, Vector4 value)
        {
            glUniform4f(id, value.X, value.Y, value.Z, value.W);
        }

        public unsafe override void SetUniform(int id, IReadOnlyArray<int> value)
        {
            var hdl = value.GetPinnedHandle();
            glUniform1iv(id, value.Count, (int*)hdl.AddrOfPinnedObject());
            hdl.Free();
        }

        public unsafe override void SetUniform(int id, IReadOnlyArray<float> value)
        {
            var hdl = value.GetPinnedHandle();
            glUniform1fv(id, value.Count, (float*)hdl.AddrOfPinnedObject());
            hdl.Free();
        }

        public unsafe override void SetUniform(int id, IReadOnlyArray<Vector2> value)
        {
            var hdl = value.GetPinnedHandle();
            glUniform2fv(id, value.Count, (float*)hdl.AddrOfPinnedObject());
            hdl.Free();
        }

        public unsafe override void SetUniform(int id, IReadOnlyArray<Vector3> value)
        {
            var hdl = value.GetPinnedHandle();
            glUniform3fv(id, value.Count, (float*)hdl.AddrOfPinnedObject());
            hdl.Free();
        }

        public unsafe override void SetUniform(int id, IReadOnlyArray<Vector4> value)
        {
            var hdl = value.GetPinnedHandle();
            glUniform4fv(id, value.Count, (float*)hdl.AddrOfPinnedObject());
            hdl.Free();
        }

        public unsafe override void SetUniform(int id, IReadOnlyArray<Matrix3x2> value)
        {
            var hdl = value.GetPinnedHandle();
            glUniformMatrix3x2fv(id, value.Count, false, (float*)hdl.AddrOfPinnedObject());
            hdl.Free();
        }

        public unsafe override void SetUniform(int id, IReadOnlyArray<Matrix4x4> value)
        {
            var hdl = value.GetPinnedHandle();
            glUniformMatrix4fv(id, value.Count, false, (float*)hdl.AddrOfPinnedObject());
            hdl.Free();
        }

        public override int GetUniformID(string name)
        {
            var id = glGetUniformLocation(Id, name);
            if (id == -1)
            {
                string msg = $"Shader '{this.Name}' does not contain uniform with name '{name}'";
                Log.Error(msg);
                throw new Exception(msg);
            }
            return id;
        }

        protected override void OnDestroy()
        {
            glDeleteProgram(Id);
            Id = 0;
        }
    }
}