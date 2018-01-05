using System;
using System.IO;
using System.Numerics;

using Engine;

public class EditorState : AppState
{
    DataBrowser browser;

    Vector2 pan;
    Mesh grid;
    Vector2 last_mpos;
    bool drag;

    public override void OnBegin()
    {
        base.OnBegin();
        
        browser = new DataBrowser();

        grid = new Mesh();
        grid.MainTexture = Texture.Load("editor/grid.png", FilterMode.Trilinear, WrapMode.Repeat);
        grid.AddVertex(new Vertex(0, 0, 0, 0, Color.White));
        grid.AddVertex(new Vertex(0, 0, 0, 0, Color.White));
        grid.AddVertex(new Vertex(0, 0, 0, 0, Color.White));
        grid.AddVertex(new Vertex(0, 0, 0, 0, Color.White));
        grid.AddIndex(0);
        grid.AddIndex(1);
        grid.AddIndex(2);
        grid.AddIndex(0);
        grid.AddIndex(1);
        grid.AddIndex(3);
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
        drag = true;
    }

    public override void OnMouseUp(MouseButton btn, Vector2 pos)
    {
        drag = false;
    }

    public void OnDrawCanvas()
    {
        var gfx = App.Graphics;
        var screen = App.Window.Size;
        gfx.SetViewport(0, 0, (int)screen.X, (int)screen.Y);
        gfx.ViewMatrix = Matrix4x4.CreateOrthographicOffCenter(pan.X, screen.X + pan.X, pan.Y, screen.Y + pan.Y, -1, 1);
        Matrix4x4 inv_view;
        Matrix4x4.Invert(gfx.ViewMatrix, out inv_view);
        var p1 = Vector2.Transform(new Vector2(-1, -1), inv_view);
        var p2 = Vector2.Transform(new Vector2(1, 1), inv_view);
        var p3 = Vector2.Transform(new Vector2(-1, 1), inv_view);
        var p4 = Vector2.Transform(new Vector2(1, -1), inv_view);
        grid.SetVertex(0, new Vertex(p1, p1 * 0.0035f, Color.White));
        grid.SetVertex(1, new Vertex(p2, p2 * 0.0035f, Color.White));
        grid.SetVertex(2, new Vertex(p3, p3 * 0.0035f, Color.White));
        grid.SetVertex(3, new Vertex(p4, p4 * 0.0035f, Color.White));
        grid.Draw();

        var mpos = App.Window.MousePosition;
        if (drag)
        {
            var delta = (mpos - last_mpos);
            delta.Y *= -1;
            pan -= delta;
        }
        last_mpos = mpos;

    }

    public void OnGui()
    {
        browser.Draw();
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
        OnDrawCanvas();
        App.Gui.Render(OnGui);
        gfx.Display();
    }

}
