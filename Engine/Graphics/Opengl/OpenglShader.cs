using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using static OpenGL;

namespace Engine
{
    class OpenglShader : Shader
    {
        const string assert_msg = "You can not set shader's uniform before making it the current shader.";
        internal static OpenglShader current = null;

        internal uint id;
        Dictionary<string, int> uniforms = new Dictionary<string, int>();

        public OpenglShader(string vert, string frag) 
        { 
            
            uint program;

            uint vertID = glCreateShader(ShaderType.VertexShader);
            uint fragID = glCreateShader(ShaderType.FragmentShader);

            // Compile vertex shader
            glShaderSource(vertID, vert);
            glCompileShader(vertID);

            if (glGetShaderiv(vertID, ShaderParameter.CompileStatus) == 0)
            {
                var msg = glGetShaderInfoLog(vertID);
                App.Log.Error(msg);
                throw new Exception(msg);
            }

            // Compile fragment shader
            glShaderSource(fragID, frag);
            glCompileShader(fragID);

            if (glGetShaderiv(fragID, ShaderParameter.CompileStatus) == 0)
            {
                var msg = glGetShaderInfoLog(fragID);
                App.Log.Error(msg);
                throw new Exception(msg);
            }

            // Create, attach and link shaderProgram
            try
            {
                program = glCreateProgram();
                glAttachShader(program, vertID);
                glAttachShader(program, fragID);
                glLinkProgram(program);

                if (glGetProgramiv(program, ProgramParameter.LinkStatus) == 0)
                {
                    var msg = glGetProgramInfoLog(program);
                    glDeleteProgram(program);
                    App.Log.Error(msg);
                    throw new Exception(msg);
                }
            }
            finally
            {
                // Delete shader objects
                glDeleteShader(vertID);
                glDeleteShader(fragID);
            }

            this.id = program;
        }


        public override void SetColor(int id, Color value)
        {
            Assert.IsTrue(current == this, assert_msg);
            SetVector4(id, value.ToVector4());
        }

        public override void SetFloat(int id, float value)
        {
            Assert.IsTrue(current == this, assert_msg);
            glUniform1f(id, value);
        }

        public override void SetInt(int id, int value)
        {
            Assert.IsTrue(current == this, assert_msg);
            glUniform1i(id, value);
        }

        public override unsafe void SetMatrix4x4(int id, Matrix4x4 value)
        {
            Assert.IsTrue(current == this, assert_msg);
            Matrix4x4* ptr = &value;
            float* f = (float*)ptr;
            glUniformMatrix4fv(id, 1, false, f);
        }

        public override unsafe void SetMatrix3x2(int id, Matrix3x2 value)
        {
            Assert.IsTrue(current == this, assert_msg);
            Matrix3x2* ptr = &value;
            float* f = (float*)ptr;
            glUniformMatrix3x2fv(id, 1, false, f);
        }

        public override void SetTexture(int id, int index, Texture value)
        {
            Assert.IsTrue(current == this, assert_msg);
            glActiveTexture(TextureUnit.Texture0 + index);
            glBindTexture(TextureTarget.Texture2D, (value as OpenglTexture).id);
            SetInt(id, index);
        }

        public override void SetVector2(int id, Vector2 value)
        {
            Assert.IsTrue(current == this, assert_msg);
            glUniform2f(id, value.X, value.Y);
        }

        public override void SetVector3(int id, Vector3 value)
        {
            Assert.IsTrue(current == this, assert_msg);
            glUniform3f(id, value.X, value.Y, value.Z);
        }

        public override void SetVector4(int id, Vector4 value)
        {
            Assert.IsTrue(current == this, assert_msg);
            glUniform4f(id, value.X, value.Y, value.Z, value.W);
        }

        public override int GetUniformID(string name)
        {
            int uniform_id;
            if (!uniforms.TryGetValue(name, out uniform_id))
            {
                uniform_id = glGetUniformLocation(id, name);
                if (uniform_id == -1)
                {
                    string msg = $"Shader does not contain uniform with name '{name}'";
                    App.Log.Error(msg);
                    throw new Exception(msg);
                }
                else
                {
                    uniforms[name] = uniform_id;
                }
            }

            return uniform_id;
        }

        protected override void OnDisposeUnmanaged()
        {
            base.OnDisposeUnmanaged();
            glDeleteProgram(this.id);
            this.id = 0;
        }
    }
}