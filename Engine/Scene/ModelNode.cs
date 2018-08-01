namespace Engine
{
    public class ModelNode : Node
    {
        Model model;

        public Model Model
        {
            get => model;
            set
            {
                if (model == value)
                    return;

                for (int i = ChildCount - 1; i >= 0; --i)
                    Destroy(this[i]);

                model = value;
                for (int i = 0; i < model.EntryCount; i++)
                {
                    var mesh = AddChild<MeshNode>();
                    mesh.Name = model[i].Name;
                    mesh.MeshBuffer = model[i].MeshBuffer;
                    mesh.Material = model[i].Material;
                }
            }
        }
    }
}