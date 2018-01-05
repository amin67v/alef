using System;
using System.IO;
using System.Numerics;

using Engine;

public class DataBrowser
{
    string current_path;
    string[] filter;
    Array<DirectoryInfo> folders = new Array<DirectoryInfo>();
    Array<FileInfo> files = new Array<FileInfo>();
    string target_path = null;

    public DataBrowser()
    {
        filter = new string[]
        {
            ".spr",
            ".tex",
            ".shp",
            ".prt",
            ".ent"
        };
        
        ChangePath(App.ExePath);
    }

    public void Draw()
    {
        var gui = App.Gui;

        if (target_path != null)
        {
            ChangePath(target_path);
            target_path = null;
        }

        gui.SetNextWindowPos(Vector2.Zero, GuiCondition.Always);
        gui.SetNextWindowSize(new Vector2(250, App.Window.Size.Y), GuiCondition.Always);

        gui.BeginWindow("Data Browser", WindowFlags.NoCollapse | WindowFlags.NoMove | WindowFlags.NoResize);

        if (gui.Button("Back", new Vector2(80, 20)))
        {
            if (current_path != App.ExePath)
                target_path = Directory.GetParent(current_path).FullName;
        }

        gui.SameLine();
        if (gui.Button("New", new Vector2(80, 20)))
            gui.OpenPopup("new_data");

        if (gui.BeginPopup("new_data"))
        {
            gui.Selectable("Sprite Sheet");
            gui.Selectable("Texture");
            gui.Selectable("Shape2D");
            gui.EndPopup();
        }

        gui.PushStyleVar(StyleVar.ButtonTextAlign, new Vector2(0, .5f));
        gui.PushStyleColor(ColorTarget.Text, new Color(20, 20, 20, 255));
        gui.PushStyleColor(ColorTarget.Button, new Color(240, 190, 90, 255));
        gui.PushStyleColor(ColorTarget.ButtonHovered, new Color(245, 220, 160, 255));
        gui.PushStyleColor(ColorTarget.ButtonActive, new Color(250, 230, 180, 255));

        for (int i = 0; i < folders.Count; i++)
        {
            if (gui.Button(folders[i].Name, new Vector2(220, 20)))
                target_path = folders[i].FullName;
        }

        gui.PopStyleColor(3);

        gui.PushStyleColor(ColorTarget.Button, new Color(200, 200, 200, 255));
        gui.PushStyleColor(ColorTarget.ButtonHovered, new Color(200, 200, 200, 200));
        gui.PushStyleColor(ColorTarget.ButtonActive, new Color(200, 200, 200, 150));

        for (int i = 0; i < files.Count; i++)
        {
            gui.Button(files[i].Name, new Vector2(220, 20));
        }

        gui.PopStyleColor(4);
        gui.PopStyleVar();

        gui.EndWindow();
    }

    public void ChangePath(string path)
    {
        current_path = path;
        folders.Clear();
        var folder_paths = Directory.GetDirectories(path);
        for (int i = 0; i < folder_paths.Length; i++)
            folders.Push(new DirectoryInfo(folder_paths[i]));

        files.Clear();
        var file_paths = Directory.GetFiles(path);
        for (int i = 0; i < file_paths.Length; i++)
        {
            var item = file_paths[i];
            if (is_known_file(item))
                files.Push(new FileInfo(item));
        }
    }

    bool is_known_file(string file)
    {
        for (int i = 0; i < filter.Length; i++)
        {
            if (file.EndsWith(filter[i]))
                return true;
        }
        return false;
    }
}