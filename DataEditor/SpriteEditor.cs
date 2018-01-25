using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.InteropServices;

using Engine;

public class SpriteEditor : Editable
{
    Array<Frame> frames = new Array<Frame>();
    InputBuffer packer_src = new InputBuffer(256);
    InputBuffer image_path = new InputBuffer(256);
    InputBuffer fsearch = new InputBuffer(128);
    SpriteSheetFrame search_frame;
    FilterMode filter;
    WrapMode wrap;
    Image img;
    Texture tex;
    int padding;
    int act_frm = -1;
    int ppu = 24;

    public override DataKind Type => DataKind.SpriteSheet;

    public override void OnDrawInspector(Gui gui)
    {
        base.OnDrawInspector(gui);
        gui.Spacing();

        if (gui.CollapsingHeader("Texture", TreeNodeFlags.DefaultOpen))
        {
            Vector2 tex_size = tex.Size;
            gui.DragVector2("Size", ref tex_size, 0, float.MaxValue, 1, "%.0f", 1);
            gui.DragInt("Pixel Per Unit", ref ppu, .1f, 0, 1024, "%.0f");
            if (gui.Combo<FilterMode>("Filter Mode", ref filter))
                tex.Filter = filter;

            if (gui.Combo<WrapMode>("Wrap Mode", ref wrap))
                tex.Wrap = wrap;

            gui.PushID("image_path");
            gui.InputText(string.Empty, image_path, InputTextFlags.Default);
            gui.PopID();

            gui.SameLine();
            if (gui.Button("Save Image"))
            {
                try
                {
                    img.ToFile(image_path.String, 100);
                }
                catch (Exception e)
                {
                    Dialog.Info($"Failed to save texture to file.\n \n{e.Message}");
                }
            }
        }

        if (gui.CollapsingHeader("Frames", TreeNodeFlags.DefaultOpen))
        {
            gui.Text(frames.Count.ToString());
            gui.SameLine();
            if (gui.InputText(string.Empty, fsearch, InputTextFlags.Default))
                act_frm = -1;

            gui.SameLine();
            gui.Image(search_frame);

            gui.BeginChild("all_frames", new Vector2(0, 100), true, WindowFlags.Default);

            int rm_i = -1;
            var search = fsearch.String.ToLower();

            for (int i = 0; i < frames.Count; i++)
            {
                var name = frames[i].Name.String;
                if (!string.IsNullOrWhiteSpace(search) && !name.ToLower().Contains(search))
                    continue;

                gui.PushID(i);
                if (act_frm == i)
                {
                    if (gui.Button(name, new Vector2(-20, 0), Color.White))
                        act_frm = i;
                }
                else
                {
                    if (gui.Button(name, new Vector2(-20, 0)))
                        act_frm = i;
                }
                gui.PopID();

                gui.PushID(i);
                gui.SameLine(0, 2);
                if (gui.Button("X", new Vector2(20, 0), new Color(240, 80, 100, 255)))
                {
                    act_frm = -1;
                    rm_i = i;
                }
                gui.PopID();

            }
            if (rm_i != -1)
                frames.RemoveAt(rm_i);

            gui.EndChild();

            if (act_frm > -1)
            {
                int i = act_frm;
                gui.InputText("Name", frames[i].Name, InputTextFlags.Default);
                Vector4 rect = frames[i].Rect.ToVector4();
                gui.DragVector4("Rect", ref rect, float.MinValue, float.MaxValue, 1f, "%.0f", 1);
                frames[i].Rect = new Rect(rect);
                gui.DragVector2("Offset", ref frames[i].Offset, float.MinValue, float.MaxValue, .01f, "%.2f", 1);
            }
        }

        if (gui.CollapsingHeader("Packer", TreeNodeFlags.DefaultOpen))
        {
            gui.InputText("Source", packer_src, InputTextFlags.Default);
            gui.DragInt("Padding", ref padding, 1, 0, 100, "%.0f");

            if (gui.Button("Pack", new Vector2(-40, 0)))
                apply_pack();
        }
    }

