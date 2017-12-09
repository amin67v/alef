using System;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteBatch : Entity
    {
        SortedList<int, DrawData> draw_list = new SortedList<int, DrawData>(10);

        public static void AddForDraw(Transform xform, SpriteSheet sheet, int frame, Color color, int depth)
        {
            
        }

        struct DrawData
        {
            public Texture Texture;
            public int Depth;
        }
    }
}