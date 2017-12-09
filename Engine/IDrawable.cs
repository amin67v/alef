namespace Engine
{
    public interface IDrawable
    {
        int SortKey { get; }
        bool IsVisible();
        void Draw();
    }
}