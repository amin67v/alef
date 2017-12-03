using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using IdMap = System.Collections.Generic.Dictionary<string, int>;

using static OpenGL;

namespace Engine
{
    public class Shader : Resource
    {
        const string assert_msg = "You can not set shader's uniform before making it the active shader.";
        static Shader active = null;
        IdMap idMap = new IdMap();
        uint obj_id = 0;

        Shader() { }

        /// <summary>
        /// Gets or sets the active shader
        /// </summary>
        public static Shader Active
        {
            get => active;
            set
            {
                if (active == value)
                    return;

                active = value;
                glUseProgram(value == null ? 0 : value.obj_id);
            }
        }

        /// <summary>
        /// Creates shader using vertex and fragment source code
        /// </summary>
        public static Shader Create(string vert, string frag)
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
                Log.Error(msg);
                throw new Exception(msg);
            }

            // Compile fragment shader
            glShaderSource(fragID, frag);
            glCompileShader(fragID);

            if (glGetShaderiv(fragID, ShaderParameter.CompileStatus) == 0)
            {
                var msg = glGetShaderInfoLog(fragID);
                Log.Error(msg);
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
                    Log.Error(msg);
                    throw new Exception(msg);
                }
            }
            finally
            {
                // Delete shader objects
                glDeleteShader(vertID);
                glDeleteShader(fragID);
            }

            var r = new Shader();
            Shader.Active = r;
            r.obj_id = program;
            glUseProgram(Active.obj_id);
            return r;
        }

        /// <summary>
        /// Loads and cache shader from file
        /// </summary>
        public static Shader Load(string file)
        {
            string extract(string source, string section)
            {
                string secStart = $"#{section}";
                string secEnd = $"#end_{section}";

                int start = source.IndexOf(secStart);
                if (start < 0)
                    return null;

                int end = source.IndexOf(secEnd);
                if (end < 0)
                    return null;

                int i0 = start + secStart.Length;
                return source.Substring(i0, end - i0);
            }

            Resource load_file(string abs_path)
            {
                var src = System.IO.File.ReadAllText(abs_path);
                var vert_src = extract(src, "vert");
                var frag_src = extract(src, "frag");
                var shader = Shader.Create(vert_src, frag_src);
                return shader;
            }
            return FromCacheOrFile(file, load_file) as Shader;
        }


        public void SetColor(int id, Color value)
        {
            Assert.IsTrue(active == this, assert_msg);
            SetVector4(id, value.ToVector4());
        }

        public void SetFloat(int id, float value)
        {
            Assert.IsTrue(active == this, assert_msg);
            glUniform1f(id, value);
        }

        public void SetInt(int id, int value)
        {
            Assert.IsTrue(active == this, assert_msg);
            glUniform1i(id, value);
        }

        public unsafe void SetMatrix4x4(int id, Matrix4x4 value)
        {
            Assert.IsTrue(active == this, assert_msg);
            Matrix4x4* ptr = &value;
            float* f = (float*)ptr;
            glUniformMatrix4fv(id, 1, false, f);
        }

        public unsafe void SetMatrix3x2(int id, Matrix3x2 value)
        {
            Assert.IsTrue(active == this, assert_msg);
            Matrix3x2* ptr = &value;
            float* f = (float*)ptr;
            glUniformMatrix3x2fv(id, 1, false, f);
        }

        public void SetTexture(int id, int index, Texture value)
        {
            Assert.IsTrue(active == this, assert_msg);
            glActiveTexture(TextureUnit.Texture0 + index);
            glBindTexture(TextureTarget.Texture2D, value.id);
            SetInt(id, index);
        }

        public void SetVector2(int id, Vector2 value)
        {
            Assert.IsTrue(active == this, assert_msg);
            glUniform2f(id, value.X, value.Y);
        }

        public void SetVector3(int id, Vector3 value)
        {
            Assert.IsTrue(active == this, assert_msg);
            glUniform3f(id, value.X, value.Y, value.Z);
        }

        public void SetVector4(int id, Vector4 value)
        {
            Assert.IsTrue(active == this, assert_msg);
            glUniform4f(id, value.X, value.Y, value.Z, value.W);
        }

        public void SetColor(string name, Color value) => SetColor(GetUniformID(name), value);

        public void SetFloat(string name, float value) => SetFloat(GetUniformID(name), value);

        public void SetInt(string name, int value) => SetInt(GetUniformID(name), value);

        public void SetMatrix4x4(string name, Matrix4x4 value) => SetMatrix4x4(GetUniformID(name), value);

        public void SetMatrix3x2(string name, Matrix3x2 value) => SetMatrix3x2(GetUniformID(name), value);

        public void SetTexture(string name, int index, Texture value) => SetTexture(GetUniformID(name), index, value);

        public void SetVector2(string name, Vector2 value) => SetVector2(GetUniformID(name), value);

        public void SetVector3(string name, Vector3 value) => SetVector3(GetUniformID(name), value);

        public void SetVector4(string name, Vector4 value) => SetVector4(GetUniformID(name), value);

        public int GetUniformID(string name)
        {
            int id;
            if (!idMap.TryGetValue(name, out id))
            {
                id = glGetUniformLocation(obj_id, name);
                if (id == -1)
                {
                    string msg = $"Shader does not contain uniform with name '{name}'";
                    Log.Error(msg);
                    throw new Exception(msg);
                }
                else
                {
                    idMap[name] = id;
                }
            }

            return id;
        }

        protected override void OnDisposeUnmanaged()
        {
            base.OnDisposeUnmanaged();
            glDeleteProgram(obj_id);
            obj_id = 0;
        }
    }
}