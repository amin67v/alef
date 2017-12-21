using System;
using System.Numerics;

using Engine;

namespace Editor
{
    public class Gui
    {
        static Color PanelColor = new Color(230, 230, 230, 255);
        static Color LabelColor = new Color(045, 045, 045, 255);
        static Color BackIdleColor = new Color(200, 200, 200, 255);
        static Color ForeIdleColor = new Color(105, 105, 105, 255);
        static Color BackHotColor = new Color(210, 210, 210, 255);
        static Color ForeHotColor = new Color(140, 140, 140, 255);
        static Color SelectBackColor = new Color(000, 180, 255, 255);
        static Color SelectForeColor = new Color(255, 255, 255, 255);

        Array<Window> windows;
        Texture tex;
        Shader shader;
        Vector2 m_pos;
        Text text;
        int hot_id;
        int act_id;
        int focus_id;
        int wnd_id = 0;
        char? keychar;
        bool m_down;

        public Gui()
        {
            windows = new Array<Window>();
            CreateWindow<Window>("test window", new Rect(100, 100, 400, 400));
            shader = DefaultShaders.ColorMult;
            tex = Texture.Create(1, 1, FilterMode.Point, true, new Color[] { Color.White });
            text = new Text("", Font.Load("fonts/mono8.font"), Color.White);
        }

        public Window CreateWindow<T>(string title, Rect rect) where T : Window, new()
        {
            var new_win = new T();
            new_win.Gui = this;
            new_win.Rect = rect;
            wnd_id++;
            new_win.window_id = wnd_id;
            windows.Push(new_win);
            return new_win;
        }

        public void NewFrame(Vector2 mpos, bool mdown, char? keychar)
        {
            this.m_pos = mpos;
            this.m_down = mdown;
            this.keychar = keychar;

            var gfx = App.Graphics;
            gfx.SetBlendMode(BlendMode.AlphaBlend);
            var screen = App.Window.Size;
            gfx.ViewMatrix = Matrix4x4.CreateOrthographicOffCenter(0, screen.X, screen.Y, 0, -1, 1);
            gfx.SetViewport(new Rect(Vector2.Zero, screen));

            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].OnGui();
                windows[i].draw();
            }

            // reset state
            if (!m_down)
                act_id = 0;

