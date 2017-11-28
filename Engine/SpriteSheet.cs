using System;
using System.Json;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteSheet
    {
        public static int PixelPerUnit = 24;

        SpriteSheet() { }

        public struct Frame
        {
            internal string Name;

            internal Vertex Vertex0;
            internal Vertex Vertex1;
            internal Vertex Vertex2;
            internal Vertex Vertex3;
            internal Vertex Vertex4;
            internal Vertex Vertex5;

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