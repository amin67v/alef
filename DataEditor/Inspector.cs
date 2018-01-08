using System;
using System.IO;
using System.Numerics;

using Engine;

public class Inspector
{
    static readonly Inspector instance = new Inspector();
    
    float width = 320;

    public static ref float Width => ref instance.width;

    public static void Draw()
    {
        var gui = App.Gui;

        var screen = App.Window.Size;
        gui.SetNextWindowPos(new Vector2(screen.X - Width, 0), GuiCondition.Always);
        gui.SetNextWindowSize(new Vector2(Width, App.Window.Size.Y), GuiCondition.Always);
        gui.BeginWindow("Inspector", WindowFlags.NoCollapse | WindowFlags.NoMove | WindowFlags.NoResize);
        Editable.Active?.OnDrawInspector();
        gui.EndWindow();
    }
}