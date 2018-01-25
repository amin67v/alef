using System;
using System.Numerics;

using Engine;

public static class Cursor
{
    static Array<IntPtr> cursors = new Array<IntPtr>();
    static CursorMode cmode = CursorMode.Arrow;
    static int cpriority = 0;

    static Cursor()
    {
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/arrow.png")), new Vector2(1, 1)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/grab.png")), new Vector2(8, 8)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/select.png")), new Vector2(4, 4)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/move.png")), new Vector2(12, 12)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/size_h.png")), new Vector2(12, 5)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/size_v.png")), new Vector2(5, 12)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/size_s.png")), new Vector2(9, 9)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/size_b.png")), new Vector2(9, 9)));
        cursors.Push(App.Window.CreateCursor(Image.FromFile(App.GetAbsolutePath("editor/cursors/text.png")), new Vector2(5, 11)));
    }

    public static void Set(CursorMode mode, int priority = 1000)
    {
        if (cpriority <= priority)
        {
            cpriority = priority;
            cmode = mode;
        }
    }

    public static void Update(Gui gui)
    {
        var gui_curosr = gui.MouseCursor;

        switch (gui_curosr)
        {
            case MouseCursorKind.Arrow:
                Set(CursorMode.Arrow, 100);
                break;
            case MouseCursorKind.TextInput:
                Set(CursorMode.Text, 100);
                break;
            case MouseCursorKind.Move:
                Set(CursorMode.Move, 100);
                break;
            case MouseCursorKind.ResizeNS:
                Set(CursorMode.SizeV, 100);
                break;
            case MouseCursorKind.ResizeEW:
                Set(CursorMode.SizeH, 100);
                break;
            case MouseCursorKind.ResizeNESW:
                Set(CursorMode.SizeS, 100);
                break;
            case MouseCursorKind.ResizeNWSE:
                Set(CursorMode.SizeV, 100);
                break;
        }

        App.Window.SetCursor(cursors[(int)cmode]);
        cpriority = 0;
        cmode = CursorMode.Arrow;
    }
}

public enum CursorMode
{
    Arrow,
    Grab,
    Select,
    Move,
    SizeH,
    SizeV,
    SizeS,
    SizeB,
    Text
}