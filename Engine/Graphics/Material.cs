
namespace Engine
{
    public class Material
    {
        Texture2D albedo;
        Texture2D normal;
        Texture2D surface;

        public Color Color;
        public BlendMode BlendMode;

        public Material()
        {
            Color = Color.White;
            Albedo = Texture2D.White;
            SurfaceMap = Texture2D.Black;
            BlendMode = BlendMode.Disable;
        }

        public Texture2D Albedo
        {
            get => albedo;
            set => albedo = value ?? Texture2D.White;
        }

        public Texture2D SurfaceMap
        {
            get => surface;
            set => surface = value ?? Texture2D.Black;
        }

        public void CopyTo(Material other)
        {
            other.Color = Color;
            other.Albedo = Albedo;
            other.SurfaceMap = SurfaceMap;
            other.BlendMode = BlendMode;
        }
    }
}