            hot_id = 0;
        }

        public class Window
        {
            Gui owner;
            string title;
            Rect pos;
            Array<Vertex> verts;
            Array<TextDraw> texts;
            MeshBuffer mesh;
            internal int window_id;
            int widget_id = 0;

            public Window()
            {
                this.verts = new Array<Vertex>(100);
                this.texts = new Array<TextDraw>(20);
                this.mesh = MeshBuffer.Create();
            }

            public Rect Rect
            {
                get => pos;
                set => pos = value;
            }

            public string Title
            {
                get => title;
                set => title = value;
            }

            public Gui Gui
            {
                get => owner;
                internal set => owner = value;
            }

            Vector2 mpos => Gui.m_pos - pos.Position;

            public virtual void OnGui()
            {
                if (Button(new Rect(50, 50, 100, 20), "Test"))
                {
                    EditorBase.clear_color = Color.Green;
                }

                if (Button(new Rect(50, 150, 100, 20), "Test 2"))
                {
                    EditorBase.clear_color = Color.Blue;
                }

                if (Button(new Rect(50, 250, 100, 20), "Test 3"))
                {
                    EditorBase.clear_color = Color.Red;
                }
            }

            public bool Button(Rect rect, string str)
            {
                int id = get_id();

                if (rect.Contains(mpos))
                {
                    Gui.hot_id = id;
                    if (Gui.act_id == 0 && Gui.m_down)
                    {
                        Gui.act_id = id;
                    }
                }

                if (Gui.act_id == id)
                {
                    add_rect(rect, SelectBackColor);
                    add_text(rect, str, TextAlignment.Middle, SelectForeColor);
                }
                else if (Gui.hot_id == id)
                {
                    add_rect(rect, BackHotColor);
                    add_text(rect, str, TextAlignment.Middle, ForeHotColor);
                }
                else
                {   
                    add_rect(rect, BackIdleColor);
                    add_text(rect, str, TextAlignment.Middle, ForeIdleColor);
                }

                return !Gui.m_down && Gui.hot_id == id && Gui.act_id == id;
            }

            // public float Slider(int id, Rect rect, float min, float max, float value)
            // {
            //     if (rect.Contains(m_pos))
            //     {
            //         hot_id = id;
            //         if (act_id == 0 && m_down)
            //         {
            //             act_id = id;
            //         }
            //     }
            // 
            //     var norm_value = ((value - min) / max);
            //     Rect hdl_pos = new Rect(0, 0, rect.Width * .1f, rect.Height);
            //     hdl_pos.Center = new Vector2(rect.Width * norm_value + rect.X, rect.YHalf);
            // 
            //     if (act_id == id || hot_id == id)
            //         draw_rect(rect, BackHotColor);
            //     else
            //         draw_rect(rect, BackIdleColor);
            // 
            //     if (act_id == id)
            //         draw_rect(hdl_pos, SelectBackColor);
            //     else if (hot_id == id)
            //         draw_rect(hdl_pos, ForeHotColor);
            //     else
            //         draw_rect(hdl_pos, ForeIdleColor);
            // 
            //     if (act_id == id)
            //     {
            //         var norm_mpos = ((m_pos.X - rect.X) / rect.Width);
            //         value = min + (max - min) * norm_mpos;
            //     }
            // 
            //     return Math.Clamp(value, min, max);
            // }
            // 
            // 
            // public int IntSlider(int id, Rect rect, int min, int max, int value)
            // {
            //     return (int)MathF.Round(Slider(id, rect, min, max, value));
            // }
            // 
            // public string TextInput(int id, Rect rect, string value)
            // {
            //     if (rect.Contains(m_pos))
            //     {
            //         hot_id = id;
            //         if (act_id == 0 && m_down)
            //         {
            //             act_id = id;
            //             focus_id = id;
            //         }
            //     }
            // 
            //     if (focus_id == id && last_char != null)
            //     {
            //         if (last_char == 'b')
            //             value = value.Substring(0, value.Length - 1);
            //         else
            //             value += last_char;
            //     }
            // 
            //     if (focus_id == id)
            //     {
            //         draw_rect(rect, SelectBackColor);
            //         draw_text(rect, value, TextAlignX.Left, TextAlignY.Center, SelectForeColor);
            //     }
            //     else if (hot_id == id)
            //     {
            //         draw_rect(rect, BackHotColor);
            //         draw_text(rect, value, TextAlignX.Left, TextAlignY.Center, ForeHotColor);
            //     }
            //     else
            //     {
            //         draw_rect(rect, BackIdleColor);
            //         draw_text(rect, value, TextAlignX.Left, TextAlignY.Center, ForeIdleColor);
            //     }
            // 
            //     return value;
            // }

            void add_rect(Rect rect, Color c)
            {
                verts.Push(new Vertex(rect.XMinYMin, Vector2.Zero, c));
                verts.Push(new Vertex(rect.XMaxYMax, Vector2.Zero, c));
                verts.Push(new Vertex(rect.XMinYMax, Vector2.Zero, c));
                verts.Push(new Vertex(rect.XMinYMin, Vector2.Zero, c));
                verts.Push(new Vertex(rect.XMaxYMax, Vector2.Zero, c));
                verts.Push(new Vertex(rect.XMaxYMin, Vector2.Zero, c));
            }

            void add_text(Rect rect, string str, TextAlignment align, Color c)
            {
                texts.Push(new TextDraw(rect, str, align, c));
            }

            void draw_text(TextDraw data)
            {
                var text = Gui.text;
                text.Content = data.Str;
                text.Alignment = data.Alignment;
                text.Color = data.Color;
                var rect = data.Rect;
                float x, y;

                if ((data.Alignment & TextAlignment.Left) != 0)
                    x = rect.XMin;
                else if ((data.Alignment & TextAlignment.Right) != 0)
                    x = rect.XMax;
                else
                    x = rect.XHalf;

                if ((data.Alignment & TextAlignment.Top) != 0)
                    y = rect.YMin;
                else if ((data.Alignment & TextAlignment.Bottom) != 0)
                    y = rect.YMax;
                else
                    y = rect.YHalf;

                text.Transform.Position = new Vector2(x, y) + pos.Position;
                text.Draw();
            }

            int get_id()
            {
                widget_id++;
                return (window_id << 16) | widget_id;
            }

            internal void draw()
            {
                var gfx = App.Graphics;
                gfx.SetShader(Gui.shader);
                Gui.shader.SetMatrix3x2("model_mat", Matrix3x2.CreateTranslation(Rect.Position));
                Gui.shader.SetMatrix4x4("view_mat", gfx.ViewMatrix);
                Gui.shader.SetTexture("main_tex", 0, Gui.tex);

                mesh.UpdateVertices(verts);
                mesh.Draw(PrimitiveType.Triangles);

                for (int i = 0; i < texts.Count; i++)
                    draw_text(texts[i]);

                verts.Clear();
                texts.Clear();
                add_rect(new Rect(Vector2.Zero, pos.Size), PanelColor);
                widget_id = 0;
            }

            struct TextDraw
            {
                public Rect Rect;
                public String Str;
                public TextAlignment Alignment;
                public Color Color;

                public TextDraw(Rect rect, string str, TextAlignment align, Color c)
                {
                    this.Rect = rect;
                    this.Str = str;
                    this.Alignment = align;
                    this.Color = c;
                }
            }
        }
    }
}