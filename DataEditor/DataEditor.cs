using System;
using System.IO;
using System.Numerics;

using Engine;

public class DataEditor : AppState
{
    static DataEditor instance;

    public override void OnBegin()
    {
        base.OnBegin();
        instance = this;
    }

    public override void OnEnd()
    {
        instance = null;
    }

    public override void OnFileDrop(string file)
    {
        App.Log.Debug(file);
    }

    public override void OnTextInput(string text)
    {
        App.Gui.AddInputChar(text[0]);
    }

    public override void OnMouseDown(MouseButton btn, Vector2 pos)
    {
        //drag = true;
    }

    public override void OnMouseUp(MouseButton btn, Vector2 pos)
    {
        //drag = false;
    }

    public void OnDrawCanvas()
    {

    }

    public void OnGui()
    {
        Browser.Draw();
        Inspector.Draw();
        Modal.Draw();
    }

    void custom_style()
    {
        var gui = App.Gui;
        if (gui.Button("     Save     "))
        {
            string content = "";
            for (int i = 0; i < (int)ColorTarget.Count; i++)
            {
                var target = (ColorTarget)i;
                var c = gui.Style.GetColor(target);
                content += $"style.SetColor(ColorTarget.{target.ToString()}, new Color({(c.R / 255f).ToString("0.00")}f, {(c.G / 255f).ToString("0.00")}f, {(c.B / 255f).ToString("0.00")}f, {(c.A / 255f).ToString("0.00")}f));\n";
            }
            File.WriteAllText(App.GetAbsolutePath("style.txt"), content);
        }

        for (int i = 0; i < (int)ColorTarget.Count; i++)
        {
            var target = (ColorTarget)i;
            var c = gui.Style.GetColor(target);
            gui.ColorEdit(target.ToString(), ref c, ColorEditFlags.Default | ColorEditFlags.AlphaPreview);
            gui.Style.SetColor(target, c);
        }
    }

    public override void OnFrame()
    {
        var gfx = App.Graphics;
        gfx.Clear(Color.SkyBlue);
        Canvas.Render();
        App.Gui.Render(OnGui);
        gfx.Display();
    }

}
