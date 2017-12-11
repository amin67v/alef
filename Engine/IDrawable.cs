namespace Engine
{
    public interface IDrawable
    {
        /// <summary>
        /// The key used to sort this drawable object inside draw list
        /// </summary>
        int SortKey { get; }

        /// <summary>
        /// The render function used to draw object
        /// </summary>
        void Draw();
    }
}