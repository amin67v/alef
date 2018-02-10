using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

using Engine;

public class DataEditor : Disposable, IAppState
{
    static SpriteSheet icons;

    public static SpriteSheet Icons => icons;

    public void OnEnter()
    {
        void app_quit()
        {
            App.OverrideQuit(null);
            App.Quit();
        }

        App.OverrideQuit(() =>
        {
            if (Editable.Active != null)
                Editable.Active.Save(app_quit, true);
            else
                app_quit();
        });

        App.Window.OnFileDrop += (s) =>
        {
            for (int i = 0; i < s.Length; i++)
            {
                Log.Debug(s[i]);
            }
        };

        icons = SpriteSheet.Load("editor/icons.spr");

        var app_icon = Image.FromFile(App.GetAbsolutePath("editor/data-editor.png"));
        App.Window.SetIcon(app_icon);
        app_icon.Dispose();
    }

    public void OnExit()
    {
        Editable.Active?.OnEnd();
    }

    public void OnFrame()
    {
        var gfx = App.Graphics;
        gfx.Clear(Color.Gray);
        Gui.Render(OnGui);
        gfx.Display();

        if (Input.IsKeyPressed(KeyCode.Space))
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
    }

    public void OnGui(Gui gui)
    {
        Rect remain = new Rect(Vector2.Zero, App.Window.Size);
        var browser = remain; browser.Width = remain.XMin = 250;
        var inspector = remain; inspector.XMin = remain.XMax = (remain.XMax - 320);
        var toolbar = remain; toolbar.Height = remain.YMin = 28;
        var canvas = remain;

        Toolbar.Instance.Draw(gui, toolbar);
        Browser.Instance.Draw(gui, browser);
        Inspector.Instance.Draw(gui, inspector);
        Canvas.Instance.Draw(gui, canvas);
        Dialog.Instance.Draw(gui);
        Cursor.Update(gui);
    }

    void custom_style(Gui gui)
    {
        if (gui.Button("     Save     "))
        {
            string content = "";
            for (int i = 0; i < (int)ColorTarget.Count; i++)
            {
                var target = (ColorTarget)i;
                var c = gui.Style.GetColor(target);
                content += $"style.SetColor(ColorTarget.{target.ToString()}, new Color({(c.R / 255f).ToString("0.00")}f, {(c.G / 255f).ToString("0.00")}f, {(c.B / 255f).ToString("0.00")}f, {(c.A / 255f).ToString("0.00")}f));\n";
            }
            File.WriteAllText(App.GetAbsolutePath("style.txt"), content);
        }

        for (int i = 0; i < (int)ColorTarget.Count; i++)
        {
            var target = (ColorTarget)i;
            var c = gui.Style.GetColor(target);

            gui.ColorEdit(target.ToString(), ref c, ColorEditFlags.Default | ColorEditFlags.AlphaPreview);
            gui.Style.SetColor(target, c);
            gui.SameLine();
            if (gui.Button($"Lighter{i}"))
                gui.Style.SetColor(target, gui.Style.GetColor(target).Lighter(0.1f));
            gui.SameLine();
            if (gui.Button($"Darker{i}"))
                gui.Style.SetColor(target, gui.Style.GetColor(target).Darker(0.1f));
        }

    }
}

public delegate void FileDropHandler(string file);

public delegate void MouseButtonHandler(MouseButton btn, Vector2 pos);
