using System;
using System.IO;
using System.Numerics;

using Engine;

public class DataEditor : Disposable, IAppState
{
    public void OnBegin() { }

    public void OnEnd() 
    {
        Editable.Active?.TrySave(null);
        Editable.Active?.OnEnd();
    }

    public void OnFrame()
    {
        var gfx = App.Graphics;
        gfx.Clear(Color.Gray);
        Gui.Render(OnGui);
        gfx.Display();
    }

    public void OnGui(Gui gui)
    {
        Rect remain = new Rect(Vector2.Zero, App.Window.Size);
        var toolbar = remain; toolbar.Height = remain.YMin = 44;
        var browser = remain; browser.Width = remain.XMin = 250;
        var inspector = remain; inspector.XMin = remain.XMax = (remain.XMax - 320);
        var canvas = remain;

        Toolbar.Instance.Draw(gui, toolbar);
        Browser.Instance.Draw(gui, browser);
        Inspector.Instance.Draw(gui, inspector);
        Canvas.Instance.Draw(gui, canvas);
        ContextMenu.Instance.Draw(gui);
        Dialog.Instance.Draw(gui);

    }

    void custom_style(Gui gui)
    {
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
}

public delegate void FileDropHandler(string file);

public delegate void MouseButtonHandler(MouseButton btn, Vector2 pos);
