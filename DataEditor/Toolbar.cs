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
        var sheet = SpriteSheet.Load("editor/icons.spr");
        new_frame = sheet["New"];
        save_frame = sheet["Save"];
    }

    public override string Title => nameof(Toolbar);

    public override void Draw(Gui gui, Rect rect, WindowFlags flags = WindowFlags.NoCollapse |
                                                                      WindowFlags.NoMove |
                                                                      WindowFlags.NoResize |
                                                                      WindowFlags.NoTitleBar |
                                                                      WindowFlags.MenuBar)
    {
        gui.PushStyleVar(StyleVar.WindowPadding, new Vector2(6, 0));
        base.Draw(gui, rect, flags);
        gui.PopStyleVar();
    }

    protected override void OnGui(Gui gui)
    {
        if (gui.BeginMenuBar())
        {
            if (gui.BeginMenu("File"))
            {
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
            ContextMenu.Begin();
            var types = Editable.GetDataTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var item = (DataKind)i;
                void on_click_item()
                {
                    if (Editable.Active == null)
                        // new immediately !
                        Editable.Active = Activator.CreateInstance(Editable.DataKindToEditorType(item)) as Editable;
                    else
                        // new after save  !
                        Editable.Active.TrySave(() => Editable.Active = Activator.CreateInstance(Editable.DataKindToEditorType(item)) as Editable);
                };

                ContextMenu.AddItem(types[i].Item1.ToString(), on_click_item);
            }
            ContextMenu.Show();

            gui.EndPopup();
        }

        gui.SameLine(0, 0);
        if (gui.ImageButton(save_frame))
            Editable.Active?.TrySave(null);

        gui.PopStyleColor();
    }


}