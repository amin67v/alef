using System;
using System.IO;
using System.Numerics;
using Process = System.Diagnostics.Process;

using Engine;

public class Browser
{
    static readonly Browser instance = new Browser();

    FileSystemWatcher watcher;
    string current_path;
    string target_path = null;
    Array<DirectoryInfo> folders = new Array<DirectoryInfo>();
    Array<FileInfo> files = new Array<FileInfo>();
    float width = 250;

    Browser()
    {
        watcher = new FileSystemWatcher(App.ExePath);
        watcher.EnableRaisingEvents = true;
        watcher.Created += file_changed;
        watcher.Deleted += file_changed;
        watcher.Renamed += file_renamed;

        ChangePath("");
    }

    public static ref float Width => ref instance.width;

    public static void Draw() => instance.draw();

    void draw()
    {
        var gui = App.Gui;

        if (target_path != null)
        {
            ChangePath(target_path);
            target_path = null;
        }

        gui.SetNextWindowPos(Vector2.Zero, GuiCondition.Always);
        gui.SetNextWindowSize(new Vector2(Width, App.Window.Size.Y), GuiCondition.Always);

        gui.BeginWindow("Browser", WindowFlags.NoCollapse | WindowFlags.NoMove | WindowFlags.NoResize);

        if (gui.Button("Open In Explorer"))
        {
            try { Process.Start("explorer.exe", App.GetAbsolutePath(current_path)); } catch { }
        }

        gui.SameLine();
        if (gui.Button("New"))
            gui.OpenPopup("new_data");

        if (gui.BeginPopup("new_data"))
        {
            int count = (int)EditableType.Count;
            for (int i = 0; i < count; i++)
            {
                var item = (EditableType)i;
                if (gui.Selectable(item.ToString()))
                    Editable.Active = Activator.CreateInstance(Editable.GetTypeFor(item)) as Editable;

            }
            gui.EndPopup();
        }

        if (gui.Button("Back", new Vector2(80, 20)))
        {
            if (!string.IsNullOrWhiteSpace(current_path))
            {
                App.Log.Debug("current : " + current_path);
                target_path = Path.GetRelativePath(App.ExePath, Directory.GetParent(current_path).FullName);
                App.Log.Debug("target : " + target_path);
            }
        }

        gui.PushStyleVar(StyleVar.ButtonTextAlign, new Vector2(0, .5f));
        gui.PushStyleColor(ColorTarget.Text, new Color(20, 20, 20, 255));
        gui.PushStyleColor(ColorTarget.Button, new Color(240, 190, 90, 255));
        gui.PushStyleColor(ColorTarget.ButtonHovered, new Color(245, 220, 160, 255));
        gui.PushStyleColor(ColorTarget.ButtonActive, new Color(250, 230, 180, 255));

        for (int i = 0; i < folders.Count; i++)
        {
            if (gui.Button(folders[i].Name, new Vector2(width - 30, 20)))
                target_path = folders[i].FullName;
        }

        gui.PopStyleColor(3);

        gui.PushStyleColor(ColorTarget.Button, new Color(200, 200, 200, 255));
        gui.PushStyleColor(ColorTarget.ButtonHovered, new Color(200, 200, 200, 200));
        gui.PushStyleColor(ColorTarget.ButtonActive, new Color(200, 200, 200, 150));

        for (int i = 0; i < files.Count; i++)
        {
            gui.Button(files[i].Name, new Vector2(width - 30, 20));
        }

        gui.PopStyleColor(4);
        gui.PopStyleVar();

        gui.EndWindow();
    }

    public void ChangePath(string path)
    {
        current_path = path;
        var fullpath = App.GetAbsolutePath(path);
        folders.Clear();
        var folder_paths = Directory.GetDirectories(fullpath);
        for (int i = 0; i < folder_paths.Length; i++)
            folders.Push(new DirectoryInfo(folder_paths[i]));

        files.Clear();
        var file_paths = Directory.GetFiles(fullpath);
        for (int i = 0; i < file_paths.Length; i++)
        {
            var item = file_paths[i];
            if (is_known_file(item))
                files.Push(new FileInfo(item));
        }
    }

    void file_changed(object sender, FileSystemEventArgs e)
    {
        ChangePath(current_path);
    }

    void file_renamed(object sender, RenamedEventArgs e)
    {
        ChangePath(current_path);
    }

    bool is_known_file(string file)
    {
        for (int i = 0; i < (int)EditableType.Count; i++)
        {
            var item = (EditableType)i;
            var ext = Editable.GetExtensionFor(item);
            if (file.EndsWith(ext))
                return true;
        }
        return false;
    }
}