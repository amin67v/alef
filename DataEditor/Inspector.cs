using System;
using System.IO;
using System.Numerics;

using Engine;

public class Inspector : Panel
{
    public static readonly Inspector Instance = new Inspector();

    Inspector() { }

    public override string Title => nameof(Inspector);

    protected override void OnGui(Gui gui)
    {
        Editable.Active?.OnDrawInspector(gui);
    }
}