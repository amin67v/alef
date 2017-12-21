using System;
using System.Numerics;

namespace Engine
{
    public class Text
    {
        string content = null;
        Transform xform = new Transform();
        TextAlignment align = TextAlignment.TopLeft;
        Font font = null;
        Color color = Color.White;
        float line = 1;
        Array<Vertex> verts = null;
        MeshBuffer mesh = null;
        Shader shader = null;
        bool dirty = true;

        public Text(string content, Font font, Color color)
        {
            Assert.IsTrue(font != null);
            this.content = content;
            this.font = font;
            this.color = color;

            int vtx_count = string.IsNullOrEmpty(content) ? 0 : content.Length * 6;
            verts = new Array<Vertex>(vtx_count);
            mesh = MeshBuffer.Create(IntPtr.Zero, vtx_count);
            shader = DefaultShaders.Font;
        }

        public string Content
        {
            get => content;
            set
            {
                dirty = true;
                content = value;
            }
        }

        public Transform Transform => xform;

        public TextAlignment Alignment
        {
            get => align;
            set
            {
                align = value;
                dirty = true;
            }
        }

        public Font Font
        {
            get => font;
            set
            {
                dirty = true;
                font = value;
            }
        }

        public Color Color
        {
            get => color;
            set => color = value;
        }

        public float LineSpacing
        {
            get => line;
            set
            {
                dirty = true;
                line = value;
            }
        }

        public void Draw()
        {
            if (dirty)
                build_mesh();

            var gfx = App.Graphics;
            gfx.SetBlendMode(BlendMode.AlphaBlend);
            gfx.SetShader(shader);
            shader.SetMatrix4x4("view_mat", gfx.ViewMatrix);
            shader.SetMatrix3x2("model_mat", xform.Matrix);
            shader.SetTexture("main_tex", 0, font.Texture);
            shader.SetColor("color", color);

            mesh.Draw(PrimitiveType.Triangles);
        }

        void build_mesh()
        {
            if (font == null || string.IsNullOrEmpty(content))
            {
                mesh.UpdateVertices(IntPtr.Zero, 0);
                return;
            }

            verts.Clear(false);
            var tex_size = font.Texture.Size;

            Vector2 pos = Vector2.Zero;
            int line_end_idx = 0;
            int line_start_idx = 0;

            void align_h()
            {
                float xpush = 0;

                if ((align & TextAlignment.Left) != 0)
                    return;
                else if ((align & TextAlignment.Right) != 0)
                    xpush = MathF.Round(pos.X);
                else
                    xpush = MathF.Round(pos.X * .5f);

                for (int i = line_start_idx; i < line_end_idx; i++)
                    verts.Items[i].Position.X -= xpush;
            }

            void aling_v()
            {
                float ypush = 0;

                if ((align & TextAlignment.Top) != 0)
                    return;
                else if ((align & TextAlignment.Bottom) != 0)
                    ypush = MathF.Round(pos.Y + font.Height * .5f);
                else
                    ypush = MathF.Round(pos.Y * .5f + font.Height * .5f);

                for (int i = 0; i < verts.Count; i++)
                    verts.Items[i].Position.Y -= ypush;
            }

            for (int i = 0; i < content.Length; i++)
            {
                var c = content[i];
                if (c == ' ')
                {
                    pos.X += font.SpaceWidth;
                    continue;
                }
                else if (c == '\n')
                {
                    align_h();
                    line_start_idx = line_end_idx;
                    pos.X = 0;
                    pos.Y += font.Height;
                }

                var glyph = font[c];
                if (glyph == null)
                    continue;

                var vert_uv = glyph.Rect / tex_size;
                vert_uv.Y = 1 - vert_uv.YMax;
                var vert_pos = glyph.Rect;
                vert_pos.Position = glyph.Offset + pos;

                verts.Push(new Vertex(vert_pos.XMinYMin, vert_uv.XMinYMax, Color.White));
                verts.Push(new Vertex(vert_pos.XMinYMax, vert_uv.XMinYMin, Color.White));
                verts.Push(new Vertex(vert_pos.XMaxYMax, vert_uv.XMaxYMin, Color.White));
                verts.Push(new Vertex(vert_pos.XMinYMin, vert_uv.XMinYMax, Color.White));
                verts.Push(new Vertex(vert_pos.XMaxYMax, vert_uv.XMaxYMin, Color.White));
                verts.Push(new Vertex(vert_pos.XMaxYMin, vert_uv.XMaxYMax, Color.White));
                line_end_idx = verts.Count;
                pos.X += glyph.Width;
            }
            align_h();
            aling_v();

            mesh.UpdateVertices(verts);
            dirty = false;
        }
    }

    public enum TextAlignment
    {
        Middle = 0x00,
        Left = 0x01,
        Right = 0x02,
        Top = 0x04,
        Bottom = 0x08,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right
    }
}