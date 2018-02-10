using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Engine
{
    public class SpriteSheet : Data
    {
        SpriteSheetFrame[] frames;
        Texture tex;
        int ppu;

        SpriteSheet() { }

        public SpriteSheetFrame this[string name]
        {
            get
            {
                bool match(SpriteSheetFrame f) => f.Name == name;
                var idx = Array.FindIndex(frames, match);
                if (idx < 0)
                    return null;
                else
                    return frames[idx];
            }
        }

        public int FrameCount => frames.Length;

        public Texture Texture => tex;

        public int PixelPerUnit => ppu;

        /// <summary>
        /// Load and cache sprite sheet and related texture from file
        /// </summary>
        public static SpriteSheet Load(string file)
        {
            return Data.FromCacheOrFile(file, deserialize) as SpriteSheet;
        }

        static SpriteSheet deserialize(Stream stream)
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
            var img = new Image(width, height, pin.AddrOfPinnedObject());
            Texture texture = Texture.Create(img, filter, wrap);
            img.Dispose();
            pin.Free();

            deflate.Dispose();

            var spr = new SpriteSheet();
            spr.tex = texture;
            spr.ppu = ppu;
            spr.frames = new SpriteSheetFrame[frame_count];
            for (int i = 0; i < frame_count; i++)
                spr.frames[i] = new SpriteSheetFrame(spr, frame_name[i], frame_rect[i], frame_offset[i], texture.Size, ppu);

            return spr;
        }

        protected override void OnDisposeManaged()
        {
            tex.Dispose();
            frames = null;
        }


    }

    public class SpriteSheetFrame
    {
        SpriteSheet owner;
        Rect rect;
        Vector2 offset;
        string name;
        Vertex[] verts;

        internal SpriteSheetFrame(SpriteSheet owner, string name, Rect rect, Vector2 offset, Vector2 tex_size, float ppu)
        {
            this.name = name;
            this.owner = owner;
            this.rect = rect;
            this.offset = offset;

            var texcoord = rect / tex_size;
            rect /= ppu;
            offset /= ppu;
            rect.Position = -offset * rect.Size;
            verts = new Vertex[4];
            verts[0] = new Vertex(rect.XMinYMin, texcoord.XMinYMin, Color.White);
            verts[1] = new Vertex(rect.XMaxYMax, texcoord.XMaxYMax, Color.White);
            verts[2] = new Vertex(rect.XMinYMax, texcoord.XMinYMax, Color.White);
            verts[3] = new Vertex(rect.XMaxYMin, texcoord.XMaxYMin, Color.White);
        }

        /// <summary>
        /// Name of the frame
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Local space vertex data
        /// </summary>
        public Vertex[] Vertices => verts;

        /// <summary>
        /// The owner sprite sheet
        /// </summary>
        public SpriteSheet SpriteSheet => owner;

        public Rect Rect => rect;

        public Vector2 Offset => offset;
    }
}