using System;
using System.Numerics;

namespace Engine
{
    public interface IDrawNode
    {
        Matrix4x4 Matrix { get; }
        Material Material { get; }
        MeshBuffer MeshBuffer { get; }
    }
}