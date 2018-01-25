using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.InteropServices;

using Engine;

public class TextureEditor : Editable
{
    public override DataKind Type => DataKind.Texture;

    protected override void Deserialize(Stream steam)
    {

    }

    protected override void Serialize(Stream stream)
    {
        
    }
}