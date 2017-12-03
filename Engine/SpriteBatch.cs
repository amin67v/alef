using System;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteBatch : Actor
    {
        SortedList<int, DrawData> draw_list = new SortedList<int, DrawData>(10);

        public static void AddForDraw(Transform xform, SpriteSheet sheet, int frame, Color color, int depth)
        {
            
        }

        public override void OnRender()
        {
            for (int i = 0; i < draw_list.Count; i++)
            {

            }
        }

        struct DrawData
        {
            public Texture Texture;
            public int Depth;
        }
    }
}