using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

using Engine;

public class Canvas : Panel
{
    public static readonly Canvas Instance = new Canvas();

    Mesh quad;
    float pnt_sz = 6;
    float zoom = 1f;
    float rot;
    Vector2 pos;
    Vector2 mpos_w;
    Vector2 lm_normal; // last frame
    Vector2 mdelta_w;
    Vector2[] circle;
    Rect crect;
    Texture ctex; // 1x1 pixel white texture
    Array<Vertex> points;
    Array<Vertex> lines;
    Array<Vertex> triangles;
    MeshBuffer mbuffer;
    Matrix4x4 viewmat;
    Matrix4x4 inv_viewmat;
    int act_id = -1;
    int nxt_id = 0;
    bool grab;
    bool minside;
    bool clicked;

    Canvas()
    {
        quad = new Mesh();
        Engine.Texture.Load("editor/checker.png", FilterMode.Point, WrapMode.Repeat);

        quad.AddVertex(Vertex.Zero, Vertex.Zero, Vertex.Zero, Vertex.Zero);
        quad.AddIndex(0, 1, 2, 0, 1, 3);
        quad.BlendMode = BlendMode.AlphaBlend;

        ctex = Engine.Texture.Create(1, 1, FilterMode.Point, WrapMode.Clamp, new Color[] { Color.White });
        points = new Array<Vertex>(50);
        lines = new Array<Vertex>(50);
        triangles = new Array<Vertex>(50);

        mbuffer = MeshBuffer.Create();
    }

    public override string Title => nameof(Canvas);

    public Vector2 ViewPosition
    {
        get => pos;
        set => pos = value;
    }

    public bool IsInteractable => minside && !Dialog.IsShowing;

    public override void Draw(Gui gui, Rect rect, WindowFlags flags = WindowFlags.Default /* not used */)
    {
        // crect is client rect in screen coord
        crect = rect;
        minside = crect.Contains(Input.MousePosition);

        if (IsInteractable && Input.IsKeyPressed(MouseButton.Middle))
        {
            App.Window.SetCursor(SystemCursor.SizeAll);
            grab = true;
        }

        if (Input.IsKeyReleased(MouseButton.Middle))
        {
            App.Window.SetCursor(SystemCursor.Arrow);
            grab = false;
        }

        if (Input.IsKeyReleased(MouseButton.Left))
        {
            act_id = -1;
        }

        clicked = IsInteractable && Input.IsKeyPressed(MouseButton.Left);

        // convert screen to viewport coord
        rect.Y = App.Window.Size.Y - rect.YMax;

        var gfx = App.Graphics;
        gfx.SetViewport(rect);
        gfx.SetScissor(rect);

        // calculate view matrix
        var size = crect.Size * zoom;
        var proj = Matrix4x4.CreateOrthographic(size.X, size.Y, -1, 1);
        var view = Matrix4x4.CreateFromYawPitchRoll(0, 0, rot);
        view.Translation = new Vector3(pos.X, pos.Y, 0);
        Matrix4x4.Invert(view, out view);
        viewmat = view * proj;
        Matrix4x4.Invert(viewmat, out inv_viewmat);
        gfx.ViewMatrix = viewmat;

        draw_checker();

        // apply zoom
        if (IsInteractable)
            zoom -= Input.MouseScrollDelta.Y * Time.FrameTime * zoom * 4f;

        mpos_w = ScreenToWorld(Input.MousePosition); // transforms pos
        var mpos_wn = ScreenToWorldNormal(Input.MousePosition); // transforms normal
        mdelta_w = (mpos_wn - lm_normal); // calc delta using normal diff

        // apply grab
        if (grab)
        {
            pos -= mdelta_w;

            // continuous grab
            Vector2 mpos = Input.MousePosition;
            bool cgrab = true;
            if (mpos.X < crect.X + 10)
                mpos.X = crect.XMax - 10;
            else if (mpos.X > crect.XMax - 10)
                mpos.X = crect.X + 10;
            else if (mpos.Y < crect.Y + 10)
                mpos.Y = crect.YMax - 10;
            else if (mpos.Y > crect.YMax - 10)
                mpos.Y = crect.Y + 10;
            else
                cgrab = false;

            if (cgrab)
                Input.MousePosition = mpos;
        }
        lm_normal = ScreenToWorldNormal(Input.MousePosition);

        Editable.Active?.OnDrawCanvas(Instance);

        draw_all_shapes();
        nxt_id = 0;
    }

