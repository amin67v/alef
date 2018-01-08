using System;
using System.IO;

using Engine;

public abstract class Editable
{
    static Editable active;
    static Type[] type_map;
    static string[] ext_map;

    InputBuffer filename = new InputBuffer(256);
    bool saved = true;

    public static Editable Active
    {
        get => active;
        set
        {
            active?.Save(true);
            active?.OnEnd();
            active = value;
            active?.OnBegin();
        }
    }

    public string Filename => filename.String;

    public static Type GetTypeFor(EditableType type)
    {
        if (type_map == null)
        {
            type_map = new Type[(int)EditableType.Count];
            type_map[(int)EditableType.SpriteSheet] = typeof(SpritePacker);
            type_map[(int)EditableType.Texture] = null;
            type_map[(int)EditableType.Shape] = null;
            type_map[(int)EditableType.Particle] = null;
            type_map[(int)EditableType.Entity] = null;
        }

        return type_map[(int)type];
    }

    public static string GetExtensionFor(EditableType type)
    {
        if (ext_map == null)
        {
            ext_map = new string[(int)EditableType.Count];
            ext_map[(int)EditableType.SpriteSheet] = ".spr";
            ext_map[(int)EditableType.Texture] = ".tex";
            ext_map[(int)EditableType.Shape] = ".shp";
            ext_map[(int)EditableType.Particle] = ".prt";
            ext_map[(int)EditableType.Entity] = ".ent";
        }

        return ext_map[(int)type];
    }

    protected abstract byte[] Serialize();

    public virtual void OnBegin() { }

    public virtual void OnEnd() { }

    public virtual void OnDrawCanvas(Canvas g) { }

    public virtual void OnDrawInspector()
    {
        var gui = App.Gui;
        InputTextFlags flags = InputTextFlags.Default;
        if (saved)
            flags |= InputTextFlags.ReadOnly;

        gui.InputText(string.Empty, filename, flags);

        gui.SameLine();
        if (gui.Button("Save"))
        {
            Save(false);
            saved = true;
        }
    }

    public void Save(bool ask = false)
    {
        void save_to_file()
        {
            var fullpath = App.GetAbsolutePath(Filename);
            File.WriteAllBytes(fullpath, Serialize());
        }

        if (!string.IsNullOrWhiteSpace(Filename))
        {
            if (ask)
            {
                Modal.ShowDialog($"Do you want to save the edited object to '{Filename} ?'", save_to_file, null);
            }
            else
            {
                save_to_file();
            }
        }
    }
}

public enum EditableType
{
    SpriteSheet,
    Texture,
    Shape,
    Particle,
    Entity,

    Count
}