    public override void OnDrawCanvas(Gui gui, Canvas g)
    {
        var mpos = Input.MousePosition;
        if (Canvas.Instance.IsInteractable)
        {
            if (Input.IsKeyPressed(MouseButton.Right))
                gui.OpenPopup("context_menu");
        }

        if (gui.BeginPopup("context_menu"))
        {
            if (gui.Selectable("Add Frame"))
                add_frame();

            if (act_frm >= 0 && gui.Selectable("Remove Frame"))
                remove_frame();

            gui.EndPopup();
        }

        if (Input.IsKeyPressed(KeyCode.Delete))
            remove_frame();

        Rect img_rect = new Rect(0, 0, tex.Width, tex.Height);
        g.DrawRect(img_rect, new Color(0, 0, 0, 128));
        g.DrawTexture(tex, img_rect);

        if (act_frm >= 0)
        {
            Vector2 offset = (frames[act_frm].Offset * frames[act_frm].Rect.Size) + frames[act_frm].Rect.Position;
            if (g.PointHandle(ref offset, Color.DodgerBlue, CursorMode.Select))
            {
                frames[act_frm].Offset = (offset - frames[act_frm].Rect.Position) / frames[act_frm].Rect.Size;
                Cursor.Set(CursorMode.Move, 1200);
            }

            if (Input.IsKeyReleased(MouseButton.Left))
            {
                frames[act_frm].Rect.X = MathF.Round(frames[act_frm].Rect.X);
                frames[act_frm].Rect.Y = MathF.Round(frames[act_frm].Rect.Y);
                frames[act_frm].Rect.Width = MathF.Round(frames[act_frm].Rect.Width);
                frames[act_frm].Rect.Height = MathF.Round(frames[act_frm].Rect.Height);
            }

            g.RectHandle(ref frames[act_frm].Rect, Color.DodgerBlue);
        }

        for (int i = 0; i < frames.Count; i++)
        {
            if (act_frm != i)
            {
                if (g.SelectRect(frames[i].Rect, Color.White))
                    act_frm = i;
            }
        }
    }

    public override void OnBegin()
    {
        if (img == null)
            ChangeImage(new Image(256, 256, Color.Magenta));

        App.Window.OnFileDrop += on_file_drop;
        search_frame = DataEditor.Icons["Search"];
    }

    public override void OnEnd()
    {
        img?.Dispose();
        tex?.Dispose();

        App.Window.OnFileDrop -= on_file_drop;
    }

    public void ChangeImage(Image value)
    {
        tex?.Dispose();
        img?.Dispose();

        img = value;
        tex = Texture.Create(img, filter, wrap);

        if (img == null)
            Canvas.Instance.ViewPositionLimits = new Rect(0, 0, 0, 0);
        else
            Canvas.Instance.ViewPositionLimits = new Rect(0, 0, img.Width, img.Height);
        
        Canvas.Instance.ViewPosition = Canvas.Instance.ViewPositionLimits.Center;
    }

    protected override void Deserialize(Stream stream)
    {
        BinaryReader reader = new BinaryReader(stream);
        int sign = reader.ReadInt32();
        if (sign != 0x00737072)
            throw new FormatException("Failed to deserialize sprite sheet, Invalid sign");

        int version = reader.ReadInt32();
        if (version != 1) // for now version should always be 1
            throw new FormatException("Failed to deserialize sprite sheet, invalid version");

        int width = reader.ReadInt32();
        int height = reader.ReadInt32();
        int ppu = reader.ReadInt32();
        FilterMode filter = (FilterMode)reader.ReadInt32();
        WrapMode wrap = (WrapMode)reader.ReadInt32();

        for (int i = 0; i < 13; i++)
            reader.ReadInt32(); // not used

        int frame_count = reader.ReadInt32();
        string[] frame_name = new string[frame_count];
        Rect[] frame_rect = new Rect[frame_count];
        Vector2[] frame_offset = new Vector2[frame_count];

        for (int i = 0; i < frame_count; i++)
        {
            frame_name[i] = reader.ReadString();

            frame_rect[i].X = reader.ReadSingle();
            frame_rect[i].Y = reader.ReadSingle();
            frame_rect[i].Width = reader.ReadSingle();
            frame_rect[i].Height = reader.ReadSingle();

            frame_offset[i].X = reader.ReadSingle();
            frame_offset[i].Y = reader.ReadSingle();
        }

        int comp_tex_size = reader.ReadInt32();
        byte[] comp_tex_data = reader.ReadBytes(comp_tex_size);
        byte[] decomp_tex_data = new byte[width * height * 4];
        MemoryStream mem_stream = new MemoryStream(comp_tex_data);
        DeflateStream deflate = new DeflateStream(mem_stream, CompressionMode.Decompress, false);
        int num_read = deflate.Read(decomp_tex_data, 0, decomp_tex_data.Length);
        if (num_read != decomp_tex_data.Length)
            throw new Exception("Failed to decompress texture data, [num_read != decomp_tex_data.Length]");

        var pin = GCHandle.Alloc(decomp_tex_data, GCHandleType.Pinned);
        Image img = new Image(width, height, pin.AddrOfPinnedObject());
        pin.Free();

        deflate.Dispose();

        ChangeImage(img);
        this.ppu = ppu;
        this.filter = filter;
        this.wrap = wrap;
        frames.Clear();
        for (int i = 0; i < frame_count; i++)
            frames.Push(Frame.Create(frame_name[i], frame_rect[i], frame_offset[i]));

    }

