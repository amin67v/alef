using System;
using System.IO;
using System.Numerics;

using Engine;

public abstract class Editable
{
    static Editable active;
    static (DataKind, string, Type)[] data_types;

    string data_path;

    public static Editable Active
    {
        get => active;
        set
        {
            active?.OnEnd();
            active = value;
            active?.OnBegin();
        }
    }
    public string DataPath => data_path;

    public abstract DataKind Type { get; }

    public static (DataKind, string, Type)[] GetDataTypes()
    {
        if (data_types == null)
        {
            data_types = new(DataKind, string, Type)[]
            {
                (DataKind.SpriteSheet, ".spr", typeof(SpritePacker)),
                (DataKind.Texture, ".tex", null),
                (DataKind.Shape, ".shp", null),
                (DataKind.Particle, ".prt", null),
                (DataKind.Entity, ".ent", null),
            };
        }
        return data_types;
    }


    public static void Save(Editable obj, string path)
    {
        string fullpath = App.GetAbsolutePath(path);
        var stream = File.OpenWrite(fullpath);
        try
        {
            obj.Serialize(stream);
        }
        finally
        {
            stream.Dispose();
        }
    }

    public static Editable Load(string path)
    {
        var ext = Path.GetExtension(path);
        var fullpath = App.GetAbsolutePath(path);
        var stream = File.OpenRead(fullpath);
        var obj = Activator.CreateInstance(ExtensionToEditorType(ext)) as Editable;
        obj.data_path = path;
        obj.Deserialize(stream);
        stream.Dispose();
        return obj;
    }

    public static string DataKindToExtension(DataKind kind)
    {
        var types = GetDataTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].Item1 == kind)
                return types[i].Item2;
        }

        return null;
    }

    public static Type DataKindToEditorType(DataKind kind)
    {
        var types = GetDataTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].Item1 == kind)
                return types[i].Item3;
        }

        return null;
    }

    public static Type ExtensionToEditorType(string ext)
    {
        var types = GetDataTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].Item2 == ext)
                return types[i].Item3;
        }

        return null;
    }

    public static DataKind ExtensionToDataKind(string ext)
    {
        var types = GetDataTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].Item2 == ext)
                return types[i].Item1;
        }

        return (DataKind)(-1);
    }

    public static bool IsKnownExtension(string ext)
    {
        var types = GetDataTypes();
        
        for (int i = 0; i < types.Length; i++)
            if (types[i].Item2 == ext)
                return true;

        return false;
    }

    protected abstract void Deserialize(Stream steam);

    protected abstract void Serialize(Stream stream);

    public virtual void OnBegin() { }

    public virtual void OnEnd() { }

    public virtual void OnFileDrop(string file) { }

    public virtual void OnMouseDown(MouseButton btn, Vector2 pos) { }

    public virtual void OnMouseUp(MouseButton btn, Vector2 pos) { }

    public virtual void OnDrawCanvas(Canvas g) { }

    public virtual void OnDrawInspector(Gui gui)
    {
        if (DataPath != null)
        {
            gui.Text(DataPath);
            gui.Separator();
        }
    }

    public void TrySave(Action done)
    {
        if (DataPath == null)
        {
            Dialog.UserInput("Some changes have not been saved.\nEnter name for the file to save data into:",
            () =>
            {
                var path = Path.Combine(Browser.Instance.CurrentPath, Dialog.UserText);
                path = Path.ChangeExtension(path, Editable.DataKindToExtension(Type));
                Editable.Save(this, path);
                done?.Invoke();
            }, done);
        }
        else
        {
            Editable.Save(this, DataPath);
            done?.Invoke();
        }

    }

}

public enum DataKind
{
    SpriteSheet,
    Texture,
    Shape,
    Particle,
    Entity
}