using System;

namespace Engine
{
    public interface IDrawable
    {
        /// <summary>
        /// The layer used to sort this drawable object inside draw list
        /// </summary>
        int Layer { get; }

        /// <summary>
        /// Axis aligned bounding rect
        /// </summary>
        Rect Bounds { get; }

        /// <summary>
        /// The render function used to draw object
        /// </summary>
        void Draw();
    }
}