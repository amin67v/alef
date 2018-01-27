using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace Engine
{
    public abstract class Shader : Data
    {
        /// <summary>
        /// Creates shader using vertex, geometry and fragment source code
        /// </summary>
        public static Shader Create(string vert, string geom, string frag)
        {
            return App.Graphics.CreateShader(vert, geom, frag);
        }

        /// <summary>
        /// Creates shader using vertex and fragment source code
        /// </summary>
        public static Shader Create(string vert, string frag)
        {
            return App.Graphics.CreateShader(vert, null, frag);
        }

        /// <summary>
        /// Loads and cache shader from file
        /// </summary>
        public static Shader Load(string file)
        {
            string extract(string source, string section)
            {
                string sec_start = $"#begin_{section}";
                string sec_end = $"#end_{section}";

                int start = source.IndexOf(sec_start);
                if (start < 0)
                    return null;

                int end = source.IndexOf(sec_end);
                if (end < 0)
                    return null;

                int i0 = start + sec_start.Length;
                return source.Substring(i0, end - i0);
            }

            Data load_file(Stream stream)
            {
                TextReader reader = new StreamReader(stream);
                    var src = reader.ReadToEnd();
                    var vert_src = extract(src, "vert");
                    var geom_src = extract(src, "geom");
                    var frag_src = extract(src, "frag");
                    var shader = Shader.Create(vert_src, geom_src, frag_src);
                    return shader;
            }

            return DataCache.FromCacheOrFile(file, load_file) as Shader;
        }


        public abstract void SetColor(int id, Color value);

        public abstract void SetFloat(int id, float value);

        public abstract void SetInt(int id, int value);

        public abstract void SetMatrix4x4(int id, Matrix4x4 value);

        public abstract void SetMatrix3x2(int id, Matrix3x2 value);

        public abstract void SetTexture(int id, int index, Texture value);

        public abstract void SetVector2(int id, Vector2 value);

        public abstract void SetVector3(int id, Vector3 value);

        public abstract void SetVector4(int id, Vector4 value);

        public void SetColor(string name, Color value) => SetColor(GetUniformID(name), value);

        public void SetFloat(string name, float value) => SetFloat(GetUniformID(name), value);

        public void SetInt(string name, int value) => SetInt(GetUniformID(name), value);

        public void SetMatrix4x4(string name, Matrix4x4 value) => SetMatrix4x4(GetUniformID(name), value);

        public void SetMatrix3x2(string name, Matrix3x2 value) => SetMatrix3x2(GetUniformID(name), value);

        public void SetTexture(string name, int index, Texture value) => SetTexture(GetUniformID(name), index, value);

        public void SetVector2(string name, Vector2 value) => SetVector2(GetUniformID(name), value);

        public void SetVector3(string name, Vector3 value) => SetVector3(GetUniformID(name), value);

        public void SetVector4(string name, Vector4 value) => SetVector4(GetUniformID(name), value);

        public abstract int GetUniformID(string name);
    }
}