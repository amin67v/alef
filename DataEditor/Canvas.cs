using System;
using System.IO;
using System.Numerics;

using Engine;

public class Canvas
{
    static readonly Canvas instance = new Canvas();
    Mesh quad;
    Rect pan_limit = new Rect(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue);
    Rect area;
    Vector2 pos;
    Vector2 last_mpos;
    bool drag;

    Canvas()
    {
        quad = new Mesh();
        Texture.Load("editor/checker.png", FilterMode.Point, WrapMode.Repeat);
        quad.AddVertex(Vertex.Zero, Vertex.Zero, Vertex.Zero, Vertex.Zero);
        quad.AddIndex(0, 1, 2, 0, 1, 3);
        quad.BlendMode = BlendMode.AlphaBlend;
    }

    public static ref Rect PanLimit => ref instance.pan_limit;

    public static Rect Area => instance.area;

    public static void Render() => instance.render();

    public static void DragBegin()
    {

    }

    public static void DragEnd()
    {

    }

    public void render()
    {
        var gfx = App.Graphics;
        var screen = App.Window.Size;
        var w = screen.X - (Browser.Width + Inspector.Width);
        Rect rect = new Rect(0, 0, 0, 0); ;
        if (w > 0)
            rect = new Rect(Browser.Width, 0, w, screen.Y);

        gfx.SetViewport(rect);
        gfx.SetScissor(rect);
        pos.X = pos.X.Clamp(PanLimit.X, PanLimit.XMax);
        pos.Y = pos.Y.Clamp(PanLimit.Y, PanLimit.YMax);
        gfx.ViewMatrix = Matrix4x4.CreateOrthographicOffCenter(pos.X, rect.Width + pos.X, pos.Y, rect.Height + pos.Y, -1, 1);

        draw_checker();

        var mpos = App.Window.MousePosition;
        if (drag)
        {
            var delta = (mpos - last_mpos);
            delta.Y *= -1;
            pos -= delta;
        }
        last_mpos = mpos;

        Editable.Active?.OnDrawCanvas(instance);
    }

    public void DrawTexture(Texture tex)
    {
        DrawTexture(tex, new Rect(0, 0, tex.Width, tex.Height));
    }

    public void DrawTexture(Texture tex, Rect dest)
    {
        DrawTexture(tex, new Rect(0, 0, 1, 1), dest, Color.White);
    }

    public void DrawTexture(Texture tex, Rect src, Rect dest, Color c)
    {
        quad.MainTexture = tex;
        quad.SetVertex(0, new Vertex(dest.X, dest.Y, src.X, src.Y, c));
        quad.SetVertex(1, new Vertex(dest.XMax, dest.YMax, src.XMax, src.YMax, c));
        quad.SetVertex(2, new Vertex(dest.X, dest.YMax, src.X, src.YMax, c));
        quad.SetVertex(3, new Vertex(dest.XMax, dest.Y, src.XMax, src.Y, c));
        quad.Draw();
    }

    void draw_checker()
    {
        var gfx = App.Graphics;
        Matrix4x4 inv_view;
        Matrix4x4.Invert(gfx.ViewMatrix, out inv_view);
        var p1 = Vector2.Transform(new Vector2(-1, -1), inv_view);
        var p2 = Vector2.Transform(new Vector2(1, 1), inv_view);
        var p3 = Vector2.Transform(new Vector2(-1, 1), inv_view);
        var p4 = Vector2.Transform(new Vector2(1, -1), inv_view);
        quad.SetVertex(0, new Vertex(p1, p1 * 0.05f, Color.White));
        quad.SetVertex(1, new Vertex(p2, p2 * 0.05f, Color.White));
        quad.SetVertex(2, new Vertex(p3, p3 * 0.05f, Color.White));
        quad.SetVertex(3, new Vertex(p4, p4 * 0.05f, Color.White));
        quad.MainTexture = Texture.Load("editor/checker.png");
        quad.Draw();
    }
}