using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.InteropServices;

using Engine;

public class SpritePacker : Editable
{
    Array<Frame> frames = new Array<Frame>();
    FilterMode filter;
    WrapMode wrap;
    Image img;
    int ppu = 24;

    int filter_idx;
    int wrap_idx;

    public override void OnDrawInspector()
    {
        base.OnDrawInspector();
        var gui = App.Gui;
        gui.Spacing();
        gui.DragInt("Pixel Per Unit", ref ppu, 1f, 0, 1024, "%.0f");
        gui.Combo<FilterMode>("Filter Mode", ref filter_idx, ref filter);
        gui.Combo<WrapMode>("Wrap Mode", ref wrap_idx, ref wrap);
    }

    public override void OnDrawCanvas(Canvas g)
    {
        var tex = Texture.Load("editor/dungeon_sheet.png");
        g.DrawTexture(tex, new Rect(0, 0, tex.Width * 4, tex.Height * 4));
    }

    public override void OnEnd()
    {
        img?.Dispose();
    }

    protected override byte[] Serialize()
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(0x00737072); // sign
        writer.Write(1); // for now version should always be 1
        writer.Write(img.Width);
        writer.Write(img.Height);
        writer.Write(ppu);
        writer.Write((int)filter);
        writer.Write((int)wrap);

        for (int i = 0; i < 13; i++)
            writer.Write(0); // not used

        writer.Write(frames.Count);
        for (int i = 0; i < frames.Count; i++)
        {
            var f = frames[i];
            writer.Write(f.Name);

            writer.Write(f.Rect.X);
            writer.Write(f.Rect.Y);
            writer.Write(f.Rect.Width);
            writer.Write(f.Rect.Height);

            writer.Write(f.Offset.X);
            writer.Write(f.Offset.Y);
        }

        // compress img data into output stream
        MemoryStream output = new MemoryStream();
        DeflateStream deflate = new DeflateStream(output, CompressionLevel.Optimal, true);
        byte[] img_data = new byte[img.Width * img.Height * 4];
        Marshal.Copy(img.PixelData, img_data, 0, img_data.Length);
        deflate.Write(img_data, 0, img_data.Length);
        deflate.Dispose();
        // write compressed length and data into stream
        var comp_tex_data = output.ToArray();
        writer.Write(comp_tex_data.Length);
        writer.Write(comp_tex_data, 0, comp_tex_data.Length);
        output.Dispose();

        var arr = stream.ToArray();
        stream.Dispose();
        return arr;
    }

    struct Frame
    {
        public string Name;
        public Rect Rect;
        public Vector2 Offset;
    }
}