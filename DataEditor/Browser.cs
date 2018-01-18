using System;
using System.IO;
using System.Numerics;
using Process = System.Diagnostics.Process;

using Engine;

public class Browser : Panel
{
    public static readonly Browser Instance = new Browser();

    InputBuffer current_path_buffer;
    FileSystemWatcher watcher;
    //string current_path;
    InputBuffer current_path = new InputBuffer(256);
    string target_path = null;
    Array<DirectoryInfo> folders = new Array<DirectoryInfo>();
    Array<FileInfo> files = new Array<FileInfo>();

    SpriteSheetFrame opendir_frame, backdir_frame;

    Browser()
    {
        watcher = new FileSystemWatcher(App.ExePath);
        watcher.EnableRaisingEvents = true;
        watcher.Created += file_changed;
        watcher.Deleted += file_changed;
        watcher.Renamed += file_renamed;

        var sheet = SpriteSheet.Load("editor/icons.spr");
        backdir_frame = sheet["BackDirectory"];
        opendir_frame = sheet["OpenDirectory"];

        change_path(string.Empty);
    }

    public override string Title => nameof(Browser);

    public string CurrentPath => current_path.String;

    protected override void OnGui(Gui gui)
    {
        var cpath = CurrentPath;

        if (target_path != null)
        {
            change_path(target_path);
            target_path = null;
        }

        // Back directory
        gui.PushStyleColor(ColorTarget.Button, Color.Transparent);
        if (gui.ImageButton(backdir_frame))
        {
            if (!string.IsNullOrWhiteSpace(cpath))
            {
                var idx = cpath.LastIndexOf(Path.DirectorySeparatorChar);
                if (idx == -1)
                    target_path = "";
                else
                    target_path = cpath.Substring(0, idx);
            }
        }
        if (gui.IsLastItemHovered())
            gui.SetTooltip("Back");

        gui.SameLine(0, 0);
        gui.InputText(string.Empty, current_path, InputTextFlags.ReadOnly);
        gui.SameLine();

        // Open directory
        gui.PushID("OpenDirectory");
        if (gui.ImageButton(opendir_frame))
        {
            try
            {
                Process.Start("explorer.exe", App.GetAbsolutePath(cpath));
            }
            catch
            {
                Log.Error($"Failed to open directory '{App.GetAbsolutePath(cpath)}'");
            }
        }
        gui.PopID();

        if (gui.IsLastItemHovered())
            gui.SetTooltip("Open Directory");

        gui.PopID();
        gui.PopStyleColor();

        gui.BeginChild("FilesAndFolders", true, WindowFlags.Default);
        {
            string tooltip = null;
            gui.PushStyleVar(StyleVar.ButtonTextAlign, new Vector2(0, .5f));
            gui.PushStyleColor(ColorTarget.Text, new Color(20, 20, 20, 255));
            gui.PushStyleColor(ColorTarget.Button, new Color(240, 190, 90, 255));
            gui.PushStyleColor(ColorTarget.ButtonHovered, new Color(245, 220, 160, 255));
            gui.PushStyleColor(ColorTarget.ButtonActive, new Color(250, 230, 180, 255));

            for (int i = 0; i < folders.Count; i++)
            {
                if (gui.Button(folders[i].Name, new Vector2(-1, 0)))
                    target_path = Path.Combine(cpath, folders[i].Name);
            }

            gui.PopStyleColor(3);

            gui.PushStyleColor(ColorTarget.Button, new Color(200, 200, 200, 255));
            gui.PushStyleColor(ColorTarget.ButtonHovered, new Color(200, 200, 200, 200));
            gui.PushStyleColor(ColorTarget.ButtonActive, new Color(200, 200, 200, 150));

            for (int i = 0; i < files.Count; i++)
            {
                if (gui.Button(files[i].Name, new Vector2(-1, 0)))
                {
                    var rpath = Path.GetRelativePath(App.ExePath, files[i].FullName);
                    if (Editable.Active == null)
                        Editable.Active = Editable.Load(rpath);
                    else
                        Editable.Active.TrySave(() => Editable.Active = Editable.Load(rpath));
                }
                if (gui.IsLastItemHovered())
                    tooltip = get_file_tooltip(files[i]);
            }

            gui.PopStyleColor(4);
            gui.PopStyleVar();

            if (tooltip != null)
                gui.SetTooltip(tooltip);
        }
        gui.EndChild();
    }

    void change_path(string path)
    {
        current_path.String = path;
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
            if (Editable.IsKnownExtension(Path.GetExtension(item)))
                files.Push(new FileInfo(item));
        }
    }

    void file_changed(object sender, FileSystemEventArgs e)
    {
        change_path(CurrentPath);
    }

    void file_renamed(object sender, RenamedEventArgs e)
    {
        change_path(CurrentPath);
    }

    string get_file_tooltip(FileInfo file)
    {
        return $"Data type: {Editable.ExtensionToDataKind(file.Extension)}\nSize: {(file.Length / 1024f).ToString("0.00")}kb";
    }

}