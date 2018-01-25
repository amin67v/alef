using System;
using System.IO;
using System.Numerics;
using Process = System.Diagnostics.Process;

using Engine;

public class Toolbar : Panel
{
    public static readonly Toolbar Instance = new Toolbar();

    SpriteSheetFrame new_frame, save_frame;

    Toolbar()
    {
        new_frame = DataEditor.Icons["New"];
        save_frame = DataEditor.Icons["Save"];
    }

    public override string Title => nameof(Toolbar);

    public override void Draw(Gui gui, Rect rect, WindowFlags flags = WindowFlags.NoCollapse |
                                                                      WindowFlags.NoMove |
                                                                      WindowFlags.NoResize |
                                                                      WindowFlags.NoTitleBar |
                                                                      WindowFlags.MenuBar)
    {
        gui.PushStyleVar(StyleVar.WindowPadding, new Vector2(6, 2));
        base.Draw(gui, rect, flags);
        gui.PopStyleVar();
    }

    protected override void OnGui(Gui gui)
    {
        if (gui.BeginMenuBar())
        {
            if (gui.BeginMenu("File"))
            {
                if (gui.BeginMenu("New"))
                {
                    var types = Editable.GetDataTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        var item = (DataKind)i;
                        if (gui.MenuItem(types[i].Item1.ToString()))
                            new_data(item);
                    }
                    gui.EndMenu();
                }

                if (gui.MenuItem("Save"))
                    save_active();

                gui.Separator();
                if (gui.MenuItem("Quit"))
                    App.Quit();

                gui.EndMenu();
            }
            gui.EndMenuBar();
        }


        gui.PushStyleColor(ColorTarget.Button, Color.Transparent);

        if (gui.ImageButton(new_frame))
            gui.OpenPopup("new_data");

        if (gui.BeginPopup("new_data"))
        {
            var types = Editable.GetDataTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var item = (DataKind)i;
                if (gui.Selectable(types[i].Item1.ToString()))
                    new_data(item);
            }
            gui.EndPopup();
        }

        gui.SameLine(0, 0);
        if (gui.ImageButton(save_frame))
            save_active();

        gui.PopStyleColor();
    }

    void new_data(DataKind item)
    {
        if (Editable.Active == null)
            // new immediately !
            Editable.Active = Activator.CreateInstance(Editable.DataKindToEditorType(item)) as Editable;
        else
            // new after save  !
            Editable.Active.Save(() => Editable.Active = Activator.CreateInstance(Editable.DataKindToEditorType(item)) as Editable, true);
    }

    void save_active()
    {
        Editable.Active?.Save();
    }

}