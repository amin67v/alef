using System;
using System.Json;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteSheet
    {
        public static int PixelPerUnit = 24;
        FrameData[] frames;

        SpriteSheet() { }

        public class FrameData
        {
            string name;
            Vertex[] verts;

            //public Frame(Rect rect, Vector2 origin)
            //{
//
            //}

            //public fixed Vertex Vertices[10];

            // public Vertex[] Vertices = new Vertex[6];
            // 
            // public Frame(Texture tex, Rect rect, Vector2 origin)
            // {
            //     Texture = tex;
            // 
            //     var texcoord = rect / tex.Size;
            // 
            //     rect /= PixelPerUnit;
            //     origin /= PixelPerUnit;
            //     rect.Position -= origin;
            // 
            //     Vertices[0] = new Vertex(rect.XMinYMin, texcoord.XMinYMin, Color.Black);
            //     Vertices[1] = new Vertex(rect.XMaxYMax, texcoord.XMaxYMax, Color.Black);
            //     Vertices[2] = new Vertex(rect.XMinYMax, texcoord.XMinYMax, Color.Black);
            //     Vertices[3] = Vertices[0];
            //     Vertices[4] = new Vertex(rect.XMaxYMin, texcoord.XMaxYMin, Color.Black);
            //     Vertices[5] = Vertices[1];
            // 
            // }
        }
    }
}