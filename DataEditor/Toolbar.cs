using System;
using System.IO;
using System.Numerics;
using Process = System.Diagnostics.Process;

using Engine;

public class Toolbar : Panel
{
    public static readonly Toolbar Instance = new Toolbar();

    SpriteSheetFrame new_frame, save_frame, zoom_frame;

    Toolbar()
    {
        new_frame = DataEditor.Icons["New"];
        save_frame = DataEditor.Icons["Save"];
        zoom_frame = DataEditor.Icons["Zoom"];
    }

    public override string Title => nameof(Toolbar);

    public override void Draw(Gui gui, Rect rect, WindowFlags flags = WindowFlags.NoCollapse |
                                                                      WindowFlags.NoMove |
                                                                      WindowFlags.NoResize |
                                                                      WindowFlags.NoTitleBar)
    {
        gui.PushStyleVar(StyleVar.WindowPadding, new Vector2(2, 4));
        gui.PushStyleVar(StyleVar.WindowMinSize, Vector2.One);
        base.Draw(gui, rect, flags);
        gui.PopStyleVar(2);
    }

    protected override void OnGui(Gui gui)
    {
        //if (gui.BeginMenuBar())
        //{
        //    if (gui.BeginMenu("File"))
        //    {
        //        if (gui.BeginMenu("New"))
        //        {
        //            var types = Editable.GetDataTypes();
        //            for (int i = 0; i < types.Length; i++)
        //            {
        //                var item = (DataKind)i;
        //                if (gui.MenuItem(types[i].Item1.ToString()))
        //                    new_data(item);
        //            }
        //            gui.EndMenu();
        //        }
        //
        //        if (gui.MenuItem("Save"))
        //            save_active();
        //
        //        gui.Separator();
        //        if (gui.MenuItem("Quit"))
        //            App.Quit();
        //
        //        gui.EndMenu();
        //    }
        //    gui.EndMenuBar();
        //}
        gui.PushStyleColor(ColorTarget.Button, Color.Transparent);

        if (gui.ImageButton(new_frame, 1))
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
        if (gui.ImageButton(save_frame, 1))
            save_active();

        gui.SameLine(0, 40);
        int zoom = (int)((1f / Canvas.Instance.ViewSize) * 100);
        gui.PushItemWidth(100);
        gui.DragInt(string.Empty, ref zoom, 1, 0, 5000, "%.0f %%");
        gui.PopItemWidth();
        Canvas.Instance.ViewSize = 1f / (zoom / 100f);
        gui.SameLine(0, 0);
        gui.Image(zoom_frame);

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