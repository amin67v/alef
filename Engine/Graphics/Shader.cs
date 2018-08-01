using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Engine
{
    public abstract class Shader : Resource
    {
        /// <summary>
        /// Callback used to set uniforms when this shader is set to be used
        /// </summary>
        public Action<object> OnSetUniforms;

        public abstract void SetUniformBlock(string name, uint index);

        public abstract void SetUniform(int id, Color value);

        public abstract void SetUniform(int id, float value);

        public abstract void SetUniform(int id, int value);

        public abstract void SetUniform(int id, ref Matrix4x4 value);

        public abstract void SetUniform(int id, ref Matrix3x2 value);

        public abstract void SetUniform(int id, int index, Texture2D value);

        public abstract void SetUniform(int id, int index, CubeMap value);

        public abstract void SetUniform(int id, Vector2 value);

        public abstract void SetUniform(int id, Vector3 value);

        public abstract void SetUniform(int id, Vector4 value);

        public abstract void SetUniform(int id, IReadOnlyArray<int> value);

        public abstract void SetUniform(int id, IReadOnlyArray<float> value);

        public abstract void SetUniform(int id, IReadOnlyArray<Vector2> value);

        public abstract void SetUniform(int id, IReadOnlyArray<Vector3> value);

        public abstract void SetUniform(int id, IReadOnlyArray<Vector4> value);

        public abstract void SetUniform(int id, IReadOnlyArray<Matrix3x2> value);

        public abstract void SetUniform(int id, IReadOnlyArray<Matrix4x4> value);

        public abstract int GetUniformID(string name);
    }
}