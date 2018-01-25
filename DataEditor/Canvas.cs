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
    float vzoom = 1f;
    float vrot;
    Vector2 vpos;
    Vector2 mpos_w;
    Vector2 lm_normal; // last frame
    Vector2 mdelta_w;
    Vector2[] circle;
    Rect vpos_limit;
    Rect client_rect;
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
    bool hovered;

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
        Cursor.Set(CursorMode.Arrow);
    }

    public override string Title => nameof(Canvas);

    public Vector2 ViewPosition
    {
        get => vpos;
        set 
        {
            vpos = value;
            vpos.X = MathF.Min(vpos.X, vpos_limit.XMax);
            vpos.X = MathF.Max(vpos.X, vpos_limit.XMin);
            vpos.Y = MathF.Min(vpos.Y, vpos_limit.YMax);
            vpos.Y = MathF.Max(vpos.Y, vpos_limit.YMin);
        }
    }

    public Rect ViewPositionLimits
    {
        get => vpos_limit;
        set
        {
            vpos_limit = value;
            ViewPosition = ViewPosition; // refresh it !
        }
    }

    public bool IsInteractable => minside && !Dialog.IsShowing;

    public override void Draw(Gui gui, Rect rect, WindowFlags flags = WindowFlags.Default /* not used */)
    {
        // crect is client rect in screen coord
        client_rect = rect;
        minside = client_rect.Contains(Input.MousePosition);

        if (IsInteractable)
        {
            if (Input.IsKeyPressed(MouseButton.Middle))
                grab = true;
        }
        if (Input.IsKeyReleased(MouseButton.Middle))
            grab = false;

        if (Input.IsKeyReleased(MouseButton.Left))
        {
            act_id = -1;
        }

        if (hovered)
        {
            Cursor.Set(CursorMode.Arrow);
            hovered = false;
        }
        clicked = Input.IsKeyPressed(MouseButton.Left);

        // convert screen to viewport coord
        rect.Y = App.Window.Size.Y - rect.YMax;

        var gfx = App.Graphics;
        gfx.SetViewport(rect);
        gfx.SetScissor(rect);

        // calculate view matrix
        var size = client_rect.Size * vzoom;
        var proj = Matrix4x4.CreateOrthographic(size.X, size.Y, -1, 1);
        var view = Matrix4x4.CreateFromYawPitchRoll(0, 0, vrot);
        view.Translation = new Vector3(vpos.X, vpos.Y, 0);
        Matrix4x4.Invert(view, out view);
        viewmat = view * proj;
        Matrix4x4.Invert(viewmat, out inv_viewmat);
        gfx.ViewMatrix = viewmat;

        draw_checker();

        // apply zoom
        if (IsInteractable)
            vzoom -= Input.MouseScrollDelta.Y * Time.FrameTime * vzoom * 4f;

        mpos_w = ScreenToWorld(Input.MousePosition); // transforms pos
        var mpos_wn = ScreenToWorldNormal(Input.MousePosition); // transforms normal
        mdelta_w = (mpos_wn - lm_normal); // calc delta using normal diff

        // apply grab
        if (grab)
        {
            Cursor.Set(CursorMode.Grab, 2000);
            ViewPosition -= mdelta_w;

            // continuous grab
            Vector2 mpos = Input.MousePosition;
            bool cgrab = true;
            if (mpos.X < client_rect.X + 10)
                mpos.X = client_rect.XMax - 10;
            else if (mpos.X > client_rect.XMax - 10)
                mpos.X = client_rect.X + 10;
            else if (mpos.Y < client_rect.Y + 10)
                mpos.Y = client_rect.YMax - 10;
            else if (mpos.Y > client_rect.YMax - 10)
                mpos.Y = client_rect.Y + 10;
            else
                cgrab = false;

            if (cgrab)
                Input.MousePosition = mpos;
        }
        lm_normal = ScreenToWorldNormal(Input.MousePosition);

        Editable.Active?.OnDrawCanvas(gui, Instance);

        draw_all_shapes();
        nxt_id = 0;
    }

    public bool PointHandle(ref Vector2 pos, Color c, CursorMode cursor = CursorMode.Arrow)
    {
        var id = nxt_id++;
        if (IsInteractable && (WorldToScreen(pos) - Input.MousePosition).Length() < pnt_sz * 1.2f) // hover ?
        {
            if (!hovered)
            {
                Cursor.Set(cursor, 1000);
                hovered = true;
            }

            if (clicked)
            {
                clicked = false;
                act_id = id;
            }
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

        if (IsInteractable && rect.Contains(mpos_w))
        {
            if (!hovered)
            {
                Cursor.Set(CursorMode.Select); ;
                hovered = true;
            }

            if (clicked)
            {
                clicked = false;
                return true;
            }
        }

        return false;
    }

    public bool RectHandle(ref Rect rect, Color c)
    {
        var ct = c; ct.A /= 3;

        Vector2 tmp = rect.XMinYMin;
        if (PointHandle(ref tmp, c, CursorMode.SizeS))
            Cursor.Set(CursorMode.SizeS, 1200);

        rect.XMinYMin = tmp;

        tmp = rect.XMaxYMax;
        if (PointHandle(ref tmp, c, CursorMode.SizeS))
            Cursor.Set(CursorMode.SizeS, 1200);
        rect.XMaxYMax = tmp;

        tmp = rect.XMinYMax;
        if (PointHandle(ref tmp, c, CursorMode.SizeB))
            Cursor.Set(CursorMode.SizeB, 1200);
        rect.XMinYMax = tmp;

        tmp = rect.XMaxYMin;
        if (PointHandle(ref tmp, c, CursorMode.SizeB))
            Cursor.Set(CursorMode.SizeB, 1200);
        rect.XMaxYMin = tmp;

        tmp = new Vector2(rect.XMin, rect.YHalf);
        if (PointHandle(ref tmp, c, CursorMode.SizeH))
            Cursor.Set(CursorMode.SizeH, 1200);
        rect.XMin = tmp.X;

        tmp = new Vector2(rect.XMax, rect.YHalf);
        if (PointHandle(ref tmp, c, CursorMode.SizeH))
            Cursor.Set(CursorMode.SizeH, 1200);
        rect.XMax = tmp.X;

        tmp = new Vector2(rect.XHalf, rect.YMin);
        if (PointHandle(ref tmp, c, CursorMode.SizeV))
            Cursor.Set(CursorMode.SizeV, 1200);
        rect.YMin = tmp.Y;

        tmp = new Vector2(rect.XHalf, rect.YMax);
        if (PointHandle(ref tmp, c, CursorMode.SizeV))
            Cursor.Set(CursorMode.SizeV, 1200);
        rect.YMax = tmp.Y;

        var id = nxt_id++;
        if (IsInteractable && rect.Contains(mpos_w))
        {
            if (!hovered)
            {
                Cursor.Set(CursorMode.Move);
                hovered = true;
            }

            if (clicked)
            {
                clicked = false;
                act_id = id;
            }
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
        value *= client_rect.Size;
        value += client_rect.Position;
        return value;
    }

    Vector2 screen_to_ndc(Vector2 value)
    {
        value = ((value - client_rect.Position) / client_rect.Size);
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
        float tcoord_mult = (1 / vzoom) * 0.05f;
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