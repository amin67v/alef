using System;
using System.Collections.Generic;

using Engine;

public abstract class Panel
{
    public abstract string Title { get; }

    public virtual void Draw(Gui gui, Rect rect, WindowFlags flags = WindowFlags.NoCollapse | WindowFlags.NoMove | WindowFlags.NoResize)
    {
        gui.SetNextWindowPos(rect.Position, GuiCondition.Always);
        gui.SetNextWindowSize(rect.Size, GuiCondition.Always);
        gui.BeginWindow(Title, flags);
        OnGui(gui);
        gui.EndWindow();
    }

    protected virtual void OnGui(Gui gui)
    {

    }
}