    protected override void Serialize(Stream stream)
    {
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(0x00737072); // sign
        writer.Write(1); // for now version should always be 1
        writer.Write(img?.Width ?? 0);
        writer.Write(img?.Height ?? 0);
        writer.Write(ppu);
        writer.Write((int)filter);
        writer.Write((int)wrap);

        for (int i = 0; i < 13; i++)
            writer.Write(0); // not used

        writer.Write(frames.Count);
        for (int i = 0; i < frames.Count; i++)
        {
            var f = frames[i];
            writer.Write(f.Name.String);

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
    }

    void add_frame()
    {
        var frect = new Rect(0, 0, 100, 50);
        frect.Center = Canvas.Instance.ScreenToWorld(Input.MousePosition);
        frames.Push(Frame.Create("new frame", frect, Vector2.One * .5f));
    }

    void remove_frame()
    {
        if (act_frm >= 0)
        {
            frames.RemoveAt(act_frm);
            act_frm = -1;
        }
    }

    void on_file_drop(string[] file)
    {
        if (Directory.Exists(file[0]))
        {
            packer_src.String = file[0];
            apply_pack();
        }
    }

    int next_pot(int value)
    {
        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value++;
        return value;
    }

    void apply_pack()
    {
        var path = packer_src.String;
        Array<Image> images = new Array<Image>(10);
        string[] files;
        if (!Directory.Exists(path))
            return;

        // add all .png for packing
        files = Directory.GetFiles(path, "*.png");
        for (int i = 0; i < files.Length; i++)
        {
            var img = Image.FromFile(files[i]);
            img.FlipVertical();
            images.Push(img);
        }
        // sort by area
        int comparsion(Image x, Image y) => (y.Width * y.Height) - (x.Width * x.Height);
        images.Sort(comparsion);

        var rp = new RectPack();
        act_frm = -1;

        // last frames used to restore offset
        var last_frames = frames.ToArray();
        frames.Clear();

        // pack rects
        bool failed = false;
        for (int i = 0; i < images.Count; i++)
        {
            float x, y;
            if (!rp.Pack(images[i].Width + padding, images[i].Height + padding, out x, out y))
            {
                Dialog.Info("Failed to pack some images !");
                failed = true;
                break;
            }
            frames.Push(Frame.Create(images[i].Name, new Rect(x, y, images[i].Width, images[i].Height), Vector2.One * .5f));
        }

        if (!failed)
        {
            var w = (int)rp.Width;
            var h = (int)rp.Height;

            // paste each frame image into the pack image
            var pack_img = new Image(w, h, Color.Transparent);
            for (int i = 0; i < images.Count; i++)
            {
                var pos = frames[i].Rect.Position;
                var src = images[i];
                for (int cy = 0; cy < src.Height; cy++)
                    for (int cx = 0; cx < src.Width; cx++)
                        pack_img[cx + (int)pos.X, cy + (int)pos.Y] = src[cx, cy];
            }

            // restore offset
            for (int i = 0; i < frames.Count; i++)
            {
                var name = frames[i].Name.String;
                var target = Array.FindIndex(last_frames, (item) => item.Name.String == name);
                if (target >= 0)
                    frames[i].Offset = last_frames[target].Offset;

            }

            ChangeImage(pack_img);
        }
    }

    struct Frame
    {
        public InputBuffer Name;
        public Rect Rect;
        public Vector2 Offset;

        public static Frame Create()
        {
            return Create(string.Empty, new Rect(0, 0, 100, 100), new Vector2(.5f, .5f));
        }

        public static Frame Create(string name, Rect rect, Vector2 Offset)
        {
            var f = new Frame();
            f.Name = new InputBuffer(128);
            f.Name.String = name;
            f.Rect = rect;
            f.Offset = Offset;
            return f;
        }
    }

    class RectPack
    {
        Array<Rect> nodes = new Array<Rect>();
        float width, height;

        public RectPack()
        {
            nodes.Push(new Rect(0, 0, float.MaxValue, float.MaxValue));
        }

        public float Width => width;

        public float Height => height;

        public bool Pack(float w, float h, out float x, out float y)
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                if (w <= nodes[i].Width && h <= nodes[i].Height)
                {
                    var node = nodes[i];
                    nodes.RemoveAt(i);
                    x = node.X;
                    y = node.Y;
                    float r = x + w;
                    float b = y + h;
                    nodes.Push(new Rect(r, y, node.XMax - r, h));
                    nodes.Push(new Rect(x, b, w, node.YMax - b));
                    nodes.Push(new Rect(r, b, node.XMax - r, node.YMax - b));
                    width = Math.Max(width, r);
                    height = Math.Max(height, b);
                    return true;
                }
            }
            x = y = 0;
            return false;
        }
    }
}