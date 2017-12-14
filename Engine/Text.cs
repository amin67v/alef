using System;
using System.Numerics;

namespace Engine
{
    public class Text
    {
        string content = null;
        Transform xform = new Transform();
        TextAlignX align_x = TextAlignX.Left;
        TextAlignY align_y = TextAlignY.Top;
        Font font = null;
        Color color = Color.White;
        float line = 1;
        Array<Vertex> vert_arr = null;
        MeshBuffer mesh = null;
        Shader font_fx = null;
        bool dirty = true;

        public Text(string content, Font font, Color color)
        {
            Assert.IsTrue(font != null);
            this.content = content;
            this.font = font;
            this.color = color;

            int vtx_count = string.IsNullOrEmpty(content) ? 0 : content.Length * 6;
            vert_arr = new Array<Vertex>(vtx_count);
            mesh = MeshBuffer.Create(IntPtr.Zero, vtx_count);
            font_fx = DefaultShaders.Font;
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

        public TextAlignX AlignX
        {
            get => align_x;
            set
            {
                dirty = true;
                align_x = value;
            }
        }

        public TextAlignY AlignY
        {
            get => align_y;
            set
            {
                dirty = true;
                align_y = value;
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
            gfx.BlendMode = BlendMode.AlphaBlend;
            Shader.Active = font_fx;
            font_fx.SetMatrix4x4("view", gfx.ViewMatrix);
            font_fx.SetMatrix3x2("model", xform.Matrix);
            font_fx.SetTexture("tex", 0, font.Texture);
            font_fx.SetColor("color", color);

            mesh.Draw(PrimitiveType.Triangles);
        }

        void build_mesh()
        {
            if (font == null || string.IsNullOrEmpty(content))
            {
                mesh.UpdateVertices(IntPtr.Zero, 0);
                return;
            }

            vert_arr.Clear();
            var tex_size = font.Texture.Size;

            Vector2 pos = Vector2.Zero;
            int line_end_idx = 0;
            int line_start_idx = 0;

            void align_h()
            {
                if (align_x == TextAlignX.Left)
                    return;

                float xpush = MathF.Round((float)align_x * .5f * pos.X);

                for (int i = line_start_idx; i < line_end_idx; i++)
                    vert_arr.Items[i].Position.X -= xpush;
            }

            void aling_v()
            {
                if (align_y == TextAlignY.Top)
                    return;

                float ypush = MathF.Round(((float)align_y * .5f * pos.Y) + (font.Height * .5f));
                for (int i = 0; i < vert_arr.Count; i++)
                    vert_arr.Items[i].Position.Y -= ypush;
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

                vert_arr.Push(new Vertex(vert_pos.XMinYMin, vert_uv.XMinYMax, Color.White));
                vert_arr.Push(new Vertex(vert_pos.XMinYMax, vert_uv.XMinYMin, Color.White));
                vert_arr.Push(new Vertex(vert_pos.XMaxYMax, vert_uv.XMaxYMin, Color.White));
                vert_arr.Push(new Vertex(vert_pos.XMinYMin, vert_uv.XMinYMax, Color.White));
                vert_arr.Push(new Vertex(vert_pos.XMaxYMax, vert_uv.XMaxYMin, Color.White));
                vert_arr.Push(new Vertex(vert_pos.XMaxYMin, vert_uv.XMaxYMax, Color.White));
                line_end_idx = vert_arr.Count;
                pos.X += glyph.Width;
            }
            align_h();
            aling_v();

            mesh.UpdateVertices(vert_arr);
            dirty = false;
        }
    }

    public enum TextAlignX { Left, Center, Right }
    public enum TextAlignY { Top, Center, Bottom }
}