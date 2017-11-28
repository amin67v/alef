using System;
using System.Numerics;

using Engine;

namespace Editor
{
    public static class Gui
    {
        static Color PanelColor = new Color(230, 230, 230, 255);
        static Color LabelColor = new Color(045, 045, 045, 255);
        static Color BackIdleColor = new Color(200, 200, 200, 255);
        static Color ForeIdleColor = new Color(105, 105, 105, 255);
        static Color BackHotColor = new Color(210, 210, 210, 255);
        static Color ForeHotColor = new Color(140, 140, 140, 255);
        static Color SelectBackColor = new Color(000, 180, 255, 255);
        static Color SelectForeColor = new Color(255, 255, 255, 255);

        static Array<Vertex> verts = new Array<Vertex>(5000);
        static MeshBuffer mesh;
        static Texture tex;
        static Shader shader;
        static Vector2 m_pos;
        static Text text;
        static int hot_id;
        static int act_id;
        static int focus_id;
        static char? last_char;
        static bool m_down;

        static Gui()
        {
            shader = ResourceManager.LoadShader("shaders/default.glsl");
            tex = Texture.Create(1, 1, FilterMode.Point, true, new Color[] { Color.White });
            mesh = MeshBuffer.Create();
            text = new Text("", ResourceManager.LoadFont("fonts/roboto_light_8.json"), Color.White);
        }

        public static Vector2 MousePos
        {
            get => m_pos;
            set => m_pos = value;
        }

        public static bool MouseIsDown
        {
            get => m_down;
            set => m_down = value;
        }

        public static void NewFrame()
        {
            Graphics.BlendMode = BlendMode.AlphaBlend;
            var wnd_size = Window.Size;
            Graphics.SetView(0, wnd_size.X, wnd_size.Y, 0);
        }

        public static void EndRender()
        {
            // reset state
            if (!m_down)
                act_id = 0;

            hot_id = 0;
            last_char = null;
        }

        public static void Panel(Rect rect)
        {
            draw_rect(rect, PanelColor);
        }

        public static bool Button(int id, Rect rect, string str)
        {
            if (rect.Contains(m_pos))
            {
                hot_id = id;
                if (act_id == 0 && m_down)
                    act_id = id;
            }

            if (act_id == id)
            {
                draw_rect(rect, SelectBackColor);
                draw_text(rect, str, TextAlignX.Center, TextAlignY.Center, SelectForeColor);
            }
            else if (hot_id == id)
            {
                draw_rect(rect, BackHotColor);
                draw_text(rect, str, TextAlignX.Center, TextAlignY.Center, ForeHotColor);
            }
            else
            {
                draw_rect(rect, BackIdleColor);
                draw_text(rect, str, TextAlignX.Center, TextAlignY.Center, ForeIdleColor);
            }

            return !m_down && hot_id == id && act_id == id;
        }

        public static float Slider(int id, Rect rect, float min, float max, float value)
        {
            if (rect.Contains(m_pos))
            {
                hot_id = id;
                if (act_id == 0 && m_down)
                    act_id = id;
            }

            var norm_value = ((value - min) / max);
            Rect hdl_pos = new Rect(0, 0, rect.Width * .1f, rect.Height);
            hdl_pos.Center = new Vector2(rect.Width * norm_value + rect.X, rect.YHalf);

            if (act_id == id || hot_id == id)
                draw_rect(rect, BackHotColor);
            else
                draw_rect(rect, BackIdleColor);

            if (act_id == id)
                draw_rect(hdl_pos, SelectBackColor);
            else if (hot_id == id)
                draw_rect(hdl_pos, ForeHotColor);
            else
                draw_rect(hdl_pos, ForeIdleColor);

            if (act_id == id)
            {
                var norm_mpos = ((m_pos.X - rect.X) / rect.Width);
                value = min + (max - min) * norm_mpos;
            }

            return Math.Clamp(value, min, max);
        }


        public static int IntSlider(int id, Rect rect, int min, int max, int value)
        {
            return (int)MathF.Round(Slider(id, rect, min, max, value));
        }

        public static string TextInput(int id, Rect rect, string value)
        {
            if (rect.Contains(m_pos))
            {
                hot_id = id;
                if (act_id == 0 && m_down)
                {
                    act_id = id;
                    focus_id = id;
                }
            }

            if (focus_id == id && last_char != null)
            {
                if (last_char == 'b')
                    value = value.Substring(0, value.Length - 1);
                else
                    value += last_char;
            }
            if (focus_id == id)
            {
                draw_rect(rect, SelectBackColor);
                draw_text(rect, value, TextAlignX.Left, TextAlignY.Center, SelectForeColor);
            }
            else if (hot_id == id)
            {
                draw_rect(rect, BackHotColor);
                draw_text(rect, value, TextAlignX.Left, TextAlignY.Center, ForeHotColor);
            }
            else
            {
                draw_rect(rect, BackIdleColor);
                draw_text(rect, value, TextAlignX.Left, TextAlignY.Center, ForeIdleColor);
            }

            return value;
        }

        public static void SetCharInput(char value)
        {
            last_char = value;
        }

        static void draw_text(Rect rect, string str, TextAlignX alignx, TextAlignY aligny, Color c)
        {
            text.Content = str;
            text.AlignX = alignx;
            text.AlignY = aligny;
            text.Color = c;
            float x, y;

            if (alignx == TextAlignX.Left)
                x = rect.XMin;
            else if (alignx == TextAlignX.Center)
                x = rect.XHalf;
            else
                x = rect.XMax;

            if (aligny == TextAlignY.Top)
                y = rect.YMin;
            else if (aligny == TextAlignY.Center)
                y = rect.YHalf;
            else
                y = rect.YMax;

            text.Transform.Position = new Vector2(x, y);

            text.Draw();
        }

        public static void draw_rect(Rect rect, Color c)
        {
            Shader.Active = shader;
            shader.SetMatrix4x4("view", Graphics.ViewMatrix);
            shader.SetTexture("tex", 0, tex);

            verts.Clear();
            verts.Push(new Vertex(rect.XMinYMin, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMaxYMax, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMinYMax, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMinYMin, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMaxYMax, Vector2.Zero, c));
            verts.Push(new Vertex(rect.XMaxYMin, Vector2.Zero, c));
            mesh.UpdateVertices(verts);
            mesh.Draw(PrimitiveType.Triangles);
        }

    }
}