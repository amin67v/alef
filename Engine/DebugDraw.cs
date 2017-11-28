using System;
using System.Numerics;

namespace Engine
{
    /*
    public static class DebugDraw
    {
        static Shader debug_color;
        static Shader debug_tex;
        static Array<Vertex> verts;
        static MeshBuffer mb;

        static Vector2[] circle;

        static DebugDraw()
        {
            mb = MeshBuffer.Create();

            debug_tex = ResourceManager.LoadShader("internal/DebugTex.glsl");
            debug_color = ResourceManager.LoadShader("internal/DebugColor.glsl");

            verts = new Array<Vertex>(50);

            var pointCnt = 32;
            circle = new Vector2[pointCnt];
            for (int i = 0; i < pointCnt; i++)
            {
                float angle = i;
                angle = angle.Remap(0, 32, 0, MathF.PI * 2);
                circle[i] = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            }
        }

        public static void Point(Vector2 pos, float size, Color c)
        {
            Graphics.PointSize = size;
            Graphics.BlendMode = BlendMode.Disabled;
            Shader.Active = debug_color;
            debug_color.SetUniform("view", Graphics.ViewMatrix);

            verts.Clear();
            verts.Push(new Vertex(pos, Vector2.Zero, c));
            mb.UpdateVertices(verts);

            mb.Draw(PrimitiveType.Points);
        }


        public static void Point(Array<Vector2> points, float size, Color c)
        {
            Graphics.PointSize = size;
            Graphics.BlendMode = BlendMode.Disabled;
            Shader.Active = debug_color;
            debug_color.SetUniform("view", Graphics.ViewMatrix);

            verts.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                verts.Push(new Vertex(points[i], Vector2.Zero, c));
            }
            mb.UpdateVertices(verts);

            mb.Draw(PrimitiveType.Points);
        }

        public static void Circle(Vector2 pos, float radius, float width, Color c)
        {
            Graphics.SetLineWidth(width);
            Graphics.SetBlendMode(BlendMode.Disabled);
            debug_color.Apply();
            debug_color.SetUniform("view", Graphics.ViewMatrix);

            verts.Clear();
            for (int i = 0; i < circle.Length; i++)
            {
                verts.Push(new Vertex(circle[i] * radius + pos, Vector2.Zero, c));
            }
            mb.UpdateVertices(verts);

            Graphics.Draw(mb, PrimitiveType.LineLoop);
        }

        public static void Line(Vector2 start, Vector2 end, float width, Color c)
        {
            Graphics.SetLineWidth(width);
            Graphics.SetBlendMode(BlendMode.Disabled);
            debug_color.Apply();
            debug_color.SetUniform("view", Graphics.ViewMatrix);

            verts.Clear();
            verts.Push(new Vertex(start, Vector2.Zero, c));
            verts.Push(new Vertex(end, Vector2.Zero, c));
            mb.UpdateVertices(verts);

            Graphics.Draw(mb, PrimitiveType.Lines);
        }

        public static void Line(Array<Vector2> points, float width, bool loop, Color c)
        {
            Graphics.SetLineWidth(width);
            Graphics.SetBlendMode(BlendMode.Disabled);
            debug_color.Apply();
            debug_color.SetUniform("view", Graphics.ViewMatrix);

            verts.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                verts.Push(new Vertex(points[i], Vector2.Zero, c));
            }
            mb.UpdateVertices(verts);

            Graphics.Draw(mb, loop ? PrimitiveType.LineLoop : PrimitiveType.LineStrip);
        }

        public static void FillRect(Rect rect, Color c)
        {
            Graphics.SetBlendMode(BlendMode.Disabled);
            debug_color.Apply();
            debug_color.SetUniform("view", Graphics.ViewMatrix);

            verts.Clear();
            verts.Push(new Vertex(rect.XMinYMax, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMinYMin, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMaxYMax, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMaxYMin, Vector2.Zero, c));
            mb.UpdateVertices(verts);

            Graphics.Draw(mb, PrimitiveType.TriangleStrip);
        }

        public static void Rect(Rect rect, float width, Color c)
        {
            Graphics.SetLineWidth(width);
            Graphics.SetBlendMode(BlendMode.Disabled);
            debug_color.Apply();
            debug_color.SetUniform("view", Graphics.ViewMatrix);

            verts.Clear();
            verts.Push(new Vertex(rect.XMinYMax, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMinYMin, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMaxYMax, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMaxYMin, Vector2.Zero, c));
            mb.UpdateVertices(verts);

            Graphics.Draw(mb, PrimitiveType.LineLoop);
        }

        public static void Texture(Rect rect, Rect src, Texture tex)
        {
            Graphics.SetBlendMode(BlendMode.AlphaBlend);
            debug_tex.Apply();
            debug_tex.SetUniform("view", Graphics.ViewMatrix);
            debug_tex.SetUniform("tex", 0, tex);

            verts.Clear();
            verts.Push(new Vertex(rect.XMinYMax, src.XMinYMax, Color.White));
            verts.Push(new Vertex(rect.XMinYMin, src.XMinYMin, Color.White));
            verts.Push(new Vertex(rect.XMaxYMax, src.XMaxYMax, Color.White));
            verts.Push(new Vertex(rect.XMaxYMin, src.XMaxYMin, Color.White));
            mb.UpdateVertices(verts);

            Graphics.Draw(mb, PrimitiveType.TriangleStrip);
        }


    }
    */
}