using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Runtime.InteropServices;

using Assimp;

namespace Engine
{
    using System.Numerics;
    public class Model : Resource
    {
        Array<Entry> entries = new Array<Entry>();

        public int EntryCount => entries.Count;

        public Entry this[int index] => entries[index];

        public Entry this[string name]
        {
            get
            {
                var index = entries.FindIndex((item) => item.Name == name);
                if (index == -1)
                    return null;
                else
                    return entries[index];
            }
        }

        /// <summary>
        /// Saves model into binary format
        /// </summary>
        public void Save(string file)
        {

        }

        // <summary>
        /// Loads and cache model from file
        /// </summary>
        public static Model Load(string file)
        {
            Resource load_file(Stream stream)
            {
                var path = Path.GetDirectoryName(file);
                var swapYZ = Path.GetExtension(file).ToLower() == ".dae";
                var model = new Model();
                var context = new AssimpContext();
                var scene = context.ImportFileFromStream(stream, PostProcessPreset.TargetRealTimeFast);

                // import meshes
                if (scene.HasMeshes)
                {
                    for (int i = 0; i < scene.MeshCount; i++)
                    {
                        Assimp.Mesh aiMesh = scene.Meshes[i];
                        Assimp.Material aiMat = null;

                        var matIndex = scene.Meshes[i].MaterialIndex;
                        if ((uint)matIndex < scene.MaterialCount)
                            aiMat = scene.Materials[matIndex];

                        model.entries.Push(new Entry(aiMesh, aiMat, path, swapYZ));
                    }
                }
                else
                {
                    var msg = $"Model '{file}' does not contain any meshes.";
                    Log.Error(msg);
                    throw new Exception(msg);
                }

                context.Dispose();
                return model;
            }

            return ResourceManager.Load(file, load_file) as Model;
        }

        protected override void OnDestroy()
        {
            for (int i = 0; i < entries.Count; i++)
                Destroy(entries[i].MeshBuffer);

            entries.Clear();
        }

        static Vector3 convertVec3(Vector3D value) => new Vector3(value.X, value.Y, value.Z);

        static Vector3 convertVec3SwapYZ(Vector3D value) => new Vector3(value.X, value.Z, -value.Y);

        static Vector2 convertVec2(Vector2D value) => new Vector2(value.X, value.Y);

        static Color convertColor(Color4D value) => (new Color(value.R, value.G, value.B, value.A));

        static Color convertColor(Color3D value) => new Color(value.R, value.G, value.B, 1f);

        public class Entry
        {
            string name;
            MeshBuffer buffer;
            Material material;
            Array<Vertex> vertices;
            Array<ushort> indices;

            internal Entry(Assimp.Mesh aiMesh, Assimp.Material aiMat, string modelPath, bool swapYZ)
            {
                name = aiMesh.Name;
                vertices = new Array<Vertex>(aiMesh.VertexCount);
                indices = new Array<ushort>(aiMesh.GetUShortIndices());

                if (swapYZ)
                {
                    for (int j = 0; j < aiMesh.VertexCount; j++)
                    {
                        var vertex = new Vertex();
                        vertex.Position = convertVec3SwapYZ(aiMesh.Vertices[j]);
                        vertex.Normal = convertVec3SwapYZ(aiMesh.Normals[j]);
                        vertex.Tangent = convertVec3SwapYZ(aiMesh.Tangents[j]);
                        vertex.Texcoord = convertVec3(aiMesh.TextureCoordinateChannels[0][j]);
                        vertex.Texcoord.Z = 0;

                        vertices.Push(vertex);
                    }
                }
                else
                {
                    for (int j = 0; j < aiMesh.VertexCount; j++)
                    {
                        var vertex = new Vertex();
                        vertex.Position = convertVec3(aiMesh.Vertices[j]);
                        vertex.Normal = convertVec3(aiMesh.Normals[j]);
                        vertex.Tangent = convertVec3SwapYZ(aiMesh.Tangents[j]);
                        vertex.Texcoord = convertVec3(aiMesh.TextureCoordinateChannels[0][j]);
                        vertex.Texcoord.Z = 0;

                        vertices.Push(vertex);
                    }
                }

                PrimitiveType primitve;
                if (aiMesh.PrimitiveType == Assimp.PrimitiveType.Point)
                    primitve = PrimitiveType.Points;
                else if (aiMesh.PrimitiveType == Assimp.PrimitiveType.Line)
                    primitve = PrimitiveType.Lines;
                else
                    primitve = PrimitiveType.Triangles;

                buffer = MeshBuffer.Create<Vertex>(Vertex.Format, primitve, vertices, indices);
                material = new Material();

                if (aiMat != null)
                {
                    if (aiMat.HasTextureDiffuse)
                        material.Albedo = Texture2D.Load(Path.Combine(modelPath, aiMat.TextureDiffuse.FilePath));

                    if (aiMat.HasBlendMode)
                    {
                        if (aiMat.BlendMode == Assimp.BlendMode.Default)
                            material.BlendMode = BlendMode.AlphaBlend;
                        else if (aiMat.BlendMode == Assimp.BlendMode.Additive)
                            material.BlendMode = BlendMode.Additive;
                    }
                    else
                    {
                        material.BlendMode = BlendMode.Disable;
                    }

                    material.Color = convertColor(aiMat.ColorDiffuse);
                }
            }

            public string Name => name;

            public MeshBuffer MeshBuffer => buffer;

            public Material Material => material;

            public IReadOnlyArray<Vertex> Vertices => vertices;

            public IReadOnlyArray<ushort> Indices => indices;
        }
    }



}