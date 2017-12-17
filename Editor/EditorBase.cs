using System;
using System.Numerics;

using Engine;

namespace Editor
{
    public class EditorBase : AppState
    {
        Color clear_color = Color.Black;
        string str = "test input text";

        public override void OnResize(int width, int height)
        {
            App.Graphics.Viewport(0, 0, width, height);
        }
        
        public override void OnFrame()
        {
            var gfx = App.Graphics;
            gfx.Clear(clear_color);

            var rect = new Rect(Vector2.Zero, App.Window.Size);
            gfx.SetView(rect.X, rect.XMax, rect.YMax, rect.Y);

            Gui.NewFrame();

            Gui.Panel(new Rect(0, 0, 500, 500));

            if (Gui.Button(1, new Rect(10, 10, 100, 20), "Change color"))
            {
                clear_color = Engine.Random.Color();
            }

            var c = clear_color.ToVector4();

            c.X = Gui.Slider(2, new Rect(100, 100, 100, 20), 0f, 1f, c.X);
            c.Y = Gui.Slider(3, new Rect(100, 140, 100, 20), 0f, 1f, c.Y);
            c.Z = Gui.Slider(4, new Rect(100, 180, 100, 20), 0f, 1f, c.Z);
            clear_color = new Color(c.X, c.Y, c.Z, 1f);

            str = Gui.TextInput(6, new Rect(100, 250, 100, 20), str);

            Gui.EndRender();
            gfx.Display();
        }

        public override void OnMouseMove(Vector2 pos)
        {
            Gui.MousePos = pos;
        }

        public override void OnMouseDown(int key, Vector2 pos)
        {
            Gui.MouseIsDown = true;
        }

        public override void OnMouseUp(int key, Vector2 pos)
        {
            Gui.MouseIsDown = false;
        }

        public override void OnKeyChar(string code)
        {
            Gui.SetCharInput(code[0]);
        }
    }
}