    public bool PointHandle(ref Vector2 pos, Color c)
    {
        var id = nxt_id++;
        if (clicked && (WorldToScreen(pos) - Input.MousePosition).Length() < pnt_sz * 1.2f)
        {
            clicked = false;
            act_id = id;
        }

        DrawPoint(pos, c);
        if (act_id == id)
            pos += mdelta_w;

        return act_id == id;
    }

    public bool SelectRect(Rect rect, Color c)
    {
        var c2 = c; c2.A /= 3;
        DrawFillRect(rect, c2);
        DrawRect(rect, c);

        if (clicked && rect.Contains(mpos_w))
            return !(clicked = false);
        
        return false;
    }

    public bool RectHandle(ref Rect rect, Color c)
    {
        var ct = c; ct.A /= 3;

        Vector2 tmp = rect.XMinYMin;
        PointHandle(ref tmp, c);
        rect.XMinYMin = tmp;

        tmp = rect.XMaxYMax;
        PointHandle(ref tmp, c);
        rect.XMaxYMax = tmp;

        tmp = rect.XMinYMax;
        PointHandle(ref tmp, c);
        rect.XMinYMax = tmp;

        tmp = rect.XMaxYMin;
        PointHandle(ref tmp, c);
        rect.XMaxYMin = tmp;

        tmp = new Vector2(rect.XMin, rect.YHalf);
        PointHandle(ref tmp, c);
        rect.XMin = tmp.X;

        tmp = new Vector2(rect.XMax, rect.YHalf);
        PointHandle(ref tmp, c);
        rect.XMax = tmp.X;

        tmp = new Vector2(rect.XHalf, rect.YMin);
        PointHandle(ref tmp, c);
        rect.YMin = tmp.Y;

        tmp = new Vector2(rect.XHalf, rect.YMax);
        PointHandle(ref tmp, c);
        rect.YMax = tmp.Y;

        var id = nxt_id++;
        if (clicked && rect.Contains(mpos_w))
        {
            clicked = false;
            act_id = id;
        }

        DrawRect(rect, c);
        DrawFillRect(rect, ct);
        if (act_id == id)
        {
            rect.Position += mdelta_w;
            return true;
        }
        else
        {
            return false;
        }
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

    public void DrawPoint(Vector2 pos, Color c)
    {
        points.Push(new Vertex(pos, Vector2.Zero, c));
    }

    public void DrawCircle(Vector2 pos, float radius, Color c)
    {
        if (circle == null)
        {
            int segments = 32;
            circle = new Vector2[segments];
            for (int i = 0; i < segments; i++)
            {
                float angle = i;
                angle = angle.Remap(0, segments, 0, MathF.PI * 2);
                circle[i] = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            }
        }

        var last_idx = circle.Length - 1;
        for (int i = 0; i < circle.Length - 1; i++)
        {
            lines.Push(new Vertex(circle[i + 0] * radius + pos, Vector2.Zero, c));
            lines.Push(new Vertex(circle[i + 1] * radius + pos, Vector2.Zero, c));
        }
        lines.Push(new Vertex(circle[0] * radius + pos, Vector2.Zero, c));
        lines.Push(new Vertex(circle[last_idx] * radius + pos, Vector2.Zero, c));
    }


    public void DrawLine(Vector2 start, Vector2 end, Color c)
    {
        lines.Push(new Vertex(start, Vector2.Zero, c));
        lines.Push(new Vertex(end, Vector2.Zero, c));
    }

    public void DrawRect(Rect rect, Color c)
    {
        var v1 = rect.XMinYMin;
        var v2 = rect.XMinYMax;
        var v3 = rect.XMaxYMax;
        var v4 = rect.XMaxYMin;

        lines.Push(new Vertex(v1, Vector2.Zero, c));
        lines.Push(new Vertex(v2, Vector2.Zero, c));

        lines.Push(new Vertex(v2, Vector2.Zero, c));
        lines.Push(new Vertex(v3, Vector2.Zero, c));

        lines.Push(new Vertex(v3, Vector2.Zero, c));
        lines.Push(new Vertex(v4, Vector2.Zero, c));

        lines.Push(new Vertex(v4, Vector2.Zero, c));
        lines.Push(new Vertex(v1, Vector2.Zero, c));
    }

    public void DrawFillRect(Rect rect, Color c)
    {
        var v1 = rect.XMinYMin;
        var v2 = rect.XMinYMax;
        var v3 = rect.XMaxYMax;
        var v4 = rect.XMaxYMin;

        triangles.Push(new Vertex(v1, Vector2.Zero, c));
        triangles.Push(new Vertex(v3, Vector2.Zero, c));
        triangles.Push(new Vertex(v2, Vector2.Zero, c));

        triangles.Push(new Vertex(v1, Vector2.Zero, c));
        triangles.Push(new Vertex(v3, Vector2.Zero, c));
        triangles.Push(new Vertex(v4, Vector2.Zero, c));
    }

    public Vector2 ScreenToWorld(Vector2 value)
    {
        return Vector2.Transform(screen_to_ndc(value), inv_viewmat);
    }

    public Vector2 ScreenToWorldNormal(Vector2 value)
    {
        return Vector2.TransformNormal(screen_to_ndc(value), inv_viewmat);
    }

    public Vector2 WorldToScreen(Vector2 value)
    {
        return ndc_to_screen(Vector2.Transform(value, viewmat));
    }

    public Vector2 WorldToScreenNormal(Vector2 value)
    {
        return ndc_to_screen(Vector2.TransformNormal(value, viewmat));
    }

    Vector2 ndc_to_screen(Vector2 value)
    {
        value.X = value.X * .5f + .5f;
        value.Y = -value.Y * .5f + .5f;
        value *= crect.Size;
        value += crect.Position;
        return value;
    }

    Vector2 screen_to_ndc(Vector2 value)
    {
        value = ((value - crect.Position) / crect.Size);
        value.X = (value.X * 2 - 1);
        value.Y = -(value.Y * 2 - 1);
        return value;
    }

    void draw_checker()
    {
        var gfx = App.Graphics;
        var p1 = Vector2.Transform(new Vector2(-1, -1), inv_viewmat);
        var p2 = Vector2.Transform(new Vector2(1, 1), inv_viewmat);
        var p3 = Vector2.Transform(new Vector2(-1, 1), inv_viewmat);
        var p4 = Vector2.Transform(new Vector2(1, -1), inv_viewmat);
        float tcoord_mult = (1 / zoom) * 0.05f;
        quad.SetVertex(0, new Vertex(p1, p1 * tcoord_mult, Color.White));
        quad.SetVertex(1, new Vertex(p2, p2 * tcoord_mult, Color.White));
        quad.SetVertex(2, new Vertex(p3, p3 * tcoord_mult, Color.White));
        quad.SetVertex(3, new Vertex(p4, p4 * tcoord_mult, Color.White));
        quad.MainTexture = Engine.Texture.Load("editor/checker.png");
        quad.Draw();
    }

    public void draw_all_shapes()
    {
        var gfx = App.Graphics;
        gfx.SetPointSize(pnt_sz);
        gfx.SetLineWidth(1);
        gfx.SetBlendMode(BlendMode.AlphaBlend);
        var shader = DefaultShaders.ColorMult;
        gfx.SetShader(shader);
        shader.SetTexture("main_tex", 0, ctex);
        shader.SetMatrix3x2("model_mat", Matrix3x2.Identity);
        shader.SetMatrix4x4("view_mat", gfx.ViewMatrix);

        // draw triangles
        triangles.Reverse();
        mbuffer.UpdateVertices(triangles);
        mbuffer.Draw(PrimitiveType.Triangles);

        // draw lines
        lines.Reverse();
        mbuffer.UpdateVertices(lines);
        mbuffer.Draw(PrimitiveType.Lines);

        // draw points
        points.Reverse();
        mbuffer.UpdateVertices(points);
        mbuffer.Draw(PrimitiveType.Points);

        points.Clear(false);
        lines.Clear(false);
        triangles.Clear(false);
    }
}