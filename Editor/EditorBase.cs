using System;
using System.Numerics;

using Engine;

namespace Editor
{
    public class EditorBase : AppState
    {
        string str = "test input text";
        public static Color clear_color = Color.Black;
        Gui gui;
        char? keychar;

        public override void OnBegin()
        {
            gui = new Gui();
        }

        public override void OnFrame()
        {
            var gfx = App.Graphics;
            gfx.Clear(clear_color);


            gui.NewFrame(App.Window.CursorPosition, App.Window.IsMouseDown(0), keychar);
            keychar = null;

            //gui.Panel(new Rect(0, 0, 500, 500));

            // if (gui.Button(1, new Rect(10, 10, 100, 20), "Change color"))
            // {
            //     clear_color = Engine.Random.Color();
            // }
// 
            // var c = clear_color.ToVector4();
// 
            // c.X = gui.Slider(2, new Rect(100, 100, 100, 20), 0f, 1f, c.X);
            // c.Y = gui.Slider(3, new Rect(100, 140, 100, 20), 0f, 1f, c.Y);
            // c.Z = gui.Slider(4, new Rect(100, 180, 100, 20), 0f, 1f, c.Z);
            // clear_color = new Color(c.X, c.Y, c.Z, 1f);
// 
            // str = gui.TextInput(6, new Rect(100, 250, 100, 20), str);
// 
            // gui.End();

            gfx.Display();
        }

        public override void OnKeyChar(string code)
        {
            keychar = code[0];
        }
    }
}
