using System;
using System.Numerics;
using System.Diagnostics;
using static System.MathF;

namespace Engine
{
    public class DebugDraw
    {
        Texture tex;
        Shader shader;
        Array<Vertex> points;
        Array<Vertex> lines;
        Array<Vertex> triangles;
        MeshBuffer<Vertex> mbuffer;

        Vector2[] circle;

        public DebugDraw()
        {
            tex = DataCache.Get<Texture>("White.Texture");
            shader = DefaultShaders.ColorMult;
            points = new Array<Vertex>(50);
            lines = new Array<Vertex>(50);
            triangles = new Array<Vertex>(50);

            mbuffer = MeshBuffer<Vertex>.Create();
        }

        [Conditional("DEBUG")]
        public void Point(Vector2 pos, Color c)
        {
            points.Push(new Vertex(pos, Vector2.Zero, c));
        }

        [Conditional("DEBUG")]
        public void Circle(Vector2 pos, float radius, Color c)
        {
            if (circle == null)
            {
                int segments = 32;
                circle = new Vector2[segments];
                for (int i = 0; i < segments; i++)
                {
                    float angle = i;
                    angle = angle.Remap(0, segments, 0, PI * 2);
                    circle[i] = new Vector2(Cos(angle), Sin(angle));
                }
            }

            var last_idx = circle.Length - 1;
            for (int i = 0; i < circle.Length - 1; i++)
            {
                lines.Push(new Vertex(circle[i + 0] * radius + pos, Vector2.Zero, c));
                lines.Push(new Vertex(circle[i + 1] * radius + pos, Vector2.Zero, c));
            }
            lines.Push(new Vertex(circle[0] * radius + pos, Vector2.Zero, c));
            lines.Push(new Vertex(circle[last_idx] * radius + pos, Vector2.Zero, c));
        }
        
        [Conditional("DEBUG")]
        public void Line(Vector2 start, Vector2 end, Color c)
        {
            lines.Push(new Vertex(start, Vector2.Zero, c));
            lines.Push(new Vertex(end, Vector2.Zero, c));
        }

        [Conditional("DEBUG")]
        public void Rect(Rect rect, Color c)
        {
            var v1 = rect.XMinYMin;
            var v2 = rect.XMinYMax;
            var v3 = rect.XMaxYMax;
            var v4 = rect.XMaxYMin;

            lines.Push(new Vertex(v1, Vector2.Zero, c));
            lines.Push(new Vertex(v2, Vector2.Zero, c));

            lines.Push(new Vertex(v2, Vector2.Zero, c));
            lines.Push(new Vertex(v3, Vector2.Zero, c));

            lines.Push(new Vertex(v3, Vector2.Zero, c));
            lines.Push(new Vertex(v4, Vector2.Zero, c));

            lines.Push(new Vertex(v4, Vector2.Zero, c));
            lines.Push(new Vertex(v1, Vector2.Zero, c));
        }

        [Conditional("DEBUG")]
        public void FillRect(Rect rect, Color c)
        {
            var v1 = rect.XMinYMin;
            var v2 = rect.XMinYMax;
            var v3 = rect.XMaxYMax;
            var v4 = rect.XMaxYMin;

            triangles.Push(new Vertex(v1, Vector2.Zero, c));
            triangles.Push(new Vertex(v3, Vector2.Zero, c));
            triangles.Push(new Vertex(v2, Vector2.Zero, c));

            triangles.Push(new Vertex(v1, Vector2.Zero, c));
            triangles.Push(new Vertex(v3, Vector2.Zero, c));
            triangles.Push(new Vertex(v4, Vector2.Zero, c));
        }

        [Conditional("DEBUG")]
        public void DrawAll()
        {
            var gfx = App.Graphics;
            gfx.SetPointSize(3);
            gfx.SetLineWidth(1);
            gfx.SetBlendMode(BlendMode.Disabled);
            gfx.SetShader(shader);
            shader.SetTexture("main_tex", 0, tex);
            shader.SetMatrix3x2("model_mat", Matrix3x2.Identity);
            shader.SetMatrix4x4("view_mat", gfx.ViewMatrix);

            // draw triangles
            mbuffer.UpdateVertices(triangles);
            mbuffer.Draw(PrimitiveType.Triangles);

            // draw lines
            mbuffer.UpdateVertices(lines);
            mbuffer.Draw(PrimitiveType.Lines);

            // draw points
            mbuffer.UpdateVertices(points);
            mbuffer.Draw(PrimitiveType.Points);
        }

        [Conditional("DEBUG")]
        public void Clear()
        {
            points.Clear(false);
            lines.Clear(false);
            triangles.Clear(false);
        }


    }
}