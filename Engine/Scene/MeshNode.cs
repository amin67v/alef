namespace Engine
{
    public class MeshNode : Node, IDrawNode
    {
        MeshBuffer meshBuffer;
        Material material = new Material();

        public Material Material
        {
            get => material;
            set => value.CopyTo(material);
        }

        public MeshBuffer MeshBuffer
        {
            get => meshBuffer;
            set => meshBuffer = value;
        }
    }
}